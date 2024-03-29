using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public event EventHandler ChangeSceneEvent;

    string currentScene = "";
    public enum SceneName
    {
        Lobby,
        SelectScene,
        Map_1,
        Map_2,
        Map_3,
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != currentScene)
        {
            currentScene = sceneName;
            if (currentScene != "Lobby")
            {
                ChangeSceneEvent?.Invoke(this, null);
            }
        }
    }
    public void ChangeScene(SceneName name, bool isSingle)
    {
        if (isSingle)
        {
            SceneManager.LoadScene(name.ToString(), LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(name.ToString(), LoadSceneMode.Additive);
        }
    }
    public void ChangeSceneSync(SceneName name, bool isSingle)
    {

        if (isSingle)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Single);
        }
        else
        {
            NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Additive);
        }
    }
    public void ChangeSceneSync(int sceneV, bool isSingle)
    {
        SceneName sceneName = (SceneName)sceneV;
        ChangeSceneSync(sceneName, isSingle);
    }
}
