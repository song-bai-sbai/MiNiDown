using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiNiDown
{
    class ShowData
    {
        public MyDownLoad m;
        public ShowData(MyDownLoad m1)
        {
            this.m = m1;
        }
        public string ShowName()
        {
            return m.true_filename;
        }
        public string ShowSpeed()
        {
            return m.DownLoadSpeed.ToString("f3")+"kb/s";
        }
        public string ShowLeftTime()
        {
            string second="0";
            string minite = "0";
            string hour = "0";
            string final = "0小时" + "0分" + "0秒";
            if (m.LeftDownLoadTime>0)
            {
                if (m.LeftDownLoadTime<60)
                {
                    final = "0小时"  + "0分" + m.LeftDownLoadTime.ToString("f2") + "秒";
                }
                if (m.LeftDownLoadTime>60&&m.LeftDownLoadTime<3600)
                {
                    second = ((int)(m.LeftDownLoadTime % 60)).ToString();
                    minite = ((int)((m.LeftDownLoadTime / 60) % 60)).ToString();
                    final = "0小时" + minite + "分" + second + "秒";
                }
                if (m.LeftDownLoadTime > 60 && m.LeftDownLoadTime < 3600)
                {
                    second = ((int)(m.LeftDownLoadTime % 60)).ToString();
                    minite = ((int)((m.LeftDownLoadTime / 60) % 60)).ToString();
                    final = "0小时" + minite + "分" + second + "秒";
                }
                if (m.LeftDownLoadTime > 3600)
                {
                    second = ((int)(m.LeftDownLoadTime % 60)).ToString();
                    minite = ((int)((m.LeftDownLoadTime / 60) % 60)).ToString();
                    hour = ((int)((m.LeftDownLoadTime / 60 / 60) % 24)).ToString();
                    final = hour + "小时" + minite + "分" + second + "秒";
                }
                
            }
            else
            {
                final =  "0小时" +  "0分" + "0秒";
            }
            return final;
        }
        public double ShowPercentage()
        {
            return m.DownLoadPercentage*100;
        }
        
    }
}
