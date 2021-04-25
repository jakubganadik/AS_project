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
        private string in_out;
        private string p_type;
        public void set_packet(Packet p,string io,string t)
        {
            this.packet = p;
            in_out = io;
            p_type = t;
        }
        public Packet get_packet()
        {
            
            return this.packet;
        }
        public string get_io()
        {

            return in_out;
        }
        public string get_packet_type()
        {

            return p_type;
        }
    }
}
