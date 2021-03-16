using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_test_2
{
    public class Cam_table
    {
        private string MAC;
        private string port;
        private int timer;
        public void set_cam(string m, string p, int t)
        {
            this.MAC = m;
            this.port = p;
            this.timer = t;
        }
        public void set_timer(int t)
        {
            this.timer = t;
        }
        public void set_port(string p)
        {
            this.port = p;
        }
        public string get_mac()
        {
            return this.MAC;
        }
        public string get_port()
        {
            return this.port;
        }
        public int get_timer()
        {
            return this.timer;
        }
        

       
    }
}
