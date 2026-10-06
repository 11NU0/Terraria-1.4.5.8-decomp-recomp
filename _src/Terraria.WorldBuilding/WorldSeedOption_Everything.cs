using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.WorldBuilding;

public class WorldSeedOption_Everything : AWorldGenerationOption
{
	protected List<AWorldGenerationOption> _dependencies;

	protected override string KeyName => "Seed_Everything";

	public override string ServerConfigName => "zenith";

	public List<AWorldGenerationOption> Dependencies
	{
		get
		{
			if (_dependencies == null)
			{
				_dependencies = new List<AWorldGenerationOption>
				{
					WorldGenerationOptions.Get<WorldSeedOption_Remix>(),
					WorldGenerationOptions.Get<WorldSeedOption_Drunk>(),
					WorldGenerationOptions.Get<WorldSeedOption_NotTheBees>(),
					WorldGenerationOptions.Get<WorldSeedOption_NoTraps>(),
					WorldGenerationOptions.Get<WorldSeedOption_DontStarve>(),
					WorldGenerationOptions.Get<WorldSeedOption_Anniversary>(),
					WorldGenerationOptions.Get<WorldSeedOption_ForTheWorthy>()
				};
			}
			return _dependencies;
		}
	}

	public WorldSeedOption_Everything()
	{
		SpecialSeedNames = new string[1] { "getfixedboi" };
		SpecialSeedValues = new int[0];
		AWorldGenerationOption.OnOptionStateChanged += UpdateDependentState;
	}

	private void UpdateDependentState(AWorldGenerationOption changed)
	{
		if (Dependencies.Contains(changed) && changed.Enabled != Enabled)
		{
			Enabled = Dependencies.All((AWorldGenerationOption d) => d.Enabled);
		}
	}

	protected override void OnEnabledStateChanged()
	{
		if (!Enabled && Dependencies.Any((AWorldGenerationOption d) => !d.Enabled))
		{
			return;
		}
		foreach (AWorldGenerationOption dependency in Dependencies)
		{
			dependency.Enabled = Enabled;
		}
	}

	public override UIElement ProvideUIElement()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		UIImageFramed image = new UIImageFramed(Texture, Texture.Frame(7, 16))
		{
			Left = StyleDimension.FromPixels(-1f)
		};
		int glitchFrameCounter = 0;
		int glitchFrame = 0;
		int glitchVariation = 0;
		image.OnUpdate += (UIElement _) =>
		{
			int minValue = 3;
			int num = 3;
			if (glitchFrame == 0)
			{
				minValue = 15;
				num = 120;
			}
			if (++glitchFrameCounter >= Main.rand.Next(minValue, num + 1))
			{
				glitchFrameCounter = 0;
				glitchFrame = (glitchFrame + 1) % 16;
				if ((glitchFrame == 4 || glitchFrame == 8 || glitchFrame == 12) && Main.rand.Next(3) == 0)
				{
					glitchVariation = Main.rand.Next(7);
				}
			}
			image.SetFrame(7, 16, glitchVariation, glitchFrame, 0, 0);
		};
		return image;
	}
}
