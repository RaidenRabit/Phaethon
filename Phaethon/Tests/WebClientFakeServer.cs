using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebClient;

namespace Tests
{
    public class WebClientFakeServer
    {
        private Process _iisProcess;
        
        public void StartServer()
        {
            var webClientApplicationPath = GetApplicationPath("WebClient");
            var internalApiApplicationPath = GetApplicationPath("InternalApi");

            KillAllIIS();
            IISProcess(49873, webClientApplicationPath);
            IISProcess(64007, internalApiApplicationPath);

        }

        private void IISProcess(int iisPort, string path)
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:{0} /port:{1}", path, iisPort);
            _iisProcess.StartInfo.CreateNoWindow = true;
            _iisProcess.StartInfo.RedirectStandardOutput = true;
            _iisProcess.StartInfo.UseShellExecute = false;
            _iisProcess.Start();
        }

        protected virtual string GetApplicationPath(string app)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
            return Path.Combine(solutionFolder, app);
        }

        private void KillAllIIS()
        {
            Process[] ps = Process.GetProcessesByName("iisexpress");

            foreach (Process p in ps)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        p.Kill();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Unable to kill process {0}, exception: {1}", p.ToString(), ex.ToString()));
                }
            }
        }


        public void Dispose()
        {
            // Ensure IISExpress is stopped
            if (_iisProcess.HasExited == false)
            {
                _iisProcess.Kill();
            }

        }

    }
}
