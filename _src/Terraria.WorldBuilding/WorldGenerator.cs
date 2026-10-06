#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ReLogic.Threading;
using Terraria.GameContent.UI.States;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.WorldBuilding;

public class WorldGenerator
{
	public enum SnapshotFrequency
	{
		None = -1,
		Manual,
		Automatic,
		Always
	}

	public class Controller
	{
		private WorldManifest _previousManifest;

		private Dictionary<GenPass, WorldGenSnapshot> _snapshots;

		public Action<Controller> OnPassesLoaded;

		private WorldGenerator _generator;

		private bool _paused;

		public List<GenPass> Passes => _generator._passes;

		public GenPass CurrentPass => _generator._currentPass;

		public GenPass LastCompletedPass
		{
			get
			{
				if (PassResults.Count != 0)
				{
					return Passes[PassResults.Count - 1];
				}
				return null;
			}
		}

		public GenPass PauseAfterPass { get; set; }

		public bool PauseOnHashMismatch { get; set; }

		public bool PausedDueToHashMismatch { get; set; }

		public SnapshotFrequency SnapshotFrequency { get; set; }

		public bool Paused
		{
			get
			{
				return _paused;
			}
			set
			{
				_paused = value;
				if (value)
				{
					PauseAfterPass = null;
				}
				else
				{
					PausedDueToHashMismatch = false;
				}
			}
		}

		public bool QueuedAbort { get; set; }

		public WorldGenSnapshot GetSnapshot(GenPass pass)
		{
			if (!_snapshots.TryGetValue(pass, out var value))
			{
				return null;
			}
			return value;
		}

		public Controller(WorldManifest prevManifest = null)
		{
			_previousManifest = prevManifest;
			PauseOnHashMismatch = true;
			SnapshotFrequency = SnapshotFrequency.None;
		}

		internal void SetGenerator(WorldGenerator generator)
		{
			_generator = generator;
			_snapshots = WorldGenSnapshot.LoadSnapshots(_previousManifest, Passes);
			if (_previousManifest != null)
			{
				foreach (GenPassResult r in _previousManifest.GenPassResults.Where((GenPassResult genPassResult) => genPassResult.Skipped))
				{
					Passes.SingleOrDefault((GenPass p) => p.Name == r.Name)?.Disable();
				}
			}
			if (OnPassesLoaded != null)
			{
				OnPassesLoaded(this);
			}
		}

		internal void OnPaused()
		{
			SetDebugWorldGenUIVisibility(visible: true);
			ForceUpdateProgress();
			Thread.Sleep(10);
		}

		internal void OnPassCompleted()
		{
			int num = PassResults.Count - 1;
			GenPassResult genPassResult = PassResults[num];
			WorldGenSnapshot snapshot = GetSnapshot(CurrentPass);
			GenPass genPass = Passes.Skip(PassResults.Count).FirstOrDefault();
			if (UIWorldGenDebug.ActiveInstance != null || genPass == null)
			{
				genPassResult.Hash = HashWorld();
			}
			Trace.WriteLine(genPassResult);
			foreach (GenPass item in Passes.Skip(num))
			{
				WorldGenSnapshot snapshot2 = GetSnapshot(item);
				if (snapshot2 != null && !snapshot2.GenPassResults[num].Matches(genPassResult))
				{
					_snapshots.Remove(item);
				}
			}
			bool flag = SnapshotFrequency == SnapshotFrequency.Always || (SnapshotFrequency == SnapshotFrequency.Automatic && (MsSinceLastSnapshot() > 500 || (genPass != null && genPass == PauseAfterPass)));
			if (genPassResult.Skipped)
			{
				flag = false;
			}
			if (QueuedAbort)
			{
				flag = false;
			}
			if (snapshot != null && snapshot.IsValidHistoryOf(WorldGen.Manifest))
			{
				flag = false;
				if (snapshot.Outdated)
				{
					snapshot.ResaveForCurrentVersion();
				}
			}
			if (flag)
			{
				TryCreateSnapshot();
			}
			CheckLatestPassResultAgainstManifest(num, genPassResult, snapshot);
			if (PauseAfterPass == CurrentPass)
			{
				Paused = true;
			}
			if (!Main.gameMenu)
			{
				Main.QueueMainThreadAction(Main.sectionManager.SetAllFramedSectionsAsNeedingRefresh);
			}
		}

		private void CheckLatestPassResultAgainstManifest(int currentPassIndex, GenPassResult result, WorldGenSnapshot prevSnapshot)
		{
			if (_previousManifest == null || currentPassIndex >= _previousManifest.GenPassResults.Count || _previousManifest.GenPassResults[currentPassIndex].Matches(result))
			{
				return;
			}
			_previousManifest = null;
			string text = $"{CurrentPass.Name} output changed since last gen.";
			if (PauseOnHashMismatch && prevSnapshot != null)
			{
				try
				{
					prevSnapshot.Load();
					ReportException(text + " The previous output has been loaded as a snapshot (use /swap and /snapshotdiff to compare)");
				}
				catch (Exception ex)
				{
					ReportException(text + "An attempt was made to load a snapshot of the previous output, but an exception occurred", ex);
				}
			}
			else
			{
				ReportException(text, null, PauseOnHashMismatch);
			}
			if (PauseOnHashMismatch)
			{
				Paused = true;
				PausedDueToHashMismatch = true;
			}
		}

		public void DeleteSnapshot(GenPass pass)
		{
			Utils.TryOperateInLock(pass, () =>
			{
				if (_snapshots.TryGetValue(pass, out var value))
				{
					_snapshots.Remove(pass);
					WorldGenSnapshot.Delete(value);
				}
			});
		}

		public void DeleteAllSnapshots()
		{
			TryOperateInControlLock(() =>
			{
				_snapshots.Clear();
				WorldGenSnapshot.DeleteAllForCurrentWorld();
			});
		}

		private int MsSinceLastSnapshot()
		{
			int num = Passes.GetRange(0, PassResults.Count).FindLastIndex(_snapshots.ContainsKey);
			return PassResults.Skip(num + 1).Sum((GenPassResult r) => r.DurationMs);
		}

		public void ForceUpdateProgress()
		{
			GenerationProgress progress = _generator._progress;
			progress.Message = ((PassResults.Count == 0) ? "World Cleared" : ("Paused after " + Passes[PassResults.Count - 1].Name));
			progress.TotalWeight = Passes.Where((GenPass p) => p.Enabled).Sum((GenPass p) => p.Weight);
			progress.TotalWeightedProgress = (from p in Passes.Take(PassResults.Count)
				where p.Enabled
				select p).Sum((GenPass p) => p.Weight);
		}

		public bool TryOperateInControlLock(Action action)
		{
			return Utils.TryOperateInLock(_generator._controlLock, action);
		}

		public bool TryCreateSnapshot()
		{
			return TryOperateInControlLock(() =>
			{
				if (!WorldGen.Manifest.FinalHash.HasValue)
				{
					Main.NewText("Pass was not run with worldgen debugging enabled, please re-run", 240, 30, 30);
				}
				else
				{
					if (WorldGen.Manifest.FinalHash == HashWorld())
					{
						try
						{
							_snapshots[LastCompletedPass] = WorldGenSnapshot.Create();
							return;
						}
						catch (Exception ex)
						{
							ReportException("Exception occured while creating snapshot", ex);
							return;
						}
					}
					Main.NewText("World has been modified since last gen pass completed. Please rerun or use /snapshot instead", 240, 30, 30);
				}
			});
		}

		public bool TryReset()
		{
			return TryOperateInControlLock(() =>
			{
				UpdatePreviousManifest();
				WorldGen.RestoreTemporaryStateChanges();
				WorldGen.clearWorld();
				WorldGen.Reset();
				ForceUpdateProgress();
				Paused = true;
				Main.NewText("World Reset", byte.MaxValue, byte.MaxValue, 0);
			});
		}

		private void UpdatePreviousManifest()
		{
			if (_previousManifest == null || PassResults.Count > _previousManifest.GenPassResults.Count)
			{
				_previousManifest = WorldGen.Manifest;
			}
		}

		public bool TryResetToSnapshot(GenPass pass)
		{
			WorldGenSnapshot snap = GetSnapshot(pass);
			if (snap == null || snap.Outdated)
			{
				return false;
			}
			return TryOperateInControlLock(() =>
			{
				try
				{
					UpdatePreviousManifest();
					snap.Restore();
					ForceUpdateProgress();
				}
				catch (Exception ex)
				{
					ReportException("Exception occured while restoring snapshot", ex);
				}
			});
		}

		public bool TryRunToEndOfPass(GenPass pass, bool useSnapshots = true, bool mustRunPass = true)
		{
			if (!pass.Enabled)
			{
				return false;
			}
			int passIndex = Passes.IndexOf(pass);
			if (TryOperateInControlLock(() =>
			{
				GenPass genPass = Passes.Take(passIndex + ((!mustRunPass) ? 1 : 0)).Reverse().FirstOrDefault((GenPass p) => GetSnapshot(p) != null && !GetSnapshot(p).Outdated);
				bool flag = passIndex < PassResults.Count;
				if (useSnapshots && genPass != null && (flag || Passes.IndexOf(genPass) >= PassResults.Count))
				{
					TryResetToSnapshot(genPass);
				}
				else if (flag)
				{
					TryReset();
				}
				if (PassResults.Count == passIndex + 1)
				{
					Invariant.Assert(!mustRunPass, "mustRunPass, but pass already completed, TryReset above failed?");
					Paused = true;
				}
				else
				{
					PauseAfterPass = pass;
					Paused = false;
				}
			}))
			{
				return true;
			}
			if (pass == CurrentPass || passIndex > PassResults.Count)
			{
				PauseAfterPass = pass;
				return true;
			}
			return false;
		}

		public bool TryResetToPreviousPass(GenPass pass)
		{
			int count = Passes.IndexOf(pass);
			GenPass genPass = Passes.Take(count).Reverse().FirstOrDefault((GenPass p) => p.Enabled);
			if (genPass == null)
			{
				return TryReset();
			}
			return TryRunToEndOfPass(genPass, useSnapshots: true, mustRunPass: false);
		}

		internal void ReportException(string message, Exception ex = null, bool pause = true)
		{
			Trace.WriteLine((ex != null) ? ex.ToString() : message);
			if (DebugOptions.enableDebugCommands)
			{
				if (pause)
				{
					Paused = true;
				}
				SetDebugWorldGenUIVisibility(visible: true);
				UIWorldGenDebug.ActiveInstance.UnhideChat();
				Main.NewText(message, byte.MaxValue, 0, 0);
			}
		}
	}

	internal readonly List<GenPass> _passes = new List<GenPass>();

	private readonly int _seed;

	private readonly WorldGenConfiguration _configuration;

	private readonly GenerationProgress _progress;

	private readonly Controller _controller;

	private readonly object _controlLock = new object();

	private GenPass _currentPass;

	public static GenerationProgress CurrentGenerationProgress;

	public static Controller CurrentController;

	private static Stopwatch _hashTime = new Stopwatch();

	public static List<GenPassResult> PassResults => WorldGen.Manifest.GenPassResults;

	public WorldGenerator(int seed, WorldGenConfiguration configuration, GenerationProgress progress = null, Controller controller = null)
	{
		_seed = seed;
		_configuration = configuration;
		_progress = ((progress == null) ? new GenerationProgress() : progress);
		_controller = ((controller == null) ? new Controller() : controller);
	}

	public void Append(GenPass pass)
	{
		_passes.Add(pass);
	}

	public bool GenerateWorld()
	{
		_hashTime.Reset();
		_controller.SetGenerator(this);
		CurrentController = _controller;
		_progress.TotalWeight = _passes.Where((GenPass p) => p.Enabled).Sum((GenPass p) => p.Weight);
		CurrentGenerationProgress = _progress;
		if (_controller.PauseAfterPass != null)
		{
			SetDebugWorldGenUIVisibility(visible: true);
		}
		bool flag = false;
		while (true)
		{
			if (_controller.QueuedAbort)
			{
				flag = true;
				break;
			}
			if (_controller.Paused)
			{
				_controller.OnPaused();
				continue;
			}
			lock (_controlLock)
			{
				if (PassResults.Count == _passes.Count)
				{
					break;
				}
				_currentPass = _passes[PassResults.Count];
				lock (_currentPass)
				{
					PassResults.Add(RunPass(_currentPass));
					_controller.OnPassCompleted();
				}
				_currentPass = null;
				continue;
			}
		}
		Trace.WriteLine(string.Join("\n", PassResults) + $"\nFinished world - Seed: {Main.ActiveWorldFileData.SeedText} Width: {Main.maxTilesX}, Height: {Main.maxTilesY}, Evil: {WorldGen.WorldGenParam_Evil}, Difficulty: {Main.GameMode}\nTotal Generation Time: {PassResults.Sum((GenPassResult r) => r.DurationMs)}\n");
		SetDebugWorldGenUIVisibility(visible: false);
		CurrentGenerationProgress = null;
		CurrentController = null;
		return !flag;
	}

	private static void SetDebugWorldGenUIVisibility(bool visible)
	{
		bool flag = UIWorldGenDebug.ActiveInstance != null;
		if (visible == flag)
		{
			return;
		}
		Main.RunOnMainThread(() =>
		{
			if (visible)
			{
				UIWorldGenDebug.Open();
			}
			else
			{
				UIWorldGenDebug.Close();
			}
		}).Wait();
	}

	private GenPassResult RunPass(GenPass pass)
	{
		if (!pass.Enabled)
		{
			return new GenPassResult
			{
				Name = pass.Name,
				Skipped = true
			};
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		Main.rand = new UnifiedRandom(_seed);
		_progress.Start(pass.Weight);
		try
		{
			pass.Apply(_progress, _configuration.GetPassConfiguration(pass.Name));
		}
		catch (Exception ex)
		{
			_controller.ReportException("Exception in Pass: " + pass.Name, ex);
		}
		_progress.End();
		Invariant.Assert(Liquid.numLiquid == 0 && LiquidBuffer.numLiquidBuffer == 0, "Liquid left in flux by pass: {0}", pass.Name);
		return new GenPassResult
		{
			Name = pass.Name,
			DurationMs = (int)stopwatch.ElapsedMilliseconds,
			RandNext = WorldGen.genRand.Next()
		};
	}

	public static uint HashWorld()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected Obj, but got Unknown
		_hashTime.Start();
		uint[] line_hashes = new uint[Main.maxTilesX];
		FastParallel.For(0, Main.maxTilesX, (ParallelForAction)((int x0, int x1, object _) =>
		{
			Tile[,] tile = Main.tile;
			int maxTilesY = Main.maxTilesY;
			for (int i = x0; i < x1; i++)
			{
				uint num4 = 0u;
				for (int j = 0; j < maxTilesY; j++)
				{
					num4 ^= (uint)TileSnapshot.TileStruct.From(tile[i, j]).GetHashCode();
					num4 = (num4 << 13) | (num4 >> 19);
					num4 = num4 * 5 + 3864292196u;
				}
				line_hashes[i] = num4;
			}
		}), (object)null);
		uint num = 0u;
		uint[] array = line_hashes;
		foreach (uint num3 in array)
		{
			num ^= num3;
			num = (num << 13) | (num >> 19);
			num = num * 5 + 3864292196u;
		}
		_hashTime.Stop();
		return num;
	}
}
