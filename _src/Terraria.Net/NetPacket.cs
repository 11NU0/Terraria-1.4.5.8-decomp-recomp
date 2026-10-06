using System;
using System.IO;
using Terraria.DataStructures;

namespace Terraria.Net;

public struct NetPacket
{
	public const int HEADER_SIZE = 5;

	public readonly ushort Id;

	public readonly CachedBuffer Buffer;

	public int Length { get; private set; }

	public BinaryWriter Writer => Buffer.Writer;

	public BinaryReader Reader => Buffer.Reader;

	public NetPacket(ushort id, int size)
	{
		this = default;
		Id = id;
		Length = size + 5;
		if (Length > 65535)
		{
			throw new ArgumentOutOfRangeException("Tried to create a packet with length > " + ushort.MaxValue);
		}
		Buffer = BufferPool.Request(Length);
		Writer.Write((ushort)Length);
		Writer.Write((byte)82);
		Writer.Write(id);
	}

	public void Recycle()
	{
		Buffer.Recycle();
	}

	public void ShrinkToFit()
	{
		if (Length != (int)Writer.BaseStream.Position)
		{
			if (Writer.BaseStream.Position > Length)
			{
				throw new IndexOutOfRangeException("Overwrite on supplied Length. Consider letting Length default to max packet size if you don't know how long it will be");
			}
			Length = (int)Writer.BaseStream.Position;
			Writer.Seek(0, SeekOrigin.Begin);
			Writer.Write((ushort)Length);
			Writer.Seek(Length, SeekOrigin.Begin);
		}
	}
}
