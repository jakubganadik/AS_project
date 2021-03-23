using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Transport;
using System;
using System.Threading;
using System.Collections.Concurrent;
namespace c_sharp_test_2
{
    class Listener
    {
        private PacketCommunicator pack_comm;
        private string name;
        private string name_loop;
        Packet_handler h;
        Packet_handler h_loop;
        private Form1 myform;
        private string mac_dest;
        private BlockingCollection<Cam_table> tbl;
        
        private bool has_src;
        private bool has_dst;
        private bool not_a_pc;
        private string port_name;
        public void list_get(PacketCommunicator pack_comm, string name, Packet_handler h, Form1 myform,string n_l,Packet_handler h_l)
        {

            this.pack_comm = pack_comm;
            this.name = name;
            this.h = h;
            this.myform = myform;
            this.name_loop = n_l;
            this.h_loop = h_l;
        }
        public void recv()
        {

            BlockingCollection<Cam_table> cam_vals = new BlockingCollection<Cam_table>();
            BlockingCollection<Packet> packet_buff = new BlockingCollection<Packet>();
            Packet_counter.cam_values = cam_vals;
            Table_update t_up = new Table_update(); 
            //Packet_counter.cam_values = cam_vals;
            //ph.get_device_send(allDevices[deviceIndex_2 - 1]);
            int[] num_packets = new int[14];
            myform.Invoke(myform.myDelegate_3);//--------------------------------------------new delegate
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
            t_up.set_table(name, myform, packet_buff);
            ThreadStart send = new ThreadStart(t_up.update);

            Thread childThread_3 = new Thread(send);
            childThread_3.Start();

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
                        //mozno samost
                        //-------------------------------------------------------------------------------------------
                        mac_dest = "";
                        not_a_pc = true;
                        if (packet.Ethernet.Source.ToString()[1] == '0' || packet.Ethernet.Source.ToString()[1] == '4' || packet.Ethernet.Source.ToString()[1] == '8' || packet.Ethernet.Source.ToString()[1] == 'C') //kontrola zariadenia
                        {
                            not_a_pc = false;
                            has_src = false;
                            
                            tbl =Packet_counter.cam_values;
                            //if empty
                            if (tbl == null)
                            {
                                Cam_table c = new Cam_table();
                                
                                c.set_cam(packet.Ethernet.Source.ToString(), name);
                                
                                
                                tbl.Add(c);
                                Packet_counter.cam_values = tbl;//zaznam v cam tab

                            }
                            else
                            {
                                foreach (Cam_table t in tbl)//osetrit null aj tu
                                {
                                    if (t != null)
                                    {
                                        if (t.get_mac() == packet.Ethernet.Source.ToString())
                                        {
                                            t.set_timer();
                                            t.set_port(name);
                                            has_src = true;
                                        }
                                        if (t.get_mac() == packet.Ethernet.Destination.ToString())
                                        {
                                            mac_dest = t.get_mac();
                                            port_name = t.get_port();
                                        }
                                    }
                                    

                                }
                                if (has_src == false)
                                {
                                    Cam_table c = new Cam_table();
                                    
                                    c.set_cam(packet.Ethernet.Source.ToString(), name);
                                   
                                    
                                    tbl.Add(c);
                                    Packet_counter.cam_values = tbl;//zaznam v cam tab

                                }
                            }
                            
                            //send
                            


                        }
                        //iny thread a funkcia
                        //---------------------------------------------------------------------------------------------
                        
                        

                        //determine where to send
                        packet_buff.Add(packet);//new packet added
                        if (mac_dest != "")//dest already in the cam tab
                        {
                            if (name == port_name)//we are sending back on this port
                            {
                                h_loop.run_handler(packet);
                            }
                            else
                            {
                                h.run_handler(packet);
                                
                            }
                        }
                        else//broadcast everywhere except current port
                        {
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
