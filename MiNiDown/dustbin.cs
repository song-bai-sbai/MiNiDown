using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

namespace MiNiDown
{
    class dustbin
    {
        MyDownLoad m;
        public dustbin(MyDownLoad mm)
        {
            this.m = mm;
        }
        public string back_url()
        {
            string str="";
            if (!File.Exists("MiniDowndustbin.txt"))
            {
               
                return str;
            }
            else
            {
                FileStream MyFileStream = File.OpenRead("MiniDowndustbin.txt");
                StreamReader sr = new StreamReader(MyFileStream);
                str=sr.ReadLine();
                sr.Close();
                return str;
            }
        }
        public string back_path()
        {
            string str = "";
            if (!File.Exists("MiniDowndustbin.txt"))
            {
                
                return str;
            }
            else
            {
                FileStream MyFileStream = File.OpenRead("MiniDowndustbin.txt");
                StreamReader sr = new StreamReader(MyFileStream);
                sr.ReadLine();
                str = sr.ReadLine();
                sr.Close();
                return str;
            }
        }
        public void check()
        {
            if (!File.Exists("MiniDowndustbin.txt"))
            {
                MessageBox.Show("empty");
                
            }
            else
            {
                FileStream MyFileStream = File.OpenRead("MiniDowndustbin.txt");
                StreamReader sr = new StreamReader(MyFileStream);
                string str;
                str = "任务URL: ";
                str += sr.ReadLine()+"\n";
                str += "保存路径: ";
                str += sr.ReadLine();
                sr.Close();
                MessageBox.Show(str);
            }
        }
        public void Add()
        {
            FileStream MyFileStream;
            if (!File.Exists("MiniDowndustbin.txt"))
            {
                MessageBox.Show("empty");
                MyFileStream = new FileStream("MiniDowndustbin.txt", System.IO.FileMode.Create); 
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine(m.strurl);
                sw.WriteLine(m.path);
                sw.Close();
            }
            else
            {
                MessageBox.Show("已有垃圾文件，将覆盖");
                MyFileStream = File.OpenWrite("MiniDowndustbin.txt");
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine(m.strurl);
                sw.WriteLine(m.path);
                sw.Flush();
                //关闭流
                sw.Close();
            }
        }
    }
    
}
