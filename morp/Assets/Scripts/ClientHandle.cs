using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
  public static void Welcome(Packet _packet)
  {
    int _id = _packet.ReadInt();
    string _message = _packet.ReadString();

    Debug.Log($"Received message from server: {_message}");
    Client.instance.myId = _id;
    // respond to welcome here
    ClientMessage.WelcomeReceived();
  }
}
