using System.Collections;
using UnityEngine;

public class PanelTween : MonoBehaviour
{
    public enum TweenType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut
    }

    public TweenType tweenType = TweenType.Linear;
    public float tweenDuration = 1f;

    public RectTransform panel;

    Vector3 offScreenPosition;
    Vector3 onScreenPosition;

    void Start()
    {
        // The onscreen position is set to the center of the game window.
        // Try to change that to something better, like just at the edge 
        // of the window.
        onScreenPosition = new Vector3(0f, 0f, panel.localPosition.z);
        offScreenPosition = new Vector3(panel.localPosition.x, Screen.height, panel.localPosition.z);

        panel.localPosition = offScreenPosition; // Start off-screen
        ShowPanel();//tween in panel onstart
    }

    // Create a button, with the label Toggle Panel. You need to link the button's
    // OnClick() event to this function. 
    // Look here for how to do this: https://www.youtube.com/watch?v=Tcrg2KZck2Y
    //
    public void TogglePanel()
    {
        if (panel.localPosition == onScreenPosition)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        StartCoroutine(TweenPanel(panel.localPosition, onScreenPosition, tweenDuration, tweenType));
    }

    public void HidePanel()
    {
        StartCoroutine(TweenPanel(panel.localPosition, offScreenPosition, tweenDuration, tweenType));
    }

    IEnumerator TweenPanel(Vector3 start, Vector3 end, float duration, TweenType tweenType)
    {
        float elapsedTime = 0f;

        Debug.Log("Tweening panel from " + start + " to " + end + " over " + duration + " seconds with " + tweenType + " tween type.");

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Apply the selected tween type
            switch (tweenType)
            {
                case TweenType.Linear:
                    // No modification, t remains linear
                    break;
                case TweenType.EaseIn:
                    t = t * t; // Quadratic ease-in
                    break;
                case TweenType.EaseOut:
                    t = 1 - (1 - t) * (1 - t);// Quadratic ease-out - implement this code
                    break;
                case TweenType.EaseInOut:
                    if (t < 0.5f)
                    {
                        t = 2 * t * t;// Ease-in for the first half - implement this code
                    }
                    else
                    {
                        t = 1 - Mathf.Pow(-2 * t + 2, 2) / 2;// Ease-out for the second half - implement this code
                    }
                    break;
            }

            // Custom linear interpolation with modified t based on the tween type
            float x = start.x + (end.x - start.x) * t; ; 
            float y = start.y + (end.y - start.y) * t; ; 

            // Set the panel position
            panel.localPosition = new Vector3(x, y, start.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the panel reaches the final position
        panel.localPosition = end;
    }
}