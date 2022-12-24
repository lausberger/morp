using System;
using System.Text;
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

    // TODO: add definition that takes other data types
    public void WriteToBuffer(byte[] _data)
    {
      writableBuffer.AddRange(_data);
    }

    // append a string size and a string to the payload
    public void WriteToBuffer(string _data)
    {
      WriteToBuffer(_data.Length);
      writableBuffer.AddRange(Encoding.ASCII.GetBytes(_data));
    }

    // inserts the size of the payload to the start of the packet
    public void WriteLengthIndicator()
    {
      writableBuffer.InsertRange(0, BitConverter.GetBytes(writableBuffer.Count));
    }

    // TODO: definitions for other types
    public byte[] ReadBytes(int _numBytes, bool _incrementReadPos = true)
    {
      if (readPos > writableBuffer.Count)
      {
        throw new Exception("Read position exceeds buffer length!");
      }

      byte[] _data = writableBuffer.GetRange(readPos, _numBytes).ToArray();
      if (_incrementReadPos)
      {
        readPos += _numBytes;
      }
      return _data;
    }

    public int ReadInt(bool _incrementReadPos = true)
    {
      if (readPos > writableBuffer.Count)
      {
        throw new Exception("Read position exceeds buffer length!");
      }
      
      int _data = BitConverter.ToInt32(readableBuffer, readPos);
      if (_incrementReadPos)
      {
        readPos += 4; // length of int
      }
      return _data;
    }

    public int UnreadLength()
    {
      return writableBuffer.Count - readPos;
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