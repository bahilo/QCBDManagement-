using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Classes
{
    public static class Log
    {
        static string directory;
        static string fileName;
        static string fileFullPath;
        static object _lock = new object();
                
        public static void initialize()
        {
            fileName = "log_"+DateTime.Now.ToString("yyyy_MM")+".txt";
            directory = Directory.GetCurrentDirectory()+@"\Logs";
            fileFullPath = string.Format(@"{0}\{1}", directory, fileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(fileFullPath))
                File.Create(fileFullPath);
        }

        public static void write(string message, string messageType, [CallerMemberName] string callerName = null)
        {
            initialize();
            lock(_lock)
                File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), messageType, callerName, message) });
        }
    }
}
