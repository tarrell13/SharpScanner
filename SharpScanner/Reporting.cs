using System;
using System.IO;

namespace SharpScanner
{
    public class Reporting
    {

        private Host host;

        public Reporting(Host host, bool DISCOVERY=false) {
            this.host = host;

            if (DISCOVERY) {
                DiscoverOutput();
            }
        }

        public Reporting(Host host)
        {
            this.host = host;
        }

        public void DiscoverOutput() {
            Console.WriteLine("{0} [ {1} ]", host.address,host.hostname);
        }
        public void OutputHost()
        {

            Console.WriteLine("Host: {0} [ {1} ]", host.address, host.hostname);

            if (host.ports.Capacity != 0)
            {
                Console.WriteLine("PORTS");
                host.ports.Sort();
            }
            else
            {
                Console.WriteLine("No Ports Detected");
            }

            foreach (int port in host.ports)
            {
                Console.WriteLine("{0}/open", port);
            }

            Console.WriteLine("\n");
        }

    }
}
