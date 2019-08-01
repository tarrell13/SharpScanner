using System;
using System.Text.RegularExpressions;

namespace SharpScanner
{
    class MainClass
    {

        private static String _VERSION = "1.0";
        private static String _AUTHOR = "Tarrell Fletcher";

        public static void Main(string[] args)
        {

            Console.WriteLine(@"   _____ _                      _____                                   ");
            Console.WriteLine(@"  / ____| |                    / ____|                                  ");                              
            Console.WriteLine(@" | (___ | |__   __ _ _ __ _ __| (___   _'__ __ _ _ __  _ __   ___ _ __  ");
            Console.WriteLine(@"  \___ \| '_ \ / _` | '__| '_ \\___ \ / __/ _` | '_ \| '_ \ / _ \ '__|  ");
            Console.WriteLine(@"  ____) | | | | (_| | |  | |_) |___) | (_| (_| | | | | | | |  __/ |     ");
            Console.WriteLine(@" |_____/|_| |_|\__,_|_|  | .__/_____/ \___\__,_|_| |_|_| |_|\___|_|     ");
            Console.WriteLine(@"                         | |                                            ");
            Console.WriteLine(@"                         |_|                                            ");
            Console.Write("\n");
            Console.WriteLine("Author: " + _AUTHOR);
            Console.WriteLine("Version: " + _VERSION);
            Console.Write("\n");

            string command = "";

            foreach (string arg in args) {
                command += arg + " ";
            }

            OptionParser run = new OptionParser(command);
        }
    }
}
