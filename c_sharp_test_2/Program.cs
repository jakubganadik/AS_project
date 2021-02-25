using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using System.Windows.Forms;
using System.Threading;
namespace c_sharp_test_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int deviceIndex_1, deviceIndex_2;
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form2 myForm_2 = new Form2();
            myForm_2.set_form(myForm_2);
            Application.Run(myForm_2);//Form1 myForm = new Form1();

            (deviceIndex_1, deviceIndex_2)= myForm_2.get_packet_device_index(); // test
            
            Form1 myForm = new Form1();
            

            Gui gui = new Gui();

            




            gui.set_form(myForm);
            ThreadStart childref_gui = new ThreadStart(gui.start_gui);
            //ThreadStart childref_2 = new ThreadStart(lis_2.recv);
            Thread childThread_gui = new Thread(childref_gui);
            //Thread childThread_2 = new Thread(childref_2);
            childThread_gui.Start();



            //-------------------------------------------------------------------------------------------------



            //Packet_counter pc = new Packet_counter();
            // Retrieve the device list from the local machine
            //vypise ssa do dalsieho okna


            // Take the selected adapter

            //---------------------------------------------------------------------------------------------------starting new threads
            PacketDevice selectedDevice_1 = allDevices[deviceIndex_1 - 1];

            PacketDevice selectedDevice_2 = allDevices[deviceIndex_2 - 1];
            Listener lis_1 = new Listener();
            Listener lis_2 = new Listener();
            Packet_handler h_1 = new Packet_handler();
            Packet_handler h_2 = new Packet_handler();
            string name_1, name_2;
            name_1 = "one";
            name_2 = "two";
            h_1.get_device_send(selectedDevice_2, name_1);
            h_2.get_device_send(selectedDevice_1, name_2);
            lis_1.list_get(selectedDevice_1, name_1, h_1, myForm);
            lis_2.list_get(selectedDevice_2, name_2, h_2, myForm);
            ThreadStart childref_1 = new ThreadStart(lis_1.recv);
            ThreadStart childref_2 = new ThreadStart(lis_2.recv);
            Thread childThread_1 = new Thread(childref_1);
            Thread childThread_2 = new Thread(childref_2);
            childThread_1.Start();
            childThread_2.Start();




            //--------------------------------------------------------------------------------------------------------



            //int test = 0;
            /*
            Console.WriteLine("This is C#");
            AutoResetEvent event_1 = new AutoResetEvent(true);
            Packet_counter.thr_wait = event_1;
            */
            /*
            while (true)
            {
                Thread.Sleep(10);
                test++;
                Packet_counter.load_values = test;
                //myForm.update_values(test);

                myForm.Invoke(myForm.myDelegate);
                */
                /*
                event_1.Set();
                event_1.Reset();
                */

                // update things in myOtherForm here


            }
            
        }
    }
//}