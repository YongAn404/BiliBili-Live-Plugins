using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


//关于Live.dll C#的使用说明

//由于C#是运行于.Net架构之上的，因此编写的DLL不是Win32标准的DLL。所以不能被互动姬直接调用
//因此，需要将C#编写的DLL进行进一步的类库编译，将接口编译成标准的Win32 DLL，

//提示：.Net的进一步编译工具：[Net类库编译器] 已放在同一目录
//方法：
//1、工程中添加 MethodProperty.cs
//2、将需要公开的函数添加属性：[MethodProperty(Export = true)]  (详情请看第27行)
//3、运行[.Net类库编译器]进行进一步编译
//4、将重编译后的dll放入互动姬



namespace Live
{
    public class LiveSystem
    {
        /// <summary>
        /// 抽奖数据
        /// </summary>
        public static LuckDrawData luckDrawData = new LuckDrawData();
        /// <summary>
        /// 参加数据
        /// </summary>
        public static List<PartakeData> partakeDatas = new List<PartakeData>();

        //抽奖计时窗口是否被使用
        public static bool LuckDrawWndOccuy = false;
        public static bool LuckDrawEnd = true;

        [MethodProperty(Export = true)]//在公开静态的函数添加此属性 以便被[.Net类库编译器]识别
        public static void initialize()
        {
            try
            {
                //打开LiveCMD
                CMDWnd form1 = new CMDWnd();
                form1.Show();

                //初始化计时
                TimerSvc.Instance.Init();

                //用于Update调用
                Timer timer = new Timer();
                timer.Elapsed += new ElapsedEventHandler(Update);
                timer.AutoReset = true;
                timer.Start();

                CMDWnd.Log("已成功启动");

            }
            catch (Exception e)
            {
                CMDWnd.Log(e.ToString(), 2);
            }
        }
        public static void Update(object o,ElapsedEventArgs e)
        {
            TimerSvc.Instance.Update();
        }

        [MethodProperty(Export = true)]
        public static void LuckDraw(string BilibiliUID, string BilibiliName, string MedalName, string Medallevel)
        {
            if (!LuckDrawEnd)
            {
                if (!luckDrawData.yes)
                {
                    //CMDWnd.Log("未确定抽奖",1);
                    return;
                }
                foreach (var item in partakeDatas)
                {
                    if (item.BilibiliUID == BilibiliUID)
                    {
                        return;
                    }
                }
                partakeDatas.Add(new PartakeData()
                {
                     BilibiliUID = BilibiliUID,
                     BilibiliName = BilibiliName,
                });
            }
        }

        /// <summary>
        /// 己弃用
        /// </summary>
        [MethodProperty(Export = true)]
        public static void AddLuckDraw(string BilibiliUID, string BilibiliName, string MedalName, string Medallevel, string Parameter)
        {
            if (luckDrawData == null)
            {
                string[] vs = Parameter.Split(',');

            }
        }
        /// <summary>
        /// 己弃用
        /// </summary>
        [MethodProperty(Export = true)]
        public static void Test(string BilibiliUID, string BilibiliName, string MedalName, string Medallevel)
        {
            //try
            //{
            //    CMDWnd.Log("己执行测试");
            //    CMDWnd.Log("用户ID:" + BilibiliUID);
            //    CMDWnd.Log("用户名称:" + BilibiliName);
            //    CMDWnd.Log("粉丝牌名称:" + MedalName);
            //    CMDWnd.Log("粉丝牌等级:" + Medallevel);

            //    TimerSvc.Instance.AddTimeTask((int uid1) =>
            //    {
            //        CMDWnd.Log("己10秒了");

            //    }, 10, PETimeUnit.Second);
            //}
            //catch (Exception e)
            //{
            //    CMDWnd.Log(e.ToString(), 2);
            //}
        }

    }

    /// <summary>
    /// 抽奖数据
    /// </summary>
    public class LuckDrawData
    {
        /// <summary>
        /// 奖品内容
        /// </summary>
        public string content;
        /// <summary>
        /// 时间
        /// </summary>
        public int time;
        /// <summary>
        /// 奖品数量
        /// </summary>
        public int num;
        /// <summary>
        /// 抽奖确定
        /// </summary>
        public bool yes = false;
    };

    /// <summary>
    /// 参加数据
    /// </summary>
    public class PartakeData
    {
        /// <summary>
        /// Bilibili ID
        /// </summary>
        public string BilibiliUID;
        /// <summary>
        /// Bilibili 用户
        /// </summary>
        public string BilibiliName;
    }
} 
