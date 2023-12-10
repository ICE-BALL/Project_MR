using System.Net;
using ServerCore;

namespace DummyClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress IPAddr = IPAddress.Parse("192.168.249.185");
            IPEndPoint endPoint = new IPEndPoint(IPAddr, 7777);

            Connector connector = new Connector();
            connector.Init(endPoint, () => new ServerSession());

            while (true)
            {
                ;
            }
        }
    }
}