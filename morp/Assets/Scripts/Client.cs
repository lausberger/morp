using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
	public static Client instance;
	public static int dataBufferSize = 4096;

	public string ip = "127.0.0.1";
	public int port = 3390;
	public int myId = 0;
	public TCP tcp;

	private delegate void PacketHandler(Packet _packet);
	private static Dictionary<int, PacketHandler> packetHandlers;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Debug.Log("Client instance already exists. Destroying.");
			Destroy(this);
		}
	}

	private void Start()
	{
		tcp = new TCP();
	}

	public void ConnectToServer()
	{
		InitializeClientData();
		tcp.Connect();
	}

	public class TCP
	{
		public TcpClient socket;

		private NetworkStream stream;
		private Packet receivedPacket;
		private byte[] receiveBuffer;

		public void Connect()
		{
			socket = new TcpClient
			{
				ReceiveBufferSize = dataBufferSize,
				SendBufferSize = dataBufferSize
			};

			receiveBuffer = new byte[dataBufferSize];
			socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
		}

		private void ConnectCallback(IAsyncResult _result)
		{
			socket.EndConnect(_result);
			if (!socket.Connected)
			{
				return;
			}

			stream = socket.GetStream();
			receivedPacket = new Packet();
			stream.BeginRead(receiveBuffer, dataBufferSize, 0, ReceiveCallback, null);
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
				Debug.Log($"Error sending data to server (TCP): {_e}");
			}
		}

		private void ReceiveCallback(IAsyncResult _result)
		{
			try
			{
				int _byteLength = stream.EndRead(_result);
				if (_byteLength < 1)
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
				// disconnect
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
						packetHandlers[_packetId](_packet);
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

	private void InitializeClientData()
	{
		packetHandlers = new Dictionary<int, PacketHandler>()
		{
			{ (int)1, ClientHandle.Welcome }
		};
		Debug.Log("Initialized client packets.");
	}
}
