using Terraria.Localization;

namespace Terraria.GameContent.UI;

public class CharacterCreationTipsProvider : ITipProvider
{
	public LocalizedText RollAvailableTip()
	{
		return Language.SelectRandom(Lang.CreateDialogFilter("LoadingTips_CharacterCreation."));
	}
}
