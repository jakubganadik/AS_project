﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace c_sharp_test_2
{
    class Cam_table_print
    {
        Form1 myform;
        public void set(Form1 f)
        {
            this.myform = f;
        }
        public void run_cam_print()
        {
            Thread.Sleep(1000);
            while (true)
            {
                

                myform.Invoke(myform.myDelegate_3);
                Thread.Sleep(1000);
            }
            


        }
    }
}
