using System;
using System.Collections.Generic;
using System.Text;
using PcapDotNet.Packets;
using System.Collections.Concurrent;
namespace c_sharp_test_2
{
    
    class Table_update//run thread to update
    {
        private string name;

        private Packet packet;
        private BlockingCollection<Packet> packet_buf;//check if LIFO behavior-------------------------------------------
        private Form1 myform;
        public void set_table(string name,Form1 f, BlockingCollection<Packet> p)
        {
            this.name = name;
            
            
            
            this.myform = f;
            this.packet_buf = p;
        }
        public void update()// new delegate
        {
            
            
            while (true)
            {
                packet = packet_buf.Take();
                int[] num_packets = new int[14]; //test
                if (name == "one")
                {
                    num_packets = Packet_counter.load_values;
                }
                else if (name == "two")
                {
                    num_packets = Packet_counter.load_values_2;
                }
                //update form for cam

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

                }
                else if (name == "two")
                {
                    Packet_counter.load_values_2 = num_packets;//tuto test
                    myform.Invoke(myform.myDelegate_2);

                }

            }
            
        }
    }
}
