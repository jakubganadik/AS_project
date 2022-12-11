using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_test_2
{
    public class Rule
    {
        //private string mac_src;
        //private string mac_dst;
        private string ip_src;
        private string ip_dst;
        private string port;
        private string filter;
        //private int num;
        private string in_out;
        private string except;
        
        public string SourceMac { get; set; }
        public string DestinationMac { get; set; }
        public string Port { get; set; }
        public string Filter { get; set; }
        public string SourceIP { get; set; }
        public string DestinationeIP { get; set; }
        public string InOutRule { get; set; }
        public string ExceptRule { get; set; }
       
    }
}
