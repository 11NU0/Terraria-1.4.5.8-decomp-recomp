using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent;

public class TilePaintSystemV2
{
	public abstract class ARenderTargetHolder
	{
		public RenderTarget2D Target;

		protected bool _wasPrepared;

		public bool IsReady => _wasPrepared;

		public abstract void Prepare();

		public abstract void PrepareShader();

		public void Clear()
		{
			if (Target != null && !((GraphicsResource)Target).IsDisposed)
			{
				((GraphicsResource)Target).Dispose();
			}
		}

		protected void PrepareTextureIfNecessary(Texture2D originalTexture, Rectangle? sourceRect = null)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected Obj, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (Target == null || Target.IsContentLost)
			{
				Main instance = Main.instance;
				if (!sourceRect.HasValue)
				{
					sourceRect = originalTexture.Frame();
				}
				Target = new RenderTarget2D(((Game)instance).GraphicsDevice, sourceRect.Value.Width, sourceRect.Value.Height, false, ((Game)instance).GraphicsDevice.PresentationParameters.BackBufferFormat, (DepthFormat)0, 0, (RenderTargetUsage)1);
				Target.ContentLost += Target_ContentLost;
				((GraphicsResource)Target).Disposing += Target_Disposing;
				((GraphicsResource)Target).Name = ((GraphicsResource)originalTexture).Name;
				((Game)instance).GraphicsDevice.SetRenderTarget(Target);
				((Game)instance).GraphicsDevice.Clear(Color.Transparent);
				Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
				PrepareShader();
				Rectangle value = sourceRect.Value;
				value.X = 0;
				value.Y = 0;
				Main.spriteBatch.Draw(originalTexture, value, Color.White);
				Main.spriteBatch.End();
				((Game)instance).GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
				_wasPrepared = true;
			}
		}

		private void Target_Disposing(object sender, EventArgs e)
		{
			_wasPrepared = false;
			Target = null;
		}

		private void Target_ContentLost(object sender, EventArgs e)
		{
			_wasPrepared = false;
		}

		protected void PrepareShader(int paintColor, TreePaintingSettings settings)
		{
			Effect tileShader = Main.tileShader;
			tileShader.Parameters["leafHueTestOffset"].SetValue(settings.HueTestOffset);
			tileShader.Parameters["leafMinHue"].SetValue(settings.SpecialGroupMinimalHueValue);
			tileShader.Parameters["leafMaxHue"].SetValue(settings.SpecialGroupMaximumHueValue);
			tileShader.Parameters["leafMinSat"].SetValue(settings.SpecialGroupMinimumSaturationValue);
			tileShader.Parameters["leafMaxSat"].SetValue(settings.SpecialGroupMaximumSaturationValue);
			tileShader.Parameters["invertSpecialGroupResult"].SetValue(settings.InvertSpecialGroupResult);
			int num = Main.ConvertPaintIdToTileShaderIndex(paintColor, settings.UseSpecialGroups, settings.UseWallShaderHacks);
			tileShader.CurrentTechnique.Passes[num].Apply();
			RenderTarget2D target = Target;
			((GraphicsResource)target).Name = ((GraphicsResource)target).Name + " paint: " + paintColor;
		}
	}

	public class TreeTopRenderTargetHolder : ARenderTargetHolder
	{
		public TreeFoliageVariantKey Key;

		public override void Prepare()
		{
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.TreeTop[Key.TextureIndex].Name, (AssetRequestMode)1);
			PrepareTextureIfNecessary(val.Value);
		}

		public override void PrepareShader()
		{
			PrepareShader(Key.PaintColor, TreePaintSystemData.GetTreeFoliageSettings(Key.TextureIndex, Key.TextureStyle));
		}
	}

	public class TreeBranchTargetHolder : ARenderTargetHolder
	{
		public TreeFoliageVariantKey Key;

		public override void Prepare()
		{
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.TreeBranch[Key.TextureIndex].Name, (AssetRequestMode)1);
			PrepareTextureIfNecessary(val.Value);
		}

		public override void PrepareShader()
		{
			PrepareShader(Key.PaintColor, TreePaintSystemData.GetTreeFoliageSettings(Key.TextureIndex, Key.TextureStyle));
		}
	}

	public class TileRenderTargetHolder : ARenderTargetHolder
	{
		public TileVariationkey Key;

		public override void Prepare()
		{
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.Tile[Key.TileType].Name, (AssetRequestMode)1);
			PrepareTextureIfNecessary(val.Value);
		}

		public override void PrepareShader()
		{
			PrepareShader(Key.PaintColor, TreePaintSystemData.GetTileSettings(Key.TileType, Key.TileStyle));
		}
	}

	public class CageTopRenderTargetHolder : ARenderTargetHolder
	{
		public CageTopVariationkey Key;

		public override void Prepare()
		{
			PrepareTextureIfNecessary(TextureAssets.CageTop[Key.CageStyle].Value);
		}

		public override void PrepareShader()
		{
			PrepareShader(Key.PaintColor, TreePaintSystemData.GetCageTopSettings());
		}
	}

	public class WallRenderTargetHolder : ARenderTargetHolder
	{
		public WallVariationKey Key;

		public override void Prepare()
		{
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.Wall[Key.WallType].Name, (AssetRequestMode)1);
			PrepareTextureIfNecessary(val.Value);
		}

		public override void PrepareShader()
		{
			PrepareShader(Key.PaintColor, TreePaintSystemData.GetWallSettings(Key.WallType));
		}
	}

	public struct TileVariationkey : IEquatable<TileVariationkey>
	{
		public int TileType;

		public int TileStyle;

		public int PaintColor;

		public bool Equals(TileVariationkey other)
		{
			if (TileType == other.TileType && TileStyle == other.TileStyle)
			{
				return PaintColor == other.PaintColor;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is TileVariationkey)
			{
				return Equals((TileVariationkey)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((TileType * 397) ^ TileStyle) * 397) ^ PaintColor;
		}

		public static bool operator ==(TileVariationkey left, TileVariationkey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TileVariationkey left, TileVariationkey right)
		{
			return !left.Equals(right);
		}
	}

	public struct WallVariationKey : IEquatable<WallVariationKey>
	{
		public int WallType;

		public int PaintColor;

		public bool Equals(WallVariationKey other)
		{
			if (WallType == other.WallType)
			{
				return PaintColor == other.PaintColor;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is WallVariationKey)
			{
				return Equals((WallVariationKey)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (WallType * 397) ^ PaintColor;
		}

		public static bool operator ==(WallVariationKey left, WallVariationKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(WallVariationKey left, WallVariationKey right)
		{
			return !left.Equals(right);
		}
	}

	public struct TreeFoliageVariantKey : IEquatable<TreeFoliageVariantKey>
	{
		public int TextureIndex;

		public int TextureStyle;

		public int PaintColor;

		public bool Equals(TreeFoliageVariantKey other)
		{
			if (TextureIndex == other.TextureIndex && TextureStyle == other.TextureStyle)
			{
				return PaintColor == other.PaintColor;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is TreeFoliageVariantKey)
			{
				return Equals((TreeFoliageVariantKey)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((TextureIndex * 397) ^ TextureStyle) * 397) ^ PaintColor;
		}

		public static bool operator ==(TreeFoliageVariantKey left, TreeFoliageVariantKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TreeFoliageVariantKey left, TreeFoliageVariantKey right)
		{
			return !left.Equals(right);
		}
	}

	public struct CageTopVariationkey
	{
		public int CageStyle;

		public int PaintColor;

		public bool Equals(CageTopVariationkey other)
		{
			if (CageStyle == other.CageStyle)
			{
				return PaintColor == other.PaintColor;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is CageTopVariationkey)
			{
				return Equals((CageTopVariationkey)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (CageStyle * 397) ^ PaintColor;
		}

		public static bool operator ==(CageTopVariationkey left, CageTopVariationkey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(CageTopVariationkey left, CageTopVariationkey right)
		{
			return !left.Equals(right);
		}
	}

	private Dictionary<CageTopVariationkey, CageTopRenderTargetHolder> _cageTopRenders = new Dictionary<CageTopVariationkey, CageTopRenderTargetHolder>();

	private Dictionary<TileVariationkey, TileRenderTargetHolder> _tilesRenders = new Dictionary<TileVariationkey, TileRenderTargetHolder>();

	private Dictionary<WallVariationKey, WallRenderTargetHolder> _wallsRenders = new Dictionary<WallVariationKey, WallRenderTargetHolder>();

	private Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder> _treeTopRenders = new Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder>();

	private Dictionary<TreeFoliageVariantKey, TreeBranchTargetHolder> _treeBranchRenders = new Dictionary<TreeFoliageVariantKey, TreeBranchTargetHolder>();

	private List<ARenderTargetHolder> _requests = new List<ARenderTargetHolder>();

	public void Reset()
	{
		foreach (TileRenderTargetHolder value in _tilesRenders.Values)
		{
			value.Clear();
		}
		_tilesRenders.Clear();
		foreach (CageTopRenderTargetHolder value2 in _cageTopRenders.Values)
		{
			value2.Clear();
		}
		_cageTopRenders.Clear();
		foreach (WallRenderTargetHolder value3 in _wallsRenders.Values)
		{
			value3.Clear();
		}
		_wallsRenders.Clear();
		foreach (TreeTopRenderTargetHolder value4 in _treeTopRenders.Values)
		{
			value4.Clear();
		}
		_treeTopRenders.Clear();
		foreach (TreeBranchTargetHolder value5 in _treeBranchRenders.Values)
		{
			value5.Clear();
		}
		_treeBranchRenders.Clear();
		foreach (ARenderTargetHolder request in _requests)
		{
			request.Clear();
		}
		_requests.Clear();
	}

	public void RequestTile(ref TileVariationkey lookupKey)
	{
		if (!_tilesRenders.TryGetValue(lookupKey, out var value))
		{
			value = new TileRenderTargetHolder
			{
				Key = lookupKey
			};
			_tilesRenders.Add(lookupKey, value);
		}
		if (!value.IsReady)
		{
			_requests.Add(value);
		}
	}

	public void RequestCageTop(ref CageTopVariationkey lookupKey)
	{
		if (!_cageTopRenders.TryGetValue(lookupKey, out var value))
		{
			value = new CageTopRenderTargetHolder
			{
				Key = lookupKey
			};
			_cageTopRenders.Add(lookupKey, value);
		}
		if (!value.IsReady)
		{
			_requests.Add(value);
		}
	}

	private void RequestTile_CheckForRelatedTileRequests(ref TileVariationkey lookupKey)
	{
		if (lookupKey.TileType == 83)
		{
			TileVariationkey lookupKey2 = new TileVariationkey
			{
				TileType = 84,
				TileStyle = lookupKey.TileStyle,
				PaintColor = lookupKey.PaintColor
			};
			RequestTile(ref lookupKey2);
		}
	}

	public void RequestWall(ref WallVariationKey lookupKey)
	{
		if (!_wallsRenders.TryGetValue(lookupKey, out var value))
		{
			value = new WallRenderTargetHolder
			{
				Key = lookupKey
			};
			_wallsRenders.Add(lookupKey, value);
		}
		if (!value.IsReady)
		{
			_requests.Add(value);
		}
	}

	public void RequestTreeTop(ref TreeFoliageVariantKey lookupKey)
	{
		if (!_treeTopRenders.TryGetValue(lookupKey, out var value))
		{
			value = new TreeTopRenderTargetHolder
			{
				Key = lookupKey
			};
			_treeTopRenders.Add(lookupKey, value);
		}
		if (!value.IsReady)
		{
			_requests.Add(value);
		}
	}

	public void RequestTreeBranch(ref TreeFoliageVariantKey lookupKey)
	{
		if (!_treeBranchRenders.TryGetValue(lookupKey, out var value))
		{
			value = new TreeBranchTargetHolder
			{
				Key = lookupKey
			};
			_treeBranchRenders.Add(lookupKey, value);
		}
		if (!value.IsReady)
		{
			_requests.Add(value);
		}
	}

	public Texture2D TryGetTileAndRequestIfNotReady(int tileType, int tileStyle, int paintColor)
	{
		TileVariationkey lookupKey = new TileVariationkey
		{
			TileType = tileType,
			TileStyle = tileStyle,
			PaintColor = paintColor
		};
		if (_tilesRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
		{
			return (Texture2D)(object)value.Target;
		}
		RequestTile(ref lookupKey);
		return null;
	}

	public Texture2D TryGetCageTopAndRequestIfNotReady(int cageStyle, int paintColor)
	{
		CageTopVariationkey lookupKey = new CageTopVariationkey
		{
			CageStyle = cageStyle,
			PaintColor = paintColor
		};
		if (_cageTopRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
		{
			return (Texture2D)(object)value.Target;
		}
		RequestCageTop(ref lookupKey);
		return null;
	}

	public Texture2D TryGetWallAndRequestIfNotReady(int wallType, int paintColor)
	{
		WallVariationKey lookupKey = new WallVariationKey
		{
			WallType = wallType,
			PaintColor = paintColor
		};
		if (_wallsRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
		{
			return (Texture2D)(object)value.Target;
		}
		RequestWall(ref lookupKey);
		return null;
	}

	public Texture2D TryGetTreeTopAndRequestIfNotReady(int treeTopIndex, int treeTopStyle, int paintColor)
	{
		TreeFoliageVariantKey lookupKey = new TreeFoliageVariantKey
		{
			TextureIndex = treeTopIndex,
			TextureStyle = treeTopStyle,
			PaintColor = paintColor
		};
		if (_treeTopRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
		{
			return (Texture2D)(object)value.Target;
		}
		RequestTreeTop(ref lookupKey);
		return null;
	}

	public Texture2D TryGetTreeBranchAndRequestIfNotReady(int treeTopIndex, int treeTopStyle, int paintColor)
	{
		TreeFoliageVariantKey lookupKey = new TreeFoliageVariantKey
		{
			TextureIndex = treeTopIndex,
			TextureStyle = treeTopStyle,
			PaintColor = paintColor
		};
		if (_treeBranchRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
		{
			return (Texture2D)(object)value.Target;
		}
		RequestTreeBranch(ref lookupKey);
		return null;
	}

	public void PrepareAllRequests()
	{
		if (_requests.Count != 0)
		{
			for (int i = 0; i < _requests.Count; i++)
			{
				_requests[i].Prepare();
			}
			_requests.Clear();
		}
	}
}
