//使用教程连接：https://blog.csdn.net/li3781695/article/details/86348960
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.IO;
namespace usTimer
{
    //使用windows.h中的函数，加载DLL来使用
    public static class QueryPerformanceMethd
    {
        [DllImport("kernel32.dll")]
        public extern static short QueryPerformanceCounter(ref long x);

        [DllImport("kernel32.dll")]
        public extern static short QueryPerformanceFrequency(ref long x);
    }
    public class usTimerEventArgs : EventArgs
    {
        public double Timenow = 0;
        public usTimerEventArgs(double inputTime)
        {
            Timenow = inputTime;
        }
    }
    public delegate void ElapsedEvent(object Sender, usTimerEventArgs e);
    class usTimer
    {
        long stop_Value = 0;
        long start_Value = 0;
        long freq = 0;
        double time = 0;
        bool stop = true;
        //计时器是否有效
        public bool Enabled { get; set; }
        public event ElapsedEvent Elapsed;
        public void Start()
        {
            if (stop == false) return;
            stop = false;
            Thread Mythread = new Thread(ReadTime);
            Mythread.Start();
        }
        public void Stop()
        {
            stop = true;
        }
        private void ReadTime()
        {
            QueryPerformanceMethd.QueryPerformanceFrequency(ref freq);
            QueryPerformanceMethd.QueryPerformanceCounter(ref start_Value);
            while (true)
            {
                if (stop == true) break;
                QueryPerformanceMethd.QueryPerformanceCounter(ref stop_Value);
                if (Enabled == true)
                {
                    time = ((double)stop_Value - (double)start_Value) / (double)freq * 1000000;//转化为us
                    Elapsed(this, new usTimerEventArgs(time));//触发事件
                }
            }
        }
    }
}
