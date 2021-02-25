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
        private PacketDevice selectedDevice;
        private string name;
        Packet_handler h;
        private Form1 myform;
        public void list_get(PacketDevice p, string name, Packet_handler h, Form1 myform)
        {

            selectedDevice = p;
            this.name = name;
            this.h = h;
            this.myform = myform;
        }
        public void recv()
        {

            //ph.get_device_send(allDevices[deviceIndex_2 - 1]);
            int[] num_packets = new int[10];
            
            Thread.Sleep(10);

            for (int i = 0; i < 10; i++)//tu povodne bolo len pre jeden delegate
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








            using (PacketCommunicator communicator_lis =
                selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                            // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous | PacketDeviceOpenAttributes.NoCaptureLocal, // promiscuous mode
                                    1000))                                  // read timeout
            {
                // Check the link layer. We support only Ethernet for simplicity.
                if (communicator_lis.DataLink.Kind != DataLinkKind.Ethernet)
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
                Console.WriteLine("Listening on " + selectedDevice.Description + "...");
                Packet packet;
                do
                {
                    PacketCommunicatorReceiveResult result = communicator_lis.ReceivePacket(out packet);
                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:
                            // Timeout elapsed
                            continue;
                        case PacketCommunicatorReceiveResult.Ok:
                            Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length + "thread name" + name + " type " + packet.Ethernet.EtherType);
                            if (packet.Ethernet.EtherType.ToString() == "IpV4")
                            {
                                Console.WriteLine(packet.Ethernet.IpV4.Protocol);
                                if (packet.Ethernet.IpV4.Protocol.ToString() == "Tcp")
                                {
                                    if (packet.Ethernet.IpV4.Tcp.SourcePort == 80 || packet.Ethernet.IpV4.Tcp.DestinationPort == 80)
                                    {
                                        num_packets[4]++;
                                        num_packets[9]++;
                                    }
                                    else
                                    {
                                        num_packets[0]++;
                                        num_packets[5]++;
                                    }



                                }
                                else if (packet.Ethernet.IpV4.Protocol.ToString() == "Udp")
                                {
                                    num_packets[1]++;
                                    num_packets[6]++;
                                }
                                else if (packet.Ethernet.IpV4.Protocol.ToString() == "InternetControlMessageProtocol")
                                {
                                    num_packets[2]++;
                                    num_packets[7]++;
                                }

                            }
                            else if (packet.Ethernet.EtherType.ToString() == "Arp")
                            {
                                num_packets[3]++;
                                num_packets[8]++;
                            }
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
}
