using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerEX
{
    GameObject _myPlayer;
    GameObject _sceneUI;

    public void Start()
    {
        if (Managers.Scene.GetScene() == Managers.Scene.SelectServerScene)
        {
            Managers.Scene.LoadScene(Managers.Scene.GameScene);
            SceneManager.sceneLoaded += Game_001;
        }
    }

    public void Game_001(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.Find("Player") != null)
            Managers.Resource.Destroy(GameObject.Find("Player"));
        if (GameObject.Find("Main_UI") == null)
            _sceneUI = Managers.Resource.Instantiate("UI/UI_Scene/Main_UI", Managers.UI_Root.transform);
        else
            _sceneUI = GameObject.Find("Main_UI");

        // TODO

        // Monster Generate

        //

        //

    }

}
