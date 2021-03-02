using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Media;
using AntiAlexa.Properties;

namespace AntiAlexa
{
    class Program
    {
        // allow MessageBox in console application
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type); // User32.dll method

        static void Main(string[] args)
        {
            const int waitTime = 1000 * 60 * 5; // 5 minutes (milliseconds)
            string appPath = Assembly.GetExecutingAssembly().Location;
            string dest = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\AntiAlexa.exe";
            SoundPlayer sp = new SoundPlayer(Resources.Silent1s); // Silent1s is a 1s silent wav file
            
            if (System.IO.File.Exists(dest) == false) // False -> 1st execution -> install AntiAlexa
            {
                System.IO.File.Copy(appPath, dest);
                Process.Start(dest);

                MessageBox((IntPtr)0, "AntiAlexa has been installed correctly", "AntiAlexa", 0);

                // remove this copy of AntiAlexa using cmd (after 3s -> the app needs some time to terminate)
                Process.Start(new ProcessStartInfo()
                {
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + appPath + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                });

                return; // close the app -> cmd will delete it in 3s
            }
            
            // fake sound every 5 min -> avoid bluetooth standby -> avoid nag echo dot's connection sound
            while (true)
            {
                sp.PlaySync();
                System.Threading.Thread.Sleep(waitTime);
            }
        }
    }
}