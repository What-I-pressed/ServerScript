using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerScript
{
    public struct Client
    {
        public int ID;
        public IPEndPoint endPoint;

        public Client(int _ID, IPEndPoint _endPoint) { 
            ID = _ID;
            endPoint = _endPoint;
        }
    }
}
