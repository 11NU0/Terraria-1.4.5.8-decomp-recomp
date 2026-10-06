using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Renderers;

public class FlameParticle : ABasicParticle
{
	public float FadeOutNormalizedTime = 1f;

	private float _timeTolive;

	private float _timeSinceSpawn;

	private int _indexOfPlayerWhoSpawnedThis;

	private int _packedShaderIndex;

	public override void FetchFromPool()
	{
		base.FetchFromPool();
		FadeOutNormalizedTime = 1f;
		_timeTolive = 0f;
		_timeSinceSpawn = 0f;
		_indexOfPlayerWhoSpawnedThis = 0;
		_packedShaderIndex = 0;
	}

	public override void SetBasicInfo(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		base.SetBasicInfo(textureAsset, frame, initialVelocity, initialLocalPosition);
		_origin = new Vector2((float)(_frame.Width / 2), (float)(_frame.Height - 2));
	}

	public void SetTypeInfo(float timeToLive, int indexOfPlayerWhoSpawnedIt, int packedShaderIndex)
	{
		_timeTolive = timeToLive;
		_indexOfPlayerWhoSpawnedThis = indexOfPlayerWhoSpawnedIt;
		_packedShaderIndex = packedShaderIndex;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		base.Update(ref settings);
		_timeSinceSpawn++;
		if (_timeSinceSpawn >= _timeTolive)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		Color color = new Color(120, 120, 120, 60) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true);
		Vector2 val = settings.AnchorPosition + LocalPosition;
		ulong seed = Main.TileFrameSeed ^ (((ulong)LocalPosition.X << 32) | (uint)LocalPosition.Y);
		Player player = Main.player[_indexOfPlayerWhoSpawnedThis];
		for (int i = 0; i < 4; i++)
		{
			DrawData drawData = new DrawData(position: val + new Vector2((float)Utils.RandomInt(ref seed, -2, 3), (float)Utils.RandomInt(ref seed, -2, 3)) * Scale, texture: _texture.Value, sourceRect: _frame, color: color, rotation: Rotation, origin: _origin, scale: Scale, effect: (SpriteEffects)0);
			drawData.shader = _packedShaderIndex;
			DrawData cdd = drawData;
			PlayerDrawHelper.SetShaderForData(player, 0, ref cdd);
			cdd.Draw(spritebatch);
		}
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}
