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
            g.AddPlayer(Player.CreateMockPlayer());
        }
    }
}