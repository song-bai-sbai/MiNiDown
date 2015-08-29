using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows;
namespace MiNiDown
{
    class MyDownLoad
    {
        public bool[] threadw; //每个线程结束标志
        public string[] filenamew;//每个线程接收文件的文件名
        public long[] filestartw;//每个线程接收文件的起始位置
        public long[] filesizew;//每个线程接收文件的大小
        public string strurl;//接受文件的URL
        public bool hb;//文件合并标志
        public int thread;//进程数
        DateTime dt;//开始下载时间
        public string path = null;//
        public double DownLoadtotal=0;
        public double DownLoadPercentage=0;
        public double DownLoadSpeed=0;
        public double TotalDownLoadTime;//用的时间
        public double LeftDownLoadTime=9999999;
        public string filetype;
        public double DownLoadsize;
        public string true_filename;
        public string setfilename;
        public bool downornot=false;
        public Thread testspeed;//计算数据 
        Thread hbth;   //合并
        Thread[] threadk;    //下载
        HttpFile[] httpfile;   //下载
        public Writehistory his;// history
        public void GetPath(string Path)
        {
            this.path = Path;
        }
        public void GetURLType(string url)
        {
            int TypePosition = url.LastIndexOf(".");
            filetype = url.Substring(TypePosition + 1, url.Length - TypePosition - 1);
            
        }
        public void GetFileName(string url)
        {
            int FileNamePosition = url.LastIndexOf("/");
            int TypePosition = url.LastIndexOf(".");
            true_filename = url.Substring(FileNamePosition + 1, TypePosition - FileNamePosition - 1); 
        }
        public void Start_load(int number,string url,string path1)
        {
            downornot = true;
            strurl = url;
            GetPath(path1);
            GetURLType(url);
            GetFileName(url);
            setfilename = path+true_filename + "set." + "txt";
            if (File.Exists(setfilename))
            {
                Console.WriteLine("exist");
                Restart_load();
            }
            else
            {
                Console.WriteLine("not exist");
                dt = DateTime.Now;//开始接收时间
                HttpWebRequest request;  
                long filesize = 0;
                downloadcontrol downcontrol = new downloadcontrol(this);   //set speed etc.
                testspeed = new Thread(downcontrol.set);
                testspeed.Start();
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(strurl);
                    filesize = request.GetResponse().ContentLength;//取得目标文件的长度
                    DownLoadsize = filesize;
                    request.Abort();
                }
                catch (Exception er)
                {
                    //MessageBox.Show (er.Message );
                }
                // 接收线程数
                thread = number;
                //根据线程数初始化数组
                threadw = new bool[thread];
                filenamew = new string[thread];
                filestartw = new long[thread];
                filesizew = new long[thread];
                //计算每个线程应该接收文件的大小
                long filethread = filesize / thread;//平均分配
                long filethreade = filesize - filethread * (thread - 1);//剩余部分由最后一个线程完成
                //为数组赋值
                for (int i = 0; i < thread; i++)
                {
                    threadw[i] = false;//每个线程状态的初始值为假
                    filenamew[i] = path+true_filename + i.ToString() + ".dat";//每个线程接收文件的临时文件名
                    if (i < thread - 1)
                    {
                        filestartw[i] = filethread * i;//每个线程接收文件的起始点
                        filesizew[i] = filethread - 1;//每个线程接收文件的长度
                    }
                    else
                    {
                        filestartw[i] = filethread * i;
                        filesizew[i] = filethreade - 1;
                    }

                }
                //保存到文件
                FileStream setfs;
                setfs = new FileStream(path+true_filename + "set." + "txt", System.IO.FileMode.Create);
                StreamWriter sw = new StreamWriter(setfs);
                sw.WriteLine(strurl);
                sw.WriteLine(thread);
                sw.WriteLine(path);
                for (int i = 0; i < thread; i++)
                {
                    sw.WriteLine(filenamew[i]);//每个线程接收文件的文件名
                    sw.WriteLine(filestartw[i]);//每个线程接收文件的起始位置
                    sw.WriteLine(filesizew[i]);//每个线程接收文件的大小
                    sw.WriteLine(threadw[i]);
                }
                sw.WriteLine(dt);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                setfs.Close();
                //定义线程数组，启动接收线程
                threadk = new Thread[thread];
                httpfile = new HttpFile[thread];
                for (int j = 0; j < thread; j++)
                {
                    httpfile[j] = new HttpFile(this, j);
                    threadk[j] = new Thread(new ThreadStart(httpfile[j].receive));
                    threadk[j].Start();
                }
                //启动合并各线程接收的文件线程
                hbth = new Thread(new ThreadStart(hbfile));
                hbth.Start();
            }
            
        }

        public void hbfile()
        {
　          while (true)//等待
　          {
　　           hb=true;
　　           for (int i=0;i<thread;i++)
　　           {
　　　           if (threadw[i]==false)//有未结束线程，等待
　　　           {
　　　　           hb=false;
　　　　           Thread.Sleep (100);
　　　　           break;
           　　　}
　　           }
           　　if (hb==true)//所有线程均已结束，停止等待，
　　           {
                 //
　　           　break;
           　　}
　           }
　           FileStream fs;//开始合并
           　FileStream fstemp;
　           int readfile;
　           byte[] bytes=new byte[512];
　           fs=new FileStream (path+true_filename+"."+filetype,System.IO.FileMode.Create);
　           for (int k=0;k<thread;k++)
　           {
　　           fstemp=new FileStream (filenamew[k],System.IO.FileMode .Open);
　　           while (true)
　　           {
　　　           readfile=fstemp.Read (bytes,0,512);
　　　           if (readfile>0)
　　　           {
　　　　           fs.Write (bytes,0,readfile);
                  
　　　           }
　　　           else
　　　           {
　　　           　break;
　　　           }
　　           }
　　           fstemp.Close ();
           　}
            for (int i=0;i<thread;i++)
            {
                File.Delete(filenamew[i]);
            }
            File.Delete(setfilename);
　          fs.Close ();
           
            for (int j = 0; j < thread; j++)
            {
                threadk[j].Abort();
            }
            testspeed.Abort();
            TimeSpan tp = new TimeSpan();
　          tp=DateTime.Now-dt;
            TotalDownLoadTime = Convert.ToDouble(tp.TotalSeconds);
　           //textBox1.Text =dt.ToString ();//结束时间
            DownLoadsize = 0;
            DownLoadtotal = 0;
　           Console.WriteLine("接收完毕!!!"+TotalDownLoadTime.ToString()+"s");
            his = new Writehistory(this);
            his.AddHistory();
            MessageBox.Show("下载完成");
            downornot = false;
            hbth.Abort();
           }

        public void Restart_load()
        {
            HttpWebRequest request;
            request = (HttpWebRequest)HttpWebRequest.Create(strurl);
            DownLoadsize= request.GetResponse().ContentLength;//取得目标文件的长度 
            request.Abort();
            StreamReader sw = new StreamReader(path+true_filename + "set." + "txt");
            strurl=sw.ReadLine();
            thread=Convert.ToInt32(sw.ReadLine());
            path = sw.ReadLine();
            //根据线程数初始化数组
            threadw = new bool[thread];
            filenamew = new string[thread];
            filestartw = new long[thread];
            filesizew = new long[thread];
            for (int i = 0; i < thread; i++)
            {
                filenamew[i]=sw.ReadLine();//每个线程接收文件的文件名
                filestartw[i]=Convert.ToInt32(sw.ReadLine());//每个线程接收文件的起始位置
                filesizew[i] = Convert.ToInt32(sw.ReadLine());//每个线程接收文件的大小
                threadw[i] = Convert.ToBoolean(sw.ReadLine());
                Console.WriteLine(filenamew[i]+filesizew[i].ToString());
            }
            dt = Convert.ToDateTime(sw.ReadLine());       
            sw.Close();
            Console.ReadLine();
            threadk = new Thread[thread];
            httpfile = new HttpFile[thread];
            for (int j = 0; j < thread; j++)
            {
                   httpfile[j] = new HttpFile(this, j);
                   threadk[j] = new Thread(new ThreadStart(httpfile[j].receive));
                   threadk[j].Start();              
            }
            downloadcontrol downcontrol = new downloadcontrol(this);
            testspeed = new Thread(downcontrol.set);
            testspeed.Start();
            //启动合并各线程接收的文件线程
            hbth = new Thread(new ThreadStart(hbfile));
            hbth.Start();
        }
        
        public void stop_down()
        {
            downornot = false;
            for (int j = 0; j < thread; j++)
            {
                
                threadk[j].Abort();
            }
            hbth.Abort();
            DownLoadtotal = 0;
            testspeed.Abort();
        }
        public void deletedown()
        {
            for (int i = 0; i < thread; i++)
            {
                File.Delete(filenamew[i]);
            }
            File.Delete(setfilename);
        }
    }
   
}