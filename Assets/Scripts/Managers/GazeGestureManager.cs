using UnityEngine;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;
using System.Collections;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }
    public GameObject CameraObject { get; private set; }

    public GameObject textIndicator;
    private Vector3 focusedGazePoint;

    //TODO:
    public bool isVisualAnalysisOn = false;


    public int gazePointsInsertIndex;
    public Vector3[] gazePoints;
    public int gazeRate = 5;
    public int gazePointsTimePeriod = 5;
    public int focusQualiferTime = 4;
    public float focusGazeDensityQualifier;
    public float focusQualiferRadus = 0.10f;
    public int checkMostRecentGazeTime = 5;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Start Gaze-focus Ray Manager");
        Instance = this;
        FocusedObject = GameObject.FindGameObjectWithTag("MainCamera");
        CameraObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        gazeRate = 5;
        gazePointsTimePeriod = 3;
        focusQualiferTime = 2;
        focusQualiferRadus = 0.10f;
        checkMostRecentGazeTime = 2;
        focusGazeDensityQualifier = focusQualiferTime / (float)gazePointsTimePeriod;

        gazePoints = new Vector3[gazePointsTimePeriod * gazeRate]; 
        for (int i = 0; i < gazePoints.Length; i++) { gazePoints[i] = Vector3.zero; }
        gazePointsInsertIndex = 0;
        TurnVisualAnalysis(isVisualAnalysisOn ? "ON" : "OFF");
    }

    void BCChangeVisualAnalysisStatus(string newStatus)
    {
        TurnVisualAnalysis(newStatus);
    }

    void TurnVisualAnalysis(string signal)
    {
        if(signal == "ON")
        {
            isVisualAnalysisOn = true;
            InvokeRepeating("TakeRayCast", 0.001f, 1 / (float)gazeRate);
        }
        if(signal == "OFF")
        {
            isVisualAnalysisOn = false;
            CancelInvoke("TakeRayCast");
            resetGazePoints();
        }
    }


    Vector3 AnalyzeFocusPoint(Vector3[] gazePoints)
    {
        //assuming 0,0,0 is a impossbile focus point
        Vector3 focusCenter = Vector3.zero;
        float currentFocusGazeDensity = 0;
        float maxFocusGazeDensity = 0;
        int checkMostRecentGazePointsLength = gazeRate * checkMostRecentGazeTime;

        for(int i = gazePointsInsertIndex, counter = 0; counter < checkMostRecentGazePointsLength; counter++, i --)
        {
            int pointsInRadius = 0;
            if(i < 0)
            {
                i = gazePoints.Length - 1;
            }
            for (int j = 0; j < gazePoints.Length; j++)
            {
                //Debug.Log(Vector3.Distance(gazePoints[i], gazePoints[j]));
                //Debug.Log(focusQualiferRadus);

                //Debug.Log("==" + (Vector3.Distance(gazePoints[i], gazePoints[j]) - focusQualiferRadus));
                if (Vector3.Distance(gazePoints[i], gazePoints[j]) < focusQualiferRadus)
                {
                    pointsInRadius++;
                }
            }
            currentFocusGazeDensity = (pointsInRadius + 1) / (float)gazePoints.Length;
            if (currentFocusGazeDensity > focusGazeDensityQualifier && currentFocusGazeDensity > maxFocusGazeDensity && gazePoints[i] != Vector3.zero)
            {
                maxFocusGazeDensity = currentFocusGazeDensity;
                focusCenter = gazePoints[i];
            }
        }

        return focusCenter;
    }

    void TakeRayCast()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
            //Debug.Log("FocusedObject.transform.position:" + FocusedObject.transform.position);
            //Debug.Log(string.Format("hitInfo.point:{0},{1},{2}", hitInfo.point.x, hitInfo.point.y, hitInfo.point.z));
            pushGazePoint(hitInfo.point);
            Vector3 focusPoint = AnalyzeFocusPoint(gazePoints);
            if ( focusPoint != Vector3.zero)
            {
                focusedGazePoint = focusPoint;
                //TODO:Three states, should have analyzing
                TurnVisualAnalysis("OFF");
                CameraObject.SendMessage("OnImageUploadRequest");
                CameraObject.SendMessage("OnShowProgressCircle", focusPoint);
            }
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
            //push an empty point so there's the time elipse effect to push out the old points in gaze buffer
            pushGazePoint(Vector3.zero);
        }

       
    }

    void OnImageAnalysisResult(string result)
    {
        TurnVisualAnalysis("ON");

        CameraObject.SendMessage("OnHideProgressCircle");
        CameraObject.SendMessage("OnTextIndicatorRequest", new TextIndicator(result, focusedGazePoint));
        //TODO: result analysis manager
        if (result.Contains("coca"))
        {
            CameraObject.SendMessage("OnDisplayAttitudeVisualFeedback", new AttitudeVisualFeedback(true, "NegtiveVisualFeedback"));
            CameraObject.SendMessage("OnDisplayAttitudeVisualFeedback", new AttitudeVisualFeedback(false, "PositiveVisualFeedback"));
            string negtiveMessage = "You know not buying the soda is your best chance to avoid drinking it at home!";
            CameraObject.SendMessage("OnSpeakText", negtiveMessage);
        }

        if (result.Contains("water"))
        {
            CameraObject.SendMessage("OnDisplayAttitudeVisualFeedback", new AttitudeVisualFeedback(true, "PositiveVisualFeedback"));
            CameraObject.SendMessage("OnDisplayAttitudeVisualFeedback", new AttitudeVisualFeedback(false, "NegtiveVisualFeedback"));
            string positiveMessage = "Yep, good choice!";
            CameraObject.SendMessage("OnSpeakText", positiveMessage);
        }
        StartCoroutine(HideAttitudeVisualFeedbackAfter(3));
    }
    IEnumerator HideAttitudeVisualFeedbackAfter(int sec)
    {
        yield return new WaitForSeconds(sec);
        CameraObject.SendMessage("OnDisplayAttitudeVisualFeedback", new AttitudeVisualFeedback(false, "PositiveVisualFeedback"));
        CameraObject.SendMessage("OnDisplayAttitudeVisualFeedback", new AttitudeVisualFeedback(false, "NegtiveVisualFeedback"));
    }
    //void OnTextIndicatorRequest(string content)
    //{
    //    GameObject textMessage = Instantiate(textIndicator, focusedGazePoint, Quaternion.LookRotation(Camera.main.transform.forward)) as GameObject;
    //    textMessage.GetComponent<TextMesh>().text = content;
    //}

    void resetGazePoints()
    {
        for(int i = 0; i < gazePoints.Length; i++) { gazePoints[i] = Vector3.zero; }
    }
    void pushGazePoint(Vector3 newGazePoint)
    {
        //NOTE: can't use temp int len, seems like it will be cached to 0...
        gazePoints[gazePointsInsertIndex] = newGazePoint;
        if (gazePointsInsertIndex < gazePoints.Length - 1)
        {
            gazePointsInsertIndex++;
        }
        else
        {
            gazePointsInsertIndex = 0;
        }
    }
}