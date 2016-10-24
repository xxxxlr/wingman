using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TextIndicatorManager : MonoBehaviour {

    private static TextIndicatorManager textIndicatorManager;
    public GameObject textIndicatorPrefab;
    public GameObject progressCircleIndicatorPrefab;
    public GameObject CameraObject { get; private set; }
    private List<GameObject> recoredTextIndicatorObjects = new List<GameObject>();
    private Dictionary<string, GameObject> recoredProgressCircleDict = new Dictionary<string, GameObject>();
    public GameObject progressCircle;

    // Use this for initialization
    public static TextIndicatorManager Instance()
    {
        if (!textIndicatorManager)
        {
            textIndicatorManager = FindObjectOfType(typeof(TextIndicatorManager)) as TextIndicatorManager;
            if (!textIndicatorManager)
            {
                Debug.Log("I can't continute because I need at least one text indicator manager instance!");
            }
        }

        return textIndicatorManager;
    }
    void Start () {
        CameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 initial_point = Vector3.zero;
        progressCircle = Instantiate(progressCircleIndicatorPrefab, initial_point, Quaternion.LookRotation(Camera.main.transform.forward)) as GameObject;
        SetSprintRendersAbility(progressCircle, false);

        string NEG_VI_FB = "NegtiveVisualFeedback";
        string POS_VI_FB = "PositiveVisualFeedback";
        GameObject NegtiveVisualFeedback = GameObject.Find(NEG_VI_FB);
        GameObject PositiveVisualFeedback = GameObject.Find(POS_VI_FB);
        RawImage NegtiveVisualFeedbackRawImage = NegtiveVisualFeedback.GetComponent<RawImage>();
        RawImage PositiveVisualFeedbackRawImage = PositiveVisualFeedback.GetComponent<RawImage>();
        NegtiveVisualFeedbackRawImage.enabled = false;
        PositiveVisualFeedbackRawImage.enabled = false;
    }

    void SetSprintRendersAbility(GameObject progressCircle, bool ability)
    {
        SpriteRenderer[] sprites = progressCircle.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = ability;
        }
    }

    void OnDisplayAttitudeVisualFeedback(AttitudeVisualFeedback attitudeVisualFeedback)
    {
        string NEG_VI_FB = "NegtiveVisualFeedback";
        string POS_VI_FB = "PositiveVisualFeedback";
        //TODO:prefab them and use them not as a gloable feedback but like an indicator for each analysis.
        GameObject NegtiveVisualFeedback = GameObject.Find(NEG_VI_FB);
        GameObject PositiveVisualFeedback = GameObject.Find(POS_VI_FB); ;
        RawImage NegtiveVisualFeedbackRawImage = NegtiveVisualFeedback.GetComponent<RawImage>();
        RawImage PositiveVisualFeedbackRawImage = PositiveVisualFeedback.GetComponent<RawImage>();

        if (attitudeVisualFeedback.attitudeVisualFeedbackName == NEG_VI_FB  
            || attitudeVisualFeedback.attitudeVisualFeedbackName == POS_VI_FB)
        {
            if (attitudeVisualFeedback.attitudeVisualFeedbackName == NEG_VI_FB)
            {
                NegtiveVisualFeedbackRawImage.enabled = attitudeVisualFeedback.isShow;
            } 
            if (attitudeVisualFeedback.attitudeVisualFeedbackName == POS_VI_FB){
                PositiveVisualFeedbackRawImage.enabled = attitudeVisualFeedback.isShow; ;
            }
        }
        else
        {
            Debug.LogError("OnDisplayAttitudeVisualFeedback get a unknow name, should be the name of the UI raw image, but got:" + attitudeVisualFeedback.attitudeVisualFeedbackName);
        }

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnShowProgressCircle(Vector3 point)
    {
        try
        {
            progressCircle.transform.position = point;
            progressCircle.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            SetSprintRendersAbility(progressCircle, true);
            //GameObject progressCircle
            //    = Instantiate(progressCircleIndicatorPrefab, point, Quaternion.LookRotation(Camera.main.transform.forward)) as GameObject;
            //progressCircle.GetComponent<Transform>().localScale 
            //    = new Vector3(transform.localScale.x /2, transform.localScale.y, transform.localScale.z);
            //recoredProgressCircleDict.Add(hashVector3ToString(point), progressCircle);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnHideProgressCircle()
    {
        SetSprintRendersAbility(progressCircle, false);
    }

    public void OnTextIndicatorRequest(TextIndicator textIndicator)
    {
        try
        {
            OnClearAllAnalysisTextMessages();
            GameObject textMessage = Instantiate(textIndicatorPrefab, textIndicator.point, Quaternion.LookRotation(Camera.main.transform.forward)) as GameObject;
            textMessage.GetComponent<TextMesh>().text = textIndicator.text;
            recoredTextIndicatorObjects.Add(textMessage);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnClearAllAnalysisTextMessages()
    {
        foreach (GameObject indicator in recoredTextIndicatorObjects)
        {
            Destroy(indicator);
        }
        recoredTextIndicatorObjects.Clear();
    }

    public string hashVector3ToString(Vector3 point)
    {
        return point.x.ToString("0.000") + point.y.ToString("0.000") + point.z.ToString("0.000");
    }

}
