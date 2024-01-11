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
            ChangeSceneEvent?.Invoke(this, null);
        }
    }
    public void ChangeScene(SceneName name, bool isSingle)
    {
        switch (name)
        {
            case SceneName.Lobby:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Lobby);
                break;
            case SceneName.SelectScene:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.SelectWeapon);
                break;
            case SceneName.Map_1:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Play);
                break;
        }

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
        switch (name)
        {
            case SceneName.Lobby:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Lobby);
                break;
            case SceneName.SelectScene:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.SelectWeapon);
                break;
            default:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Play);
                break;
        }

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
        switch (sceneV)
        {
            case 0:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Lobby);
                break;
            case 1:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.SelectWeapon);
                break;
            default:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Play);
                break;
        }

        ChangeSceneSync(sceneName, isSingle);
    }
}
