using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Utilities;

namespace Terraria.Graphics.Renderers;

public class StormLightningParticle : IPooledParticle, IParticle
{
	public Color Color;

	public float Width;

	private List<LightningGenerator.Bolt> bolts = new List<LightningGenerator.Bolt>();

	public int AnchorToPlayerHand = -1;

	private float Intensity = 1f;

	private bool SteadyLight;

	private int _lifeTimeCounted;

	private int _lifeTimeTotal;

	private StormLightningDrawer.AnimParams _animParams;

	public bool ShouldBeRemovedFromRenderer { get; private set; }

	private LightningGenerator.Bolt MainBolt => bolts.Last();

	public Vector2 EndPosition
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return MainBolt.positions.Last();
		}
	}

	public bool IsRestingInPool { get; private set; }

	public void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void FetchFromPool()
	{
		AnchorToPlayerHand = -1;
		_lifeTimeCounted = 0;
		_lifeTimeTotal = 0;
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
		bolts.Clear();
	}

	public void Prepare(LightningGenerator generator, uint seed, Vector2 sourcePosition, Vector2 targetPosition, int lifeTimeTotal, Color color, float width, float intensity, StormLightningDrawer.AnimParams animParams, int anchorToPlayer, bool steadyLight)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		_Prepare(generator, seed, targetPosition, targetPosition - sourcePosition, lifeTimeTotal, color, width, intensity, animParams, anchorToPlayer, steadyLight);
	}

	public void Prepare(LightningGenerator generator, uint seed, Vector2 targetPosition, int lifeTimeTotal, Color color, float width, StormLightningDrawer.AnimParams animParams)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		_Prepare(generator, seed, targetPosition, null, lifeTimeTotal, color, width, 1f, animParams, null, steadyLight: false);
	}

	private void _Prepare(LightningGenerator generator, uint seed, Vector2 targetPosition, Vector2? fromDirection, int lifeTimeTotal, Color color, float width, float intensity, StormLightningDrawer.AnimParams animParams, int? anchorToPlayerHand, bool steadyLight)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		AnchorToPlayerHand = anchorToPlayerHand ?? (-1);
		Color = color;
		Width = width;
		Intensity = intensity;
		SteadyLight = steadyLight;
		_animParams = animParams;
		_lifeTimeTotal = lifeTimeTotal;
		LightningGenerator.Bolt mainBolt = generator.Generate(bolts, seed, targetPosition, fromDirection);
		EmitSpawnDust(seed, color, mainBolt);
	}

	private void EmitSpawnDust(uint seed, Color color, LightningGenerator.Bolt mainBolt)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		LCG32Random lCG32Random = new LCG32Random(seed);
		short type = 226;
		float num = 1f;
		float num2 = 0.4f;
		int num3 = 6;
		float num4 = 10f;
		bool flag = AnchorToPlayerHand >= 0;
		if (flag)
		{
			num4 = Utils.Remap(Vector2.Distance(mainBolt.positions.Last(), mainBolt.positions.First()), 100f, 500f, 2f, 5f);
			type = 278;
			num2 = 0.7f;
			color *= 1.2f;
			num = 1.2f;
			num3 = 2;
		}
		int maxValue = (int)Math.Ceiling((float)mainBolt.positions.Length / num4);
		for (int i = 5; i < mainBolt.positions.Length - 5; i++)
		{
			if (lCG32Random.Next(maxValue) == 0)
			{
				Vector2 position = mainBolt.positions[i];
				Vector2 velocity = Vector2.UnitY;
				if (mainBolt.rotations != null)
				{
					velocity = -mainBolt.rotations[i].ToRotationVector2();
				}
				Dust dust = Dust.NewDustPerfect(position, type);
				dust.HackFrame(278);
				dust.color = color;
				dust.velocity = velocity;
				dust.velocity *= (3f + lCG32Random.NextFloat() * 6.5f) * num;
				dust.fadeIn = 0f;
				dust.scale = num2 + lCG32Random.NextFloat() * 0.5f;
				dust.noGravity = true;
				if (SteadyLight)
				{
					dust.noLight = (dust.noLightEmittance = true);
				}
				dust.position -= dust.velocity * (float)num3;
				if (AnchorToPlayerHand >= 0 && lCG32Random.Next(mainBolt.positions.Length) >= i)
				{
					dust.customData = Main.player[AnchorToPlayerHand];
				}
				Dust dust2 = Dust.CloneDust(dust);
				dust2.velocity *= 0.5f;
				dust2.scale -= 0.3f;
				if (flag)
				{
					dust2.velocity = dust.velocity;
					dust2.scale = dust.scale * 0.66f;
					dust2.color = new Color(255, 255, 255, 0);
				}
			}
		}
	}

	public static Vector2 GetPlayerAnchorPos(Player player)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return player.RotatedRelativePoint(player.HandPosition ?? player.MountedCenter);
	}

	public void Update(ref ParticleRendererSettings settings)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		Vector2 val = MainBolt.positions.First();
		Vector2 movementVector = Vector2.Zero;
		if (AnchorToPlayerHand >= 0)
		{
			Player obj = Main.player[AnchorToPlayerHand];
			val = GetPlayerAnchorPos(obj);
			movementVector = obj.velocity;
			flag = false;
		}
		Color color = Color;
		float num = (float)_lifeTimeCounted / (float)_lifeTimeTotal;
		float num2 = Utils.Remap(num, 0f, 0.4f, 1f, 0f);
		if (SteadyLight)
		{
			Vector2[] positions = MainBolt.positions;
			Vector3 rgb = Color.ToVector3() * Utils.Remap(num, 0f, 0.3f, 0f, 1f) * Utils.Remap(num, 0.7f, 1f, 1f, 0f);
			for (int i = 0; i < positions.Length; i += 20)
			{
				Lighting.AddLight(positions[i], rgb);
			}
		}
		if (flag)
		{
			if (num < 0.3f)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.StormlightningWindup, new ParticleOrchestraSettings
				{
					PositionInWorld = val,
					MovementVector = movementVector,
					UniqueInfoPiece = (int)color.PackedValue
				});
			}
			if (num < 0.5f)
			{
				for (int j = 0; j < 3; j++)
				{
					if (Main.rand.Next(4) == 0 && !(Main.rand.NextFloat() > num2 * 0.13f))
					{
						Dust dust = Dust.NewDustDirect(val, 16, 16, 306, 0f, 0f, 0, new Color((int)color.R, (int)color.G, (int)color.B, 0));
						dust.velocity = new Vector2(0f, -4f).RotatedByRandom(1.5707963705062866) * (0.5f + 0.2f * Main.rand.NextFloatDirection());
						dust.scale = 1.8f;
						dust.fadeIn = 0f;
						dust.noGravity = Main.rand.Next(3) != 0;
						dust.noLight = (dust.noLightEmittance = true);
						Dust dust2 = Dust.CloneDust(dust);
						dust2.color = new Color(255, 255, 255, 0);
						dust2.scale = 1.3f;
					}
				}
				for (int k = -1; k <= 1; k += 2)
				{
					if (Main.rand.Next(4) == 0 && !(Main.rand.NextFloat() > num2 * 0.2f))
					{
						Dust dust3 = Dust.NewDustPerfect(val, 306, new Vector2(0f, -4f).RotatedBy((float)Math.PI / 4f * (float)k * 1f));
						dust3.color = new Color((int)color.R, (int)color.G, (int)color.B, 0);
						dust3.scale = 1.8f;
						dust3.fadeIn = 0f;
						dust3.noGravity = Main.rand.Next(3) != 0;
						dust3.noLight = (dust3.noLightEmittance = true);
						Dust dust4 = Dust.CloneDust(dust3);
						dust4.color = new Color(255, 255, 255, 0);
						dust4.scale = 1.3f;
					}
				}
				for (int l = 0; l < 2; l++)
				{
					if (Main.rand.Next(4) == 0 && !(Main.rand.NextFloat() > 0.2f))
					{
						Dust dust5 = Dust.NewDustPerfect(val, 226);
						dust5.HackFrame(278);
						dust5.color = color;
						dust5.customData = dust5.color;
						dust5.velocity *= 1f + Main.rand.NextFloat() * 2.5f;
						dust5.velocity += new Vector2(0f, -2f);
						dust5.fadeIn = 0f;
						dust5.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
						dust5.velocity.X *= 2f;
						dust5.velocity = Main.rand.NextVector2Circular(3f, 2f) + new Vector2(0f, -2f);
						dust5.noLight = (dust5.noLightEmittance = true);
						dust5.position -= dust5.velocity * 3f;
					}
				}
			}
		}
		if (++_lifeTimeCounted >= _lifeTimeTotal)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		_ = MainBolt;
		if (AnchorToPlayerHand >= 0)
		{
			Vector2 val = MainBolt.positions.First();
			Vector2 val2 = MainBolt.positions.Last();
			float num = Vector2.DistanceSquared(val, val2);
			Vector2 val3 = GetPlayerAnchorPos(Main.player[AnchorToPlayerHand]) - val;
			foreach (LightningGenerator.Bolt bolt in bolts)
			{
				for (int i = 0; i < bolt.positions.Length; i++)
				{
					float num2 = MathHelper.Clamp(1f - Vector2.DistanceSquared(bolt.positions[i], val) / num, 0f, 1f);
					num2 *= num2;
					ref Vector2 reference = ref bolt.positions[i];
					reference += num2 * val3;
				}
				LightningGenerator.CalcRotations(bolt.positions, bolt.rotations);
			}
		}
		StormLightningDrawer stormLightningDrawer = default;
		float progress = (float)_lifeTimeCounted / (float)_lifeTimeTotal;
		foreach (LightningGenerator.Bolt bolt2 in bolts)
		{
			float num3 = (bolt2.IsMainBolt ? 1f : (0.5f * (float)Math.Pow(0.8, bolt2.forkDepth - 1)));
			num3 *= Intensity;
			stormLightningDrawer.Draw(bolt2.positions, bolt2.rotations, Width, Color, progress, bolt2.IsMainBolt, bolt2.progressRange, num3, _animParams);
		}
	}
}
