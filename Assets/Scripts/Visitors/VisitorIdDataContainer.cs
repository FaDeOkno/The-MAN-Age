using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VisitorIdDataContainer : MonoBehaviour
{
    [SerializeField] private VisitorIdData _type;
    private TextMeshProUGUI _tmp;

    void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    public void OnEvent(Component sender, object data)
    {
        if (sender is not Visitor visitor)
            return;

        switch (_type)
        {
            case VisitorIdData.Name:
                _tmp.SetText(visitor.Name);
                break;
            case VisitorIdData.DoB:
                _tmp.SetText(visitor.BirthDate.ToString("dd.MM.yyyy"));
                break;
        }
    }
}

public enum VisitorIdData
{
    Name,
    DoB
}
