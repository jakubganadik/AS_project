using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Transport;
using System;
using System.Threading;
namespace c_sharp_test_2
{
    class Listener
    {
        private PacketCommunicator pack_comm;
        private string name;
        Packet_handler h;
        private Form1 myform;
        private string mac_dest;
        private List<Cam_table> tbl;
        private bool has_src;
        private bool has_dst;
        
        public void list_get(PacketCommunicator pack_comm, string name, Packet_handler h, Form1 myform)
        {

            this.pack_comm = pack_comm;
            this.name = name;
            this.h = h;
            this.myform = myform;
        }
        public void recv()
        {

            List<Cam_table> cam_vals = new List<Cam_table>();
            //Packet_counter.cam_values = cam_vals;
            //ph.get_device_send(allDevices[deviceIndex_2 - 1]);
            int[] num_packets = new int[14];
            
            Thread.Sleep(10);

            for (int i = 0; i < 14; i++)//tu povodne bolo len pre jeden delegate
            {
                num_packets[i] = 0;
            }
            if (name == "one")
            {
                Packet_counter.load_values = num_packets;//tuto test
                myform.Invoke(myform.myDelegate);
            }
            else if (name == "two")
            {
                Packet_counter.load_values_2 = num_packets;//tuto test
                myform.Invoke(myform.myDelegate_2);
            }

            // Check the link layer. We support only Ethernet for simplicity.
            if (pack_comm.DataLink.Kind != DataLinkKind.Ethernet)
            {
                Console.WriteLine("This program works only on Ethernet networks.");
                return;
            }

            // Compile the filter
            /*
            using (BerkeleyPacketFilter filter = communicator.CreateFilter("ip and udp"))
            {
                // Set the filter
                communicator.SetFilter(filter);
            }
            */
            //zisti ci sa using opakuje

            //nedari sa zachytit premavku, otestovat, ci je spravny adapter cez gns a wireshark
            
            Packet packet;
            do
            {
                PacketCommunicatorReceiveResult result = pack_comm.ReceivePacket(out packet);
                switch (result)
                {
                    case PacketCommunicatorReceiveResult.Timeout:
                        // Timeout elapsed
                        continue;
                    case PacketCommunicatorReceiveResult.Ok:
                        Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length + "thread name" + name + " type " + packet.Ethernet.EtherType);
                        //new_mac = 0;
                        //-------------------------------------------------------------------------------------------
                        if (packet.Ethernet.Source.ToString()[1] == '0' || packet.Ethernet.Source.ToString()[1] == '4' || packet.Ethernet.Source.ToString()[1] == '8' || packet.Ethernet.Source.ToString()[1] == 'C') //kontrola zariadenia
                        {
                            has_dst = false;
                            has_src = false;
                            tbl =Packet_counter.cam_values;
                            foreach (Cam_table t in tbl)
                            {
                                if (t.get_mac() == packet.Ethernet.Source.ToString())
                                {
                                    t.set_timer(0);
                                    has_src = true;
                                }
                                if (t.get_mac() == packet.Ethernet.Destination.ToString())
                                {
                                    mac_dest = t.get_mac();
                                    has_dst = true;
                                }
                                
                            }
                            if (has_src == false)
                            {
                                Cam_table c = new Cam_table();
                                c.set_cam(packet.Ethernet.Destination.ToString(),name,0);//solve timer counting--------------------------------------------
                                tbl.Add(c);
                                Packet_counter.cam_values =tbl;//zaznam v cam tab
                                
                            }
                            if (has_dst == false)
                            {

                                //send everywhere
                            }
                            
                            
                        }
                        //iny thread a funkcia
                        //---------------------------------------------------------------------------------------------
                        if (packet.DataLink.Kind.ToString() == "Ethernet") // toto spojit
                        {
                            num_packets[6]++;
                            num_packets[13]++;
                            if (packet.Ethernet.EtherType.ToString() == "IpV4")
                            {
                                num_packets[5]++;
                                num_packets[12]++;
                                Console.WriteLine(packet.Ethernet.IpV4.Protocol);
                                if (packet.Ethernet.IpV4.Protocol.ToString() == "Tcp")
                                {
                                    if (packet.Ethernet.IpV4.Tcp.SourcePort == 80 || packet.Ethernet.IpV4.Tcp.DestinationPort == 80)
                                    {
                                        num_packets[4]++;
                                        num_packets[11]++;
                                    }
                                    else
                                    {
                                        num_packets[0]++;
                                        num_packets[7]++;
                                    }



                                }
                                else if (packet.Ethernet.IpV4.Protocol.ToString() == "Udp")
                                {
                                    num_packets[1]++;
                                    num_packets[8]++;
                                }
                                else if (packet.Ethernet.IpV4.Protocol.ToString() == "InternetControlMessageProtocol")
                                {
                                    num_packets[2]++;
                                    num_packets[9]++;
                                }

                            }
                            else if (packet.Ethernet.EtherType.ToString() == "Arp")
                            {
                                num_packets[3]++;
                                num_packets[10]++;
                            }

                        }
                        //new threads here
                        if (name == "one")
                        {
                            Packet_counter.load_values = num_packets;//tuto test
                            myform.Invoke(myform.myDelegate);
                            h.run_handler(packet);
                        }
                        else if (name == "two")
                        {
                            Packet_counter.load_values_2 = num_packets;//tuto test
                            myform.Invoke(myform.myDelegate_2);
                            h.run_handler(packet);
                        }






                        break;
                    default:
                        throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                }




            } while (true);
            // start the capture
            // communicator_lis.ReceivePackets(0, PacketHandler);//communicator_lis.ReceivePackets(0, PacketHandler)









        }
    }
}
