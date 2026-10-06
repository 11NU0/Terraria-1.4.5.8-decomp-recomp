using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.Utilities.Terraria.Utilities;

namespace Terraria.GameContent;

public class LightningGenerator
{
	public class Bolt
	{
		public Vector2[] positions;

		public float[] rotations;

		public FloatRange progressRange;

		public int forkDepth;

		public bool collidedWithTile;

		public bool IsMainBolt => forkDepth == 0;
	}

	public bool SolidTileCollision;

	public float SourceRotationLimit = (float)Math.PI / 9f;

	public float Length = 1000f;

	public float RotationStrength;

	public int StepSize;

	public int Layers;

	public float LayerStrengthFactor;

	public float PerpendicularDeviationFactor;

	public float ReduceRandomnessAfter;

	public float ForkGenerationThresholdAngleFraction;

	public float ForkReflectAngleMultiplier;

	public float ForkRotationStrengthMultiplier;

	public float ForkStepSizeMultiplier;

	public float ForkLengthMultiplier;

	public int MaxForksPerBolt;

	public int MaxForkDepth;

	public FloatRange ForkProgressRange;

	public static LightningGenerator StormLightning = new LightningGenerator
	{
		SourceRotationLimit = (float)Math.PI / 9f,
		Length = 1000f,
		RotationStrength = 0.9f,
		StepSize = 8,
		Layers = 4,
		LayerStrengthFactor = 1.5f,
		PerpendicularDeviationFactor = 5f,
		ReduceRandomnessAfter = 0.8f,
		ForkGenerationThresholdAngleFraction = 0.65f,
		ForkReflectAngleMultiplier = 0.4f,
		ForkRotationStrengthMultiplier = 0.9f,
		ForkStepSizeMultiplier = 0.8f,
		ForkLengthMultiplier = 0.8f,
		MaxForksPerBolt = 2,
		MaxForkDepth = 2,
		ForkProgressRange = new FloatRange(0.3f, 0.8f),
		SolidTileCollision = true
	};

	public static LightningGenerator LightningStrikeWeapon = new LightningGenerator
	{
		SourceRotationLimit = (float)Math.PI / 36f,
		Length = 750f,
		RotationStrength = 0.9f,
		StepSize = 8,
		Layers = 4,
		LayerStrengthFactor = 1.5f,
		PerpendicularDeviationFactor = 5f,
		ReduceRandomnessAfter = 0.8f,
		ForkGenerationThresholdAngleFraction = 0.65f,
		ForkReflectAngleMultiplier = 0.4f,
		ForkRotationStrengthMultiplier = 0.9f,
		ForkStepSizeMultiplier = 0.8f,
		ForkLengthMultiplier = 0.8f,
		MaxForksPerBolt = 2,
		MaxForkDepth = 2,
		ForkProgressRange = new FloatRange(0.3f, 0.8f),
		SolidTileCollision = false
	};

	public Bolt Generate(List<Bolt> bolts, uint seed, Vector2 targetPosition, Vector2 direction, bool calcPositions, bool calcRotations)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		direction = direction.RotatedBy((new LCG32Random(seed).NextDouble() * 2.0 - 1.0) * (double)SourceRotationLimit) * Length;
		Bolt result = GenerateBolt(bolts, seed, 0, calcPositions, targetPosition - direction, targetPosition, RotationStrength, StepSize, new FloatRange(0f, 1f));
		if (calcRotations)
		{
			foreach (Bolt bolt in bolts)
			{
				bolt.rotations = CalcRotations(bolt.positions);
			}
		}
		return result;
	}

	private Bolt GenerateBolt(List<Bolt> bolts, uint seed, int depth, bool calcPositions, Vector2 startPos, Vector2 targetPos, float rotationStrength, float stepSize, FloatRange progressRange)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		LCG32Random lCG32Random = new LCG32Random(seed);
		float num = 0f;
		float[] array = new float[Layers];
		Point val = targetPos.ToTileCoordinates();
		Vector2 val2 = startPos;
		Vector2 val3 = targetPos - startPos;
		float num2 = val3.Length();
		val3 /= num2;
		Vector2 val4 = new Vector2(val3.Y, 0f - val3.X);
		int num3 = (int)Math.Max(num2 * 2f / stepSize, 1f);
		int num4 = 0;
		Vector2[] array2 = (calcPositions ? new Vector2[num3] : null);
		Bolt bolt = new Bolt
		{
			positions = array2,
			forkDepth = depth,
			progressRange = progressRange
		};
		int i;
		for (i = 0; i < num3; i++)
		{
			if (calcPositions)
			{
				array2[i] = val2;
			}
			Vector2 val5 = targetPos - val2;
			float num5 = Vector2.Dot(val5, val3);
			if (num5 < stepSize)
			{
				break;
			}
			float num6 = MathHelper.Clamp(1f - num5 / num2, 0f, 1f);
			if (SolidTileCollision && val2.ToTileCoordinates() != val && TileCollision(val2))
			{
				bolt.progressRange = new FloatRange(progressRange.Minimum, progressRange.Lerp(num6));
				bolt.collidedWithTile = true;
				break;
			}
			val5 /= val5.Length();
			float num7 = 0f - Vector2.Dot(val5, val4);
			float num8 = Math.Max(0.01f, Math.Min(num6, 1f - num6) * PerpendicularDeviationFactor * 2f);
			float num9 = MathHelper.Clamp(num7 / num8, -1f, 1f);
			if (PickLayerToReroll(lCG32Random.NextDouble(), 0.5f, out var layer))
			{
				float num10 = rotationStrength;
				for (int num11 = Layers - 1; num11 > layer; num11--)
				{
					num10 /= LayerStrengthFactor;
				}
				float num12 = (float)lCG32Random.NextDouble() * 2f - 1f;
				num12 += (num9 - num12 * Math.Abs(num9)) / 2f;
				float num13 = num12 * num10;
				float num14 = array[layer];
				float num15 = num13 - num14;
				num += num15;
				array[layer] = num13;
				if (layer == Layers - 1)
				{
					float num16 = lCG32Random.NextFloat();
					float num17 = Utils.Remap(num4, 0f, MaxForksPerBolt, 1f, 0f);
					float num18 = num - num15 * (1f + ForkReflectAngleMultiplier);
					if (bolts != null && Math.Abs(num15) >= rotationStrength * ForkGenerationThresholdAngleFraction && ForkProgressRange.Contains(num6) && depth < MaxForkDepth && num16 < num17 && Math.Abs(num18) < (float)Math.PI * 4f / 9f)
					{
						num4++;
						float num19 = (1f - num6) * ForkLengthMultiplier;
						Vector2 targetPos2 = val2 + val5.RotatedBy(num18) * num2 * num19;
						GenerateBolt(bolts, lCG32Random.state + 1, depth + 1, calcPositions, val2, targetPos2, rotationStrength * ForkRotationStrengthMultiplier, stepSize * ForkStepSizeMultiplier, new FloatRange(progressRange.Lerp(num6), progressRange.Lerp(num6 + num19)));
					}
				}
			}
			float num20 = Utils.Remap(num6, ReduceRandomnessAfter, 1f, 0f, 1f);
			num20 += Utils.Remap(Math.Abs(num9), 0.5f, 1f, 0f, 1f);
			if (PickHighLayerToReroll(lCG32Random.NextDouble(), num20, out layer))
			{
				num -= array[layer];
				array[layer] = 0f;
			}
			val2 += val5.RotatedBy(num) * stepSize;
		}
		if (calcPositions && i < num3)
		{
			Array.Resize(ref array2, i + 1);
			bolt.positions = array2;
		}
		if (bolts != null && (bolt.IsMainBolt || i > 2))
		{
			bolts.Add(bolt);
		}
		return bolt;
	}

	private bool TileCollision(Vector2 pos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		Point val = pos.ToTileCoordinates();
		if (!WorldGen.InWorld(val) || Main.tile[val.X, val.Y] == null)
		{
			return false;
		}
		if (WorldGen.SolidOrSlopedTile(val.X, val.Y))
		{
			return true;
		}
		int liquid = Main.tile[val.X, val.Y].liquid;
		if (liquid > 0 && (int)pos.Y % 16 > 16 * (255 - liquid) / 255)
		{
			return true;
		}
		return false;
	}

	private bool PickLayerToReroll(double r, float chance, out int layer)
	{
		for (layer = 0; layer < Layers; layer++)
		{
			if (r >= (double)(1f - chance))
			{
				return true;
			}
			r /= (double)chance;
		}
		return false;
	}

	private bool PickHighLayerToReroll(double r, float chance, out int layer)
	{
		if (!PickLayerToReroll(r, chance, out layer))
		{
			return false;
		}
		layer = Layers - 1 - layer;
		return true;
	}

	private static float[] CalcRotations(Vector2[] positions)
	{
		float[] array = new float[positions.Length];
		CalcRotations(positions, array);
		return array;
	}

	public static void CalcRotations(Vector2[] positions, float[] rotations)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (rotations.Length >= 2)
		{
			int num = 0;
			float num2 = (positions[0] - positions[1]).ToRotation();
			rotations[num++] = num2;
			while (num < rotations.Length - 1)
			{
				float num3 = (positions[num] - positions[num + 1]).ToRotation();
				rotations[num++] = num2 + MathHelper.WrapAngle(num3 - num2) / 2f;
				num2 = num3;
			}
			rotations[num] = num2;
			SmoothRotations(rotations);
		}
	}

	private static void SmoothRotations(float[] rotations)
	{
		float num = rotations[0];
		for (int i = 1; i < rotations.Length - 1; i++)
		{
			float num2 = rotations[i];
			float num3 = rotations[i + 1];
			rotations[i] = num2 + (MathHelper.WrapAngle(num - num2) + MathHelper.WrapAngle(num3 - num2)) / 2f;
			num = num2;
		}
	}

	public bool CanHitTarget(uint seed, Vector2 targetPosition, Vector2? direction = null)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return !Generate(null, seed, targetPosition, direction, calcPositions: false, calcRotations: false).collidedWithTile;
	}

	public Bolt GenerateMainBoltPath(uint seed, Vector2 targetPosition, Vector2? direction = null)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return Generate(null, seed, targetPosition, direction, calcPositions: true, calcRotations: false);
	}

	public Bolt Generate(List<Bolt> bolts, uint seed, Vector2 targetPosition, Vector2? direction = null, bool calcPositions = true, bool calcRotations = true)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return Generate(bolts, seed, targetPosition, direction ?? Vector2.UnitY, calcPositions, calcRotations);
	}

	public static LightningGenerator GetArcSurgeWeaponGenerator(Vector2 start, Vector2 end)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		float fromValue = Vector2.Distance(start, end);
		return new LightningGenerator
		{
			SourceRotationLimit = 0f,
			Length = 1f,
			RotationStrength = 0.7f,
			StepSize = 6,
			Layers = 5,
			LayerStrengthFactor = 1.2f,
			PerpendicularDeviationFactor = Utils.Remap(fromValue, 0f, 1000f, 5f, 1f),
			ReduceRandomnessAfter = 0.7f,
			ForkGenerationThresholdAngleFraction = Utils.Remap(fromValue, 0f, 1000f, 0.3f, 0.5f),
			ForkReflectAngleMultiplier = Utils.Remap(fromValue, 0f, 1000f, 0.6f, 0.2f),
			ForkRotationStrengthMultiplier = 0.9f,
			ForkStepSizeMultiplier = 0.8f,
			ForkLengthMultiplier = 0.7f,
			MaxForksPerBolt = 3,
			MaxForkDepth = 3,
			ForkProgressRange = new FloatRange(Utils.Remap(fromValue, 0f, 1000f, 0.1f, 0.5f), 0.8f),
			SolidTileCollision = true
		};
	}
}
