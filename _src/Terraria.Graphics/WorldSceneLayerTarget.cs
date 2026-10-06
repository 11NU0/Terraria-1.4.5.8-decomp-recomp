using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class WorldSceneLayerTarget
{
	private readonly RenderTarget2D _target;

	private Vector2 _position;

	public Texture2D Texture => (Texture2D)(object)_target;

	public Vector2 Position
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _position;
		}
	}

	public bool IsPartiallyOffscreen
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			if (_position == Vector2.Zero)
			{
				return true;
			}
			Vector2 val = new Vector2((float)((Texture2D)_target).Width, (float)((Texture2D)_target).Height);
			Vector2 val2 = Position + val / 2f - Main.Camera.Center;
			Vector2 val3 = (val - Main.Camera.ScaledSize) / 2f;
			if (!(Math.Abs(val2.X) > val3.X))
			{
				return Math.Abs(val2.Y) > val3.Y;
			}
			return true;
		}
	}

	public bool IsContentLost => _target.IsContentLost;

	public WorldSceneLayerTarget(GraphicsDevice graphicsDevice, int width, int height)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected Obj, but got Unknown
		_target = new RenderTarget2D(graphicsDevice, width, height, false, graphicsDevice.PresentationParameters.BackBufferFormat, (DepthFormat)0);
	}

	public void UpdateContent(Action render)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		Vector2 screenPosition = Main.screenPosition;
		Point screenSize = Main.ScreenSize;
		Vector2 zoom = Main.GameViewMatrix.Zoom;
		Vector2 center = Main.Camera.Center;
		Main.screenWidth = ((Texture2D)_target).Width - Main.offScreenRange * 2;
		Main.screenHeight = ((Texture2D)_target).Height - Main.offScreenRange * 2;
		Main.screenPosition = Utils.Round(center - Main.ScreenSize.ToVector2() / 2f);
		Main.GameViewMatrix.Zoom = Vector2.One;
		GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
		RenderTargetBinding[] renderTargets = graphicsDevice.GetRenderTargets();
		graphicsDevice.SetRenderTarget(_target);
		graphicsDevice.Clear(Color.Transparent);
		_position = Main.screenPosition - new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
		render();
		graphicsDevice.SetRenderTargets(renderTargets);
		Main.screenPosition = screenPosition;
		Main.screenWidth = screenSize.X;
		Main.screenHeight = screenSize.Y;
		Main.GameViewMatrix.Zoom = zoom;
	}

	public void Dispose()
	{
		((GraphicsResource)_target).Dispose();
	}
}
