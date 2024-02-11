using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerScript
{
    public enum RequestType
    {
        AddClient,
        AudioConnectWith,
        SendTextMessage
    }

    public static class ClientManager
    {
        public static volatile List<Client> _clients = new List<Client>();

        public static void AddClient(int ID, IPEndPoint endPoint)
        {
            endPoint.Port -= 1;
            _clients.Add(new Client(ID, endPoint));
        }

        public static void AddClient(Client client)
        {
            _clients.Add(client);
        }

        public static bool ContainClientWith(IPEndPoint endPoint)
        {
            if (endPoint == null) throw new ArgumentNullException("Client endPoint was null");
            for(int i = 0; i < _clients.Count; i++) 
                if (_clients[i].endPoint.Address.Equals(endPoint.Address)) return true;
            return false;
        }

        public static IPEndPoint GetClientIPEndPoint(int ID)
        {
            for(int i = 0; i < _clients.Count; i++)
                if (_clients[i].ID == ID) 
                    return _clients[i].endPoint;

            return null;
        }
    }
} 
