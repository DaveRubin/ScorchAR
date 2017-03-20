using System;
using System.Collections.Generic;
using ScorchEngine;
using ScorchEngine.Config;

namespace TestProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GameConfig tempConfig = new GameConfig();
            Game g = new Game(tempConfig);
            g.debugLog += str => Console.WriteLine(str);
            g.AddPlayer(g.self);
            g.AddPlayer(Player.CreateMockPlayer());
            g.StartListening();
            while (true)
            {

            }
        }
    }
}