using UnityEngine;
using System.Collections;

public class CoroutineTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Cotine(3f));
        Debug.Log("End start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Cotine(float time)
    {
        Debug.Log(Time.time);
        yield return 5;
        Debug.Log(Time.time);
    }
}
