using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiNiDown
{
    class Writehistory
    {
        MyDownLoad m;
        int number;
        string[] save;
        public Writehistory(MyDownLoad mm)
        {
            this.m = mm;
        }
        public void AddHistory()
        {
            FileStream MyFileStream;
            if (!File.Exists("MiniDownHis.txt"))
            {
                MyFileStream = new FileStream("MiniDownHis.txt", System.IO.FileMode.Create); 
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine("1");
                sw.WriteLine(m.true_filename);
                sw.WriteLine(m.path);
                sw.WriteLine(m.strurl);
                sw.Close();
                //sw.WriteLine();
            }
            else
            {
                MyFileStream = File.OpenRead("MiniDownHis.txt");
                StreamReader sr = new StreamReader(MyFileStream);
                number = Convert.ToInt32(sr.ReadLine());
                save = new string[3*number];
                for (int i=0;i<(3*number);i++)
                {
                    save[i]=sr.ReadLine();
                }
                sr.Close();
                MyFileStream = File.OpenWrite("MiniDownHis.txt");
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine((number+1).ToString());
                sw.WriteLine(m.true_filename);
                sw.WriteLine(m.path);
                sw.WriteLine(m.strurl);
                for (int i = 0; i < (3 * number); i++)
                {
                    sw.WriteLine(save[i]);
                }

                sw.Flush();
                //关闭流
                sw.Close();
            }
        }
    }
}
