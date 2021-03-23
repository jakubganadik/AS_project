using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Concurrent;
namespace c_sharp_test_2
{
    public partial class Form1 : Form
    {
        public delegate void AddListItem();
        public AddListItem myDelegate;
        public AddListItem myDelegate_2;
        public AddListItem myDelegate_3;
        private int i;
        private int j;
        private int help_print;
        private bool one_row;
        private int cur_i;
        private Cam_table[] up_cams;
        private int[] removed_cams;
        private int cur_time;
        private int max_time;
        private bool remove_cam_val;
        private int size;
        static readonly object _object = new object();
        private bool cleared_table;
        public Form1()
        {
            InitializeComponent();

            myDelegate = new AddListItem(update_values);
            myDelegate_2 = new AddListItem(update_values_2);
            myDelegate_3 = new AddListItem(update_cam);
            max_time = 10;
            cleared_table = false;

        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // invoke
        {


            foreach (TextBox tb in groupBox1.Controls.OfType<TextBox>())
            {

                //tb.Text = stats[tb.TabIndex-1].ToString();
                if (tb.TabIndex <= 14)
                {
                    tb.Text = "0";
                    //break;
                }

                
            }
            foreach (TextBox tb in groupBox2.Controls.OfType<TextBox>())
            {
                if (tb.TabIndex > 14)//asi zbytocne
                {
                    tb.Text = "0";

                }




            }



            int[] num_pck1 = Packet_counter.load_values;
            int[] num_pck2 = Packet_counter.load_values_2;
            for (int i = 0; i < 14; i++)//tu povodne bolo len pre jeden delegate
            {
                num_pck1[i] = 0;
                num_pck2[i] = 0;
            }
            
            

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
        /*
        public void run_thread_cam()
        {
            ThreadStart childref_1 = new ThreadStart(this.update_cam);//nie pocas inicializacie

            Thread camthr = new Thread(childref_1);

            camthr.Start();
        }
        */
        private void clear_cam()
        {
            j = 0;
            size = 0;
            BlockingCollection<Cam_table> a = Packet_counter.cam_values;
            int[] removed_cams = new int[4];
            Cam_table[] up_cams = new Cam_table[4];
            BlockingCollection<Cam_table> b = new BlockingCollection<Cam_table>();//prerobit
            foreach (Cam_table c in a)
            {
                
                size++;

            }
            
            j = 0;
            foreach (Cam_table item in a.GetConsumingEnumerable())
            {
                if (size == j + 1)
                {
                    break;
                }
                j++;
            }
            a = b;
            Packet_counter.cam_values = a;
            foreach (TextBox tb in groupBox3.Controls.OfType<TextBox>())
            {
                tb.Text = "";

            }



        }
        public void update_cam()//after the first cam table update run a delegate with a thread in it
        {


            i = 0;
            help_print = 0;
            BlockingCollection<Cam_table> a = Packet_counter.cam_values;//wont work if its empty
                                                                        //a.CopyTo();


            //different approach, solve the controls
            one_row = false;
            i = 1;
            cur_i = 0;
            remove_cam_val = false;
            
            lock (_object)
            {
                if (cleared_table == true)
                {
                    clear_cam();
                    cleared_table = false;
                }
                if (a != null)
                {   //odpocitavat podla poctu tabuliek
                    foreach (Cam_table c in a)//upravit tento bullshit a pridat lock na cele toto
                    {
                        foreach (TextBox tb in groupBox3.Controls.OfType<TextBox>())
                        {
                            if (tb.TabIndex < 3 * i && tb.TabIndex >= 3 * i - 3)
                            {

                                if (tb.TabIndex % 3 == 0)
                                {
                                    if (c != null)
                                    {
                                        tb.Text = c.get_mac();
                                    }
                                    else
                                    {

                                        break;
                                    }

                                }
                                else if (tb.TabIndex % 3 == 1)
                                {
                                    if (c != null)
                                    {
                                        tb.Text = c.get_port();

                                    }
                                    else
                                    {

                                        break;
                                    }

                                }
                                else if (tb.TabIndex % 3 == 2)
                                {
                                    if (c != null)
                                    {
                                        tb.Text = c.get_timer();

                                        cur_time = Int32.Parse(c.get_timer().Substring(6, 2));

                                        if (cur_time > max_time)
                                        {
                                            remove_cam_val = true;
                                        }
                                    }
                                    else
                                    {

                                        break;
                                    }




                                }


                            }




                        }
                        i++;
                    }

                    i = 0;
                    if (remove_cam_val == true)
                    {
                        j = 0;
                        size = 0;
                        int[] removed_cams = new int[4];
                        Cam_table[] up_cams = new Cam_table[4];
                        BlockingCollection<Cam_table> b = new BlockingCollection<Cam_table>();//prerobit
                        foreach (Cam_table c in a)
                        {
                            cur_time = Int32.Parse(c.get_timer().Substring(6, 2));
                            if (cur_time <= max_time)
                            {
                                up_cams[i] = c;
                                i++;
                            }
                            else
                            {
                                removed_cams[j] = j + 1;
                                j++;
                            }
                            size++;

                        }
                        foreach (Cam_table t in up_cams)
                        {
                            if (t != null)
                            {
                                b.Add(t);
                            }

                        }
                        j = 0;
                        foreach (Cam_table item in a.GetConsumingEnumerable())
                        {
                            if (size == j + 1)
                            {
                                break;
                            }
                            j++;
                        }
                        a = b;
                        Packet_counter.cam_values = a;
                         foreach (int rem in removed_cams)
                        {
                            foreach (TextBox tb in groupBox3.Controls.OfType<TextBox>())
                            {
                                if (tb.TabIndex < 3 * rem && tb.TabIndex >= 3 * rem - 3)
                                {
                                    tb.Text = "";
                                }

                            }
                        }
                    }
                }
            }
            
        }
            




        
        public void update_values_2()
        {//prerobit
            int[] stats = Packet_counter.load_values_2;
            int i = 0;//set this textbox
            foreach (TextBox tb in groupBox2.Controls.OfType<TextBox>())
            {
                if (tb.TabIndex > 14)//asi zbytocne
                {
                    tb.Text = stats[tb.TabIndex - 15].ToString();
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
                if (tb.TabIndex <= 14)
                {
                    tb.Text = stats[tb.TabIndex - 1].ToString();
                    //break;
                }

                i++;
            }
                
           
                    
                
            
                
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            

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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox43_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)//kontrola ci pouzivatel nezada nieco zle
        {
            
            max_time = Int32.Parse(textBox29.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cleared_table = true;
        }
    }
}
