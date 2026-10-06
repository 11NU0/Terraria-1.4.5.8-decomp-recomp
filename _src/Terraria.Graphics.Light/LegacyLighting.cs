using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.DataStructures;
using Terraria.Graphics.Capture;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Graphics.Light;

public class LegacyLighting : ILightingEngine
{
	public struct RectArea
	{
		public int Left;

		public int Right;

		public int Top;

		public int Bottom;

		public void Set(int left, int right, int top, int bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}
	}

	private class LightingSwipeData
	{
		public int InnerLoop1Start;

		public int InnerLoop1End;

		public int InnerLoop2Start;

		public int InnerLoop2End;

		public LightingState[][] JaggedArray;

		public LightingSwipeData()
		{
			InnerLoop1Start = 0;
			InnerLoop1End = 0;
			InnerLoop2Start = 0;
			InnerLoop2End = 0;
		}

		public void CopyFrom(LightingSwipeData from)
		{
			InnerLoop1Start = from.InnerLoop1Start;
			InnerLoop1End = from.InnerLoop1End;
			InnerLoop2Start = from.InnerLoop2Start;
			InnerLoop2End = from.InnerLoop2End;
			JaggedArray = from.JaggedArray;
		}
	}

	private class LightingState
	{
		public float R;

		public float R2;

		public float G;

		public float G2;

		public float B;

		public float B2;

		public bool CrackedLight;

		public bool StopLight;

		public bool WetLight;

		public bool HoneyLight;

		public Vector3 ToVector3()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(R, G, B);
		}
	}

	private struct ColorTriplet
	{
		public float R;

		public float G;

		public float B;

		public ColorTriplet(float R, float G, float B)
		{
			this.R = R;
			this.G = G;
			this.B = B;
		}

		public ColorTriplet(float averageColor)
		{
			R = (G = (B = averageColor));
		}
	}

	public static int RenderPhases = 4;

	private bool _rgb = true;

	private int _offScreenTiles2 = 35;

	private float _oldSkyColor;

	private float _skyColor;

	private int _requestedRectLeft;

	private int _requestedRectRight;

	private int _requestedRectTop;

	private int _requestedRectBottom;

	private LightingState[][] _states;

	private LightingState[][] _axisFlipStates;

	private LightingSwipeData _swipe;

	private LightingSwipeData[] _threadSwipes;

	private int _scrX;

	private int _scrY;

	private int _minX;

	private int _maxX;

	private int _minY;

	private int _maxY;

	private const int MAX_TEMP_LIGHTS = 2000;

	private Dictionary<Point16, ColorTriplet> _tempLights;

	private int _expandedRectLeft;

	private int _expandedRectTop;

	private int _expandedRectRight;

	private int _expandedRectBottom;

	private float _negLight = 0.04f;

	private float _negLight2 = 0.16f;

	private float _negLight3 = 0.08f;

	private float _wetLightR = 0.16f;

	private float _wetLightG = 0.16f;

	private float _wetLightB = 0.16f;

	private float _honeyLightR = 0.16f;

	private float _honeyLightG = 0.16f;

	private float _honeyLightB = 0.16f;

	private float _blueWave = 1f;

	private int _blueDir = 1;

	private RectArea _minBoundArea;

	private RectArea _requestedArea;

	private RectArea _expandedArea;

	private RectArea _offScreenTiles2ExpandedArea;

	private TileLightScanner _tileScanner = new TileLightScanner();

	private readonly Camera _camera;

	private Vector2? _lastCameraPosition;

	private static FastRandom _swipeRandom = FastRandom.CreateWithRandomSeed();

	private LightMap _lightMap = new LightMap();

	public int Mode { get; set; }

	public bool IsColorOrWhiteMode => Mode < 2;

	public LegacyLighting(Camera camera)
	{
		_camera = camera;
	}

	public Vector3 GetColor(int x, int y)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (x < _expandedRectLeft || x >= _expandedRectRight || y < _expandedRectTop || y >= _expandedRectBottom)
		{
			return Vector3.Zero;
		}
		LightingState lightingState = _states[x - _expandedRectLeft][y - _expandedRectTop];
		return new Vector3(lightingState.R, lightingState.G, lightingState.B);
	}

	public void Rebuild()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (!CaptureManager.Instance.IsCapturing)
		{
			_tempLights = new Dictionary<Point16, ColorTriplet>();
		}
		_swipe = new LightingSwipeData();
		_threadSwipes = new LightingSwipeData[Environment.ProcessorCount];
		for (int i = 0; i < _threadSwipes.Length; i++)
		{
			_threadSwipes[i] = new LightingSwipeData();
		}
		int num = (int)_camera.UnscaledSize.X / 16 + 90 + 10;
		int num2 = (int)_camera.UnscaledSize.Y / 16 + 90 + 10;
		_lightMap.SetSize(num, num2);
		if (_states != null && _states.Length >= num && _states[0].Length >= num2)
		{
			return;
		}
		_states = new LightingState[num][];
		_axisFlipStates = new LightingState[num2][];
		for (int j = 0; j < num2; j++)
		{
			_axisFlipStates[j] = new LightingState[num];
		}
		for (int k = 0; k < num; k++)
		{
			LightingState[] array = new LightingState[num2];
			for (int l = 0; l < num2; l++)
			{
				LightingState lightingState = (array[l] = new LightingState());
				_axisFlipStates[l][k] = lightingState;
			}
			_states[k] = array;
		}
	}

	public void AddLight(int x, int y, Vector3 color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		float x2 = color.X;
		float y2 = color.Y;
		float z = color.Z;
		if (x - _requestedRectLeft + Lighting.OffScreenTiles < 0 || !((float)(x - _requestedRectLeft + Lighting.OffScreenTiles) < _camera.UnscaledSize.X / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f) || y - _requestedRectTop + Lighting.OffScreenTiles < 0 || !((float)(y - _requestedRectTop + Lighting.OffScreenTiles) < _camera.UnscaledSize.Y / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f) || _tempLights.Count == 2000)
		{
			return;
		}
		Point16 key = new Point16(x, y);
		if (_tempLights.TryGetValue(key, out var value))
		{
			if (_rgb)
			{
				if (value.R < x2)
				{
					value.R = x2;
				}
				if (value.G < y2)
				{
					value.G = y2;
				}
				if (value.B < z)
				{
					value.B = z;
				}
				_tempLights[key] = value;
			}
			else
			{
				float num = (x2 + y2 + z) / 3f;
				if (value.R < num)
				{
					_tempLights[key] = new ColorTriplet(num);
				}
			}
		}
		else
		{
			value = ((!_rgb) ? new ColorTriplet((x2 + y2 + z) / 3f) : new ColorTriplet(x2, y2, z));
			_tempLights.Add(key, value);
		}
	}

	public void ProcessArea(Rectangle area)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		_oldSkyColor = _skyColor;
		float num = (float)(int)Main.tileColor.R / 255f;
		float num2 = (float)(int)Main.tileColor.G / 255f;
		float num3 = (float)(int)Main.tileColor.B / 255f;
		_skyColor = (num + num2 + num3) / 3f;
		if (IsColorOrWhiteMode)
		{
			_offScreenTiles2 = 34;
			Lighting.OffScreenTiles = 40;
		}
		else
		{
			_offScreenTiles2 = 18;
			Lighting.OffScreenTiles = 23;
		}
		_requestedRectLeft = area.Left;
		_requestedRectRight = area.Right;
		_requestedRectTop = area.Top;
		_requestedRectBottom = area.Bottom;
		_expandedRectLeft = _requestedRectLeft - Lighting.OffScreenTiles;
		_expandedRectTop = _requestedRectTop - Lighting.OffScreenTiles;
		_expandedRectRight = _requestedRectRight + Lighting.OffScreenTiles;
		_expandedRectBottom = _requestedRectBottom + Lighting.OffScreenTiles;
		Main.renderCount++;
		int maxLightArrayX = (int)_camera.UnscaledSize.X / 16 + Lighting.OffScreenTiles * 2;
		int maxLightArrayY = (int)_camera.UnscaledSize.Y / 16 + Lighting.OffScreenTiles * 2;
		if (Main.renderCount < 3)
		{
			DoColors();
		}
		if (Main.renderCount == 2)
		{
			CopyFullyProcessedDataOver(maxLightArrayX, maxLightArrayY);
		}
		else if (_lastCameraPosition.HasValue)
		{
			ShiftUnProcessedDataOver(maxLightArrayX, maxLightArrayY);
			if (Netplay.Connection.StatusMax > 0)
			{
				Main.mapTime = 1;
			}
			if (Main.mapDelay > 0)
			{
				Main.mapDelay--;
			}
			else if (Main.mapTime == 0 && Main.mapEnabled && Main.renderCount == 3)
			{
				try
				{
					TryUpdatingMapWithLight();
				}
				catch
				{
				}
			}
			if (_oldSkyColor != _skyColor)
			{
				UpdateLightToSkyColor(num, num2, num3);
			}
		}
		if (Main.renderCount > RenderPhases)
		{
			PreRenderPhase();
		}
		_lastCameraPosition = Main.Camera.UnscaledPosition;
	}

	private void TryUpdatingMapWithLight()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		Main.mapTime = Main.mapTimeMax;
		int num = 40;
		Vector2 unscaledPosition = _camera.UnscaledPosition;
		int num2 = (int)_camera.ScaledSize.X;
		int num3 = (int)_camera.ScaledSize.Y;
		Vector2 val = unscaledPosition + Main.GameViewMatrix.Translation;
		int value = (int)Math.Floor(val.X / 16f);
		int value2 = (int)Math.Floor((val.X + (float)num2) / 16f) + 1;
		int value3 = (int)Math.Floor(val.Y / 16f);
		int value4 = (int)Math.Floor((val.Y + (float)num3) / 16f) + 1;
		value = Utils.Clamp(value, Lighting.OffScreenTiles, Main.maxTilesX - Lighting.OffScreenTiles);
		value2 = Utils.Clamp(value2, Lighting.OffScreenTiles, Main.maxTilesX - Lighting.OffScreenTiles);
		value3 = Utils.Clamp(value3, Lighting.OffScreenTiles, Main.maxTilesY - Lighting.OffScreenTiles);
		value4 = Utils.Clamp(value4, Lighting.OffScreenTiles, Main.maxTilesY - Lighting.OffScreenTiles);
		int value5 = Utils.Clamp(_requestedRectLeft, num, Main.maxTilesX - num);
		int value6 = Utils.Clamp(_requestedRectRight, num, Main.maxTilesX - num);
		int value7 = Utils.Clamp(_requestedRectTop, num, Main.maxTilesY - num);
		int value8 = Utils.Clamp(_requestedRectBottom, num, Main.maxTilesY - num);
		value5 = Utils.Clamp(value5, value, value2);
		value6 = Utils.Clamp(value6, value, value2);
		value7 = Utils.Clamp(value7, value3, value4);
		value8 = Utils.Clamp(value8, value3, value4);
		int offScreenTiles = Lighting.OffScreenTiles;
		for (int i = value5; i < value6; i++)
		{
			LightingState[] array = _states[i - _requestedRectLeft + offScreenTiles];
			for (int j = value7; j < value8; j++)
			{
				LightingState lightingState = array[j - _requestedRectTop + offScreenTiles];
				Tile tile = Main.tile[i, j];
				float num4 = 0f;
				if (lightingState.R > num4)
				{
					num4 = lightingState.R;
				}
				if (lightingState.G > num4)
				{
					num4 = lightingState.G;
				}
				if (lightingState.B > num4)
				{
					num4 = lightingState.B;
				}
				if (IsColorOrWhiteMode)
				{
					num4 *= 1.5f;
				}
				byte b = (byte)Math.Min(255f, num4 * 255f);
				if ((double)j < Main.worldSurface && !tile.active() && tile.wall == 0 && tile.liquid == 0)
				{
					b = 22;
				}
				if ((b > 18 || Main.Map[i, j].Light > 0) && b < 22)
				{
					b = 22;
				}
				Main.Map.UpdateLighting(i, j, b);
			}
		}
		Main.updateMap = new Rectangle(value5, value7, value6 - value5, value8 - value7);
	}

	private void UpdateLightToSkyColor(float tileR, float tileG, float tileB)
	{
		int num = Utils.Clamp(_expandedRectLeft, 0, Main.maxTilesX - 1);
		int num2 = Utils.Clamp(_expandedRectRight, 0, Main.maxTilesX - 1);
		int num3 = Utils.Clamp(_expandedRectTop, 0, Main.maxTilesY - 1);
		int num4 = Utils.Clamp(_expandedRectBottom, 0, (int)Main.worldSurface - 1);
		if (!((double)num3 < Main.worldSurface))
		{
			return;
		}
		for (int i = num; i < num2; i++)
		{
			LightingState[] array = _states[i - _expandedRectLeft];
			for (int j = num3; j < num4; j++)
			{
				LightingState lightingState = array[j - _expandedRectTop];
				Tile tile = Main.tile[i, j];
				if (tile == null)
				{
					tile = new Tile();
					Main.tile[i, j] = tile;
				}
				if ((!tile.active() || !Main.tileNoSunLight[tile.type]) && lightingState.R < _skyColor && tile.liquid < 200 && (Main.wallLight[tile.wall] || tile.wall == 73))
				{
					lightingState.R = tileR;
					if (lightingState.G < _skyColor)
					{
						lightingState.G = tileG;
					}
					if (lightingState.B < _skyColor)
					{
						lightingState.B = tileB;
					}
				}
			}
		}
	}

	private void ShiftUnProcessedDataOver(int maxLightArrayX, int maxLightArrayY)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 value = _lastCameraPosition.Value;
		Vector2 unscaledPosition = _camera.UnscaledPosition;
		int num = (int)Math.Floor(unscaledPosition.X / 16f) - (int)Math.Floor(value.X / 16f);
		if (num > 5 || num < -5)
		{
			num = 0;
		}
		int num2;
		int num3;
		int num4;
		if (num < 0)
		{
			num2 = -1;
			num *= -1;
			num3 = maxLightArrayX;
			num4 = num;
		}
		else
		{
			num2 = 1;
			num3 = 0;
			num4 = maxLightArrayX - num;
		}
		int num5 = (int)Math.Floor(unscaledPosition.Y / 16f) - (int)Math.Floor(value.Y / 16f);
		if (num5 > 5 || num5 < -5)
		{
			num5 = 0;
		}
		int num6;
		int num7;
		int num8;
		if (num5 < 0)
		{
			num6 = -1;
			num5 *= -1;
			num7 = maxLightArrayY;
			num8 = num5;
		}
		else
		{
			num6 = 1;
			num7 = 0;
			num8 = maxLightArrayY - num5;
		}
		if (num == 0 && num5 == 0)
		{
			return;
		}
		for (int i = num3; i != num4; i += num2)
		{
			LightingState[] array = _states[i];
			LightingState[] array2 = _states[i + num * num2];
			for (int j = num7; j != num8; j += num6)
			{
				LightingState obj = array[j];
				LightingState lightingState = array2[j + num5 * num6];
				obj.R = lightingState.R;
				obj.G = lightingState.G;
				obj.B = lightingState.B;
			}
		}
	}

	private void CopyFullyProcessedDataOver(int maxLightArrayX, int maxLightArrayY)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = _camera.UnscaledPosition;
		int num = (int)Math.Floor(unscaledPosition.X / 16f) - _scrX;
		int num2 = (int)Math.Floor(unscaledPosition.Y / 16f) - _scrY;
		if (num > 16)
		{
			num = 0;
		}
		if (num2 > 16)
		{
			num2 = 0;
		}
		int num3 = 0;
		int num4 = maxLightArrayX;
		int num5 = 0;
		int num6 = maxLightArrayY;
		if (num < 0)
		{
			num3 -= num;
		}
		else
		{
			num4 -= num;
		}
		if (num2 < 0)
		{
			num5 -= num2;
		}
		else
		{
			num6 -= num2;
		}
		if (_rgb)
		{
			int num7 = maxLightArrayX;
			if (_states.Length <= num7 + num)
			{
				num7 = _states.Length - num - 1;
			}
			for (int i = num3; i < num7; i++)
			{
				LightingState[] array = _states[i];
				LightingState[] array2 = _states[i + num];
				int num8 = num6;
				if (array2.Length <= num8 + num)
				{
					num8 = array2.Length - num2 - 1;
				}
				for (int j = num5; j < num8; j++)
				{
					LightingState obj = array[j];
					LightingState lightingState = array2[j + num2];
					obj.R = lightingState.R2;
					obj.G = lightingState.G2;
					obj.B = lightingState.B2;
				}
			}
			return;
		}
		int num9 = num4;
		if (_states.Length <= num9 + num)
		{
			num9 = _states.Length - num - 1;
		}
		for (int k = num3; k < num9; k++)
		{
			LightingState[] array3 = _states[k];
			LightingState[] array4 = _states[k + num];
			int num10 = num6;
			if (array4.Length <= num10 + num)
			{
				num10 = array4.Length - num2 - 1;
			}
			for (int l = num5; l < num10; l++)
			{
				LightingState obj2 = array3[l];
				LightingState lightingState2 = array4[l + num2];
				obj2.R = lightingState2.R2;
				obj2.G = lightingState2.R2;
				obj2.B = lightingState2.R2;
			}
		}
	}

	public void Clear()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		_lastCameraPosition = null;
		int num = (int)_camera.UnscaledSize.X / 16 + Lighting.OffScreenTiles * 2;
		int num2 = (int)_camera.UnscaledSize.Y / 16 + Lighting.OffScreenTiles * 2;
		for (int i = 0; i < num; i++)
		{
			if (i >= _states.Length)
			{
				continue;
			}
			LightingState[] array = _states[i];
			for (int j = 0; j < num2; j++)
			{
				if (j < array.Length)
				{
					LightingState obj = array[j];
					obj.R = 0f;
					obj.G = 0f;
					obj.B = 0f;
				}
			}
		}
	}

	private void PreRenderPhase()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		_ = (float)(int)Main.tileColor.R / 255f;
		_ = (float)(int)Main.tileColor.G / 255f;
		_ = (float)(int)Main.tileColor.B / 255f;
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		int num = 0;
		int num2 = (int)_camera.UnscaledSize.X / 16 + Lighting.OffScreenTiles * 2 + 10;
		int num3 = 0;
		int num4 = (int)_camera.UnscaledSize.Y / 16 + Lighting.OffScreenTiles * 2 + 10;
		_minX = num2;
		_maxX = num;
		_minY = num4;
		_maxY = num3;
		_rgb = Mode == 0 || Mode == 3;
		for (int i = num; i < num2; i++)
		{
			LightingState[] array = _states[i];
			for (int j = num3; j < num4; j++)
			{
				LightingState obj = array[j];
				obj.R2 = 0f;
				obj.G2 = 0f;
				obj.B2 = 0f;
				obj.CrackedLight = false;
				obj.StopLight = false;
				obj.WetLight = false;
				obj.HoneyLight = false;
			}
		}
		if (Main.wofNPCIndex >= 0 && Main.player[Main.myPlayer].gross)
		{
			try
			{
				int num5 = (int)_camera.UnscaledPosition.Y / 16 - 10;
				int num6 = (int)(_camera.UnscaledPosition.Y + _camera.UnscaledSize.Y) / 16 + 10;
				int num7 = (int)Main.npc[Main.wofNPCIndex].position.X / 16;
				num7 = ((Main.npc[Main.wofNPCIndex].direction <= 0) ? (num7 + 2) : (num7 - 3));
				int num8 = num7 + 8;
				float num9 = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
				float num10 = 0.3f;
				float num11 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
				num9 *= 0.2f;
				num10 *= 0.1f;
				num11 *= 0.3f;
				for (int k = num7; k <= num8; k++)
				{
					LightingState[] array2 = _states[k - num7];
					for (int l = num5; l <= num6; l++)
					{
						LightingState lightingState = array2[l - _expandedRectTop];
						if (lightingState.R2 < num9)
						{
							lightingState.R2 = num9;
						}
						if (lightingState.G2 < num10)
						{
							lightingState.G2 = num10;
						}
						if (lightingState.B2 < num11)
						{
							lightingState.B2 = num11;
						}
					}
				}
			}
			catch
			{
			}
		}
		num = Utils.Clamp(_expandedRectLeft, 5, Main.maxTilesX - 1);
		num2 = Utils.Clamp(_expandedRectRight, 5, Main.maxTilesX - 1);
		num3 = Utils.Clamp(_expandedRectTop, 5, Main.maxTilesY - 1);
		num4 = Utils.Clamp(_expandedRectBottom, 5, Main.maxTilesY - 1);
		Main.UpdateSceneMetrics();
		_tileScanner.Update();
		_tileScanner.ExportTo(new Rectangle(num, num3, num2 - num, num4 - num3), _lightMap, new TileLightScannerOptions
		{
			DrawInvisibleWalls = Main.ShouldShowInvisibleBlocksAndWalls()
		});
		for (int m = num; m < num2; m++)
		{
			LightingState[] array3 = _states[m - _expandedRectLeft];
			for (int n = num3; n < num4; n++)
			{
				LightingState lightingState2 = array3[n - _expandedRectTop];
				Tile tile = Main.tile[m, n];
				if (tile == null)
				{
					tile = new Tile();
					Main.tile[m, n] = tile;
				}
				_lightMap.GetLight(m - num, n - num3, out var color);
				if (_rgb)
				{
					lightingState2.R2 = color.X;
					lightingState2.G2 = color.Y;
					lightingState2.B2 = color.Z;
				}
				else
				{
					lightingState2.B2 = (lightingState2.G2 = (lightingState2.R2 = (color.X + color.Y + color.Z) / 3f));
				}
				switch (_lightMap.GetMask(m - num, n - num3))
				{
				case LightMaskMode.CrackedBricks:
					lightingState2.CrackedLight = true;
					break;
				case LightMaskMode.Solid:
					lightingState2.StopLight = true;
					break;
				case LightMaskMode.Water:
					lightingState2.WetLight = true;
					break;
				case LightMaskMode.Honey:
					lightingState2.WetLight = true;
					lightingState2.HoneyLight = true;
					break;
				}
				if (lightingState2.R2 > 0f || (_rgb && (lightingState2.G2 > 0f || lightingState2.B2 > 0f)))
				{
					int num12 = m - _expandedRectLeft;
					int num13 = n - _expandedRectTop;
					if (_minX > num12)
					{
						_minX = num12;
					}
					if (_maxX < num12 + 1)
					{
						_maxX = num12 + 1;
					}
					if (_minY > num13)
					{
						_minY = num13;
					}
					if (_maxY < num13 + 1)
					{
						_maxY = num13 + 1;
					}
				}
			}
		}
		foreach (KeyValuePair<Point16, ColorTriplet> tempLight in _tempLights)
		{
			int num14 = tempLight.Key.X - _requestedRectLeft + Lighting.OffScreenTiles;
			int num15 = tempLight.Key.Y - _requestedRectTop + Lighting.OffScreenTiles;
			if (num14 >= 0 && (float)num14 < _camera.UnscaledSize.X / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f && num15 >= 0 && (float)num15 < _camera.UnscaledSize.Y / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f)
			{
				LightingState lightingState3 = _states[num14][num15];
				if (lightingState3.R2 < tempLight.Value.R)
				{
					lightingState3.R2 = tempLight.Value.R;
				}
				if (lightingState3.G2 < tempLight.Value.G)
				{
					lightingState3.G2 = tempLight.Value.G;
				}
				if (lightingState3.B2 < tempLight.Value.B)
				{
					lightingState3.B2 = tempLight.Value.B;
				}
				if (_minX > num14)
				{
					_minX = num14;
				}
				if (_maxX < num14 + 1)
				{
					_maxX = num14 + 1;
				}
				if (_minY > num15)
				{
					_minY = num15;
				}
				if (_maxY < num15 + 1)
				{
					_maxY = num15 + 1;
				}
			}
		}
		if (!Main.gamePaused && !CaptureManager.Instance.IsCapturing)
		{
			_tempLights.Clear();
		}
		if (DebugOptions.devLightTilesCheat)
		{
			Main.renderCount = 1;
			TimeLogger.LightingInit.AddTime(fromTimestamp);
			return;
		}
		_minX += _expandedRectLeft;
		_maxX += _expandedRectLeft;
		_minY += _expandedRectTop;
		_maxY += _expandedRectTop;
		_minBoundArea.Set(_minX, _maxX, _minY, _maxY);
		_requestedArea.Set(_requestedRectLeft, _requestedRectRight, _requestedRectTop, _requestedRectBottom);
		_expandedArea.Set(_expandedRectLeft, _expandedRectRight, _expandedRectTop, _expandedRectBottom);
		_offScreenTiles2ExpandedArea.Set(_requestedRectLeft - _offScreenTiles2, _requestedRectRight + _offScreenTiles2, _requestedRectTop - _offScreenTiles2, _requestedRectBottom + _offScreenTiles2);
		_scrX = (int)Math.Floor(_camera.UnscaledPosition.X / 16f);
		_scrY = (int)Math.Floor(_camera.UnscaledPosition.Y / 16f);
		Main.renderCount = 0;
		TimeLogger.LightingInit.AddTime(fromTimestamp);
		DoColors();
	}

	private void DoColors()
	{
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a65: Expected Obj, but got Unknown
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Expected Obj, but got Unknown
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Expected Obj, but got Unknown
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a95: Expected Obj, but got Unknown
		//IL_0a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Expected Obj, but got Unknown
		if (IsColorOrWhiteMode)
		{
			_blueWave += (float)_blueDir * 0.0001f;
			if (_blueWave > 1f)
			{
				_blueWave = 1f;
				_blueDir = -1;
			}
			else if (_blueWave < 0.97f)
			{
				_blueWave = 0.97f;
				_blueDir = 1;
			}
			if (_rgb)
			{
				_negLight = 0.91f;
				_negLight2 = 0.56f;
				_negLight3 = 0.8f;
				_honeyLightG = 0.7f * _negLight * _blueWave;
				_honeyLightR = 0.75f * _negLight * _blueWave;
				_honeyLightB = 0.6f * _negLight * _blueWave;
				switch (Main.waterStyle)
				{
				case 0:
				case 1:
				case 7:
				case 8:
					_wetLightG = 0.96f * _negLight * _blueWave;
					_wetLightR = 0.88f * _negLight * _blueWave;
					_wetLightB = 1.015f * _negLight * _blueWave;
					break;
				case 2:
					_wetLightG = 0.85f * _negLight * _blueWave;
					_wetLightR = 0.94f * _negLight * _blueWave;
					_wetLightB = 1.01f * _negLight * _blueWave;
					break;
				case 3:
					_wetLightG = 0.95f * _negLight * _blueWave;
					_wetLightR = 0.84f * _negLight * _blueWave;
					_wetLightB = 1.015f * _negLight * _blueWave;
					break;
				case 4:
					_wetLightG = 0.86f * _negLight * _blueWave;
					_wetLightR = 0.9f * _negLight * _blueWave;
					_wetLightB = 1.01f * _negLight * _blueWave;
					break;
				case 5:
					_wetLightG = 0.99f * _negLight * _blueWave;
					_wetLightR = 0.84f * _negLight * _blueWave;
					_wetLightB = 1.01f * _negLight * _blueWave;
					break;
				case 6:
					_wetLightR = 0.83f * _negLight * _blueWave;
					_wetLightG = 0.93f * _negLight * _blueWave;
					_wetLightB = 0.98f * _negLight * _blueWave;
					break;
				case 9:
					_wetLightG = 0.88f * _negLight * _blueWave;
					_wetLightR = 1f * _negLight * _blueWave;
					_wetLightB = 0.84f * _negLight * _blueWave;
					break;
				case 10:
					_wetLightG = 1f * _negLight * _blueWave;
					_wetLightR = 0.83f * _negLight * _blueWave;
					_wetLightB = 1f * _negLight * _blueWave;
					break;
				case 12:
					_wetLightG = 0.98f * _negLight * _blueWave;
					_wetLightR = 0.95f * _negLight * _blueWave;
					_wetLightB = 0.85f * _negLight * _blueWave;
					break;
				default:
					_wetLightG = 0f;
					_wetLightR = 0f;
					_wetLightB = 0f;
					break;
				}
			}
			else
			{
				_negLight = 0.9f;
				_negLight2 = 0.54f;
				_negLight3 = 0.8f;
				_wetLightR = 0.95f * _negLight * _blueWave;
			}
			if (Main.player[Main.myPlayer].nightVision)
			{
				_negLight *= 1.03f;
				_negLight2 *= 1.03f;
				_negLight3 *= 1.03f;
			}
			if (Main.player[Main.myPlayer].blind)
			{
				_negLight *= 0.95f;
				_negLight2 *= 0.95f;
				_negLight3 *= 0.95f;
			}
			if (Main.player[Main.myPlayer].blackout)
			{
				_negLight *= 0.85f;
				_negLight2 *= 0.85f;
				_negLight3 *= 0.85f;
			}
			if (Main.player[Main.myPlayer].headcovered)
			{
				_negLight *= 0.85f;
				_negLight2 *= 0.85f;
				_negLight3 *= 0.85f;
			}
		}
		else
		{
			_negLight = 0.04f;
			_negLight2 = 0.16f;
			_negLight3 = 0.08f;
			if (Main.player[Main.myPlayer].nightVision)
			{
				_negLight -= 0.013f;
				_negLight2 -= 0.04f;
				_negLight3 -= 0.04f;
			}
			if (Main.player[Main.myPlayer].blind)
			{
				_negLight += 0.03f;
				_negLight2 += 0.06f;
				_negLight3 += 0.06f;
			}
			if (Main.player[Main.myPlayer].blackout)
			{
				_negLight += 0.09f;
				_negLight2 += 0.18f;
				_negLight3 += 0.18f;
			}
			if (Main.player[Main.myPlayer].headcovered)
			{
				_negLight += 0.09f;
				_negLight2 += 0.18f;
				_negLight3 += 0.18f;
			}
			_wetLightR = _negLight * 1.2f;
			_wetLightG = _negLight * 1.1f;
		}
		int num;
		int num2;
		switch (Main.renderCount)
		{
		case 0:
			num = 0;
			num2 = 1;
			break;
		case 1:
			num = 1;
			num2 = 3;
			break;
		case 2:
			num = 3;
			num2 = 4;
			break;
		default:
			num = 0;
			num2 = 0;
			break;
		}
		int left = _expandedArea.Left;
		int top = _expandedArea.Top;
		for (int i = num; i < num2; i++)
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			int num3 = 0;
			int num4 = 0;
			switch (i)
			{
			case 0:
				_swipe.InnerLoop1Start = _minBoundArea.Top - top;
				_swipe.InnerLoop2Start = _minBoundArea.Bottom - top;
				_swipe.InnerLoop1End = _requestedArea.Bottom + RenderPhases - top;
				_swipe.InnerLoop2End = _requestedArea.Top - RenderPhases - top;
				num3 = _minBoundArea.Left - left;
				num4 = _minBoundArea.Right - left;
				_swipe.JaggedArray = _states;
				break;
			case 1:
				_swipe.InnerLoop1Start = _expandedArea.Left - left;
				_swipe.InnerLoop2Start = _expandedArea.Right - left;
				_swipe.InnerLoop1End = _requestedArea.Right + RenderPhases - left;
				_swipe.InnerLoop2End = _requestedArea.Left - RenderPhases - left;
				num3 = _expandedArea.Top - top;
				num4 = _expandedArea.Bottom - top;
				_swipe.JaggedArray = _axisFlipStates;
				break;
			case 2:
				_swipe.InnerLoop1Start = _offScreenTiles2ExpandedArea.Top - top;
				_swipe.InnerLoop2Start = _offScreenTiles2ExpandedArea.Bottom - top;
				_swipe.InnerLoop1End = _requestedArea.Bottom + RenderPhases - top;
				_swipe.InnerLoop2End = _requestedArea.Top - RenderPhases - top;
				num3 = _offScreenTiles2ExpandedArea.Left - left;
				num4 = _offScreenTiles2ExpandedArea.Right - left;
				_swipe.JaggedArray = _states;
				break;
			case 3:
				_swipe.InnerLoop1Start = _offScreenTiles2ExpandedArea.Left - left;
				_swipe.InnerLoop2Start = _offScreenTiles2ExpandedArea.Right - left;
				_swipe.InnerLoop1End = _requestedArea.Right + RenderPhases - left;
				_swipe.InnerLoop2End = _requestedArea.Left - RenderPhases - left;
				num3 = _offScreenTiles2ExpandedArea.Top - top;
				num4 = _offScreenTiles2ExpandedArea.Bottom - top;
				_swipe.JaggedArray = _axisFlipStates;
				break;
			}
			if (_swipe.InnerLoop1Start > _swipe.InnerLoop1End)
			{
				_swipe.InnerLoop1Start = _swipe.InnerLoop1End;
			}
			if (_swipe.InnerLoop2Start < _swipe.InnerLoop2End)
			{
				_swipe.InnerLoop2Start = _swipe.InnerLoop2End;
			}
			if (num3 > num4)
			{
				num3 = num4;
			}
			FastParallel.For(num3, num4, Mode switch
			{
				0 => (ParallelForAction)doColors_Mode0_Swipe, 
				1 => (ParallelForAction)doColors_Mode1_Swipe, 
				2 => (ParallelForAction)doColors_Mode2_Swipe, 
				3 => (ParallelForAction)doColors_Mode3_Swipe, 
				_ => (ParallelForAction)doColors_Mode0_Swipe, 
			}, (object)_swipe);
			_swipeRandom.NextSeed();
			TimeLogger.LightingByPass[i].AddTime(fromTimestamp);
		}
	}

	private void doColors_Mode0_Swipe(int outerLoopStart, int outerLoopEnd, object context)
	{
		LightingSwipeData lightingSwipeData = context as LightingSwipeData;
		FastRandom fastRandom = default;
		try
		{
			bool flag = true;
			while (true)
			{
				int num;
				int val;
				int val2;
				if (flag)
				{
					num = 1;
					val = lightingSwipeData.InnerLoop1Start;
					val2 = lightingSwipeData.InnerLoop1End;
				}
				else
				{
					num = -1;
					val = lightingSwipeData.InnerLoop2Start;
					val2 = lightingSwipeData.InnerLoop2End;
				}
				for (int i = outerLoopStart; i < outerLoopEnd; i++)
				{
					LightingState[] array = lightingSwipeData.JaggedArray[i];
					float num2 = 0f;
					float num3 = 0f;
					float num4 = 0f;
					int num5 = Math.Min(array.Length - 1, Math.Max(0, val));
					int num6 = Math.Min(array.Length - 1, Math.Max(0, val2));
					for (int j = num5; j != num6; j += num)
					{
						LightingState lightingState = array[j];
						LightingState lightingState2 = array[j + num];
						bool flag3;
						bool flag2 = (flag3 = false);
						if (lightingState.R2 > num2)
						{
							num2 = lightingState.R2;
						}
						else if ((double)num2 <= 0.0185)
						{
							flag2 = true;
						}
						else if (lightingState.R2 < num2)
						{
							lightingState.R2 = num2;
						}
						if (lightingState.WetLight)
						{
							fastRandom = _swipeRandom.WithModifier((ulong)(i * 1000 + j));
						}
						if (!flag2 && lightingState2.R2 <= num2)
						{
							if (lightingState.StopLight)
							{
								num2 *= _negLight2;
							}
							else if (lightingState.CrackedLight)
							{
								num2 *= _negLight3;
							}
							else if (lightingState.WetLight)
							{
								num2 = ((!lightingState.HoneyLight) ? (num2 * (_wetLightR * (float)fastRandom.Next(98, 100) * 0.01f)) : (num2 * (_honeyLightR * (float)fastRandom.Next(98, 100) * 0.01f)));
							}
							else
							{
								num2 *= _negLight;
							}
						}
						if (lightingState.G2 > num3)
						{
							num3 = lightingState.G2;
						}
						else if ((double)num3 <= 0.0185)
						{
							flag3 = true;
						}
						else
						{
							lightingState.G2 = num3;
						}
						if (!flag3 && lightingState2.G2 <= num3)
						{
							if (lightingState.StopLight)
							{
								num3 *= _negLight2;
							}
							else if (lightingState.CrackedLight)
							{
								num3 *= _negLight3;
							}
							else if (lightingState.WetLight)
							{
								num3 = ((!lightingState.HoneyLight) ? (num3 * (_wetLightG * (float)fastRandom.Next(97, 100) * 0.01f)) : (num3 * (_honeyLightG * (float)fastRandom.Next(97, 100) * 0.01f)));
							}
							else
							{
								num3 *= _negLight;
							}
						}
						if (lightingState.B2 > num4)
						{
							num4 = lightingState.B2;
						}
						else
						{
							if ((double)num4 <= 0.0185)
							{
								continue;
							}
							lightingState.B2 = num4;
						}
						if (!(lightingState2.B2 >= num4))
						{
							if (lightingState.StopLight)
							{
								num4 *= _negLight2;
							}
							else if (lightingState.CrackedLight)
							{
								num4 *= _negLight3;
							}
							else if (lightingState.WetLight)
							{
								num4 = ((!lightingState.HoneyLight) ? (num4 * (_wetLightB * (float)fastRandom.Next(97, 100) * 0.01f)) : (num4 * (_honeyLightB * (float)fastRandom.Next(97, 100) * 0.01f)));
							}
							else
							{
								num4 *= _negLight;
							}
						}
					}
				}
				if (flag)
				{
					flag = false;
					continue;
				}
				break;
			}
		}
		catch
		{
		}
	}

	private void doColors_Mode1_Swipe(int outerLoopStart, int outerLoopEnd, object context)
	{
		LightingSwipeData lightingSwipeData = context as LightingSwipeData;
		FastRandom fastRandom = default;
		try
		{
			bool flag = true;
			while (true)
			{
				int num;
				int num2;
				int num3;
				if (flag)
				{
					num = 1;
					num2 = lightingSwipeData.InnerLoop1Start;
					num3 = lightingSwipeData.InnerLoop1End;
				}
				else
				{
					num = -1;
					num2 = lightingSwipeData.InnerLoop2Start;
					num3 = lightingSwipeData.InnerLoop2End;
				}
				for (int i = outerLoopStart; i < outerLoopEnd; i++)
				{
					LightingState[] array = lightingSwipeData.JaggedArray[i];
					float num4 = 0f;
					for (int j = num2; j != num3; j += num)
					{
						LightingState lightingState = array[j];
						if (lightingState.R2 > num4)
						{
							num4 = lightingState.R2;
						}
						else
						{
							if ((double)num4 <= 0.0185)
							{
								continue;
							}
							if (lightingState.R2 < num4)
							{
								lightingState.R2 = num4;
							}
						}
						if (!(array[j + num].R2 > num4))
						{
							if (lightingState.StopLight)
							{
								num4 *= _negLight2;
							}
							else if (lightingState.CrackedLight)
							{
								num4 *= _negLight3;
							}
							else if (lightingState.WetLight)
							{
								fastRandom = _swipeRandom.WithModifier((ulong)(i * 1000 + j));
								num4 = ((!lightingState.HoneyLight) ? (num4 * (_wetLightR * (float)fastRandom.Next(98, 100) * 0.01f)) : (num4 * (_honeyLightR * (float)fastRandom.Next(98, 100) * 0.01f)));
							}
							else
							{
								num4 *= _negLight;
							}
						}
					}
				}
				if (flag)
				{
					flag = false;
					continue;
				}
				break;
			}
		}
		catch
		{
		}
	}

	private void doColors_Mode2_Swipe(int outerLoopStart, int outerLoopEnd, object context)
	{
		LightingSwipeData lightingSwipeData = context as LightingSwipeData;
		try
		{
			bool flag = true;
			while (true)
			{
				int num;
				int num2;
				int num3;
				if (flag)
				{
					num = 1;
					num2 = lightingSwipeData.InnerLoop1Start;
					num3 = lightingSwipeData.InnerLoop1End;
				}
				else
				{
					num = -1;
					num2 = lightingSwipeData.InnerLoop2Start;
					num3 = lightingSwipeData.InnerLoop2End;
				}
				for (int i = outerLoopStart; i < outerLoopEnd; i++)
				{
					LightingState[] array = lightingSwipeData.JaggedArray[i];
					float num4 = 0f;
					for (int j = num2; j != num3; j += num)
					{
						LightingState lightingState = array[j];
						if (lightingState.R2 > num4)
						{
							num4 = lightingState.R2;
						}
						else
						{
							if (num4 <= 0f)
							{
								continue;
							}
							lightingState.R2 = num4;
						}
						if (lightingState.StopLight)
						{
							num4 -= _negLight2;
						}
						else if (lightingState.CrackedLight)
						{
							num4 -= _negLight3;
						}
						else
						{
							num4 = ((!lightingState.WetLight) ? (num4 - _negLight) : (num4 - _wetLightR));
						}
					}
				}
				if (flag)
				{
					flag = false;
					continue;
				}
				break;
			}
		}
		catch
		{
		}
	}

	private void doColors_Mode3_Swipe(int outerLoopStart, int outerLoopEnd, object context)
	{
		LightingSwipeData lightingSwipeData = context as LightingSwipeData;
		try
		{
			bool flag = true;
			while (true)
			{
				int num;
				int num2;
				int num3;
				if (flag)
				{
					num = 1;
					num2 = lightingSwipeData.InnerLoop1Start;
					num3 = lightingSwipeData.InnerLoop1End;
				}
				else
				{
					num = -1;
					num2 = lightingSwipeData.InnerLoop2Start;
					num3 = lightingSwipeData.InnerLoop2End;
				}
				for (int i = outerLoopStart; i < outerLoopEnd; i++)
				{
					LightingState[] array = lightingSwipeData.JaggedArray[i];
					float num4 = 0f;
					float num5 = 0f;
					float num6 = 0f;
					for (int j = num2; j != num3; j += num)
					{
						LightingState lightingState = array[j];
						bool flag3;
						bool flag2 = (flag3 = false);
						if (lightingState.R2 > num4)
						{
							num4 = lightingState.R2;
						}
						else if (num4 <= 0f)
						{
							flag2 = true;
						}
						else
						{
							lightingState.R2 = num4;
						}
						if (!flag2)
						{
							if (lightingState.StopLight)
							{
								num4 -= _negLight2;
							}
							if (lightingState.CrackedLight)
							{
								num4 -= _negLight3;
							}
							else
							{
								num4 = ((!lightingState.WetLight) ? (num4 - _negLight) : (num4 - _wetLightR));
							}
						}
						if (lightingState.G2 > num5)
						{
							num5 = lightingState.G2;
						}
						else if (num5 <= 0f)
						{
							flag3 = true;
						}
						else
						{
							lightingState.G2 = num5;
						}
						if (!flag3)
						{
							if (lightingState.StopLight)
							{
								num5 -= _negLight2;
							}
							if (lightingState.CrackedLight)
							{
								num5 -= _negLight3;
							}
							else
							{
								num5 = ((!lightingState.WetLight) ? (num5 - _negLight) : (num5 - _wetLightG));
							}
						}
						if (lightingState.B2 > num6)
						{
							num6 = lightingState.B2;
						}
						else
						{
							if (num6 <= 0f)
							{
								continue;
							}
							lightingState.B2 = num6;
						}
						if (lightingState.StopLight)
						{
							num6 -= _negLight2;
						}
						num6 = ((!lightingState.CrackedLight) ? (num6 - _negLight) : (num6 - _negLight3));
					}
				}
				if (flag)
				{
					flag = false;
					continue;
				}
				break;
			}
		}
		catch
		{
		}
	}
}
