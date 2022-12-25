using System;
using System.Threading;

namespace MorpNet
{
  class Program
  {
    private static bool running = false;

    private static void Main(string[] args)
    {
      Console.Title = "MorpNet Server";
      running = true;

      Thread mainThread = new Thread(new ThreadStart(MainThread));
      mainThread.Start();

      Server.Start(5, 3390);
    }

    private static void MainThread()
    {
      Console.WriteLine($"Main thread started. Tick rate: {Constants.TICKS_PER_SEC}");
      DateTime _nextLoop = DateTime.Now;

      while (running)
      {
        while (_nextLoop < DateTime.Now);
        {
          GameLogic.Update();
          _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
          if (_nextLoop > DateTime.Now)
          {
            Thread.Sleep(_nextLoop - DateTime.Now);
          }
        }
      }
    }
  }
}