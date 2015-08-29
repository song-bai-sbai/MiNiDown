using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;
using System.IO;

namespace MiNiDown
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MyDownLoad m1;
        MyDownLoad m2;
        ShowData show1;
        ShowData show2;
        Thread updateshow;
        dustbin dustbin1;
        dustbin dustbin2;
        Readhistory readhistory;
        public MainWindow()
        {
            InitializeComponent();
            m1= new MyDownLoad();
            m2 = new MyDownLoad();
            show1 = new ShowData(m1);
            show2 = new ShowData(m2);
            dustbin1=new dustbin(m1);
            dustbin2=new dustbin(m2);
            readhistory = new Readhistory();
            updateshow = new Thread(new ThreadStart(updatedata));
            updateshow.Start();
        }
        private void Mouse_Move(object sender,RoutedEventArgs e)
        {
             this.DragMove();
        }
        private void button6_Click(object sender, RoutedEventArgs e) //关闭窗口
        {
            if (m1.downornot==true)
            {
                m1.stop_down();
            }
            if (m2.downornot == true)
            {
                m2.stop_down();
            }
            updateshow.Abort();
            this.Close();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button1_Click(object sender, RoutedEventArgs e)    // 下载一下载
        {
            if (textBox_url_1.Text!="")
            {
                HttpWebRequest request=null;
                 try
              {
                 request = (HttpWebRequest)HttpWebRequest.Create(textBox_url_1.Text);
                 request.GetResponse();//获得接收流
                 request.Abort();
                 chosepath ch = new chosepath();
                 string path = ch.SaveFilePath()+@"\";
                 m1.Start_load(3,textBox_url_1.Text,path);
                 labelname1.Content = m1.true_filename;
                 textBox_url_1.Text = "";
              }
               catch (Exception er)
              {
                MessageBox.Show("建立连接失败");
                
              }
            } 
           
            else
            {
                MessageBox.Show("建立连接失败");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)//下载一取消
        {
            textBox_url_1.Text = "";
        }

        private void button4_Click(object sender, RoutedEventArgs e)//下载一开新任务、垃圾
        {
            if (m1.downornot==true)
            {
                m1.stop_down();
                Thread.Sleep(500);
                dustbin1.Add();
            }
        }

        private void button_stop1_Click(object sender, RoutedEventArgs e)//下载一暂停
        {
            if (m1.downornot == true)
            {
                m1.stop_down();
                m1.downornot = false;
            }
            
        }

        private void button_start1_Click(object sender, RoutedEventArgs e)//下载一开始
        {
            if (m1.downornot == false)
            {
                m1.Restart_load();
                m1.downornot = true;
            }
        }

        public void updatedata()   //显示下载信息
        {
            while(true)
            {
          
                if (m1.downornot == true)
                {
                    Dispatcher.Invoke((Action)delegate
                    { labelspeed1.Content = show1.ShowSpeed();});
                    Dispatcher.Invoke((Action)delegate
                    { labellefttime1.Content = show1.ShowLeftTime(); });
                    Dispatcher.Invoke((Action)delegate
                    { progressBar1.Value = show1.ShowPercentage(); });

                }
                if (m1.downornot==false)
                {
                    Dispatcher.Invoke((Action)delegate
                    { labelspeed1.Content = "0kb/s"; });
                    Dispatcher.Invoke((Action)delegate
                    { labellefttime1.Content = "---"; });
                }
                if (m2.downornot == true)
                {
                    Dispatcher.Invoke((Action)delegate
                    { labelspeed2.Content = show2.ShowSpeed(); });
                    Dispatcher.Invoke((Action)delegate
                    { labellefttime2.Content = show2.ShowLeftTime(); });
                    Dispatcher.Invoke((Action)delegate
                    { progressBar3.Value = show2.ShowPercentage(); });

                }
                if (m2.downornot == false)
                {
                    Dispatcher.Invoke((Action)delegate
                    { labelspeed2.Content = "0kb/s"; });
                    Dispatcher.Invoke((Action)delegate
                    { labellefttime2.Content = "---"; });
                }
                Thread.Sleep(500);
            }
            
        }

        private void button5_Click_1(object sender, RoutedEventArgs e)  //下载2下载
        {
            if (textBox_url2.Text != "")
            {
                HttpWebRequest request = null;
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(textBox_url2.Text);
                    request.GetResponse();//获得接收流
                    request.Abort();
                    chosepath ch = new chosepath();
                    string path = ch.SaveFilePath() + @"\";
                    m2.Start_load(3, textBox_url2.Text, path);
                    labelname1.Content = m2.true_filename;
                    textBox_url2.Text = "";
                }
                catch (Exception er)
                {
                    MessageBox.Show("建立连接失败");

                }
            }

            else
            {
                MessageBox.Show("建立连接失败");
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)//下载2取消 
        {
            textBox_url2.Text = "";
        }

        private void buttonstart2_Click(object sender, RoutedEventArgs e)// 下载2开始
        {
            if (m2.downornot == false)
            {
                m2.Restart_load();
                m2.downornot = true;
            }
        }

        private void buttonstop2_Click(object sender, RoutedEventArgs e)//下载2暂停
        {
            if (m2.downornot == true)
            {
                m2.stop_down();
                m2.downornot = false;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)//彻底删除1
        {
            m1.stop_down();
            Thread.Sleep(1000);
            m1.deletedown();
        }

        private void button6_Click_1(object sender, RoutedEventArgs e)//彻底删除2
        {
            m2.stop_down();
            Thread.Sleep(1000);
            m2.deletedown();
        }

        private void button11_Click(object sender, RoutedEventArgs e)//开新任务、垃圾2
        {
            if (m2.downornot==true)
            {
                m2.stop_down();
                Thread.Sleep(500);
                dustbin2.Add();
            }
        }

        private void button8_Click_1(object sender, RoutedEventArgs e)//查看垃圾
        {
            dustbin1.check();
        }

        private void button9_Click(object sender, RoutedEventArgs e)// 还原垃圾
        {
            if (m2.downornot==false)
            {
                string url, path;
                url = dustbin1.back_url();
                path = dustbin1.back_path();
                if (url=="")
                {
                    MessageBox.Show("no record");
                }
                else
                {
                    m2.Start_load(3, url, path);
                    Thread.Sleep(500);
                    File.Delete("MiniDowndustbin.txt");
                }
            }
        }

        private void 返回_Click(object sender, RoutedEventArgs e)//历史记录返回
        {
            labelhis.Content = "";
        }

        private void 查看历史记录_Click(object sender, RoutedEventArgs e)// 查看历史记录
        {
           labelhis.Content=readhistory.readhistroy();
        }
     
     }
}
