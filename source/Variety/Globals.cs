﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Variety
{
    public static class Globals
    {
        public static string PhpVersion { get; set; }
        public static string HttpServiceName { get; }
        public static string SmtpServiceName { get; }

        static Globals()
        {
            PhpVersion = "php-7.3-ts";
            HttpServiceName = "VarletHttpd";
            SmtpServiceName = "VarletMailhog";
        }

        public static string AppVersion
        {
            get {
                var asm = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return $"{fvi.ProductMajorPart}.{fvi.ProductMinorPart}.{fvi.ProductBuildPart}";
            }
        }

        public static string AppBuildNumber
        {
            get {
                var asm = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return $"{fvi.ProductPrivatePart}";
            }
        }

        public static string AppConfigFile
        {
            get {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return path + @"\varlet.json";
            }
        }
    }
}
