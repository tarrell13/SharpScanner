using System.Collections.Generic;

namespace SharpScanner

{
    public class Host {

        public string address;
        public string hostname;
        public string mac_address;
        public List<int> ports = new List<int>();

        private bool _isAlive;

        public Host(string address) {
            this.address = address;
            _isAlive = false;
        }

        public bool IsAlive {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

    }
}
