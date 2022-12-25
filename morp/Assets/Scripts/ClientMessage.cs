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

  public static void WelcomeReceived()
  {
    using (Packet _packet = new Packet((int)1))
    {
      _packet.WriteToBuffer(Client.instance.myId);
      _packet.WriteToBuffer(UIManager.instance.usernameField.text);

      SendTCPData(_packet);
    }
  }
}
