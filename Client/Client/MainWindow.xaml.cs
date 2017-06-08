using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket sender;
        byte[] bytes;
        public MainWindow()
        {
            InitializeComponent();
            Canvas_Think1.Visibility = Visibility.Collapsed;
            textBox_Say1.Text = "All variant of encription: Encript and ROT--" + Environment.NewLine + "Try to decript:" + Environment.NewLine + "Decript and ROT--";
            comboBox_ROT.SelectedIndex = 1;

        }


        private void butten_encrypt_Click(object sender, RoutedEventArgs e)
        {
            Canvas_Say1.Visibility = Visibility.Visible;
            textBox_Say1.Text = "All variant of encription: Encript and ROT--" + Environment.NewLine + "Try to decript:"+ Environment.NewLine +"Decript and ROT--";
            Canvas_Think1.Visibility = Visibility.Collapsed;
            string message = "";
            byte[] msg;
            int bytesSent;
            int bytesRec;
            if (Check(textBox_encrypt.Text))
            {
                if (Connect())
                {
                    if (comboBox_ROT.SelectedIndex != 0)
                    {
                        message = "encrypt";
                        msg = Encoding.UTF8.GetBytes(message);
                        bytesSent = this.sender.Send(msg);
                        bytesRec = this.sender.Receive(bytes);
                        if (Encoding.UTF8.GetString(bytes, 0, bytesRec) == "ok")
                        {

                            message = textBox_encrypt.Text + ";" + (comboBox_ROT.SelectedIndex - 1).ToString();
                            msg = Encoding.UTF8.GetBytes(message);
                            bytesSent = this.sender.Send(msg);
                            bytesRec = this.sender.Receive(bytes);
                            textBox_decrypt.Text = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        }
                    }
                    else
                    {
                        message = "encryptall";
                        msg = Encoding.UTF8.GetBytes(message);
                        bytesSent = this.sender.Send(msg);
                        bytesRec = this.sender.Receive(bytes);
                        if (Encoding.UTF8.GetString(bytes, 0, bytesRec) == "ok")
                        {
                            message = textBox_encrypt.Text;
                            msg = Encoding.UTF8.GetBytes(message);
                            bytesSent = this.sender.Send(msg);
                            bytesRec = this.sender.Receive(bytes);
                            string[] all = Encoding.UTF8.GetString(bytes, 0, bytesRec).Split(';');
                            textBox_decrypt.Text = "";
                            for (int i = 0; i < all.Length; i++)
                                textBox_decrypt.Text += all[i] + System.Environment.NewLine;
                        }
                    }
                    CreateTable();
                }
            }
            else
            {
                textBox_Say1.Text = "Incorrect input text!";
            }
        }
        private bool Connect()
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
                sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(ipEndPoint);
                bytes = new byte[12000];
                return true;
            }
            catch (Exception)
            {
                textBox_Say1.Text = "Connection Error!";
                return false;
            }
        }

        private void butten_decrypt_Click(object sender, RoutedEventArgs e)
        {
            Canvas_Say1.Visibility = Visibility.Visible;
            textBox_Say1.Text = "All variant of encription: Encript and ROT--" + Environment.NewLine + "Try to decript:" + Environment.NewLine + "Decript and ROT--";
            Canvas_Think1.Visibility = Visibility.Collapsed;
            string message = "";
            if (Check(textBox_encrypt.Text))
            {
                if (Connect())
                {
                    if (comboBox_ROT.SelectedIndex != 0)
                    {
                        message = "decrypt";
                        byte[] msg = Encoding.UTF8.GetBytes(message);
                        // Отправляем данные через сокет
                        int bytesSent = this.sender.Send(msg);
                        int bytesRec = this.sender.Receive(bytes);
                        if (Encoding.UTF8.GetString(bytes, 0, bytesRec) == "ok")
                        {

                            message = textBox_encrypt.Text + ";" + (comboBox_ROT.SelectedIndex - 1).ToString();
                            msg = Encoding.UTF8.GetBytes(message);
                            bytesSent = this.sender.Send(msg);
                            bytesRec = this.sender.Receive(bytes);
                            textBox_decrypt.Text = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        }
                    }
                    else
                    {
                        message = "decrypt?";
                        byte[] msg = Encoding.UTF8.GetBytes(message);
                        int bytesSent = this.sender.Send(msg);
                        int bytesRec = this.sender.Receive(bytes);
                        if (Encoding.UTF8.GetString(bytes, 0, bytesRec) == "ok")
                        {
                            message = textBox_encrypt.Text;
                            msg = Encoding.UTF8.GetBytes(message);
                            bytesSent = this.sender.Send(msg);
                            bytesRec = this.sender.Receive(bytes);
                            message = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                            string[] data = message.Split(';');
                            textBox_decrypt.Text = data[0];
                            Canvas_Think1.Visibility = Visibility.Visible;
                            Canvas_Say1.Visibility = Visibility.Collapsed;
                            textBox_Think1.Text = "I think ROT" + data[1];
                        }

                    }
                    CreateTable();
                }
            }
            else
            {
                textBox_Say1.Text = "Incorrect input text!";
            }
        }
        public void CreateTable()
        {
            if (Connect())
            {
                string message = "diagramm";
                byte[] msg = Encoding.UTF8.GetBytes(message);             
                int bytesSent = this.sender.Send(msg);
                int bytesRec = this.sender.Receive(bytes);
                string[] fr = Encoding.UTF8.GetString(bytes, 0, bytesRec).Split(';');
                int sum = 0, max=0;
                for (int i = 0; i < 26; i++)
                {
                    sum += Int32.Parse(fr[i]);
                }
                int[] f = new int[26];
                for(int i=0; i<26; i++)
                {
                    f[i] = int.Parse(fr[i]) * 100 / sum;
                    if (f[max] < f[i])
                        max = i;
                }
                int k;
                if (max <= 50)
                    k = 4;
                else k = 1;
                double x = 0;
                for (int i = 0; i < 26; i++)
                {       
                    var rectangle = new Rectangle();
                    rectangle.Width = Canvas_diagramm.Width / 26;
                    rectangle.Height =(Canvas_diagramm.Height*f[i]/100)*k;
                    rectangle.ToolTip = Convert.ToChar(65 + i) + " " + f[i].ToString() + "%";                
                    if (i % 2 == 0)
                        rectangle.Fill = Brushes.Bisque;
                    else
                        rectangle.Fill = Brushes.LightSlateGray;
                    Canvas.SetLeft(rectangle, x);
                    x+= Canvas_diagramm.Width / 26;
                    Canvas.SetTop(rectangle, Canvas_diagramm.Height-rectangle.Height);
                    Canvas_diagramm.Children.Add(rectangle);
                }
              
            }
        }
        public bool Check(string data)
        {
            for (int i = 0; i < data.Length; i++)
                if (data[i] > 127)
                    return false;
            return true;
        }
    }
   
}

