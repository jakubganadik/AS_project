using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PcapDotNet.Core;
namespace c_sharp_test_2
{
    public partial class Form2 : Form
    {
        IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
        private int deviceIndex_1, deviceIndex_2;
        private Form2 form;
        public Form2()
        {
            InitializeComponent();
        }
        public void set_form(Form2 form)
        {
            this.form = form;
        }
        private void Form2_Load(object sender, EventArgs e)
        {

            string dev_descr = "";
            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];

                if (device.Description != null)
                {
                    dev_descr += device.Description;
                    dev_descr += "\n";
                }


                else
                {
                    dev_descr += "No description available\n";
                }



            }
            label2.Text = dev_descr;





        }
        public Tuple<int,int> get_packet_device_index(){ //test
            return Tuple.Create(deviceIndex_1, deviceIndex_2);
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usr_input = textBox1.Text;
            int deviceIndex_1 = usr_input[0] - '0';
            int deviceIndex_2 = usr_input[2] - '0';
            if (deviceIndex_1 < 1 || deviceIndex_1 > allDevices.Count || deviceIndex_2 < 1 || deviceIndex_2 > allDevices.Count || deviceIndex_1 == deviceIndex_2)
            {
                label3.Text = "nieco sa pokazilo";
            }
            else
            {
                this.deviceIndex_1 = deviceIndex_1;
                this.deviceIndex_2 = deviceIndex_2;
                form.Close();
            }
        }
    }
}
