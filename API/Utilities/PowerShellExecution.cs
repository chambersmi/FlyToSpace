using System.Diagnostics;

namespace API.Utilities
{
    public class PowerShellExecution
    {
        public static void RunPowerShellScript()
        {
            var scriptPath = Path.Combine(AppContext.BaseDirectory, "Scripts", "redis.ps1");
            var processInfo = new ProcessStartInfo
            {
                FileName = "pwsh", 
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            if (process != null)
            {
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("Output:");
                //Console.WriteLine(output);

                if (!string.IsNullOrWhiteSpace(errors))
                {
                    Console.WriteLine("Errors:");
                    Console.WriteLine(errors);
                }
            }
            else
            {
                Console.WriteLine("Failed to start Powershell Process");
            }

        }
    }
}
