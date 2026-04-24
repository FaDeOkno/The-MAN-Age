using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DialogueBox : MonoBehaviour
{
    private TextMeshProUGUI _dialogueTextUI;
    private CanvasGroup _canvasGroup;

    private bool _isTyping;
    private int _currentDialogueIndex;
    private DialogueData[] _currentDialogueData;
    private float _startTime;

    void Start()
    {
        _dialogueTextUI = GetComponentInChildren<TextMeshProUGUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_dialogueTextUI == null)
        {
            Debug.LogError("No TextMeshPro component found in children of DialogueBox.");
        }
    }

    void Update()
    {
        if (!_isTyping || _currentDialogueData == null)
            return;

        bool isAnotherTag = false;
        var current = _currentDialogueData[_currentDialogueIndex];

        StringBuilder sb = new StringBuilder();
        for (var i = 0; i < current.DialogueText.Length; i++)
        {
            if (current.DialogueText[i] == '<')
                isAnotherTag = true;

            if (isAnotherTag)
            {
                if (current.DialogueText[i] == '>')
                    isAnotherTag = false;

                sb.Append(current.DialogueText[i]);
                continue;
            }

            var charTime = current.PositionCurve.Evaluate(Mathf.Clamp01(Time.time - _startTime - (i * current.TypingSpeed)));

            var colorValue = Color.Lerp(current.StartColor, current.EndColor, charTime);
            var offsetValue = Mathf.Lerp(current.StartOffset, current.EndOffset, charTime);
            var scaleValue = charTime;

            sb.Append($"<color=#{ColorUtility.ToHtmlStringRGBA(colorValue)}><voffset={offsetValue}em><size={scaleValue * 100}%>{current.DialogueText[i]}</color></voffset></size>");
        }

        _dialogueTextUI.text = sb.ToString();

        if (Time.time - _startTime > current.DialogueText.Length * current.TypingSpeed + 1f)
        {
            _isTyping = false;
        }
    }

    public void OnDialogueEvent(Component sender, object data)
    {
        if (data == null)
        {
            EndDialogue();
            return;
        }

        if (data is not DialogueData[] dialogueData)
        {
            Debug.LogWarning("Received data is not of type DialogueData.");
            return;
        }

        if (_canvasGroup.alpha >= 0)
        {
            _canvasGroup.alpha = 1f;
            _currentDialogueData = dialogueData;
            _currentDialogueIndex = 0;
            _startTime = Time.time;
            _isTyping = true;
            return;
        }

        _canvasGroup.DOFade(1f, .75f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _canvasGroup.alpha = 1f;
            _currentDialogueData = dialogueData;
            _currentDialogueIndex = 0;
            _startTime = Time.time;
            _isTyping = true;
        });

    }

    public void OnNextDialogueEvent()
    {
        if (_currentDialogueData == null)
        {
            EndDialogue();
            return;
        }
        if (_currentDialogueIndex < _currentDialogueData.Length - 1)
        {
            Debug.Log("Next dialogue");
            _currentDialogueIndex++;
            _startTime = Time.time;
            _isTyping = true;
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        _canvasGroup.DOFade(0f, .75f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _isTyping = false;
            _currentDialogueData = null;
            _dialogueTextUI.text = string.Empty;
            _canvasGroup.alpha = 0f;
        });
    }
}
