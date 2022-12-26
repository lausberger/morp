using System;
using System.Collections.Generic;
using System.Text;

namespace MorpNet
{
  class ServerHandle
  {
    public static void WelcomeReceived(int _client, Packet _packet)
    {
      int _id = _packet.ReadInt();
      string _username = _packet.ReadString();

      Console.WriteLine($"{Server.clients[_client].tcp.socket.Client.RemoteEndPoint} connected successfully as player {_client}.");
      if (_client != _id)
      {
        Console.WriteLine($"Player \"{_username}\" (ID: {_client}) has assumed incorrect client ID: {_id}!");
      }

      // send player into game here
    }
  }
}