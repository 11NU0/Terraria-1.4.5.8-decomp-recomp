using System.Collections.Generic;

namespace Terraria.DataStructures;

public class MinionRespawner
{
	private List<MinionSpawnInfo> _minions = new List<MinionSpawnInfo>();

	public void Clear()
	{
		_minions.Clear();
	}

	public void CollectMinionsFor(Player player)
	{
		int whoAmI = player.whoAmI;
		Clear();
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.owner == whoAmI && projectile.MinionSpawnInfo != null)
			{
				_minions.Add(projectile.MinionSpawnInfo);
			}
		}
	}

	public void Replace(IEnumerable<MinionSpawnInfo> records)
	{
		Clear();
		_minions.AddRange(records);
	}

	public void RestoreMinionsFor(Player player)
	{
		int mouseX = Main.mouseX;
		int mouseY = Main.mouseY;
		Main.mouseX = Main.screenWidth / 2;
		Main.mouseY = Main.screenHeight / 2;
		foreach (MinionSpawnInfo minion in _minions)
		{
			minion.TryRespawn(player);
		}
		Main.mouseX = mouseX;
		Main.mouseY = mouseY;
		Clear();
	}
}
