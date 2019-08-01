using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SharpScanner
{
    public class Scanner
    {
        List<Host> hosts = new List<Host>();
        OptionParser parser;

        int counter;

        public Scanner(OptionParser parser)
        {
            this.parser = parser;
            StructureInput(parser.hosts);
        }

        public void ScanHost()
        {

            foreach (Host host in hosts)
            {
                if (host.IsAlive)
                {
                    counter++;
                    foreach (int port in parser.ports)
                    {
                        try
                        {
                            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                            socket.Connect(host.address, port);
                            host.ports.Add(port);
                            socket.Close();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    
                     Reporting report = new Reporting(host);
                     report.OutputHost();
                }
            }
            Console.WriteLine("Scan Finished: {0} hosts are up", counter.ToString());
        }

        public string GetHostname(string address)
        {
            if (parser.NO_RESOLVE) {
                return "Unknown";
            }

            try
            {
                return System.Net.Dns.GetHostEntry(address).HostName;
            }
            catch
            {
                return "Unknown";
            }
        }

        public string GetAddress(string hostname)
        {
            try
            {
                return System.Net.Dns.GetHostEntry(hostname).AddressList[0].ToString();
            }
            catch {
                return "";
            }
        }


        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

        public void DiscoverHost()
        {
            if (parser.ports.Count == 0) {
                Console.WriteLine("DISCOVERY MODE - Host List\n");
            }

            if (parser.ICMP)
            {
                System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
                foreach (Host host in hosts)
                {
                    if (String.IsNullOrEmpty(host.address)){
                        continue;
                    }

                    System.Net.NetworkInformation.PingReply reply = pingSender.Send(host.address, 1);

                    if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        if (parser.ports.Count == 0)
                        {
                            counter++;
                            host.hostname = GetHostname(host.address);
                            Reporting report = new Reporting(host, true);
                        }
                        else
                        {
                            host.IsAlive = true;
                        }
                    }  
                }
            }
            else if (parser.ARP)
            {

                byte[] mac = new byte[6];
                int macAddrLen = mac.Length;

                foreach (Host host in hosts)
                {
                    if (String.IsNullOrEmpty(host.address)){
                        continue;
                    }
                    uint uintAddress = BitConverter.ToUInt32(System.Net.IPAddress.Parse(host.address).GetAddressBytes(), 0);
                    if (SendARP(uintAddress, 0, mac, ref macAddrLen) == 0)
                    {
                        if (parser.ports.Count == 0)
                        {
                            counter++;
                            host.hostname = GetHostname(host.address);
                            Reporting report = new Reporting(host, true);
                        }
                        else
                        {
                            host.IsAlive = true;
                        }
                    }
                }
            }
        }

        public void StructureInput(List<string> input)
        {
            Regex single = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\z");
            Regex range = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\-\d{1,3}\z");
            Regex name = new Regex(@"^[0-9A-Z]([-.\w]*[0-9A-Z])*$",RegexOptions.IgnoreCase);

            foreach (string item in input)
            {
                Match single_match = single.Match(item);
                Match range_match = range.Match(item);
                Match name_match = name.Match(item);


                if (single_match.Success)
                {
                    Host host = new Host(item);
                    hosts.Add(host);
                }
                else if (range_match.Success)
                {
                    StructureRange(item);
                }
                else if (name_match.Success) {
                    Host host = new Host(GetAddress(item));
                    hosts.Add(host);
                }

                RetrieveHostNames();
            }

            if (hosts.Capacity != 0)
            {
                if (parser.ports.Count == 0)
                {
                    Console.WriteLine("\nScan Finished: {0} hosts are up", counter.ToString());
                }
                else
                {
                    ScanHost();
                }
            }
            else
            {
                Console.WriteLine("No Host Alive");
            }
        }

        public void RetrieveHostNames()
        {

            if (parser.FORCE)
            {
                foreach (Host host in hosts)
                {
                    host.IsAlive = true;
                }
            }
            else
            {
                DiscoverHost();
            }

            foreach (Host host in hosts)
            {
                if (host.IsAlive)
                {
                    host.hostname = GetHostname(host.address);
                }
            }
        }

        public void StructureName(string input)
        {
            string address = GetAddress(input);

            if (address != null)
            {
                Host host = new Host(address);
                host.hostname = input;
                hosts.Add(host);
            }
        }

        public void StructureRange(string input)
        {

            Regex ip = new Regex(@"\b\d{1,3}\.?");
            MatchCollection result = ip.Matches(input);
            string network = result[0].ToString() + result[1].ToString() + result[2].ToString();

            int start_host = Convert.ToInt32(result[3].ToString());

            Regex hypen = new Regex(@"\b-\d?");
            MatchCollection end_result = ip.Matches(input);
            int end_host = Convert.ToInt32(end_result[4].ToString());

            while (start_host <= end_host)
            {
                Host host = new Host(network + start_host);
                hosts.Add(host);
                start_host++;
            }
        }
    }
}
