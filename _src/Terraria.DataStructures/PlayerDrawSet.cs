using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Golf;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.DataStructures;

public struct PlayerDrawSet
{
	public List<DrawData> DrawDataCache;

	public List<int> DustCache;

	public List<int> GoreCache;

	public Player drawPlayer;

	public float shadow;

	public Vector2 Position;

	public Vector2 VisualPositionOffset;

	public int projectileDrawPosition;

	public Vector2 ItemLocation;

	public int armorAdjust;

	public bool missingHand;

	public bool missingArm;

	public int skinVar;

	public bool fullHair;

	public bool drawsBackHairWithoutHeadgear;

	public bool hatHair;

	public bool hideHair;

	public int hairDyePacked;

	public int skinDyePacked;

	public float mountOffSet;

	public int cHead;

	public int cBody;

	public int cLegs;

	public int cHandOn;

	public int cHandOff;

	public int cBack;

	public int cFront;

	public int cShoe;

	public int cFlameWaker;

	public int cWaist;

	public int cShield;

	public int cNeck;

	public int cFace;

	public int cBalloon;

	public int cWings;

	public int cCarpet;

	public int cPortableStool;

	public int cFloatingTube;

	public int cUnicornHorn;

	public int cAngelHalo;

	public int cBeard;

	public int cLeinShampoo;

	public int cBackpack;

	public int cTail;

	public int cFaceHead;

	public int cFaceFlower;

	public int cFaceMask;

	public int cBalloonFront;

	public int cCoat;

	public SpriteEffects playerEffect;

	public SpriteEffects itemEffect;

	public Color colorHair;

	public Color colorEyeWhites;

	public Color colorEyes;

	public Color colorHead;

	public Color colorBodySkin;

	public Color colorLegs;

	public Color colorShirt;

	public Color colorUnderShirt;

	public Color colorPants;

	public Color colorShoes;

	public Color colorArmorHead;

	public Color colorArmorBody;

	public Color colorMount;

	public Color colorArmorLegs;

	public Color colorElectricity;

	public Color colorDisplayDollSkin;

	public int headGlowMask;

	public int bodyGlowMask;

	public int armGlowMask;

	public int legsGlowMask;

	public Color headGlowColor;

	public Color bodyGlowColor;

	public Color armGlowColor;

	public Color legsGlowColor;

	public Color ArkhalisColor;

	public float stealth;

	public Vector2 legVect;

	public Vector2 bodyVect;

	public Vector2 headVect;

	public Color selectionGlowColor;

	public float torsoOffset;

	public bool hidesTopSkin;

	public bool hidesBottomSkin;

	public float rotation;

	public Vector2 rotationOrigin;

	public Rectangle hairFrontFrame;

	public Rectangle hairBackFrame;

	public bool backHairDraw;

	public Color itemColor;

	public bool usesCompositeTorso;

	public bool usesCompositeFrontHandAcc;

	public bool usesCompositeBackHandAcc;

	public bool compShoulderOverFrontArm;

	public Rectangle compBackShoulderFrame;

	public Rectangle compFrontShoulderFrame;

	public Rectangle compBackArmFrame;

	public Rectangle compFrontArmFrame;

	public Rectangle compTorsoFrame;

	public float compositeBackArmRotation;

	public float compositeFrontArmRotation;

	public bool hideCompositeShoulders;

	public Vector2 frontShoulderOffset;

	public Vector2 backShoulderOffset;

	public WeaponDrawOrder weaponDrawOrder;

	public bool weaponOverFrontArm;

	public bool isSitting;

	public bool isSleeping;

	public float seatYOffset;

	public int sittingIndex;

	public bool drawFrontAccInNeckAccLayer;

	public bool drawFrontAccInNeckAccLayerAlways;

	public bool mountHandlesHeadDraw;

	public bool mountDrawsEyelid;

	public Item heldItem;

	public bool drawFloatingTube;

	public bool drawUnicornHorn;

	public bool drawAngelHalo;

	public Color floatingTubeColor;

	public Vector2 hairOffset;

	public Vector2 helmetOffset;

	public Vector2 legsOffset;

	public bool hideEntirePlayer;

	public bool hideEntirePlayerExceptHelmetsAndFaceAccessories;

	public Projectile SelectedDrawnProjectile;

	public Vector2 Center
	{
		get
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(Position.X + (float)(drawPlayer.width / 2), Position.Y + (float)(drawPlayer.height / 2));
		}
	}

	public void BoringSetup(Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin, Projectile overrideHeldProjectile)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1040: Unknown result type (might be due to invalid IL or missing references)
		//IL_104b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1050: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1126: Unknown result type (might be due to invalid IL or missing references)
		//IL_112b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1136: Unknown result type (might be due to invalid IL or missing references)
		//IL_113b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1195: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1210: Unknown result type (might be due to invalid IL or missing references)
		//IL_1215: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Unknown result type (might be due to invalid IL or missing references)
		//IL_1225: Unknown result type (might be due to invalid IL or missing references)
		//IL_127f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1284: Unknown result type (might be due to invalid IL or missing references)
		//IL_128f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1294: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1302: Unknown result type (might be due to invalid IL or missing references)
		//IL_1308: Unknown result type (might be due to invalid IL or missing references)
		//IL_130d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1443: Unknown result type (might be due to invalid IL or missing references)
		//IL_1448: Unknown result type (might be due to invalid IL or missing references)
		//IL_147b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1480: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_151f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1524: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_155d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1562: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2087: Unknown result type (might be due to invalid IL or missing references)
		//IL_208e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2093: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20de: Unknown result type (might be due to invalid IL or missing references)
		//IL_1632: Unknown result type (might be due to invalid IL or missing references)
		//IL_1637: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_165f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1664: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_169d: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_16df: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1706: Unknown result type (might be due to invalid IL or missing references)
		//IL_170b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1746: Unknown result type (might be due to invalid IL or missing references)
		//IL_1751: Unknown result type (might be due to invalid IL or missing references)
		//IL_1756: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_178b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1790: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Unknown result type (might be due to invalid IL or missing references)
		//IL_1808: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_200b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2010: Unknown result type (might be due to invalid IL or missing references)
		//IL_183d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1842: Unknown result type (might be due to invalid IL or missing references)
		//IL_2046: Unknown result type (might be due to invalid IL or missing references)
		//IL_204b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2062: Unknown result type (might be due to invalid IL or missing references)
		//IL_206d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2072: Unknown result type (might be due to invalid IL or missing references)
		//IL_188f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1896: Unknown result type (might be due to invalid IL or missing references)
		//IL_189b: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1903: Unknown result type (might be due to invalid IL or missing references)
		//IL_1908: Unknown result type (might be due to invalid IL or missing references)
		//IL_1941: Unknown result type (might be due to invalid IL or missing references)
		//IL_1946: Unknown result type (might be due to invalid IL or missing references)
		//IL_196d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1972: Unknown result type (might be due to invalid IL or missing references)
		//IL_259f: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_25aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_25af: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25db: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2603: Unknown result type (might be due to invalid IL or missing references)
		//IL_2609: Unknown result type (might be due to invalid IL or missing references)
		//IL_260e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2614: Unknown result type (might be due to invalid IL or missing references)
		//IL_2619: Unknown result type (might be due to invalid IL or missing references)
		//IL_261f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2624: Unknown result type (might be due to invalid IL or missing references)
		//IL_262a: Unknown result type (might be due to invalid IL or missing references)
		//IL_262f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2635: Unknown result type (might be due to invalid IL or missing references)
		//IL_263a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2640: Unknown result type (might be due to invalid IL or missing references)
		//IL_2645: Unknown result type (might be due to invalid IL or missing references)
		//IL_264b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2650: Unknown result type (might be due to invalid IL or missing references)
		//IL_2656: Unknown result type (might be due to invalid IL or missing references)
		//IL_265b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_2afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_29cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_29da: Unknown result type (might be due to invalid IL or missing references)
		//IL_29df: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_29fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2def: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e18: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3460: Unknown result type (might be due to invalid IL or missing references)
		//IL_314a: Unknown result type (might be due to invalid IL or missing references)
		//IL_319a: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_303a: Unknown result type (might be due to invalid IL or missing references)
		//IL_308a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3090: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3239: Unknown result type (might be due to invalid IL or missing references)
		//IL_323e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3486: Unknown result type (might be due to invalid IL or missing references)
		//IL_348b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3355: Unknown result type (might be due to invalid IL or missing references)
		//IL_3384: Unknown result type (might be due to invalid IL or missing references)
		//IL_338a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3270: Unknown result type (might be due to invalid IL or missing references)
		//IL_329f: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_35da: Unknown result type (might be due to invalid IL or missing references)
		//IL_35df: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3636: Unknown result type (might be due to invalid IL or missing references)
		//IL_363b: Unknown result type (might be due to invalid IL or missing references)
		//IL_364d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3676: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_33ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3411: Unknown result type (might be due to invalid IL or missing references)
		//IL_341c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3421: Unknown result type (might be due to invalid IL or missing references)
		//IL_3426: Unknown result type (might be due to invalid IL or missing references)
		//IL_32f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3302: Unknown result type (might be due to invalid IL or missing references)
		//IL_3307: Unknown result type (might be due to invalid IL or missing references)
		//IL_3324: Unknown result type (might be due to invalid IL or missing references)
		//IL_332f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3334: Unknown result type (might be due to invalid IL or missing references)
		//IL_3339: Unknown result type (might be due to invalid IL or missing references)
		//IL_3880: Unknown result type (might be due to invalid IL or missing references)
		//IL_38b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_38b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_38c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_38d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_38d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3901: Unknown result type (might be due to invalid IL or missing references)
		//IL_390b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3910: Unknown result type (might be due to invalid IL or missing references)
		//IL_3758: Unknown result type (might be due to invalid IL or missing references)
		//IL_375d: Unknown result type (might be due to invalid IL or missing references)
		//IL_376f: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_37de: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3539: Unknown result type (might be due to invalid IL or missing references)
		//IL_3543: Unknown result type (might be due to invalid IL or missing references)
		//IL_3548: Unknown result type (might be due to invalid IL or missing references)
		//IL_3565: Unknown result type (might be due to invalid IL or missing references)
		//IL_3570: Unknown result type (might be due to invalid IL or missing references)
		//IL_3575: Unknown result type (might be due to invalid IL or missing references)
		//IL_357a: Unknown result type (might be due to invalid IL or missing references)
		//IL_396b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3970: Unknown result type (might be due to invalid IL or missing references)
		//IL_3982: Unknown result type (might be due to invalid IL or missing references)
		//IL_3987: Unknown result type (might be due to invalid IL or missing references)
		//IL_398c: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_39e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_3be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_417f: Unknown result type (might be due to invalid IL or missing references)
		//IL_419c: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_41db: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4231: Unknown result type (might be due to invalid IL or missing references)
		//IL_423b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4240: Unknown result type (might be due to invalid IL or missing references)
		//IL_42db: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_430a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4319: Unknown result type (might be due to invalid IL or missing references)
		//IL_4323: Unknown result type (might be due to invalid IL or missing references)
		//IL_4328: Unknown result type (might be due to invalid IL or missing references)
		//IL_432b: Unknown result type (might be due to invalid IL or missing references)
		//IL_434b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4352: Unknown result type (might be due to invalid IL or missing references)
		//IL_4363: Unknown result type (might be due to invalid IL or missing references)
		//IL_4369: Unknown result type (might be due to invalid IL or missing references)
		//IL_4380: Unknown result type (might be due to invalid IL or missing references)
		//IL_438a: Unknown result type (might be due to invalid IL or missing references)
		//IL_438f: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_442e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4459: Unknown result type (might be due to invalid IL or missing references)
		//IL_445f: Unknown result type (might be due to invalid IL or missing references)
		//IL_449a: Unknown result type (might be due to invalid IL or missing references)
		//IL_44a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_44a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_450a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4523: Unknown result type (might be due to invalid IL or missing references)
		//IL_452e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4533: Unknown result type (might be due to invalid IL or missing references)
		//IL_4538: Unknown result type (might be due to invalid IL or missing references)
		//IL_453d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4572: Unknown result type (might be due to invalid IL or missing references)
		//IL_457c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4581: Unknown result type (might be due to invalid IL or missing references)
		//IL_44cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_44d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4971: Unknown result type (might be due to invalid IL or missing references)
		//IL_497c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4981: Unknown result type (might be due to invalid IL or missing references)
		//IL_4993: Unknown result type (might be due to invalid IL or missing references)
		//IL_499e: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_49c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_49c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_49d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_49e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_49e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_49fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a60: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_4af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_484c: Unknown result type (might be due to invalid IL or missing references)
		//IL_487e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4884: Unknown result type (might be due to invalid IL or missing references)
		//IL_4895: Unknown result type (might be due to invalid IL or missing references)
		//IL_489f: Unknown result type (might be due to invalid IL or missing references)
		//IL_48a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_48c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_48cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_48d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_48dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e31: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e91: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_46db: Unknown result type (might be due to invalid IL or missing references)
		//IL_4706: Unknown result type (might be due to invalid IL or missing references)
		//IL_470c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4724: Unknown result type (might be due to invalid IL or missing references)
		//IL_4729: Unknown result type (might be due to invalid IL or missing references)
		//IL_4736: Unknown result type (might be due to invalid IL or missing references)
		//IL_473b: Unknown result type (might be due to invalid IL or missing references)
		//IL_474b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4750: Unknown result type (might be due to invalid IL or missing references)
		//IL_4764: Unknown result type (might be due to invalid IL or missing references)
		//IL_476b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4770: Unknown result type (might be due to invalid IL or missing references)
		//IL_4775: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ffd: Unknown result type (might be due to invalid IL or missing references)
		//IL_5009: Unknown result type (might be due to invalid IL or missing references)
		//IL_5010: Unknown result type (might be due to invalid IL or missing references)
		//IL_5015: Unknown result type (might be due to invalid IL or missing references)
		//IL_5021: Unknown result type (might be due to invalid IL or missing references)
		//IL_5028: Unknown result type (might be due to invalid IL or missing references)
		//IL_502d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5039: Unknown result type (might be due to invalid IL or missing references)
		//IL_5044: Unknown result type (might be due to invalid IL or missing references)
		//IL_5049: Unknown result type (might be due to invalid IL or missing references)
		//IL_5055: Unknown result type (might be due to invalid IL or missing references)
		//IL_5060: Unknown result type (might be due to invalid IL or missing references)
		//IL_5065: Unknown result type (might be due to invalid IL or missing references)
		//IL_5071: Unknown result type (might be due to invalid IL or missing references)
		//IL_507c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5081: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5094: Unknown result type (might be due to invalid IL or missing references)
		//IL_509b: Unknown result type (might be due to invalid IL or missing references)
		//IL_50a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5891: Unknown result type (might be due to invalid IL or missing references)
		//IL_5896: Unknown result type (might be due to invalid IL or missing references)
		//IL_5899: Unknown result type (might be due to invalid IL or missing references)
		//IL_589b: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_58b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_58bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_58cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_58d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_58db: Unknown result type (might be due to invalid IL or missing references)
		//IL_58e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_58eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_58fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5901: Unknown result type (might be due to invalid IL or missing references)
		//IL_5903: Unknown result type (might be due to invalid IL or missing references)
		//IL_5181: Unknown result type (might be due to invalid IL or missing references)
		//IL_5186: Unknown result type (might be due to invalid IL or missing references)
		//IL_51cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_51d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5217: Unknown result type (might be due to invalid IL or missing references)
		//IL_521c: Unknown result type (might be due to invalid IL or missing references)
		//IL_522a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5231: Unknown result type (might be due to invalid IL or missing references)
		//IL_5236: Unknown result type (might be due to invalid IL or missing references)
		//IL_523d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5244: Unknown result type (might be due to invalid IL or missing references)
		//IL_5249: Unknown result type (might be due to invalid IL or missing references)
		//IL_5250: Unknown result type (might be due to invalid IL or missing references)
		//IL_5257: Unknown result type (might be due to invalid IL or missing references)
		//IL_525c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5263: Unknown result type (might be due to invalid IL or missing references)
		//IL_526a: Unknown result type (might be due to invalid IL or missing references)
		//IL_526f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5276: Unknown result type (might be due to invalid IL or missing references)
		//IL_527d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5282: Unknown result type (might be due to invalid IL or missing references)
		//IL_5289: Unknown result type (might be due to invalid IL or missing references)
		//IL_5290: Unknown result type (might be due to invalid IL or missing references)
		//IL_5295: Unknown result type (might be due to invalid IL or missing references)
		//IL_529c: Unknown result type (might be due to invalid IL or missing references)
		//IL_52a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_52a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_52af: Unknown result type (might be due to invalid IL or missing references)
		//IL_52b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_52bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_52d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_52dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_52e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_52e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_52f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_52fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5302: Unknown result type (might be due to invalid IL or missing references)
		//IL_5307: Unknown result type (might be due to invalid IL or missing references)
		//IL_530e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5315: Unknown result type (might be due to invalid IL or missing references)
		//IL_531a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5321: Unknown result type (might be due to invalid IL or missing references)
		//IL_5328: Unknown result type (might be due to invalid IL or missing references)
		//IL_532d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5334: Unknown result type (might be due to invalid IL or missing references)
		//IL_533b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5340: Unknown result type (might be due to invalid IL or missing references)
		//IL_5347: Unknown result type (might be due to invalid IL or missing references)
		//IL_534e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5353: Unknown result type (might be due to invalid IL or missing references)
		//IL_591e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5923: Unknown result type (might be due to invalid IL or missing references)
		//IL_5926: Unknown result type (might be due to invalid IL or missing references)
		//IL_5928: Unknown result type (might be due to invalid IL or missing references)
		//IL_592e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5930: Unknown result type (might be due to invalid IL or missing references)
		//IL_5936: Unknown result type (might be due to invalid IL or missing references)
		//IL_5938: Unknown result type (might be due to invalid IL or missing references)
		//IL_593e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5940: Unknown result type (might be due to invalid IL or missing references)
		//IL_5946: Unknown result type (might be due to invalid IL or missing references)
		//IL_5948: Unknown result type (might be due to invalid IL or missing references)
		//IL_594e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5950: Unknown result type (might be due to invalid IL or missing references)
		//IL_5956: Unknown result type (might be due to invalid IL or missing references)
		//IL_5958: Unknown result type (might be due to invalid IL or missing references)
		//IL_595e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5960: Unknown result type (might be due to invalid IL or missing references)
		//IL_5966: Unknown result type (might be due to invalid IL or missing references)
		//IL_5968: Unknown result type (might be due to invalid IL or missing references)
		//IL_596e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5970: Unknown result type (might be due to invalid IL or missing references)
		//IL_5976: Unknown result type (might be due to invalid IL or missing references)
		//IL_5978: Unknown result type (might be due to invalid IL or missing references)
		//IL_597e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5980: Unknown result type (might be due to invalid IL or missing references)
		//IL_5986: Unknown result type (might be due to invalid IL or missing references)
		//IL_5988: Unknown result type (might be due to invalid IL or missing references)
		//IL_598e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5990: Unknown result type (might be due to invalid IL or missing references)
		//IL_5996: Unknown result type (might be due to invalid IL or missing references)
		//IL_5998: Unknown result type (might be due to invalid IL or missing references)
		//IL_599e: Unknown result type (might be due to invalid IL or missing references)
		//IL_59a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_59a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_59a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_59b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_544c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5451: Unknown result type (might be due to invalid IL or missing references)
		//IL_5497: Unknown result type (might be due to invalid IL or missing references)
		//IL_549c: Unknown result type (might be due to invalid IL or missing references)
		//IL_54e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_54e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_54fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5501: Unknown result type (might be due to invalid IL or missing references)
		//IL_5508: Unknown result type (might be due to invalid IL or missing references)
		//IL_550f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5514: Unknown result type (might be due to invalid IL or missing references)
		//IL_551b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5522: Unknown result type (might be due to invalid IL or missing references)
		//IL_5527: Unknown result type (might be due to invalid IL or missing references)
		//IL_552e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5535: Unknown result type (might be due to invalid IL or missing references)
		//IL_553a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5541: Unknown result type (might be due to invalid IL or missing references)
		//IL_5548: Unknown result type (might be due to invalid IL or missing references)
		//IL_554d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5554: Unknown result type (might be due to invalid IL or missing references)
		//IL_555b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5560: Unknown result type (might be due to invalid IL or missing references)
		//IL_5567: Unknown result type (might be due to invalid IL or missing references)
		//IL_556e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5573: Unknown result type (might be due to invalid IL or missing references)
		//IL_557a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5581: Unknown result type (might be due to invalid IL or missing references)
		//IL_5586: Unknown result type (might be due to invalid IL or missing references)
		//IL_558d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5594: Unknown result type (might be due to invalid IL or missing references)
		//IL_5599: Unknown result type (might be due to invalid IL or missing references)
		//IL_55a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_55a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_55ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_55b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_55ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_55bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_55cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_55d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_55d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_55ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_55f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_55f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_55ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5606: Unknown result type (might be due to invalid IL or missing references)
		//IL_560b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5612: Unknown result type (might be due to invalid IL or missing references)
		//IL_5619: Unknown result type (might be due to invalid IL or missing references)
		//IL_561e: Unknown result type (might be due to invalid IL or missing references)
		//IL_536a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5371: Unknown result type (might be due to invalid IL or missing references)
		//IL_5376: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_56db: Unknown result type (might be due to invalid IL or missing references)
		//IL_56e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_56f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_56f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_56fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5700: Unknown result type (might be due to invalid IL or missing references)
		//IL_5707: Unknown result type (might be due to invalid IL or missing references)
		//IL_570c: Unknown result type (might be due to invalid IL or missing references)
		//IL_570e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5713: Unknown result type (might be due to invalid IL or missing references)
		//IL_571a: Unknown result type (might be due to invalid IL or missing references)
		//IL_571f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5721: Unknown result type (might be due to invalid IL or missing references)
		//IL_5726: Unknown result type (might be due to invalid IL or missing references)
		//IL_5734: Unknown result type (might be due to invalid IL or missing references)
		//IL_573b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5740: Unknown result type (might be due to invalid IL or missing references)
		//IL_5747: Unknown result type (might be due to invalid IL or missing references)
		//IL_574e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5753: Unknown result type (might be due to invalid IL or missing references)
		//IL_575a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5761: Unknown result type (might be due to invalid IL or missing references)
		//IL_5766: Unknown result type (might be due to invalid IL or missing references)
		//IL_576d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5774: Unknown result type (might be due to invalid IL or missing references)
		//IL_5779: Unknown result type (might be due to invalid IL or missing references)
		//IL_5780: Unknown result type (might be due to invalid IL or missing references)
		//IL_5787: Unknown result type (might be due to invalid IL or missing references)
		//IL_578c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5793: Unknown result type (might be due to invalid IL or missing references)
		//IL_579a: Unknown result type (might be due to invalid IL or missing references)
		//IL_579f: Unknown result type (might be due to invalid IL or missing references)
		//IL_57a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_57b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_57b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_57c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_57c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_57cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_57d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_57d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_57df: Unknown result type (might be due to invalid IL or missing references)
		//IL_57e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_57eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_57f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_57f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_57fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_5805: Unknown result type (might be due to invalid IL or missing references)
		//IL_580c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5811: Unknown result type (might be due to invalid IL or missing references)
		//IL_5818: Unknown result type (might be due to invalid IL or missing references)
		//IL_581f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5824: Unknown result type (might be due to invalid IL or missing references)
		//IL_582b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5832: Unknown result type (might be due to invalid IL or missing references)
		//IL_5837: Unknown result type (might be due to invalid IL or missing references)
		//IL_583e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5845: Unknown result type (might be due to invalid IL or missing references)
		//IL_584a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5851: Unknown result type (might be due to invalid IL or missing references)
		//IL_5858: Unknown result type (might be due to invalid IL or missing references)
		//IL_585d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5635: Unknown result type (might be due to invalid IL or missing references)
		//IL_563c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5641: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_59d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_59de: Unknown result type (might be due to invalid IL or missing references)
		//IL_5871: Unknown result type (might be due to invalid IL or missing references)
		//IL_5878: Unknown result type (might be due to invalid IL or missing references)
		//IL_587d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_5baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f51: Unknown result type (might be due to invalid IL or missing references)
		DrawDataCache = drawData;
		SelectedDrawnProjectile = null;
		if (player.heldProj != -1)
		{
			SelectedDrawnProjectile = Main.projectile[player.heldProj];
		}
		if (overrideHeldProjectile != null)
		{
			SelectedDrawnProjectile = overrideHeldProjectile;
		}
		DustCache = dust;
		GoreCache = gore;
		drawPlayer = player;
		shadow = shadowOpacity;
		this.rotation = rotation;
		this.rotationOrigin = rotationOrigin;
		heldItem = player.lastVisualizedSelectedItem;
		cHead = drawPlayer.cHead;
		cBody = drawPlayer.cBody;
		cLegs = drawPlayer.cLegs;
		if (drawPlayer.wearsRobe)
		{
			cLegs = cBody;
		}
		cHandOn = drawPlayer.cHandOn;
		cHandOff = drawPlayer.cHandOff;
		cBack = drawPlayer.cBack;
		cFront = drawPlayer.cFront;
		cShoe = drawPlayer.cShoe;
		cFlameWaker = drawPlayer.cFlameWaker;
		cWaist = drawPlayer.cWaist;
		cShield = drawPlayer.cShield;
		cNeck = drawPlayer.cNeck;
		cFace = drawPlayer.cFace;
		cBalloon = drawPlayer.cBalloon;
		cWings = drawPlayer.cWings;
		cCarpet = drawPlayer.cCarpet;
		cPortableStool = drawPlayer.cPortableStool;
		cFloatingTube = drawPlayer.cFloatingTube;
		cUnicornHorn = drawPlayer.cUnicornHorn;
		cAngelHalo = drawPlayer.cAngelHalo;
		cLeinShampoo = drawPlayer.cLeinShampoo;
		cBackpack = drawPlayer.cBackpack;
		cTail = drawPlayer.cTail;
		cFaceHead = drawPlayer.cFaceHead;
		cFaceFlower = drawPlayer.cFaceFlower;
		cFaceMask = drawPlayer.cFaceMask;
		cBalloonFront = drawPlayer.cBalloonFront;
		cBeard = drawPlayer.cBeard;
		cCoat = drawPlayer.cCoat;
		isSitting = drawPlayer.sitting.isSitting;
		seatYOffset = 0f;
		sittingIndex = 0;
		Vector2 posOffset = Vector2.Zero;
		drawPlayer.sitting.GetSittingOffsetInfo(drawPlayer, out posOffset, out seatYOffset);
		if (isSitting)
		{
			sittingIndex = drawPlayer.sitting.sittingIndex;
		}
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 17)
		{
			isSitting = true;
		}
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 23)
		{
			isSitting = true;
		}
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 45)
		{
			isSitting = true;
		}
		isSleeping = drawPlayer.sleeping.isSleeping;
		VisualPositionOffset = Vector2.Zero;
		Position = drawPosition;
		Position += new Vector2(drawPlayer.MountXOffset * (float)drawPlayer.direction, 0f);
		if (isSitting)
		{
			torsoOffset = seatYOffset;
			Position += posOffset;
		}
		else
		{
			sittingIndex = -1;
		}
		if (isSleeping)
		{
			this.rotationOrigin = player.Size / 2f;
			drawPlayer.sleeping.GetSleepingOffsetInfo(drawPlayer, out var posOffset2);
			Position += posOffset2;
		}
		weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
		if (heldItem.type == 4952)
		{
			weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
		}
		if (GolfHelper.IsPlayerHoldingClub(player) && player.itemAnimation > player.itemAnimationMax)
		{
			weaponDrawOrder = WeaponDrawOrder.OverFrontArm;
		}
		projectileDrawPosition = -1;
		ItemLocation = Position + (drawPlayer.itemLocation - drawPlayer.position);
		armorAdjust = 0;
		missingHand = false;
		missingArm = false;
		skinVar = drawPlayer.skinVariant;
		if (drawPlayer.body == 77 || drawPlayer.body == 103 || drawPlayer.body == 41 || drawPlayer.body == 100 || drawPlayer.body == 10 || drawPlayer.body == 11 || drawPlayer.body == 12 || drawPlayer.body == 13 || drawPlayer.body == 14 || drawPlayer.body == 43 || drawPlayer.body == 15 || drawPlayer.body == 16 || drawPlayer.body == 20 || drawPlayer.body == 39 || drawPlayer.body == 50 || drawPlayer.body == 38 || drawPlayer.body == 40 || drawPlayer.body == 57 || drawPlayer.body == 44 || drawPlayer.body == 52 || drawPlayer.body == 53 || drawPlayer.body == 68 || drawPlayer.body == 81 || drawPlayer.body == 85 || drawPlayer.body == 88 || drawPlayer.body == 98 || drawPlayer.body == 86 || drawPlayer.body == 87 || drawPlayer.body == 99 || drawPlayer.body == 165 || drawPlayer.body == 166 || drawPlayer.body == 167 || drawPlayer.body == 171 || drawPlayer.body == 45 || drawPlayer.body == 168 || drawPlayer.body == 169 || drawPlayer.body == 42 || drawPlayer.body == 180 || drawPlayer.body == 181 || drawPlayer.body == 183 || drawPlayer.body == 186 || drawPlayer.body == 187 || drawPlayer.body == 188 || drawPlayer.body == 64 || drawPlayer.body == 189 || drawPlayer.body == 191 || drawPlayer.body == 192 || drawPlayer.body == 198 || drawPlayer.body == 199 || drawPlayer.body == 202 || drawPlayer.body == 203 || drawPlayer.body == 58 || drawPlayer.body == 59 || drawPlayer.body == 60 || drawPlayer.body == 61 || drawPlayer.body == 62 || drawPlayer.body == 63 || drawPlayer.body == 36 || drawPlayer.body == 104 || drawPlayer.body == 184 || drawPlayer.body == 74 || drawPlayer.body == 78 || drawPlayer.body == 185 || drawPlayer.body == 196 || drawPlayer.body == 197 || drawPlayer.body == 182 || drawPlayer.body == 87 || drawPlayer.body == 76 || drawPlayer.body == 209 || drawPlayer.body == 168 || drawPlayer.body == 210 || drawPlayer.body == 211 || drawPlayer.body == 213)
		{
			missingHand = true;
		}
		int body = drawPlayer.body;
		if (body == 83)
		{
			missingArm = false;
		}
		else
		{
			missingArm = true;
		}
		drawPlayer.GetHairSettings(out fullHair, out hatHair, out hideHair, out backHairDraw, out drawsBackHairWithoutHeadgear);
		hairDyePacked = PlayerDrawHelper.PackShader(drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
		if (drawPlayer.head == 0 && drawPlayer.hairDye == 0)
		{
			hairDyePacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
		}
		skinDyePacked = player.skinDyePacked;
		if (drawPlayer.mount.Active)
		{
			if (drawPlayer.mount.Type == 52)
			{
				AdjustmentsForWolfMount();
			}
			if (drawPlayer.mount.Type == 54)
			{
				AdjustmentsForVelociraptorMount();
			}
			if (drawPlayer.mount.Type == 55)
			{
				AdjustmentsForRatMount();
			}
			if (drawPlayer.mount.Type == 56)
			{
				AdjustmentsForBatMount();
			}
			if (drawPlayer.mount.Type == 61)
			{
				AdjustmentsForPixieMount();
			}
		}
		if (drawPlayer.isDisplayDollOrInanimate)
		{
			Point val = Center.ToTileCoordinates();
			if (Main.InSmartCursorHighlightArea(val.X, val.Y, out var actuallySelected))
			{
				Color color = Lighting.GetColor(val.X, val.Y);
				int num = (color.R + color.G + color.B) / 3;
				if (num > 10)
				{
					selectionGlowColor = Colors.GetSelectionGlowColor(actuallySelected, num);
				}
			}
		}
		mountOffSet = drawPlayer.HeightOffsetVisual;
		Position.Y -= mountOffSet;
		if (drawPlayer.mount.Active)
		{
			Mount.currentShader = (drawPlayer.mount.Cart ? drawPlayer.cMinecart : drawPlayer.cMount);
		}
		else
		{
			Mount.currentShader = 0;
		}
		playerEffect = (SpriteEffects)0;
		itemEffect = (SpriteEffects)1;
		colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(), shadow);
		colorEyeWhites = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), Color.White), shadow);
		colorEyes = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.eyeColor), shadow);
		colorHead = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.skinColor), shadow);
		colorBodySkin = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.skinColor), shadow);
		colorLegs = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.skinColor), shadow);
		colorShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.shirtColor), shadow);
		colorUnderShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.underShirtColor), shadow);
		colorPants = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.pantsColor), shadow);
		colorShoes = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.shoeColor), shadow);
		colorArmorHead = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.25) / 16, Color.White), shadow);
		colorArmorBody = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), shadow);
		colorMount = colorArmorBody;
		colorArmorLegs = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
		floatingTubeColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
		colorElectricity = new Color(255, 255, 255, 100);
		colorDisplayDollSkin = colorBodySkin;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		headGlowMask = -1;
		bodyGlowMask = -1;
		armGlowMask = -1;
		legsGlowMask = -1;
		headGlowColor = Color.Transparent;
		bodyGlowColor = Color.Transparent;
		armGlowColor = Color.Transparent;
		legsGlowColor = Color.Transparent;
		switch (drawPlayer.head)
		{
		case 169:
			num2++;
			break;
		case 170:
			num3++;
			break;
		case 171:
			num4++;
			break;
		case 189:
			num5++;
			break;
		}
		switch (drawPlayer.body)
		{
		case 175:
			num2++;
			break;
		case 176:
			num3++;
			break;
		case 177:
			num4++;
			break;
		case 190:
			num5++;
			break;
		}
		switch (drawPlayer.legs)
		{
		case 110:
			num2++;
			break;
		case 111:
			num3++;
			break;
		case 112:
			num4++;
			break;
		case 130:
			num5++;
			break;
		}
		num2 = 3;
		num3 = 3;
		num4 = 3;
		num5 = 3;
		ArkhalisColor = drawPlayer.underShirtColor;
		ArkhalisColor.A = 180;
		if (drawPlayer.head == 169)
		{
			headGlowMask = 15;
			byte b = (byte)(62.5f * (float)(1 + num2));
			headGlowColor = new Color((int)b, (int)b, (int)b, 0);
		}
		else if (drawPlayer.head == 216)
		{
			headGlowMask = 256;
			byte b2 = 127;
			headGlowColor = new Color((int)b2, (int)b2, (int)b2, 0);
		}
		else if (drawPlayer.head == 210)
		{
			headGlowMask = 242;
			byte b3 = 127;
			headGlowColor = new Color((int)b3, (int)b3, (int)b3, 0);
		}
		else if (drawPlayer.head == 214)
		{
			headGlowMask = 245;
			headGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.head == 240)
		{
			headGlowMask = 273;
			headGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.head == 267)
		{
			headGlowMask = 301;
			headGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.head == 268)
		{
			headGlowMask = 302;
			float num6 = (float)(int)Main.mouseTextColor / 255f;
			num6 *= num6;
			headGlowColor = new Color(255, 255, 255) * num6;
		}
		else if (drawPlayer.head == 269)
		{
			headGlowMask = 304;
			headGlowColor = new Color(200, 200, 200);
		}
		else if (drawPlayer.head == 270)
		{
			headGlowMask = 305;
			headGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.head == 271)
		{
			headGlowMask = 309;
			headGlowColor = Color.White;
		}
		else if (drawPlayer.head == 170)
		{
			headGlowMask = 16;
			byte b4 = (byte)(62.5f * (float)(1 + num3));
			headGlowColor = new Color((int)b4, (int)b4, (int)b4, 0);
		}
		else if (drawPlayer.head == 189)
		{
			headGlowMask = 184;
			byte b5 = (byte)(62.5f * (float)(1 + num5));
			headGlowColor = new Color((int)b5, (int)b5, (int)b5, 0);
			colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color((int)b5, (int)b5, (int)b5, 255), shadow);
		}
		else if (drawPlayer.head == 171)
		{
			byte b6 = (byte)(62.5f * (float)(1 + num4));
			colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color((int)b6, (int)b6, (int)b6, 255), shadow);
		}
		else if (drawPlayer.head == 175)
		{
			headGlowMask = 41;
			headGlowColor = new Color(255, 255, 255, 0);
		}
		else if (drawPlayer.head == 193)
		{
			headGlowMask = 209;
			headGlowColor = new Color(255, 255, 255, 127);
		}
		else if (drawPlayer.head == 109)
		{
			headGlowMask = 208;
			headGlowColor = new Color(255, 255, 255, 0);
		}
		else if (drawPlayer.head == 178)
		{
			headGlowMask = 96;
			headGlowColor = new Color(255, 255, 255, 0);
		}
		else if (drawPlayer.head == 282)
		{
			headGlowMask = 357;
			float num7 = (float)(int)Main.mouseTextColor / 255f;
			num7 *= num7;
			headGlowColor = new Color(255, 255, 255, 0) * num7;
		}
		else if (drawPlayer.head == 284)
		{
			headGlowMask = 365;
			headGlowColor = PlayerDrawLayers.GetChickenBonesGlowColor(ref this, scaleByShadow: false);
		}
		else if (drawPlayer.head == 285)
		{
			headGlowMask = 367;
			headGlowColor = new Color(255, 255, 255, 0);
		}
		else if (drawPlayer.head == 291)
		{
			headGlowMask = 375;
			headGlowColor = new Color(255, 255, 255, 255);
		}
		else if (drawPlayer.head == 292)
		{
			headGlowMask = 378;
			headGlowColor = PlayerDrawLayers.GetLunaGlowColor(ref this, scaleByShadow: false);
		}
		if (drawPlayer.body == 175)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 13;
			}
			else
			{
				bodyGlowMask = 18;
			}
			byte b7 = (byte)(62.5f * (float)(1 + num2));
			bodyGlowColor = new Color((int)b7, (int)b7, (int)b7, 0);
		}
		else if (drawPlayer.body == 208)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 246;
			}
			else
			{
				bodyGlowMask = 247;
			}
			armGlowMask = 248;
			bodyGlowColor = ArkhalisColor;
			armGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.body == 227)
		{
			bodyGlowColor = new Color(230, 230, 230, 60);
			armGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.body == 237)
		{
			float num8 = (float)(int)Main.mouseTextColor / 255f;
			num8 *= num8;
			bodyGlowColor = new Color(255, 255, 255) * num8;
		}
		else if (drawPlayer.body == 238 || drawPlayer.body == 260)
		{
			bodyGlowColor = new Color(255, 255, 255);
			armGlowColor = new Color(255, 255, 255);
		}
		else if (drawPlayer.body == 239)
		{
			bodyGlowColor = new Color(200, 200, 200, 150);
			armGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.body == 190)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 185;
			}
			else
			{
				bodyGlowMask = 186;
			}
			armGlowMask = 188;
			byte b8 = (byte)(62.5f * (float)(1 + num5));
			bodyGlowColor = new Color((int)b8, (int)b8, (int)b8, 0);
			armGlowColor = new Color((int)b8, (int)b8, (int)b8, 0);
			colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color((int)b8, (int)b8, (int)b8, 255), shadow);
		}
		else if (drawPlayer.body == 176)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 14;
			}
			else
			{
				bodyGlowMask = 19;
			}
			armGlowMask = 12;
			byte b9 = (byte)(62.5f * (float)(1 + num3));
			bodyGlowColor = new Color((int)b9, (int)b9, (int)b9, 0);
			armGlowColor = new Color((int)b9, (int)b9, (int)b9, 0);
		}
		else if (drawPlayer.body == 194)
		{
			bodyGlowMask = 210;
			armGlowMask = 211;
			bodyGlowColor = new Color(255, 255, 255, 127);
			armGlowColor = new Color(255, 255, 255, 127);
		}
		else if (drawPlayer.body == 177)
		{
			byte b10 = (byte)(62.5f * (float)(1 + num4));
			colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color((int)b10, (int)b10, (int)b10, 255), shadow);
		}
		else if (drawPlayer.body == 179)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 42;
			}
			else
			{
				bodyGlowMask = 43;
			}
			armGlowMask = 44;
			bodyGlowColor = new Color(255, 255, 255, 0);
			armGlowColor = new Color(255, 255, 255, 0);
		}
		if (drawPlayer.legs == 111)
		{
			legsGlowMask = 17;
			byte b11 = (byte)(62.5f * (float)(1 + num3));
			legsGlowColor = new Color((int)b11, (int)b11, (int)b11, 0);
		}
		else if (drawPlayer.legs == 157)
		{
			legsGlowMask = 249;
			legsGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.legs == 158)
		{
			legsGlowMask = 250;
			legsGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.legs == 210)
		{
			legsGlowMask = 274;
			legsGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.legs == 222)
		{
			legsGlowMask = 303;
			float num9 = (float)(int)Main.mouseTextColor / 255f;
			num9 *= num9;
			legsGlowColor = new Color(255, 255, 255) * num9;
		}
		else if (drawPlayer.legs == 225)
		{
			legsGlowMask = 306;
			legsGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.legs == 226)
		{
			legsGlowMask = 307;
			legsGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.legs == 110)
		{
			legsGlowMask = 199;
			byte b12 = (byte)(62.5f * (float)(1 + num2));
			legsGlowColor = new Color((int)b12, (int)b12, (int)b12, 0);
		}
		else if (drawPlayer.legs == 112)
		{
			byte b13 = (byte)(62.5f * (float)(1 + num4));
			colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color((int)b13, (int)b13, (int)b13, 255), shadow);
		}
		else if (drawPlayer.legs == 134)
		{
			legsGlowMask = 212;
			legsGlowColor = new Color(255, 255, 255, 127);
		}
		else if (drawPlayer.legs == 130)
		{
			byte b14 = (byte)(127 * (1 + num5));
			legsGlowMask = 187;
			legsGlowColor = new Color((int)b14, (int)b14, (int)b14, 0);
			colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color((int)b14, (int)b14, (int)b14, 255), shadow);
		}
		float alphaReduction = shadow;
		headGlowColor = drawPlayer.GetImmuneAlphaPure(headGlowColor, alphaReduction);
		bodyGlowColor = drawPlayer.GetImmuneAlphaPure(bodyGlowColor, alphaReduction);
		armGlowColor = drawPlayer.GetImmuneAlphaPure(armGlowColor, alphaReduction);
		legsGlowColor = drawPlayer.GetImmuneAlphaPure(legsGlowColor, alphaReduction);
		if (drawPlayer.head > 0 && drawPlayer.head < ArmorIDs.Head.Count)
		{
			Main.instance.LoadArmorHead(drawPlayer.head);
			int num10 = ArmorIDs.Head.Sets.FrontToBackID[drawPlayer.head];
			if (num10 >= 0)
			{
				Main.instance.LoadArmorHead(num10);
			}
		}
		if (drawPlayer.body > 0 && drawPlayer.body < ArmorIDs.Body.Count)
		{
			Main.instance.LoadArmorBody(drawPlayer.body);
		}
		if (drawPlayer.legs > 0 && drawPlayer.legs < ArmorIDs.Legs.Count)
		{
			Main.instance.LoadArmorLegs(drawPlayer.legs);
		}
		if (drawPlayer.handon > 0 && drawPlayer.handon < ArmorIDs.HandOn.Count)
		{
			Main.instance.LoadAccHandsOn(drawPlayer.handon);
		}
		if (drawPlayer.handoff > 0 && drawPlayer.handoff < ArmorIDs.HandOff.Count)
		{
			Main.instance.LoadAccHandsOff(drawPlayer.handoff);
		}
		if (drawPlayer.back > 0 && drawPlayer.back < ArmorIDs.Back.Count)
		{
			Main.instance.LoadAccBack(drawPlayer.back);
		}
		if (drawPlayer.front > 0 && drawPlayer.front < ArmorIDs.Front.Count)
		{
			Main.instance.LoadAccFront(drawPlayer.front);
		}
		if (drawPlayer.shoe > 0 && drawPlayer.shoe < ArmorIDs.Shoe.Count)
		{
			Main.instance.LoadAccShoes(drawPlayer.shoe);
		}
		if (drawPlayer.waist > 0 && drawPlayer.waist < ArmorIDs.Waist.Count)
		{
			Main.instance.LoadAccWaist(drawPlayer.waist);
		}
		if (drawPlayer.shield > 0 && drawPlayer.shield < ArmorIDs.Shield.Count)
		{
			Main.instance.LoadAccShield(drawPlayer.shield);
		}
		if (drawPlayer.neck > 0 && drawPlayer.neck < ArmorIDs.Neck.Count)
		{
			Main.instance.LoadAccNeck(drawPlayer.neck);
		}
		if (drawPlayer.face > 0 && drawPlayer.face < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.face);
		}
		if (drawPlayer.balloon > 0 && drawPlayer.balloon < ArmorIDs.Balloon.Count)
		{
			Main.instance.LoadAccBalloon(drawPlayer.balloon);
		}
		if (drawPlayer.backpack > 0 && drawPlayer.backpack < ArmorIDs.Back.Count)
		{
			Main.instance.LoadAccBack(drawPlayer.backpack);
		}
		if (drawPlayer.tail > 0 && drawPlayer.tail < ArmorIDs.Back.Count)
		{
			Main.instance.LoadAccBack(drawPlayer.tail);
		}
		if (drawPlayer.faceHead > 0 && drawPlayer.faceHead < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.faceHead);
		}
		if (drawPlayer.faceFlower > 0 && drawPlayer.faceFlower < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.faceFlower);
		}
		if (drawPlayer.faceMask > 0 && drawPlayer.faceMask < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.faceMask);
		}
		if (drawPlayer.balloonFront > 0 && drawPlayer.balloonFront < ArmorIDs.Balloon.Count)
		{
			Main.instance.LoadAccBalloon(drawPlayer.balloonFront);
		}
		if (drawPlayer.beard > 0 && drawPlayer.beard < ArmorIDs.Beard.Count)
		{
			Main.instance.LoadAccBeard(drawPlayer.beard);
		}
		if (drawPlayer.coat > 0 && drawPlayer.coat < ArmorIDs.Body.Count)
		{
			Main.instance.LoadArmorBody(drawPlayer.coat);
		}
		Main.instance.LoadHair(drawPlayer.hair);
		if (drawPlayer.eyebrellaCloud)
		{
			Main.instance.LoadProjectile(238);
		}
		if (drawPlayer.isHatRackDoll)
		{
			colorLegs = Color.Transparent;
			colorBodySkin = Color.Transparent;
			colorHead = Color.Transparent;
			colorHair = Color.Transparent;
			colorEyes = Color.Transparent;
			colorEyeWhites = Color.Transparent;
		}
		if (drawPlayer.isDisplayDollOrInanimate)
		{
			if (drawPlayer.isFullbright)
			{
				colorHead = Color.White;
				colorBodySkin = Color.White;
				colorLegs = Color.White;
				colorEyes = Color.White;
				colorEyeWhites = Color.White;
				colorArmorHead = Color.White;
				colorArmorBody = Color.White;
				colorArmorLegs = Color.White;
				colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR;
			}
			else
			{
				colorDisplayDollSkin = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR), shadow);
			}
		}
		if (!drawPlayer.isDisplayDollOrInanimate)
		{
			if ((drawPlayer.head == 80 || drawPlayer.head == 79 || drawPlayer.head == 78 || drawPlayer.head == 283) && drawPlayer.body == 51 && drawPlayer.legs == 47)
			{
				float num11 = (float)(int)Main.mouseTextColor / 200f - 0.3f;
				if (shadow != 0f)
				{
					num11 = 0f;
				}
				colorArmorHead.R = (byte)((float)(int)colorArmorHead.R * num11);
				colorArmorHead.G = (byte)((float)(int)colorArmorHead.G * num11);
				colorArmorHead.B = (byte)((float)(int)colorArmorHead.B * num11);
				colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num11);
				colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num11);
				colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num11);
				colorArmorLegs.R = (byte)((float)(int)colorArmorLegs.R * num11);
				colorArmorLegs.G = (byte)((float)(int)colorArmorLegs.G * num11);
				colorArmorLegs.B = (byte)((float)(int)colorArmorLegs.B * num11);
			}
			if (drawPlayer.head == 193 && drawPlayer.body == 194 && drawPlayer.legs == 134)
			{
				float num12 = 0.6f - drawPlayer.ghostFade * 0.3f;
				if (shadow != 0f)
				{
					num12 = 0f;
				}
				colorArmorHead.R = (byte)((float)(int)colorArmorHead.R * num12);
				colorArmorHead.G = (byte)((float)(int)colorArmorHead.G * num12);
				colorArmorHead.B = (byte)((float)(int)colorArmorHead.B * num12);
				colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num12);
				colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num12);
				colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num12);
				colorArmorLegs.R = (byte)((float)(int)colorArmorLegs.R * num12);
				colorArmorLegs.G = (byte)((float)(int)colorArmorLegs.G * num12);
				colorArmorLegs.B = (byte)((float)(int)colorArmorLegs.B * num12);
			}
			if (shadow > 0f)
			{
				colorLegs = Color.Transparent;
				colorBodySkin = Color.Transparent;
				colorHead = Color.Transparent;
				colorHair = Color.Transparent;
				colorEyes = Color.Transparent;
				colorEyeWhites = Color.Transparent;
			}
		}
		float num13 = 1f;
		float num14 = 1f;
		float num15 = 1f;
		float num16 = 1f;
		Color newColor;
		if (drawPlayer.honey && Main.rand.Next(30) == 0 && shadow == 0f)
		{
			Vector2 position = Position;
			int width = drawPlayer.width;
			int height = drawPlayer.height;
			newColor = default;
			Dust dust2 = Dust.NewDustDirect(position, width, height, 152, 0f, 0f, 150, newColor);
			dust2.velocity.Y = 0.3f;
			dust2.velocity.X *= 0.1f;
			dust2.scale += (float)Main.rand.Next(3, 4) * 0.1f;
			dust2.alpha = 100;
			dust2.noGravity = true;
			dust2.velocity += drawPlayer.velocity * 0.1f;
			DustCache.Add(dust2.dustIndex);
		}
		if (drawPlayer.dryadWard && drawPlayer.velocity.X != 0f && Main.rand.Next(4) == 0)
		{
			Vector2 position2 = new Vector2(drawPlayer.position.X - 2f, drawPlayer.position.Y + (float)drawPlayer.height - 2f);
			int width2 = drawPlayer.width + 4;
			newColor = default;
			Dust dust3 = Dust.NewDustDirect(position2, width2, 4, 163, 0f, 0f, 100, newColor, 1.5f);
			dust3.noGravity = true;
			dust3.noLight = true;
			dust3.velocity *= 0f;
			DustCache.Add(dust3.dustIndex);
		}
		if (drawPlayer.poisoned)
		{
			if (Main.rand.Next(50) == 0 && shadow == 0f)
			{
				Vector2 position3 = Position;
				int width3 = drawPlayer.width;
				int height2 = drawPlayer.height;
				newColor = default;
				Dust dust4 = Dust.NewDustDirect(position3, width3, height2, 46, 0f, 0f, 150, newColor, 0.2f);
				dust4.noGravity = true;
				dust4.fadeIn = 1.9f;
				DustCache.Add(dust4.dustIndex);
			}
			num13 *= 0.65f;
			num15 *= 0.75f;
		}
		if (drawPlayer.venom)
		{
			if (Main.rand.Next(10) == 0 && shadow == 0f)
			{
				Vector2 position4 = Position;
				int width4 = drawPlayer.width;
				int height3 = drawPlayer.height;
				newColor = default;
				Dust dust5 = Dust.NewDustDirect(position4, width4, height3, 171, 0f, 0f, 100, newColor, 0.5f);
				dust5.noGravity = true;
				dust5.fadeIn = 1.5f;
				DustCache.Add(dust5.dustIndex);
			}
			num14 *= 0.45f;
			num13 *= 0.75f;
		}
		if (drawPlayer.chlorophyteSpore)
		{
			if (Main.rand.Next(5) < 4 && shadow == 0f)
			{
				Vector2 position5 = Position;
				int width5 = drawPlayer.width;
				int height4 = drawPlayer.height;
				float speedX = drawPlayer.velocity.X * 0.4f;
				float speedY = drawPlayer.velocity.Y * 0.4f;
				newColor = default;
				Dust dust6 = Dust.NewDustDirect(position5, width5, height4, 157, speedX, speedY, 180, newColor, 1.95f);
				dust6.noGravity = true;
				dust6.velocity *= 0.75f;
				dust6.velocity.X *= 0.75f;
				dust6.velocity.Y--;
				if (Main.rand.Next(4) == 0)
				{
					dust6.noGravity = false;
					dust6.scale *= 0.5f;
				}
			}
			num13 *= 0.65f;
			num15 *= 0.75f;
		}
		if (drawPlayer.onFire)
		{
			if (Main.vampireSeed)
			{
				if (shadow == 0f)
				{
					for (int i = 0; i < 5; i++)
					{
						Vector2 position6 = new Vector2(Position.X - 2f, Position.Y - 2f);
						int width6 = drawPlayer.width + 10;
						int height5 = drawPlayer.height + 10;
						float speedX2 = drawPlayer.velocity.X * 0.4f;
						float speedY2 = drawPlayer.velocity.Y * 0.4f;
						newColor = default;
						Dust dust7 = Dust.NewDustDirect(position6, width6, height5, 6, speedX2, speedY2, 100, newColor, 3f);
						dust7.noGravity = true;
						dust7.velocity *= 2.3f;
						dust7.velocity.Y -= 0.8f;
						if (i == 0)
						{
							dust7.velocity.X *= 0.5f;
							dust7.velocity.Y -= 1.5f;
							dust7.noGravity = false;
							dust7.scale *= 0.4f;
						}
						DustCache.Add(dust7.dustIndex);
					}
				}
				num15 *= 0.6f;
				num14 *= 0.7f;
			}
			else
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Vector2 position7 = new Vector2(Position.X - 2f, Position.Y - 2f);
					int width7 = drawPlayer.width + 4;
					int height6 = drawPlayer.height + 4;
					float speedX3 = drawPlayer.velocity.X * 0.4f;
					float speedY3 = drawPlayer.velocity.Y * 0.4f;
					newColor = default;
					Dust dust8 = Dust.NewDustDirect(position7, width7, height6, 6, speedX3, speedY3, 100, newColor, 3f);
					dust8.noGravity = true;
					dust8.velocity *= 1.8f;
					dust8.velocity.Y -= 0.5f;
					DustCache.Add(dust8.dustIndex);
				}
				num15 *= 0.6f;
				num14 *= 0.7f;
			}
		}
		if (drawPlayer.onFire3)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Vector2 position8 = new Vector2(Position.X - 2f, Position.Y - 2f);
				int width8 = drawPlayer.width + 4;
				int height7 = drawPlayer.height + 4;
				float speedX4 = drawPlayer.velocity.X * 0.4f;
				float speedY4 = drawPlayer.velocity.Y * 0.4f;
				newColor = default;
				Dust dust9 = Dust.NewDustDirect(position8, width8, height7, 6, speedX4, speedY4, 100, newColor, 3f);
				dust9.noGravity = true;
				dust9.velocity *= 1.8f;
				dust9.velocity.Y -= 0.5f;
				DustCache.Add(dust9.dustIndex);
			}
			num15 *= 0.6f;
			num14 *= 0.7f;
		}
		if (drawPlayer.dripping && shadow == 0f && Main.rand.Next(4) != 0)
		{
			Vector2 position9 = Position;
			position9.X -= 2f;
			position9.Y -= 2f;
			if (Main.rand.Next(2) == 0)
			{
				Vector2 position10 = position9;
				int width9 = drawPlayer.width + 4;
				int height8 = drawPlayer.height + 2;
				newColor = default;
				Dust dust10 = Dust.NewDustDirect(position10, width9, height8, 211, 0f, 0f, 50, newColor, 0.8f);
				if (Main.rand.Next(2) == 0)
				{
					dust10.alpha += 25;
				}
				if (Main.rand.Next(2) == 0)
				{
					dust10.alpha += 25;
				}
				dust10.noLight = true;
				dust10.velocity *= 0.2f;
				dust10.velocity.Y += 0.2f;
				dust10.velocity += drawPlayer.velocity;
				DustCache.Add(dust10.dustIndex);
			}
			else
			{
				Vector2 position11 = position9;
				int width10 = drawPlayer.width + 8;
				int height9 = drawPlayer.height + 8;
				newColor = default;
				Dust dust11 = Dust.NewDustDirect(position11, width10, height9, 211, 0f, 0f, 50, newColor, 1.1f);
				if (Main.rand.Next(2) == 0)
				{
					dust11.alpha += 25;
				}
				if (Main.rand.Next(2) == 0)
				{
					dust11.alpha += 25;
				}
				dust11.noLight = true;
				dust11.noGravity = true;
				dust11.velocity *= 0.2f;
				dust11.velocity.Y++;
				dust11.velocity += drawPlayer.velocity;
				DustCache.Add(dust11.dustIndex);
			}
		}
		if (drawPlayer.drippingSlime)
		{
			int alpha = 175;
			Color newColor2 = new Color(0, 80, 255, 100);
			if (Main.rand.Next(4) != 0 && shadow == 0f)
			{
				Vector2 position12 = Position;
				position12.X -= 2f;
				position12.Y -= 2f;
				if (Main.rand.Next(2) == 0)
				{
					Dust dust12 = Dust.NewDustDirect(position12, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha, newColor2, 1.4f);
					if (Main.rand.Next(2) == 0)
					{
						dust12.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust12.alpha += 25;
					}
					dust12.noLight = true;
					dust12.velocity *= 0.2f;
					dust12.velocity.Y += 0.2f;
					dust12.velocity += drawPlayer.velocity;
					DustCache.Add(dust12.dustIndex);
				}
			}
			num13 *= 0.8f;
			num14 *= 0.8f;
		}
		if (drawPlayer.drippingSparkleSlime)
		{
			int alpha2 = 100;
			if (Main.rand.Next(4) != 0 && shadow == 0f)
			{
				Vector2 position13 = Position;
				position13.X -= 2f;
				position13.Y -= 2f;
				if (Main.rand.Next(4) == 0)
				{
					Color newColor3 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
					newColor3.A /= 2;
					Dust dust13 = Dust.NewDustDirect(position13, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha2, newColor3, 0.65f);
					if (Main.rand.Next(2) == 0)
					{
						dust13.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust13.alpha += 25;
					}
					dust13.noLight = true;
					dust13.velocity *= 0.2f;
					dust13.velocity += drawPlayer.velocity * 0.7f;
					dust13.fadeIn = 0.8f;
					DustCache.Add(dust13.dustIndex);
				}
				if (Main.rand.Next(30) == 0)
				{
					Color val2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
					val2.A /= 2;
					Dust dust14 = Dust.NewDustDirect(position13, drawPlayer.width + 4, drawPlayer.height + 2, 43, 0f, 0f, 254, new Color(127, 127, 127, 0), 0.45f);
					dust14.noLight = true;
					dust14.velocity.X *= 0f;
					dust14.velocity *= 0.03f;
					dust14.fadeIn = 0.6f;
					DustCache.Add(dust14.dustIndex);
				}
			}
			num13 *= 0.94f;
			num14 *= 0.82f;
		}
		if (drawPlayer.ichor)
		{
			num15 = 0f;
		}
		if (drawPlayer.electrified && shadow == 0f && Main.rand.Next(3) == 0)
		{
			Vector2 position14 = new Vector2(Position.X - 2f, Position.Y - 2f);
			int width11 = drawPlayer.width + 4;
			int height10 = drawPlayer.height + 4;
			newColor = default;
			Dust dust15 = Dust.NewDustDirect(position14, width11, height10, 226, 0f, 0f, 100, newColor, 0.5f);
			dust15.velocity *= 1.6f;
			dust15.velocity.Y--;
			dust15.position = Vector2.Lerp(dust15.position, drawPlayer.Center, 0.5f);
			DustCache.Add(dust15.dustIndex);
		}
		if (drawPlayer.blueLightning && shadow == 0f && Main.rand.Next(10) == 0)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlueLightningSmall, new ParticleOrchestraSettings
			{
				MovementVector = Main.rand.NextVector2Circular(1f, 1f),
				PositionInWorld = Main.rand.NextVector2FromRectangle(drawPlayer.Hitbox)
			});
		}
		if (drawPlayer.redLightning && shadow == 0f && Main.rand.Next(10) == 0)
		{
			ParticleOrchestraSettings settings = new ParticleOrchestraSettings
			{
				MovementVector = Main.rand.NextVector2Circular(1f, 1f),
				PositionInWorld = Main.rand.NextVector2FromRectangle(drawPlayer.Hitbox)
			};
			newColor = Projectile.GetLightningColor(1122);
			settings.UniqueInfoPiece = (int)newColor.PackedValue;
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.RedLightningSmall, settings);
		}
		if (drawPlayer.burned)
		{
			if (shadow == 0f)
			{
				Vector2 position15 = new Vector2(Position.X - 2f, Position.Y - 2f);
				int width12 = drawPlayer.width + 4;
				int height11 = drawPlayer.height + 4;
				float speedX5 = drawPlayer.velocity.X * 0.4f;
				float speedY5 = drawPlayer.velocity.Y * 0.4f;
				newColor = default;
				Dust dust16 = Dust.NewDustDirect(position15, width12, height11, 6, speedX5, speedY5, 100, newColor, 2f);
				dust16.noGravity = true;
				dust16.velocity *= 1.8f;
				dust16.velocity.Y -= 0.75f;
				DustCache.Add(dust16.dustIndex);
			}
			num13 = 1f;
			num15 *= 0.6f;
			num14 *= 0.7f;
		}
		if (drawPlayer.onFrostBurn)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Vector2 position16 = new Vector2(Position.X - 2f, Position.Y - 2f);
				int width13 = drawPlayer.width + 4;
				int height12 = drawPlayer.height + 4;
				float speedX6 = drawPlayer.velocity.X * 0.4f;
				float speedY6 = drawPlayer.velocity.Y * 0.4f;
				newColor = default;
				Dust dust17 = Dust.NewDustDirect(position16, width13, height12, 135, speedX6, speedY6, 100, newColor, 3f);
				dust17.noGravity = true;
				dust17.velocity *= 1.8f;
				dust17.velocity.Y -= 0.5f;
				DustCache.Add(dust17.dustIndex);
			}
			num13 *= 0.5f;
			num14 *= 0.7f;
		}
		if (drawPlayer.onFrostBurn2)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Vector2 position17 = new Vector2(Position.X - 2f, Position.Y - 2f);
				int width14 = drawPlayer.width + 4;
				int height13 = drawPlayer.height + 4;
				float speedX7 = drawPlayer.velocity.X * 0.4f;
				float speedY7 = drawPlayer.velocity.Y * 0.4f;
				newColor = default;
				Dust dust18 = Dust.NewDustDirect(position17, width14, height13, 135, speedX7, speedY7, 100, newColor, 3f);
				dust18.noGravity = true;
				dust18.velocity *= 1.8f;
				dust18.velocity.Y -= 0.5f;
				DustCache.Add(dust18.dustIndex);
			}
			num13 *= 0.5f;
			num14 *= 0.7f;
		}
		if (drawPlayer.onFire2)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Vector2 position18 = new Vector2(Position.X - 2f, Position.Y - 2f);
				int width15 = drawPlayer.width + 4;
				int height14 = drawPlayer.height + 4;
				float speedX8 = drawPlayer.velocity.X * 0.4f;
				float speedY8 = drawPlayer.velocity.Y * 0.4f;
				newColor = default;
				Dust dust19 = Dust.NewDustDirect(position18, width15, height14, 75, speedX8, speedY8, 100, newColor, 3f);
				dust19.noGravity = true;
				dust19.velocity *= 1.8f;
				dust19.velocity.Y -= 0.5f;
				DustCache.Add(dust19.dustIndex);
			}
			num15 *= 0.6f;
			num14 *= 0.7f;
		}
		if (drawPlayer.noItems)
		{
			num14 *= 0.8f;
			num13 *= 0.65f;
		}
		if (drawPlayer.blind)
		{
			num14 *= 0.65f;
			num13 *= 0.7f;
		}
		if (drawPlayer.bleed)
		{
			num14 *= 0.9f;
			num15 *= 0.9f;
			if (!drawPlayer.dead && Main.rand.Next(20) == 0 && shadow == 0f)
			{
				Vector2 position19 = Position;
				int width16 = drawPlayer.width;
				int height15 = drawPlayer.height;
				newColor = default;
				Dust dust20 = Dust.NewDustDirect(position19, width16, height15, 5, 0f, 0f, 0, newColor);
				dust20.velocity.Y += 0.5f;
				dust20.velocity *= 0.25f;
				DustCache.Add(dust20.dustIndex);
			}
		}
		if (shadow == 0f && drawPlayer.palladiumRegen && drawPlayer.statLife < drawPlayer.statLifeMax2 && FocusHelper.AllowPlayerToEmitEffects && drawPlayer.miscCounter % 10 == 0 && shadow == 0f)
		{
			Vector2 position20 = default;
			position20.X = Position.X + (float)Main.rand.Next(drawPlayer.width);
			position20.Y = Position.Y + (float)Main.rand.Next(drawPlayer.height);
			position20.X = Position.X + (float)(drawPlayer.width / 2) - 6f;
			position20.Y = Position.Y + (float)(drawPlayer.height / 2) - 6f;
			position20.X -= Main.rand.Next(-10, 11);
			position20.Y -= Main.rand.Next(-20, 21);
			int item = Gore.NewGore(position20, new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-20, -10) * 0.1f), 331, (float)Main.rand.Next(80, 120) * 0.01f);
			GoreCache.Add(item);
		}
		if (shadow == 0f && drawPlayer.loveStruck && FocusHelper.AllowPlayerToEmitEffects && Main.rand.Next(5) == 0)
		{
			Vector2 val3 = new Vector2((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
			val3.Normalize();
			val3.X *= 0.66f;
			int num17 = Gore.NewGore(Position + new Vector2((float)Main.rand.Next(drawPlayer.width + 1), (float)Main.rand.Next(drawPlayer.height + 1)), val3 * (float)Main.rand.Next(3, 6) * 0.33f, 331, (float)Main.rand.Next(40, 121) * 0.01f);
			Main.gore[num17].sticky = false;
			Gore obj = Main.gore[num17];
			obj.velocity *= 0.4f;
			Main.gore[num17].velocity.Y -= 0.6f;
			GoreCache.Add(num17);
		}
		if (drawPlayer.stinky && FocusHelper.AllowPlayerToEmitEffects)
		{
			num13 *= 0.7f;
			num15 *= 0.55f;
			if (Main.rand.Next(5) == 0 && shadow == 0f)
			{
				Vector2 val4 = new Vector2((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
				val4.Normalize();
				val4.X *= 0.66f;
				val4.Y = Math.Abs(val4.Y);
				Vector2 val5 = val4 * (float)Main.rand.Next(3, 5) * 0.25f;
				Vector2 position21 = Position;
				int width17 = drawPlayer.width;
				int height16 = drawPlayer.height;
				float x = val5.X;
				float speedY9 = val5.Y * 0.5f;
				newColor = default;
				int num18 = Dust.NewDust(position21, width17, height16, 188, x, speedY9, 100, newColor, 1.5f);
				Dust obj2 = Main.dust[num18];
				obj2.velocity *= 0.1f;
				Main.dust[num18].velocity.Y -= 0.5f;
				DustCache.Add(num18);
			}
		}
		if (drawPlayer.slowOgreSpit && FocusHelper.AllowPlayerToEmitEffects)
		{
			num13 *= 0.6f;
			num15 *= 0.45f;
			if (Main.rand.Next(5) == 0 && shadow == 0f)
			{
				int type = Utils.SelectRandom<int>(Main.rand, 4, 256);
				Dust[] dust21 = Main.dust;
				Vector2 position22 = Position;
				int width18 = drawPlayer.width;
				int height17 = drawPlayer.height;
				newColor = default;
				Dust dust22 = dust21[Dust.NewDust(position22, width18, height17, type, 0f, 0f, 100, newColor)];
				dust22.scale = 0.8f + Main.rand.NextFloat() * 0.6f;
				dust22.fadeIn = 0.5f;
				dust22.velocity *= 0.05f;
				dust22.noLight = true;
				if (dust22.type == 4)
				{
					dust22.color = new Color(80, 170, 40, 120);
				}
				DustCache.Add(dust22.dustIndex);
			}
			if (Main.rand.Next(5) == 0 && shadow == 0f)
			{
				int num19 = Gore.NewGore(Position + new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat()) * drawPlayer.Size, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, 1024, 1025, 1026), 0.65f);
				Gore obj3 = Main.gore[num19];
				obj3.velocity *= 0.05f;
				GoreCache.Add(num19);
			}
		}
		if (FocusHelper.AllowPlayerToEmitEffects && shadow == 0f)
		{
			float num20 = (float)drawPlayer.miscCounter / 180f;
			float num21 = 0f;
			float num22 = 10f;
			int num23 = 90;
			int num24 = 0;
			for (int j = 0; j < 3; j++)
			{
				switch (j)
				{
				case 0:
					if (drawPlayer.nebulaLevelLife < 1)
					{
						continue;
					}
					num21 = (float)Math.PI * 2f / (float)drawPlayer.nebulaLevelLife;
					num24 = drawPlayer.nebulaLevelLife;
					break;
				case 1:
					if (drawPlayer.nebulaLevelMana < 1)
					{
						continue;
					}
					num21 = (float)Math.PI * -2f / (float)drawPlayer.nebulaLevelMana;
					num24 = drawPlayer.nebulaLevelMana;
					num20 = (float)(-drawPlayer.miscCounter) / 180f;
					num22 = 20f;
					num23 = 88;
					break;
				case 2:
					if (drawPlayer.nebulaLevelDamage < 1)
					{
						continue;
					}
					num21 = (float)Math.PI * 2f / (float)drawPlayer.nebulaLevelDamage;
					num24 = drawPlayer.nebulaLevelDamage;
					num20 = (float)drawPlayer.miscCounter / 180f;
					num22 = 30f;
					num23 = 86;
					break;
				}
				for (int k = 0; k < num24; k++)
				{
					Vector2 position23 = Position;
					int width19 = drawPlayer.width;
					int height18 = drawPlayer.height;
					int type2 = num23;
					newColor = default;
					Dust dust23 = Dust.NewDustDirect(position23, width19, height18, type2, 0f, 0f, 100, newColor, 1.5f);
					dust23.noGravity = true;
					dust23.velocity = Vector2.Zero;
					dust23.position = drawPlayer.Center + Vector2.UnitY * drawPlayer.gfxOffY + (num20 * ((float)Math.PI * 2f) + num21 * (float)k).ToRotationVector2() * num22;
					dust23.customData = drawPlayer;
					DustCache.Add(dust23.dustIndex);
				}
			}
		}
		if (drawPlayer.witheredArmor && FocusHelper.AllowPlayerToEmitEffects)
		{
			num14 *= 0.5f;
			num13 *= 0.75f;
		}
		if (drawPlayer.witheredWeapon && drawPlayer.itemAnimation > 0 && heldItem.damage > 0 && FocusHelper.AllowPlayerToEmitEffects && Main.rand.Next(3) == 0)
		{
			Vector2 position24 = new Vector2(Position.X - 2f, Position.Y - 2f);
			int width20 = drawPlayer.width + 4;
			int height19 = drawPlayer.height + 4;
			newColor = default;
			Dust dust24 = Dust.NewDustDirect(position24, width20, height19, 272, 0f, 0f, 50, newColor, 0.5f);
			dust24.velocity *= 1.6f;
			dust24.velocity.Y--;
			dust24.position = Vector2.Lerp(dust24.position, drawPlayer.Center, 0.5f);
			DustCache.Add(dust24.dustIndex);
		}
		_ = drawPlayer.shimmering;
		if (num13 != 1f || num14 != 1f || num15 != 1f || num16 != 1f)
		{
			if (drawPlayer.onFire || drawPlayer.onFire2 || drawPlayer.onFrostBurn || drawPlayer.onFire3 || drawPlayer.onFrostBurn2)
			{
				colorEyeWhites = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorEyes = drawPlayer.GetImmuneAlpha(drawPlayer.eyeColor, shadow);
				colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(useLighting: false), shadow);
				colorHead = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
				colorBodySkin = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
				colorShirt = drawPlayer.GetImmuneAlpha(drawPlayer.shirtColor, shadow);
				colorUnderShirt = drawPlayer.GetImmuneAlpha(drawPlayer.underShirtColor, shadow);
				colorPants = drawPlayer.GetImmuneAlpha(drawPlayer.pantsColor, shadow);
				colorLegs = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
				colorShoes = drawPlayer.GetImmuneAlpha(drawPlayer.shoeColor, shadow);
				colorArmorHead = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorArmorBody = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorArmorLegs = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = drawPlayer.GetImmuneAlpha(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, shadow);
				}
			}
			else
			{
				colorEyeWhites = Main.buffColor(colorEyeWhites, num13, num14, num15, num16);
				colorEyes = Main.buffColor(colorEyes, num13, num14, num15, num16);
				colorHair = Main.buffColor(colorHair, num13, num14, num15, num16);
				colorHead = Main.buffColor(colorHead, num13, num14, num15, num16);
				colorBodySkin = Main.buffColor(colorBodySkin, num13, num14, num15, num16);
				colorShirt = Main.buffColor(colorShirt, num13, num14, num15, num16);
				colorUnderShirt = Main.buffColor(colorUnderShirt, num13, num14, num15, num16);
				colorPants = Main.buffColor(colorPants, num13, num14, num15, num16);
				colorLegs = Main.buffColor(colorLegs, num13, num14, num15, num16);
				colorShoes = Main.buffColor(colorShoes, num13, num14, num15, num16);
				colorArmorHead = Main.buffColor(colorArmorHead, num13, num14, num15, num16);
				colorArmorBody = Main.buffColor(colorArmorBody, num13, num14, num15, num16);
				colorArmorLegs = Main.buffColor(colorArmorLegs, num13, num14, num15, num16);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = Main.buffColor(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, num13, num14, num15, num16);
				}
			}
		}
		if (drawPlayer.socialGhost)
		{
			colorEyeWhites = Color.Transparent;
			colorEyes = Color.Transparent;
			colorHair = Color.Transparent;
			colorHead = Color.Transparent;
			colorBodySkin = Color.Transparent;
			colorShirt = Color.Transparent;
			colorUnderShirt = Color.Transparent;
			colorPants = Color.Transparent;
			colorShoes = Color.Transparent;
			colorLegs = Color.Transparent;
			if (colorArmorHead.A > Main.gFade)
			{
				colorArmorHead.A = Main.gFade;
			}
			if (colorArmorBody.A > Main.gFade)
			{
				colorArmorBody.A = Main.gFade;
			}
			if (colorArmorLegs.A > Main.gFade)
			{
				colorArmorLegs.A = Main.gFade;
			}
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Transparent;
			}
		}
		if (drawPlayer.socialIgnoreLight)
		{
			float num25 = 1f;
			colorEyeWhites = Color.White * num25;
			colorEyes = drawPlayer.eyeColor * num25;
			colorHair = GameShaders.Hair.GetColor(drawPlayer.hairDye, drawPlayer, Color.White);
			colorHead = drawPlayer.skinColor * num25;
			colorBodySkin = drawPlayer.skinColor * num25;
			colorShirt = drawPlayer.shirtColor * num25;
			colorUnderShirt = drawPlayer.underShirtColor * num25;
			colorPants = drawPlayer.pantsColor * num25;
			colorShoes = drawPlayer.shoeColor * num25;
			colorLegs = drawPlayer.skinColor * num25;
			colorArmorHead = Color.White;
			colorArmorBody = Color.White;
			colorArmorLegs = Color.White;
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * num25;
			}
		}
		if (drawPlayer.opacityForAnimation != 1f)
		{
			shadow = 1f - drawPlayer.opacityForAnimation;
			float opacityForAnimation = drawPlayer.opacityForAnimation;
			opacityForAnimation *= opacityForAnimation;
			colorEyeWhites = Color.White * opacityForAnimation;
			colorEyes = drawPlayer.eyeColor * opacityForAnimation;
			colorHair = GameShaders.Hair.GetColor(drawPlayer.hairDye, drawPlayer, Color.White) * opacityForAnimation;
			colorHead = drawPlayer.skinColor * opacityForAnimation;
			colorBodySkin = drawPlayer.skinColor * opacityForAnimation;
			colorShirt = drawPlayer.shirtColor * opacityForAnimation;
			colorUnderShirt = drawPlayer.underShirtColor * opacityForAnimation;
			colorPants = drawPlayer.pantsColor * opacityForAnimation;
			colorShoes = drawPlayer.shoeColor * opacityForAnimation;
			colorLegs = drawPlayer.skinColor * opacityForAnimation;
			colorArmorHead = drawPlayer.GetImmuneAlpha(Color.White, shadow);
			colorArmorBody = drawPlayer.GetImmuneAlpha(Color.White, shadow);
			colorArmorLegs = drawPlayer.GetImmuneAlpha(Color.White, shadow);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * opacityForAnimation;
			}
		}
		stealth = 1f;
		if (heldItem.type == 3106)
		{
			float num26 = drawPlayer.stealth;
			if ((double)num26 < 0.03)
			{
				num26 = 0.03f;
			}
			float num27 = (1f + num26 * 10f) / 11f;
			if (num26 < 0f)
			{
				num26 = 0f;
			}
			if (!(num26 < 1f - shadow) && shadow > 0f)
			{
				num26 = shadow * 0.5f;
			}
			stealth = num27;
			colorArmorHead = new Color((int)(byte)((float)(int)colorArmorHead.R * num26), (int)(byte)((float)(int)colorArmorHead.G * num26), (int)(byte)((float)(int)colorArmorHead.B * num27), (int)(byte)((float)(int)colorArmorHead.A * num26));
			colorArmorBody = new Color((int)(byte)((float)(int)colorArmorBody.R * num26), (int)(byte)((float)(int)colorArmorBody.G * num26), (int)(byte)((float)(int)colorArmorBody.B * num27), (int)(byte)((float)(int)colorArmorBody.A * num26));
			colorArmorLegs = new Color((int)(byte)((float)(int)colorArmorLegs.R * num26), (int)(byte)((float)(int)colorArmorLegs.G * num26), (int)(byte)((float)(int)colorArmorLegs.B * num27), (int)(byte)((float)(int)colorArmorLegs.A * num26));
			num26 *= num26;
			colorEyeWhites = Color.Multiply(colorEyeWhites, num26);
			colorEyes = Color.Multiply(colorEyes, num26);
			colorHair = Color.Multiply(colorHair, num26);
			colorHead = Color.Multiply(colorHead, num26);
			colorBodySkin = Color.Multiply(colorBodySkin, num26);
			colorShirt = Color.Multiply(colorShirt, num26);
			colorUnderShirt = Color.Multiply(colorUnderShirt, num26);
			colorPants = Color.Multiply(colorPants, num26);
			colorShoes = Color.Multiply(colorShoes, num26);
			colorLegs = Color.Multiply(colorLegs, num26);
			colorMount = Color.Multiply(colorMount, num26);
			floatingTubeColor = Color.Multiply(floatingTubeColor, num26);
			headGlowColor = Color.Multiply(headGlowColor, num26);
			bodyGlowColor = Color.Multiply(bodyGlowColor, num26);
			armGlowColor = Color.Multiply(armGlowColor, num26);
			legsGlowColor = Color.Multiply(legsGlowColor, num26);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num26);
			}
		}
		else if (drawPlayer.shroomiteStealth)
		{
			float num28 = drawPlayer.stealth;
			if ((double)num28 < 0.03)
			{
				num28 = 0.03f;
			}
			float num29 = (1f + num28 * 10f) / 11f;
			if (num28 < 0f)
			{
				num28 = 0f;
			}
			if (!(num28 < 1f - shadow) && shadow > 0f)
			{
				num28 = shadow * 0.5f;
			}
			stealth = num29;
			colorArmorHead = new Color((int)(byte)((float)(int)colorArmorHead.R * num28), (int)(byte)((float)(int)colorArmorHead.G * num28), (int)(byte)((float)(int)colorArmorHead.B * num29), (int)(byte)((float)(int)colorArmorHead.A * num28));
			colorArmorBody = new Color((int)(byte)((float)(int)colorArmorBody.R * num28), (int)(byte)((float)(int)colorArmorBody.G * num28), (int)(byte)((float)(int)colorArmorBody.B * num29), (int)(byte)((float)(int)colorArmorBody.A * num28));
			colorArmorLegs = new Color((int)(byte)((float)(int)colorArmorLegs.R * num28), (int)(byte)((float)(int)colorArmorLegs.G * num28), (int)(byte)((float)(int)colorArmorLegs.B * num29), (int)(byte)((float)(int)colorArmorLegs.A * num28));
			num28 *= num28;
			colorEyeWhites = Color.Multiply(colorEyeWhites, num28);
			colorEyes = Color.Multiply(colorEyes, num28);
			colorHair = Color.Multiply(colorHair, num28);
			colorHead = Color.Multiply(colorHead, num28);
			colorBodySkin = Color.Multiply(colorBodySkin, num28);
			colorShirt = Color.Multiply(colorShirt, num28);
			colorUnderShirt = Color.Multiply(colorUnderShirt, num28);
			colorPants = Color.Multiply(colorPants, num28);
			colorShoes = Color.Multiply(colorShoes, num28);
			colorLegs = Color.Multiply(colorLegs, num28);
			colorMount = Color.Multiply(colorMount, num28);
			floatingTubeColor = Color.Multiply(floatingTubeColor, num28);
			headGlowColor = Color.Multiply(headGlowColor, num28);
			bodyGlowColor = Color.Multiply(bodyGlowColor, num28);
			armGlowColor = Color.Multiply(armGlowColor, num28);
			legsGlowColor = Color.Multiply(legsGlowColor, num28);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num28);
			}
		}
		else if (drawPlayer.setVortex)
		{
			float num30 = drawPlayer.stealth;
			if ((double)num30 < 0.03)
			{
				num30 = 0.03f;
			}
			if (num30 < 0f)
			{
				num30 = 0f;
			}
			if (!(num30 < 1f - shadow) && shadow > 0f)
			{
				num30 = shadow * 0.5f;
			}
			stealth = num30;
			Color secondColor = new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num30));
			colorArmorHead = colorArmorHead.MultiplyRGBA(secondColor);
			colorArmorBody = colorArmorBody.MultiplyRGBA(secondColor);
			colorArmorLegs = colorArmorLegs.MultiplyRGBA(secondColor);
			num30 *= num30;
			colorEyeWhites = Color.Multiply(colorEyeWhites, num30);
			colorEyes = Color.Multiply(colorEyes, num30);
			colorHair = Color.Multiply(colorHair, num30);
			colorHead = Color.Multiply(colorHead, num30);
			colorBodySkin = Color.Multiply(colorBodySkin, num30);
			colorShirt = Color.Multiply(colorShirt, num30);
			colorUnderShirt = Color.Multiply(colorUnderShirt, num30);
			colorPants = Color.Multiply(colorPants, num30);
			colorShoes = Color.Multiply(colorShoes, num30);
			colorLegs = Color.Multiply(colorLegs, num30);
			colorMount = Color.Multiply(colorMount, num30);
			floatingTubeColor = Color.Multiply(floatingTubeColor, num30);
			headGlowColor = Color.Multiply(headGlowColor, num30);
			bodyGlowColor = Color.Multiply(bodyGlowColor, num30);
			armGlowColor = Color.Multiply(armGlowColor, num30);
			legsGlowColor = Color.Multiply(legsGlowColor, num30);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num30);
			}
		}
		if (hideEntirePlayerExceptHelmetsAndFaceAccessories)
		{
			hideHair = true;
			colorDisplayDollSkin = (legsGlowColor = (armGlowColor = (bodyGlowColor = (colorLegs = (colorShoes = (colorPants = (colorUnderShirt = (colorShirt = (colorBodySkin = (colorEyes = (colorEyeWhites = (colorArmorLegs = (colorArmorBody = Color.Transparent)))))))))))));
		}
		if (hideEntirePlayer)
		{
			stealth = 1f;
			colorDisplayDollSkin = (legsGlowColor = (armGlowColor = (bodyGlowColor = (headGlowColor = (colorLegs = (colorShoes = (colorPants = (colorUnderShirt = (colorShirt = (colorBodySkin = (colorHead = (colorHair = (colorEyes = (colorEyeWhites = (colorArmorLegs = (colorArmorBody = (colorArmorHead = Color.Transparent)))))))))))))))));
		}
		if (drawPlayer.gravDir == 1f)
		{
			if (drawPlayer.direction == 1)
			{
				playerEffect = (SpriteEffects)0;
				itemEffect = (SpriteEffects)0;
			}
			else
			{
				playerEffect = (SpriteEffects)1;
				itemEffect = (SpriteEffects)1;
			}
			if (!drawPlayer.dead)
			{
				drawPlayer.legPosition.Y = 0f;
				drawPlayer.headPosition.Y = 0f;
				drawPlayer.bodyPosition.Y = 0f;
			}
		}
		else
		{
			if (drawPlayer.direction == 1)
			{
				playerEffect = (SpriteEffects)2;
				itemEffect = (SpriteEffects)2;
			}
			else
			{
				playerEffect = (SpriteEffects)3;
				itemEffect = (SpriteEffects)3;
			}
			if (!drawPlayer.dead)
			{
				drawPlayer.legPosition.Y = 6f;
				drawPlayer.headPosition.Y = 6f;
				drawPlayer.bodyPosition.Y = 6f;
			}
		}
		switch (heldItem.type)
		{
		case 4343:
		case 4344:
			itemEffect = (SpriteEffects)(itemEffect ^ (SpriteEffects)1);
			break;
		case 3182:
		case 3184:
		case 3185:
		case 3782:
			itemEffect = (SpriteEffects)(itemEffect ^ (SpriteEffects)3);
			break;
		case 5118:
			if (player.gravDir < 0f)
			{
				itemEffect = (SpriteEffects)(itemEffect ^ (SpriteEffects)3);
			}
			break;
		}
		legVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.75f);
		bodyVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
		headVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
		if ((drawPlayer.merman || drawPlayer.forceMerman) && !drawPlayer.hideMerman)
		{
			drawPlayer.headRotation = drawPlayer.velocity.Y * (float)drawPlayer.direction * 0.1f;
			if ((double)drawPlayer.headRotation < -0.3)
			{
				drawPlayer.headRotation = -0.3f;
			}
			if ((double)drawPlayer.headRotation > 0.3)
			{
				drawPlayer.headRotation = 0.3f;
			}
		}
		else if (!drawPlayer.dead)
		{
			drawPlayer.headRotation = 0f;
		}
		Rectangle bodyFrame = drawPlayer.bodyFrame;
		bodyFrame = drawPlayer.bodyFrame;
		bodyFrame.Height -= 2;
		bodyFrame.Y -= 336;
		if (bodyFrame.Y < 0)
		{
			bodyFrame.Y = 0;
		}
		hairFrontFrame = bodyFrame;
		hairBackFrame = bodyFrame;
		if (hideHair)
		{
			hairFrontFrame.Height = 0;
			hairBackFrame.Height = 0;
		}
		else if (backHairDraw)
		{
			int height20 = 26;
			hairFrontFrame.Height = height20;
		}
		hidesTopSkin = drawPlayer.body == 82 || drawPlayer.body == 83 || drawPlayer.body == 93 || drawPlayer.body == 21 || drawPlayer.body == 22;
		hidesBottomSkin = drawPlayer.body == 93 || drawPlayer.legs == 20 || drawPlayer.legs == 21 || drawPlayer.legs == 216 || drawPlayer.legs == 214 || drawPlayer.legs == 215;
		drawFloatingTube = drawPlayer.hasFloatingTube && !hideEntirePlayer && !hideEntirePlayerExceptHelmetsAndFaceAccessories;
		drawUnicornHorn = drawPlayer.hasUnicornHorn;
		drawAngelHalo = drawPlayer.hasAngelHalo;
		drawFrontAccInNeckAccLayer = false;
		if (drawPlayer.front > 0 && drawPlayer.front < ArmorIDs.Front.Count)
		{
			if (ArmorIDs.Front.Sets.DrawsInNeckLayerRegardlessOfPlayerFrame[drawPlayer.front])
			{
				drawFrontAccInNeckAccLayer = true;
			}
			else if (drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height == 5 && ArmorIDs.Front.Sets.DrawsInNeckLayer[drawPlayer.front])
			{
				drawFrontAccInNeckAccLayer = true;
			}
		}
		mountHandlesHeadDraw = false;
		mountDrawsEyelid = false;
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 54)
		{
			mountHandlesHeadDraw = true;
			mountDrawsEyelid = true;
		}
		hairOffset = drawPlayer.GetHairDrawOffset(drawPlayer.hair, hatHair);
		helmetOffset = drawPlayer.GetHelmetDrawOffset();
		legsOffset = drawPlayer.GetLegsDrawOffset();
		CreateCompositeData();
	}

	private void AdjustmentsForWolfMount()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		hideEntirePlayer = true;
		weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
		Vector2 val = new Vector2((float)(10 + drawPlayer.direction * 14), 12f);
		Vector2 val2 = Position + val;
		VisualPositionOffset = new Vector2(-10f, 0f) * drawPlayer.Directions;
		Position += VisualPositionOffset;
		bool flag = heldItem.useStyle == 5 || SelectedDrawnProjectile != null;
		bool flag2 = heldItem.useStyle == 2;
		bool flag3 = heldItem.useStyle == 9;
		bool flag4 = drawPlayer.itemAnimation > 0;
		bool flag5 = heldItem.fishingPole != 0;
		bool flag6 = heldItem.useStyle == 14;
		bool flag7 = heldItem.useStyle == 8;
		bool flag8 = heldItem.holdStyle == 1;
		bool flag9 = heldItem.holdStyle == 2;
		bool flag10 = heldItem.holdStyle == 5;
		if (flag2)
		{
			ItemLocation += new Vector2((float)(drawPlayer.direction * 14), -4f);
		}
		else if (!flag5)
		{
			if (flag3)
			{
				ItemLocation += (flag4 ? new Vector2((float)(drawPlayer.direction * 18), -4f) : new Vector2((float)(drawPlayer.direction * 14), -18f));
			}
			else if (flag10)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 17), -8f);
			}
			else if (flag8 && drawPlayer.itemAnimation == 0)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 14), -6f);
			}
			else if (flag9 && drawPlayer.itemAnimation == 0)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 17), 4f);
			}
			else if (flag7)
			{
				ItemLocation = val2 + new Vector2((float)(drawPlayer.direction * 12), 2f);
			}
			else if (flag6)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 5), -2f);
			}
			else if (flag)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 4), -4f);
			}
			else
			{
				ItemLocation = val2;
			}
		}
	}

	private void AdjustmentsForVelociraptorMount()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		hideEntirePlayerExceptHelmetsAndFaceAccessories = true;
		weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
		VisualPositionOffset = new Vector2(-14f, 0f) * drawPlayer.Directions;
		Position += VisualPositionOffset;
		bool flag = drawPlayer.itemAnimation > 0;
		if ((heldItem.useStyle == 8) & flag)
		{
			weaponDrawOrder = WeaponDrawOrder.OverFrontArm;
		}
		drawPlayer.ApplyItemPositionOffsetFromMount(ref ItemLocation);
	}

	private void AdjustmentsForRatMount()
	{
		hideEntirePlayer = true;
		weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
	}

	private void AdjustmentsForBatMount()
	{
		hideEntirePlayer = true;
		weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
	}

	private void AdjustmentsForPixieMount()
	{
		hideEntirePlayer = true;
		weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
	}

	private void CreateCompositeData()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		frontShoulderOffset = Vector2.Zero;
		backShoulderOffset = Vector2.Zero;
		usesCompositeTorso = drawPlayer.body > 0 && drawPlayer.body < ArmorIDs.Body.Count && ArmorIDs.Body.Sets.UsesNewFramingCode[drawPlayer.body];
		usesCompositeFrontHandAcc = drawPlayer.handon > 0 && drawPlayer.handon < ArmorIDs.HandOn.Count && ArmorIDs.HandOn.Sets.UsesNewFramingCode[drawPlayer.handon];
		usesCompositeBackHandAcc = drawPlayer.handoff > 0 && drawPlayer.handoff < ArmorIDs.HandOff.Count && ArmorIDs.HandOff.Sets.UsesNewFramingCode[drawPlayer.handoff];
		if (drawPlayer.body < 1)
		{
			usesCompositeTorso = true;
		}
		if (!usesCompositeTorso)
		{
			return;
		}
		Point pt = new Point(1, 1);
		Point pt2 = new Point(0, 1);
		Point pt3 = default;
		Point frameIndex = default;
		Point frameIndex2 = default;
		int num = drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height;
		compShoulderOverFrontArm = true;
		hideCompositeShoulders = false;
		bool flag = true;
		if (drawPlayer.body > 0)
		{
			flag = ArmorIDs.Body.Sets.showsShouldersWhileJumping[drawPlayer.body];
		}
		if (drawPlayer.coat > 0)
		{
			hideCompositeShoulders = true;
		}
		if (drawPlayer.front > 0 && ArmorIDs.Front.Sets.HidesCompositeShoulders[drawPlayer.front])
		{
			hideCompositeShoulders = true;
		}
		bool flag2 = false;
		if (drawPlayer.handon > 0)
		{
			flag2 = ArmorIDs.HandOn.Sets.UsesOldFramingTexturesForWalking[drawPlayer.handon];
		}
		bool flag3 = !flag2;
		switch (num)
		{
		case 0:
			frameIndex2.X = 2;
			flag3 = true;
			break;
		case 1:
			frameIndex2.X = 3;
			compShoulderOverFrontArm = false;
			flag3 = true;
			break;
		case 2:
			frameIndex2.X = 4;
			compShoulderOverFrontArm = false;
			flag3 = true;
			break;
		case 3:
			frameIndex2.X = 5;
			compShoulderOverFrontArm = true;
			flag3 = true;
			break;
		case 4:
			frameIndex2.X = 6;
			compShoulderOverFrontArm = true;
			flag3 = true;
			break;
		case 5:
			frameIndex2.X = 2;
			frameIndex2.Y = 1;
			pt3.X = 1;
			compShoulderOverFrontArm = false;
			flag3 = true;
			if (!flag)
			{
				hideCompositeShoulders = true;
			}
			break;
		case 6:
			frameIndex2.X = 3;
			frameIndex2.Y = 1;
			break;
		case 7:
		case 8:
		case 9:
		case 10:
			frameIndex2.X = 4;
			frameIndex2.Y = 1;
			break;
		case 11:
		case 12:
		case 13:
			frameIndex2.X = 3;
			frameIndex2.Y = 1;
			break;
		case 14:
			frameIndex2.X = 5;
			frameIndex2.Y = 1;
			break;
		case 15:
		case 16:
			frameIndex2.X = 6;
			frameIndex2.Y = 1;
			break;
		case 17:
			frameIndex2.X = 5;
			frameIndex2.Y = 1;
			break;
		case 18:
		case 19:
			frameIndex2.X = 3;
			frameIndex2.Y = 1;
			break;
		}
		CreateCompositeData_DetermineShoulderOffsets(drawPlayer.body, num);
		backShoulderOffset *= new Vector2((float)drawPlayer.direction, drawPlayer.gravDir);
		frontShoulderOffset *= new Vector2((float)drawPlayer.direction, drawPlayer.gravDir);
		if (drawPlayer.body > 0 && ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[drawPlayer.body])
		{
			compShoulderOverFrontArm = false;
		}
		usesCompositeFrontHandAcc = flag3;
		frameIndex.X = frameIndex2.X;
		frameIndex.Y = frameIndex2.Y + 2;
		UpdateCompositeArm(drawPlayer.compositeFrontArm, ref compositeFrontArmRotation, ref frameIndex2, 7);
		UpdateCompositeArm(drawPlayer.compositeBackArm, ref compositeBackArmRotation, ref frameIndex, 8);
		if (!drawPlayer.Male)
		{
			pt.Y += 2;
			pt2.Y += 2;
			pt3.Y += 2;
		}
		compBackShoulderFrame = CreateCompositeFrameRect(pt);
		compFrontShoulderFrame = CreateCompositeFrameRect(pt2);
		compBackArmFrame = CreateCompositeFrameRect(frameIndex);
		compFrontArmFrame = CreateCompositeFrameRect(frameIndex2);
		compTorsoFrame = CreateCompositeFrameRect(pt3);
	}

	private void CreateCompositeData_DetermineShoulderOffsets(int armor, int targetFrameNumber)
	{
		int num = 0;
		switch (armor)
		{
		case 55:
			num = 1;
			break;
		case 71:
			num = 2;
			break;
		case 204:
			num = 3;
			break;
		case 183:
			num = 4;
			break;
		case 201:
			num = 5;
			break;
		case 101:
			num = 6;
			break;
		case 207:
			num = 7;
			break;
		}
		switch (num)
		{
		case 1:
			switch (targetFrameNumber)
			{
			case 6:
				frontShoulderOffset.X = -2f;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -4f;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
				frontShoulderOffset.X = -2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				break;
			}
			break;
		case 2:
			switch (targetFrameNumber)
			{
			case 6:
				frontShoulderOffset.X = -2f;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -4f;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
				frontShoulderOffset.X = -2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				break;
			}
			break;
		case 3:
			switch (targetFrameNumber)
			{
			case 7:
			case 8:
			case 9:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				frontShoulderOffset.X = 2f;
				break;
			}
			break;
		case 4:
			switch (targetFrameNumber)
			{
			case 6:
				frontShoulderOffset.X = -2f;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -4f;
				break;
			case 11:
			case 12:
			case 13:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
				frontShoulderOffset.X = 2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 14:
			case 17:
				break;
			}
			break;
		case 5:
			switch (targetFrameNumber)
			{
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
				frontShoulderOffset.X = 2f;
				break;
			}
			break;
		case 6:
			switch (targetFrameNumber)
			{
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -2f;
				break;
			case 14:
			case 15:
			case 16:
			case 17:
				frontShoulderOffset.X = 2f;
				break;
			}
			break;
		case 7:
			switch (targetFrameNumber)
			{
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -2f;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
				frontShoulderOffset.X = -2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				break;
			}
			break;
		}
	}

	private Rectangle CreateCompositeFrameRect(Point pt)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return new Rectangle(pt.X * 40, pt.Y * 56, 40, 56);
	}

	private void UpdateCompositeArm(Player.CompositeArmData data, ref float rotation, ref Point frameIndex, int targetX)
	{
		if (data.enabled)
		{
			rotation = data.rotation;
			switch (data.stretch)
			{
			case Player.CompositeArmStretchAmount.Full:
				frameIndex.X = targetX;
				frameIndex.Y = 0;
				break;
			case Player.CompositeArmStretchAmount.ThreeQuarters:
				frameIndex.X = targetX;
				frameIndex.Y = 1;
				break;
			case Player.CompositeArmStretchAmount.Quarter:
				frameIndex.X = targetX;
				frameIndex.Y = 2;
				break;
			case Player.CompositeArmStretchAmount.None:
				frameIndex.X = targetX;
				frameIndex.Y = 3;
				break;
			}
		}
		else
		{
			rotation = 0f;
		}
	}
}
