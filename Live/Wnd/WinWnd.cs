using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Live
{
    public partial class WinWnd : Form
    {
        public static WinWnd liveform;
        public WinWnd()
        {
            liveform = this;
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public void WinningList(PartakeData data)
        {
            liveform.listBox1.Items.Add(string.Format("恭喜B站用户:{0} UID:{1} 抽到{2}", data.BilibiliName , data.BilibiliUID,LiveSystem.luckDrawData.content));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
