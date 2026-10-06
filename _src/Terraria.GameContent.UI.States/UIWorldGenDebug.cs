using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using ReLogic.Content;
using ReLogic.Graphics;
using ReLogic.Threading;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Testing;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.UI.States;

public class UIWorldGenDebug : UIState
{
	private class TooltipElement : UIElement
	{
		private Func<string> _getTitle;

		private Func<string> _getDescription;

		public TooltipElement(Func<string> getTitle, Func<string> getDescription = null)
		{
			_getTitle = getTitle;
			_getDescription = getDescription;
			IgnoresMouseInteraction = true;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (Parent.IsMouseHovering)
			{
				string nameOverride = _getTitle();
				string text = ((_getDescription == null) ? null : _getDescription());
				if (text == null)
				{
					text = string.Empty;
				}
				Item item = Main.DisplayAndGetFakeItem(ItemRarityColor.StrongRed10);
				item.SetNameOverride(nameOverride);
				item.ToolTip = ItemTooltip.FromHardcodedText(text);
			}
		}
	}

	private class Config
	{
		private static readonly string FilePath = Path.Combine(Main.SavePath, "dev-worldgen.json");

		public static Config Instance = new Config();

		public HashSet<string> HighlightedPassNames = new HashSet<string>();

		public static void Save()
		{
			File.WriteAllText(FilePath, JsonConvert.SerializeObject((object)Instance));
		}

		public static void Load()
		{
			try
			{
				if (File.Exists(FilePath))
				{
					Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(FilePath));
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private class UIImageButtonWithExtraIcon(Asset<Texture2D> texture, Rectangle? frame = null) : UIImageButton(texture, frame)
	{
		private Rectangle? _iconFrame;

		private Asset<Texture2D> _iconTexture;

		public Color IconColor = Color.White;

		public Texture2D Icon
		{
			get
			{
				if (_iconTexture == null)
				{
					return null;
				}
				return _iconTexture.Value;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			base.DrawSelf(spriteBatch);
			if (_iconTexture == null)
			{
				return;
			}
			Rectangle val = GetDimensions().ToRectangle();
			val.Inflate(-2, -2);
			int width;
			int height;
			if (_iconFrame.HasValue)
			{
				width = _iconFrame.Value.Width;
				height = _iconFrame.Value.Height;
			}
			else
			{
				width = _iconTexture.Value.Width;
				height = _iconTexture.Value.Height;
			}
			if (width != height)
			{
				if (width < height)
				{
					float num = (float)width / (float)height;
					int num2 = val.Width - (int)((float)val.Width * num);
					val.Width -= num2;
					val.X += num2 / 2;
				}
				else
				{
					float num3 = (float)height / (float)width;
					int num4 = val.Height - (int)((float)val.Height * num3);
					val.Height -= num4;
					val.Y += num4 / 2;
				}
			}
			spriteBatch.Draw(_iconTexture.Value, val, _iconFrame, IconColor * (IsMouseHovering ? _visibilityActive : _visibilityInactive));
		}

		public void SetIcon(string iconTexturePath)
		{
			if (iconTexturePath != null)
			{
				_iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, (AssetRequestMode)1);
			}
			else
			{
				_iconTexture = null;
			}
		}

		public void SetIconFrame(Rectangle region)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_iconFrame = region;
		}
	}

	private class GenPassElement : UIPanel
	{
		internal struct PassIconEntry
		{
			internal string Icon;

			internal Rectangle Region;

			internal int Width;

			internal int Height;

			internal static PassIconEntry FromBestiaryIcon(int index)
			{
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				string text = "Images/UI/Bestiary/Icon_Tags_Shadow";
				Asset<Texture2D> tex = Main.Assets.Request<Texture2D>(text, (AssetRequestMode)1);
				return new PassIconEntry
				{
					Icon = text,
					Region = tex.Frame(16, 5, index % 16, index / 16),
					Width = 26,
					Height = 26
				};
			}

			internal static PassIconEntry FromItem(int index)
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				string text = "Images/Item_" + index;
				Asset<Texture2D> val = Main.Assets.Request<Texture2D>(text, (AssetRequestMode)1);
				Rectangle val2 = val.Frame();
				int num = ((val2.Width > val2.Height) ? val2.Width : val.Height());
				float num2 = 20f / (float)num;
				if (num2 > 1.2f)
				{
					num2 = 1.2f;
				}
				return new PassIconEntry
				{
					Icon = text,
					Region = val2,
					Width = (int)((float)val2.Width * num2),
					Height = (int)((float)val2.Height * num2)
				};
			}

			internal static PassIconEntry FromImageFrame(string image, int index, int rowCount, int lineCount)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				Asset<Texture2D> val = Main.Assets.Request<Texture2D>(image, (AssetRequestMode)1);
				Rectangle val2 = val.Frame(rowCount, lineCount, index % rowCount, index / rowCount);
				int num = ((val2.Width > val2.Height) ? val2.Width : val.Height());
				float num2 = 20f / (float)num;
				if (num2 > 1.2f)
				{
					num2 = 1.2f;
				}
				return new PassIconEntry
				{
					Icon = image,
					Region = val2,
					Width = (int)((float)val2.Width * num2),
					Height = (int)((float)val2.Height * num2)
				};
			}
		}

		public readonly GenPass Pass;

		private bool Hovered;

		private bool SuppressPassRightClick;

		private static Dictionary<string, PassIconEntry> passIcons = new Dictionary<string, PassIconEntry>();

		public int Index => Controller.Passes.IndexOf(Pass);

		public bool IsRunning => Controller.CurrentPass == Pass;

		public bool HasCompleted => WorldGen.Manifest.GenPassResults.Count > Index;

		public bool Skipped
		{
			get
			{
				if (HasCompleted)
				{
					return WorldGen.Manifest.GenPassResults[Index].Skipped;
				}
				return false;
			}
		}

		public WorldGenSnapshot Snapshot => Controller.GetSnapshot(Pass);

		public bool IsPausedAfterThisPass
		{
			get
			{
				if (CanSubmitActions && HasCompleted && !Skipped)
				{
					return WorldGen.Manifest.GenPassResults.Count == Index + 1;
				}
				return false;
			}
		}

		public bool IsHighlighted => Config.Instance.HighlightedPassNames.Contains(Pass.Name);

		private UIImageButtonWithExtraIcon AddButton(string assetPath, string iconAsset, float x, float y, Action onClick, Func<string> getTitle, Func<string> getDescription = null)
		{
			UIImageButtonWithExtraIcon uIImageButtonWithExtraIcon = new UIImageButtonWithExtraIcon(Main.Assets.Request<Texture2D>(assetPath, (AssetRequestMode)1))
			{
				Left = StyleDimension.FromPixelsAndPercent(x, 0f),
				Top = StyleDimension.FromPixelsAndPercent(y, 0f)
			};
			if (!string.IsNullOrEmpty(iconAsset))
			{
				uIImageButtonWithExtraIcon.SetIcon(iconAsset);
			}
			uIImageButtonWithExtraIcon.OnLeftClick += (UIMouseEvent evt, UIElement e) =>
			{
				onClick();
			};
			if (getTitle != null)
			{
				uIImageButtonWithExtraIcon.Append(new TooltipElement(getTitle, getDescription));
			}
			Append(uIImageButtonWithExtraIcon);
			return uIImageButtonWithExtraIcon;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			RefreshColors();
			base.DrawSelf(spriteBatch);
		}

		private static void InitPassIcons()
		{
			passIcons.Add(GenPassNameID.Terrain, PassIconEntry.FromBestiaryIcon(1));
			passIcons.Add(GenPassNameID.Skyblock, PassIconEntry.FromBestiaryIcon(26));
			passIcons.Add(GenPassNameID.DunesAndPyramidLocations, PassIconEntry.FromBestiaryIcon(4));
			passIcons.Add(GenPassNameID.OceanSand, PassIconEntry.FromBestiaryIcon(28));
			passIcons.Add(GenPassNameID.SandPatches, PassIconEntry.FromItem(169));
			passIcons.Add(GenPassNameID.Tunnels, PassIconEntry.FromItem(4501));
			passIcons.Add(GenPassNameID.MountainCaves, PassIconEntry.FromItem(4510));
			passIcons.Add(GenPassNameID.DirtWallBackgrounds, PassIconEntry.FromItem(30));
			passIcons.Add(GenPassNameID.RocksInDirt, PassIconEntry.FromItem(3));
			passIcons.Add(GenPassNameID.DirtInRocks, PassIconEntry.FromItem(2));
			passIcons.Add(GenPassNameID.Clay, PassIconEntry.FromItem(133));
			passIcons.Add(GenPassNameID.SmallHoles, PassIconEntry.FromItem(4538));
			passIcons.Add(GenPassNameID.DirtLayerCaves, PassIconEntry.FromItem(4510));
			passIcons.Add(GenPassNameID.RockLayerCaves, PassIconEntry.FromItem(4512));
			passIcons.Add(GenPassNameID.SurfaceCaves, PassIconEntry.FromItem(4501));
			passIcons.Add(GenPassNameID.WavyCaves, PassIconEntry.FromItem(4537));
			passIcons.Add(GenPassNameID.IceBiome, PassIconEntry.FromBestiaryIcon(6));
			passIcons.Add(GenPassNameID.Grass, PassIconEntry.FromImageFrame("Images/Tiles_3", 5, 45, 1));
			passIcons.Add(GenPassNameID.Jungle, PassIconEntry.FromBestiaryIcon(22));
			passIcons.Add(GenPassNameID.MudCavesToJungleGrass, PassIconEntry.FromItem(745));
			passIcons.Add(GenPassNameID.DesertBiome, PassIconEntry.FromBestiaryIcon(3));
			passIcons.Add(GenPassNameID.GlowingMushroomPatches, PassIconEntry.FromBestiaryIcon(24));
			passIcons.Add(GenPassNameID.Marble, PassIconEntry.FromBestiaryIcon(29));
			passIcons.Add(GenPassNameID.Granite, PassIconEntry.FromBestiaryIcon(30));
			passIcons.Add(GenPassNameID.FloatingIslands, PassIconEntry.FromBestiaryIcon(26));
			passIcons.Add(GenPassNameID.DirtToMud, PassIconEntry.FromItem(176));
			passIcons.Add(GenPassNameID.Silt, PassIconEntry.FromItem(424));
			passIcons.Add(GenPassNameID.OresAndShinies, PassIconEntry.FromItem(19));
			passIcons.Add(GenPassNameID.Webs, PassIconEntry.FromItem(150));
			passIcons.Add(GenPassNameID.Underworld, PassIconEntry.FromBestiaryIcon(33));
			passIcons.Add(GenPassNameID.CorruptionAndCrimson, PassIconEntry.FromBestiaryIcon(7));
			passIcons.Add(GenPassNameID.Lakes, PassIconEntry.FromBestiaryIcon(28));
			passIcons.Add(GenPassNameID.StoneToIceAndSiltPlusMudIntoSlush, PassIconEntry.FromItem(1103));
			passIcons.Add(GenPassNameID.DualDungeonsDitherSnake, PassIconEntry.FromBestiaryIcon(32));
			passIcons.Add(GenPassNameID.Dungeon, PassIconEntry.FromBestiaryIcon(32));
			passIcons.Add(GenPassNameID.MountainCaveOpenings, PassIconEntry.FromBestiaryIcon(2));
			passIcons.Add(GenPassNameID.BeachesAndOceanCleanup, PassIconEntry.FromBestiaryIcon(27));
			passIcons.Add(GenPassNameID.Gems, PassIconEntry.FromItem(178));
			passIcons.Add(GenPassNameID.GravitatingSandCleanup, PassIconEntry.FromItem(169));
			passIcons.Add(GenPassNameID.OceanCaves, PassIconEntry.FromBestiaryIcon(28));
			passIcons.Add(GenPassNameID.Shimmer, PassIconEntry.FromItem(5340));
			passIcons.Add(GenPassNameID.DirtWallCleanup, PassIconEntry.FromItem(2));
			passIcons.Add(GenPassNameID.Pyramids, PassIconEntry.FromItem(607));
			passIcons.Add(GenPassNameID.DirtRockWallRunner, PassIconEntry.FromItem(4501));
			passIcons.Add(GenPassNameID.LivingTrees, PassIconEntry.FromBestiaryIcon(0));
			passIcons.Add(GenPassNameID.LivingTreeWalls, PassIconEntry.FromItem(1723));
			passIcons.Add(GenPassNameID.DemonAndCrimsonAltars, PassIconEntry.FromItem(5467));
			passIcons.Add(GenPassNameID.SurfaceWaterInJungle, PassIconEntry.FromBestiaryIcon(22));
			passIcons.Add(GenPassNameID.LihzahrdTemple, PassIconEntry.FromBestiaryIcon(31));
			passIcons.Add(GenPassNameID.Beehives, PassIconEntry.FromItem(1126));
			passIcons.Add(GenPassNameID.JungleShrines, PassIconEntry.FromItem(680));
			passIcons.Add(GenPassNameID.SettleLiquids, PassIconEntry.FromBestiaryIcon(28));
			passIcons.Add(GenPassNameID.RemoveSurfaceWaterAboveSand, PassIconEntry.FromItem(169));
			passIcons.Add(GenPassNameID.Oasis, PassIconEntry.FromBestiaryIcon(27));
			passIcons.Add(GenPassNameID.ShellPilesMarblePilesAndSpikePits, PassIconEntry.FromItem(4090));
			passIcons.Add(GenPassNameID.SmoothWorld, PassIconEntry.FromBestiaryIcon(1));
			passIcons.Add(GenPassNameID.Waterfalls, PassIconEntry.FromItem(2169));
			passIcons.Add(GenPassNameID.FragileIceOverIceBiomeWater, PassIconEntry.FromItem(664));
			passIcons.Add(GenPassNameID.CaveWallVariety, PassIconEntry.FromItem(4540));
			passIcons.Add(GenPassNameID.LifeCrystals, PassIconEntry.FromItem(29));
			passIcons.Add(GenPassNameID.Statues, PassIconEntry.FromItem(52));
			passIcons.Add(GenPassNameID.UndergroundHousesAndBuriedChests, PassIconEntry.FromItem(306));
			passIcons.Add(GenPassNameID.SurfaceChests, PassIconEntry.FromItem(48));
			passIcons.Add(GenPassNameID.ChestsInJungleShrines, PassIconEntry.FromItem(680));
			passIcons.Add(GenPassNameID.UnderwaterChests, PassIconEntry.FromItem(1298));
			passIcons.Add(GenPassNameID.SpiderCaves, PassIconEntry.FromBestiaryIcon(34));
			passIcons.Add(GenPassNameID.GemCaves, PassIconEntry.FromItem(4644));
			passIcons.Add(GenPassNameID.MossAndMossCaves, PassIconEntry.FromItem(4496));
			passIcons.Add(GenPassNameID.LihzahrdTemplePart2, PassIconEntry.FromBestiaryIcon(31));
			passIcons.Add(GenPassNameID.CaveWallsInEnclosedSpaces, PassIconEntry.FromItem(4510));
			passIcons.Add(GenPassNameID.UndergroundJungleTrees, PassIconEntry.FromBestiaryIcon(23));
			passIcons.Add(GenPassNameID.FloatingIslandHouses, PassIconEntry.FromBestiaryIcon(26));
			passIcons.Add(GenPassNameID.QuickCleanup, PassIconEntry.FromBestiaryIcon(41));
			passIcons.Add(GenPassNameID.PotsGraveyardsAndBoulderPiles, PassIconEntry.FromItem(222));
			passIcons.Add(GenPassNameID.Hellforges, PassIconEntry.FromItem(221));
			passIcons.Add(GenPassNameID.SpreadingGrassOnSurfaceSunflowersEvilsOnSurfaceAndLavaCleanup, PassIconEntry.FromImageFrame("Images/Tiles_3", 5, 45, 1));
			passIcons.Add(GenPassNameID.SurfaceOreAndStone, PassIconEntry.FromItem(19));
			passIcons.Add(GenPassNameID.FallenLogsAndWaterFeatures, PassIconEntry.FromBestiaryIcon(0));
			passIcons.Add(GenPassNameID.Traps, PassIconEntry.FromItem(580));
			passIcons.Add(GenPassNameID.Piles, PassIconEntry.FromBestiaryIcon(1));
			passIcons.Add(GenPassNameID.SpawnPoint, PassIconEntry.FromItem(224));
			passIcons.Add(GenPassNameID.SurfaceDirtWallsToGrassWalls, PassIconEntry.FromItem(745));
			passIcons.Add(GenPassNameID.SpawnStarterNPCs, PassIconEntry.FromItem(867));
			passIcons.Add(GenPassNameID.SunflowersPart2, PassIconEntry.FromItem(63));
			passIcons.Add(GenPassNameID.Trees, PassIconEntry.FromBestiaryIcon(0));
			passIcons.Add(GenPassNameID.AlchemyHerbs, PassIconEntry.FromItem(3093));
			passIcons.Add(GenPassNameID.DyePlants, PassIconEntry.FromItem(1109));
			passIcons.Add(GenPassNameID.WebsInSpiderCavesAndHoneyPlusSpeleothemsInBeehives, PassIconEntry.FromItem(150));
			passIcons.Add(GenPassNameID.GrassPlantsEvilPlantsAndPumpkinsOnSurface, PassIconEntry.FromImageFrame("Images/Tiles_3", 5, 45, 1));
			passIcons.Add(GenPassNameID.GlowingMushroomPlantsUndergroundAndJunglePlants, PassIconEntry.FromBestiaryIcon(25));
			passIcons.Add(GenPassNameID.JunglePlantsPart2, PassIconEntry.FromBestiaryIcon(23));
			passIcons.Add(GenPassNameID.Vines, PassIconEntry.FromItem(3005));
			passIcons.Add(GenPassNameID.Flowers, PassIconEntry.FromImageFrame("Images/Tiles_3", 33, 45, 1));
			passIcons.Add(GenPassNameID.Mushrooms, PassIconEntry.FromItem(5));
			passIcons.Add(GenPassNameID.ExposedGemsInIceBiome, PassIconEntry.FromItem(182));
			passIcons.Add(GenPassNameID.ExposedGemsUnderground, PassIconEntry.FromItem(4400));
			passIcons.Add(GenPassNameID.LongMoss, PassIconEntry.FromItem(4496));
			passIcons.Add(GenPassNameID.DirtWallsIntoMudWallsInJungleAndJungleMinMax, PassIconEntry.FromItem(4487));
			passIcons.Add(GenPassNameID.BeeLarvaInBeehives, PassIconEntry.FromItem(2108));
			passIcons.Add(GenPassNameID.SettleLiquidsPart2AndNotTheBees, PassIconEntry.FromBestiaryIcon(28));
			passIcons.Add(GenPassNameID.CactusPalmTreesAndCoral, PassIconEntry.FromBestiaryIcon(3));
			passIcons.Add(GenPassNameID.TileCleanup, PassIconEntry.FromBestiaryIcon(41));
			passIcons.Add(GenPassNameID.LihzahrdAltar, PassIconEntry.FromBestiaryIcon(31));
			passIcons.Add(GenPassNameID.MicroBiomes, PassIconEntry.FromBestiaryIcon(0));
			passIcons.Add(GenPassNameID.LilypadsCattailsBambooAndSeaweed, PassIconEntry.FromItem(4564));
			passIcons.Add(GenPassNameID.SpeleothemsAndGemTrees, PassIconEntry.FromBestiaryIcon(6));
			passIcons.Add(GenPassNameID.BrokenTrapCleanup, PassIconEntry.FromItem(580));
			passIcons.Add(GenPassNameID.FinalCleanup, PassIconEntry.FromBestiaryIcon(41));
		}

		private PassIconEntry GetPassIcon(GenPass pass)
		{
			if (passIcons.Count == 0)
			{
				InitPassIcons();
			}
			if (!passIcons.TryGetValue(pass.Name, out var value))
			{
				return PassIconEntry.FromBestiaryIcon(64);
			}
			return value;
		}

		private UIImage AddIcon()
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			PassIconEntry passIcon = GetPassIcon(Pass);
			return new UIImage(Main.Assets.Request<Texture2D>(passIcon.Icon, (AssetRequestMode)1))
			{
				Width = new StyleDimension(passIcon.Width, 0f),
				Height = new StyleDimension(passIcon.Height, 0f),
				Top = new StyleDimension((26 - passIcon.Height) / 2, 0f),
				Left = new StyleDimension((26 - passIcon.Width) / 2, 0f),
				Frame = passIcon.Region,
				ScaleToFit = true
			};
		}

		public GenPassElement(UIWorldGenDebug parent, GenPass pass)
		{
			GenPassElement genPassElement = this;
			Pass = pass;
			SetPadding(2f);
			Height.Set(96f, 0f);
			Append(new TooltipElement(GetTitle, GetDescription));
			UIImage uIImage = AddIcon();
			uIImage.IgnoresMouseInteraction = true;
			Append(uIImage);
			UIText indexText = new UIText(Index.ToString(), 0.5f)
			{
				Left = StyleDimension.FromPixels(2f),
				Top = StyleDimension.FromPixels(2f),
				IgnoresMouseInteraction = true
			};
			Append(indexText);
			UIText text = new UIText(pass.Name)
			{
				Left = StyleDimension.FromPixels(32f),
				Top = StyleDimension.FromPixels(4f),
				IgnoresMouseInteraction = true
			};
			text.OnUpdate += (UIElement e) =>
			{
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				text.TextColor = (Color)(genPassElement.IsRunning ? Color.Yellow : (genPassElement.Skipped ? Color.DarkGreen : (genPassElement.HasCompleted ? new Color(0, 230, 0) : ((!pass.Enabled) ? Color.DarkGray : Color.White))));
				UIText uIText = text;
				uIText.TextColor *= (parent.MatchesSearch(pass) ? 1f : 0.6f);
				indexText.TextColor = text.TextColor;
			};
			Append(text);
			SetColorsToNotHovered();
			UIImageButtonWithExtraIcon snapshotIcon = AddButton("Images/UI/ButtonBacking", "Images/UI/Camera_4", 72f, 3f, () =>
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				if (!Main.keyState.PressingAlt())
				{
					if (genPassElement.Snapshot == null)
					{
						Controller.TryCreateSnapshot();
					}
					else if (!genPassElement.Snapshot.Outdated)
					{
						Controller.TryResetToSnapshot(pass);
					}
				}
			}, GetSnapshotButtonTitle, GetSnapshotButtonDescription);
			snapshotIcon.OnUpdate += (UIElement e) =>
			{
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
				if (genPassElement.Snapshot != null)
				{
					snapshotIcon.SetIcon("Images/UI/Camera_4");
				}
				else if (Controller.LastCompletedPass == genPassElement.Pass)
				{
					snapshotIcon.SetIcon("Images/UI/Camera_7");
				}
				SetButtonState(snapshotIcon, (!genPassElement.Pass.Enabled || (genPassElement.Snapshot == null && (Controller.LastCompletedPass != genPassElement.Pass || Controller.CurrentPass != null || !Controller.Paused))) ? ButtonState.NotVisible : ButtonState.Enabled);
				if (genPassElement.Snapshot != null && genPassElement.Snapshot.Outdated)
				{
					snapshotIcon.IconColor = Color.PaleVioletRed;
				}
				else
				{
					snapshotIcon.IconColor = Color.White;
				}
			};
			snapshotIcon.OnRightClick += (UIMouseEvent evt, UIElement e) =>
			{
				if (genPassElement.Snapshot != null)
				{
					Controller.DeleteSnapshot(pass);
					UserInterface.ActiveInstance.ClearPointers();
					genPassElement.SuppressPassRightClick = true;
				}
			};
			snapshotIcon.Left = new StyleDimension(-28f, 1f);
			OnLeftClick += (UIMouseEvent evt, UIElement e) =>
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				if (genPassElement == evt.Target && !spaceWasPressed)
				{
					if (Main.keyState.PressingAlt())
					{
						genPassElement.ToggleHighlight();
						genPassElement.SetColorsToHovered();
					}
					else if (!pass.Enabled)
					{
						parent.RangePassClickEvent(genPassElement, (GenPassElement x) =>
						{
							x.Enable();
							x.RefreshColors();
						});
					}
					else if (pass.Enabled)
					{
						Controller.TryRunToEndOfPass(pass, !Main.keyState.PressingShift());
					}
				}
			};
			OnRightClick += (UIMouseEvent evt, UIElement e) =>
			{
				if (genPassElement.SuppressPassRightClick)
				{
					genPassElement.SuppressPassRightClick = false;
				}
				else if (pass.Enabled)
				{
					parent.RangePassClickEvent(genPassElement, (GenPassElement x) =>
					{
						x.Disable();
						x.RefreshColors();
					});
				}
			};
		}

		private void RefreshColors()
		{
			if (Hovered)
			{
				SetColorsToHovered();
			}
			else
			{
				SetColorsToNotHovered();
			}
		}

		private void SetColorsToHovered()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			BackgroundColor = new Color(73, 94, 171);
			BorderColor = new Color(89, 116, 213);
			if (IsHighlighted)
			{
				BackgroundColor = new Color(110, 30, 150);
				BorderColor = new Color(171, 53, 255);
			}
			if (CurrentTargetPass == Pass)
			{
				BorderColor = new Color(255, 231, 69);
			}
			if (!Pass.Enabled)
			{
				BorderColor = new Color(150, 150, 150) * 1f;
				BackgroundColor = Color.Lerp(BackgroundColor, new Color(120, 120, 120), 0.5f) * 1f;
			}
		}

		private void SetColorsToNotHovered()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			BackgroundColor = new Color(63, 82, 151) * 0.7f;
			BorderColor = new Color(89, 116, 213) * 0.7f;
			if (IsHighlighted)
			{
				BackgroundColor = new Color(110, 30, 150) * 0.7f;
				BorderColor = new Color(171, 53, 255) * 0.7f;
			}
			if (CurrentTargetPass == Pass)
			{
				BorderColor = new Color(255, 231, 69);
			}
			if (!Pass.Enabled)
			{
				BorderColor = new Color(127, 127, 127) * 0.7f;
				BackgroundColor = Color.Lerp(BackgroundColor, new Color(80, 80, 80), 0.5f) * 0.7f;
			}
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Hovered = true;
			base.MouseOver(evt);
			SetColorsToHovered();
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			Hovered = false;
			base.MouseOut(evt);
			SetColorsToNotHovered();
		}

		private string GetTitle()
		{
			if (Skipped)
			{
				return "Skipped: " + Pass.Name;
			}
			if (!Pass.Enabled)
			{
				return "Disabled: " + Pass.Name;
			}
			return ((!HasCompleted) ? "Run" : "Rerun") + " to " + Pass.Name;
		}

		private string GetDescription()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			string text = string.Empty;
			if (Pass.Enabled)
			{
				text += "Hold shift to ignore snapshots\n";
				if (!CanSubmitActions && (HasCompleted || Main.keyState.PressingShift()))
				{
					text += "[c/FFA500:Must be paused to rerun or load snapshots]\n";
				}
			}
			if (!HasCompleted && !Skipped)
			{
				text += (Pass.Enabled ? "Right click to disable\n" : "Left click to enable\n");
				text += "Shift to edit ranges\n";
			}
			return text + "Alt click to toggle highlight\n";
		}

		private string GetSnapshotButtonTitle()
		{
			if (Snapshot != null && Snapshot.Outdated)
			{
				return "Snapshot is outdated and will only be used for comparison when the pass is run again";
			}
			if (Snapshot != null)
			{
				return "Reset to snapshot";
			}
			if (Controller.LastCompletedPass == Pass)
			{
				return "Take snapshot";
			}
			return null;
		}

		private string GetSnapshotButtonDescription()
		{
			if (Snapshot != null)
			{
				string text = "Left click to load snapshot\n";
				text += "Right click to delete snapshot\n";
				if (!CanSubmitActions)
				{
					text += "[c/FFA500:Must be paused to load a snapshot]";
				}
				return text;
			}
			if (Controller.LastCompletedPass == Pass)
			{
				return "Left click to take snapshot\n";
			}
			return null;
		}

		private void Enable()
		{
			Utils.TryOperateInLock(Pass, () =>
			{
				if (!HasCompleted)
				{
					Pass.Enable();
					Controller.ForceUpdateProgress();
				}
			});
		}

		private void Disable()
		{
			Utils.TryOperateInLock(Pass, () =>
			{
				if (!HasCompleted)
				{
					Pass.Disable();
					Controller.ForceUpdateProgress();
					Controller.DeleteSnapshot(Pass);
				}
			});
		}

		private void ToggleHighlight()
		{
			if (IsHighlighted)
			{
				Config.Instance.HighlightedPassNames.Remove(Pass.Name);
			}
			else
			{
				Config.Instance.HighlightedPassNames.Add(Pass.Name);
			}
			Config.Save();
		}
	}

	private enum ButtonState
	{
		Enabled,
		NotVisible
	}

	private UIWrappedSearchBar searchBar;

	private string lastSearchText;

	private string searchText;

	private bool showMap;

	private bool hideChat;

	private bool hideUI;

	private bool disableDebugOnClose;

	private bool disableLightOnClose;

	private IEnumerator<object> TestEnumerator;

	private UIElement controlListArea;

	private UIPanel controlPanel;

	private UIPanel scrollPanel;

	private UIScrollbar scrollbar;

	private UIList GenPassList;

	private GroupOptionButton<bool> SearchButton;

	private List<GenPassElement> allPasses = new List<GenPassElement>();

	private bool searchVisible = true;

	private int LassPassIndex;

	private Tuple<GenPassElement, Action<GenPassElement>> _previousRangePassClickEvent;

	private int ignoreEscapeAttempt;

	private static bool spaceWasPressed;

	private Point nextMapSection;

	private TimeSpan fullMapScanPeriod;

	private Stopwatch fullMapScanTimer;

	private Stopwatch lastScanRateUpdate = Stopwatch.StartNew();

	public static UIWorldGenDebug ActiveInstance => UserInterface.ActiveInstance.CurrentState as UIWorldGenDebug;

	public static bool IsActive => (Main.gameMenu ? Main.MenuUI.CurrentState : Main.InGameUI.CurrentState) is UIWorldGenDebug;

	private static WorldGenerator.Controller Controller => WorldGenerator.CurrentController;

	private static bool CanSubmitActions
	{
		get
		{
			if (Controller.Paused)
			{
				return Controller.CurrentPass == null;
			}
			return false;
		}
	}

	public static GenPass CurrentTargetOrLatestPass
	{
		get
		{
			GenPass genPass = Controller.PauseAfterPass;
			if (genPass == null)
			{
				genPass = Controller.CurrentPass;
			}
			if (genPass == null)
			{
				genPass = Controller.LastCompletedPass;
			}
			return genPass;
		}
	}

	public static GenPass CurrentTargetPass
	{
		get
		{
			GenPass genPass = Controller.PauseAfterPass;
			if (genPass == Controller.LastCompletedPass)
			{
				genPass = null;
			}
			return genPass;
		}
	}

	private static void SetButtonState(UIImageButtonWithExtraIcon button, ButtonState state)
	{
		switch (state)
		{
		case ButtonState.Enabled:
			button.SetVisibility(1f, 0.4f);
			break;
		case ButtonState.NotVisible:
			button.SetVisibility(0f, 0f);
			break;
		}
		button.IgnoresMouseInteraction = state != ButtonState.Enabled;
	}

	public static void Open()
	{
		if (Main.gameMenu)
		{
			Main.MenuUI.SetState(new UIWorldGenDebug());
		}
		else
		{
			IngameFancyUI.OpenUIState(new UIWorldGenDebug());
		}
	}

	public static void Close()
	{
		if (ActiveInstance != null)
		{
			if (Main.gameMenu)
			{
				Main.MenuUI.SetState(new UIWorldLoad());
			}
			else
			{
				IngameFancyUI.Close();
			}
		}
	}

	public UIWorldGenDebug()
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		NoGamepadSupport = true;
		IgnoresMouseInteraction = true;
		UIGenProgressBar progressBar = new UIGenProgressBar
		{
			VAlign = 0f,
			HAlign = 0.5f,
			Top = StyleDimension.FromPixels(20f),
			IgnoresMouseInteraction = true
		};
		Append(progressBar);
		UIHeader progressMessage = new UIHeader
		{
			VAlign = 0f,
			HAlign = 0.5f,
			IgnoresMouseInteraction = true
		};
		Append(progressMessage);
		OnUpdate += (UIElement e) =>
		{
			progressBar.SetProgress((float)WorldGenerator.CurrentGenerationProgress.TotalProgress, (float)WorldGenerator.CurrentGenerationProgress.Value);
			progressMessage.Text = WorldGenerator.CurrentGenerationProgress.Message;
			if (WorldGenerator.CurrentController.QueuedAbort)
			{
				progressMessage.Text = Language.GetTextValue("UI.Canceling");
			}
			if (WorldGen.Manifest.GenPassResults.Count != LassPassIndex)
			{
				LassPassIndex = WorldGen.Manifest.GenPassResults.Count;
				EnsurePassVisible(LassPassIndex);
			}
		};
		controlListArea = new UIElement
		{
			Width = StyleDimension.FromPixels(450f),
			Height = StyleDimension.FromPixelsAndPercent(-60f, 1f),
			Top = StyleDimension.FromPixels(30f),
			Left = StyleDimension.FromPixels(10f)
		};
		Append(controlListArea);
		controlPanel = new UIPanel
		{
			Height = StyleDimension.FromPixels(50f)
		};
		controlPanel.SetPadding(8f);
		controlPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
		GroupOptionButton<bool> groupOptionButton = AddButton(controlPanel, "Images/UI/Camera_0", () =>
		{
			Controller.DeleteAllSnapshots();
		}, () => "Delete all snapshots", () => "Click to clear all snapshots\nEstimated Disk Usage: " + WorldGenSnapshot.EstimatedDiskUsage / 1024 / 1024 + "MB" + (CanSubmitActions ? "" : "\n[c/FFA500:Must be paused to manipulate snapshots]"));
		UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/CoolDown", (AssetRequestMode)1))
		{
			ScaleToFit = true,
			Width = new StyleDimension(28f, 0f),
			Height = new StyleDimension(28f, 0f),
			Left = new StyleDimension(3f, 0f),
			Top = new StyleDimension(3f, 0f)
		};
		groupOptionButton.Append(element);
		GroupOptionButton<bool> groupOptionButton2 = AddButton(controlPanel, "Images/UI/IconReset", () =>
		{
			Controller.TryReset();
		}, () => "Reset", () => (!CanSubmitActions) ? "[c/FFA500:Must be paused to reset]" : null);
		groupOptionButton2.IconScale = 28f / (float)groupOptionButton2.Icon.Width;
		groupOptionButton2.IconOffset = new Vector2(2f, 3f);
		GroupOptionButton<bool> groupOptionButton3 = AddButton(controlPanel, "Images/UI/IconPrev", StepBack, () => "Step Back", () => "Hotkey: Up/Left");
		groupOptionButton3.IconScale = 28f / (float)groupOptionButton3.Icon.Width;
		groupOptionButton3.IconOffset = new Vector2(2f, 3f);
		GroupOptionButton<bool> playPauseButton = AddButton(controlPanel, "Images/UI/IconPlayPause", () =>
		{
			WorldGenerator.Controller controller = Controller;
			controller.Paused = !controller.Paused;
		}, () => (!WorldGenerator.CurrentController.Paused) ? "Pause" : "Play", () => "Hotkey: Space");
		playPauseButton.IconScale = 28f / (float)playPauseButton.Icon.Width;
		playPauseButton.IconOffset = new Vector2(3f, 3f);
		playPauseButton.OnUpdate += (UIElement e) =>
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			playPauseButton.SetIconFrame(playPauseButton.Icon.Frame(1, 2, 0, (!Controller.Paused) ? 1 : 0));
		};
		GroupOptionButton<bool> groupOptionButton4 = AddButton(controlPanel, "Images/UI/IconNext", StepForward, () => "Step Forward", () => "Hotkey: Down/Right");
		groupOptionButton4.IconScale = 28f / (float)groupOptionButton4.Icon.Width;
		groupOptionButton4.IconOffset = new Vector2(2f, 3f);
		AddButton(controlPanel, "Images/Map_0", () =>
		{
			ToggleMap();
		}, () => "Toggle Map", () => "Left click to toggle the map display").IconOffset = new Vector2(4f, 5f);
		GroupOptionButton<bool> groupOptionButton5 = AddButton(controlPanel, "Images/Extra_" + (short)48, () =>
		{
			hideChat = !hideChat;
		}, () => "Toggle Chat", () => "Left click to toggle the chat log");
		Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/Extra_" + (short)48, (AssetRequestMode)1);
		Rectangle val2 = val.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, 1);
		groupOptionButton5.IconScale = 28f / (float)val2.Width;
		groupOptionButton5.IconOffset = new Vector2(3f, 5f);
		groupOptionButton5.SetIconFrame(val2);
		val2 = val.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, 4, 3);
		UIImage element2 = new UIImage(val)
		{
			Frame = val2,
			ScaleToFit = true,
			Width = new StyleDimension(28f, 0f),
			Height = new StyleDimension(28f, 0f),
			Left = new StyleDimension(2f, 0f),
			Top = new StyleDimension(6f, 0f)
		};
		groupOptionButton5.Append(element2);
		GroupOptionButton<bool> snapshotFrequencyButton = AddButton(controlPanel, "Images/UI/IconSnapshotFrequency", CycleSnapshotMode, GetSnapshotModeButtonTitle);
		snapshotFrequencyButton.OnUpdate += (UIElement e) =>
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			snapshotFrequencyButton.SetIconFrame(snapshotFrequencyButton.Icon.Frame(1, 3, 0, (int)WorldGenerator.CurrentController.SnapshotFrequency));
		};
		GroupOptionButton<bool> mismatchPauseButton = AddButton(controlPanel, "Images/UI/IconMismatchPause", () =>
		{
			WorldGenerator.Controller controller = Controller;
			controller.PauseOnHashMismatch = !controller.PauseOnHashMismatch;
		}, () => "Pause on gen pass change: " + (WorldGenerator.CurrentController.PauseOnHashMismatch ? "On" : "Off"), () => "Stop the generator when the output of a pass is different\nto the last time it was run in the save, or current session");
		mismatchPauseButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
		mismatchPauseButton.OnUpdate += (UIElement e) =>
		{
			mismatchPauseButton.SetCurrentOption(WorldGenerator.CurrentController.PauseOnHashMismatch);
		};
		string quickLoadCommand = (Main.gameMenu ? "/quickload-regen" : "/quickload");
		AddButton(controlPanel, "Images/UI/IconQuickload", () =>
		{
			DebugUtils.QuickSPMessage(quickLoadCommand);
		}, () => "Save current settings to " + quickLoadCommand, () => "Future launches of the game will automatically load the world\nfrom the most recent snapshot, and run to the current pass");
		UIImage uIImage = AddImage(controlPanel, "Images/UI/Bestiary/Icon_Locked", () =>
		{
		}, () => "Controls", () => GetControls());
		uIImage.ImageScale = 24f / (float)uIImage.Texture.Value.Height;
		uIImage.NormalizedOrigin.X = 0.75f;
		AddButton(controlPanel, "Images/UI/Camera_5", () =>
		{
			Controller.QueuedAbort = true;
		}, () => "Cancel").IconOffset = new Vector2(4f, 4f);
		controlListArea.Append(controlPanel);
		float num = controlPanel.Height.Pixels + 2f;
		scrollPanel = new UIPanel
		{
			Width = StyleDimension.FromPixelsAndPercent(300f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num, 1f),
			Top = StyleDimension.FromPixels(num),
			Left = controlPanel.Left,
			HAlign = 0f,
			VAlign = 0f
		};
		scrollPanel.PaddingTop = 8f;
		scrollPanel.PaddingBottom = 8f;
		scrollPanel.PaddingLeft = 4f;
		scrollPanel.PaddingRight = 4f;
		controlListArea.Append(scrollPanel);
		searchBar = new UIWrappedSearchBar(() =>
		{
			UserInterface.ActiveInstance.SetState(this);
		})
		{
			Left = StyleDimension.FromPixels(-2f),
			Top = StyleDimension.FromPixels(-2f),
			Height = StyleDimension.FromPixels(28f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0f
		};
		searchBar.OnSearchContentsChanged += (string s) =>
		{
			searchText = s;
		};
		searchBar.HideSearchButton();
		scrollPanel.Append(searchBar);
		num = 30f;
		GenPassList = new UIList
		{
			Top = StyleDimension.FromPixels(num),
			Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num, 1f),
			ListPadding = 0f,
			ManualSortMethod = (List<UIElement> _) =>
			{
			}
		};
		scrollPanel.Append(GenPassList);
		foreach (GenPass pass in Controller.Passes)
		{
			GenPassElement item = new GenPassElement(this, pass)
			{
				Width = new StyleDimension(-4f, 1f),
				Height = StyleDimension.FromPixels(32f),
				PaddingLeft = 7f
			};
			allPasses.Add(item);
			GenPassList.Add(item);
		}
		scrollbar = new UIScrollbar
		{
			Top = StyleDimension.FromPixels(34f),
			Height = StyleDimension.FromPixelsAndPercent(-38f, 1f),
			Left = StyleDimension.FromPixels(-1f),
			HAlign = 1f
		};
		scrollbar.SetView(100f, 1000f);
		GenPassList.SetScrollbar(scrollbar);
		scrollPanel.Append(scrollbar);
		RefreshControlsPosition();
	}

	private void EnsurePassVisible(int passIndex)
	{
		if (passIndex >= allPasses.Count)
		{
			return;
		}
		GenPassElement genPassElement = allPasses[passIndex];
		if (!searchVisible || string.IsNullOrEmpty(searchText) || MatchesSearch(genPassElement.Pass))
		{
			float height = scrollPanel.GetDimensions().Height;
			if (genPassElement.Height.Pixels * 2f + genPassElement.Top.Pixels > scrollbar.ViewPosition + height - 8f && genPassElement.Top.Pixels <= scrollbar.ViewPosition + height - 8f)
			{
				scrollbar.ViewPosition = genPassElement.Top.Pixels - (height - 8f) + genPassElement.Height.Pixels * 2f;
			}
		}
	}

	private void RefreshControlsPosition()
	{
	}

	private string GetControls()
	{
		return "[c/FFF014:Space] to pause/resume\n[c/FFF014:R] to rerun current step\n[c/FFF014:Up]/[c/FFF014:Down] or [c/FFF014:Left]/[c/FFF014:Right] to step back/forward\n[c/FFF014:H] to hide UI\n[c/FFF014:M] to toggle map\n[c/FFF014:C] to hide chat log\n";
	}

	private GroupOptionButton<bool> AddButton(UIPanel controlPanel, string assetPath, Action onClick, Func<string> getTitle, Func<string> getDescription = null)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, assetPath)
		{
			Width = new StyleDimension(34f, 0f),
			Height = new StyleDimension(34f, 0f),
			Left = StyleDimension.FromPixelsAndPercent(36 * controlPanel.Children.Count(), 0f),
			ShowHighlightWhenSelected = false
		};
		groupOptionButton.IconScale = 24f / (float)groupOptionButton.Icon.Width;
		groupOptionButton.IconOffset = new Vector2(3f, 3f);
		groupOptionButton.OnLeftClick += (UIMouseEvent evt, UIElement e) =>
		{
			onClick();
		};
		groupOptionButton.Append(new TooltipElement(getTitle, getDescription));
		controlPanel.Append(groupOptionButton);
		controlPanel.Width = StyleDimension.FromPixelsAndPercent((float)(36 * controlPanel.Children.Count() - 2) + controlPanel.PaddingLeft + controlPanel.PaddingRight, 0f);
		return groupOptionButton;
	}

	private UIImage AddImage(UIPanel controlPanel, string assetPath, Action onClick, Func<string> getTitle, Func<string> getDescription = null)
	{
		UIImage uIImage = new UIImage(Main.Assets.Request<Texture2D>(assetPath, (AssetRequestMode)1))
		{
			Width = new StyleDimension(34f, 0f),
			Height = new StyleDimension(34f, 0f),
			Left = StyleDimension.FromPixelsAndPercent(36 * controlPanel.Children.Count(), 0f)
		};
		uIImage.OnLeftClick += (UIMouseEvent evt, UIElement e) =>
		{
			onClick();
		};
		uIImage.Append(new TooltipElement(getTitle, getDescription));
		controlPanel.Append(uIImage);
		controlPanel.Width = StyleDimension.FromPixelsAndPercent((float)(36 * controlPanel.Children.Count() - 2) + controlPanel.PaddingLeft + controlPanel.PaddingRight, 0f);
		return uIImage;
	}

	private void RangePassClickEvent(GenPassElement target, Action<GenPassElement> evt)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (_previousRangePassClickEvent != null && _previousRangePassClickEvent.Item2.Method == evt.Method && _previousRangePassClickEvent.Item1 != target && _previousRangePassClickEvent.Item1.Parent == target.Parent && Main.keyState.PressingShift())
		{
			IEnumerable<GenPassElement> enumerable = ((UIList)target.Parent.Parent).Cast<GenPassElement>();
			GenPassElement item = _previousRangePassClickEvent.Item1;
			int num = 0;
			foreach (GenPassElement item2 in enumerable)
			{
				if (item2 == item || item2 == target)
				{
					num++;
				}
				if (num > 0)
				{
					evt(item2);
				}
				if (num == 2)
				{
					break;
				}
			}
		}
		else
		{
			evt(target);
		}
		_previousRangePassClickEvent = new Tuple<GenPassElement, Action<GenPassElement>>(target, evt);
	}

	private void RangePassClickEventCheckHistory_OnElementClicked(UIElement clicked)
	{
		if (_previousRangePassClickEvent != null && clicked != _previousRangePassClickEvent.Item1)
		{
			_previousRangePassClickEvent = null;
		}
	}

	public override void OnActivate()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Config.Load();
		if (Controller.SnapshotFrequency == WorldGenerator.SnapshotFrequency.None)
		{
			Controller.SnapshotFrequency = WorldGenerator.SnapshotFrequency.Automatic;
		}
		Main.menuChat = true;
		if (Main.gameMenu)
		{
			PlayerInput.SetZoom_World();
			Main.mapFullscreenPos = new Vector2((float)(Main.maxTilesX / 2), (float)(Main.maxTilesY / 2));
			Main.mapFullscreenScale = (float)Main.screenWidth / (float)Main.maxTilesX;
		}
		else
		{
			Main.mapFullscreenScale = 2.5f;
			Main.mapFullscreenPos = Main.Camera.Center / 16f;
		}
		ToggleMap();
		if (!Main.gameMenu && !DebugOptions.devLightTilesCheat)
		{
			DebugOptions.devLightTilesCheat = true;
			disableLightOnClose = true;
		}
	}

	public override void OnDeactivate()
	{
		Main.menuChat = false;
		if (disableLightOnClose)
		{
			DebugOptions.devLightTilesCheat = false;
		}
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		Main.starGame = false;
		Main.LocalPlayer.dead = true;
		if (Controller.Paused && TestEnumerator != null)
		{
			while (!Controller.TryOperateInControlLock(() =>
			{
			}))
			{
				Thread.Yield();
			}
			if (!TestEnumerator.MoveNext())
			{
				TestEnumerator = null;
			}
		}
		base.Update(gameTime);
		if (Main.drawingPlayerChat || searchBar.IsWritingText)
		{
			ignoreEscapeAttempt = 3;
			return;
		}
		if (ignoreEscapeAttempt-- <= 0 && PlayerInput.Triggers.JustPressed.Inventory)
		{
			Controller.QueuedAbort = true;
		}
		if (KeyPressed((Keys)32))
		{
			WorldGenerator.Controller controller = Controller;
			controller.Paused = !controller.Paused;
		}
		if (KeyPressed((Keys)82) && CanSubmitActions && Controller.LastCompletedPass != null)
		{
			Controller.TryRunToEndOfPass(Controller.LastCompletedPass, !Main.keyState.PressingShift());
		}
		if (KeyPressed((Keys)38) || KeyPressed((Keys)37))
		{
			StepBack();
		}
		if (KeyPressed((Keys)40) || KeyPressed((Keys)39))
		{
			StepForward();
		}
		if (KeyPressed((Keys)67))
		{
			hideChat = !hideChat;
		}
		if (KeyPressed((Keys)72))
		{
			ToggleUI();
		}
		if (KeyPressed((Keys)77))
		{
			ToggleMap();
		}
		PlayerInput.SetZoom_World();
		if (showMap)
		{
			if (PlayerInput.Triggers.Current.Up && !Main.oldKeyState.IsKeyDown((Keys)38))
			{
				Main.mapFullscreenPos.Y -= 1f * (16f / Main.mapFullscreenScale);
			}
			if (PlayerInput.Triggers.Current.Down && !Main.oldKeyState.IsKeyDown((Keys)40))
			{
				Main.mapFullscreenPos.Y += 1f * (16f / Main.mapFullscreenScale);
			}
			if (PlayerInput.Triggers.Current.Left && !Main.oldKeyState.IsKeyDown((Keys)37))
			{
				Main.mapFullscreenPos.X -= 1f * (16f / Main.mapFullscreenScale);
			}
			if (PlayerInput.Triggers.Current.Right && !Main.oldKeyState.IsKeyDown((Keys)39))
			{
				Main.mapFullscreenPos.X += 1f * (16f / Main.mapFullscreenScale);
			}
			if (!UserInterface.ActiveInstance.IsElementUnderMouse())
			{
				Main.mapFullscreenScale *= 1f + (float)(PlayerInput.ScrollWheelDelta / 120) * 0.3f;
			}
			Main.screenPosition = Main.mapFullscreenPos * 16f - Main.Camera.UnscaledSize / 2f;
		}
		else if (!Main.gameMenu)
		{
			Main.DebugCameraPan(PlayerInput.Triggers.Current.Left, PlayerInput.Triggers.Current.Right, PlayerInput.Triggers.Current.Up, PlayerInput.Triggers.Current.Down);
		}
		if (!Main.gameMenu)
		{
			Main.ClampScreenPositionToWorld();
			Player localPlayer = Main.LocalPlayer;
			localPlayer.position += Main.screenPosition - Main.PlayerFocusedScreenPosition();
			Main.mapFullscreenPos = Main.Camera.Center / 16f;
		}
		PlayerInput.SetZoom_UI();
		spaceWasPressed = Main.keyState.IsKeyDown((Keys)32) || Main.oldKeyState.IsKeyDown((Keys)32);
	}

	private void ToggleUI()
	{
		hideUI = !hideUI;
		foreach (UIElement element in Elements)
		{
			element.IgnoresMouseInteraction = hideUI;
		}
	}

	public void UnhideChat()
	{
		hideChat = false;
	}

	private void StepBack()
	{
		if (CurrentTargetOrLatestPass != null)
		{
			Controller.TryResetToPreviousPass(CurrentTargetOrLatestPass);
		}
	}

	private void StepForward()
	{
		int num = Controller.Passes.IndexOf(CurrentTargetOrLatestPass);
		GenPass genPass = Controller.Passes.Skip(num + 1).FirstOrDefault((GenPass p) => p.Enabled);
		if (genPass != null)
		{
			Controller.TryRunToEndOfPass(genPass);
		}
	}

	private void CycleSnapshotMode()
	{
		Controller.SnapshotFrequency = (WorldGenerator.SnapshotFrequency)((int)(Controller.SnapshotFrequency + 1) % 3);
	}

	private string GetSnapshotModeButtonTitle()
	{
		return WorldGenerator.CurrentController.SnapshotFrequency switch
		{
			WorldGenerator.SnapshotFrequency.Manual => "Create snaphots: Manually", 
			WorldGenerator.SnapshotFrequency.Automatic => "Create snaphots: Automatically", 
			WorldGenerator.SnapshotFrequency.Always => "Create snaphots: After every pass", 
			_ => "", 
		};
	}

	private static bool KeyPressed(Keys key)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (Main.keyState.IsKeyDown(key))
		{
			return !Main.oldKeyState.IsKeyDown(key);
		}
		return false;
	}

	private bool MatchesSearch(GenPass pass)
	{
		if (!string.IsNullOrWhiteSpace(searchText))
		{
			return pass.Name.ToLowerInvariant().Contains(searchText.Trim().ToLowerInvariant());
		}
		return true;
	}

	private void UpdateFilter()
	{
		if (searchVisible && !string.IsNullOrEmpty(searchText))
		{
			if (!(lastSearchText != searchText))
			{
				return;
			}
			GenPassList.Clear();
			lastSearchText = searchText;
			{
				foreach (GenPassElement allPass in allPasses)
				{
					if (MatchesSearch(allPass.Pass))
					{
						GenPassList.Add(allPass);
					}
				}
				return;
			}
		}
		if (allPasses.Count == GenPassList.Count)
		{
			return;
		}
		lastSearchText = null;
		GenPassList.Clear();
		foreach (GenPassElement allPass2 in allPasses)
		{
			GenPassList.Add(allPass2);
		}
	}

	public override void Recalculate()
	{
		if (Main.gameMenu)
		{
			Main.UIScale = Main.UIScaleWanted;
			PlayerInput.SetZoom_UI();
		}
		base.Recalculate();
	}

	protected override void DrawChildren(SpriteBatch spriteBatch)
	{
		if (!hideUI)
		{
			base.DrawChildren(spriteBatch);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		Main.starGame = false;
		Main.onlyDrawFancyUI = showMap;
		if (showMap)
		{
			Main.alreadyGrabbingSunOrMoon = false;
			UpdateAndDrawMap();
			Main.instance.DrawFPS();
		}
		if (!hideChat || Main.drawingPlayerChat)
		{
			spriteBatch.End();
			spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Matrix.CreateTranslation(250f, 0f, 0f) * Main.UIScaleMatrix);
			Main.instance.DrawPlayerChat();
			spriteBatch.End();
			spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		UpdateFilter();
		if (Main.gameMenu)
		{
			Main.UIScale = Main.UIScaleWanted;
			PlayerInput.SetZoom_UI();
			Main.spriteBatch.End();
			Main.spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		}
		base.Draw(spriteBatch);
		if (!showMap)
		{
			Main.DrawInterface_37_DebugStuff();
		}
	}

	private void ToggleMap()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		showMap = !showMap;
		Main.onlyDrawFancyUI = showMap;
		if (showMap)
		{
			nextMapSection = Point.Zero;
			fullMapScanTimer = Stopwatch.StartNew();
		}
	}

	private void UpdateAndDrawMap()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected Obj, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected Obj, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.End();
		if (Main.clearMap)
		{
			Main.Map.Clear();
			MapRenderer.DrawToMap(default);
		}
		PlayerInput.SetZoom_Unscaled();
		Rectangle val = Utils.CenteredRectangle(Main.mapFullscreenPos.ToPoint(), (Main.ScreenSize.ToVector2() / Main.mapFullscreenScale).ToPoint());
		val.Inflate(2, 2);
		val = WorldUtils.ClampToWorld(val, 40);
		Stopwatch stopwatch = Stopwatch.StartNew();
		Point val2 = nextMapSection;
		while (stopwatch.ElapsedMilliseconds < 10)
		{
			Rectangle sectionStripRect = new Rectangle(nextMapSection.X * 200, 0, 200, Main.maxTilesY);
			sectionStripRect = Rectangle.Intersect(sectionStripRect, val);
			bool mapUpdate = false;
			FastParallel.For(sectionStripRect.Left, sectionStripRect.Right, (ParallelForAction)((int x1, int x2, object _) =>
			{
				bool flag2 = false;
				int top = sectionStripRect.Top;
				int bottom = sectionStripRect.Bottom;
				for (int i = x1; i < x2; i++)
				{
					for (int j = top; j < bottom; j++)
					{
						flag2 |= Main.Map.UpdateLighting(i, j, byte.MaxValue);
					}
				}
				if (flag2)
				{
					mapUpdate = true;
				}
			}), (object)null);
			nextMapSection.Y = 0;
			while ((nextMapSection.Y < Main.maxSectionsY) & mapUpdate)
			{
				Rectangle val3 = new Rectangle(nextMapSection.X * 200, nextMapSection.Y * 150, 200, 150);
				if (val3.Intersects(val))
				{
					MapRenderer.DrawToMap_Section(nextMapSection.X, nextMapSection.Y);
				}
				nextMapSection.Y++;
			}
			nextMapSection.X++;
			if (nextMapSection.X >= Main.maxSectionsX)
			{
				if (lastScanRateUpdate.Elapsed > TimeSpan.FromMilliseconds(200.0))
				{
					lastScanRateUpdate.Restart();
					fullMapScanPeriod = fullMapScanTimer.Elapsed;
				}
				fullMapScanTimer.Restart();
				nextMapSection.X = 0;
			}
			if (nextMapSection.X == val2.X)
			{
				break;
			}
		}
		((Game)Main.instance).GraphicsDevice.Clear(new Color(100, 100, 255));
		Main.spriteBatch.Begin();
		Main.mapReady = true;
		Main.MapPylonTile = new Point16(-1, -1);
		Main.mapFullscreen = true;
		bool flag = UserInterface.ActiveInstance.MouseCaptured() || UserInterface.ActiveInstance.IsElementUnderMouse();
		bool t = Main.mouseLeft && !flag && !spaceWasPressed;
		bool t2 = Main.mouseRight && !flag;
		Utils.Swap(ref Main.mouseLeft, ref t);
		Utils.Swap(ref Main.mouseRight, ref t2);
		Main.instance.DrawMap(new GameTime());
		Utils.Swap(ref Main.mouseLeft, ref t);
		Utils.Swap(ref Main.mouseRight, ref t2);
		Main.mapFullscreen = false;
		PlayerInput.SetZoom_UI();
		Main.spriteBatch.End();
		Main.spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		if (Main.showFrameRate)
		{
			double num = Math.Min(1.0 / fullMapScanPeriod.TotalSeconds, 60.0);
			string text = string.Format((num >= 10.0) ? "{0:0}" : "{0:0.0}", num);
			text += " map scans/s";
			DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, text, new Vector2((float)(Main.screenWidth - (int)FontAssets.MouseText.Value.MeasureString(text).X), 4f), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor));
		}
	}

	public void RunTest(IEnumerable<object> test)
	{
		TestEnumerator = test.GetEnumerator();
	}

	private static IEnumerable<bool> TestSetupResetAndCreateSnapshots()
	{
		Controller.TryReset();
		Controller.SnapshotFrequency = WorldGenerator.SnapshotFrequency.Always;
		Controller.PauseOnHashMismatch = true;
		Main.NewText("Creating Snapshots", byte.MaxValue, 100, 0);
		GenPass lastPass = Controller.Passes.Last();
		Controller.TryRunToEndOfPass(lastPass, useSnapshots: false);
		yield return true;
		if (Controller.LastCompletedPass != lastPass || Controller.PausedDueToHashMismatch)
		{
			Main.NewText("Test aborted", byte.MaxValue, 0, 0);
			yield return false;
		}
		Controller.SnapshotFrequency = WorldGenerator.SnapshotFrequency.Manual;
	}

	public static IEnumerable<object> TestResetFromPassesAndRegen()
	{
		foreach (bool item in TestSetupResetAndCreateSnapshots())
		{
			if (item)
			{
				yield return null;
				continue;
			}
			yield break;
		}
		GenPass lastPass = Controller.Passes.Last();
		List<GenPass> passes = Controller.Passes.Where((GenPass p) => p.Enabled).ToList();
		for (int i = 0; i < passes.Count; i++)
		{
			GenPass pass = passes[i];
			Controller.TryReset();
			Main.NewText($"[{i + 1}/{passes.Count}] Running to {pass.Name}", byte.MaxValue, 100, 0);
			Controller.TryRunToEndOfPass(pass, useSnapshots: false);
			yield return null;
			if (Controller.LastCompletedPass != pass || Controller.PausedDueToHashMismatch)
			{
				Main.NewText("Test aborted", byte.MaxValue, 0, 0);
				yield break;
			}
			Controller.TryReset();
			Controller.TryRunToEndOfPass(lastPass, useSnapshots: false);
			yield return null;
			if (Controller.LastCompletedPass != lastPass || Controller.PausedDueToHashMismatch)
			{
				Main.NewText("Test aborted", byte.MaxValue, 0, 0);
				yield break;
			}
		}
		Main.NewText("Test Completed Successfully", 0, byte.MaxValue, 0);
	}

	public static IEnumerable<object> TestHiddenTileData()
	{
		foreach (bool item in TestSetupResetAndCreateSnapshots())
		{
			if (item)
			{
				yield return null;
				continue;
			}
			yield break;
		}
		Controller.TryReset();
		foreach (GenPass pass in Controller.Passes.Where((GenPass p) => p.Enabled))
		{
			TileSnapshot.Create();
			TileSnapshot.Restore();
			Controller.TryRunToEndOfPass(pass, useSnapshots: false);
			yield return null;
			if (Controller.LastCompletedPass != pass || Controller.PausedDueToHashMismatch)
			{
				Main.NewText("Test aborted", byte.MaxValue, 0, 0);
				yield break;
			}
		}
		Main.NewText("Test Completed Successfully", 0, byte.MaxValue, 0);
	}

	public static IEnumerable<object> TestResumeFromSnapshots()
	{
		foreach (bool item in TestSetupResetAndCreateSnapshots())
		{
			if (item)
			{
				yield return null;
				continue;
			}
			yield break;
		}
		GenPass lastPass = Controller.Passes.Last();
		foreach (GenPass pass in Controller.Passes.Where((GenPass p) => p.Enabled).Reverse())
		{
			Controller.TryRunToEndOfPass(pass);
			yield return null;
			if (Controller.LastCompletedPass != pass || Controller.PausedDueToHashMismatch)
			{
				Main.NewText("Test aborted", byte.MaxValue, 0, 0);
				yield break;
			}
		}
		Main.NewText("Single pass rerun test completed successfully", 0, byte.MaxValue, 0);
		foreach (GenPass item2 in Controller.Passes.Where((GenPass p) => p.Enabled))
		{
			Controller.TryResetToSnapshot(item2);
			Controller.TryRunToEndOfPass(lastPass, useSnapshots: false);
			yield return null;
			if (Controller.LastCompletedPass != lastPass || Controller.PausedDueToHashMismatch)
			{
				Main.NewText("Test aborted", byte.MaxValue, 0, 0);
				yield break;
			}
		}
		Main.NewText("Load snapshot and run to end test completed successfully", 0, byte.MaxValue, 0);
		foreach (GenPass item3 in Controller.Passes.Where((GenPass p) => p.Enabled))
		{
			Controller.TryReset();
			Controller.TryResetToSnapshot(item3);
			Controller.TryRunToEndOfPass(lastPass, useSnapshots: false);
			yield return null;
			if (Controller.LastCompletedPass != lastPass || Controller.PausedDueToHashMismatch)
			{
				Main.NewText("Test aborted", byte.MaxValue, 0, 0);
				yield break;
			}
		}
		Main.NewText("Clean load snapshot and run to end test completed successfully", 0, byte.MaxValue, 0);
	}
}
