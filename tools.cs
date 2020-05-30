using System;
using System.Diagnostics;

namespace Telebot
{
    class tools
    {
        private static readonly string SHELL = Program.SHELL;
        private static readonly string CHROME_PATH = Program.CHROME_PATH;
        private static  readonly string FIREFOX_PATH = Program.FIREFOX_PATH;
        private static void DEFAULT_BROWSER (string site)
        {
            Firefox(site);
        }

        public static bool pkill(string name)
        {
            if (name == "winhost")
                return false;
            try
            {
                foreach (Process p in Process.GetProcessesByName(name))
                    p.Kill();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string ConversationLogic(string msg)
        {
            string response = "that's my staff:\n/cmd /screenshot /download /proc /kill /run /chrome /firefox";
            switch (msg.ToLower())
            {
                case "?": response = "?"; break;
                case "what is the weather today?": response = "haha i am not the weather bot"; break;
                case "oh":
                case "ok": response = "yeah"; break;
                default: break;
            }
            return response;
        }


        public static string GetProcesses()
        {
            UInt16 i = 0;
            string result = "";
            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                i++;
                result += $"{i} {process.ProcessName}\n";
            }
            return result;
        }
        public static string PathByProgram(string program)
        {
            string path = string.Empty;
            switch (program)
            {
                case "chrome":
                    path = CHROME_PATH;
                    break;
                case "firefox":
                    path = FIREFOX_PATH;
                    break;
            }
            return path;
        }
        public static void Firefox()
        {
            try { Process.Start(FIREFOX_PATH); }
            catch { }
        }

        public static void Search(string[] msg)
        {
            string query = "";
            for (int i = 1; i < msg.Length; i++)
            {
                query += msg[i];
                query += ' ';
            }
            query = query.Substring(0, query.Length - 1);
            query = query.Replace(' ', '+');
            DEFAULT_BROWSER($"https://duckduckgo.com/?q={query}");
        }

        public static void Youtube(string[] msg)
        {
            string query = "";
            for (int i = 1; i < msg.Length; i++)
            {
                query += msg[i];
                query += ' ';
            }
            query = query.Replace(' ', '+');
            DEFAULT_BROWSER("https://www.youtube.com/results?search_query=" + query);
        }

        public static void Chrome()
        {
            try { Process.Start(@"C:/Program Files (x86)/Google/Chrome/Application/chrome.exe"); }
            catch { }
        }

        public static void Firefox(string site)
        {
            try
            {
                Process app = new Process();
                app.StartInfo.FileName = SHELL;
                app.StartInfo.RedirectStandardInput = true;
                app.StartInfo.RedirectStandardOutput = true;
                app.StartInfo.CreateNoWindow = true;
                app.StartInfo.UseShellExecute = false;
                app.Start();
                app.StandardInput.WriteLine($"\"{FIREFOX_PATH}\" " + site);
                app.StandardInput.Flush();
                app.StandardInput.Close();
                app.WaitForExit();
            }
            catch
            { }
        }
        public static void FirefoxOpenSite(string[] msg)
        {
            string site = "";
            for (int i = 1; i < msg.Length; i++)
            {
                site += msg[i];
            }
            try
            {
                Process app = new Process();
                app.StartInfo.FileName = SHELL;
                app.StartInfo.RedirectStandardInput = true;
                app.StartInfo.RedirectStandardOutput = true;
                app.StartInfo.CreateNoWindow = true;
                app.StartInfo.UseShellExecute = false;
                app.Start();
                app.StandardInput.WriteLine($"\"{FIREFOX_PATH}\" " + site);
                app.StandardInput.Flush();
                app.StandardInput.Close();
                app.WaitForExit();
            }
            catch
            { }
        }
        public static void ChromeOpenSite(string[] msg)
        {
            string site = "";
            for (int i = 1; i < msg.Length; i++)
            {
                site += msg[i];
            }
            try
            {
                Process app = new Process();
                app.StartInfo.FileName = SHELL;
                app.StartInfo.RedirectStandardInput = true;
                app.StartInfo.RedirectStandardOutput = true;
                app.StartInfo.CreateNoWindow = true;
                app.StartInfo.UseShellExecute = false;
                app.Start();
                app.StandardInput.WriteLine($"\"{CHROME_PATH}\" " + site);
                app.StandardInput.Flush();
                app.StandardInput.Close();
                app.WaitForExit();
            }
            catch
            { }
        }
        public static void Run(string program)
        {
            try { Process.Start(program); }
            catch { }
        }
    }
}
