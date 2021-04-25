using System;
using System.Collections.Generic;
using System.Text;
using PcapDotNet.Packets;
using System.Collections.Concurrent;
namespace c_sharp_test_2
{
    //exc a IN nefunguju
    class Table_update//run thread to update
    {
        private string name;
        private string port_out;
        private Packet packet;
        private Captured_packet c_p;
        private string p_type;
        private BlockingCollection<Captured_packet> packet_buf;//check if LIFO behavior-------------------------------------------
        private Form1 myform;
        public void set_table(string name,Form1 f, BlockingCollection<Captured_packet> p)
        {
            this.name = name;
            
            
            
            this.myform = f;
            this.packet_buf = p;
        }
        public void update()// new delegate
        {
            
            
            while (true)
            {
                c_p = packet_buf.Take();
                packet = c_p.get_packet();
                port_out = c_p.get_io();
                p_type = c_p.get_packet_type();
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

                if (p_type == "Ethernet" || p_type == "IpV4" || p_type == "Tcp" || p_type == "Udp" || p_type == "InternetControlMessageProtocol" || p_type == "Arp") // toto spojit
                {
                    if (port_out == "IN") { 
                         
                    }
                    
                        
                        
                    
                    else if (port_out == "OUT") {
                        num_packets[6]++;
                         
                    }
                    else
                    {
                        num_packets[6]++;
                        num_packets[13]++;
                    }


                    if (p_type == "IpV4" || p_type == "Tcp" || p_type == "Udp" || p_type == "InternetControlMessageProtocol")
                    {
                        if (port_out == "IN")
                        {

                        }

                        else if (port_out == "OUT")
                        {
                            num_packets[5]++;
                        }
                        else
                        {
                            num_packets[5]++;
                            num_packets[12]++;
                        }
                            
                        
                        
                        
                        if (p_type == "Tcp")
                        {
                            if (packet.Ethernet.IpV4.Tcp.SourcePort == 80 || packet.Ethernet.IpV4.Tcp.DestinationPort == 80)
                            {
                                if (port_out == "IN")
                                {

                                }

                                else if (port_out == "OUT")
                                {
                                    num_packets[4]++;
                                }
                                else
                                {
                                    num_packets[4]++;
                                    num_packets[11]++;
                                }
                                
                                
                                
                            }
                            else
                            {
                                if (port_out == "IN")
                                {

                                }

                                else if (port_out == "OUT")
                                {
                                    num_packets[0]++;
                                }
                                else
                                {
                                    num_packets[0]++;
                                    num_packets[7]++;
                                }
                                
                                
                                
                            }



                        }
                        else if (p_type == "Udp")
                        {
                            if (port_out == "IN")
                            {

                            }

                            else if (port_out == "OUT")
                            {
                                num_packets[1]++;
                            }
                            else
                            {
                                num_packets[1]++;
                                num_packets[8]++;
                            }
                            
                            
                            
                        }
                        else if (p_type == "InternetControlMessageProtocol")
                        {
                            if (port_out == "IN")
                            {

                            }

                            else if (port_out == "OUT")
                            {
                                num_packets[2]++;
                            }
                            else
                            {
                                num_packets[2]++;
                                num_packets[9]++;
                            }
                            
                            
                            
                        }

                    }
                    else if (p_type == "Arp")
                    {
                        if (port_out == "IN")
                        {

                        }

                        else if (port_out == "OUT")
                        {
                            num_packets[3]++;
                        }
                        else
                        {
                            num_packets[3]++;
                            num_packets[10]++;
                        }
                        
                        
                        
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
