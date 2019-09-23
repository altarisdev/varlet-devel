﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace VarletCli
{
    /**
      * Credit:
      *  https://samyn.co/post/structuring-neat-net-core-console-apps/
     */
    public class Program
    {
        public static int Main(string[] args)
        {
            var options = CommandLineOptions.Parse(args);

            if (options?.Command == null)
            {
                // RootCommand will have printed help
                return 1;
            }

            return options.Command.Run();

        }
    }
}
