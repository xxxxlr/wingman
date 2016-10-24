using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour {

    //public RectTransform test;
    string text = "";
    Text child;
    float floatGap = 0.1f, floatRate = 0.002f;

    Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //test.position = transform.position;
        if (text.Length > 0)
        {
            print(child.text.Length);
            child.text = text.Substring(0, (child.text.Length + 1));
            if (child.text == text) text = "";
        }
        Vector3 temp = GetComponent<RectTransform>().position;
        temp.y += floatRate;
        floatGap -= 0.002f;
        if(floatGap <= 0)
        {
            floatGap = 0.1f;
            floatRate *= -1;
        }
        GetComponent<RectTransform>().position = temp;
             
	}

    public void Init(string name, string detail)
    {
        child = transform.GetChild(0).FindChild("Info").GetComponent<Text>();
        transform.GetChild(0).FindChild("Name").GetComponent<Text>().text = name;
        text = detail;
    }
}
