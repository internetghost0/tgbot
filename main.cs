using System;
using System.Diagnostics;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Telebot
{
    class Program
    {
        private static readonly string BOT_API = "000000:zzzzzzz";
        private static readonly long MY_ID = 6969696;

        private static int WIDTH = 1440;
        private static int HEIGHT = 900;

        public static readonly string SHELL = "cmd";
        public static readonly string CHROME_PATH = @"C:/Program Files (x86)/Google/Chrome/Application/chrome.exe";
        public static readonly string FIREFOX_PATH = @"C:/Program Files/Mozilla Firefox/firefox.exe";

        private static readonly TelegramBotClient Bot = new TelegramBotClient(BOT_API);
        private static readonly long[] trustedUsers = { MY_ID };
        private static List<long> blacklist = new List<long>();
        private static int userIndex = 0;
        static bool CheckId(long id)
        {
            for (int i = 0; i < trustedUsers.Length; i++)
            {
                if (id == trustedUsers[i])
                {
                    userIndex = i;
                    return true;
                }
            }
            return false;
        }
        static void Main(string[] args)
        {
        start:

            try
            {
                var me = Bot.GetMeAsync().Result;
            }
            catch 
            {
               // Cannot connect to tg servers
                Thread.Sleep(60 * 1000);
                goto start;
            }

            Bot.OnUpdate += OnUpdate;
            Bot.StartReceiving(Array.Empty<UpdateType>());
            _ = Bot.SendTextMessageAsync(trustedUsers[userIndex], "I waked up...");
            while (true)
            {
                Thread.Sleep(1 * 60 * 1000);
                try { Bot.GetMeAsync(); }
                catch { Bot.StopReceiving(); goto start; } // if bot started, but suddenly the internet connection dropped
            }
        }

        public static async void OnUpdate(object sender, UpdateEventArgs updateEventArgs)
        {
            var update = updateEventArgs.Update;

            Console.WriteLine($"firstname {update.Message.Chat.FirstName} id {update.Message.Chat.Id}");
            if (CheckId(update.Message.Chat.Id))
            {
                if (update.Message.Type.ToString() == "Document")
                {
                    try
                    {
                        Upload(update.Message.Document.FileName, update.Message.Document.FileId);
                        Send("Successful");
                    }
                    catch { Send("Error"); }
                }
                else if (update.Message.Type.ToString() == "Text")
                {
                    Check(update.Message.Text);
                }
                else
                {
                    Console.WriteLine("Message Type Error");
                }
            }
            else
            {
                if (!blacklist.Contains(update.Message.Chat.Id))
                {
                    await Bot.SendTextMessageAsync(update.Message.Chat.Id, "get away");
                    blacklist.Add(update.Message.Chat.Id);
                }

            }
        }
        private static void Check(string message)
        {
            string[] msg = message.Split(' ');
            Console.WriteLine(message);
            if (msg[0] == "/start")
            {
                Send("Nice to meet you");
                return;
            }
            if (msg[0].ToLower() == "hi" || msg[0].ToLower() == "hello")
            {
                Send(msg[0] + "\nbut I am not a smart bot, just simple program :(");
                return;
            }
            if (message[0] == '>')
            {
                message = message.Trim('>');
                string[] shellcommands = message.Split('\n');
                try
                {
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = SHELL;
                    cmd.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();
                    cmd.StandardOutput.ReadLine();
                    cmd.StandardOutput.ReadLine();
                    for (int i = 0; i < shellcommands.Length; i++)
                    {
                        cmd.StandardInput.WriteLine(shellcommands[i]);
                    }
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    Bot.SendTextMessageAsync(trustedUsers[userIndex], cmd.StandardOutput.ReadToEnd());
                }
                catch (Exception a)
                {
                    Send(a.Message);
                }
            }
            switch (msg[0].ToLower())
            {

                case "/cmd":
                    if (msg.Length > 1)
                        Execute(msg);
                    else
                        Send($"{SHELL} program arg1,arg2,...\nexample: {SHELL} dir");
                    break;
                case "/run":
                    if (msg.Length > 1)
                    {
                        string program = tools.PathByProgram(message.Substring(5));
                        tools.Run(program == string.Empty ? message.Substring(5) : program);
                    }
                    else
                        Send("run program arg1,arg2,...\nexample: run path/firefox.exe");
                    break;
                case "/shell":
                    break;
                case "/screenshot":
                    Send("one second...");
                    ScreenShot();
                    break;
                case "/screenshoth":
                    Send("wait few seconds...");
                    ScreenShotH();
                    break;
                case "/search":
                    tools.Search(msg);
                    break;
                case "/youtube":
                    tools.Youtube(msg);
                    break;
                case "/firefox":
                    if (msg.Length < 2)
                        tools.Firefox();
                    else
                        tools.FirefoxOpenSite(msg);
                    break;
                case "/chrome":
                    if (msg.Length < 2)
                        tools.Chrome();
                    else
                        tools.ChromeOpenSite(msg);
                    break;
                case "/download":
                    Download(msg);
                    break;
                case "/proc":
                    Send(tools.GetProcesses());
                    break;
                case "/kill":
                    if (msg.Length == 2)
                        Send(tools.pkill(msg[1]) ? "successful" : "error priv");
                    break;
                default:
                    Send(tools.ConversationLogic(message));
                    break;
            }
        }
        public static async void Send(string msg)
        {
            if (msg == string.Empty) return;
            if (msg.Length > 3000)
            {
                Send("msg is too long");
            }
            await Bot.SendTextMessageAsync(trustedUsers[userIndex], msg);
        }
        static void Execute(string[] message)
        { 
            // message = cmd args args
            string appName = SHELL;
            string args = "";
            for (int i = 1; i < message.Length; i++)
            {
                args += message[i] + " ";
            }
            Console.WriteLine($"app = {appName}, comm = {args}");
            try
            {
                Process app = new Process();
                app.StartInfo.FileName = appName;
                app.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
                app.StartInfo.RedirectStandardInput = true;
                app.StartInfo.RedirectStandardOutput = true;
                app.StartInfo.CreateNoWindow = true;
                app.StartInfo.UseShellExecute = false;
                app.Start();
                app.StandardInput.WriteLine(args);
                app.StandardInput.Flush();
                app.StandardInput.Close();
                Thread.Sleep(1);
                string temp = app.StandardOutput.ReadToEnd();
                if (temp.Length > 4000) temp = temp.Substring(0, 4000);
                Bot.SendTextMessageAsync(trustedUsers[userIndex], temp);
            }
            catch (Exception a)
            {
                Send(a.Message);
            }
        }
        static private async void ScreenShotH(string path = @"hsomepic.png")
        {
            Bitmap memoryImage = new Bitmap(WIDTH, HEIGHT);
            Size s = new Size(memoryImage.Width, memoryImage.Height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);
            memoryImage.Save(path);
            Thread.Sleep(500);
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Telegram.Bot.Types.InputFiles.InputOnlineFile inputOnlineFile = new
                        Telegram.Bot.Types.InputFiles.InputOnlineFile(fileStream, "screen" + new Random().Next(10, 100) + ".png");
                    await Bot.SendDocumentAsync(trustedUsers[userIndex], inputOnlineFile);
                }
                Thread.Sleep(500);
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Send(ex.Message);
            }
            Thread.Sleep(500);
        }
        static private async void ScreenShot(string path = @"somepic.png")
        {
            Bitmap memoryImage = new Bitmap(WIDTH, HEIGHT);
            Size s = new Size(memoryImage.Width, memoryImage.Height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);
            memoryImage.Save(path);
            Thread.Sleep(500);
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    _ = await Bot.SendPhotoAsync(trustedUsers[userIndex], fileStream);
                }
                Thread.Sleep(500);
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Send(ex.Message);
            }
            Thread.Sleep(500);
        }
        static async void Download(string[] msg)
        {
            string path = msg[1];
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    await Bot.SendDocumentAsync(trustedUsers[userIndex], fileStream);
                }
            }
            catch (Exception ex)
            {
                Send(ex.Message);
            }
        }
        private static async void Upload(string FileName, string FileId)
        {
            var file = await Bot.GetFileAsync(FileId);
            FileStream fs = new FileStream(FileName, FileMode.Create);
            await Bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            fs.Dispose();
        }
    }
}