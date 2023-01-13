using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
  public static void Welcome(Packet _packet)
  {
    string _message = _packet.ReadString();
    int _id = _packet.ReadInt();

    Debug.Log($"Received message from server: {_message}");
    Client.instance.myId = _id;
    ClientMessage.WelcomeReceived();

    Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
  }

  public static void UDPWelcome(Packet _packet)
  {
    string _message = _packet.ReadString();

    Debug.Log($"Received message from server via UDP: {_message}");
    ClientMessage.UDPWelcomeReceived();
  }
}
