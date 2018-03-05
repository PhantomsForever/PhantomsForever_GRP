using PhantomsForever_GRP.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhantomsForever_GRP.Core.Python
{
    public class PythonScript
    {
        private static readonly string CheckSumScript = Path.Combine(Application.StartupPath, "checksum.py");
        private static readonly string DecompressScript = Path.Combine(Application.StartupPath, "decompress.py");
        private static void Install()
        {
            if (!File.Exists(CheckSumScript))
            {
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("PhantomsForever_GRP.Core.Python.checksum.py"))
                {
                    using (var file = new FileStream(CheckSumScript, FileMode.Create, FileAccess.Write))
                    {
                        resource.CopyTo(file);
                    }
                }
            }
            if (!File.Exists(DecompressScript))
            {
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("PhantomsForever_GRP.Core.Python.decompress.py"))
                {
                    using (var file = new FileStream(DecompressScript, FileMode.Create, FileAccess.Write))
                    {
                        resource.CopyTo(file);
                    }
                }
            }
        }
        public static async Task<string> GetPacketChecksum(string hex)
        {
            if (!File.Exists(CheckSumScript))
                Install();
            var psi = new ProcessStartInfo(Settings.Python36Path)
            {
                Arguments = CheckSumScript + " " + hex,
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
        public static async Task<string> DecompressPacketPayload(string hex)
        {
            if (!File.Exists(DecompressScript))
                Install();
            var psi = new ProcessStartInfo(Settings.Python36Path)
            {
                Arguments = DecompressScript + " " + hex,
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