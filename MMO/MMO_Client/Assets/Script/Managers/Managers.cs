using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instace;
    static Managers Instance { get { Init(); return s_instace; } }

    ResourceManager _resource = new ResourceManager();
    SceneManagerEX _sceneManager = new SceneManagerEX();
    GameManagerEX _gameManager = new GameManagerEX();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEX Scene { get { return Instance._sceneManager; } }
    public static GameManagerEX Game { get {  return Instance._gameManager; } }

    static GameObject _uiRoot;
    public static GameObject UI_Root { get { Init(); return _uiRoot; } }


    void Awake()
    {
        Init();
    }

    void Update()
    {

    }

    static void Init()
    {
        if (s_instace == null)
        {
            GameObject obj = GameObject.Find("@Managers");
            if (obj == null)
            {
                obj = new GameObject { name = "@Managers" };
                obj.AddComponent<Managers>();
            }

            DontDestroyOnLoad(obj);
            s_instace = obj.GetComponent<Managers>();
        }

        if (_uiRoot == null)
        {
            _uiRoot = GameObject.Find("@UI_Root");
            if (_uiRoot == null)
            {
                _uiRoot = new GameObject { name = "@UI_Root" };
            }
            DontDestroyOnLoad(_uiRoot);
        }
    }
}
