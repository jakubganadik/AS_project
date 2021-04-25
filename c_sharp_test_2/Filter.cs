using System;
using System.Collections.Generic;
using System.Text;
using PcapDotNet.Packets;
namespace c_sharp_test_2
{
    class Filter
    {
        private Packet pck;
        private string rules;
        
        public void set_rules(string r)
        {
            this.rules = r;
        }
        public Packet run_filter(Packet p)
        {
            this.pck = p;

            return pck;
        }
    }
}
