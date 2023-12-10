using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    internal class ServerSession : PacketSession
    {
        public override void OnConnect(EndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public override void OnDisconnect(EndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public override void OnRecvPacket(ArraySegment<byte> Buffer)
        {
            throw new NotImplementedException();
        }

        public override void OnSend(int numOfbytes)
        {
            throw new NotImplementedException();
        }
    }
}
