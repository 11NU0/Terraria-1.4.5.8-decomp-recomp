using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Terraria.Graphics.Renderers;

public class FadingPlayerShaderParticle : FadingParticle
{
	private Player _player;

	private int _shader;

	public override void FetchFromPool()
	{
		base.FetchFromPool();
		_player = null;
		_shader = 0;
	}

	public void SetTypeInfo(float timeToLive, Player player, int shader, bool fullbright = true)
	{
		SetTypeInfo(timeToLive, fullbright);
		_player = player;
		_shader = shader;
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		if (_player == null || _shader == 0)
		{
			base.Draw(ref settings, spritebatch);
			return;
		}
		Effect pixelShader = Main.pixelShader;
		Color val = (fullbright ? ColorTint : ColorTint.MultiplyRGB(Lighting.GetColor(LocalPosition.ToTileCoordinates()))) * Utils.GetLerpValue(0f, FadeInNormalizedTime, timeSinceSpawn / timeTolive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, timeSinceSpawn / timeTolive, clamped: true);
		DrawData cdd = new DrawData
		{
			texture = _texture.Value,
			sourceRect = _texture.Frame(),
			shader = _shader
		};
		PlayerDrawHelper.SetShaderForData(_player, _shader, ref cdd);
		spritebatch.Draw(_texture.Value, settings.AnchorPosition + LocalPosition, (Rectangle?)_frame, val, Rotation, _origin, Scale, (SpriteEffects)0, 0f);
		pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}
