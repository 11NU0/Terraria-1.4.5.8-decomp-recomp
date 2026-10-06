using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UIWorldCreationAdvancedSecretSeedsList : UIState, IHaveBackButtonCommand
{
	private UIWorldCreationAdvanced _creationState;

	private UIElement _backButton;

	private UIList _worldList;

	private UIElement _containerPanel;

	private UIScrollbar _scrollbar;

	private bool _isScrollbarAttached;

	private UIWorldCreation _creationState2;

	private ParticleRenderer SeedParticleSystem = new ParticleRenderer();

	private UIDust SeedDust = new UIDust();

	private UIText _descriptionText;

	private UIGamepadHelper _helper;

	public UIWorldCreationAdvancedSecretSeedsList(UIWorldCreationAdvanced state, UIWorldCreation state2)
	{
		_creationState = state;
		_creationState2 = state2;
		BuildPage();
	}

	private void BuildPage()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		SeedDust.Clear();
		SeedParticleSystem.Clear();
		RemoveAllChildren();
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixels(500f),
			Height = StyleDimension.FromPixelsAndPercent(-200f, 1f),
			Top = StyleDimension.FromPixels(202f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.MaxHeight = StyleDimension.FromPixels(400f);
		uIElement.SetPadding(0f);
		Append(uIElement);
		UIPanel uIPanel = new UIPanel
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixelsAndPercent(-102f, 1f),
			BackgroundColor = new Color(33, 43, 79) * 0.8f
		};
		uIPanel.SetPadding(0f);
		uIElement.Append(uIPanel);
		MakeBackAndCreatebuttons(uIElement);
		int num = 56;
		int num2 = 4;
		UIElement uIElement2 = new UIElement
		{
			Top = StyleDimension.FromPixelsAndPercent(num2, 0f),
			Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
			Left = StyleDimension.FromPixelsAndPercent(2f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(-num2 - num, 1f),
			HAlign = 0.5f
		};
		uIElement2.SetPadding(0f);
		uIElement2.PaddingTop = 8f;
		uIElement2.PaddingBottom = 12f;
		uIPanel.Append(uIElement2);
		_worldList = new UIList();
		_worldList.Width.Set(0f, 1f);
		_worldList.Height.Set(0f, 1f);
		_worldList.ListPadding = 5f;
		uIElement2.Append(_worldList);
		_containerPanel = uIElement2;
		_scrollbar = new UIScrollbar();
		_scrollbar.SetView(100f, 1000f);
		_scrollbar.Height.Set(0f, 1f);
		_scrollbar.HAlign = 1f;
		_worldList.SetScrollbar(_scrollbar);
		List<WorldGen.SecretSeed> seedsForInterface = SecretSeedsTracker.SeedsForInterface;
		_worldList.ManualSortMethod = CustomSort;
		int num3 = 0;
		foreach (WorldGen.SecretSeed item in seedsForInterface)
		{
			GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = new GroupOptionButton<WorldGen.SecretSeed>(item, null, Language.GetText(item.Localization), Color.White, null)
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = new StyleDimension(40f, 0f),
				HAlign = 0f
			};
			groupOptionButton.SetSnapPoint("Seed", num3++);
			UIElement uIElement3 = new UIElement();
			groupOptionButton.Append(uIElement3);
			groupOptionButton.SetTextWithoutLocalization(item.TextThatWasUsedToUnlock, 1f, Color.White, 0f, 10f);
			groupOptionButton.OnLeftMouseDown += ClickSecretSeed;
			groupOptionButton.OnMouseOver += MouseOverSeed;
			groupOptionButton.OnMouseOut += MouseOutSeed;
			groupOptionButton.SetCurrentOption(item.Enabled ? item : null);
			uIElement3.OnDraw += DrawGlowRing;
			_worldList.Add(groupOptionButton);
		}
		UIElement uIElement4 = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(num + num2, 0f),
			HAlign = 0.5f,
			VAlign = 1f
		};
		uIElement4.SetPadding(0f);
		uIElement4.PaddingBottom = 12f;
		uIPanel.Append(uIElement4);
		AddDescriptionPanel(uIElement4, num, "desc");
	}

	private void AddDescriptionPanel(UIElement container, float accumulatedHeight, string tagGroup)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1))
		{
			HAlign = 0.5f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
			Left = StyleDimension.FromPixels(0f - num),
			Height = StyleDimension.FromPixelsAndPercent(accumulatedHeight, 0f),
			Top = StyleDimension.FromPixels(2f)
		};
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = Color.LightGray * 0.7f;
		container.Append(uISlicedImage);
		UIText uIText = new UIText(Language.GetText("UI.WorldDescriptionDefault"), 0.7f)
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(2f, 0f)
		};
		uIText.IsWrapped = true;
		uIText.PaddingLeft = 20f;
		uIText.PaddingRight = 20f;
		uIText.PaddingTop = 4f;
		uISlicedImage.Append(uIText);
		_descriptionText = uIText;
	}

	public void MouseOutSeed(UIMouseEvent evt, UIElement listeningElement)
	{
		ClearOptionDescription(evt, listeningElement);
	}

	public void MouseOverSeed(UIMouseEvent evt, UIElement listeningElement)
	{
		if (evt.Target is GroupOptionButton<WorldGen.SecretSeed> groupOptionButton)
		{
			_ = groupOptionButton.IsSelected;
			if (Main.mouseLeft)
			{
				listeningElement.LeftMouseDown(evt);
			}
			ShowOptionDescription(evt, listeningElement);
		}
	}

	public void DrawGlowRing(UIElement listeningElement, SpriteBatch spriteBatch)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = (GroupOptionButton<WorldGen.SecretSeed>)listeningElement.Parent;
		if (groupOptionButton.OptionValue.Enabled)
		{
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconRandomSeed", (AssetRequestMode)1);
			CalculatedStyle dimensions = groupOptionButton.GetDimensions();
			Vector2 val2 = dimensions.ToRectangle().TopRight() + new Vector2(-22f, 22f);
			Texture2D value = val.Value;
			Rectangle r = new Rectangle(0, 0, 4, 4);
			Vector2 val3 = new Vector2((float)value.Width * 0.45f, (float)value.Height * 0.95f);
			float num = 0.25f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.3f + dimensions.Position().Y);
			num = (float)Math.PI / 4f;
			val3 = r.Size() / 2f;
			float num2 = 1.5f;
			_ = num2 + 1f;
			Math.Sin(Main.GlobalTimeWrappedHourly * 1.3f + dimensions.Position().Y * 0.00153178f);
			num2 = 1f;
			num = 0f;
			r = value.Frame();
			val3 = r.Size() / 2f;
			spriteBatch.Draw(value, val2, (Rectangle?)r, Color.White, num, val3, num2, (SpriteEffects)0, 0f);
		}
	}

	private void CustomSort(List<UIElement> items)
	{
		items.Sort((UIElement a, UIElement b) =>
		{
			GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = a as GroupOptionButton<WorldGen.SecretSeed>;
			GroupOptionButton<WorldGen.SecretSeed> groupOptionButton2 = b as GroupOptionButton<WorldGen.SecretSeed>;
			if (groupOptionButton != null && groupOptionButton2 == null)
			{
				return -1;
			}
			return (groupOptionButton == null && groupOptionButton2 != null) ? 1 : groupOptionButton.OptionValue.TextThatWasUsedToUnlock.CompareTo(groupOptionButton2.OptionValue.TextThatWasUsedToUnlock);
		});
	}

	private void ClickSecretSeed(UIMouseEvent evt, UIElement listeningElement)
	{
		GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = (GroupOptionButton<WorldGen.SecretSeed>)listeningElement;
		WorldGen.SecretSeed optionValue = groupOptionButton.OptionValue;
		if (optionValue.Enabled)
		{
			groupOptionButton.SetCurrentOption(null);
			WorldGen.SecretSeed.Disable(optionValue);
			_creationState2.RemoveSeedFromSeedMenu(optionValue.TextThatWasUsedToUnlock);
		}
		else
		{
			groupOptionButton.SetCurrentOption(optionValue);
			WorldGen.SecretSeed.Enable(optionValue);
			_creationState2.AddSeedFromSeedmenu(optionValue.TextThatWasUsedToUnlock);
			SpawnParticles(groupOptionButton);
		}
	}

	private void SpawnParticles(GroupOptionButton<WorldGen.SecretSeed> element)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = element.GetDimensions();
		dimensions.Center();
		Spawn_RainbowRodHit(new ParticleOrchestraSettings
		{
			PositionInWorld = dimensions.Position() + new Vector2(dimensions.Width - 20f, dimensions.Height / 2f),
			MovementVector = new Vector2(0f, 16f) + Main.rand.NextVector2Circular(10f, 2f)
		});
		float num = 8f;
		for (int i = 0; (float)i < num + 1f; i++)
		{
			Spawn_BestReforge(new ParticleOrchestraSettings
			{
				PositionInWorld = dimensions.Position() + new Vector2(0f, dimensions.Height / 2f) + new Vector2(dimensions.Width * (1f / num) * (float)i, 0f)
			});
		}
	}

	private void Spawn_RainbowRodHit(ParticleOrchestraSettings settings)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 6f;
		float num3 = Main.rand.NextFloat();
		for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
		{
			Vector2 val = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
			Vector2 val2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 val3 = 1.5f * val2;
			float num5 = 60f;
			Vector2 val4 = Main.rand.NextVector2Circular(8f, 8f) * val2;
			PrettySparkleParticle prettySparkleParticle = new PrettySparkleParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num5) - val * 1f / 60f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			prettySparkleParticle.ColorTint.A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2;
			SeedParticleSystem.Add(prettySparkleParticle);
			prettySparkleParticle = new PrettySparkleParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num5) - val * 1f / 60f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2 * 0.6f;
			SeedParticleSystem.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 12; i++)
		{
			Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			Dust dust = SeedDust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			dust.velocity = Main.rand.NextVector2Circular(1f, 1f);
			dust.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
			dust.noGravity = true;
			dust.scale = 0.6f + Main.rand.NextFloat() * 0.9f;
			dust.fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
			if (dust.dustIndex != 200 && dust.type != 0)
			{
				Dust dust2 = SeedDust.CloneDust(dust);
				dust2.scale /= 2f;
				dust2.fadeIn *= 0.75f;
				dust2.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private void Spawn_BestReforge(ParticleOrchestraSettings settings)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
		Asset<Texture2D> textureAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", (AssetRequestMode)1);
		for (int i = 0; i < 2; i++)
		{
			Vector2 val = Main.rand.NextVector2Circular(3f, 4f);
			Vector2 val2 = new Vector2(0f, Main.rand.NextFloatDirection() * 20f);
			SeedParticleSystem.Add(new CreativeSacrificeParticle(textureAsset, null, settings.MovementVector + val, settings.PositionInWorld + val2)
			{
				AccelerationPerFrame = accelerationPerFrame,
				ScaleOffsetPerFrame = -1f / 60f
			});
		}
		float num = Main.rand.NextFloat();
		for (int j = 0; j < 3; j++)
		{
			Color newColor = Main.hslToRgb((num + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			Dust dust = SeedDust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			dust.velocity = Main.rand.NextVector2Circular(1f, 1f);
			dust.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
			dust.noGravity = true;
			dust.scale = 0.6f + Main.rand.NextFloat() * 0.9f;
			dust.fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
			Vector2 val3 = new Vector2(0f, Main.rand.NextFloatDirection() * 20f);
			dust.position += val3;
			if (dust.dustIndex != 200 && dust.type != 0)
			{
				Dust dust2 = SeedDust.CloneDust(dust);
				dust2.scale /= 2f;
				dust2.fadeIn *= 0.75f;
				dust2.color = new Color(255, 255, 255, 255);
			}
		}
	}

	public override void Recalculate()
	{
		if (_scrollbar != null)
		{
			if (_isScrollbarAttached && !_scrollbar.CanScroll)
			{
				_containerPanel.RemoveChild(_scrollbar);
				_isScrollbarAttached = false;
				_worldList.Width.Set(0f, 1f);
			}
			else if (!_isScrollbarAttached && _scrollbar.CanScroll)
			{
				_containerPanel.Append(_scrollbar);
				_isScrollbarAttached = true;
				_worldList.Width.Set(-25f, 1f);
			}
		}
		base.Recalculate();
	}

	private void MakeBackAndCreatebuttons(UIElement outerContainer)
	{
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Apply"), 0.65f, large: true)
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
			Height = StyleDimension.FromPixels(50f),
			VAlign = 1f,
			HAlign = 0.5f,
			Top = StyleDimension.FromPixels(-43f)
		};
		uITextPanel.OnMouseOver += FadedMouseOver;
		uITextPanel.OnMouseOut += FadedMouseOut;
		uITextPanel.OnLeftMouseDown += Click_GoBack;
		uITextPanel.SetSnapPoint("Back", 0);
		outerContainer.Append(uITextPanel);
		_backButton = uITextPanel;
	}

	private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
	{
		GoBack();
	}

	private void GoBack()
	{
		SoundEngine.PlaySound(11);
		Main.MenuUI.SetState(_creationState);
		_creationState.RefreshSecretSeedButton();
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		ShowOptionDescription(evt, listeningElement);
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
		ClearOptionDescription(evt, listeningElement);
	}

	public void HandleBackButtonUsage()
	{
		GoBack();
	}

	public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		_descriptionText.SetText(Language.GetText("UI.WorldDescriptionDefault"));
	}

	public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		LocalizedText localizedText = null;
		if (listeningElement is GroupOptionButton<WorldGen.SecretSeed> groupOptionButton)
		{
			localizedText = groupOptionButton.Description;
		}
		if (localizedText != null)
		{
			_descriptionText.SetText(localizedText);
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
		DrawSeedSystems(spriteBatch);
	}

	public void DrawSeedSystems(SpriteBatch spriteBatch)
	{
		SeedDust.UpdateDust();
		SeedDust.DrawDust();
		SeedParticleSystem.Update();
		SeedParticleSystem.Draw(spriteBatch);
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
		int num = 3000;
		int currentID = num;
		GetSnapPoints();
		UILinkPoint linkPoint = _helper.GetLinkPoint(currentID++, _backButton);
		List<SnapPoint> snapPoints = _worldList.GetSnapPoints();
		UILinkPoint[,] array = _helper.CreateUILinkPointGrid(ref currentID, snapPoints, 1, null, null, null, linkPoint);
		UILinkPoint upSide = array[0, array.GetLength(1) - 1];
		_helper.PairUpDown(upSide, linkPoint);
		_helper.MoveToVisuallyClosestPoint(num, currentID);
	}
}
