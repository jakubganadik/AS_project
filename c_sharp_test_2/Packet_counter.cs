using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading;
namespace c_sharp_test_2
{
    public class Packet_counter
    {
        private static int[] statistics;
        private static int[] statistics_2;
        private static bool val1;
        private static AutoResetEvent event_1;
        
        public static int[] load_values
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
        
        //public static int tcp_c { get{ return test} set { } } //dostanem  jednotlive hodnoty
        //public static int udp_c { get { return test} set { } } 
        // public static int tcp_c { get { return test} set { } } 
        //public static int tcp_c { get { return test} set { } } 
        //public static int tcp_c { get { return test} set { } } 

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
