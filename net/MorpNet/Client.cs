using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MorpNet
{
  class Client
  {
    public static int dataBufferSize = 4096;
    public int id;
    public TCP tcp;

    public Client(int _clientId)
    {
      id = _clientId;
      tcp = new TCP(id);
    }

    public class TCP
    {
      public TcpClient socket;

      private readonly int id;
      private NetworkStream stream;
      private Packet receivedPacket;
      private byte[] receiveBuffer;
      
      public TCP(int _id)
      {
        id = _id;
      }

      public void Connect(TcpClient _socket)
      {
        socket = _socket;
        socket.ReceiveBufferSize = dataBufferSize;
        socket.SendBufferSize = dataBufferSize;

        stream = socket.GetStream();

        receivedPacket = new Packet();
        receiveBuffer = new byte[dataBufferSize];

        stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

        ServerMessage.Welcome(id, "Welcome to MorpNet!");
      }

      public void SendPacket(Packet _packet)
      {
        try
        {
          if (socket != null)
          {
            stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
          }
        }
        catch (Exception _e)
        {
          Console.WriteLine($"Error sending packet to player {id} via TCP: {_e}");
        }
      }

      private void ReceiveCallback(IAsyncResult _result)
      {
        try
        {
          int _byteLength = stream.EndRead(_result);
          if (_byteLength <= 0)
          {
            // TODO: disconnect
            return;
          }
          
          byte[] _data = new byte[_byteLength];
          Array.Copy(receiveBuffer, _data, _byteLength);

          receivedPacket.ResetBuffer(HandleData(_data));
          stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }
        catch (Exception _e)
        {
          Console.WriteLine($"Error receiving TCP data: {_e}");
        }
      }

      private bool HandleData(byte[] _data)
      {
        int _packetLength = 0;

        receivedPacket.SetBytes(_data);
        if (receivedPacket.UnreadLength() >= 4)
        {
          _packetLength = receivedPacket.ReadInt();
          if (_packetLength <= 0)
          {
            return true;
          }
        }

        while (_packetLength > 0 && _packetLength <= receivedPacket.UnreadLength())
        {
          byte[] _packetBytes = receivedPacket.ReadBytes(_packetLength);
          ThreadManager.ExecuteOnMainThread(() =>
          {
            using (Packet _packet = new Packet(_packetBytes))
            {
              int _packetId = _packet.ReadInt();
              Server.packetHandlers[_packetId](id, _packet);
            }
          });

          _packetLength = 0;
          if (receivedPacket.UnreadLength() >= 4)
          {
            _packetLength = receivedPacket.ReadInt();
            if (_packetLength <= 0)
            {
              return true;
            }
          }
        }

        if (_packetLength <= 1)
        {
          return true;
        }

        return false;
      }
    }
  }
}