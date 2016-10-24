using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using UnityEngine.Windows.Speech;

public class AudioFeedbackManager : MonoBehaviour {
    // Use this for initialization
    // Inspector Variables
    [Tooltip("The audio source where speech will be played.")]
    public AudioSource audioSource;
    public TextToSpeechManager ttsManager;

    void Start ()
    {
        //StartCoroutine(SpeakAfter(1));
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator SpeakAfter(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        ttsManager.SpeakText("Come on man! Drink more water!");
    }

    void OnSpeakText(string content)
    {
        ttsManager.SpeakText(content);
    }

}
