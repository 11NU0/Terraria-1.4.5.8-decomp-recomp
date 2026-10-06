using System;
using System.IO;
using Terraria.Net.Sockets;

namespace Terraria;

public class RemoteServer
{
	public ISocket Socket = new TcpSocket();

	public bool IsActive;

	public volatile int State;

	public int TimeOutTimer;

	public bool PendingTermination;

	public bool IsReading;

	public byte[] ReadBuffer;

	public string StatusText;

	public int StatusCount;

	public int StatusMax;

	public BitsByte ServerSpecialFlags;

	public bool HideStatusTextPercent => ServerSpecialFlags[0];

	public bool StatusTextHasShadows => ServerSpecialFlags[1];

	public bool ReadBufferFull => NetMessage.buffer[256].RemainingReadBufferLength < ReadBuffer.Length;

	public void ResetSpecialFlags()
	{
		ServerSpecialFlags = (byte)0;
	}

	public bool IsConnected()
	{
		if (!PendingTermination)
		{
			return Socket.IsConnected();
		}
		return false;
	}

	public void ClientWriteCallBack(object state)
	{
		NetMessage.buffer[256].spamCount--;
	}

	public void ClientReadCallBack(object state, int streamLength)
	{
		try
		{
			if (!Netplay.Disconnect)
			{
				if (streamLength == 0)
				{
					PendingTermination = true;
				}
				else
				{
					try
					{
						NetMessage.ReceiveBytes(ReadBuffer, streamLength);
					}
					catch
					{
						if (!Main.ignoreErrors)
						{
							throw;
						}
					}
				}
			}
			IsReading = false;
		}
		catch (Exception value)
		{
			try
			{
				using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
				streamWriter.WriteLine(DateTime.Now);
				streamWriter.WriteLine(value);
				streamWriter.WriteLine("");
			}
			catch
			{
			}
			Netplay.Disconnect = true;
		}
	}
}
