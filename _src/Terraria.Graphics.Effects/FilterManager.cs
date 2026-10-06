using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.IO;

namespace Terraria.Graphics.Effects;

public class FilterManager : EffectManager<Filter>
{
	private const float OPACITY_RATE = 1f;

	private LinkedList<Filter> _activeFilters = new LinkedList<Filter>();

	private int _filterLimit = 16;

	private EffectPriority _priorityThreshold;

	private int _activeFilterCount;

	private bool _captureThisFrame;

	public void BindTo(Preferences preferences)
	{
		preferences.OnSave += Configuration_OnSave;
		preferences.OnLoad += Configuration_OnLoad;
	}

	private void Configuration_OnSave(Preferences preferences)
	{
		preferences.Put("FilterLimit", _filterLimit);
		preferences.Put("FilterPriorityThreshold", Enum.GetName(typeof(EffectPriority), _priorityThreshold));
	}

	private void Configuration_OnLoad(Preferences preferences)
	{
		_filterLimit = preferences.Get("FilterLimit", 16);
		if (Enum.TryParse<EffectPriority>(preferences.Get("FilterPriorityThreshold", "VeryLow"), out var result))
		{
			_priorityThreshold = result;
		}
	}

	public override void OnActivate(Filter effect, Vector2 position)
	{
		if (_activeFilters.Contains(effect))
		{
			if (effect.Active)
			{
				return;
			}
			if (effect.Priority >= _priorityThreshold)
			{
				_activeFilterCount--;
			}
			_activeFilters.Remove(effect);
		}
		else
		{
			effect.Opacity = 0f;
		}
		if (effect.Priority >= _priorityThreshold)
		{
			_activeFilterCount++;
		}
		if (_activeFilters.Count == 0)
		{
			_activeFilters.AddLast(effect);
			return;
		}
		for (LinkedListNode<Filter> linkedListNode = _activeFilters.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			Filter value = linkedListNode.Value;
			if (effect.Priority <= value.Priority)
			{
				_activeFilters.AddAfter(linkedListNode, effect);
				return;
			}
		}
		_activeFilters.AddLast(effect);
	}

	public void BeginCapture(RenderTarget2D screenTarget1)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		_captureThisFrame = true;
		((Game)Main.instance).GraphicsDevice.SetRenderTarget(screenTarget1);
		((Game)Main.instance).GraphicsDevice.Clear(Color.Transparent);
	}

	public void Update(GameTime gameTime)
	{
		LinkedListNode<Filter> linkedListNode = _activeFilters.First;
		_ = _activeFilters.Count;
		int num = 0;
		while (linkedListNode != null)
		{
			Filter value = linkedListNode.Value;
			LinkedListNode<Filter> next = linkedListNode.Next;
			bool flag = false;
			if (value.Priority >= _priorityThreshold)
			{
				num++;
				if (num > _activeFilterCount - _filterLimit)
				{
					value.Update(gameTime);
					flag = true;
				}
			}
			if (value.Active & flag)
			{
				value.Opacity = Math.Min(value.Opacity + (float)gameTime.ElapsedGameTime.TotalSeconds * 1f, 1f);
			}
			else
			{
				value.Opacity = Math.Max(value.Opacity - (float)gameTime.ElapsedGameTime.TotalSeconds * 1f, 0f);
			}
			if (!value.Active && value.Opacity == 0f)
			{
				if (value.Priority >= _priorityThreshold)
				{
					_activeFilterCount--;
				}
				_activeFilters.Remove(linkedListNode);
			}
			linkedListNode = next;
		}
	}

	public void EndCapture(RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		EndCapture(finalTexture, screenTarget1, screenTarget2, ((Texture2D)(object)screenTarget1).Size(), ((Texture2D)(object)screenTarget1).Size(), Vector2.Zero);
	}

	public void EndCapture(RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Vector2 screenSize, Vector2 sceneSize, Vector2 sceneOffset)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		if (!_captureThisFrame)
		{
			return;
		}
		_captureThisFrame = false;
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		Rectangle value = new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y);
		RenderTarget2D t = screenTarget1;
		RenderTarget2D t2 = screenTarget2;
		GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
		graphicsDevice.SetRenderTarget(t2);
		graphicsDevice.Clear(Color.Transparent);
		Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
		SpriteEffects effects = Main.GameViewMatrix.Effects;
		Main.spriteBatch.Draw((Texture2D)(object)Main.skyTarget, Vector2.Zero, (Rectangle?)value, Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
		Main.spriteBatch.Draw((Texture2D)(object)t, Vector2.Zero, (Rectangle?)value, Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
		Main.spriteBatch.End();
		Utils.Swap(ref t2, ref t);
		int num = 0;
		LinkedListNode<Filter> linkedListNode = _activeFilters.First;
		Filter filter = null;
		while (linkedListNode != null)
		{
			Filter value2 = linkedListNode.Value;
			LinkedListNode<Filter> next = linkedListNode.Next;
			if (value2.Priority >= _priorityThreshold)
			{
				num++;
				if (num > _activeFilterCount - _filterLimit && value2.IsVisible())
				{
					if (filter != null)
					{
						graphicsDevice.SetRenderTarget(t2);
						graphicsDevice.Clear(Color.Transparent);
						Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
						filter.Apply(((Texture2D)(object)t).Size(), sceneSize, sceneOffset);
						Main.spriteBatch.Draw((Texture2D)(object)t, Vector2.Zero, (Rectangle?)value, Main.ColorOfTheSkies);
						Main.spriteBatch.End();
						Utils.Swap(ref t2, ref t);
					}
					filter = value2;
				}
			}
			linkedListNode = next;
		}
		graphicsDevice.SetRenderTarget(finalTexture);
		graphicsDevice.Clear(Color.Transparent);
		if (Main.player[Main.myPlayer].gravDir == -1f)
		{
			Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.EffectMatrix);
		}
		else
		{
			Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
		}
		if (filter != null)
		{
			filter.Apply(((Texture2D)(object)t).Size(), sceneSize, sceneOffset);
			Main.spriteBatch.Draw((Texture2D)(object)t, Vector2.Zero, (Rectangle?)value, Main.ColorOfTheSkies);
		}
		else
		{
			Main.spriteBatch.Draw((Texture2D)(object)t, Vector2.Zero, (Rectangle?)value, Color.White);
		}
		Main.spriteBatch.End();
		for (int i = 0; i < 8; i++)
		{
			graphicsDevice.Textures[i] = null;
		}
		TimeLogger.Filters.AddTime(fromTimestamp);
	}

	public bool HasActiveFilter()
	{
		return _activeFilters.Count != 0;
	}

	public bool CanCapture()
	{
		return HasActiveFilter();
	}
}
