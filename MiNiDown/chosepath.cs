using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiNiDown
{
    class chosepath
    {
        public string SaveFilePath()
        {
            string str;
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowDialog();
            str = fb.SelectedPath;
            return str;
        }

    }
}
