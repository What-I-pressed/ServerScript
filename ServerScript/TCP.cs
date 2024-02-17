using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;

namespace ServerScript
{

    public class TCP
    {
        public TcpListener _tcpListener;
        private int _port = 5518;
        private event AsyncCallback _incomingMessageHandler;

        public TCP()
        {
            try
            {
                IPAddress serverIP = Dns.GetHostAddresses(Dns.GetHostName())[0];
                _tcpListener = new TcpListener(new IPEndPoint(serverIP, _port));
                _tcpListener.Start();
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
            
        }

        public async Task StartListen()
        {
            //Console.WriteLine("Server start listen");
            //while (true)
            //{
            //    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
            //    Console.WriteLine("New client has been accepted!!");
            //}

            _tcpListener.BeginAcceptTcpClient(AcceptClient, null);
        }

        private void AcceptClient(IAsyncResult res) { 
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
            
            
            Task.Run(() => MessageHandler(_tcpListener.EndAcceptTcpClient(res)));
            _tcpListener.BeginAcceptTcpClient(AcceptClient, null);
            
            //byte[] message = new byte[1];
            //
            //AsyncCallback handler = async (IAsyncResult result) =>
            //{
            //    client.Client.EndReceive(result);
            //    Console.WriteLine("Send message request was accepted");
            //    NetworkStream networkStream = client.GetStream();
            //    message = new byte[networkStream.Length];
            //    networkStream.Read(message, 0, message.Length);
            //    SocketAsyncEventArgs asyncEventArgs = new SocketAsyncEventArgs();
            //    asyncEventArgs.SetBuffer(message);
            //    client.Client.SendToAsync(asyncEventArgs, ClientManager.GetClientIPEndPoint(BitConverter.ToInt32(new byte[] { message[1], message[2], message[3], message[4] })));
            //};
            //_incomingMessageHandler += handler;
            //_incomingMessageHandler += (IAsyncResult res) => { client.Client.BeginReceive(message, 0, message.Length, SocketFlags.None, _incomingMessageHandler, null); };
            //client.Client.BeginReceive(message, 0, message.Length, SocketFlags.None, _incomingMessageHandler, null);
        }

        private async void MessageHandler(TcpClient client)
        {
            while (true)
            {
                try
                {
                    NetworkStream nStream = client.GetStream();
                    Console.WriteLine("Connection accepted");
                    byte[] buffer = new byte[5];
                    nStream.Read(buffer, 0, 5);
                    if(buffer.Length > 3) {
                        switch (buffer[0])
                        {
                            case (byte)RequestType.AddClient:
                                ClientManager.AddClient(BitConverter.ToInt32(new byte[] { buffer[1], buffer[2], buffer[3], buffer[4] }, 0),
                                    client.Client.RemoteEndPoint as IPEndPoint);
                                Console.WriteLine("New client has been added");
                                ClientManager.Info();
                                break;
                            case (byte)RequestType.AudioConnectWith:


                                break;
                            case (byte)RequestType.SendTextMessage:

                                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                MemoryStream memoryStream = new MemoryStream();
                                nStream.CopyTo(memoryStream);
                                byte[] message = memoryStream.GetBuffer();
                                Console.WriteLine("Server sending connection request...");
                                socket.Connect(ClientManager.GetClientIPEndPoint(BitConverter.ToInt32(new byte[] { message[5], message[6], message[7], message[8] })));             //Change IT!!!@!!!!!!!!!!
                                Console.WriteLine("Connection request was accepted");
                                SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
                                socketAsyncEventArgs.SetBuffer(message);
                                Task.Run(() => socket.SendAsync(socketAsyncEventArgs));
                                Console.WriteLine("Message was forwarded");
                                break;

                        }
                    }
                    
                }
                catch (ArgumentException ex) { Console.WriteLine("Argument exception \n" + ex.Message); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                System.Threading.Thread.Sleep(200);
            }
        }
    }
}
