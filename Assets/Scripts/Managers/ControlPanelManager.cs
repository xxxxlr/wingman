using UnityEngine;
using System.Collections;

public class ControlPanelManager : MonoBehaviour {
    public string visualAnalysisStatus = "OFF";

    private static ControlPanelManager myControlPanelManager;

    //Modifyed singleton pattern
    public static ControlPanelManager Instance()
    {
        if(!myControlPanelManager)
        {
            myControlPanelManager = FindObjectOfType(typeof(ControlPanelManager)) as ControlPanelManager;
            if (!myControlPanelManager)
            {
                Debug.Log("I can't continute because I need at least one control panel manager instance!");
            }
        }

        return myControlPanelManager;
    }

	// Use this for initialization
	void Start () {
	
	}

    public void ChangeVisualAnalysisStatus(string newStatus)
    {
        visualAnalysisStatus = newStatus;
        myControlPanelManager.BroadcastMessage("BCChangeVisualAnalysisStatus", newStatus);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
