using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent;

public abstract class ARenderTargetContentByRequest : INeedRenderTargetContent
{
	protected RenderTarget2D _target;

	protected bool _wasPrepared;

	private bool _wasRequested;

	public bool IsReady => _wasPrepared;

	public void Request()
	{
		_wasRequested = true;
	}

	public RenderTarget2D GetTarget()
	{
		return _target;
	}

	public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
	{
		_wasPrepared = false;
		if (_wasRequested)
		{
			_wasRequested = false;
			HandleUseRequest(device, spriteBatch);
		}
	}

	protected abstract void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch);

	protected void PrepareARenderTarget_AndListenToEvents(ref RenderTarget2D target, GraphicsDevice device, int neededWidth, int neededHeight, RenderTargetUsage usage)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected Obj, but got Unknown
		if (target == null || ((GraphicsResource)target).IsDisposed || ((Texture2D)target).Width != neededWidth || ((Texture2D)target).Height != neededHeight)
		{
			if (target != null)
			{
				target.ContentLost -= target_ContentLost;
				((GraphicsResource)target).Disposing -= target_Disposing;
			}
			target = new RenderTarget2D(device, neededWidth, neededHeight, false, device.PresentationParameters.BackBufferFormat, (DepthFormat)0, 0, usage);
			target.ContentLost += target_ContentLost;
			((GraphicsResource)target).Disposing += target_Disposing;
		}
	}

	private void target_Disposing(object sender, EventArgs e)
	{
		_wasPrepared = false;
		_target = null;
	}

	private void target_ContentLost(object sender, EventArgs e)
	{
		_wasPrepared = false;
	}

	public void Reset()
	{
		_wasPrepared = false;
		_wasRequested = false;
		_target = null;
	}

	protected void PrepareARenderTarget_WithoutListeningToEvents(ref RenderTarget2D target, GraphicsDevice device, int neededWidth, int neededHeight, RenderTargetUsage usage)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected Obj, but got Unknown
		if (target == null || ((GraphicsResource)target).IsDisposed || ((Texture2D)target).Width != neededWidth || ((Texture2D)target).Height != neededHeight)
		{
			target = new RenderTarget2D(device, neededWidth, neededHeight, false, device.PresentationParameters.BackBufferFormat, (DepthFormat)0, 0, usage);
		}
	}
}
