using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace SharpScanner

{
    public class OptionParser

    {
        public bool WRITE = false;
        public bool ARP = false;
        public bool ICMP = true;
        public bool FORCE = false;
        public bool NO_RESOLVE = false;

        public List<int> ports = new List<int> { };
        public List<string> hosts = new List<string> { };
        private List<string> commands = new List<string> { "-f", "--force", "--arp", "--icmp", "-p", "--port", "-n", "--no-resolve"};

        public OptionParser(string input)

        {
            string[] args = input.Trim().Split(' ');
            List<string> arguments = new List<string>();

            for (int i = 0; i <= args.Length-1; i++)
            {
                arguments.Add(args[i]);
            }

            for (int i = 0; i <= arguments.Count-1; i++)
            {

                if (arguments[i] is "--arp" || arguments[i] is "--icmp")
                {
                    if (arguments[i] is "--arp")
                    {
                        ARP = true;
                        ICMP = false;
                    }
                    else
                    {
                        continue;
                    }

                }
                else if (arguments[i] is "-p" || arguments[i] is "--ports")
                {

                    if (arguments.Count - 1 == i)
                    {
                        Console.WriteLine("No Ports Specified");
                        continue;
                    }
                    else
                    {
                        if (arguments[i + 1].Contains(","))
                        {
                            if (arguments[i + 1].Contains(".") || Regex.IsMatch(arguments[i + 1], @"^[a-zA-Z]+$"))
                            {
                                Console.WriteLine("Invalid Ports Separate Ports using comma: 80,445,3389");
                                return;
                            }

                            string[] port_input = arguments[i + 1].Trim().Split(',');

                            foreach (string port in port_input)
                            {
                                ports.Add(Convert.ToInt32(port));
                            }

                        }
                        else if (arguments[i + 1].Contains(".") || Regex.IsMatch(arguments[i + 1], @"^[a-zA-Z]+$"))
                        {
                            Console.WriteLine("Invalid Ports Separate Ports using comma: 80,445,3389");
                            return;
                        }
                        else if (Regex.IsMatch(arguments[i + 1], @"\b\d{1,5}\z"))
                        {
                            ports.Add(Convert.ToInt32(arguments[i + 1]));
                        }
                    }
                }
                else if (Regex.IsMatch(arguments[i], @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\z") || Regex.IsMatch(arguments[i], @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\-\d{1,3}\z")
                    || Regex.IsMatch(arguments[i], @"^[a-zA-Z]+$") || Regex.IsMatch(arguments[i], @"^[0-9A-Z]([-.\w]*[0-9A-Z])*$", RegexOptions.IgnoreCase))
                {
                    hosts.Add(arguments[i]);
                }
                else if (arguments[i] is "-n" || arguments[i] is "--no-resolve")
                {
                    NO_RESOLVE = true;
                }
                else if (arguments[i] is "-f" || arguments[i] is "--force") {
                    FORCE = true;
                }
                else if (arguments[i].Contains("-") || arguments[i].Contains("--"))
                {
                    if (commands.Contains(arguments[i]))
                    {
                        continue;
                    }
                    else
                    {
                        Usage();
                    }
                }
            }

            Scanner scan = new Scanner(this);
        }

        public void Usage()
        {
            Console.WriteLine("\nUsage: SharpScanner.exe <options> <IP address or range>");
            Console.WriteLine("");
            Console.WriteLine("--arp                    Discover host using ARP");
            Console.WriteLine("--icmp [Default]         Discover host using ICMP");
            Console.WriteLine("--no-resolve, -n         Disable DNS lookups on hosts");
            Console.WriteLine("--ports, -p              Specifies ports to scan for, by default performs Disovery Scan using ARP or ICMP");
            Console.WriteLine("\nExamples:");
            Console.WriteLine("(1) SharpScanner.exe --arp 192.168.1.1");
            Console.WriteLine("(2) SharpScanner.exe --arp 192.168.1.1-15 -p 22,80,445");
            Console.WriteLine("(3) SharpScanner.exe 192.168.1.10 172.16.8.136 -n -p 80");
            System.Environment.Exit(1);
        }
    }
}
