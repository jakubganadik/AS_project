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
        public static BlockingCollection<CamTable> CamValues = new BlockingCollection<CamTable>();
        public static int[] NumberPacketsPort1 = new int[1000];
        public static int[] NumberPacketsPort2 = new int[1000];
        
        public static int TimerValue { get; set; }
 
        public void list_get(PacketCommunicator pack_comm, string name, Packet_handler h, Form1 myform,string n_l,Packet_handler h_l)
        {

            this.pack_comm = pack_comm;
            this.name = name;
            this.h = h;
            this.myform = myform;
            this.name_loop = n_l;
            this.h_loop = h_l;
            TimerValue = 10;
        }
        public void recv()
        {
            
            BlockingCollection<Captured_packet> packet_buff = new BlockingCollection<Captured_packet>();
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
                NumberPacketsPort1 = num_packets;//tuto test
                myform.Invoke(myform.myDelegate);
            }
            else if (name == "two")
            {
                NumberPacketsPort2 = num_packets;//tuto test
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
                        filtered = false;//careful with all keyword
                        filtered_cam = false;
                        
                        if (Rules_parser.SetOfRules != null)
                        {//exclusive
                            foreach (Rule r in Rules_parser.SetOfRules)//osetrit null aj tu
                            {   //src and dst mac combination
                                if (( (packet.Ethernet.Source.ToString().Equals(r.SourceMac) && packet.Ethernet.Destination.ToString().Equals(r.DestinationMac)) || (packet.Ethernet.Source.ToString().Equals(r.SourceMac) && r.DestinationMac.Equals("")) || (packet.Ethernet.Destination.ToString().Equals(r.DestinationMac) && r.SourceMac.Equals(""))) || ((packet.Ethernet.IpV4.Source.ToString().Equals(r.SourceIp) && packet.Ethernet.IpV4.Destination.ToString().Equals(r.DestinationeIp)) || (packet.Ethernet.IpV4.Source.ToString().Equals(r.SourceIp) && r.DestinationeIp.Equals("")) || (packet.Ethernet.IpV4.Destination.ToString().Equals(r.DestinationeIp) && r.SourceIp.Equals(""))))


                                {
                                    if (p_type.Equals(r.Filter) && r.ExceptRule.Equals("")) // if nothing, then filter everything
                                    {
                                        if ((packet.Ethernet.Source.ToString()[1] == '0' || packet.Ethernet.Source.ToString()[1] == '4' || packet.Ethernet.Source.ToString()[1] == '8' || packet.Ethernet.Source.ToString()[1] == 'C') && (p_type == "InternetControlMessageProtocol" || p_type == "Arp") && r.InOutRule == "IN")
                                        {
                                            filtered_cam = true;
                                        }
                                        else if ((packet.Ethernet.Source.ToString()[1] == '0' || packet.Ethernet.Source.ToString()[1] == '4' || packet.Ethernet.Source.ToString()[1] == '8' || packet.Ethernet.Source.ToString()[1] == 'C') && (p_type == "InternetControlMessageProtocol" || p_type == "Arp") && r.InOutRule == "OUT")
                                        {
                                            filtered_cam = false;
                                        }

                                        filtered = true;
                                        in_out = r.InOutRule;
                                        
                                        if (p_type == "Udp" && packet.Ethernet.IpV4.Udp.SourcePort.ToString() != r.Port) // test for port
                                        {
                                            filtered = false;
                                            in_out = "";
                                        }
                                        


                                    }
                                    else if (p_type.Equals(r.Filter) && r.ExceptRule.Equals("exc"))
                                    {
                                        if (r.InOutRule == "")
                                        {
                                            filtered = false;
                                            in_out = r.InOutRule;
                                            filtered_cam = false;
                                        }
                                        else
                                        {
                                            filtered = true;
                                            in_out = r.InOutRule;
                                            filtered_cam = true;
                                        }
                                        
                                    }

                                    //if ((packet.Ethernet.IpV4.Source.ToString().Equals(r.get_ip_src()) && packet.Ethernet.IpV4.Destination.ToString().Equals(r.get_ip_dst())) || (packet.Ethernet.IpV4.Source.ToString().Equals(r.get_ip_src()) && r.get_ip_dst().Equals("")) || (packet.Ethernet.IpV4.Destination.ToString().Equals(r.get_ip_dst()) && r.get_ip_src().Equals("")))
                                    else if (p_type != r.Filter && r.ExceptRule.Equals("exc"))
                                    {

                                        filtered = true;
                                        filtered_cam = true;
                                       
                                        in_out = "IN";
                                    }



                                }
                               
                                else if(r.DestinationMac.Equals("") && r.SourceMac.Equals("") && r.DestinationeIp.Equals("") && r.SourceIp.Equals("")) //urobit aj pre except
                                {
                                    if (p_type.Equals(r.Filter) && r.ExceptRule == "")
                                    {
                                        filtered = true;
                                        in_out = r.InOutRule;
                                        if (p_type == "Udp" && packet.Ethernet.IpV4.Udp.SourcePort.ToString() != r.Port) // test for port
                                        {
                                            filtered = false;
                                            in_out = "";
                                        }
                                    }
                                    else if (p_type.Equals(r.Filter) && r.ExceptRule == "exc")
                                    {
                                        filtered = false;
                                        in_out = "";
                                        if (p_type == "Udp" && packet.Ethernet.IpV4.Udp.SourcePort.ToString() != r.Port) // test for port
                                        {
                                            filtered = true;
                                            in_out = r.InOutRule;
                                        }
                                    }
                                    else if (p_type != r.Filter && r.ExceptRule == "exc"){
                                        filtered = true;
                                        in_out = r.InOutRule;

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
                                max_time = TimerValue;
                                if (CamValues == null)
                                {
                                    CamTable c = new CamTable();
                                    c.set_cam(packet.Ethernet.Source.ToString(), name, max_time, packet.Ethernet.IpV4.Source.ToString());
                                    CamValues.Add(c);

                                }
                                else
                                {
                                    foreach (CamTable t in CamValues)//osetrit null aj tu
                                    {
                                        if (t != null)
                                        {
                                            if (t.get_mac() == packet.Ethernet.Source.ToString())
                                            {
                                                t.set_timer(max_time);//timer
                                                t.Port=name;
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
                                        CamValues.Add(c);

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
