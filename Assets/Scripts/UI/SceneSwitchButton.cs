using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchButton : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private CanvasGroup _fade;

    public async void SwitchScene()
    {
        _fade.DOFade(1f, 2f).SetEase(Ease.InOutSine).OnComplete(async () => await SceneManager.LoadSceneAsync(_sceneName));
    }
}
