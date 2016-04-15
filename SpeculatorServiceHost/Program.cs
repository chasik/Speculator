﻿using System.ServiceProcess;

namespace SpeculatorServiceHost
{
    static class Program
    {
        public static void Main()
        {
            var servicesToRun = new ServiceBase[] {new SmartComDataServiceHost()};
            ServiceBase.Run(servicesToRun);
        }
    }
}
