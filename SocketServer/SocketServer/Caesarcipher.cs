using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace SocketServer
{
    class Caesarcipher
    {

        public string History { get; set; }
        public Caesarcipher()
        {
            History = "";
        }     

        public void AddHistory(string data)
        {
            string[] Data = History.Split(';');
            for(int i=0; i<Data.Length; i++)
            {
                if (Data[i] == data)
                    return;
            }
            History+= data + ";";
        }
        public bool Save(string name)
        {
            try
            {
                FileStream file1 = new FileStream(name, FileMode.Create); 
                StreamWriter writer = new StreamWriter(file1);
                writer.Write(History);
                writer.Close(); 
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Load(string name)
        {
            try
            {
                FileStream file1 = new FileStream(name, FileMode.Open);
                StreamReader reader = new StreamReader(file1);
                History += reader.ReadToEnd();
                reader.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string Encrypt(string data, int rot)
        {
            string rezult = "";

            char[] Data = data.ToCharArray();
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i] >= 0 && Data[i] <= 64 || Data[i] >= 91 && Data[i] <= 96 || Data[i] >= 123)
                    rezult += Convert.ToChar(Data[i]);
                else if (Data[i] < 90)
                {
                    if (Data[i] + rot > 90)
                        rezult += Convert.ToChar(64 + (rot - (90 - Data[i])));
                    else if (Data[i] + rot < 65)
                        rezult += Convert.ToChar(90 - (65 - (Data[i] + rot + 1)));
                    else
                        rezult += Convert.ToChar(Data[i] + rot);
                }
                else
                {
                    if (Data[i] + rot > 122)
                        rezult += Convert.ToChar(96 + (rot - (122 - Data[i])));
                    else if (Data[i] + rot < 97)
                        rezult += Convert.ToChar(122 - (97 - (Data[i] + rot + 1)));
                    else
                        rezult += Convert.ToChar(Data[i] + rot);

                }

            }
            return rezult;
        }
        public int[] CreateDataDiagramm(string data)
        {
            int[] rezult = new int[26];
            for (int i = 0; i < 26; i++)
                rezult[i] = 0;
            string Data = data.ToLower();
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    if (Data[i] >= 0 && Data[i] <= 64 || Data[i] >= 91 && Data[i] <= 96 || Data[i] >= 123)
                        i++;
                    else
                    {
                        rezult[Data[i] - 97]++;
                    }
                }
            }
            return rezult;
        }
        public string TryToDecrypt(string data)
        {
            data = data.ToLower();
            int s;
            string rezult = "";
            double[] f = new double[26];
            double[] fa = new double[26];
            for (int i = 0; i < 26; i++)
                f[i] = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 0 && data[i] <= 64 || data[i] >= 91 && data[i] <= 96 || data[i] >= 123)
                    i++;
                else
                {
                    f[data[i] - 97]++;
                }

            }
            for (int i = 0; i < 26; i++)
                f[i] = f[i] * 100 / data.Length;
            int k=0;

            for(int i=0; i<26; i++)
            {
                if (f[k] < f[i])
                    k = i;
            }
            s = Math.Abs(k - 4);
            if (s > 0)
                return rezult = Encrypt(data, -s) + ";" + s;
            else
                return rezult = Encrypt(data, s) + ";" + -s;
        }
    }
}
