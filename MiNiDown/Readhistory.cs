using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiNiDown
{
    class Readhistory
    {
        public string readhistroy()
        {
            int number = 0;
            FileStream MyFileStream;
            string his = "";
            if (!File.Exists("MiniDownHis.txt"))
            {
                his = "empty";
                return his;
            }
            else
            {
                MyFileStream = File.OpenRead("MiniDownHis.txt");
                StreamReader sr = new StreamReader(MyFileStream);
                number = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < number; i++)
                {
                    his += "任务名: ";
                    his += (sr.ReadLine() + "  ");
                    his += "路径: "+"\n";
                    his += (sr.ReadLine() + "\n");
                    sr.ReadLine();
                    his += "-----------------------\n";
                }
                sr.Close();
                return his;
            }
        }
    }
}
