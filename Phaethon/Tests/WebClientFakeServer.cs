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
            const int iisPort = 49873;
            var applicationPath = GetApplicationPath();
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:{0} /port:{1}", applicationPath, iisPort);
            _iisProcess.StartInfo.CreateNoWindow = true;
            _iisProcess.StartInfo.RedirectStandardOutput = true;
            _iisProcess.StartInfo.UseShellExecute = false;
            _iisProcess.Start();

        }
        protected virtual string GetApplicationPath()
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
            return Path.Combine(solutionFolder, "WebClient");
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
