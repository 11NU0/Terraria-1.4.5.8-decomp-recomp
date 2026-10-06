using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria;

public class Cloud
{
	public Vector2 position;

	public float scale;

	public float rotation;

	public float rSpeed;

	public float sSpeed;

	public bool active;

	public SpriteEffects spriteDir;

	public int type;

	public int width;

	public int height;

	public float Alpha;

	public bool kill;

	private static UnifiedRandom rand = new UnifiedRandom();

	public static Vector2? lastCameraCenter;

	public static void resetClouds()
	{
		if (!Main.dedServ)
		{
			Main.windSpeedCurrent = Main.windSpeedTarget;
			for (int i = 0; i < 200; i++)
			{
				Main.cloud[i].active = false;
			}
			for (int j = 0; j < Main.numClouds; j++)
			{
				addCloud();
				Main.cloud[j].Alpha = 1f;
			}
			for (int k = 0; k < 200; k++)
			{
				Main.cloud[k].Alpha = 1f;
			}
		}
	}

	public static void addCloud()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < 200; i++)
		{
			if (!Main.cloud[i].active)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return;
		}
		Main.cloud[num].kill = false;
		Main.cloud[num].rSpeed = 0f;
		Main.cloud[num].sSpeed = 0f;
		Main.cloud[num].scale = (float)rand.Next(70, 131) * 0.01f;
		Main.cloud[num].rotation = (float)rand.Next(-10, 11) * 0.01f;
		Main.cloud[num].width = (int)((float)TextureAssets.Cloud[Main.cloud[num].type].Width() * Main.cloud[num].scale);
		Main.cloud[num].height = (int)((float)TextureAssets.Cloud[Main.cloud[num].type].Height() * Main.cloud[num].scale);
		Main.cloud[num].Alpha = 0f;
		Main.cloud[num].spriteDir = (SpriteEffects)0;
		if (rand.Next(2) == 0)
		{
			Main.cloud[num].spriteDir = (SpriteEffects)1;
		}
		float num2 = Main.windSpeedCurrent;
		if (!Main.gameMenu)
		{
			num2 = Main.windSpeedCurrent - Main.player[Main.myPlayer].velocity.X * 0.1f;
		}
		int num3 = 0;
		int num4 = 0;
		if (num2 > 0f)
		{
			num3 -= 200;
		}
		if (num2 < 0f)
		{
			num4 += 200;
		}
		int num5 = 300;
		float x = rand.Next(num3 - num5, Main.screenWidth + num4 + num5);
		Main.cloud[num].Alpha = 0f;
		Main.cloud[num].position.Y = rand.Next((int)((float)(-Main.screenHeight) * 0.25f), (int)((float)Main.screenHeight * 0.15f));
		if (rand.Next(3) == 0)
		{
			Main.cloud[num].position.Y -= rand.Next((int)((float)Main.screenHeight * 0.1f));
		}
		Main.cloud[num].type = rand.Next(4);
		if ((Main.cloudAlpha > 0f && rand.Next(4) != 0) || (Main.cloudBGActive >= 1f && rand.Next(2) == 0))
		{
			Main.cloud[num].type = rand.Next(18, 22);
			if ((double)Main.cloud[num].scale >= 1.15)
			{
				Main.cloud[num].position.Y -= 150f;
			}
			if (Main.cloud[num].scale >= 1f)
			{
				Main.cloud[num].position.Y -= 150f;
			}
		}
		else if (Main.cloudBGActive <= 0f && Main.cloudAlpha == 0f && Main.cloud[num].scale < 1f && Main.cloud[num].position.Y < (float)(-Main.screenHeight) * 0.15f && (double)Main.numClouds <= 80.0)
		{
			Main.cloud[num].type = rand.Next(9, 14);
		}
		else if ((((double)Main.cloud[num].scale < 1.15 && Main.cloud[num].position.Y < (float)(-Main.screenHeight) * 0.3f) || ((double)Main.cloud[num].scale < 0.85 && Main.cloud[num].position.Y < (float)Main.screenHeight * 0.15f)) && ((double)Main.numClouds > 70.0 || Main.cloudBGActive >= 1f))
		{
			Main.cloud[num].type = rand.Next(4, 9);
		}
		else if (Main.cloud[num].position.Y > (float)(-Main.screenHeight) * 0.15f && rand.Next(2) == 0 && (double)Main.numClouds > 20.0)
		{
			Main.cloud[num].type = rand.Next(14, 18);
		}
		if (rand.Next((Main.dontStarveWorld || Main.tenthAnniversaryWorld) ? 25 : 150) == 0)
		{
			Main.cloud[num].type = RollRareCloud();
		}
		else if (Main.tenthAnniversaryWorld && rand.Next(3) == 0)
		{
			Main.cloud[num].type = RollRareCloud();
		}
		if ((double)Main.cloud[num].scale > 1.2)
		{
			Main.cloud[num].position.Y += 100f;
		}
		if ((double)Main.cloud[num].scale > 1.3)
		{
			Main.cloud[num].scale = 1.3f;
		}
		if ((double)Main.cloud[num].scale < 0.7)
		{
			Main.cloud[num].scale = 0.7f;
		}
		Main.cloud[num].active = true;
		Main.cloud[num].position.X = x;
		if (Main.cloud[num].position.X > (float)(Main.screenWidth + 400))
		{
			Main.cloud[num].Alpha = 1f;
		}
		if (Main.cloud[num].position.X + (float)TextureAssets.Cloud[Main.cloud[num].type].Width() * Main.cloud[num].scale < -400f)
		{
			Main.cloud[num].Alpha = 1f;
		}
		Rectangle val = new Rectangle((int)Main.cloud[num].position.X, (int)Main.cloud[num].position.Y, Main.cloud[num].width, Main.cloud[num].height);
		for (int j = 0; j < 200; j++)
		{
			if (num != j && Main.cloud[j].active)
			{
				Rectangle val2 = new Rectangle((int)Main.cloud[j].position.X, (int)Main.cloud[j].position.Y, Main.cloud[j].width, Main.cloud[j].height);
				if (val.Intersects(val2))
				{
					Main.cloud[num].active = false;
				}
			}
		}
	}

	private static int RollRareCloud()
	{
		int num = -1;
		bool flag = false;
		while (!flag)
		{
			num = ((!Main.tenthAnniversaryWorld) ? CloudID.Sets.RareClouds_Normal[rand.Next(CloudID.Sets.RareClouds_Normal.Count)] : CloudID.Sets.RareClouds_Celebration[rand.Next(CloudID.Sets.RareClouds_Celebration.Count)]);
			switch (num)
			{
			default:
				flag = true;
				break;
			case 31:
				flag = NPC.downedBoss3;
				break;
			case 36:
				flag = NPC.downedBoss2 && WorldGen.crimson;
				break;
			case 25:
			case 26:
				flag = NPC.downedBoss1;
				break;
			case 30:
			case 35:
			case 41:
				flag = Main.hardMode;
				break;
			case 28:
				if (rand.Next(10) == 0)
				{
					flag = true;
				}
				break;
			case 37:
			case 38:
			case 39:
			case 40:
				if (Main.dontStarveWorld || rand.Next(10) == 0)
				{
					flag = true;
				}
				break;
			}
		}
		return num;
	}

	public Color cloudColor(Color bgColor)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		float num = scale * Alpha;
		if (num > 1f)
		{
			num = 1f;
		}
		float num2 = (int)((float)(int)bgColor.R * num);
		float num3 = (int)((float)(int)bgColor.G * num);
		float num4 = (int)((float)(int)bgColor.B * num);
		float num5 = (int)((float)(int)bgColor.A * num);
		return new Color((int)(byte)num2, (int)(byte)num3, (int)(byte)num4, (int)(byte)num5);
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public static void UpdateClouds()
	{
		if (Main.netMode == 2)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < 200; i++)
		{
			if (Main.cloud[i].active)
			{
				Main.cloud[i].Update();
				if (!Main.cloud[i].kill)
				{
					num++;
				}
			}
		}
		for (int j = 0; j < 200; j++)
		{
			if (Main.cloud[j].active)
			{
				if (j > 1 && (!Main.cloud[j - 1].active || (double)Main.cloud[j - 1].scale > (double)Main.cloud[j].scale + 0.02))
				{
					Cloud cloud = (Cloud)Main.cloud[j - 1].Clone();
					Main.cloud[j - 1] = (Cloud)Main.cloud[j].Clone();
					Main.cloud[j] = cloud;
				}
				if (j < 199 && (!Main.cloud[j].active || (double)Main.cloud[j + 1].scale < (double)Main.cloud[j].scale - 0.02))
				{
					Cloud cloud2 = (Cloud)Main.cloud[j + 1].Clone();
					Main.cloud[j + 1] = (Cloud)Main.cloud[j].Clone();
					Main.cloud[j] = cloud2;
				}
			}
		}
		if (num < Main.numClouds)
		{
			addCloud();
		}
		else if (num > Main.numClouds)
		{
			int num2 = rand.Next(num);
			int num3 = 0;
			while (Main.cloud[num2].kill && num3 < 100)
			{
				num3++;
				num2 = rand.Next(num);
			}
			Main.cloud[num2].kill = true;
		}
	}

	public static void UpdateCloudParallax()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		Vector2 center = Main.Camera.Center;
		if (lastCameraCenter.HasValue)
		{
			num = center.X - lastCameraCenter.Value.X;
		}
		lastCameraCenter = center;
		if (num == 0f)
		{
			return;
		}
		for (int i = 0; i < 200; i++)
		{
			Cloud cloud = Main.cloud[i];
			if (cloud.active)
			{
				cloud.position.X -= num * cloud.GetParallax();
			}
		}
	}

	private float GetParallax()
	{
		float num;
		float num2;
		if (scale < 1f)
		{
			num = 0.07f;
			num2 = scale + 0.15f;
			num2 = (num2 + 1f) / 2f;
		}
		else if ((double)scale <= 1.15)
		{
			num = 0.19f;
			num2 = scale - 0.075f;
		}
		else
		{
			num = 0.23f;
			num2 = scale - 0.15f - 0.075f;
		}
		num2 *= num2;
		return num * num2;
	}

	public void Update()
	{
		if (WorldGen.drunkWorldGenText && Main.gameMenu)
		{
			if (Main.onlyShimmerOceanWorlds)
			{
				type = 29;
			}
			else if (WorldGen.dontStarveWorldGen)
			{
				if ((double)scale < 0.7)
				{
					type = 40;
				}
				else if ((double)scale < 0.8)
				{
					type = 39;
				}
				else if ((double)scale < 0.9)
				{
					type = 38;
				}
				else
				{
					type = 37;
				}
			}
			else if (WorldGen.getGoodWorldGen)
			{
				if ((double)scale < 0.8)
				{
					type = 25;
				}
				else
				{
					type = 26;
				}
			}
			else
			{
				type = 28;
			}
		}
		if (scale == 1f)
		{
			scale -= 0.0001f;
		}
		if ((double)scale == 1.15)
		{
			scale -= 0.0001f;
		}
		position.X += Main.windSpeedCurrent * 9f * GetParallax() * (float)Main.dayRate;
		float num = 600f;
		if (Main.bgAlphaFrontLayer[4] == 1f && position.Y > 200f)
		{
			kill = true;
			Alpha -= 0.005f * (float)Main.dayRate;
		}
		if (!kill)
		{
			if (Alpha < 1f)
			{
				Alpha += 0.001f * (float)Main.dayRate;
				if (Alpha > 1f)
				{
					Alpha = 1f;
				}
			}
		}
		else
		{
			Alpha -= 0.001f * (float)Main.dayRate;
			if (Alpha <= 0f)
			{
				active = false;
			}
		}
		if (position.X + (float)TextureAssets.Cloud[type].Width() * scale < 0f - num || position.X > (float)Main.screenWidth + num)
		{
			active = false;
		}
		rSpeed += (float)rand.Next(-10, 11) * 2E-05f;
		if ((double)rSpeed > 0.0002)
		{
			rSpeed = 0.0002f;
		}
		if ((double)rSpeed < -0.0002)
		{
			rSpeed = -0.0002f;
		}
		if ((double)rotation > 0.02)
		{
			rotation = 0.02f;
		}
		if ((double)rotation < -0.02)
		{
			rotation = -0.02f;
		}
		rotation += rSpeed;
		width = (int)((float)TextureAssets.Cloud[type].Width() * scale);
		height = (int)((float)TextureAssets.Cloud[type].Height() * scale);
		if (type >= 9 && type <= 13 && (Main.cloudAlpha > 0f || Main.cloudBGActive >= 1f))
		{
			kill = true;
		}
	}
}
