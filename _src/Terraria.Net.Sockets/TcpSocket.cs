using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ReLogic.OS;
using Terraria.Testing;

namespace Terraria.Net.Sockets;

public class TcpSocket : ISocket
{
	private TcpClient _connection;

	private TcpListener _listener;

	private SocketConnectionAccepted _listenerCallback;

	private RemoteAddress _remoteAddress;

	private bool _isListening;

	private DebugNetworkStream _debugStream;

	private DebugNetworkStream GetStream()
	{
		if (_debugStream == null)
		{
			return _debugStream = new DebugNetworkStream(_connection.GetStream());
		}
		return _debugStream;
	}

	public TcpSocket()
	{
		_connection = new TcpClient
		{
			NoDelay = true
		};
	}

	public TcpSocket(TcpClient tcpClient)
	{
		_connection = tcpClient;
		_connection.NoDelay = true;
		IPEndPoint iPEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
		_remoteAddress = new TcpAddress(iPEndPoint.Address, iPEndPoint.Port);
	}

	void ISocket.Close()
	{
		_remoteAddress = null;
		_connection.Close();
		if (_debugStream != null)
		{
			_debugStream.Close();
		}
	}

	bool ISocket.IsConnected()
	{
		if (_connection == null || _connection.Client == null)
		{
			return false;
		}
		if (!_connection.Connected)
		{
			return false;
		}
		if (GetStream().HasBufferedData)
		{
			return true;
		}
		return !_connection.Client.Poll(0, SelectMode.SelectRead) || _connection.Client.Available != 0;
	}

	void ISocket.Connect(RemoteAddress address)
	{
		Invariant.Assert(address.Type == AddressType.Tcp, "TcpSocket can only access a TcpAddress as a remote address.");
		TcpAddress tcpAddress = (TcpAddress)address;
		_connection.Connect(tcpAddress.Address, tcpAddress.Port);
		_remoteAddress = address;
	}

	private void ReadCallback(IAsyncResult result)
	{
		Tuple<SocketReceiveCallback, object> tuple = (Tuple<SocketReceiveCallback, object>)result.AsyncState;
		try
		{
			tuple.Item1(tuple.Item2, GetStream().EndRead(result));
		}
		catch (Exception)
		{
			((ISocket)this).Close();
			tuple.Item1(tuple.Item2, 0);
		}
	}

	private void SendCallback(IAsyncResult result)
	{
		Tuple<SocketSendCallback, object> tuple;
		if (Platform.IsWindows)
		{
			tuple = (Tuple<SocketSendCallback, object>)result.AsyncState;
		}
		else
		{
			object[] array = (object[])result.AsyncState;
			LegacyNetBufferPool.ReturnBuffer((byte[])array[1]);
			tuple = (Tuple<SocketSendCallback, object>)array[0];
		}
		try
		{
			GetStream().EndWrite(result);
			tuple.Item1(tuple.Item2);
		}
		catch (Exception)
		{
			((ISocket)this).Close();
		}
	}

	void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
	{
		if (!Platform.IsWindows)
		{
			byte[] array = LegacyNetBufferPool.RequestBuffer(data, offset, size);
			GetStream().BeginWrite(array, 0, size, SendCallback, new object[2]
			{
				new Tuple<SocketSendCallback, object>(callback, state),
				array
			});
		}
		else
		{
			GetStream().BeginWrite(data, 0, size, SendCallback, new Tuple<SocketSendCallback, object>(callback, state));
		}
	}

	void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
	{
		GetStream().BeginRead(data, offset, size, ReadCallback, new Tuple<SocketReceiveCallback, object>(callback, state));
	}

	bool ISocket.IsDataAvailable()
	{
		if (!_connection.Connected)
		{
			return false;
		}
		return GetStream().DataAvailable;
	}

	RemoteAddress ISocket.GetRemoteAddress()
	{
		return _remoteAddress;
	}

	bool ISocket.StartListening(SocketConnectionAccepted callback)
	{
		IPAddress address = IPAddress.Any;
		if (Program.LaunchParameters.TryGetValue("-ip", out var value) && !IPAddress.TryParse(value, out address))
		{
			address = IPAddress.Any;
		}
		_isListening = true;
		_listenerCallback = callback;
		if (_listener == null)
		{
			_listener = new TcpListener(address, Netplay.ListenPort);
		}
		try
		{
			_listener.Start();
		}
		catch (Exception)
		{
			return false;
		}
		Thread thread = new Thread(ListenLoop);
		thread.IsBackground = true;
		thread.Name = "TCP Listen Thread";
		thread.Start();
		return true;
	}

	void ISocket.StopListening()
	{
		_isListening = false;
	}

	private void ListenLoop()
	{
		while (_isListening && !Netplay.Disconnect)
		{
			try
			{
				ISocket client = new TcpSocket(_listener.AcceptTcpClient());
				_listenerCallback(client);
			}
			catch (Exception)
			{
			}
		}
		_listener.Stop();
	}
}
