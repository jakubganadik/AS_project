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
        public void set_cam(string m, string p)
        {
            this.MAC = m;
            this.port = p;
            Stopwatch stopwatch = new Stopwatch();
            this.stopwatch = stopwatch;
            // Begin timing.
            this.stopwatch.Start();
        }
        public void set_timer()
        {
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
        public string get_timer()
        {
            if (stopwatch.Elapsed.ToString() == null)
            {
                return "";
            }
            else
            {
                return this.stopwatch.Elapsed.ToString();
            }
            
        }
        

       
    }
}
