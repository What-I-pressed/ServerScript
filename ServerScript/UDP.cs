using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerScript
{
    public class UDP
    {
        UdpClient client;
        private int port = 5518;

        public UDP() {
            client = new UdpClient(port); 
        }

        public async Task StartChat(List<Client> clients)
        {

        }

        public async Task StartListen()
        {
            client.BeginReceive(Receive, null);
        }

        private void Receive(IAsyncResult _res)
        {
            try
            {
                IPEndPoint _endPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = client.EndReceive(_res, ref _endPoint);
                client.BeginReceive(Receive, null);
                
                //if (_data.Length < 4) return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
