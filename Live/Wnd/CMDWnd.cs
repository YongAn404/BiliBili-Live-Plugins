using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Live
{
    public partial class CMDWnd : Form
    {
        public static CMDWnd liveform;
        public CMDWnd()
        {
            liveform = this;
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="LV">内容</param>
        public static void Log(string msg, int LV = 0)
        {
            switch (LV)
            {
                case 0:
                    liveform.listBox1.Items.Add(string.Format("#{0} ThreadID:{1} Log>>{2}", DateTime.Now.ToString("hh:mm:ss-fff"), Thread.CurrentThread.ManagedThreadId,msg));
                    break;
                case 1:
                    liveform.listBox1.Items.Add(string.Format("#{0} ThreadID:{1} Warning>>{2}", DateTime.Now.ToString("hh:mm:ss-fff"), Thread.CurrentThread.ManagedThreadId, msg));
                    break;
                case 2:
                    liveform.listBox1.Items.Add(string.Format("#{0} ThreadID:{1} ERROR>>{2}", DateTime.Now.ToString("hh:mm:ss-fff"), Thread.CurrentThread.ManagedThreadId, msg));
                    foreach (var item in TraceInfo())
                    {
                        liveform.listBox1.Items.Add(string.Format("TraceInfo:{0}", item));
                    }
                    break;
            }

        }

        private static List<string> TraceInfo()
        {
            StackTrace st = new StackTrace(3, true);//跳跃3帧
            List<string> traceInfo = new List<string>();
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                traceInfo.Add(string.Format("{0}::{1} Line:{2}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber()));
            }
            return traceInfo;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            liveform.listBox1.Items.Add("CMD:" +liveform.textBox1.Text);

            string[] vs = liveform.textBox1.Text.Split(' ');

            switch (vs[0])
            {
                case "help":
                    Log(" <必须>  [可选] (参数类型)", 0);
                    Log("SetLuckDraw或SLD <时间/分钟(int)> <奖品(string)> [奖品数量(int)]", 0);
                    Log("SetLuckDraw或SLD OK 确定数据无误并开始抽奖", 0);
                    break;
                case "SetLuckDraw":
                    if (Instructions.Instance.SetLuckDrawOK(vs))
                    {
                        Instructions.Instance.SetLuckDraw(vs);
                    }
                    break;
                case "SLD":
                    if (Instructions.Instance.SetLuckDrawOK(vs))
                    {
                        Instructions.Instance.SetLuckDraw(vs);
                    }

                    break;
                default:
                    Log("无效指令，请输入help查询指令集", 1);
                    break;
            }
    }
        /// <summary>
        /// 调整颜色
        /// </summary>
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

            Brush FontBrush = Brushes.Black;
            ListBox listBox = sender as ListBox;
            if (e.Index > -1)
            {
                string LV = listBox.Items[e.Index].ToString();
                if (LV.Contains("Log>>"))
                {
                    FontBrush = Brushes.Black;
                }
                if (LV.Contains("Warning>>"))
                {
                    FontBrush = Brushes.Orange;
                }
                if (LV.Contains("ERROR>>") || LV.Contains("TraceInfo:"))
                {
                    FontBrush = Brushes.Red;
                }

                e.DrawBackground();
                e.Graphics.DrawString(listBox.Items[e.Index].ToString(), e.Font, FontBrush, e.Bounds);
                e.DrawFocusRectangle();
            }
        }
    }
}
