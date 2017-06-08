using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SocketServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Caesarcipher caesarcipher = new Caesarcipher();
            int[] frequency = new int[26];
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            caesarcipher.Load("admin.txt");
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    Socket handler = sListener.Accept();
                    string data = null;
                    string reply = "";
                    byte[] bytes = new byte[12000];
                    int bytesRec = handler.Receive(bytes);
                    byte[] msg;
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    Console.Write("Полученный текст: " + data + "\n\n");
                    if (data == "encrypt")
                    {
                        msg = Encoding.UTF8.GetBytes("ok");
                        handler.Send(msg);
                        bytes = new byte[1024];
                        bytesRec = handler.Receive(bytes);
                        data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        caesarcipher.AddHistory(data);
                        string[] Data = data.Split(';');
                        reply = caesarcipher.Encrypt(Data[0], Int32.Parse(Data[1]));
                        msg = Encoding.UTF8.GetBytes(reply);
                        handler.Send(msg);
                    }
                    if (data == "decrypt")
                    {
                        msg = Encoding.UTF8.GetBytes("ok");
                        handler.Send(msg);
                        bytes = new byte[12000];
                        bytesRec = handler.Receive(bytes);
                        data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        string[] Data = data.Split(';');
                        reply = caesarcipher.Encrypt(Data[0], -Int32.Parse(Data[1]));
                        caesarcipher.AddHistory(reply);
                        msg = Encoding.UTF8.GetBytes(reply);
                        handler.Send(msg);
                    }
                    if (data == "encryptall")
                    {
                        msg = Encoding.UTF8.GetBytes("ok");
                        handler.Send(msg);
                        bytes = new byte[12000];
                        bytesRec = handler.Receive(bytes);
                        data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        caesarcipher.AddHistory(data);
                        for (int i = 1; i < 26; i++)
                        {
                            reply += caesarcipher.Encrypt(data, i) + ";";
                        }
                        msg = Encoding.UTF8.GetBytes(reply);
                        handler.Send(msg);
                    }
                    if (data == "decrypt?")
                    {
                        msg = Encoding.UTF8.GetBytes("ok");
                        handler.Send(msg);
                        bytes = new byte[12000];
                        bytesRec = handler.Receive(bytes);
                        data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        reply += caesarcipher.TryToDecrypt(data);
                        caesarcipher.AddHistory(reply);
                        msg = Encoding.UTF8.GetBytes(reply);
                        handler.Send(msg);
                    }
                    if(data=="diagramm")
                    {
                        frequency = caesarcipher.CreateDataDiagramm(caesarcipher.History);
                        for(int i=0; i<26; i++)
                        {
                            reply += frequency[i] + ";";
                        } 
                        msg = Encoding.UTF8.GetBytes(reply);
                        handler.Send(msg);
                    }
                    
                    caesarcipher.Save("admin.txt");
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                
   
                Environment.Exit(0);

            }
        }
       
    }
}