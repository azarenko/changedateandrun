using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cdar
{
    static class Program
    {
        public struct SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;
        };

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        public extern static void Win32GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SystemTime sysTime);

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length != 2)
            {
                MessageBox.Show("Please specify a date and execution path", "Change date and run app", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime date = DateTime.Now;

            try
            {
                date = DateTime.Parse(args[0]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Change date and run app", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(args[1]))
            {
                MessageBox.Show(string.Format("File path is not valid", args[1]), "Change date and run app", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime now = DateTime.Now;

            try
            {
                ChangeDate(date);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Change date and run app", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process p = Process.Start(args[1]);
                p.WaitForExit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Change date and run app", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ChangeDate(now);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Change date and run app", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private static void ChangeDate(DateTime date)
        {
            // Set system date and time
            SystemTime updatedTime = new SystemTime();
            updatedTime.Year = (ushort)date.Year;
            updatedTime.Month = (ushort)date.Month;
            updatedTime.Day = (ushort)date.Day;
            updatedTime.Hour = (ushort)date.Hour;
            updatedTime.Minute = (ushort)date.Minute;
            updatedTime.Second = (ushort)date.Second;
            // Call the unmanaged function that sets the new date and time instantly
            Win32SetSystemTime(ref updatedTime);
        }
    }
}
