using UnityEngine;

[System.Serializable]
public class DayData
{
    public Species[] Species;
    [TextArea(3, 4)] public string News;
    public GameObject NewsFacePrefab;

    public int VisitorCount;
    public DialogueList InitialDialogue;
}
