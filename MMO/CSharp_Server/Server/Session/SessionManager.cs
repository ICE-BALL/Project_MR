using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class SessionManager
    {
        static SessionManager _instance = new SessionManager();
        public static SessionManager Session { get { return _instance; } }

        public int _sessionId = 0;
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        object _lock = new object();

        public ClientSession Generate()
        {
            lock (_lock)
            {
                int Id = ++_sessionId;
                ClientSession session = new ClientSession();
                session.SessionId = Id;
                _sessions.Add(session.SessionId, session);

                return session;
            }
        }

        public void Remove(int Id)
        {
            lock (_lock)
            {
                _sessions.Remove(Id);
            }
        }

        public ClientSession Find(int Id)
        {
            lock (_lock)
            {
                ClientSession session = null;
                if (_sessions.TryGetValue(Id, out session))
                    return session;

                return null;
            }
        }
    }
}
