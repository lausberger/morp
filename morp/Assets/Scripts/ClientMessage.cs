using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMessage : MonoBehaviour
{
  private static void SendTCPData(Packet _packet)
  {
    _packet.InsertLengthIndicator();
    Client.instance.tcp.SendPacket(_packet);
  }

  private static void SendUDPData(Packet _packet)
  {
    _packet.InsertLengthIndicator();
    Client.instance.udp.SendPacket(_packet);
  }

  public static void WelcomeReceived()
  {
    using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
    {
      _packet.WriteToBuffer(Client.instance.myId);
      _packet.WriteToBuffer(UIManager.instance.usernameField.text);

      SendTCPData(_packet);
    }
  }

  public static void UDPWelcomeReceived()
  {
    using (Packet _packet = new Packet((int)ClientPackets.udpWelcomeReceived))
    {
      _packet.WriteToBuffer("Received UDP Welcome successfully");

      SendUDPData(_packet);
    }
  }
}
