using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WinFormsApp1
{
    public delegate void Change(string s);
    public class TcpServer
    {
        
        public event Change ReceiveMes;     
        //私有成员
        private static byte[] result = new byte[1024];
        private int myProt = 500;   //端口  
        static Socket serverSocket;
        static Socket clientSocket;
        
        Thread myThread;
        static Thread receiveThread;

        //属性

        public int port { get; set; }
        //方法

        internal void StartServer()
        {
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  

            Debug.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());

            //通过Clientsoket发送数据  
            myThread = new Thread(ListenClientConnect);
            myThread.Start();

        }
        private string mes;
        public string MyMes
        {
            get { return mes; }
            set
            {
                if (value != mes)
                {
                    mes = value;
                    ReceiveMes(value);
                }
            }
        }
        internal void QuitServer()
        {

            serverSocket.Close();
            clientSocket.Close();
            myThread.Abort();
            receiveThread.Abort();


        }


        internal void SendMessage(string msg)
        {
            clientSocket.Send(Encoding.ASCII.GetBytes(msg));
        }



        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private  void ListenClientConnect()
        {
            while (true)
            {
                try
                {
                    clientSocket = serverSocket.Accept();
                    clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                    receiveThread = new Thread(ReceiveMessage);
                    receiveThread.Start(clientSocket);
                }
                catch (Exception)
                {

                }

            }
        }
       


        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private  void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);
                    Debug.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                    MyMes= Encoding.ASCII.GetString(result, 0, receiveNumber);
                    //string msg = Encoding.ASCII.GetString(result, 0, receiveNumber);
                    //ReceiveMes(msg);
                    
                }
                catch (Exception ex)
                {
                    try
                    {
                        Debug.WriteLine(ex.Message);
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Close();
                        break;
                    }
                    catch (Exception)
                    {
                    }

                }
            }
        }
    }
}
