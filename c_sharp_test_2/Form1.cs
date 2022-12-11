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
        public AddListItem myDelegate_rules;
        private int i;
        private int j;
        private int help_print;
        private bool one_row;
        private int cur_i;
        //private Cam_table[] up_cams;
        //private int[] removed_cams;
        private List<CamTable> up_cams;
        private List<int> removed_cams;
        private int cur_time;
        private int max_time;
        private bool remove_cam_val;
        private int size;
        private int counter_cam_num;
        static readonly object _object = new object();
        private int[] empty_tab;
        private bool cleared_table;
        private string to_print;
        private BlockingCollection<Rule> lor;
        private string rule_str;
        private int rule_remove;
        private List<Rule> remaining_rules;
        public Form1()
        {
            InitializeComponent();

            myDelegate = new AddListItem(update_values);
            myDelegate_2 = new AddListItem(update_values_2);
            myDelegate_3 = new AddListItem(update_cam);
            myDelegate_rules = new AddListItem(update_rules);
            //max_time = 10;
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



            int[] num_pck1 = Listener.NumberPacketsPort1;
            int[] num_pck2 = Listener.NumberPacketsPort2;
            for (int i = 0; i < 14; i++)//tu povodne bolo len pre jeden delegate
            {
                num_pck1[i] = 0;
                num_pck2[i] = 0;
            }
            
            

        }
       
        private void clear_cam()
        {
            j = 0;
            size = 0;
            BlockingCollection<CamTable> a =Listener.CamValues;
            
            //int[] removed_cams = new int[4];
            //Cam_table[] up_cams = new Cam_table[4];
            BlockingCollection<CamTable> b = new BlockingCollection<CamTable>();//prerobit
            foreach (CamTable c in a)
            {
                
                size++;

            }
            
            j = 0;
            foreach (CamTable item in a.GetConsumingEnumerable())
            {
                if (size == j + 1)
                {
                    break;
                }
                j++;
            }
            a = b;
            Listener.CamValues = a;
            
            richTextBox1.Text = " ";



        }
        private void update_rules()
        {
            rule_str = "";
            i = 0;
            foreach (Rule r in Rules_parser.SetOfRules)
            {
                rule_str += "| "+i.ToString() + " src_mac " + r.SourceMac + " dst_mac " + r.DestinationMac + " ip_src " + r.SourceIP + " ip_dst " + r.DestinationeIP + " filter " + r.Filter + " io " + r.InOutRule + " " + r.ExceptRule + "|"+"\n";
                i++;
            }
            richTextBox2.Text = rule_str;
        }
        public void update_cam()//after the first cam table update run a delegate with a thread in it
        {


            i = 0;
            help_print = 0;
            BlockingCollection<CamTable> a = Listener.CamValues;

            List<int> removed_cams = new List<int>();
            List<CamTable> up_cams = new List<CamTable>();
            //different approach, solve the controls
            one_row = false;
            i = 1;
            cur_i = 0;
            remove_cam_val = false;
            counter_cam_num = 0;
            //int[] empty_tab = new int[3];
            
            lock (_object)
            {
                if (cleared_table == true)
                {
                    clear_cam();
                    cleared_table = false;
                }
                if (a != null)
                {   //arr with updated values
                    to_print = "";
                    foreach (CamTable c in a)
                    {
                        cur_time = c.get_timer();
                        to_print+= c.get_mac() + " " + c.get_port() + " " + cur_time + "\n";
                        
                        counter_cam_num++;
                        if (cur_time < 0)
                        {
                            remove_cam_val = true;
                        }
                        
                        i++;
                    }
                    //richTextBox1.Text = "";
                    richTextBox1.Text = to_print;
                    i = 0;
                    if (remove_cam_val == true)
                    {
                        j = 0;
                        size = 0;

                        BlockingCollection<CamTable> b = new BlockingCollection<CamTable>();//prerobit
                        foreach (CamTable c in a)
                        {
                            cur_time = c.get_timer();
                            //cur_time = c.get_timer();
                            if (cur_time >= 0) //bolo <0
                            {
                                up_cams.Add(c);
                                //up_cams[i] = c;
                                i++;
                            }
                            else
                            {
                                removed_cams.Add(j + 1);
                                //removed_cams[j] = j + 1;
                                j++;
                            }
                            size++;

                        }
                        foreach (CamTable t in up_cams)
                        {
                            if (t != null)
                            {
                                b.Add(t);
                            }

                        }
                        j = 0;
                        foreach (CamTable item in a.GetConsumingEnumerable())
                        {
                            if (size == j + 1)
                            {
                                break;
                            }
                            j++;
                        }
                        a = b;
                        Listener.CamValues = a;
                       
                        richTextBox1.Text = "";
                        to_print = "";
                        foreach (CamTable c in up_cams)
                        {

                            to_print+= c.get_mac() + " " + c.get_port() + " " + c.get_timer() + "\n";
                        }
                        richTextBox1.Text = to_print;

                    }
                }
               
            }
            
        }
            




        
        public void update_values_2()
        {//prerobit
            int[] stats = Listener.NumberPacketsPort2;
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
            int[] stats = Listener.NumberPacketsPort1;
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
            Listener.TimerValue = max_time;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cleared_table = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Rules_parser r_p = new Rules_parser();
            r_p.set_form(this);
            r_p.set_rules(textBox30.Text);

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            try
            {
                rule_remove = Int32.Parse(textBox31.Text);

                List<Rule> rem = new List<Rule>();
                BlockingCollection<Rule> b_r = new BlockingCollection<Rule>();
                i = 0;
                foreach (Rule r in Rules_parser.SetOfRules)
                {
                    if (i != rule_remove)
                    {
                        rem.Add(r);
                    }
                    i++;
                    
                }
                richTextBox2.Clear();
                rule_str = "";
                i = 0;
                foreach (Rule r in rem)
                {
                    rule_str +=  "|"+i.ToString() + " src_mac " + r.SourceMac + " dst_mac " + r.DestinationMac + " ip_src " + r.SourceIP + " ip_dst " + r.DestinationeIP + " filter " + r.Filter + " io " + r.InOutRule + " "+ r.ExceptRule+"|"+"\n";
                    i++;
                    b_r.Add(r);
                }
                richTextBox2.Text = rule_str;
                Rules_parser.SetOfRules = b_r;


                }
            catch
            {
                textBox31.Text = "nebolo zadane cislo";
            }

        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
