
using System.IO;

namespace ReverseShellServer.Models
{
    public class PowerShellExecutor : ICommandExecutor
    {
        private StreamWriter streamWriter;

        public PowerShellExecutor(StreamWriter writer)
        {
            this.streamWriter = writer;
        }

        public void ExecuteCommand(string command)
        {
            streamWriter.WriteLine($"powershell {command}");
            streamWriter.Flush();
        }
    }
}
