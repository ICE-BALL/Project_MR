using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX
{
    public string SelectServerScene = "SelectServer";
    public string GameScene = "Game";

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        NetworkManager.Loading(GetLocalIPAddress());
    }

    public string GetScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    static string GetLocalIPAddress()
    {
        try
        {
            // 로컬 컴퓨터의 호스트 이름을 가져옵니다.
            string hostName = Dns.GetHostName();

            // 호스트 이름을 IP 주소로 해석합니다.
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            // 모든 IP 주소 중에서 IPv4 주소를 찾습니다.
            IPAddress ipAddress = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            return ipAddress?.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine("오류 발생: " + ex.Message);
            return null;
        }
    }
}
