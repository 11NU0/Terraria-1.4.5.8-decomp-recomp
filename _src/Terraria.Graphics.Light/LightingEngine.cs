using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.Graphics.Capture;
using Terraria.Map;
using Terraria.Testing;

namespace Terraria.Graphics.Light;

public class LightingEngine : ILightingEngine
{
	private enum EngineState
	{
		MinimapUpdate,
		ExportMetrics,
		Scan,
		Blur,
		Max
	}

	private struct PerFrameLight(Point position, Vector3 color)
	{
		public readonly Point Position = position;

		public readonly Vector3 Color = color;
	}

	public const int AREA_PADDING = 28;

	private const int NON_VISIBLE_PADDING = 18;

	private uint _perFrameLightSwapUpdateCount;

	private List<PerFrameLight> _perFrameLights = new List<PerFrameLight>();

	private List<PerFrameLight> _oldPerFrameLights = new List<PerFrameLight>();

	private TileLightScanner _tileScanner = new TileLightScanner();

	private LightMap _activeLightMap = new LightMap();

	private Rectangle _activeProcessedArea;

	private LightMap _workingLightMap = new LightMap();

	private Rectangle _workingProcessedArea;

	private EngineState _state;

	public void AddLight(int x, int y, Vector3 color)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_perFrameLights.Add(new PerFrameLight(new Point(x, y), color));
	}

	public void Clear()
	{
		_activeLightMap.Clear();
		_workingLightMap.Clear();
		_perFrameLights.Clear();
		_oldPerFrameLights.Clear();
		_perFrameLightSwapUpdateCount = 0u;
	}

	public Vector3 GetColor(int x, int y)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!_activeProcessedArea.Contains(x, y))
		{
			return Vector3.Zero;
		}
		x -= _activeProcessedArea.X;
		y -= _activeProcessedArea.Y;
		return _activeLightMap[x, y];
	}

	public void ProcessArea(Rectangle area)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		switch (_state)
		{
		case EngineState.MinimapUpdate:
			if (Main.mapDelay > 0)
			{
				Main.mapDelay--;
			}
			else
			{
				ExportToMiniMap();
			}
			Main.renderCount = 3;
			break;
		case EngineState.ExportMetrics:
			Main.UpdateSceneMetrics();
			Main.renderCount = 0;
			break;
		case EngineState.Scan:
			ProcessScan(area);
			Main.renderCount = 1;
			break;
		case EngineState.Blur:
			ProcessBlur();
			Present();
			Main.renderCount = 2;
			break;
		}
		TimeLogger.LightingByPass[(int)_state].AddTime(fromTimestamp);
		IncrementState();
	}

	private void IncrementState()
	{
		_state = (EngineState)((int)(_state + 1) % 4);
	}

	private void ProcessScan(Rectangle area)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		area.Inflate(28, 28);
		_workingProcessedArea = area;
		_workingLightMap.SetSize(area.Width, area.Height);
		_workingLightMap.NonVisiblePadding = 18;
		_tileScanner.Update();
		_tileScanner.ExportTo(area, _workingLightMap, new TileLightScannerOptions
		{
			DrawInvisibleWalls = Main.ShouldShowInvisibleBlocksAndWalls()
		});
	}

	private void ProcessBlur()
	{
		UpdateLightDecay();
		ApplyPerFrameLights();
		_workingLightMap.Blur();
	}

	private void Present()
	{
		Utils.Swap(ref _activeLightMap, ref _workingLightMap);
		Utils.Swap(ref _activeProcessedArea, ref _workingProcessedArea);
	}

	private void UpdateLightDecay()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		LightMap workingLightMap = _workingLightMap;
		workingLightMap.LightDecayThroughAir = 0.91f;
		workingLightMap.LightDecayThroughSolid = 0.56f;
		workingLightMap.LightDecayThroughHoney = new Vector3(0.75f, 0.7f, 0.6f) * 0.91f;
		switch (Main.waterStyle)
		{
		case 0:
		case 1:
		case 7:
		case 8:
			workingLightMap.LightDecayThroughWater = new Vector3(0.88f, 0.96f, 1.015f) * 0.91f;
			break;
		case 2:
			workingLightMap.LightDecayThroughWater = new Vector3(0.94f, 0.85f, 1.01f) * 0.91f;
			break;
		case 3:
			workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.95f, 1.015f) * 0.91f;
			break;
		case 4:
			workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 0.86f, 1.01f) * 0.91f;
			break;
		case 5:
			workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.99f, 1.01f) * 0.91f;
			break;
		case 6:
			workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 0.93f, 0.98f) * 0.91f;
			break;
		case 9:
			workingLightMap.LightDecayThroughWater = new Vector3(1f, 0.88f, 0.84f) * 0.91f;
			break;
		case 10:
			workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 1f, 1f) * 0.91f;
			break;
		case 12:
			workingLightMap.LightDecayThroughWater = new Vector3(0.95f, 0.98f, 0.85f) * 0.91f;
			break;
		case 13:
			workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 1f, 1.02f) * 0.91f;
			break;
		}
		Player perspectivePlayer = Main.SceneMetrics.PerspectivePlayer;
		if (perspectivePlayer.nightVision)
		{
			workingLightMap.LightDecayThroughAir *= 1.03f;
			workingLightMap.LightDecayThroughSolid *= 1.03f;
		}
		if (perspectivePlayer.blind)
		{
			workingLightMap.LightDecayThroughAir *= 0.95f;
			workingLightMap.LightDecayThroughSolid *= 0.95f;
		}
		if (perspectivePlayer.blackout)
		{
			workingLightMap.LightDecayThroughAir *= 0.85f;
			workingLightMap.LightDecayThroughSolid *= 0.85f;
		}
		if (perspectivePlayer.headcovered)
		{
			workingLightMap.LightDecayThroughAir *= 0.85f;
			workingLightMap.LightDecayThroughSolid *= 0.85f;
		}
		workingLightMap.LightDecayThroughAir *= Main.SceneState.airLightDecay;
		workingLightMap.LightDecayThroughSolid *= Main.SceneState.solidLightDecay;
	}

	private void ApplyPerFrameLights()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		List<PerFrameLight> list = _perFrameLights;
		if (Main.GameUpdateCount == _perFrameLightSwapUpdateCount)
		{
			list = _oldPerFrameLights;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Point position = list[i].Position;
			if (_workingProcessedArea.Contains(position))
			{
				Vector3 color = list[i].Color;
				Vector3 val = _workingLightMap[position.X - _workingProcessedArea.X, position.Y - _workingProcessedArea.Y];
				Vector3.Max(ref val, ref color, out color);
				_workingLightMap[position.X - _workingProcessedArea.X, position.Y - _workingProcessedArea.Y] = color;
			}
		}
		if (!CaptureManager.Instance.IsCapturing)
		{
			if (Main.GameUpdateCount != _perFrameLightSwapUpdateCount)
			{
				Utils.Swap(ref _perFrameLights, ref _oldPerFrameLights);
			}
			_perFrameLights.Clear();
			_perFrameLightSwapUpdateCount = Main.GameUpdateCount;
		}
	}

	public void Rebuild()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_activeProcessedArea = Rectangle.Empty;
		_workingProcessedArea = Rectangle.Empty;
		_state = EngineState.MinimapUpdate;
		_activeLightMap = new LightMap();
		_workingLightMap = new LightMap();
	}

	private void ExportToMiniMap()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected Obj, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.mapEnabled || _activeProcessedArea.Width <= 0 || _activeProcessedArea.Height <= 0)
		{
			return;
		}
		Rectangle area = new Rectangle(_activeProcessedArea.X + 28, _activeProcessedArea.Y + 28, _activeProcessedArea.Width - 56, _activeProcessedArea.Height - 56);
		Rectangle val = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
		val.Inflate(-40, -40);
		area = Rectangle.Intersect(area, val);
		area = Rectangle.Intersect(area, MapHelper.sceneArea);
		FastParallel.For(area.Left, area.Right, (ParallelForAction)((int start, int end, object context) =>
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			for (int i = start; i < end; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					Vector3 val2 = _activeLightMap[i - _activeProcessedArea.X, j - _activeProcessedArea.Y];
					float num = Math.Max(Math.Max(val2.X, val2.Y), val2.Z);
					byte light = (byte)Math.Min(255, (int)(num * 255f));
					Main.Map.UpdateLighting(i, j, light);
				}
			}
		}), (object)null);
		Main.updateMap = area;
	}

	internal void AddGameplaySnapshotComponents()
	{
		StateSnapshot.Gameplay.AddCollection("NewLightingEngine._perFrameLights", _perFrameLights);
		StateSnapshot.Gameplay.AddCollection("NewLightingEngine._oldPerFrameLights", _oldPerFrameLights);
		StateSnapshot.Gameplay.AddVal("NewLightingEngine._perFrameLightSwapUpdateCount", () => _perFrameLightSwapUpdateCount, (uint v) =>
		{
			_perFrameLightSwapUpdateCount = v;
		});
	}
}
