using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogController : MonoBehaviour
{
    public static LogController instance;
    public TextMeshProUGUI logTxt;
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
    public void Log(string msg)
    {
        Debug.Log(msg);
        if (logTxt != null)
        {
            logTxt.text = msg;
        }
    }
    public void Log(string msg, GameObject target)
    {
        Debug.Log(msg, target);
    }
}
