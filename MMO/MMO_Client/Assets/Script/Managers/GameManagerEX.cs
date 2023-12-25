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
    public int Map_Zone;

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
        Map_Zone = (int)define.Map_Zone.Map_001;

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

    public void SendMovePacket()
    {
        if (NetworkManager._session.SessionId == 1)
        {
            MonsterList mL = new MonsterList();
            for (int i = 1; i <= NetworkManager._monsters.Count; i++)
            {
                Creature creature;
                if (NetworkManager._monsters.TryGetValue(i, out creature))
                {
                    Slime s = creature as Slime;

                    MonsterList.Monsters m = new MonsterList.Monsters();
                    m.Map_Zone = Managers.Game.Map_Zone;
                    m.MonsterId = s._stat.MonsterId;
                    m.MonsterType = (int)define.MonsterTypes.Slime;
                    m.PosX = s.gameObject.transform.position.x;
                    m.PosY = s.gameObject.transform.position.y;
                    m.PosZ = s.gameObject.transform.position.z;

                    m.RotX = s.gameObject.transform.eulerAngles.x;
                    m.RotX = s.gameObject.transform.eulerAngles.y;
                    m.RotX = s.gameObject.transform.eulerAngles.z;

                    mL.monsterss.Add(m);
                }
            }
            NetworkManager._session.Send(mL.Write());

        }
    }

}
