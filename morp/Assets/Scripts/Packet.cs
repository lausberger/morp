using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ServerPackets
{
    welcome = 1
}

public enum ClientPackets
{
    welcomeReceived = 1
}

public class Packet : IDisposable
{
  private List<byte> buffer;
  private byte[] readableBuffer;
  private int readPos;

  // TODO: consider constructor for int
  public Packet()
  {
    buffer = new List<byte>();
    readPos = 0;
  }

  public Packet(int _id)
  {
    buffer = new List<byte>();
    readPos = 0;

    WriteToBuffer(_id);
  }

  public Packet(byte[] _data)
  {
    buffer = new List<byte>();
    readPos = 0;

    SetBytes(_data);
  }

  public void SetBytes(byte[] _data)
  {
    WriteToBuffer(_data);
    readableBuffer = buffer.ToArray();
  }

  // inserts the size of the payload to the start of the packet
  public void InsertLengthIndicator()
  {
    buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
  }

  public void InsertInt(int _value)
  {
    buffer.InsertRange(0, BitConverter.GetBytes(_value));
  }

  public byte[] ToArray()
  {
    readableBuffer = buffer.ToArray();
    return readableBuffer;
  }

  public int Length()
  {
    return buffer.Count;
  }

  public int UnreadLength()
  {
    return Length() - readPos;
  }

  public void ResetBuffer(bool _shouldReset = true)
  {
    if (_shouldReset)
    {
      buffer.Clear();
      readableBuffer = null;
      readPos = 0;
    }
    else
    {
      readPos -= 4; // Length of an integer
    }
  }

  public void WriteToBuffer(byte _value)
  {
    buffer.Add(_value);
  }

  public void WriteToBuffer(byte[] _data)
  {
    buffer.AddRange(_data);
  }
      
  public void WriteToBuffer(int _data)
  {
    buffer.AddRange(BitConverter.GetBytes(_data));
  }

  public void WriteToBuffer(short _value)
  {
    buffer.AddRange(BitConverter.GetBytes(_value));
  }

  public void WriteToBuffer(long _value)
  {
    buffer.AddRange(BitConverter.GetBytes(_value));
  }

  public void WriteToBuffer(float _value)
  {
    buffer.AddRange(BitConverter.GetBytes(_value));
  }

  public void WriteToBuffer(bool _value)
  {
    buffer.AddRange(BitConverter.GetBytes(_value));
  }

  // append a string size and a string to the payload
  public void WriteToBuffer(string _value)
  {
    WriteToBuffer(_value.Length);
    buffer.AddRange(Encoding.ASCII.GetBytes(_value));
  }

  public byte ReadByte(bool _incrementReadPos = true)
  {
    AssertValidRead();

    byte _value = readableBuffer[readPos];
    if (_incrementReadPos)
    {
      readPos += 1;
    }

    return _value;
  }

  public byte[] ReadBytes(int _length, bool _incrementReadPos = true)
  {
    AssertValidRead();

    byte[] _data = buffer.GetRange(readPos, _length).ToArray();
    if (_incrementReadPos)
    {
      readPos += _length;
    }

    return _data;
  }

  public int ReadInt(bool _incrementReadPos = true)
  {
    AssertValidRead();

    int _value = BitConverter.ToInt32(readableBuffer, readPos);
    if (_incrementReadPos)
    {
      readPos += 4; // length of int
    }
    return _value;
  }

  public short ReadShort(bool _incrementReadPos = true)
  {
    AssertValidRead();

    short _value = BitConverter.ToInt16(readableBuffer, readPos);
    if (_incrementReadPos)
    {
      readPos += 2; // length of int
    }

    return _value;
  }

  public long ReadLong(bool _incrementReadPos = true)
  {
    AssertValidRead();

    long _value = BitConverter.ToInt64(readableBuffer, readPos);
    if (_incrementReadPos)
    {
      readPos += 8; // length of int
    }

    return _value;
  }

  public float ReadFloat(bool _incrementReadPos = true)
  {
    AssertValidRead();

    float _value = BitConverter.ToSingle(readableBuffer, readPos);
    if (_incrementReadPos)
    {
      readPos += 4; // length of int
    }

    return _value;
  }

  public bool ReadBool(bool _incrementReadPos)
  {
    AssertValidRead();

    bool _value = BitConverter.ToBoolean(readableBuffer, readPos);
    if (_incrementReadPos)
    {
      readPos += 1;
    }

    return _value;
  }

  public string ReadString(bool _incrementReadPos = true)
  {
    AssertValidRead();

    try
    {
      int _length = ReadInt();
      string _data = Encoding.ASCII.GetString(readableBuffer, readPos, _length);
      if (_incrementReadPos && _data.Length > 0)
      {
        readPos += _length;
      }

      return _data;
    }
    catch (Exception _e)
    {
      throw new Exception($"Error reading string: {_e}");
    }
  }

  public void AssertValidRead()
  {
    if (readPos > buffer.Count)
    {
      throw new Exception("Read position exceeds buffer length");
    }
  }

  private bool disposed = false;

  protected virtual void Dispose(bool _disposing)
  {
    if (!disposed)
    {
      if (_disposing)
      {
        buffer = null;
        readableBuffer = null;
        readPos = 0;
      }
      disposed = true;
    }
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}