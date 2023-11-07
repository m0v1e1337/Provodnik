using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleApp1;
namespace ConsoleApp24
{
    class Program
    {
        static void Main(string[] args)
        {
            FileManager manager = new FileManager();
            manager.Explorer();
            Console.Clear();
            Console.WriteLine("Работа программы завершена");
        }
    }
}