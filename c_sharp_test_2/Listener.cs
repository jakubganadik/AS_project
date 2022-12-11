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
        private BlockingCollection<CamTable> tbl;
        private BlockingCollection<Rule> lor;
        private int max_time;
        private bool has_src;
        private bool has_dst;
        private bool not_a_pc;
        private string port_name;
        private string p_type;
        private bool filtered;
        private string in_out;
        private bool filtered_cam;
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

            BlockingCollection<CamTable> cam_vals = new BlockingCollection<CamTable>();
            BlockingCollection<Captured_packet> packet_buff = new BlockingCollection<Captured_packet>();
            Packet_counter.cam_values = cam_vals;
            Table_update t_up = new Table_update(); 

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
                        

                        p_type = "";
                        if (packet.DataLink.Kind.ToString() == "Ethernet") // toto spojit
                        {
                            p_type = "Ethernet";
                            if (packet.Ethernet.EtherType.ToString() == "IpV4")
                            {
                                p_type = "IpV4";
                                if (packet.Ethernet.IpV4.Protocol.ToString() == "Tcp")
                                {
                                    p_type = "Tcp";
                                }
                                else if (packet.Ethernet.IpV4.Protocol.ToString() == "Udp")
                                {
                                    p_type = "Udp";

                                }
                                else if (packet.Ethernet.IpV4.Protocol.ToString() == "InternetControlMessageProtocol")
                                {
                                    p_type = "InternetControlMessageProtocol";

                                }

                            }
                            else if (packet.Ethernet.EtherType.ToString() == "Arp")
                            {
                                p_type = "Arp";

                            }
                        }
                        in_out = "";
                        lor = Packet_counter.List_of_rules;
                        filtered = false;//careful with all keyword
                        filtered_cam = false;
                        
                        if (lor != null)
                        {//exclusive
                            foreach (Rule r in lor)//osetrit null aj tu
                            {   //src and dst mac combination
                                if (( (packet.Ethernet.Source.ToString().Equals(r.get_mac_src()) && packet.Ethernet.Destination.ToString().Equals(r.get_mac_dst())) || (packet.Ethernet.Source.ToString().Equals(r.get_mac_src()) && r.get_mac_dst().Equals("")) || (packet.Ethernet.Destination.ToString().Equals(r.get_mac_dst()) && r.get_mac_src().Equals(""))) || ((packet.Ethernet.IpV4.Source.ToString().Equals(r.get_ip_src()) && packet.Ethernet.IpV4.Destination.ToString().Equals(r.get_ip_dst())) || (packet.Ethernet.IpV4.Source.ToString().Equals(r.get_ip_src()) && r.get_ip_dst().Equals("")) || (packet.Ethernet.IpV4.Destination.ToString().Equals(r.get_ip_dst()) && r.get_ip_src().Equals(""))))//last rule goes packet.Ethernet.Source.ToString().Equals(r.get_mac()) || packet.Ethernet.IpV4.Source.ToString().Equals(r.get_ip())


                                {
                                    if (p_type.Equals(r.get_filter()) && r.get_excp().Equals("")) // if nothing, then filter everything
                                    {
                                        if ((packet.Ethernet.Source.ToString()[1] == '0' || packet.Ethernet.Source.ToString()[1] == '4' || packet.Ethernet.Source.ToString()[1] == '8' || packet.Ethernet.Source.ToString()[1] == 'C') && (p_type == "InternetControlMessageProtocol" || p_type == "Arp") && r.get_io() == "IN")
                                        {
                                            filtered_cam = true;
                                        }
                                        else if ((packet.Ethernet.Source.ToString()[1] == '0' || packet.Ethernet.Source.ToString()[1] == '4' || packet.Ethernet.Source.ToString()[1] == '8' || packet.Ethernet.Source.ToString()[1] == 'C') && (p_type == "InternetControlMessageProtocol" || p_type == "Arp") && r.get_io() == "OUT")
                                        {
                                            filtered_cam = false;
                                        }

                                        filtered = true;
                                        in_out = r.get_io();
                                        
                                        if (p_type == "Udp" && packet.Ethernet.IpV4.Udp.SourcePort.ToString() != r.get_port()) // test for port
                                        {
                                            filtered = false;
                                            in_out = "";
                                        }
                                        


                                    }
                                    else if (p_type.Equals(r.get_filter()) && r.get_excp().Equals("exc"))
                                    {
                                        if (r.get_io() == "")
                                        {
                                            filtered = false;
                                            in_out = r.get_io();
                                            filtered_cam = false;
                                        }
                                        else
                                        {
                                            filtered = true;
                                            in_out = r.get_io();
                                            filtered_cam = true;
                                        }
                                        
                                    }

                                    //if ((packet.Ethernet.IpV4.Source.ToString().Equals(r.get_ip_src()) && packet.Ethernet.IpV4.Destination.ToString().Equals(r.get_ip_dst())) || (packet.Ethernet.IpV4.Source.ToString().Equals(r.get_ip_src()) && r.get_ip_dst().Equals("")) || (packet.Ethernet.IpV4.Destination.ToString().Equals(r.get_ip_dst()) && r.get_ip_src().Equals("")))
                                    else if (p_type != r.get_filter() && r.get_excp().Equals("exc"))
                                    {

                                        filtered = true;
                                        filtered_cam = true;
                                       
                                        in_out = "IN";
                                    }



                                }
                               
                                else if(r.get_mac_dst().Equals("") && r.get_mac_src().Equals("") && r.get_ip_dst().Equals("") && r.get_ip_src().Equals("")) //urobit aj pre except
                                {
                                    if (p_type.Equals(r.get_filter()) && r.get_excp() == "")
                                    {
                                        filtered = true;
                                        in_out = r.get_io();
                                        if (p_type == "Udp" && packet.Ethernet.IpV4.Udp.SourcePort.ToString() != r.get_port()) // test for port
                                        {
                                            filtered = false;
                                            in_out = "";
                                        }
                                    }
                                    else if (p_type.Equals(r.get_filter()) && r.get_excp() == "exc")
                                    {
                                        filtered = false;
                                        in_out = "";
                                        if (p_type == "Udp" && packet.Ethernet.IpV4.Udp.SourcePort.ToString() != r.get_port()) // test for port
                                        {
                                            filtered = true;
                                            in_out = r.get_io();
                                        }
                                    }
                                    else if (p_type != r.get_filter() && r.get_excp() == "exc"){
                                        filtered = true;
                                        in_out = r.get_io();//in_out = r.get_io()

                                    }
                                    
                                    
                                    
                                }
                                

                            }
                        }
                        if (filtered_cam == false)
                        {
                            mac_dest = "";
                            not_a_pc = true;
                            if (packet.Ethernet.Source.ToString()[1] == '0' || packet.Ethernet.Source.ToString()[1] == '4' || packet.Ethernet.Source.ToString()[1] == '8' || packet.Ethernet.Source.ToString()[1] == 'C') //kontrola zariadenia
                            {

                                not_a_pc = false;
                                has_src = false;
                                max_time = Packet_counter.val_for_timer;
                                tbl = Packet_counter.cam_values;
                                //if empty
                                if (tbl == null)
                                {
                                    CamTable c = new CamTable();

                                    c.set_cam(packet.Ethernet.Source.ToString(), name, max_time, packet.Ethernet.IpV4.Source.ToString());


                                    tbl.Add(c);
                                    Packet_counter.cam_values = tbl;//zaznam v cam tab

                                }
                                else
                                {
                                    foreach (CamTable t in tbl)//osetrit null aj tu
                                    {
                                        if (t != null)
                                        {
                                            if (t.get_mac() == packet.Ethernet.Source.ToString())
                                            {
                                                t.set_timer(max_time);//timer
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
                                        CamTable c = new CamTable();

                                        c.set_cam(packet.Ethernet.Source.ToString(), name, max_time, packet.Ethernet.IpV4.Source.ToString());


                                        tbl.Add(c);
                                        Packet_counter.cam_values = tbl;//zaznam v cam tab

                                    }
                                }

                                //send



                            }
                        }
                        


                        Captured_packet c_p = new Captured_packet();

                        if (filtered == false)
                        {
                            if (mac_dest != "")//dest already in the cam tab
                            {

                                if (name == port_name)//we are not sending back on the cur port
                                {
                                    //h_loop.run_handler(packet);
                                    //c_p.set_packet(packet, in_out,p_type);
                                }

                                else if (name != port_name)
                                {
                                    h.run_handler(packet);
                                    //c_p.set_packet(packet,  in_out, p_type);
                                }
                            }
                            else//broadcast everywhere except current port
                            {
                                h.run_handler(packet);
                                //c_p.set_packet(packet,  in_out, p_type);
                            }
                        }
                        
                        c_p.set_packet(packet, in_out, p_type);
                        packet_buff.Add(c_p);//print vals









                        break;
                    default:
                        throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                }




            } while (true);

        }
    }
}
