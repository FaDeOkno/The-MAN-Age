using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchButton : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public async void SwitchScene()
    {
        Debug.Log($"Switching to scene: {_sceneName}");
        await SceneManager.LoadSceneAsync(_sceneName);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
    }
}
