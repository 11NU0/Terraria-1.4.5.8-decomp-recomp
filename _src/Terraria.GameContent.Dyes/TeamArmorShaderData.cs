using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes;

public class TeamArmorShaderData : ArmorShaderData
{
	private static bool isInitialized;

	private static ArmorShaderData[] dustShaderData;

	public TeamArmorShaderData(Asset<Effect> shader, string passName)
		: base(shader, passName)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (!isInitialized)
		{
			isInitialized = true;
			dustShaderData = new ArmorShaderData[Main.teamColor.Length];
			for (int i = 1; i < Main.teamColor.Length; i++)
			{
				dustShaderData[i] = new ArmorShaderData(shader, passName).UseColor(Main.teamColor[i]);
			}
			dustShaderData[0] = new ArmorShaderData(shader, "Default");
		}
	}

	public override void Apply(Entity entity, DrawData? drawData)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		Player player = entity as Player;
		if (player == null && entity is Projectile { OwnedBySomeone: not false } projectile)
		{
			player = Main.player[projectile.owner];
		}
		if (player == null || player.team == 0 || Main.netMode == 0)
		{
			dustShaderData[0].Apply(player, drawData);
			return;
		}
		UseColor(Main.teamColor[player.team]);
		base.Apply(player, drawData);
	}

	public override ArmorShaderData GetSecondaryShader(Entity entity)
	{
		Player player = entity as Player;
		return dustShaderData[player.team];
	}
}
