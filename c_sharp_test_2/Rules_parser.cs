using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
namespace c_sharp_test_2
{
    class Rules_parser
    {
        private string rules;
        private string c;
        private BlockingCollection<Cam_table> tbl;
        private BlockingCollection<Rule> lor;
        private bool exclude;
        private int i;
        private int j;
        
        Form1 form1;
        public void set_form(Form1 f)
        {
            form1 = f;
        }
        public void set_rules(string r)
        {
            string[] string_arr = r.Split(new char[0]);
            tbl = Packet_counter.cam_values;
            exclude = false;
            
            string[] r_s = new string[8];
            for (int i = 0; i < 8; i++)
            {
                r_s[i] = "";
            }
            j = 0;
            lor = Packet_counter.List_of_rules;
            
            for (int i = 0; i < string_arr.Length; i++)
            {
                j = i + 1;
                if (string_arr[i] == "mac_dst")
                {
                    
                    r_s[0] = string_arr[j];
                }
                else if (string_arr[i] == "mac_src")
                {
                    r_s[6] = string_arr[j];
                }
                else if (string_arr[i] == "ip_dst")
                {
                    r_s[7] = string_arr[j];
                }
                else if (string_arr[i] == "ip_src") //source a dest ip solve
                {
                    r_s[2] = string_arr[j];

                }
                else if (string_arr[i] == "port")
                {
                    r_s[1] = string_arr[j];
                }
                
                else if (string_arr[i] == "-exc")
                {
                    r_s[5] = "exc";
                    exclude = true;
                }
                else if (string_arr[i] == "filter")
                {
                    r_s[4] = string_arr[j];
                }
                else if (string_arr[i] == "io")
                {
                    r_s[3] = string_arr[j];
                }
            }
            
            Rule rul = new Rule();
            rul.set_filters(r_s[0],r_s[6], r_s[2],r_s[7], r_s[1], r_s[4], r_s[3],r_s[5]);//number needs incrementing---------------------------------------------------------------
            lor.Add(rul);
            Packet_counter.List_of_rules = lor;
            form1.Invoke(form1.myDelegate_rules);

            /*
            foreach (Cam_table c_t in tbl)
            {
                if ((c_t.get_mac() == r_s[0] || c_t.get_ip() == r_s[2]) && exclude == false)
                {
                    Rule rul = new Rule();
                    rul.set_filters(r_s[0], r_s[2], r_s[1], r_s[4], 1,r_s[3]);//get number from global arr
                    lor.Add(rul);
                    break;
                }
                else if (exclude == true)
                {
                    if (c_t.get_mac() != r_s[0] || c_t.get_ip() != r_s[2])
                    {
                        
                        Rule rul = new Rule();
                        rul.set_filters(r_s[0], r_s[2], r_s[1], r_s[4], 1, r_s[3]);
                        lor.Add(rul);
                    }
                }
            }
            */



        }
    }
}
