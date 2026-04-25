using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class NewspaperText : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    public void OnEvent(Component sender, object data)
    {
        if (data is not DayData day)
            return;

        _tmp.SetText(day.News);
    }
}
