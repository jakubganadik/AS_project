using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
namespace c_sharp_test_2
{
    public class Cam_table
    {
        private string MAC;
        private string port;
        private int timer;
        private Stopwatch stopwatch;
        private int cur_time;
        private int cur_time_h;
        private int cur_time_m;
        private int cur_time_s;
        private int max_time;
        private string time_passed;
        public void set_cam(string m, string p,int ma)
        {
            this.max_time = ma;
            this.MAC = m;
            this.port = p;
            Stopwatch stopwatch = new Stopwatch();
            this.stopwatch = stopwatch;
            // Begin timing.
            this.stopwatch.Start();
        }
        public void set_timer(int ma)
        {
            this.max_time = ma;
            stopwatch.Restart();
            //this.stopwatch.Stop();
            //this.stopwatch.Start();
        }
        public void set_port(string p)
        {
            this.port = p;
        }
        public string get_mac()
        {
            if (this.MAC == null)
            {
                return "";
            }
            else
            {
                return this.MAC;
            }
            
        }
        public string get_port()
        {
            if (this.port == null)
            {
                return "";
            }
            else
            {
                return this.port;
            }
            
        }
        public int get_timer()
        {
            
            //max_time = Packet_counter.val_for_timer;
            if (stopwatch.Elapsed.ToString() == null)
            {
                return 0;
            }
            else
            {//uprava casu
                time_passed = stopwatch.Elapsed.ToString();
                cur_time_h = 3600 * Int32.Parse(time_passed.Substring(0, 2));
                cur_time_m = 60*Int32.Parse(time_passed.Substring(3, 2));
                cur_time_s = Int32.Parse(time_passed.Substring(6, 2));
                cur_time = max_time - cur_time_h - cur_time_m - cur_time_s ;// sa odcita na mieste
                if (cur_time < 0)//warn when reaches zero
                {
                    cur_time = -1;
                }
                return cur_time;
            }
            
        }
        

       
    }
}
