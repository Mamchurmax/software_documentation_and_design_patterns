using Lab2.Domain.Interfaces;
using System;

namespace Lab2.PL
{
    public class ConsolePresentationLayer : IPresentationLayer
    {
        public void Run()
        {
            Console.WriteLine("Presentation layer is currently just a placeholder, as per requirements.");
        }
    }
}
