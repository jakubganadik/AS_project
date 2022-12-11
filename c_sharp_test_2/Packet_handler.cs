using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Transport;
namespace c_sharp_test_2
{
    class Packet_handler
    {
        private PacketCommunicator pack_comm;
        private string _name;
        public void get_device_send(PacketCommunicator pack_comm, string name)
        {
            this.pack_comm = pack_comm;
            _name = name;
        }

        string Name
        {
            get => _name;
        }

        public void run_handler(Packet packet)
        {
            // print timestamp and length of the packet
            Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + "it got here" + packet.Length + "got here" + Name);




            pack_comm.SendPacket(packet);
            Console.WriteLine("packet send");
            

        }

    }
}
