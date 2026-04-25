using UnityEngine;

public class DialogueCaller : MonoBehaviour
{
    public DialogueList Dialogue;
    [SerializeField] private GameEvent _startDialogueEvent;
    [SerializeField] private GameEvent _endDialogueEvent;
    [SerializeField] private DialogueList[] _dialogues;
    [SerializeField] private bool _randomDialogue = true;

    void Start()
    {
        if (!_randomDialogue)
            return;

        var random = new System.Random();
        Dialogue = random.Pick(_dialogues);
    }

    public void OnStartEvent()
    {
        _startDialogueEvent.Raise(this, Dialogue);
    }

    public void OnEndEvent()
    {
        _endDialogueEvent.Raise(this);
    }
}
