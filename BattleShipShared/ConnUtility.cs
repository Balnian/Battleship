using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipShared
{
    namespace UtilityTools
    {
        public class ConnUtility
        {
            public static bool TestClient(TcpClient conn)
            {
                if (conn == null)
                    return false;
                try
                {
                    return !(conn.Client.Poll(150, SelectMode.SelectRead) && conn.Client.Available == 0);
                }
                catch (SocketException) { return false; }


            }

            public static String GetIP(TcpClient conn)
            {
                return IPAddress.Parse(((IPEndPoint)conn.Client.RemoteEndPoint).Address.ToString()).ToString();
            }
        }
    }
}
