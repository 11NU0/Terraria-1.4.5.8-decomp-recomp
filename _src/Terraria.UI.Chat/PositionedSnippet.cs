using Microsoft.Xna.Framework;

namespace Terraria.UI.Chat;

public struct PositionedSnippet(TextSnippet snippet, int origIndex, int line, Vector2 position, Vector2 size)
{
	public readonly TextSnippet Snippet = snippet;

	public readonly int OrigIndex = origIndex;

	public readonly int Line = line;

	public Vector2 Position = position;

	public Vector2 Size = size;

	public void Scale(float scale)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Position *= scale;
		Size *= scale;
	}
}
