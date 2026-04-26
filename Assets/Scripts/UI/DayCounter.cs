using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DayCounter : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    public void OnEvent(Component component, object data)
    {
        if (data is not int day)
            return;

        _tmp.SetText($"DAY {day + 1}");
    }
}
