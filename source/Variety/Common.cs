﻿using System;
using System.Diagnostics;
using System.IO;
using static System.String;

namespace Variety
{
    public class Common
    {
        /// <summary>
        /// Print text with color
        /// </summary>
        public void PrintSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(value);
            Console.ResetColor();
        }

        public void PrintInfo(string value)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(value);
            Console.ResetColor();
        }

        public void PrintWarning(string value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(value);
            Console.ResetColor();
        }

        public void PrintError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(value);
            Console.ResetColor();
        }

        /// <summary>
        /// Print line with color
        /// </summary>
        public void PrintlnSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }

        public void PrintlnInfo(string value)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }

        public void PrintlnWarning(string value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }

        public void PrintlnError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }

         public static void OpenUrl(string url)
         {
             try {
                 Process.Start(url);
             } catch (FormatException) {
                 // do something here
             }
         }

         public static void OpenWithNotepad(string file, bool runas = false)
         {
             try {
                 var proc = new Process {StartInfo =
                 {
                     FileName = "notepad.exe",
                     Arguments = file,
                     UseShellExecute = true
                 }};
                 if (runas == true) proc.StartInfo.Verb = "runas";
                 proc.Start();
             } catch (FormatException) {
                 // do something here
             }
         }

         public static string GetAppPath(string path = "")
         {
             var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
             if (!IsNullOrEmpty(path)) {
                 return basePath + path;
             } else {
                return basePath;
             }
         }

         public static string DirProgramFiles(string path)
         {
             if( 8 == IntPtr.Size  || (!IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))  {
                 return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
             }

             if (string.IsNullOrEmpty(path)) {
                 return Environment.GetEnvironmentVariable("ProgramFiles") + @path;
             } else {
                 return Environment.GetEnvironmentVariable("ProgramFiles");
             }
         }
    }
}
