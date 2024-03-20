﻿using System;
using System.ServiceModel;
using WCF_Library_Server;

namespace ConsoleServer
{
    class Program
    {
        static void Main()
        {
            try
            {
                using (var host = new ServiceHost(typeof(WCF_Library_Server.WCF_Service)))
                {
                host.Open();
                Console.WriteLine("Server has been started...");
                Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

