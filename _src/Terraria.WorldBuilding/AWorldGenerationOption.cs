using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.WorldBuilding;

public abstract class AWorldGenerationOption
{
	private bool _enabled;

	public bool AutoGenEnabled;

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (_enabled != value)
			{
				_enabled = value;
				OnEnabledStateChanged();
				OnOptionStateChanged(this);
			}
		}
	}

	protected abstract string KeyName { get; }

	public abstract string ServerConfigName { get; }

	public string[] SpecialSeedNames { get; protected set; }

	public int[] SpecialSeedValues { get; protected set; }

	public LocalizedText Description { get; private set; }

	public LocalizedText Title { get; private set; }

	protected Asset<Texture2D> Texture { get; private set; }

	protected static event Action<AWorldGenerationOption> OnOptionStateChanged;

	protected virtual void OnEnabledStateChanged()
	{
	}

	public void Load()
	{
		if (Texture == null)
		{
			Description = Language.GetText("UI." + KeyName);
			Title = Language.GetText("UI." + KeyName + "_Title");
			if (!Main.dedServ)
			{
				Texture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/" + KeyName, (AssetRequestMode)1);
			}
		}
	}

	public virtual UIElement ProvideUIElement()
	{
		return new UIImage(Texture)
		{
			Left = StyleDimension.FromPixels(-1f)
		};
	}
}
