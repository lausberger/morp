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
      readableBuffer = new byte[];
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

    public void WriteToBuffer(string _data)
    {
      WriteLengthIndicator(_data.Length);
      writableBuffer.AddRange(Encoding.ASCII.GetBytes(_data));
    }

    // indicates how long the next set of data is
    public void WriteLengthIndicator(int _length)
    {
      writableBuffer.AddRange(BitConverter.GetBytes(_length));
    }

    // TODO: definitions for other types
    public byte[] ReadBytes(int _numBytes, bool _incrementReadPos = true)
    {
      if (writableBuffer.Count > readPos)
      {
        byte[] _data = writableBuffer.GetRange(readPos, _numBytes).ToArray();
        if (_incrementReadPos)
        {
          readPos += _numBytes;
        }
      }
      else
      {
        throw new Exception("Read position exceeds buffer length!");
      }
    }

    public int ReadInt(bool _incrementReadPos = true)
    {
      if (writableBuffer.Count > readPos)
      {
        int _data = BitConverter.ToInt32(readableBuffer, readPos);
        if (_incrementReadPos)
        {
          readPos += _incrementReadPos;
        }
        return _data;
      }
      else
      {
        throw new Exception("Read position exceeds buffer length!");
      }
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