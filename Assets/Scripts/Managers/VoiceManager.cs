using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceManager : MonoBehaviour
{
    private ControlPanelManager controlPanelManager;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywordToActionDict = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        Debug.Log("Register Voice Manager");
        controlPanelManager = ControlPanelManager.Instance();

        keywordToActionDict.Add("Reset world", () =>
        {
            // Call the OnReset method on every descendant object.
            this.BroadcastMessage("OnClearAllAnalysisTextMessages");
        });

        keywordToActionDict.Add("Turn On Visual analysis", () =>
        {
            Debug.Log("turn on visual analysis");
            controlPanelManager.ChangeVisualAnalysisStatus("ON");
        });

        keywordToActionDict.Add("Help me avoid soda", () =>
        {
            Debug.Log("Help me avoid soda");
            controlPanelManager.ChangeVisualAnalysisStatus("ON");
        });

        keywordToActionDict.Add("Turn Off Visual analysis", () =>
        {
            Debug.Log("turn off visual analysis");
            controlPanelManager.ChangeVisualAnalysisStatus("OFF");
        });


        // Tell the KeywordRecognizer about our keywordToActionDict.
        keywordRecognizer = new KeywordRecognizer(keywordToActionDict.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    void Update()
    {
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywordToActionDict.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}