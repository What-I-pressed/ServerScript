using ServerScript;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Security;

internal class Program
{
    private static void Main()
    {
        TCP TcpManager = new TCP();
        TcpManager.StartListen();
        while (true) ;

        //IPEndPoint endPoint = new IPEndPoint(GetIP(), 5000);
        //Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //tcpClient.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive);
        //tcpClient.Bind(endPoint);
        //tcpClient.Connect(new IPEndPoint(GetIP(), 6000));
        //tcpClient.Send(new byte[] {128, 0});
        
    }
    private static IPAddress GetIP()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) return ip;
        }
        //return new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        return null;
    }
}