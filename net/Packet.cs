using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace MorpNet
{
  public class Packet : IDisposable
  {
    private List<byte> writableBuffer;
    private byte[] readableBuffer;
    private int readPos;

    // TODO: consider constructor for int
    public Packet()
    {
      writableBuffer = new List<byte>();
      readPos = 0;
    }

    public Packet(int _id)
    {
      writableBuffer = new List<byte>();
      readPos = 0;

      WriteToBuffer(_id);
    }

    public Packet(byte[] _data)
    {
      writableBuffer = new List<byte>();
      readPos = 0;

      SetBytes(_data);
    }

    public void SetBytes(byte[] _data)
    {
      WriteToBuffer(_data);
      readableBuffer = writableBuffer.ToArray();
    }

    public void InsertInt(int _value)
    {
      writableBuffer.InsertRange(0, BitConverter.GetBytes(_value));
    }

    public void WriteToBuffer(byte[] _data)
    {
      writableBuffer.AddRange(_data);
    }
        
    public void WriteToBuffer(int _data)
    {
      writableBuffer.AddRange(BitConverter.GetBytes(_data));
    }

    // append a string size and a string to the payload
    public void WriteToBuffer(string _data)
    {
      WriteToBuffer(_data.Length);
      writableBuffer.AddRange(Encoding.ASCII.GetBytes(_data));
    }

    // inserts the size of the payload to the start of the packet
    public void InsertLengthIndicator()
    {
      writableBuffer.InsertRange(0, BitConverter.GetBytes(writableBuffer.Count));
    }

    // TODO: definitions for other types
    public byte[] ReadBytes(int _numBytes, bool _incrementReadPos = true)
    {
      AssertValidRead();

      byte[] _data = writableBuffer.GetRange(readPos, _numBytes).ToArray();
      if (_incrementReadPos)
      {
        readPos += _numBytes;
      }
      return _data;
    }

    public int ReadInt(bool _incrementReadPos = true)
    {
      AssertValidRead();
      
      int _data = BitConverter.ToInt32(readableBuffer, readPos);
      if (_incrementReadPos)
      {
        readPos += 4; // length of int
      }
      return _data;
    }

    // possibly need a try-catch
    public string ReadString(bool _incrementReadPos = true)
    {
      AssertValidRead();

      int _length = ReadInt();
      string _data = Encoding.ASCII.GetString(readableBuffer, readPos, _length);
      if (_incrementReadPos && _data.Length > 0)
      {
        readPos += _length;
      }
      return _data;
    }

    public void AssertValidRead()
    {
      if (readPos > writableBuffer.Count)
      {
        throw new Exception("Read position exceeds buffer length");
      }
    }

    public int UnreadLength()
    {
      return Length() - readPos;
    }

    public byte[] ToArray()
    {
      readableBuffer = writableBuffer.ToArray();
      return readableBuffer;
    }

    public int Length()
    {
      return writableBuffer.Count;
    }

    public void ResetBuffer(bool _fullReset = true)
    {
      if (_fullReset)
      {
        writableBuffer.Clear();
        readableBuffer = null;
        readPos = 0;
      }
      else
      {
        readPos -= 4; // Length of an integer
      }
    }

    private bool disposed = false;

    protected virtual void Dispose(bool _disposing)
    {
      if (!disposed)
      {
        if (_disposing)
        {
          writableBuffer = null;
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
}