using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace MiNiDown
{
    class HttpFile
    {
        public MyDownLoad formm;
        public int threadh;//线程代号
        public string filename;//文件名
        public string strUrl;//接收文件的URL
        public FileStream fs;
        public HttpWebRequest request;
        public System.IO.Stream ns;
        public byte[] nbytes;//接收缓冲区
        public int nreadsize;//接收字节数
        private long DownLoadSize;
        public HttpFile(MyDownLoad form, int thread)//构造方法
        {
            formm = form;
            threadh = thread;
        }
       
        public void receive()//接收线程
        {
            filename = formm.filenamew[threadh];
            strUrl = formm.strurl;
            ns = null;
            nbytes = new byte[512];
            nreadsize = 0;
            
            Console.WriteLine("线程" + threadh.ToString() + "开始接收");
            if (!File.Exists(filename))
            {
                fs = new FileStream(filename, System.IO.FileMode.Create);
            }
            else
            {
                fs = File.OpenWrite(filename);
                DownLoadSize = fs.Length;
                formm.DownLoadtotal += DownLoadSize;
                formm.filestartw[threadh] += DownLoadSize;
                fs.Seek(DownLoadSize, SeekOrigin.Current);   //已经存在
            }
            if (!formm.threadw[threadh])
            {
                try
              {
                 request = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                //接收的起始位置及接收的长度 
                 request.AddRange(formm.filestartw[threadh], formm.filestartw[threadh] + formm.filesizew[threadh]-DownLoadSize);
                 ns = request.GetResponse().GetResponseStream();//获得接收流
                 nreadsize = ns.Read(nbytes, 0, 512);
                
                 while (nreadsize > 0)
                 {
                    fs.Write(nbytes, 0, nreadsize);
                    nreadsize = ns.Read(nbytes, 0, 512);
                    formm.DownLoadtotal += 512;
                     Console.WriteLine("线程" + threadh.ToString() + "正在接收" + formm.DownLoadSpeed.ToString() + "k/s___已经下了" + formm.DownLoadtotal.ToString() + "还剩下" + formm.LeftDownLoadTime.ToString() + "s" + "百分比" + formm.DownLoadPercentage.ToString());
                 }
                 fs.Close();
                 ns.Close();
              }
               catch (Exception er)
              {
                Console.WriteLine("123");
                fs.Close();
              }
            }
            
           Console.WriteLine("进程" + threadh.ToString() + "接收完毕!");
            formm.threadw[threadh] = true;
        }




    }
}
