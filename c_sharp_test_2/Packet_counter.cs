using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading;
using System.Collections.Concurrent;
namespace c_sharp_test_2
{
    public class Packet_counter
    {
        private static int[] statistics;
        private static int[] statistics_2;
        private static BlockingCollection<Cam_table> vals;
        private static BlockingCollection<Rule> rules;
        private static BlockingCollection<Listener> a;
        private static bool val1;
        private static AutoResetEvent event_1;
        private static int max_val;
        public static int[] load_values   //bude musiet byt len jedno load values
        {
            get
            {
                // Reads are usually simple
                return statistics;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                statistics = value;
            }
            /*
            private static int test;
            public static void set_vars(int test)
            {
                test = test;
            }
            public static int get_vars() 
            {

                return test;
            }
            */
        }
        
        
        public static int[] load_values_2 { get { return statistics_2; } set { statistics_2 = value; } }
        public static int val_for_timer { get { return max_val; } set { max_val = value; } }
        public static BlockingCollection<Cam_table> cam_values{ get { return vals; } set { vals = value; } }
        public static BlockingCollection<Rule> List_of_rules { get { return rules; } set { rules = value; } }
        

        public static AutoResetEvent thr_wait //zbehne raz na zaciatku aby sa tam dostal tento event handler
        {
            get
            {
                // Reads are usually simple
                return event_1;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                event_1 = value;
            }
            /*
            private static int test;
            public static void set_vars(int test)
            {
                test = test;
            }
            public static int get_vars() 
            {

                return test;
            }
            */
        }


    }
}
