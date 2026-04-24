using UnityEngine;

[System.Serializable]
public class DialogueData
{
    [Header("Dialogue Settings")]
    [TextArea(0, 25)]
    public string DialogueText;

    public float TypingSpeed = 0.05f;

    [Header("Visual Effects")]
    public bool Colored = false;
    public Color StartColor = Color.white;
    public Color EndColor = Color.white;
    public float StartOffset = 0f;
    public float EndOffset = 0f;

    [Header("Animation Curves")]
    public AnimationCurve ScaleCurve = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve RotationCurve = AnimationCurve.Constant(0, 1, 0);
    public AnimationCurve ColorCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve PositionCurve = AnimationCurve.Linear(0, 0, 1, 1);
}
