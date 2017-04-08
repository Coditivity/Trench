using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogger : MonoBehaviour {

    private static GameLogger _instance = null;
    public static GameLogger Instance
    {
        get
        {
            return _instance = null;
        }
        private set
        {
            _instance = value;
        }
    }

    [SerializeField]
    private GameObject _gameLogPanel = null;
    [SerializeField]
    private Text _text = null;
    [SerializeField]
    private Text _permaText = null;


    public static void Log(string s)
    {
        _instance._text.text = s;
    }
    public static void AppendLog(string s)
    {
        _instance._text.text += s;
    }
    public static void ClearLog()
    {
        _instance._text.text = "";
    }
    public static void AppendPermaLog(string s)
    {
        _instance._permaText.text += s;
    }

    public static void ShowOrHideLogPanel(bool bShow)
    {
        _instance._gameLogPanel.SetActive(bShow);
    }
    void Awake()
    {
        _instance = this;
            
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
