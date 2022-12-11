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
        private BlockingCollection<Rule> lor;
        private bool exclude;
        private int i;
        private int j;
        public static BlockingCollection<Rule> SetOfRules = new BlockingCollection<Rule>();
        Form1 form1;
        public void set_form(Form1 f)
        {
            form1 = f;
        }
        public void set_rules(string r)
        {
            string[] string_arr = r.Split();
            Rule rul = new Rule();
            rul.DestinationMac = "";
            rul.SourceMac = "";
            rul.DestinationeIp = "";
            rul.SourceIp = "";
            rul.Port = "";
            rul.ExceptRule = "";
            rul.Filter = "";
            rul.InOutRule = "";
            for (int i = 0; i < string_arr.Length; i++)
            {
                j = i + 1;
                if (string_arr[i] == "mac_dst")
                {
                    
                    rul.DestinationMac = string_arr[j];
                }
                else if (string_arr[i] == "mac_src")
                {
                    rul.SourceMac = string_arr[j];
                }
                else if (string_arr[i] == "ip_dst")
                {
                    rul.DestinationeIp = string_arr[j];
                }
                else if (string_arr[i] == "ip_src") //source a dest ip solve
                {
                    rul.SourceIp = string_arr[j];

                }
                else if (string_arr[i] == "port")
                {
                    rul.Port = string_arr[j];
                }
                
                else if (string_arr[i] == "-exc")
                {
                    rul.ExceptRule = "exc";
                }
                else if (string_arr[i] == "filter")
                {
                    rul.Filter = string_arr[j];
                }
                else if (string_arr[i] == "io")
                {
                    rul.InOutRule = string_arr[j];
                }
            }
            
            SetOfRules.Add(rul);
            form1.Invoke(form1.myDelegate_rules);

        }
    }
}
