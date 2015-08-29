using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MiNiDown
{
    class downloadcontrol
    {

        public MyDownLoad down;
        double past_down;
        double now_down;
        bool pastornow=false;
        public downloadcontrol(MyDownLoad down1)
        {
            this.down = down1;
            pastornow = false;
        }
        public void set()
        {
            while (true)
            {
                if (pastornow==false)
               {
                  past_down = down.DownLoadtotal;
                  pastornow = true;
                  Thread.Sleep(1000);
               } 
               else
              {
                  now_down = down.DownLoadtotal;
                  down.DownLoadSpeed = (now_down - past_down) / 1024 / 2;// speed 
                  down.DownLoadPercentage = down.DownLoadtotal / down.DownLoadsize;//percentage
                  down.LeftDownLoadTime = (down.DownLoadsize - down.DownLoadtotal) / (down.DownLoadSpeed * 1024);
                  
                  past_down = now_down;
                  if (down.DownLoadPercentage > 1)
                  {
                      down.DownLoadPercentage = 1;
                  }
                  if (down.LeftDownLoadTime<0)
                  {
                      down.LeftDownLoadTime = 0;
                  }
                  
                  Thread.Sleep(1000);
                  if (down.DownLoadsize == down.DownLoadtotal || down.DownLoadsize > down.DownLoadtotal)
                  {
                      down.DownLoadSpeed = 0;
                      down.DownLoadPercentage = 1;
                     
                  }
                   
              }
            }   
        }
    }
}
