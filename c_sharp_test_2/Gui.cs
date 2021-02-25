using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using System.Windows.Forms;

namespace c_sharp_test_2
{
    class Gui
    {
        private Form1 my_form;
        public void start_gui()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(my_form);//Application.Run(new Form1())
            
            //treba pockat
        }
        
        public void set_form(Form1 m)
        {
            this.my_form = m;
        }
        
    }
}
