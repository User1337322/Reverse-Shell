
using System.IO;

namespace ReverseShellServer.Models
{
    public class CmdExecutor : ICommandExecutor
    {
        private StreamWriter streamWriter;

        public CmdExecutor(StreamWriter writer)
        {
            this.streamWriter = writer;
        }

        public void ExecuteCommand(string command)
        {
            streamWriter.WriteLine($"cmd /c {command}");
            streamWriter.Flush();
        }
    }
}
