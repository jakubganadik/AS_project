using System;
using System.Collections.Generic;
using System.Text;
using PcapDotNet.Packets;
namespace c_sharp_test_2
{
    class Captured_packet
    {
        private Packet packet;
        private Boolean port_out;
        public void set_packet(Packet p,Boolean p_o)
        {
            this.packet = p;
            this.port_out = p_o;
        }
        public Packet get_packet()
        {
            
            return this.packet;
        }
        public Boolean get_port_out()
        {

            return this.port_out;
        }
    }
}
