using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class GameManager
    {
        static GameManager _instance = new GameManager();
        public static GameManager Instance { get { return _instance; } }

        public enum Map_Zone
        {
            Map_001,
            Map_002,
            Map_003,
            Map_004,
        }

        public void SetMap(ClientSession s)
        {
            if (s.Map_Zone == 0)
                s.Map_Zone = (int)Map_Zone.Map_001;
        }
    }
}
