using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Effects;
using Terraria.Localization;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Graphics.Capture;

internal class CaptureCamera : IDisposable
{
	private class CaptureChunk
	{
		public readonly Rectangle Area;

		public readonly Rectangle ScaledArea;

		public CaptureChunk(Rectangle area, Rectangle scaledArea)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Area = area;
			ScaledArea = scaledArea;
		}
	}

	private static bool CameraExists;

	public const int CHUNK_SIZE = 128;

	public const int FRAMEBUFFER_PIXEL_SIZE = 2048;

	public const int INNER_CHUNK_SIZE = 126;

	public const int MAX_IMAGE_SIZE = 4096;

	public const string CAPTURE_DIRECTORY = "Captures";

	private RenderTarget2D _frameBuffer;

	private RenderTarget2D _scaledFrameBuffer;

	private RenderTarget2D _filterFrameBuffer1;

	private RenderTarget2D _filterFrameBuffer2;

	private WorldSceneLayerTarget _waterTarget;

	private GraphicsDevice _graphics;

	private readonly object _captureLock = new object();

	private bool _isDisposed;

	private CaptureSettings _activeSettings;

	private Queue<CaptureChunk> _renderQueue = new Queue<CaptureChunk>();

	private SpriteBatch _spriteBatch;

	private byte[] _scaledFrameData;

	private byte[] _outputData;

	private Size _outputImageSize;

	private SamplerState _downscaleSampleState;

	private float _tilesProcessed;

	private float _totalTiles;

	public bool IsCapturing
	{
		get
		{
			Monitor.Enter(_captureLock);
			bool result = _activeSettings != null;
			Monitor.Exit(_captureLock);
			return result;
		}
	}

	public CaptureCamera(GraphicsDevice graphics)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected Obj, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected Obj, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected Obj, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected Obj, but got Unknown
		Invariant.Assert(!CameraExists, "Only one camera should exist at any given time.");
		CameraExists = true;
		_graphics = graphics;
		_spriteBatch = new SpriteBatch(graphics);
		try
		{
			_frameBuffer = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
			_filterFrameBuffer1 = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
			_filterFrameBuffer2 = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
			_waterTarget = new WorldSceneLayerTarget(graphics, 2048, 2048);
		}
		catch
		{
			Main.CaptureModeDisabled = true;
			return;
		}
		_downscaleSampleState = SamplerState.AnisotropicClamp;
	}

	public void Capture(CaptureSettings settings)
	{
		Main.GlobalTimerPaused = true;
		Invariant.Assert(!_isDisposed, "A capture should not be called after the camera is disposed.");
		if (_activeSettings != null)
		{
			throw new InvalidOperationException("Capture called while another capture was already active.");
		}
		try
		{
			lock (_captureLock)
			{
				_activeSettings = settings;
				_Capture();
			}
		}
		catch (OutOfMemoryException value)
		{
			Console.WriteLine(value);
			_renderQueue.Clear();
			_outputData = null;
			FinishCapture();
			Main.NewText(Language.GetTextValue("Error.CaptureOutOfMemory"), byte.MaxValue, 0, 0);
		}
	}

	private void _Capture()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected Obj, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		Rectangle area = _activeSettings.Area;
		float num = 1f;
		if (_activeSettings.UseScaling)
		{
			if (area.Width * 16 > 4096)
			{
				num = 4096f / (float)(area.Width * 16);
			}
			if (area.Height * 16 > 4096)
			{
				num = Math.Min(num, 4096f / (float)(area.Height * 16));
			}
			num = Math.Min(1f, num);
			_outputImageSize = new Size((int)MathHelper.Clamp((float)(int)(num * (float)(area.Width * 16)), 1f, 4096f), (int)MathHelper.Clamp((float)(int)(num * (float)(area.Height * 16)), 1f, 4096f));
			_outputData = new byte[4 * _outputImageSize.Width * _outputImageSize.Height];
			int num2 = (int)Math.Floor(num * 2048f);
			_scaledFrameData = new byte[4 * num2 * num2];
			_scaledFrameBuffer = new RenderTarget2D(_graphics, num2, num2, false, _graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
		}
		else
		{
			_outputData = new byte[16777216];
		}
		_tilesProcessed = 0f;
		_totalTiles = area.Width * area.Height;
		for (int i = area.X; i < area.X + area.Width; i += 126)
		{
			for (int j = area.Y; j < area.Y + area.Height; j += 126)
			{
				int num3 = Math.Min(128, area.X + area.Width - i);
				int num4 = Math.Min(128, area.Y + area.Height - j);
				int num5 = (int)Math.Floor(num * (float)(num3 * 16));
				int num6 = (int)Math.Floor(num * (float)(num4 * 16));
				int num7 = (int)Math.Floor(num * (float)((i - area.X) * 16));
				int num8 = (int)Math.Floor(num * (float)((j - area.Y) * 16));
				_renderQueue.Enqueue(new CaptureChunk(new Rectangle(i, j, num3, num4), new Rectangle(num7, num8, num5, num6)));
			}
		}
	}

	public void DrawTick()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		Monitor.Enter(_captureLock);
		if (_activeSettings == null)
		{
			return;
		}
		if (_renderQueue.Count > 0)
		{
			CaptureChunk captureChunk = _renderQueue.Dequeue();
			_graphics.SetRenderTarget((RenderTarget2D)null);
			_graphics.Clear(Color.Transparent);
			TileDrawing tilesRenderer = Main.instance.TilesRenderer;
			Rectangle area = captureChunk.Area;
			int left = area.Left;
			area = captureChunk.Area;
			int right = area.Right;
			area = captureChunk.Area;
			int top = area.Top;
			area = captureChunk.Area;
			tilesRenderer.PrepareForAreaDrawing(left, right, top, area.Bottom, prepareLazily: false);
			Main.instance.TilePaintSystem.PrepareAllRequests();
			_graphics.SetRenderTarget(_frameBuffer);
			Main.instance.DrawCapture(captureChunk.Area, _activeSettings, this);
			if (_activeSettings.UseScaling)
			{
				_graphics.SetRenderTarget(_scaledFrameBuffer);
				_graphics.Clear(Color.Transparent);
				_spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, _downscaleSampleState, DepthStencilState.Default, RasterizerState.CullNone);
				_spriteBatch.Draw((Texture2D)(object)_frameBuffer, new Rectangle(0, 0, ((Texture2D)_scaledFrameBuffer).Width, ((Texture2D)_scaledFrameBuffer).Height), Color.White);
				_spriteBatch.End();
				_graphics.SetRenderTarget((RenderTarget2D)null);
				((Texture2D)_scaledFrameBuffer).GetData<byte>(_scaledFrameData, 0, ((Texture2D)_scaledFrameBuffer).Width * ((Texture2D)_scaledFrameBuffer).Height * 4);
				DrawBytesToBuffer(_scaledFrameData, _outputData, ((Texture2D)_scaledFrameBuffer).Width, _outputImageSize.Width, captureChunk.ScaledArea);
			}
			else
			{
				_graphics.SetRenderTarget((RenderTarget2D)null);
				SaveImage((Texture2D)(object)_frameBuffer, captureChunk.ScaledArea.Width, captureChunk.ScaledArea.Height, ImageFormat.Png, _activeSettings.OutputName, captureChunk.Area.X + "-" + captureChunk.Area.Y + ".png");
			}
			_tilesProcessed += captureChunk.Area.Width * captureChunk.Area.Height;
		}
		if (_renderQueue.Count == 0)
		{
			FinishCapture();
		}
		Monitor.Exit(_captureLock);
	}

	public void BeginDrawCapture()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (Lighting.NotRetro)
		{
			((Game)Main.instance).GraphicsDevice.SetRenderTarget(Main.skyTarget);
			((Game)Main.instance).GraphicsDevice.Clear(Color.Transparent);
			Filters.Scene.BeginCapture(_filterFrameBuffer1);
		}
		Main.waterTarget = _waterTarget;
	}

	public void EndDrawCapture(Vector2 screenSize, Vector2 sceneSize, Vector2 sceneOffset)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (Lighting.NotRetro)
		{
			Filters.Scene.EndCapture(_frameBuffer, _filterFrameBuffer1, _filterFrameBuffer2, screenSize, sceneSize, sceneOffset);
		}
	}

	private unsafe void DrawBytesToBuffer(byte[] sourceBuffer, byte[] destinationBuffer, int sourceBufferWidth, int destinationBufferWidth, Rectangle area)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		fixed (byte* ptr = &destinationBuffer[0])
		{
			fixed (byte* ptr2 = &sourceBuffer[0])
			{
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr + (destinationBufferWidth * area.Y + area.X << 2);
				for (int i = 0; i < area.Height; i++)
				{
					for (int j = 0; j < area.Width; j++)
					{
						if (Program.IsXna)
						{
							ptr4[2] = *ptr3;
							ptr4[1] = ptr3[1];
							*ptr4 = ptr3[2];
							ptr4[3] = ptr3[3];
						}
						else
						{
							*ptr4 = *ptr3;
							ptr4[1] = ptr3[1];
							ptr4[2] = ptr3[2];
							ptr4[3] = ptr3[3];
						}
						ptr3 += 4;
						ptr4 += 4;
					}
					ptr3 += sourceBufferWidth - area.Width << 2;
					ptr4 += destinationBufferWidth - area.Width << 2;
				}
			}
		}
	}

	public float GetProgress()
	{
		return _tilesProcessed / _totalTiles;
	}

	private bool SaveImage(int width, int height, ImageFormat imageFormat, string filename)
	{
		return SaveImage(_outputData, width, height, imageFormat, filename);
	}

	public static bool SaveImage(byte[] data, int width, int height, ImageFormat imageFormat, string filename)
	{
		if (!Utils.TryCreatingDirectory(Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar))
		{
			return false;
		}
		try
		{
			if (Program.IsFna)
			{
				PlatformUtilities.SavePng(filename, width, height, data);
			}
			else
			{
				using Bitmap bitmap = new Bitmap(width, height);
				System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
				BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
				IntPtr scan = bitmapData.Scan0;
				Marshal.Copy(data, 0, scan, width * height * 4);
				bitmap.UnlockBits(bitmapData);
				bitmap.Save(filename, imageFormat);
				bitmap.Dispose();
			}
			return true;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return false;
		}
	}

	private void SaveImage(Texture2D texture, int width, int height, ImageFormat imageFormat, string foldername, string filename)
	{
		string text = Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar + foldername;
		string text2 = Path.Combine(text, filename);
		if (!Utils.TryCreatingDirectory(text))
		{
			return;
		}
		if (Program.IsFna)
		{
			int num = texture.Width * texture.Height * 4;
			texture.GetData<byte>(_outputData, 0, num);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					_outputData[num3] = _outputData[num2];
					_outputData[num3 + 1] = _outputData[num2 + 1];
					_outputData[num3 + 2] = _outputData[num2 + 2];
					_outputData[num3 + 3] = _outputData[num2 + 3];
					num2 += 4;
					num3 += 4;
				}
				num2 += texture.Width - width << 2;
			}
			PlatformUtilities.SavePng(text2, width, height, _outputData);
			return;
		}
		using Bitmap bitmap = new Bitmap(width, height);
		System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
		int num4 = texture.Width * texture.Height * 4;
		texture.GetData<byte>(_outputData, 0, num4);
		int num5 = 0;
		int num6 = 0;
		for (int k = 0; k < height; k++)
		{
			for (int l = 0; l < width; l++)
			{
				byte b = _outputData[num5 + 2];
				_outputData[num6 + 2] = _outputData[num5];
				_outputData[num6] = b;
				_outputData[num6 + 1] = _outputData[num5 + 1];
				_outputData[num6 + 3] = _outputData[num5 + 3];
				num5 += 4;
				num6 += 4;
			}
			num5 += texture.Width - width << 2;
		}
		BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
		IntPtr scan = bitmapData.Scan0;
		Marshal.Copy(_outputData, 0, scan, width * height * 4);
		bitmap.UnlockBits(bitmapData);
		bitmap.Save(text2, imageFormat);
	}

	private void FinishCapture()
	{
		if (_activeSettings.UseScaling && _outputData != null)
		{
			int num = 0;
			while (!SaveImage(_outputImageSize.Width, _outputImageSize.Height, ImageFormat.Png, Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar + _activeSettings.OutputName + ".png"))
			{
				GC.Collect();
				Thread.Sleep(5);
				num++;
				Console.WriteLine(Language.GetTextValue("Error.CaptureError"));
				if (num > 5)
				{
					Console.WriteLine(Language.GetTextValue("Error.UnableToCapture"));
					break;
				}
			}
		}
		_outputData = null;
		_scaledFrameData = null;
		Main.GlobalTimerPaused = false;
		CaptureInterface.EndCamera();
		if (_scaledFrameBuffer != null)
		{
			((GraphicsResource)_scaledFrameBuffer).Dispose();
			_scaledFrameBuffer = null;
		}
		_activeSettings = null;
	}

	public void Dispose()
	{
		if (Main.dedServ)
		{
			return;
		}
		Monitor.Enter(_captureLock);
		if (_isDisposed)
		{
			Monitor.Exit(_captureLock);
			return;
		}
		((GraphicsResource)_frameBuffer).Dispose();
		((GraphicsResource)_filterFrameBuffer1).Dispose();
		((GraphicsResource)_filterFrameBuffer2).Dispose();
		_waterTarget.Dispose();
		if (_scaledFrameBuffer != null)
		{
			((GraphicsResource)_scaledFrameBuffer).Dispose();
			_scaledFrameBuffer = null;
		}
		CameraExists = false;
		_isDisposed = true;
		Monitor.Exit(_captureLock);
	}
}
