﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace c_sharp_test_2
{
    public partial class Form1 : Form
    {
        public delegate void AddListItem();
        public AddListItem myDelegate;
        public AddListItem myDelegate_2;
        /*
        public delegate void update_text(string str);
        ThreadStart threadstart;
        Thread my_thread;
        */
        public Form1()
        {
            InitializeComponent();

            myDelegate = new AddListItem(update_values);
            myDelegate_2 = new AddListItem(update_values_2);

        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // invoke
        {


            //int test = Packet_counter.Expired;
            //textBox1.Text = test.ToString();

            textBox1.Text = "0";
        }
        /*
        public void AddListItemMethod()
        {
            String myItem;
            for (int i = 1; i < 6; i++)
            {
                myItem = "MyListItem" + i.ToString();
                myListBox.Items.Add(myItem);
                myListBox.Update();
                Thread.Sleep(300);
            }
        }
        */
        /*
        public static IEnumerable<TControl> GetChildControls<TControl>(this Control control) where TControl : Control
        {
            var children = (control.Controls != null) ? control.Controls.OfType<TControl>() : Enumerable.Empty<TControl>();
            return children.SelectMany(c => GetChildControls<TControl>(c)).Concat(children);
        }
        */
        public void update_values_2()
        {//prerobit
            int[] stats = Packet_counter.load_values_2;
            int i = 0;//set this textbox
            foreach (TextBox tb in groupBox2.Controls.OfType<TextBox>())
            {
                if (tb.TabIndex > 10)
                {
                    tb.Text = stats[tb.TabIndex - 11].ToString();
                    i++;
                }
                


                
            }






        }
        public void update_values()
        {//prerobit
            int[] stats = Packet_counter.load_values;
            int i = 0;
            foreach (TextBox tb in groupBox1.Controls.OfType<TextBox>())
            {
                
                //tb.Text = stats[tb.TabIndex-1].ToString();
                if (tb.TabIndex <= 10)
                {
                    tb.Text = stats[tb.TabIndex - 1].ToString();
                    //break;
                }

                i++;
            }
                
           
                    
                
            
                
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            /*
            while (true)
            {
                int test = Packet_counter.Expired;
                textBox1.Text = test.ToString();
                //event_1.WaitOne();
            }
            */
            /*
            AutoResetEvent event_1 = Packet_counter.thr_wait;
            //tu sa zavola event handler
            while (true)
            {
                int test = Packet_counter.Expired;
                textBox1.Text = test.ToString();
                event_1.WaitOne();
            }
            */

            /*
            threadstart = new ThreadStart(function);
            my_thread = new Thread(threadstart);
            my_thread.Start();
            */
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}