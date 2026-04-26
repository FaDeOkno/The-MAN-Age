using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurDateText : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    private float _nextUpdate = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextUpdate > Time.time)
            return;

        _nextUpdate = Time.time + 1f;
        var date = new DateTime(GameManager.Year, DateTime.Now.Month, DateTime.Now.Day);
        date.AddDays(GameManager.Instance.CurDayIndex);

        _tmp.SetText("Current date: " + date.ToString("dd.MM.yyyy"));
    }
}
