using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System.Net.NetworkInformation;

namespace ServerScript
{

    public class TCP
    {
        public TcpListener _tcpListener;
        private int _port = 5518;

        public TCP()
        {
            _tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port));
            _tcpListener.Start();
        }

        public async Task StartListen()
        {
            Console.WriteLine("Server start listen");
            while (true)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                Console.WriteLine("New client has been accepted!!");
            }

            //_tcpListener.BeginAcceptTcpClient(AcceptClient, null);
        }

        private void AcceptClient(IAsyncResult res) { 
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
            try
            {
                TcpClient client = _tcpListener.EndAcceptTcpClient(res);
                Console.WriteLine("Connection accepted");
                _tcpListener.BeginAcceptTcpClient(AcceptClient, null);
                NetworkStream nStream = client.GetStream();

                byte[] buffer = new byte[5];
                nStream.Read(buffer, 0, 5);
                switch (buffer[0])
                {
                    case (byte)RequestType.AddClient:               
                        ClientManager.AddClient(BitConverter.ToInt32(new byte[] { buffer[1], buffer[2], buffer[3], buffer[4] }, 0),
                            client.Client.RemoteEndPoint as IPEndPoint);
                        Console.WriteLine("New client has been added");
                        break;
                    case (byte)RequestType.AudioConnectWith:
                                                                       

                        break;              
                    case (byte)RequestType.SendTextMessage:
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(ClientManager.GetClientIPEndPoint(BitConverter.ToInt32(new byte[] { buffer[1], buffer[2], buffer[3], buffer[4] })));
                        buffer = new byte[nStream.Length];                                      
                        nStream.Read(buffer, 0, buffer.Length);
                        SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
                        socketAsyncEventArgs.SetBuffer(buffer);
                        Task.Run(() => socket.SendAsync(socketAsyncEventArgs));
                        break;
                }
            }
            catch (ArgumentException ex) { Console.WriteLine("Argument exception \n" + ex.Message); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            
        }
    }
}
