using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

    [SerializeField]
    GameObject popup;
    public static UIManager instance;
    private Text visualAnalysisStatus;
    List<GameObject> UIs = new List<GameObject>();

    public static UIManager Instance{
        get{ return instance; }
    }

	// Use this for initialization
	void Start () {
        instance = this;
        GameObject canvasObject = GameObject.FindGameObjectWithTag("MainCanvas");
        Transform textTr = canvasObject.transform.Find("VisualAnalysisStatusText");
        visualAnalysisStatus = textTr.GetComponent<Text>();
        //initiateUI("good name", "asdfasfewfs awefsdfsda aefasdfs adafds dasfasdfa dasfsadfas afeafeaf", new Vector3(0, 1, 10));
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.D))
          //  removeUI();
	}

    void BCChangeVisualAnalysisStatus(string newStatus)
    {
        visualAnalysisStatus.text = "Avoiding soda:" + newStatus;
    }

    public void initiateUI(string name, string detail, Vector3 position)
    {
        GameObject temp = (GameObject)GameObject.Instantiate(popup, position, Quaternion.identity);
        UIs.Add(temp);
        temp.GetComponent<PopUpUI>().Init(name,detail);
    }

    public void removeUI()
    {
        for (int i = UIs.Count - 1; i >= 0; --i)
        {
            Destroy(UIs[i]);
        }
        UIs.Clear();
    }
}
