using DevExpress.XtraEditors;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace Katana_Spoofer
{
    public partial class Form1 : XtraForm
    {
        public bool isSpoofed = false;
        public string RNameID = RandomNameID(5);

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (isSpoofed == true)
                {
                    labelControl2.Text = "Spoofed!";
                    labelControl2.ForeColor = System.Drawing.Color.FromArgb(125, 124, 250);
                }
                else
                {
                    labelControl2.Text = "NOT SPOOFED!";
                    labelControl2.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                textEdit1.Text = $"Error: {ex.Message}";

                labelControl2.Text = "NOT SPOOFED!";
                labelControl2.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey HardwareGUID = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\Hardware Profiles\\0001", true);
                HardwareGUID.SetValue("HwProfileGuid", $"{{{Guid.NewGuid()}}}");

                RegistryKey MachineGUID = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography", true);
                MachineGUID.SetValue("MachineGuid", Guid.NewGuid().ToString());

                RegistryKey MachineId = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\SQMClient", true);
                MachineId.SetValue("MachineId", $"{{{Guid.NewGuid()}}}");

                RegistryKey SystemInfo = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\SystemInformation", true);

                Random rnd = new Random();
                int day = rnd.Next(1, 31);
                string dayStr;
                if (day < 10) dayStr = $"0{day}";
                else dayStr = day.ToString();

                int month = rnd.Next(1, 13);
                string monthStr;
                if (month < 10) monthStr = $"0{month}";
                else monthStr = month.ToString();

                SystemInfo.SetValue("BIOSReleaseDate", $"{dayStr}/{monthStr}/{rnd.Next(2016, 2023)}");
                SystemInfo.SetValue("BIOSVersion", RandomVersionID(10));
                SystemInfo.SetValue("ComputerHardwareId", $"{{{Guid.NewGuid()}}}");
                SystemInfo.SetValue("ComputerHardwareIds", $"{{{Guid.NewGuid()}}}\n{{{Guid.NewGuid()}}}\n{{{Guid.NewGuid()}}}\n");
                SystemInfo.SetValue("SystemManufacturer", RandomNameID(15));
                SystemInfo.SetValue("SystemProductName", RandomNameID(6));



                RegistryKey Update = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate", true);
                Update.SetValue("SusClientId", Guid.NewGuid().ToString());
                Update.SetValue("SusClientIdValidation", Encoding.UTF8.GetBytes(RandomID(25)));

                isSpoofed = true;

                textEdit1.Text = "Spoofed To: " + GetGUID();

                labelControl2.Text = "Spoofed Gloabl Unique ID!";
                labelControl2.ForeColor = System.Drawing.Color.FromArgb(125, 124, 250);
            }
            catch (Exception ex)
            {
                isSpoofed = false;

                textEdit1.Text = $"Error: {ex.Message}";

                labelControl2.Text = "NOT SPOOFED!";
                labelControl2.ForeColor = System.Drawing.Color.Red;
            }
        } //GUID

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string newProcessorId = RandomProcessorID(16);

                RegistryKey ProcessorID = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                ProcessorID = ProcessorID.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", true);

                ProcessorID.SetValue("ProcessorId", newProcessorId);

                string PID = (string)ProcessorID.GetValue("ProcessorId", "");

                ProcessorID.Close();

                isSpoofed = true;

                textEdit1.Text = "Spoofed To: " + PID;

                labelControl2.Text = "Spoofed Processor ID!";
                labelControl2.ForeColor = System.Drawing.Color.FromArgb(125, 124, 250);
            }
            catch (Exception ex)
            {
                isSpoofed = false;

                textEdit1.Text = $"Error: {ex.Message}";

                labelControl2.Text = "NOT SPOOFED!";
                labelControl2.ForeColor = System.Drawing.Color.Red;
            }
        } //Processor ID

        private void SimpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string KatanaNameID = RNameID;

                RegistryKey ActiveComputerName = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                ActiveComputerName = ActiveComputerName.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName", true);

                RegistryKey ComputerName = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                ComputerName = ComputerName.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\ComputerName\ComputerName", true);

                _ = (string)ActiveComputerName.GetValue("ComputerName");
                _ = (string)ComputerName.GetValue("ComputerName");

                ComputerName.SetValue("ComputerName", "Katana-" + KatanaNameID, RegistryValueKind.String);
                ActiveComputerName.SetValue("ComputerName", "Katana-" + KatanaNameID, RegistryValueKind.String);

                ActiveComputerName.Close();
                ComputerName.Close();

                isSpoofed = true;

                textEdit1.Text = "Spoofed To: " + "Katana-" + KatanaNameID;

                labelControl2.Text = "Spoofed Computer Name!";
                labelControl2.ForeColor = System.Drawing.Color.FromArgb(125, 124, 250);
            }
            catch (Exception ex)
            {
                isSpoofed = false;

                textEdit1.Text = $"Error: {ex.Message}";

                labelControl2.Text = "NOT SPOOFED!";
                labelControl2.ForeColor = System.Drawing.Color.Red;
            }
        } //Computer Name

        private void SimpleButton4_Click(object sender, EventArgs e)
        {
            labelControl2.ForeColor = Color.FromArgb(125, 124, 250);
            labelControl2.Text = "PLEASE WAIT...";

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                try
                {
                    Random random = new Random(Guid.NewGuid().GetHashCode());

                    ManagementObjectCollection networkAdapters = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True").Get();

                    foreach (ManagementObject adapter in networkAdapters.Cast<ManagementObject>())
                    {
                        string netConnectionID = adapter["NetConnectionID"].ToString();
                        string caption = adapter["Caption"].ToString();
                        string name = adapter["Name"].ToString();
                        string deviceId = adapter["DeviceID"].ToString().PadLeft(4, '0');

                        if (caption.Contains("Bluetooth") || name.Contains("Bluetooth") || netConnectionID.Contains("Bluetooth"))
                        {
                            continue;
                        }

                        byte[] macAddress = new byte[6];

                        random.NextBytes(macAddress);

                        string spoofedMacAddress = string.Concat(macAddress.Select(x => string.Format("{0}", x.ToString("X2"))).ToArray()).Insert(2, "-").Insert(5, "-").Insert(8, "-").Insert(11, "-").Insert(14, "-");

                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey($"SYSTEM\\CurrentControlSet\\Control\\Class\\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\\{deviceId}", true))
                        {
                            registryKey.SetValue("NetworkAddress", spoofedMacAddress);
                        }

                        Process disableProcess = new Process();
                        disableProcess.StartInfo.FileName = "netsh.exe";
                        disableProcess.StartInfo.Arguments = $"interface set interface \"{netConnectionID}\" admin=disable";
                        disableProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        disableProcess.Start();
                        disableProcess.WaitForExit();

                        Process enableProcess = new Process();
                        enableProcess.StartInfo.FileName = "netsh.exe";
                        enableProcess.StartInfo.Arguments = $"interface set interface \"{netConnectionID}\" admin=enable";
                        enableProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        enableProcess.Start();
                        enableProcess.WaitForExit();

                        isSpoofed = true;

                        textEdit1.Text = $"MAC Address Of: {caption}, Spoofed To: {spoofedMacAddress}";

                        labelControl2.Text = "Spoofed MAC Address!";
                        labelControl2.ForeColor = Color.FromArgb(125, 124, 250);
                    }
                }
                catch (Exception expection)
                {
                    isSpoofed = false;

                    textEdit1.Text = $"Error: {expection.Message}";
                    labelControl2.Text = "NOT SPOOFED!";
                    labelControl2.ForeColor = Color.Red;
                }

            }).Start();
        } //MAC Address

        public string GetGUID()
        {
            string machineGuid = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid", "");
            return machineGuid;
        }

        public static string RandomVersionID(int length)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result += chars[random.Next(chars.Length)];
            }

            return result;
        }

        public static string RandomNameID(int length)
        {
            string chars = "0123456789";
            string result = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result += chars[random.Next(chars.Length)];
            }

            return result;
        }

        public static string RandomID(int length)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result += chars[random.Next(chars.Length)];
            }

            return result;
        }

        public static string RandomProcessorID(int length)
        {
            string chars = "0123456789";
            string result = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result += chars[random.Next(chars.Length)];
            }

            return result;
        }
    }
}
