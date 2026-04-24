using UnityEngine;

public class DialogueCaller : MonoBehaviour
{
    [SerializeField] private DialogueList[] _dialogue;
    [SerializeField] private GameEvent _startDialogueEvent;
    [SerializeField] private GameEvent _endDialogueEvent;

    public void OnStartEvent()
    {
        if (_dialogue.Length == 0)
        {
            Debug.LogWarning("No dialogue data assigned to DialogueCaller.");
            return;
        }

        var random = new System.Random();
        var dialogue = random.Pick(_dialogue);

        _startDialogueEvent.Raise(this, dialogue.Dialogue);
    }

    public void OnEndEvent()
    {
        _endDialogueEvent.Raise(this);
    }
}
