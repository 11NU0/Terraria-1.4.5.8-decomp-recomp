using System;
using System.IO;
using System.Threading;
using Steamworks;

namespace Terraria.Social.Steam;

public class SteamFileReadStream : Stream
{
	private readonly string _path;

	private int? _length;

	private uint _position;

	private CallResult<RemoteStorageFileReadAsyncComplete_t> _callback = new CallResult<RemoteStorageFileReadAsyncComplete_t>((CallResult<RemoteStorageFileReadAsyncComplete_t>.APIDispatchDelegate)null);

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			if (!_length.HasValue)
			{
				_length = SteamRemoteStorage.GetFileSize(_path);
			}
			return _length.Value;
		}
	}

	public override long Position
	{
		get
		{
			return _position;
		}
		set
		{
			Seek(value, SeekOrigin.Begin);
		}
	}

	public SteamFileReadStream(string path)
	{
		_path = path;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset != 0)
		{
			throw new NotSupportedException("offset != 0");
		}
		if (count < 0 || count > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count == 0)
		{
			return 0;
		}
		if (_position >= Length)
		{
			return 0;
		}
		SteamAPICall_t val = SteamRemoteStorage.FileReadAsync(_path, _position, (uint)count);
		if (val == SteamAPICall_t.Invalid)
		{
			throw new IOException("FileReadAsync call invalid");
		}
		Tuple<int, Exception> ret = null;
		_callback.Set(val, (CallResult<RemoteStorageFileReadAsyncComplete_t>.APIDispatchDelegate)((RemoteStorageFileReadAsyncComplete_t result, bool bIOFailure) =>
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Invalid comparison between Unknown and I4
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (bIOFailure)
				{
					throw new IOException("Steam IO Failure");
				}
				if ((int)result.m_eResult != 1)
				{
					throw new IOException("Steam IO Failure: " + result.m_eResult);
				}
				if (!SteamRemoteStorage.FileReadAsyncComplete(result.m_hFileReadAsync, buffer, result.m_cubRead))
				{
					throw new IOException("FileReadAsyncComplete failed");
				}
				ret = new Tuple<int, Exception>((int)result.m_cubRead, null);
			}
			catch (Exception item)
			{
				ret = new Tuple<int, Exception>(0, item);
			}
		}));
		while (ret == null)
		{
			CoreSocialModule.Pulse();
			Thread.Sleep(0);
		}
		if (ret.Item2 != null)
		{
			throw new IOException("Exception in Read", ret.Item2);
		}
		_position += (uint)ret.Item1;
		return ret.Item1;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		long num = origin switch
		{
			SeekOrigin.Begin => offset, 
			SeekOrigin.Current => _position + offset, 
			_ => Length - offset, 
		};
		if (num < 0 || num > uint.MaxValue)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		_position = (uint)num;
		return _position;
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_callback.Dispose();
		}
	}
}
