using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary;

public static class Filters
{
	public class BySearch : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>, ISearchFilter<BestiaryEntry>
	{
		private string _search;

		public bool? ForcedDisplay => true;

		public bool FitsFilter(BestiaryEntry entry)
		{
			if (_search == null)
			{
				return true;
			}
			BestiaryUICollectionInfo info = entry.UIInfoProvider.GetEntryUICollectionInfo();
			for (int i = 0; i < entry.Info.Count; i++)
			{
				if (entry.Info[i] is IProvideSearchFilterString provideSearchFilterString)
				{
					string searchString = provideSearchFilterString.GetSearchString(ref info);
					if (searchString != null && searchString.ToLower().IndexOf(_search, StringComparison.OrdinalIgnoreCase) != -1)
					{
						return true;
					}
				}
			}
			return false;
		}

		public string GetDisplayNameKey()
		{
			return "BestiaryInfo.IfSearched";
		}

		public UIElement GetImage()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light", (AssetRequestMode)1);
			return new UIImageFramed(val, val.Frame())
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
		}

		public void SetSearch(string searchText)
		{
			_search = searchText;
		}
	}

	public class ByUnlockState : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
	{
		public bool? ForcedDisplay => true;

		public bool FitsFilter(BestiaryEntry entry)
		{
			BestiaryUICollectionInfo entryUICollectionInfo = entry.UIInfoProvider.GetEntryUICollectionInfo();
			return entry.Icon.GetUnlockState(entryUICollectionInfo);
		}

		public string GetDisplayNameKey()
		{
			return "BestiaryInfo.IfUnlocked";
		}

		public UIElement GetImage()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", (AssetRequestMode)1);
			return new UIImageFramed(val, val.Frame(16, 5, 14, 3))
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
		}
	}

	public class ByRareCreature : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
	{
		public bool? ForcedDisplay => null;

		public bool FitsFilter(BestiaryEntry entry)
		{
			for (int i = 0; i < entry.Info.Count; i++)
			{
				if (entry.Info[i] is RareSpawnBestiaryInfoElement)
				{
					return true;
				}
			}
			return false;
		}

		public string GetDisplayNameKey()
		{
			return "BestiaryInfo.IsRare";
		}

		public UIElement GetImage()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light", (AssetRequestMode)1);
			return new UIImageFramed(val, val.Frame())
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
		}
	}

	public class ByBoss : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
	{
		public bool? ForcedDisplay => null;

		public bool FitsFilter(BestiaryEntry entry)
		{
			for (int i = 0; i < entry.Info.Count; i++)
			{
				if (entry.Info[i] is BossBestiaryInfoElement)
				{
					return true;
				}
			}
			return false;
		}

		public string GetDisplayNameKey()
		{
			return "BestiaryInfo.IsBoss";
		}

		public UIElement GetImage()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", (AssetRequestMode)1);
			return new UIImageFramed(val, val.Frame(16, 5, 15, 3))
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
		}
	}

	public class ByInfoElement : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
	{
		private IBestiaryInfoElement _element;

		public bool? ForcedDisplay => null;

		public ByInfoElement(IBestiaryInfoElement element)
		{
			_element = element;
		}

		public bool FitsFilter(BestiaryEntry entry)
		{
			return entry.Info.Contains(_element);
		}

		public string GetDisplayNameKey()
		{
			if (!(_element is IFilterInfoProvider filterInfoProvider))
			{
				return null;
			}
			return filterInfoProvider.GetDisplayNameKey();
		}

		public UIElement GetImage()
		{
			if (!(_element is IFilterInfoProvider filterInfoProvider))
			{
				return null;
			}
			return filterInfoProvider.GetFilterImage();
		}
	}
}
