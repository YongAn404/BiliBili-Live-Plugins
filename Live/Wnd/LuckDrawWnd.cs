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
    public partial class LuckDrawWnd : Form
    {
        public static LuckDrawWnd liveform;
        public LuckDrawWnd()
        {
            liveform = this;
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        private int time;
        public void Timer(int tid = 0)
        {
            if (liveform.button1.Visible)
            {
                liveform.button1.Visible = false;
                return;
            }
            if (time > 1)
            {
                time--;
                liveform.label5.Text = time.ToString();
            }
            else
            {
                LiveSystem.LuckDrawEnd = true;
                liveform.button1.Visible = true;
            }
        }
        public void Page(string content, int time, int num)
        {
            liveform.label1.Text = string.Format("奖品:{0} 奖品数量:{1}", content, num);
            liveform.label2.Text = string.Format("参加条件:{0}", "无");
            liveform.label3.Text = string.Format("参加弹幕:{0}", "参加抽奖");
            liveform.label5.Text = time.ToString();
            this.time = time;


            TimerSvc.Instance.AddTimeTask(Timer, 1, PETimeUnit.Second, time);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            WinWnd form2 = new WinWnd();
            form2.Show();

            liveform.Hide();

            List<PartakeData> partakeDatas = LiveSystem.partakeDatas;
            if (partakeDatas.Count == 0)
            {
                CMDWnd.Log(string.Format("本次抽奖无人参加"));
            }

            for (int i = 0; i < LiveSystem.luckDrawData.num; i++)
            {
                int num1 = 0;
                Random random = new Random(num1);

                if (partakeDatas.Count == 0)
                {
                    CMDWnd.Log("抽奖人数不足，奖品未能全部分发", 1);
                    LiveSystem.LuckDrawWndOccuy = false;
                    return;
                }
                else
                {
                    if (num1 == 0)
                    {
                        random = new Random();
                    }
                    num1 = random.Next(partakeDatas.Count);

                    WinWnd.liveform.WinningList(partakeDatas[num1]);
                    partakeDatas.Remove(partakeDatas[num1]);
                }

                LiveSystem.LuckDrawWndOccuy = false;
            }
        }
    }
}
