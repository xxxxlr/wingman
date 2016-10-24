using UnityEngine;
using UnityEngine.VR.WSA.Input;

using System.Collections;

public class GestureManager : MonoBehaviour {
    private ControlPanelManager controlPanelManager;
    public static GestureManager Instance { get; private set; }
    GestureRecognizer recognizer;
    private bool isVisualAnalysisOn;

    // Use this for initialization
    void Start () {
        Instance = this;
        controlPanelManager = ControlPanelManager.Instance();
        recognizer = new GestureRecognizer();
        isVisualAnalysisOn = false;

        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            toggleVisualAnalysisStatus();
        };
        recognizer.StartCapturingGestures();
    }

    void toggleVisualAnalysisStatus()
    {
        TurnVisualAnalysis(isVisualAnalysisOn ? "OFF" : "ON");
    }

    void TurnVisualAnalysis(string signal)
    {
        isVisualAnalysisOn = !isVisualAnalysisOn;
        controlPanelManager.ChangeVisualAnalysisStatus(signal);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
