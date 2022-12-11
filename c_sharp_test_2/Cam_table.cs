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
        private Stopwatch _stopwatch;
        private int _curTime;
        private int _maxTime;
        private string _timePassed;
        public void set_cam(string m, string p,int ma,string i)
        {
            this._maxTime = ma;
            this._mac = m;
            this._port = p;
            Stopwatch stopwatch = new Stopwatch();
            _stopwatch = stopwatch;
            _stopwatch.Start();
        }
        public void set_timer(int ma)
        {
            this._maxTime = ma;
            _stopwatch.Restart();

        }
        public string Port { get; set; }

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
        
        private int CurrentTime(int startParse, int length, int toSeconds) => toSeconds*int.Parse(_timePassed.Substring(startParse, length));
        public int get_timer()
        {
            

                _timePassed = _stopwatch.Elapsed.ToString();

                _curTime = _maxTime - CurrentTime(0, 2, 3600)
                                    - CurrentTime(3, 2, 60)
                                    - CurrentTime(6, 2, 1);
                if (_curTime < 0)
                {
                    _curTime = -1;
                }
                return _curTime;
            
            
        }
        

       
    }
}
