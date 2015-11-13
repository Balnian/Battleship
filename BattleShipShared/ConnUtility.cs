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
            /// <summary>
            /// Test si un client est encore connecté
            /// </summary>
            /// <param name="conn">Connection</param>
            /// <returns>retourne si le client est encore connecter</returns>
            public static bool TestClient(TcpClient conn)
            {
                if (conn == null)
                    return false;
                try
                {
                    return !(conn.Client.Poll(50, SelectMode.SelectRead) && conn.Client.Available == 0);
                }
                catch (SocketException) { return false; }


            }

            /// <summary>
            /// retourne l'Adresse IP d'un client
            /// </summary>
            /// <param name="conn">Connection</param>
            /// <returns>Adresse IP</returns>
            public static String GetIP(TcpClient conn)
            {
                return IPAddress.Parse(((IPEndPoint)conn.Client.RemoteEndPoint).Address.ToString()).ToString();
            }
        }
    }
}
