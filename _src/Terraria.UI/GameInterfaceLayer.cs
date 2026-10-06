using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.UI;

public class GameInterfaceLayer
{
	public readonly string Name;

	public InterfaceScaleType ScaleType;

	public GameInterfaceLayer(string name, InterfaceScaleType scaleType)
	{
		Name = name;
		ScaleType = scaleType;
	}

	public bool Draw()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Matrix val;
		if (ScaleType == InterfaceScaleType.Game)
		{
			PlayerInput.SetZoom_World();
			val = Main.GameViewMatrix.ZoomMatrix;
		}
		else if (ScaleType == InterfaceScaleType.UI)
		{
			PlayerInput.SetZoom_UI();
			val = Main.UIScaleMatrix;
		}
		else
		{
			PlayerInput.SetZoom_Unscaled();
			val = Matrix.Identity;
		}
		bool result = false;
		Main.spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, val);
		try
		{
			result = DrawSelf();
		}
		catch (Exception e)
		{
			TimeLogger.DrawException(e);
		}
		Main.spriteBatch.End();
		return result;
	}

	protected virtual bool DrawSelf()
	{
		return true;
	}
}
