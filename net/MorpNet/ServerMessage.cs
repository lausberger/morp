using System;
using System.Text;
using System.Collections.Generic;

namespace MorpNet
{
  class ServerMessage
  {
    private static void SendTCPData(int _client, Packet _packet)
    {
      _packet.InsertLengthIndicator();
      Server.clients[_client].tcp.SendPacket(_packet);
    }

    private static async void BroadcastTCPData(Packet _packet, int? _excludedClient = null)
    {
      _packet.InsertLengthIndicator();
      for (int i = 1; i < Server.MaxPlayers; i++)
      {
        if (i != _excludedClient)
        {
          Server.clients[i].tcp.SendPacket(_packet);
        }
      }
    }

    private static void SendUDPData(int _client, Packet _packet)
    {
      _packet.InsertLengthIndicator();
      Server.clients[_client].udp.SendPacket(_packet);
    }

    private static async void BroadCastUDPData(Packet _packet, int? _excludedClient = null)
    {
      _packet.InsertLengthIndicator();
      for (int i = 1; i < Server.MaxPlayers; i++)
      {
        if (i != _excludedClient)
        {
          Server.clients[i].tcp.SendPacket(_packet);
        }
      }
    }

    public static void Welcome(int _client, string _message)
    {
      using (Packet _packet = new Packet((int)ServerPackets.welcome))
      {
        _packet.WriteToBuffer(_message);
        _packet.WriteToBuffer(_client);

        SendTCPData(_client, _packet);
      }
    }

    public static void UDPWelcome(int _client)
    {
      using (Packet _packet = new Packet((int)ServerPackets.udpWelcome))
      {
        _packet.WriteToBuffer("UDP welcome packet!");

        SendUDPData(_client, _packet);
      }
    }
  }
}