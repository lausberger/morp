using System;

namespace MorpNet
{
  class Program
  {
    private static void Main(string[] args)
    {
      Console.Title = "MorpNet Server";

      Server.Start(5, 3390);

      Console.ReadKey();
    }
  }
}