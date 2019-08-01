# SharpScanner

Basic network port scanner created in the C# language. The program is designed to work with execute-assembly enabled applications to avoid dropping binary to disk.

# Usage
```
Usage: SharpScanner.exe <options> <IP address or range>

--arp                    Discover host using ARP
--icmp [Default]         Discover host using ICMP
--no-resolve, -n         Disable DNS lookups on hosts
--ports, -p              Specifies ports to scan for, by default performs Disovery Scan using ARP or ICMP
--force, -f              Treats all hosts as alive to skip Discovery Phase

Examples:
(1) SharpScanner.exe --arp 192.168.1.1
(2) SharpScanner.exe --arp 192.168.1.1-15 -p 22,80,445
(3) SharpScanner.exe 192.168.1.10 172.16.8.136 -n -p 80
```

# Example 01 - Discovery Mode

When executing the application without any ports specified the application will simply perform a host discovery using either ICMP or ARP.

```
C:\SharpScanner.exe 172.16.8.131-140

DISCOVERY MODE - Host List

172.16.8.131 [ DESKTOP-01.localdomain ]
172.16.8.135 [ LinuxHost-01 ]
172.16.8.136 [ LinuxHost-02 ]

Scan Finished: 3 hosts are up
```

# Example 02 - Scanner Mode

When you specify ports for the application to use, it will attempt to scan these hosts on the specified ports after discovery phase.

```
C:\>SharpScanner.exe 172.16.8.131-140  -p 22,445,135 -n
 
Host: 172.16.8.131 [ Unknown ]
PORTS
135/open
445/open


Host: 172.16.8.135 [ Unknown ]
PORTS
22/open


Host: 172.16.8.136 [ Unknown ]
PORTS
22/open


Scan Finished: 3 hosts are up
```