using UnityEngine;

[System.Serializable]
public class DialogueData
{
    [Header("Dialogue Settings")]
    [TextArea(0, 25)]
    public string DialogueText;

    public float TypingSpeed = 0.05f;

    [Header("Visual Effects")]
    public Color StartColor = Color.white;
    public Color EndColor = Color.white;

    [Header("Animation Curves")]
    public AnimationCurve ColorCurve = AnimationCurve.Linear(0, 0, 1, 1);
}
