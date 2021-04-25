using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_test_2
{
    public class Rule
    {
        private string mac_src;
        private string mac_dst;
        private string ip_src;
        private string ip_dst;
        private string port;
        private string filter;
        //private int num;
        private string in_out;
        private string except;
        public void set_filters(string m_d,string m_s,string ip_src,string ip_dst,string p,string f,string io,string e)
        {
            mac_src = m_s;
            mac_dst = m_d;
            this.ip_src = ip_src;
            this.ip_dst = ip_dst;
            port = p;
            filter = f;
            //num = n;
            in_out = io;
            except = e;
        }
        public string get_mac_src()
        {
            return mac_src;
        }
        public string get_mac_dst()
        {
            return mac_dst;
        }

        public string get_port()
        {
            return port;
        }
        public string get_filter()
        {
            return filter;
        }
        /*
        public int get_num()
        {
            return num;
        }
        */
        public string get_ip_src()
        {
            return ip_src;
        }
        public string get_ip_dst()
        {
            return ip_dst;
        }
        public string get_io()
        {
            return in_out;
        }
        public string get_excp()
        {
            return except;
        }
    }
}
