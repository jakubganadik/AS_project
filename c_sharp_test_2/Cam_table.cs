using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
namespace c_sharp_test_2
{
    public class CamTable
    {
        private string _mac;
        private string _port;
        private int _timer;
        private Stopwatch _stopwatch;
        private int _cur_time;
        private int _cur_time_h;
        private int _cur_time_m;
        private int _cur_time_s;
        private int _max_time;
        private string _ip;
        private string _time_passed;
        public void set_cam(string m, string p,int ma,string i)
        {
            this._max_time = ma;
            this._mac = m;
            this._port = p;
            Stopwatch stopwatch = new Stopwatch();
            _stopwatch = stopwatch;
            // Begin timing.
            _stopwatch.Start();
            _ip = i;
        }
        public void set_timer(int ma)
        {
            this._max_time = ma;
            _stopwatch.Restart();

        }
        public void set_port(string p)
        {
            this._port = p;
        }
        public string get_mac()
        {
            if (this._mac == null)
            {
                return "";
            }
            else
            {
                return this._mac;
            }
            
        }
        public string get_port()
        {
            if (this._port == null)
            {
                return "";
            }
            else
            {
                return this._port;
            }
            
        }
        public string get_ip()
        {
            return _ip;
        }

        private int CurrentTime(int startParse, int length, int toSeconds) => toSeconds*int.Parse(_time_passed.Substring(startParse, length));
        public int get_timer()
        {
            
            //max_time = Packet_counter.val_for_timer;
            if (_stopwatch.Elapsed.ToString() == null)
            {
                return 0;
            }
            else
            {
                _time_passed = _stopwatch.Elapsed.ToString();

                _cur_time = _max_time - CurrentTime(0, 2, 3600)
                                    - CurrentTime(3, 2, 60)
                                    - CurrentTime(6, 2, 1);
                if (_cur_time < 0)
                {
                    _cur_time = -1;
                }
                return _cur_time;
            }
            
        }
        

       
    }
}
