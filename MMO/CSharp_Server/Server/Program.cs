using ServerCore;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        public static GameRoom Room { get; set; } = new GameRoom();
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            IPAddress IPAddr = IPAddress.Parse(GetLocalIPAddress());
            IPEndPoint endPoint = new IPEndPoint(IPAddr, 7777);

            _listener.Init(endPoint, () => SessionManager.Session.Generate());

            Console.WriteLine("Listening ...");
            while (true)
            {
                ;
            }
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
}