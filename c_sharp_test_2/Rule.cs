using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_test_2
{
    public class Rule
    {
        public string SourceMac { get; set; }
        public string DestinationMac { get; set; }
        public string Port { get; set; }
        public string Filter { get; set; }
        public string SourceIp { get; set; }
        public string DestinationeIp { get; set; }
        public string InOutRule { get; set; }
        public string ExceptRule { get; set; }
       
    }
}
