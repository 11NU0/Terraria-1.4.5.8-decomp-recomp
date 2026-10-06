using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Terraria.Testing;

namespace Terraria.Net.Sockets;

public class DebugNetworkStream
{
	private class Packet
	{
		public DateTime BaseTimestamp;

		public ArraySegment<byte> Data;

		public bool IsReady()
		{
			return BaseTimestamp + TimeSpan.FromMilliseconds(Latency) <= DateTime.Now;
		}

		public static Packet CopyOfSlice(byte[] buffer, int offset, int count)
		{
			byte[] array = new byte[count];
			Array.Copy(buffer, offset, array, 0, count);
			return new Packet
			{
				BaseTimestamp = DateTime.Now,
				Data = new ArraySegment<byte>(array)
			};
		}
	}

	private class CompletedAsyncResult : IAsyncResult
	{
		public object AsyncState { get; set; }

		public int Read { get; set; }

		public bool IsCompleted => true;

		public bool CompletedSynchronously => true;

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}

	private enum ReadMode
	{
		None,
		Direct,
		Buffered
	}

	public static uint Latency;

	private readonly NetworkStream _stream;

	private Queue<Packet> _outgoingQueue = new Queue<Packet>();

	private Queue<Packet> _incomingQueue = new Queue<Packet>();

	private Exception _writeException;

	private Exception _readException;

	private ReadMode _readMode;

	private bool _closed;

	private long _startTicks = Stopwatch.GetTimestamp();

	private BinaryWriter _logWriter;

	private ArraySegment<byte> _beginReadBuf;

	public bool DataAvailable
	{
		get
		{
			lock (_incomingQueue)
			{
				if (_incomingQueue.Count > 0)
				{
					return _incomingQueue.Peek().IsReady();
				}
				if (_readMode == ReadMode.None)
				{
					return _stream.DataAvailable;
				}
				return false;
			}
		}
	}

	public bool HasBufferedData
	{
		get
		{
			lock (_incomingQueue)
			{
				return _incomingQueue.Count > 0 || _stream.DataAvailable;
			}
		}
	}

	public DebugNetworkStream(NetworkStream stream)
	{
		_stream = stream;
	}

	public void BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (_closed)
		{
			throw new ObjectDisposedException("NetworkStream");
		}
		if (_writeException != null)
		{
			throw _writeException;
		}
		lock (_outgoingQueue)
		{
			if (Latency != 0 || _outgoingQueue.Count != 0)
			{
				_outgoingQueue.Enqueue(Packet.CopyOfSlice(buffer, offset, count));
				goto IL_0068;
			}
		}
		_stream.BeginWrite(buffer, offset, count, callback, state);
		return;
		IL_0068:
		callback(new CompletedAsyncResult
		{
			AsyncState = state
		});
	}

	public void EndWrite(IAsyncResult result)
	{
		if (!(result is CompletedAsyncResult))
		{
			_stream.EndWrite(result);
		}
	}

	public void BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (_closed)
		{
			throw new ObjectDisposedException("NetworkStream");
		}
		if (_readException != null)
		{
			throw _readException;
		}
		_beginReadBuf = new ArraySegment<byte>(buffer, offset, count);
		int num;
		lock (_incomingQueue)
		{
			while (_readMode == ReadMode.Buffered && _incomingQueue.Count == 0)
			{
				Monitor.Exit(_incomingQueue);
				Thread.Sleep(1);
				Monitor.Enter(_incomingQueue);
				if (_readException != null)
				{
					throw _readException;
				}
			}
			if (_readMode == ReadMode.None && _incomingQueue.Count == 0)
			{
				_readMode = ReadMode.Direct;
				goto IL_01e3;
			}
			num = 0;
			while (count > 0 && _incomingQueue.Count > 0 && _incomingQueue.Peek().IsReady())
			{
				Packet packet = _incomingQueue.Peek();
				if (packet.Data.Count == 0)
				{
					break;
				}
				int num2 = Math.Min(packet.Data.Count, count);
				Array.Copy(packet.Data.Array, packet.Data.Offset, buffer, offset, num2);
				offset += num2;
				count -= num2;
				num += num2;
				if (num2 == packet.Data.Count)
				{
					_incomingQueue.Dequeue();
				}
				else
				{
					packet.Data = new ArraySegment<byte>(packet.Data.Array, packet.Data.Offset + num2, packet.Data.Count - num2);
				}
			}
			Invariant.Assert(num > 0 || (_incomingQueue.Count == 1 && _incomingQueue.Peek().Data.Count == 0), "Read bytes > 0 || EOS");
		}
		callback(new CompletedAsyncResult
		{
			AsyncState = state,
			Read = num
		});
		return;
		IL_01e3:
		_stream.BeginRead(buffer, offset, count, callback, state);
	}

	public int EndRead(IAsyncResult result)
	{
		if (result is CompletedAsyncResult)
		{
			return ((CompletedAsyncResult)result).Read;
		}
		Invariant.Assert(_readMode == ReadMode.Direct, "DebugNetworkSteam.EndRead, _readMode != Direct");
		_readMode = ReadMode.None;
		return _stream.EndRead(result);
	}

	public void Close()
	{
		_closed = true;
		try
		{
			if (_logWriter != null)
			{
				lock (_logWriter)
				{
					_logWriter.Close();
					return;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void Run()
	{
		byte[] array = null;
		while (!_closed)
		{
			lock (_outgoingQueue)
			{
				while (_writeException == null && _outgoingQueue.Count > 0 && _outgoingQueue.Peek().IsReady())
				{
					BeginBufferedWrite(_outgoingQueue.Dequeue());
				}
			}
			lock (_incomingQueue)
			{
				if (_readMode == ReadMode.None && Latency != 0)
				{
					_readMode = ReadMode.Buffered;
					if (array == null)
					{
						array = new byte[65536];
					}
					BeginBufferedRead(array);
				}
			}
			Thread.Sleep(1);
		}
	}

	private void BeginBufferedWrite(Packet packet)
	{
		try
		{
			_stream.BeginWrite(packet.Data.Array, packet.Data.Offset, packet.Data.Count, BufferedWriteCallback, null);
		}
		catch (Exception writeException)
		{
			_writeException = writeException;
		}
	}

	private void BufferedWriteCallback(IAsyncResult result)
	{
		try
		{
			_stream.EndWrite(result);
		}
		catch (Exception writeException)
		{
			_writeException = writeException;
		}
	}

	private void BeginBufferedRead(byte[] buffer)
	{
		try
		{
			_stream.BeginRead(buffer, 0, buffer.Length, BufferedReadCallback, buffer);
		}
		catch (Exception readException)
		{
			_readException = readException;
		}
	}

	private void BufferedReadCallback(IAsyncResult result)
	{
		int num;
		try
		{
			num = _stream.EndRead(result);
		}
		catch (Exception readException)
		{
			_readException = readException;
			return;
		}
		lock (_incomingQueue)
		{
			byte[] buffer = (byte[])result.AsyncState;
			_incomingQueue.Enqueue(Packet.CopyOfSlice(buffer, 0, num));
			if (num != 0)
			{
				if (Latency == 0)
				{
					_readMode = ReadMode.None;
				}
				else
				{
					BeginBufferedRead(buffer);
				}
			}
		}
	}
}
