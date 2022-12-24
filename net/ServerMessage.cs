using System;
using System.Text;
using System.Collections.Generic;

namespace MorpNet
{
  class ServerMessage
  {
    public static void Welcome(int _client, string _message)
    {
      using (Packet _packet = new Packet((int)1))
      {
        _packet.WriteToBuffer(_client);
        _packet.WriteToBuffer(_message);

        SendTCPData(_client, _packet);
      }
    }

    private static void SendTCPData(int _client, Packet _packet)
    {
      _packet.WriteLengthIndicator();
      Server.clients[_client].tcp.SendPacket(_packet);
    }

    private static async void BroadcastTCPData(Packet _packet, int _excludedClient = null)
    {
      _packet.WriteLengthIndicator();
      for (int i=1; i<Server.MaxPlayers; i++)
      {
        if (i != _excludedClient)
        {
          Server.clients[i].tcp.SendPacket(_packet);
        }
      }
    }
  }
}