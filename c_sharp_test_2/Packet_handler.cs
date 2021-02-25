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
        private PacketDevice selectedDevice_send;
        private string name;
        public void get_device_send(PacketDevice p, string name)
        {
            selectedDevice_send = p;
            this.name = name;
        }
        public void run_handler(Packet packet)
        {
            // print timestamp and length of the packet
            Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + "it got here" + packet.Length + "got here" + name);
            //testy s tymito vecami
            //ArpDatagram arp = packet.Ethernet.Arp;
            //IpV4Datagram ip = packet.Ethernet.IpV4;
            //UdpDatagram udp = ip.Udp;


            using (PacketCommunicator communicator_send = selectedDevice_send.Open(100, // name of the device
                                                                         PacketDeviceOpenAttributes.Promiscuous | PacketDeviceOpenAttributes.NoCaptureLocal, // promiscuous mode
                                                                         1000)) // read timeout
            {
                communicator_send.SendPacket(packet);
                Console.WriteLine("packet send");
            }

            /*
            if (packet != null)
            {
                if (packet.Ethernet != null)
                {
                    if (packet.Ethernet.Arp != null)
                    {
                        Console.WriteLine("here");
                        ArpDatagram arp = packet.Ethernet.Arp;
                        Console.WriteLine(arp.SenderHardwareAddress + "send" + arp.SenderProtocolAddress + "rec");
                    }
                }
            }
            */
            // print ip addresses and udp ports

            //Console.WriteLine(arp.SenderHardwareAddress + "send" + arp.SenderProtocolAddress+"rec");
            //Console.WriteLine(ip.Source + ":" + udp.SourcePort + " -> " + ip.Destination + ":" + udp.DestinationPort);
        }

    }
}
