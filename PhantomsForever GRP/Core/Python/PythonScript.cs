using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Python
{
    public class PythonScript
    {
        public static async Task<string> GetPacketChecksum(string hex)
        {
            var psi = new ProcessStartInfo("C:\\Python36\\python.exe")
            {
                Arguments = "C:\\test.py " + hex,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var proc = Process.Start(psi);
            var _error = proc.StandardError;
            var _writer = proc.StandardInput;
            var _reader = proc.StandardOutput;
            var d = _error.ReadToEnd();
            return await _reader.ReadLineAsync();
        }
    }
}