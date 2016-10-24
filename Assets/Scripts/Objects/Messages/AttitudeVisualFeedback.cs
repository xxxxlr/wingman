using UnityEngine;

public class AttitudeVisualFeedback {
    //TODO: setter and getter with private???
    public bool isShow;
    public string attitudeVisualFeedbackName;

    public AttitudeVisualFeedback(bool isShow, string attitudeVisualFeedback)
    {
        this.isShow = isShow;
        this.attitudeVisualFeedbackName = attitudeVisualFeedback;
    }
}
