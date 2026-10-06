using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria.ID;

namespace Terraria.Audio;

public class LegacySoundPlayer
{
	public Asset<SoundEffect>[] SoundDrip = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceDrip = new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundLiquid = new Asset<SoundEffect>[2];

	public SoundEffectInstance[] SoundInstanceLiquid = new SoundEffectInstance[2];

	public Asset<SoundEffect>[] SoundMech = new Asset<SoundEffect>[1];

	public SoundEffectInstance[] SoundInstanceMech = new SoundEffectInstance[1];

	public Asset<SoundEffect>[] SoundDig = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceDig = new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundThunder = new Asset<SoundEffect>[6];

	public SoundEffectInstance[] SoundInstanceThunder = new SoundEffectInstance[6];

	public Asset<SoundEffect>[] SoundResearch = new Asset<SoundEffect>[4];

	public SoundEffectInstance[] SoundInstanceResearch = new SoundEffectInstance[4];

	public Asset<SoundEffect>[] SoundTink = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceTink = new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundCoin = new Asset<SoundEffect>[5];

	public SoundEffectInstance[] SoundInstanceCoin = new SoundEffectInstance[5];

	public Asset<SoundEffect>[] SoundPlayerHit = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstancePlayerHit = new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundFemaleHit = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceFemaleHit = new SoundEffectInstance[3];

	public Asset<SoundEffect> SoundPlayerKilled;

	public SoundEffectInstance SoundInstancePlayerKilled;

	public Asset<SoundEffect> SoundGrass;

	public SoundEffectInstance SoundInstanceGrass;

	public Asset<SoundEffect> SoundGrab;

	public SoundEffectInstance SoundInstanceGrab;

	public Asset<SoundEffect> SoundPixie;

	public SoundEffectInstance SoundInstancePixie;

	public Asset<SoundEffect>[] SoundItem = new Asset<SoundEffect>[SoundID.ItemSoundCount];

	public SoundEffectInstance[] SoundInstanceItem = new SoundEffectInstance[SoundID.ItemSoundCount];

	public Asset<SoundEffect>[] SoundNpcHit = new Asset<SoundEffect>[59];

	public SoundEffectInstance[] SoundInstanceNpcHit = new SoundEffectInstance[59];

	public Asset<SoundEffect>[] SoundNpcKilled = new Asset<SoundEffect>[SoundID.NPCDeathCount];

	public SoundEffectInstance[] SoundInstanceNpcKilled = new SoundEffectInstance[SoundID.NPCDeathCount];

	public SoundEffectInstance SoundInstanceMoonlordCry;

	public Asset<SoundEffect> SoundDoorOpen;

	public SoundEffectInstance SoundInstanceDoorOpen;

	public Asset<SoundEffect> SoundDoorClosed;

	public SoundEffectInstance SoundInstanceDoorClosed;

	public Asset<SoundEffect> SoundMenuOpen;

	public SoundEffectInstance SoundInstanceMenuOpen;

	public Asset<SoundEffect> SoundMenuClose;

	public SoundEffectInstance SoundInstanceMenuClose;

	public Asset<SoundEffect> SoundMenuTick;

	public SoundEffectInstance SoundInstanceMenuTick;

	public Asset<SoundEffect> SoundShatter;

	public SoundEffectInstance SoundInstanceShatter;

	public Asset<SoundEffect> SoundCamera;

	public SoundEffectInstance SoundInstanceCamera;

	public Asset<SoundEffect>[] SoundZombie = new Asset<SoundEffect>[131];

	public SoundEffectInstance[] SoundInstanceZombie = new SoundEffectInstance[131];

	public Asset<SoundEffect>[] SoundRoar = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceRoar = new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundSplash = new Asset<SoundEffect>[6];

	public SoundEffectInstance[] SoundInstanceSplash = new SoundEffectInstance[6];

	public Asset<SoundEffect> SoundDoubleJump;

	public SoundEffectInstance SoundInstanceDoubleJump;

	public Asset<SoundEffect> SoundRun;

	public SoundEffectInstance SoundInstanceRun;

	public Asset<SoundEffect> SoundCoins;

	public SoundEffectInstance SoundInstanceCoins;

	public Asset<SoundEffect> SoundUnlock;

	public SoundEffectInstance SoundInstanceUnlock;

	public Asset<SoundEffect> SoundChat;

	public SoundEffectInstance SoundInstanceChat;

	public Asset<SoundEffect> SoundMaxMana;

	public SoundEffectInstance SoundInstanceMaxMana;

	public Asset<SoundEffect> SoundDrown;

	public SoundEffectInstance SoundInstanceDrown;

	public Asset<SoundEffect>[] TrackableSounds;

	public SoundEffectInstance[] TrackableSoundInstances;

	private readonly IServiceProvider _services;

	private List<SoundEffectInstance> _trackedInstances;

	public static readonly float SoundAttenuationDistance = 2500f;

	public LegacySoundPlayer(IServiceProvider services)
	{
		_services = services;
		_trackedInstances = new List<SoundEffectInstance>();
		LoadAll();
	}

	public void Reload()
	{
		CreateAllSoundInstances();
	}

	private void LoadAll()
	{
		SoundMech[0] = Load("Sounds/Mech_0");
		SoundGrab = Load("Sounds/Grab");
		SoundPixie = Load("Sounds/Pixie");
		SoundDig[0] = Load("Sounds/Dig_0");
		SoundDig[1] = Load("Sounds/Dig_1");
		SoundDig[2] = Load("Sounds/Dig_2");
		SoundThunder[0] = Load("Sounds/Thunder_0");
		SoundThunder[1] = Load("Sounds/Thunder_1");
		SoundThunder[2] = Load("Sounds/Thunder_2");
		SoundThunder[3] = Load("Sounds/Thunder_3");
		SoundThunder[4] = Load("Sounds/Thunder_4");
		SoundThunder[5] = Load("Sounds/Thunder_5");
		SoundResearch[0] = Load("Sounds/Research_0");
		SoundResearch[1] = Load("Sounds/Research_1");
		SoundResearch[2] = Load("Sounds/Research_2");
		SoundResearch[3] = Load("Sounds/Research_3");
		SoundTink[0] = Load("Sounds/Tink_0");
		SoundTink[1] = Load("Sounds/Tink_1");
		SoundTink[2] = Load("Sounds/Tink_2");
		SoundPlayerHit[0] = Load("Sounds/Player_Hit_0");
		SoundPlayerHit[1] = Load("Sounds/Player_Hit_1");
		SoundPlayerHit[2] = Load("Sounds/Player_Hit_2");
		SoundFemaleHit[0] = Load("Sounds/Female_Hit_0");
		SoundFemaleHit[1] = Load("Sounds/Female_Hit_1");
		SoundFemaleHit[2] = Load("Sounds/Female_Hit_2");
		SoundPlayerKilled = Load("Sounds/Player_Killed");
		SoundChat = Load("Sounds/Chat");
		SoundGrass = Load("Sounds/Grass");
		SoundDoorOpen = Load("Sounds/Door_Opened");
		SoundDoorClosed = Load("Sounds/Door_Closed");
		SoundMenuTick = Load("Sounds/Menu_Tick");
		SoundMenuOpen = Load("Sounds/Menu_Open");
		SoundMenuClose = Load("Sounds/Menu_Close");
		SoundShatter = Load("Sounds/Shatter");
		SoundCamera = Load("Sounds/Camera");
		for (int i = 0; i < SoundCoin.Length; i++)
		{
			SoundCoin[i] = Load("Sounds/Coin_" + i);
		}
		for (int j = 0; j < SoundDrip.Length; j++)
		{
			SoundDrip[j] = Load("Sounds/Drip_" + j);
		}
		for (int k = 0; k < SoundZombie.Length; k++)
		{
			SoundZombie[k] = Load("Sounds/Zombie_" + k);
		}
		for (int l = 0; l < SoundLiquid.Length; l++)
		{
			SoundLiquid[l] = Load("Sounds/Liquid_" + l);
		}
		for (int m = 0; m < SoundRoar.Length; m++)
		{
			SoundRoar[m] = Load("Sounds/Roar_" + m);
		}
		for (int n = 0; n < SoundSplash.Length; n++)
		{
			SoundSplash[n] = Load("Sounds/Splash_" + n);
		}
		SoundDoubleJump = Load("Sounds/Double_Jump");
		SoundRun = Load("Sounds/Run");
		SoundCoins = Load("Sounds/Coins");
		SoundUnlock = Load("Sounds/Unlock");
		SoundMaxMana = Load("Sounds/MaxMana");
		SoundDrown = Load("Sounds/Drown");
		for (int num = 1; num < SoundItem.Length; num++)
		{
			SoundItem[num] = Load("Sounds/Item_" + num);
		}
		for (int num2 = 1; num2 < SoundNpcHit.Length; num2++)
		{
			SoundNpcHit[num2] = Load("Sounds/NPC_Hit_" + num2);
		}
		for (int num3 = 1; num3 < SoundNpcKilled.Length; num3++)
		{
			SoundNpcKilled[num3] = Load("Sounds/NPC_Killed_" + num3);
		}
		TrackableSounds = new Asset<SoundEffect>[SoundID.TrackableLegacySoundCount];
		TrackableSoundInstances = new SoundEffectInstance[TrackableSounds.Length];
		for (int num4 = 0; num4 < TrackableSounds.Length; num4++)
		{
			TrackableSounds[num4] = Load("Sounds/Custom" + Path.DirectorySeparatorChar + SoundID.GetTrackableLegacySoundPath(num4));
		}
	}

	public void CreateAllSoundInstances()
	{
		foreach (SoundEffectInstance trackedInstance in _trackedInstances)
		{
			trackedInstance.Dispose();
		}
		_trackedInstances.Clear();
		SoundInstanceMech[0] = CreateInstance(SoundMech[0]);
		SoundInstanceGrab = CreateInstance(SoundGrab);
		SoundInstancePixie = CreateInstance(SoundGrab);
		SoundInstanceDig[0] = CreateInstance(SoundDig[0]);
		SoundInstanceDig[1] = CreateInstance(SoundDig[1]);
		SoundInstanceDig[2] = CreateInstance(SoundDig[2]);
		SoundInstanceTink[0] = CreateInstance(SoundTink[0]);
		SoundInstanceTink[1] = CreateInstance(SoundTink[1]);
		SoundInstanceTink[2] = CreateInstance(SoundTink[2]);
		SoundInstancePlayerHit[0] = CreateInstance(SoundPlayerHit[0]);
		SoundInstancePlayerHit[1] = CreateInstance(SoundPlayerHit[1]);
		SoundInstancePlayerHit[2] = CreateInstance(SoundPlayerHit[2]);
		SoundInstanceFemaleHit[0] = CreateInstance(SoundFemaleHit[0]);
		SoundInstanceFemaleHit[1] = CreateInstance(SoundFemaleHit[1]);
		SoundInstanceFemaleHit[2] = CreateInstance(SoundFemaleHit[2]);
		SoundInstancePlayerKilled = CreateInstance(SoundPlayerKilled);
		SoundInstanceChat = CreateInstance(SoundChat);
		SoundInstanceGrass = CreateInstance(SoundGrass);
		SoundInstanceDoorOpen = CreateInstance(SoundDoorOpen);
		SoundInstanceDoorClosed = CreateInstance(SoundDoorClosed);
		SoundInstanceMenuTick = CreateInstance(SoundMenuTick);
		SoundInstanceMenuOpen = CreateInstance(SoundMenuOpen);
		SoundInstanceMenuClose = CreateInstance(SoundMenuClose);
		SoundInstanceShatter = CreateInstance(SoundShatter);
		SoundInstanceCamera = CreateInstance(SoundCamera);
		SoundInstanceSplash[0] = CreateInstance(SoundRoar[0]);
		SoundInstanceSplash[1] = CreateInstance(SoundSplash[1]);
		SoundInstanceDoubleJump = CreateInstance(SoundRoar[0]);
		SoundInstanceRun = CreateInstance(SoundRun);
		SoundInstanceCoins = CreateInstance(SoundCoins);
		SoundInstanceUnlock = CreateInstance(SoundUnlock);
		SoundInstanceMaxMana = CreateInstance(SoundMaxMana);
		SoundInstanceDrown = CreateInstance(SoundDrown);
		SoundInstanceMoonlordCry = CreateInstance(SoundNpcKilled[10]);
		for (int i = 0; i < SoundThunder.Length; i++)
		{
			SoundInstanceThunder[i] = CreateInstance(SoundThunder[i]);
		}
		for (int j = 0; j < SoundResearch.Length; j++)
		{
			SoundInstanceResearch[j] = CreateInstance(SoundResearch[j]);
		}
		for (int k = 0; k < SoundCoin.Length; k++)
		{
			SoundInstanceCoin[k] = CreateInstance(SoundCoin[k]);
		}
		for (int l = 0; l < SoundDrip.Length; l++)
		{
			SoundInstanceDrip[l] = CreateInstance(SoundDrip[l]);
		}
		for (int m = 0; m < SoundZombie.Length; m++)
		{
			SoundInstanceZombie[m] = CreateInstance(SoundZombie[m]);
		}
		for (int n = 0; n < SoundLiquid.Length; n++)
		{
			SoundInstanceLiquid[n] = CreateInstance(SoundLiquid[n]);
		}
		for (int num = 0; num < SoundRoar.Length; num++)
		{
			SoundInstanceRoar[num] = CreateInstance(SoundRoar[num]);
		}
		for (int num2 = 1; num2 < SoundItem.Length; num2++)
		{
			SoundInstanceItem[num2] = CreateInstance(SoundItem[num2]);
		}
		for (int num3 = 1; num3 < SoundNpcHit.Length; num3++)
		{
			SoundInstanceNpcHit[num3] = CreateInstance(SoundNpcHit[num3]);
		}
		for (int num4 = 1; num4 < SoundNpcKilled.Length; num4++)
		{
			SoundInstanceNpcKilled[num4] = CreateInstance(SoundNpcKilled[num4]);
		}
		for (int num5 = 0; num5 < TrackableSounds.Length; num5++)
		{
			TrackableSoundInstances[num5] = CreateInstance(TrackableSounds[num5]);
		}
	}

	private SoundEffectInstance CreateInstance(Asset<SoundEffect> asset)
	{
		SoundEffectInstance val = asset.Value.CreateInstance();
		_trackedInstances.Add(val);
		return val;
	}

	private Asset<SoundEffect> Load(string assetName)
	{
		return XnaExtensions.Get<IAssetRepository>(_services).Request<SoundEffect>(assetName, (AssetRequestMode)2);
	}

	public SoundEffectInstance PlaySound(int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Invalid comparison between Unknown and I4
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fba: Invalid comparison between Unknown and I4
		//IL_1070: Unknown result type (might be due to invalid IL or missing references)
		//IL_1076: Invalid comparison between Unknown and I4
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a6: Invalid comparison between Unknown and I4
		//IL_1378: Unknown result type (might be due to invalid IL or missing references)
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1666: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1721: Unknown result type (might be due to invalid IL or missing references)
		//IL_1895: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1553: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_2060: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_208e: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_246e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2491: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b4: Unknown result type (might be due to invalid IL or missing references)
		int num = Style;
		try
		{
			if (Main.dedServ)
			{
				return null;
			}
			if (Main.soundVolume == 0f && (type < 30 || type > 35))
			{
				return null;
			}
			bool flag = false;
			float num2 = 1f;
			float num3 = 0f;
			if (x == -1 || y == -1)
			{
				flag = true;
			}
			else
			{
				if (WorldGen.isGeneratingOrLoadingWorld)
				{
					return null;
				}
				if (Main.netMode == 2)
				{
					return null;
				}
				Vector2 val = new Vector2((float)x, (float)y) - Main.Camera.Center;
				float num4 = val.Length();
				if (num4 < SoundAttenuationDistance)
				{
					flag = true;
					num3 = MathHelper.Clamp(val.X / ((float)Main.MaxWorldViewSize.X * 0.5f), -1f, 1f);
					num2 = 1f - num4 / SoundAttenuationDistance;
				}
			}
			if (num3 < -1f)
			{
				num3 = -1f;
			}
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			if (num2 <= 0f && (type < 34 || type > 35 || type > 39))
			{
				return null;
			}
			if (flag)
			{
				if (DoesSoundScaleWithAmbientVolume(type))
				{
					num2 *= Main.ambientVolume * (float)((!FocusHelper.QuietAmbientSounds) ? 1 : 0);
					if (Main.gameMenu)
					{
						num2 = 0f;
					}
				}
				else
				{
					num2 *= Main.soundVolume;
				}
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				if (num2 <= 0f && (type < 30 || type > 35) && type != 39)
				{
					return null;
				}
				SoundEffectInstance val2 = null;
				switch (type)
				{
				case 0:
				{
					int num13 = Main.rand.Next(3);
					if (SoundInstanceDig[num13] != null)
					{
						SoundInstanceDig[num13].Stop();
					}
					SoundInstanceDig[num13] = SoundDig[num13].Value.CreateInstance();
					SoundInstanceDig[num13].Volume = num2;
					SoundInstanceDig[num13].Pan = num3;
					SoundInstanceDig[num13].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceDig[num13];
					break;
				}
				case 43:
				{
					int num12 = Main.rand.Next(SoundThunder.Length);
					for (int j = 0; j < SoundThunder.Length; j++)
					{
						if (SoundInstanceThunder[num12] == null)
						{
							break;
						}
						if ((int)SoundInstanceThunder[num12].State != 0)
						{
							break;
						}
						num12 = Main.rand.Next(SoundThunder.Length);
					}
					if (SoundInstanceThunder[num12] != null)
					{
						SoundInstanceThunder[num12].Stop();
					}
					SoundInstanceThunder[num12] = SoundThunder[num12].Value.CreateInstance();
					SoundInstanceThunder[num12].Volume = num2;
					SoundInstanceThunder[num12].Pan = num3;
					SoundInstanceThunder[num12].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceThunder[num12];
					break;
				}
				case 63:
				{
					int num14 = Main.rand.Next(1, 4);
					if (SoundInstanceResearch[num14] != null)
					{
						SoundInstanceResearch[num14].Stop();
					}
					SoundInstanceResearch[num14] = SoundResearch[num14].Value.CreateInstance();
					SoundInstanceResearch[num14].Volume = num2;
					SoundInstanceResearch[num14].Pan = num3;
					val2 = SoundInstanceResearch[num14];
					break;
				}
				case 64:
					if (SoundInstanceResearch[0] != null)
					{
						SoundInstanceResearch[0].Stop();
					}
					SoundInstanceResearch[0] = SoundResearch[0].Value.CreateInstance();
					SoundInstanceResearch[0].Volume = num2;
					SoundInstanceResearch[0].Pan = num3;
					val2 = SoundInstanceResearch[0];
					break;
				case 1:
				{
					int num15 = Main.rand.Next(3);
					if (SoundInstancePlayerHit[num15] != null)
					{
						SoundInstancePlayerHit[num15].Stop();
					}
					SoundInstancePlayerHit[num15] = SoundPlayerHit[num15].Value.CreateInstance();
					SoundInstancePlayerHit[num15].Volume = num2;
					SoundInstancePlayerHit[num15].Pan = num3;
					val2 = SoundInstancePlayerHit[num15];
					break;
				}
				case 2:
					if (num == 176)
					{
						num2 *= 0.9f;
					}
					if (num == 129)
					{
						num2 *= 0.6f;
					}
					if (num == 123)
					{
						num2 *= 0.5f;
					}
					if (num == 124 || num == 125)
					{
						num2 *= 0.65f;
					}
					if (num == 116)
					{
						num2 *= 0.5f;
					}
					switch (num)
					{
					case 1:
					{
						int num11 = Main.rand.Next(3);
						if (num11 == 1)
						{
							num = 18;
						}
						if (num11 == 2)
						{
							num = 19;
						}
						break;
					}
					case 53:
					case 55:
						num2 *= 0.75f;
						if (num == 55)
						{
							num2 *= 0.75f;
						}
						if (SoundInstanceItem[num] != null && (int)SoundInstanceItem[num].State == 0)
						{
							return null;
						}
						break;
					case 37:
						num2 *= 0.5f;
						break;
					case 52:
						num2 *= 0.35f;
						break;
					case 157:
						num2 *= 0.7f;
						break;
					case 158:
						num2 *= 0.8f;
						break;
					case 210:
						num2 *= 0.35f;
						break;
					}
					switch (num)
					{
					case 159:
						if (SoundInstanceItem[num] != null && (int)SoundInstanceItem[num].State == 0)
						{
							return null;
						}
						num2 *= 0.75f;
						break;
					default:
						if (SoundInstanceItem[num] != null)
						{
							SoundInstanceItem[num].Stop();
						}
						break;
					case 9:
					case 10:
					case 24:
					case 26:
					case 34:
					case 43:
					case 103:
					case 156:
					case 162:
					case 205:
						break;
					}
					SoundInstanceItem[num] = SoundItem[num].Value.CreateInstance();
					SoundInstanceItem[num].Volume = num2;
					SoundInstanceItem[num].Pan = num3;
					switch (num)
					{
					case 53:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-20, -11) * 0.02f;
						break;
					case 55:
						SoundInstanceItem[num].Pitch = (float)(-Main.rand.Next(-20, -11)) * 0.02f;
						break;
					case 132:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-20, 21) * 0.001f;
						break;
					case 153:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-50, 51) * 0.003f;
						break;
					case 156:
					{
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-50, 51) * 0.002f;
						SoundEffectInstance obj2 = SoundInstanceItem[num];
						obj2.Volume *= 0.6f;
						break;
					}
					case 192:
						SoundInstanceItem[num].Pitch = Projectile.kiteSoundPitch;
						break;
					default:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-6, 7) * 0.01f;
						break;
					}
					if (num == 26 || num == 35 || num == 47)
					{
						SoundInstanceItem[num].Volume = num2 * 0.75f;
						SoundInstanceItem[num].Pitch = Main.musicPitch;
					}
					if (num == 169)
					{
						SoundEffectInstance obj3 = SoundInstanceItem[num];
						obj3.Pitch -= 0.8f;
					}
					val2 = SoundInstanceItem[num];
					break;
				case 3:
					if (num >= 20 && num <= 54)
					{
						num2 *= 0.5f;
					}
					if (num == 57 && SoundInstanceNpcHit[num] != null && (int)SoundInstanceNpcHit[num].State == 0)
					{
						return null;
					}
					if (num == 57)
					{
						num2 *= 0.6f;
					}
					if (num == 55 || num == 56)
					{
						num2 *= 0.5f;
					}
					if (SoundInstanceNpcHit[num] != null)
					{
						SoundInstanceNpcHit[num].Stop();
					}
					SoundInstanceNpcHit[num] = SoundNpcHit[num].Value.CreateInstance();
					SoundInstanceNpcHit[num].Volume = num2;
					SoundInstanceNpcHit[num].Pan = num3;
					SoundInstanceNpcHit[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceNpcHit[num];
					break;
				case 4:
					if (num >= 23 && num <= 57)
					{
						num2 *= 0.5f;
					}
					if (num == 61)
					{
						num2 *= 0.6f;
					}
					if (num == 62)
					{
						num2 *= 0.6f;
					}
					if (num == 10 && SoundInstanceNpcKilled[num] != null && (int)SoundInstanceNpcKilled[num].State == 0)
					{
						return null;
					}
					SoundInstanceNpcKilled[num] = SoundNpcKilled[num].Value.CreateInstance();
					SoundInstanceNpcKilled[num].Volume = num2;
					SoundInstanceNpcKilled[num].Pan = num3;
					SoundInstanceNpcKilled[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceNpcKilled[num];
					break;
				case 5:
					if (SoundInstancePlayerKilled != null)
					{
						SoundInstancePlayerKilled.Stop();
					}
					SoundInstancePlayerKilled = SoundPlayerKilled.Value.CreateInstance();
					SoundInstancePlayerKilled.Volume = num2;
					SoundInstancePlayerKilled.Pan = num3;
					val2 = SoundInstancePlayerKilled;
					break;
				case 6:
					if (SoundInstanceGrass != null)
					{
						SoundInstanceGrass.Stop();
					}
					SoundInstanceGrass = SoundGrass.Value.CreateInstance();
					SoundInstanceGrass.Volume = num2;
					SoundInstanceGrass.Pan = num3;
					SoundInstanceGrass.Pitch = (float)Main.rand.Next(-30, 31) * 0.01f;
					val2 = SoundInstanceGrass;
					break;
				case 7:
					if (SoundInstanceGrab != null)
					{
						SoundInstanceGrab.Stop();
					}
					SoundInstanceGrab = SoundGrab.Value.CreateInstance();
					SoundInstanceGrab.Volume = num2;
					SoundInstanceGrab.Pan = num3;
					SoundInstanceGrab.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceGrab;
					break;
				case 8:
					if (SoundInstanceDoorOpen != null)
					{
						SoundInstanceDoorOpen.Stop();
					}
					SoundInstanceDoorOpen = SoundDoorOpen.Value.CreateInstance();
					SoundInstanceDoorOpen.Volume = num2;
					SoundInstanceDoorOpen.Pan = num3;
					SoundInstanceDoorOpen.Pitch = (float)Main.rand.Next(-20, 21) * 0.01f;
					val2 = SoundInstanceDoorOpen;
					break;
				case 9:
					if (SoundInstanceDoorClosed != null)
					{
						SoundInstanceDoorClosed.Stop();
					}
					SoundInstanceDoorClosed = SoundDoorClosed.Value.CreateInstance();
					SoundInstanceDoorClosed.Volume = num2;
					SoundInstanceDoorClosed.Pan = num3;
					SoundInstanceDoorClosed.Pitch = (float)Main.rand.Next(-20, 21) * 0.01f;
					val2 = SoundInstanceDoorClosed;
					break;
				case 10:
					if (SoundInstanceMenuOpen != null)
					{
						SoundInstanceMenuOpen.Stop();
					}
					SoundInstanceMenuOpen = SoundMenuOpen.Value.CreateInstance();
					SoundInstanceMenuOpen.Volume = num2;
					SoundInstanceMenuOpen.Pan = num3;
					val2 = SoundInstanceMenuOpen;
					break;
				case 11:
					if (SoundInstanceMenuClose != null)
					{
						SoundInstanceMenuClose.Stop();
					}
					SoundInstanceMenuClose = SoundMenuClose.Value.CreateInstance();
					SoundInstanceMenuClose.Volume = num2;
					SoundInstanceMenuClose.Pan = num3;
					val2 = SoundInstanceMenuClose;
					break;
				case 12:
					if (FocusHelper.AllowInputProcessing)
					{
						if (SoundInstanceMenuTick != null)
						{
							SoundInstanceMenuTick.Stop();
						}
						SoundInstanceMenuTick = SoundMenuTick.Value.CreateInstance();
						SoundInstanceMenuTick.Volume = num2;
						SoundInstanceMenuTick.Pan = num3;
						val2 = SoundInstanceMenuTick;
					}
					break;
				case 13:
					if (SoundInstanceShatter != null)
					{
						SoundInstanceShatter.Stop();
					}
					SoundInstanceShatter = SoundShatter.Value.CreateInstance();
					SoundInstanceShatter.Volume = num2;
					SoundInstanceShatter.Pan = num3;
					val2 = SoundInstanceShatter;
					break;
				case 14:
					switch (Style)
					{
					case 542:
					{
						int num21 = 7;
						SoundInstanceZombie[num21] = SoundZombie[num21].Value.CreateInstance();
						SoundInstanceZombie[num21].Volume = num2 * 0.4f;
						SoundInstanceZombie[num21].Pan = num3;
						val2 = SoundInstanceZombie[num21];
						break;
					}
					case 489:
					case 586:
					{
						int num20 = Main.rand.Next(21, 24);
						SoundInstanceZombie[num20] = SoundZombie[num20].Value.CreateInstance();
						SoundInstanceZombie[num20].Volume = num2 * 0.4f;
						SoundInstanceZombie[num20].Pan = num3;
						val2 = SoundInstanceZombie[num20];
						break;
					}
					default:
					{
						int num19 = Main.rand.Next(3);
						SoundInstanceZombie[num19] = SoundZombie[num19].Value.CreateInstance();
						SoundInstanceZombie[num19].Volume = num2 * 0.4f;
						SoundInstanceZombie[num19].Pan = num3;
						val2 = SoundInstanceZombie[num19];
						break;
					}
					}
					break;
				case 15:
				{
					float num18 = 1f;
					if (num == 4)
					{
						num = 1;
						num18 = 0.25f;
					}
					if (SoundInstanceRoar[num] == null || (int)SoundInstanceRoar[num].State == 2)
					{
						SoundInstanceRoar[num] = SoundRoar[num].Value.CreateInstance();
						SoundInstanceRoar[num].Volume = num2 * num18;
						SoundInstanceRoar[num].Pan = num3;
						val2 = SoundInstanceRoar[num];
					}
					break;
				}
				case 16:
					if (SoundInstanceDoubleJump != null)
					{
						SoundInstanceDoubleJump.Stop();
					}
					SoundInstanceDoubleJump = SoundDoubleJump.Value.CreateInstance();
					SoundInstanceDoubleJump.Volume = num2;
					SoundInstanceDoubleJump.Pan = num3;
					SoundInstanceDoubleJump.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceDoubleJump;
					break;
				case 17:
					if (SoundInstanceRun != null)
					{
						SoundInstanceRun.Stop();
					}
					SoundInstanceRun = SoundRun.Value.CreateInstance();
					SoundInstanceRun.Volume = num2;
					SoundInstanceRun.Pan = num3;
					SoundInstanceRun.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceRun;
					break;
				case 18:
					SoundInstanceCoins = SoundCoins.Value.CreateInstance();
					SoundInstanceCoins.Volume = num2;
					SoundInstanceCoins.Pan = num3;
					val2 = SoundInstanceCoins;
					break;
				case 19:
					if (SoundInstanceSplash[num] != null && (int)SoundInstanceSplash[num].State != 2)
					{
						break;
					}
					SoundInstanceSplash[num] = SoundSplash[num].Value.CreateInstance();
					if (num == 2 || num == 3)
					{
						num2 *= 0.75f;
					}
					if (num == 4 || num == 5)
					{
						num2 *= 0.75f;
						SoundInstanceSplash[num].Pitch = (float)Main.rand.Next(-20, 1) * 0.01f;
					}
					else
					{
						SoundInstanceSplash[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					}
					SoundInstanceSplash[num].Volume = num2;
					SoundInstanceSplash[num].Pan = num3;
					switch (num)
					{
					case 4:
						if (SoundInstanceSplash[5] == null || (int)SoundInstanceSplash[5].State == 2)
						{
							val2 = SoundInstanceSplash[num];
						}
						break;
					case 5:
						if (SoundInstanceSplash[4] == null || (int)SoundInstanceSplash[4].State == 2)
						{
							val2 = SoundInstanceSplash[num];
						}
						break;
					default:
						val2 = SoundInstanceSplash[num];
						break;
					}
					break;
				case 20:
				{
					int num22 = Main.rand.Next(3);
					if (SoundInstanceFemaleHit[num22] != null)
					{
						SoundInstanceFemaleHit[num22].Stop();
					}
					SoundInstanceFemaleHit[num22] = SoundFemaleHit[num22].Value.CreateInstance();
					SoundInstanceFemaleHit[num22].Volume = num2;
					SoundInstanceFemaleHit[num22].Pan = num3;
					val2 = SoundInstanceFemaleHit[num22];
					break;
				}
				case 21:
				{
					int num17 = Main.rand.Next(3);
					if (SoundInstanceTink[num17] != null)
					{
						SoundInstanceTink[num17].Stop();
					}
					SoundInstanceTink[num17] = SoundTink[num17].Value.CreateInstance();
					SoundInstanceTink[num17].Volume = num2;
					SoundInstanceTink[num17].Pan = num3;
					val2 = SoundInstanceTink[num17];
					break;
				}
				case 22:
					if (SoundInstanceUnlock != null)
					{
						SoundInstanceUnlock.Stop();
					}
					SoundInstanceUnlock = SoundUnlock.Value.CreateInstance();
					SoundInstanceUnlock.Volume = num2;
					SoundInstanceUnlock.Pan = num3;
					val2 = SoundInstanceUnlock;
					break;
				case 23:
					if (SoundInstanceDrown != null)
					{
						SoundInstanceDrown.Stop();
					}
					SoundInstanceDrown = SoundDrown.Value.CreateInstance();
					SoundInstanceDrown.Volume = num2;
					SoundInstanceDrown.Pan = num3;
					val2 = SoundInstanceDrown;
					break;
				case 24:
					SoundInstanceChat = SoundChat.Value.CreateInstance();
					SoundInstanceChat.Volume = num2;
					SoundInstanceChat.Pan = num3;
					val2 = SoundInstanceChat;
					break;
				case 25:
					SoundInstanceMaxMana = SoundMaxMana.Value.CreateInstance();
					SoundInstanceMaxMana.Volume = num2;
					SoundInstanceMaxMana.Pan = num3;
					val2 = SoundInstanceMaxMana;
					break;
				case 26:
				{
					int num16 = Main.rand.Next(3, 5);
					SoundInstanceZombie[num16] = SoundZombie[num16].Value.CreateInstance();
					SoundInstanceZombie[num16].Volume = num2 * 0.9f;
					SoundInstanceZombie[num16].Pan = num3;
					SoundInstanceZombie[num16].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceZombie[num16];
					break;
				}
				case 27:
					if (SoundInstancePixie != null && (int)SoundInstancePixie.State == 0)
					{
						SoundInstancePixie.Volume = num2;
						SoundInstancePixie.Pan = num3;
						SoundInstancePixie.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						return null;
					}
					if (SoundInstancePixie != null)
					{
						SoundInstancePixie.Stop();
					}
					SoundInstancePixie = SoundPixie.Value.CreateInstance();
					SoundInstancePixie.Volume = num2;
					SoundInstancePixie.Pan = num3;
					SoundInstancePixie.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstancePixie;
					break;
				case 28:
					if (SoundInstanceMech[num] != null && (int)SoundInstanceMech[num].State == 0)
					{
						return null;
					}
					SoundInstanceMech[num] = SoundMech[num].Value.CreateInstance();
					SoundInstanceMech[num].Volume = num2;
					SoundInstanceMech[num].Pan = num3;
					SoundInstanceMech[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceMech[num];
					break;
				case 29:
					if (num >= 24 && num <= 87)
					{
						num2 *= 0.5f;
					}
					if (num >= 88 && num <= 91)
					{
						num2 *= 0.7f;
					}
					if (num >= 93 && num <= 99)
					{
						num2 *= 0.4f;
					}
					if (num == 92)
					{
						num2 *= 0.5f;
					}
					if (num == 103)
					{
						num2 *= 0.4f;
					}
					if (num == 104)
					{
						num2 *= 0.55f;
					}
					if (num == 100 || num == 101)
					{
						num2 *= 0.25f;
					}
					if (num == 102)
					{
						num2 *= 0.4f;
					}
					if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
					{
						return null;
					}
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num2;
					SoundInstanceZombie[num].Pan = num3;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceZombie[num];
					break;
				case 44:
					num = Main.rand.Next(106, 109);
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num2 * 0.2f;
					SoundInstanceZombie[num].Pan = num3;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
					val2 = SoundInstanceZombie[num];
					break;
				case 45:
					num = 109;
					if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
					{
						return null;
					}
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num2 * 0.3f;
					SoundInstanceZombie[num].Pan = num3;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceZombie[num];
					break;
				case 46:
					if (SoundInstanceZombie[110] != null && (int)SoundInstanceZombie[110].State == 0)
					{
						return null;
					}
					if (SoundInstanceZombie[111] != null && (int)SoundInstanceZombie[111].State == 0)
					{
						return null;
					}
					num = Main.rand.Next(110, 112);
					if (Main.rand.Next(300) == 0)
					{
						if (Main.rand.Next(3) != 0)
						{
							num = ((Main.rand.Next(2) != 0) ? 112 : 113);
						}
						else
						{
							num = 114;
						}
					}
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num2 * 0.9f;
					SoundInstanceZombie[num].Pan = num3;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					val2 = SoundInstanceZombie[num];
					break;
				default:
					switch (type)
					{
					case 45:
						num = 109;
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.2f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 30:
						num = Main.rand.Next(10, 12);
						if (Main.rand.Next(300) == 0)
						{
							num = 12;
							if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
							{
								return null;
							}
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.75f;
						SoundInstanceZombie[num].Pan = num3;
						if (num != 12)
						{
							SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
						}
						else
						{
							SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-40, 21) * 0.01f;
						}
						val2 = SoundInstanceZombie[num];
						break;
					case 31:
						num = 13;
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.35f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-40, 21) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 32:
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.15f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 26) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 67:
						num = Main.rand.Next(118, 121);
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.3f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-5, 6) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 68:
						num = Main.rand.Next(126, 129);
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.22f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-5, 6) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 69:
						num = Main.rand.Next(129, 131);
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.2f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-5, 6) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 66:
						num = Main.rand.Next(121, 124);
						if (SoundInstanceZombie[121] != null && (int)SoundInstanceZombie[121].State == 0)
						{
							return null;
						}
						if (SoundInstanceZombie[122] != null && (int)SoundInstanceZombie[122].State == 0)
						{
							return null;
						}
						if (SoundInstanceZombie[123] != null && (int)SoundInstanceZombie[123].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.45f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-15, 16) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 33:
						num = 15;
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num2 * 0.2f;
						SoundInstanceZombie[num].Pan = num3;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 31) * 0.01f;
						val2 = SoundInstanceZombie[num];
						break;
					case 47:
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					{
						num = 133 + type - 47;
						for (int i = 133; i <= 138; i++)
						{
							if (SoundInstanceItem[i] != null && (int)SoundInstanceItem[i].State == 0)
							{
								SoundInstanceItem[i].Stop();
							}
						}
						SoundInstanceItem[num] = SoundItem[num].Value.CreateInstance();
						SoundInstanceItem[num].Volume = num2 * 0.45f;
						SoundInstanceItem[num].Pan = num3;
						val2 = SoundInstanceItem[num];
						break;
					}
					default:
						if (type >= 53 && type <= 62)
						{
							num = 139 + type - 53;
							if (SoundInstanceItem[num] != null && (int)SoundInstanceItem[num].State == 0)
							{
								SoundInstanceItem[num].Stop();
							}
							SoundInstanceItem[num] = SoundItem[num].Value.CreateInstance();
							SoundInstanceItem[num].Volume = num2 * 0.7f;
							SoundInstanceItem[num].Pan = num3;
							val2 = SoundInstanceItem[num];
							break;
						}
						switch (type)
						{
						case 34:
						{
							float num9 = (float)num / 50f;
							if (num9 > 1f)
							{
								num9 = 1f;
							}
							num2 *= num9;
							num2 *= 0.2f;
							num2 *= 1f - Main.shimmerAlpha;
							if (num2 <= 0f || x == -1 || y == -1)
							{
								if (SoundInstanceLiquid[0] != null && (int)SoundInstanceLiquid[0].State == 0)
								{
									SoundInstanceLiquid[0].Stop();
								}
							}
							else if (SoundInstanceLiquid[0] != null && (int)SoundInstanceLiquid[0].State == 0)
							{
								SoundInstanceLiquid[0].Volume = num2;
								SoundInstanceLiquid[0].Pan = num3;
								SoundInstanceLiquid[0].Pitch = -0.2f;
							}
							else
							{
								SoundInstanceLiquid[0] = SoundLiquid[0].Value.CreateInstance();
								SoundInstanceLiquid[0].Volume = num2;
								SoundInstanceLiquid[0].Pan = num3;
								val2 = SoundInstanceLiquid[0];
							}
							break;
						}
						case 35:
						{
							float num7 = (float)num / 50f;
							if (num7 > 1f)
							{
								num7 = 1f;
							}
							num2 *= num7;
							num2 *= 0.65f;
							num2 *= 1f - Main.shimmerAlpha;
							if (num2 <= 0f || x == -1 || y == -1)
							{
								if (SoundInstanceLiquid[1] != null && (int)SoundInstanceLiquid[1].State == 0)
								{
									SoundInstanceLiquid[1].Stop();
								}
							}
							else if (SoundInstanceLiquid[1] != null && (int)SoundInstanceLiquid[1].State == 0)
							{
								SoundInstanceLiquid[1].Volume = num2;
								SoundInstanceLiquid[1].Pan = num3;
								SoundInstanceLiquid[1].Pitch = -0f;
							}
							else
							{
								SoundInstanceLiquid[1] = SoundLiquid[1].Value.CreateInstance();
								SoundInstanceLiquid[1].Volume = num2;
								SoundInstanceLiquid[1].Pan = num3;
								val2 = SoundInstanceLiquid[1];
							}
							break;
						}
						case 36:
						{
							int num8 = Style;
							if (Style == -1)
							{
								num8 = 0;
							}
							SoundInstanceRoar[num8] = SoundRoar[num8].Value.CreateInstance();
							SoundInstanceRoar[num8].Volume = num2;
							SoundInstanceRoar[num8].Pan = num3;
							if (Style == -1)
							{
								SoundEffectInstance obj = SoundInstanceRoar[num8];
								obj.Pitch += 0.6f;
							}
							val2 = SoundInstanceRoar[num8];
							break;
						}
						case 37:
						{
							int num6 = Main.rand.Next(57, 59);
							num2 = ((!Main.starGame) ? (num2 * ((float)Style * 0.05f)) : (num2 * 0.15f));
							SoundInstanceItem[num6] = SoundItem[num6].Value.CreateInstance();
							SoundInstanceItem[num6].Volume = num2;
							SoundInstanceItem[num6].Pan = num3;
							SoundInstanceItem[num6].Pitch = (float)Main.rand.Next(-40, 41) * 0.01f;
							val2 = SoundInstanceItem[num6];
							break;
						}
						case 38:
						{
							if (Main.starGame)
							{
								num2 *= 0.15f;
							}
							int num10 = Main.rand.Next(5);
							SoundInstanceCoin[num10] = SoundCoin[num10].Value.CreateInstance();
							SoundInstanceCoin[num10].Volume = num2;
							SoundInstanceCoin[num10].Pan = num3;
							SoundInstanceCoin[num10].Pitch = (float)Main.rand.Next(-40, 41) * 0.002f;
							val2 = SoundInstanceCoin[num10];
							break;
						}
						case 39:
							num = Style;
							SoundInstanceDrip[num] = SoundDrip[num].Value.CreateInstance();
							SoundInstanceDrip[num].Volume = num2 * 0.5f;
							SoundInstanceDrip[num].Pan = num3;
							SoundInstanceDrip[num].Pitch = (float)Main.rand.Next(-30, 31) * 0.01f;
							val2 = SoundInstanceDrip[num];
							break;
						case 40:
							if (SoundInstanceCamera != null)
							{
								SoundInstanceCamera.Stop();
							}
							SoundInstanceCamera = SoundCamera.Value.CreateInstance();
							SoundInstanceCamera.Volume = num2;
							SoundInstanceCamera.Pan = num3;
							val2 = SoundInstanceCamera;
							break;
						case 41:
						{
							SoundInstanceMoonlordCry = SoundNpcKilled[10].Value.CreateInstance();
							SoundEffectInstance soundInstanceMoonlordCry = SoundInstanceMoonlordCry;
							Vector2 val3 = new Vector2((float)x, (float)y) - Main.player[Main.myPlayer].position;
							soundInstanceMoonlordCry.Volume = 1f / (1f + val3.Length());
							SoundInstanceMoonlordCry.Pan = num3;
							SoundInstanceMoonlordCry.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
							val2 = SoundInstanceMoonlordCry;
							break;
						}
						case 42:
							val2 = TrackableSounds[num].Value.CreateInstance();
							val2.Volume = num2;
							val2.Pan = num3;
							TrackableSoundInstances[num] = val2;
							break;
						case 65:
						{
							if (SoundInstanceZombie[115] != null && (int)SoundInstanceZombie[115].State == 0)
							{
								return null;
							}
							if (SoundInstanceZombie[116] != null && (int)SoundInstanceZombie[116].State == 0)
							{
								return null;
							}
							if (SoundInstanceZombie[117] != null && (int)SoundInstanceZombie[117].State == 0)
							{
								return null;
							}
							int num5 = Main.rand.Next(115, 118);
							SoundInstanceZombie[num5] = SoundZombie[num5].Value.CreateInstance();
							SoundInstanceZombie[num5].Volume = num2 * 0.5f;
							SoundInstanceZombie[num5].Pan = num3;
							val2 = SoundInstanceZombie[num5];
							break;
						}
						}
						break;
					}
					break;
				}
				if (val2 != null)
				{
					val2.Pitch = MathHelper.Clamp(val2.Pitch + pitchOffset, -1f, 1f);
					SoundEffectInstance val4 = val2;
					val4.Volume *= volumeScale;
					val2.Play();
					SoundInstanceGarbageCollector.Track(val2);
				}
				return val2;
			}
		}
		catch
		{
		}
		return null;
	}

	public SoundEffect GetTrackableSoundByStyleId(int id)
	{
		return TrackableSounds[id].Value;
	}

	public void StopAmbientSounds()
	{
		for (int i = 0; i < SoundInstanceLiquid.Length; i++)
		{
			if (SoundInstanceLiquid[i] != null)
			{
				SoundInstanceLiquid[i].Stop();
			}
		}
	}

	public bool DoesSoundScaleWithAmbientVolume(int soundType)
	{
		switch (soundType)
		{
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 39:
		case 43:
		case 44:
		case 45:
		case 46:
		case 67:
		case 68:
		case 69:
			return true;
		default:
			return false;
		}
	}
}
