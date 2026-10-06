using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing;

public struct ParticleOrchestraSettings
{
	public Vector2 PositionInWorld;

	public Vector2 MovementVector;

	public int UniqueInfoPiece;

	public byte IndexOfPlayerWhoInvokedThis;

	public void Serialize(BinaryWriter writer)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		writer.WriteVector2(PositionInWorld);
		writer.WriteVector2(MovementVector);
		writer.Write(UniqueInfoPiece);
		writer.Write(IndexOfPlayerWhoInvokedThis);
	}

	public void DeserializeFrom(BinaryReader reader)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		PositionInWorld = reader.ReadVector2();
		MovementVector = reader.ReadVector2();
		UniqueInfoPiece = reader.ReadInt32();
		IndexOfPlayerWhoInvokedThis = reader.ReadByte();
	}
}
