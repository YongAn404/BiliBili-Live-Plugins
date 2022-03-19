using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live
{
    public class Instructions
    {
        private static Instructions instance = null;
        public static Instructions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Instructions();
                }
                return instance;
            }
        }

        /// <summary>
        /// SetLuckDraw/SLD指令
        /// </summary>
        /// <param name="vs">输入</param>
        public void SetLuckDraw(string[] vs)
        {
            int time = 0;
            int num = 0;
            if (LiveSystem.LuckDrawWndOccuy == false)
            {
                if (vs.Length != 1 && int.TryParse(vs[1], out time))
                {
                    LuckDrawData luckDrawData = new LuckDrawData();

                    luckDrawData.time = time * 60;
                    if (vs.Length != 2)
                    {
                        if (vs.Length == 3)
                        {
                            luckDrawData.content = vs[2];
                            luckDrawData.num = 1;
                            LiveSystem.luckDrawData = luckDrawData;
                            CMDWnd.Log("已成功创建抽奖，确认无误后请输入SetLuckDraw/SLD OK");
                            CMDWnd.Log(string.Format("奖品:{0} 奖品数量:{1} 参加条件:{2} 参加弹幕:{3} 开奖时间:{4}秒", LiveSystem.luckDrawData.content, LiveSystem.luckDrawData.num, "无", "参加抽奖", LiveSystem.luckDrawData.time));
                            return;
                        }

                        if (vs.Length != 3 && int.TryParse(vs[3], out num) && num >= 0)
                        {
                            if (vs.Length > 4)
                            {
                                CMDWnd.Log(string.Format("指令错误出现在 SetLuckDraw {0} {1} {2} <参数过多>", time, vs[2], num), 1);
                                return;
                            }
                            luckDrawData.content = vs[2];
                            luckDrawData.num = num;
                            LiveSystem.luckDrawData = luckDrawData;
                            CMDWnd.Log("已成功创建抽奖，确认无误后请输入SetLuckDraw/SLD OK");
                            CMDWnd.Log(string.Format("奖品:{0} 奖品数量:{1} 参加条件:{2} 参加弹幕:{3} 开奖时间:{4}秒", LiveSystem.luckDrawData.content, LiveSystem.luckDrawData.num, "无", "参加抽奖", LiveSystem.luckDrawData.time));
                        }
                        else
                        {
                            CMDWnd.Log(string.Format("指令错误出现在 SetLuckDraw {0} {1} <错误>", time, vs[2]), 1);
                        }
                    }
                    else
                    {
                        CMDWnd.Log(string.Format("指令错误出现在 SetLuckDraw {0} <错误>", time), 1);
                    }
                }
                else
                {
                    CMDWnd.Log("指令错误出现在 SetLuckDraw <错误> ", 1);
                }
            }
            else
            {
                CMDWnd.Log("请等待上一个抽奖完成", 1);
            }
        }

        /// <summary>
        /// SetLuckDraw OK/SLD OK指令
        /// </summary>
        /// <param name="vs">输入</param>
        public bool SetLuckDrawOK(string[] vs)
        {
            bool result = true;

            if (vs.Length != 1)
            {
                if (vs[1] == "OK" && LiveSystem.luckDrawData.time >= 1)
                {
                    if (LiveSystem.LuckDrawWndOccuy == false)
                    {
                        LuckDrawWnd form3 = new LuckDrawWnd();
                        LiveSystem.LuckDrawWndOccuy = true;
                        LiveSystem.luckDrawData.yes = true;
                        form3.Page(LiveSystem.luckDrawData.content, LiveSystem.luckDrawData.time, LiveSystem.luckDrawData.num);
                        form3.Show();
                        form3.Timer();
                        LiveSystem.LuckDrawEnd = false;
                    }
                    else
                    {
                        CMDWnd.Log("请等待上一个抽奖完成", 1);
                    }
                    result = false;
                }
                else if (vs[1] == "OK" && LiveSystem.luckDrawData.time <= 0)
                {
                    CMDWnd.Log("未设置抽奖", 1);
                    result = false;
                }
            }

            return result;
        }

    }
}
