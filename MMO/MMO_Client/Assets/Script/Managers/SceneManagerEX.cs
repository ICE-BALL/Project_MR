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
            // ���� ��ǻ���� ȣ��Ʈ �̸��� �����ɴϴ�.
            string hostName = Dns.GetHostName();

            // ȣ��Ʈ �̸��� IP �ּҷ� �ؼ��մϴ�.
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            // ��� IP �ּ� �߿��� IPv4 �ּҸ� ã���ϴ�.
            IPAddress ipAddress = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            return ipAddress?.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine("���� �߻�: " + ex.Message);
            return null;
        }
    }
}
