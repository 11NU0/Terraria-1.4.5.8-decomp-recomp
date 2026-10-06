using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat;

public class PlainTagHandler : ITagHandler
{
	public class PlainSnippet : TextSnippet
	{
		public PlainSnippet(string text = "")
			: base(text)
		{
		}

		public PlainSnippet(string text, Color color)
			: base(text, color)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		}

		public override Color GetVisibleColor()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Color;
		}
	}

	TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
	{
		return new PlainSnippet(text);
	}
}
