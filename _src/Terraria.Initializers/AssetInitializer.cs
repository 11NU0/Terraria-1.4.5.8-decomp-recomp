using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Graphics;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Initializers;

public static class AssetInitializer
{
	public static void CreateAssetServices(GameServiceContainer services)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected Obj, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected Obj, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected Obj, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected Obj, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected Obj, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected Obj, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected Obj, but got Unknown
		AssetReaderCollection val = new AssetReaderCollection();
		val.RegisterReader((IAssetReader)new PngReader(XnaExtensions.Get<IGraphicsDeviceService>((IServiceProvider)services).GraphicsDevice), new string[1] { ".png" });
		val.RegisterReader((IAssetReader)new XnbReader((IServiceProvider)services), new string[1] { ".xnb" });
		val.RegisterReader((IAssetReader)(object)new FxReader(XnaExtensions.Get<IGraphicsDeviceService>((IServiceProvider)services).GraphicsDevice), new string[1] { ".fx" });
		AsyncAssetLoader val2 = new AsyncAssetLoader(val, 20);
		val2.RequireTypeCreationOnTransfer(typeof(Texture2D));
		val2.RequireTypeCreationOnTransfer(typeof(DynamicSpriteFont));
		val2.RequireTypeCreationOnTransfer(typeof(SpriteFont));
		IAssetRepository val3 = (IAssetRepository)new AssetRepository((IAssetLoader)new AssetLoader(val), (IAsyncAssetLoader)(object)val2);
		val3.AssetValueUpdatedHandler = (AssetValueUpdated)Delegate.Combine((Delegate)(object)val3.AssetValueUpdatedHandler, (Delegate)new AssetValueUpdated(TagAsset));
		services.AddService(typeof(AssetReaderCollection), (object)val);
		services.AddService(typeof(IAssetRepository), (object)val3);
	}

	private static void TagAsset(IAsset asset, object value)
	{
		GraphicsResource val = (GraphicsResource)((value is GraphicsResource) ? value : null);
		if (val != null)
		{
			val.Name = asset.Name;
			val.Tag = asset;
		}
	}

	public static ResourcePackList CreateResourcePackList(IServiceProvider services)
	{
		GetResourcePacksFolderPathAndConfirmItExists(out var resourcePackJson, out var resourcePackFolder);
		return ResourcePackList.FromJson(resourcePackJson, services, resourcePackFolder);
	}

	public static ResourcePackList CreatePublishableResourcePacksList(IServiceProvider services)
	{
		GetResourcePacksFolderPathAndConfirmItExists(out var resourcePackJson, out var resourcePackFolder);
		return ResourcePackList.Publishable(resourcePackJson, services, resourcePackFolder);
	}

	public static void GetResourcePacksFolderPathAndConfirmItExists(out JArray resourcePackJson, out string resourcePackFolder)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected Obj, but got Unknown
		resourcePackJson = Main.Configuration.Get<JArray>("ResourcePacks", new JArray());
		resourcePackFolder = Path.Combine(Main.SavePath, "ResourcePacks");
		Utils.TryCreatingDirectory(resourcePackFolder);
	}

	public static void LoadSplashAssets()
	{
		TextureAssets.SplashTexture16x9 = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_1", (AssetRequestMode)1);
		TextureAssets.SplashTexture4x3 = LoadAsset<Texture2D>("Images\\logo_" + new UnifiedRandom().Next(1, 9), (AssetRequestMode)1);
		TextureAssets.SplashTextureLegoResonanace = LoadAsset<Texture2D>("Images\\SplashScreens\\ResonanceArray", (AssetRequestMode)1);
		int num = new UnifiedRandom().Next(1, 11);
		TextureAssets.SplashTextureLegoBack = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_0", (AssetRequestMode)1);
		TextureAssets.SplashTextureLegoTree = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_1", (AssetRequestMode)1);
		TextureAssets.SplashTextureLegoFront = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_2", (AssetRequestMode)1);
		TextureAssets.Item[75] = LoadAsset<Texture2D>("Images\\Item_" + (short)75, (AssetRequestMode)1);
		TextureAssets.LoadingSunflower = LoadAsset<Texture2D>("Images\\UI\\Sunflower_Loading", (AssetRequestMode)1);
	}

	public static void Load(bool asyncLoad)
	{
		int num = ((!asyncLoad) ? 1 : 2);
		LoadTextures((AssetRequestMode)num);
		LoadRenderTargetAssets((AssetRequestMode)num);
		LoadSounds((AssetRequestMode)num);
		LoadFonts((AssetRequestMode)num);
	}

	private static void LoadFonts(AssetRequestMode mode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		FontAssets.ItemStack = LoadAsset<DynamicSpriteFont>("Fonts/Item_Stack", mode);
		FontAssets.MouseText = LoadAsset<DynamicSpriteFont>("Fonts/Mouse_Text", mode);
		FontAssets.DeathText = LoadAsset<DynamicSpriteFont>("Fonts/Death_Text", mode);
		FontAssets.CombatText[0] = LoadAsset<DynamicSpriteFont>("Fonts/Combat_Text", mode);
		FontAssets.CombatText[1] = LoadAsset<DynamicSpriteFont>("Fonts/Combat_Crit", mode);
	}

	private static void LoadSounds(AssetRequestMode mode)
	{
		SoundEngine.Load((IServiceProvider)((Game)Main.instance).Services);
	}

	private static void LoadRenderTargetAssets(AssetRequestMode mode)
	{
		RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerRainbowWings = new PlayerRainbowWingsTextureContent());
		RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerTitaniumStormBuff = new PlayerTitaniumStormBuffTextureContent());
		RegisterRenderTargetAsset(TextureAssets.RenderTargets.QueenSlimeMount = new PlayerQueenSlimeMountTextureContent());
	}

	private static void RegisterRenderTargetAsset(INeedRenderTargetContent content)
	{
		Main.ContentThatNeedsRenderTargets.Add(content);
	}

	private static void LoadTextures(AssetRequestMode mode)
	{
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Unknown result type (might be due to invalid IL or missing references)
		//IL_104a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1067: Unknown result type (might be due to invalid IL or missing references)
		//IL_1077: Unknown result type (might be due to invalid IL or missing references)
		//IL_1087: Unknown result type (might be due to invalid IL or missing references)
		//IL_1097: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10df: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1103: Unknown result type (might be due to invalid IL or missing references)
		//IL_110f: Unknown result type (might be due to invalid IL or missing references)
		//IL_111f: Unknown result type (might be due to invalid IL or missing references)
		//IL_112f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_1157: Unknown result type (might be due to invalid IL or missing references)
		//IL_1169: Unknown result type (might be due to invalid IL or missing references)
		//IL_117b: Unknown result type (might be due to invalid IL or missing references)
		//IL_118d: Unknown result type (might be due to invalid IL or missing references)
		//IL_119f: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_120b: Unknown result type (might be due to invalid IL or missing references)
		//IL_121d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1241: Unknown result type (might be due to invalid IL or missing references)
		//IL_1276: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1324: Unknown result type (might be due to invalid IL or missing references)
		//IL_1336: Unknown result type (might be due to invalid IL or missing references)
		//IL_1348: Unknown result type (might be due to invalid IL or missing references)
		//IL_135a: Unknown result type (might be due to invalid IL or missing references)
		//IL_136c: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Unknown result type (might be due to invalid IL or missing references)
		//IL_138a: Unknown result type (might be due to invalid IL or missing references)
		//IL_139a: Unknown result type (might be due to invalid IL or missing references)
		//IL_13aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_13da: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_140a: Unknown result type (might be due to invalid IL or missing references)
		//IL_141a: Unknown result type (might be due to invalid IL or missing references)
		//IL_142a: Unknown result type (might be due to invalid IL or missing references)
		//IL_143a: Unknown result type (might be due to invalid IL or missing references)
		//IL_144a: Unknown result type (might be due to invalid IL or missing references)
		//IL_145a: Unknown result type (might be due to invalid IL or missing references)
		//IL_146a: Unknown result type (might be due to invalid IL or missing references)
		//IL_147a: Unknown result type (might be due to invalid IL or missing references)
		//IL_148a: Unknown result type (might be due to invalid IL or missing references)
		//IL_149a: Unknown result type (might be due to invalid IL or missing references)
		//IL_14aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_150f: Unknown result type (might be due to invalid IL or missing references)
		//IL_151f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1535: Unknown result type (might be due to invalid IL or missing references)
		//IL_1547: Unknown result type (might be due to invalid IL or missing references)
		//IL_1559: Unknown result type (might be due to invalid IL or missing references)
		//IL_156b: Unknown result type (might be due to invalid IL or missing references)
		//IL_157d: Unknown result type (might be due to invalid IL or missing references)
		//IL_158f: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_162a: Unknown result type (might be due to invalid IL or missing references)
		//IL_165f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1694: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1701: Unknown result type (might be due to invalid IL or missing references)
		//IL_1711: Unknown result type (might be due to invalid IL or missing references)
		//IL_1721: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1741: Unknown result type (might be due to invalid IL or missing references)
		//IL_1751: Unknown result type (might be due to invalid IL or missing references)
		//IL_1761: Unknown result type (might be due to invalid IL or missing references)
		//IL_1771: Unknown result type (might be due to invalid IL or missing references)
		//IL_1781: Unknown result type (might be due to invalid IL or missing references)
		//IL_1791: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1801: Unknown result type (might be due to invalid IL or missing references)
		//IL_1811: Unknown result type (might be due to invalid IL or missing references)
		//IL_1821: Unknown result type (might be due to invalid IL or missing references)
		//IL_1831: Unknown result type (might be due to invalid IL or missing references)
		//IL_1841: Unknown result type (might be due to invalid IL or missing references)
		//IL_1851: Unknown result type (might be due to invalid IL or missing references)
		//IL_1861: Unknown result type (might be due to invalid IL or missing references)
		//IL_1871: Unknown result type (might be due to invalid IL or missing references)
		//IL_1881: Unknown result type (might be due to invalid IL or missing references)
		//IL_1891: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1906: Unknown result type (might be due to invalid IL or missing references)
		//IL_1916: Unknown result type (might be due to invalid IL or missing references)
		//IL_1926: Unknown result type (might be due to invalid IL or missing references)
		//IL_1936: Unknown result type (might be due to invalid IL or missing references)
		//IL_1946: Unknown result type (might be due to invalid IL or missing references)
		//IL_1956: Unknown result type (might be due to invalid IL or missing references)
		//IL_1966: Unknown result type (might be due to invalid IL or missing references)
		//IL_1976: Unknown result type (might be due to invalid IL or missing references)
		//IL_1986: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfe: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < TextureAssets.Item.Length; i++)
		{
			int num = ItemID.Sets.TextureCopyLoad[i];
			if (num != -1)
			{
				TextureAssets.Item[i] = TextureAssets.Item[num];
			}
			else
			{
				TextureAssets.Item[i] = LoadAsset<Texture2D>("Images/Item_" + i, (AssetRequestMode)0);
			}
		}
		for (int j = 0; j < TextureAssets.Npc.Length; j++)
		{
			TextureAssets.Npc[j] = LoadAsset<Texture2D>("Images/NPC_" + j, (AssetRequestMode)0);
		}
		for (int k = 0; k < TextureAssets.Projectile.Length; k++)
		{
			TextureAssets.Projectile[k] = LoadAsset<Texture2D>("Images/Projectile_" + k, (AssetRequestMode)0);
		}
		for (int l = 0; l < TextureAssets.Gore.Length; l++)
		{
			TextureAssets.Gore[l] = LoadAsset<Texture2D>("Images/Gore_" + l, (AssetRequestMode)0);
		}
		for (int m = 0; m < TextureAssets.Wall.Length; m++)
		{
			TextureAssets.Wall[m] = LoadAsset<Texture2D>("Images/Wall_" + m, (AssetRequestMode)0);
		}
		for (int n = 0; n < TextureAssets.Tile.Length; n++)
		{
			TextureAssets.Tile[n] = LoadAsset<Texture2D>("Images/Tiles_" + n, (AssetRequestMode)0);
		}
		for (int num2 = 0; num2 < TextureAssets.ItemFlame.Length; num2++)
		{
			TextureAssets.ItemFlame[num2] = LoadAsset<Texture2D>("Images/ItemFlame_" + num2, (AssetRequestMode)0);
		}
		for (int num3 = 0; num3 < TextureAssets.Wings.Length; num3++)
		{
			TextureAssets.Wings[num3] = LoadAsset<Texture2D>("Images/Wings_" + num3, (AssetRequestMode)0);
		}
		for (int num4 = 0; num4 < TextureAssets.PlayerHair.Length; num4++)
		{
			TextureAssets.PlayerHair[num4] = LoadAsset<Texture2D>("Images/Player_Hair_" + (num4 + 1), (AssetRequestMode)0);
		}
		for (int num5 = 0; num5 < TextureAssets.PlayerHairAlt.Length; num5++)
		{
			TextureAssets.PlayerHairAlt[num5] = LoadAsset<Texture2D>("Images/Player_HairAlt_" + (num5 + 1), (AssetRequestMode)0);
		}
		for (int num6 = 0; num6 < TextureAssets.ArmorHead.Length; num6++)
		{
			TextureAssets.ArmorHead[num6] = LoadAsset<Texture2D>("Images/Armor_Head_" + num6, (AssetRequestMode)0);
		}
		for (int num7 = 0; num7 < TextureAssets.FemaleBody.Length; num7++)
		{
			TextureAssets.FemaleBody[num7] = LoadAsset<Texture2D>("Images/Female_Body_" + num7, (AssetRequestMode)0);
		}
		for (int num8 = 0; num8 < TextureAssets.ArmorBody.Length; num8++)
		{
			TextureAssets.ArmorBody[num8] = LoadAsset<Texture2D>("Images/Armor_Body_" + num8, (AssetRequestMode)0);
		}
		for (int num9 = 0; num9 < TextureAssets.ArmorBodyComposite.Length; num9++)
		{
			TextureAssets.ArmorBodyComposite[num9] = LoadAsset<Texture2D>("Images/Armor/Armor_" + num9, (AssetRequestMode)0);
		}
		for (int num10 = 0; num10 < TextureAssets.ArmorArm.Length; num10++)
		{
			TextureAssets.ArmorArm[num10] = LoadAsset<Texture2D>("Images/Armor_Arm_" + num10, (AssetRequestMode)0);
		}
		for (int num11 = 0; num11 < TextureAssets.ArmorLeg.Length; num11++)
		{
			TextureAssets.ArmorLeg[num11] = LoadAsset<Texture2D>("Images/Armor_Legs_" + num11, (AssetRequestMode)0);
		}
		for (int num12 = 0; num12 < TextureAssets.AccHandsOn.Length; num12++)
		{
			TextureAssets.AccHandsOn[num12] = LoadAsset<Texture2D>("Images/Acc_HandsOn_" + num12, (AssetRequestMode)0);
		}
		for (int num13 = 0; num13 < TextureAssets.AccHandsOff.Length; num13++)
		{
			TextureAssets.AccHandsOff[num13] = LoadAsset<Texture2D>("Images/Acc_HandsOff_" + num13, (AssetRequestMode)0);
		}
		for (int num14 = 0; num14 < TextureAssets.AccHandsOnComposite.Length; num14++)
		{
			TextureAssets.AccHandsOnComposite[num14] = LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOn_" + num14, (AssetRequestMode)0);
		}
		for (int num15 = 0; num15 < TextureAssets.AccHandsOffComposite.Length; num15++)
		{
			TextureAssets.AccHandsOffComposite[num15] = LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOff_" + num15, (AssetRequestMode)0);
		}
		for (int num16 = 0; num16 < TextureAssets.AccBack.Length; num16++)
		{
			TextureAssets.AccBack[num16] = LoadAsset<Texture2D>("Images/Acc_Back_" + num16, (AssetRequestMode)0);
		}
		for (int num17 = 0; num17 < TextureAssets.AccFront.Length; num17++)
		{
			TextureAssets.AccFront[num17] = LoadAsset<Texture2D>("Images/Acc_Front_" + num17, (AssetRequestMode)0);
		}
		for (int num18 = 0; num18 < TextureAssets.AccShoes.Length; num18++)
		{
			TextureAssets.AccShoes[num18] = LoadAsset<Texture2D>("Images/Acc_Shoes_" + num18, (AssetRequestMode)0);
		}
		for (int num19 = 0; num19 < TextureAssets.AccWaist.Length; num19++)
		{
			TextureAssets.AccWaist[num19] = LoadAsset<Texture2D>("Images/Acc_Waist_" + num19, (AssetRequestMode)0);
		}
		for (int num20 = 0; num20 < TextureAssets.AccShield.Length; num20++)
		{
			TextureAssets.AccShield[num20] = LoadAsset<Texture2D>("Images/Acc_Shield_" + num20, (AssetRequestMode)0);
		}
		for (int num21 = 0; num21 < TextureAssets.AccNeck.Length; num21++)
		{
			TextureAssets.AccNeck[num21] = LoadAsset<Texture2D>("Images/Acc_Neck_" + num21, (AssetRequestMode)0);
		}
		for (int num22 = 0; num22 < TextureAssets.AccFace.Length; num22++)
		{
			TextureAssets.AccFace[num22] = LoadAsset<Texture2D>("Images/Acc_Face_" + num22, (AssetRequestMode)0);
		}
		for (int num23 = 0; num23 < TextureAssets.AccBalloon.Length; num23++)
		{
			TextureAssets.AccBalloon[num23] = LoadAsset<Texture2D>("Images/Acc_Balloon_" + num23, (AssetRequestMode)0);
		}
		for (int num24 = 0; num24 < TextureAssets.AccBeard.Length; num24++)
		{
			TextureAssets.AccBeard[num24] = LoadAsset<Texture2D>("Images/Acc_Beard_" + num24, (AssetRequestMode)0);
		}
		for (int num25 = 0; num25 < TextureAssets.Background.Length; num25++)
		{
			TextureAssets.Background[num25] = LoadAsset<Texture2D>("Images/Background_" + num25, (AssetRequestMode)0);
		}
		TextureAssets.FlameRing = LoadAsset<Texture2D>("Images/FlameRing", (AssetRequestMode)0);
		TextureAssets.TileCrack = LoadAsset<Texture2D>("Images\\TileCracks", mode);
		for (int num26 = 0; num26 < TextureAssets.ChestStack.Length; num26++)
		{
			TextureAssets.ChestStack[num26] = LoadAsset<Texture2D>("Images\\UI\\ChestStack_" + num26, mode);
		}
		for (int num27 = 0; num27 < TextureAssets.ChestCraft.Length; num27++)
		{
			TextureAssets.ChestCraft[num27] = LoadAsset<Texture2D>("Images\\UI\\ChestCraft_" + num27, mode);
		}
		TextureAssets.SmartDig = LoadAsset<Texture2D>("Images\\SmartDig", mode);
		TextureAssets.SmartCursorArrow = LoadAsset<Texture2D>("Images\\UI\\SmartCursorArrow", mode);
		TextureAssets.IceBarrier = LoadAsset<Texture2D>("Images\\IceBarrier", mode);
		TextureAssets.Frozen = LoadAsset<Texture2D>("Images\\Frozen", mode);
		for (int num28 = 0; num28 < TextureAssets.Pvp.Length; num28++)
		{
			TextureAssets.Pvp[num28] = LoadAsset<Texture2D>("Images\\UI\\PVP_" + num28, mode);
		}
		for (int num29 = 0; num29 < TextureAssets.EquipPage.Length; num29++)
		{
			TextureAssets.EquipPage[num29] = LoadAsset<Texture2D>("Images\\UI\\DisplaySlots_" + num29, mode);
		}
		TextureAssets.HouseBanner = LoadAsset<Texture2D>("Images\\UI\\House_Banner", mode);
		TextureAssets.NPCHappiness = LoadAsset<Texture2D>("Images\\UI\\NPCHappiness", mode);
		for (int num30 = 0; num30 < TextureAssets.CraftToggle.Length; num30++)
		{
			TextureAssets.CraftToggle[num30] = LoadAsset<Texture2D>("Images\\UI\\Craft_Toggle_" + num30, mode);
		}
		for (int num31 = 0; num31 < TextureAssets.BannerToggle.Length; num31++)
		{
			TextureAssets.BannerToggle[num31] = LoadAsset<Texture2D>("Images\\UI\\Banner_Toggle_" + num31, mode);
		}
		for (int num32 = 0; num32 < TextureAssets.InventorySort.Length; num32++)
		{
			TextureAssets.InventorySort[num32] = LoadAsset<Texture2D>("Images\\UI\\Sort_" + num32, mode);
		}
		for (int num33 = 0; num33 < TextureAssets.TextGlyph.Length; num33++)
		{
			TextureAssets.TextGlyph[num33] = LoadAsset<Texture2D>("Images\\UI\\Glyphs_" + num33, mode);
		}
		for (int num34 = 0; num34 < TextureAssets.HotbarRadial.Length; num34++)
		{
			TextureAssets.HotbarRadial[num34] = LoadAsset<Texture2D>("Images\\UI\\HotbarRadial_" + num34, mode);
		}
		for (int num35 = 0; num35 < TextureAssets.InfoIcon.Length; num35++)
		{
			TextureAssets.InfoIcon[num35] = LoadAsset<Texture2D>("Images\\UI\\InfoIcon_" + num35, mode);
		}
		for (int num36 = 0; num36 < TextureAssets.Reforge.Length; num36++)
		{
			TextureAssets.Reforge[num36] = LoadAsset<Texture2D>("Images\\UI\\Reforge_" + num36, mode);
		}
		for (int num37 = 0; num37 < TextureAssets.Camera.Length; num37++)
		{
			TextureAssets.Camera[num37] = LoadAsset<Texture2D>("Images\\UI\\Camera_" + num37, mode);
		}
		for (int num38 = 0; num38 < TextureAssets.WireUi.Length; num38++)
		{
			TextureAssets.WireUi[num38] = LoadAsset<Texture2D>("Images\\UI\\Wires_" + num38, mode);
		}
		TextureAssets.BuilderAcc = LoadAsset<Texture2D>("Images\\UI\\BuilderIcons", mode);
		TextureAssets.QuicksIcon = LoadAsset<Texture2D>("Images\\UI\\UI_quickicon1", mode);
		TextureAssets.TexturePackButtons = LoadAsset<Texture2D>("Images\\UI\\TexturePackButtons", mode);
		TextureAssets.CraftUpButton = LoadAsset<Texture2D>("Images\\RecUp", mode);
		TextureAssets.CraftDownButton = LoadAsset<Texture2D>("Images\\RecDown", mode);
		TextureAssets.ScrollLeftButton = LoadAsset<Texture2D>("Images\\RecLeft", mode);
		TextureAssets.ScrollRightButton = LoadAsset<Texture2D>("Images\\RecRight", mode);
		TextureAssets.OneDropLogo = LoadAsset<Texture2D>("Images\\OneDropLogo", mode);
		TextureAssets.Pulley = LoadAsset<Texture2D>("Images\\PlayerPulley", mode);
		TextureAssets.Timer = LoadAsset<Texture2D>("Images\\Timer", mode);
		TextureAssets.EmoteMenuButton = LoadAsset<Texture2D>("Images\\UI\\Emotes", mode);
		TextureAssets.BestiaryMenuButton = LoadAsset<Texture2D>("Images\\UI\\Bestiary", mode);
		TextureAssets.Wof = LoadAsset<Texture2D>("Images\\WallOfFlesh", mode);
		TextureAssets.WallOutline = LoadAsset<Texture2D>("Images\\Wall_Outline", mode);
		TextureAssets.Fade = LoadAsset<Texture2D>("Images\\fade-out", mode);
		TextureAssets.Ghost = LoadAsset<Texture2D>("Images\\Ghost", mode);
		TextureAssets.EvilCactus = LoadAsset<Texture2D>("Images\\Evil_Cactus", mode);
		TextureAssets.GoodCactus = LoadAsset<Texture2D>("Images\\Good_Cactus", mode);
		TextureAssets.CrimsonCactus = LoadAsset<Texture2D>("Images\\Crimson_Cactus", mode);
		TextureAssets.WraithEye = LoadAsset<Texture2D>("Images\\Wraith_Eyes", mode);
		TextureAssets.Firefly = LoadAsset<Texture2D>("Images\\Firefly", mode);
		TextureAssets.FireflyJar = LoadAsset<Texture2D>("Images\\FireflyJar", mode);
		TextureAssets.Lightningbug = LoadAsset<Texture2D>("Images\\LightningBug", mode);
		TextureAssets.LightningbugJar = LoadAsset<Texture2D>("Images\\LightningBugJar", mode);
		for (int num39 = 1; num39 <= 3; num39++)
		{
			TextureAssets.JellyfishBowl[num39 - 1] = LoadAsset<Texture2D>("Images\\jellyfishBowl" + num39, mode);
		}
		TextureAssets.GlowSnail = LoadAsset<Texture2D>("Images\\GlowSnail", mode);
		TextureAssets.IceQueen = LoadAsset<Texture2D>("Images\\IceQueen", mode);
		TextureAssets.SantaTank = LoadAsset<Texture2D>("Images\\SantaTank", mode);
		TextureAssets.JackHat = LoadAsset<Texture2D>("Images\\JackHat", mode);
		TextureAssets.TreeFace = LoadAsset<Texture2D>("Images\\TreeFace", mode);
		TextureAssets.PumpkingFace = LoadAsset<Texture2D>("Images\\PumpkingFace", mode);
		TextureAssets.ReaperEye = LoadAsset<Texture2D>("Images\\Reaper_Eyes", mode);
		TextureAssets.MapDeath = LoadAsset<Texture2D>("Images\\MapDeath", mode);
		TextureAssets.DukeFishron = LoadAsset<Texture2D>("Images\\DukeFishron", mode);
		TextureAssets.Map = LoadAsset<Texture2D>("Images\\Map", mode);
		for (int num40 = 0; num40 < TextureAssets.MapBGs.Length; num40++)
		{
			TextureAssets.MapBGs[num40] = LoadAsset<Texture2D>("Images\\MapBG" + (num40 + 1), mode);
		}
		TextureAssets.Hue = LoadAsset<Texture2D>("Images\\Hue", mode);
		TextureAssets.ColorSlider = LoadAsset<Texture2D>("Images\\ColorSlider", mode);
		TextureAssets.ColorBar = LoadAsset<Texture2D>("Images\\ColorBar", mode);
		TextureAssets.ColorBlip = LoadAsset<Texture2D>("Images\\ColorBlip", mode);
		TextureAssets.ColorHighlight = LoadAsset<Texture2D>("Images\\UI\\Slider_Highlight", mode);
		TextureAssets.LockOnCursor = LoadAsset<Texture2D>("Images\\UI\\LockOn_Cursor", mode);
		TextureAssets.Rain = LoadAsset<Texture2D>("Images\\Rain", mode);
		for (int num41 = 0; num41 < GlowMaskID.Count; num41++)
		{
			TextureAssets.GlowMask[num41] = LoadAsset<Texture2D>("Images\\Glow_" + num41, mode);
		}
		for (int num42 = 0; num42 < TextureAssets.HighlightMask.Length; num42++)
		{
			if (TileID.Sets.HasOutlines[num42])
			{
				TextureAssets.HighlightMask[num42] = LoadAsset<Texture2D>("Images\\Misc\\TileOutlines\\Tiles_" + num42, mode);
			}
		}
		for (int num43 = 0; num43 < ExtrasID.Count; num43++)
		{
			TextureAssets.Extra[num43] = LoadAsset<Texture2D>("Images\\Extra_" + num43, mode);
		}
		for (int num44 = 0; num44 < 4; num44++)
		{
			TextureAssets.Coin[num44] = LoadAsset<Texture2D>("Images\\Coin_" + num44, mode);
		}
		TextureAssets.MagicPixel = LoadAsset<Texture2D>("Images\\MagicPixel", mode);
		TextureAssets.SettingsPanel = LoadAsset<Texture2D>("Images\\UI\\Settings_Panel", mode);
		TextureAssets.SettingsPanel2 = LoadAsset<Texture2D>("Images\\UI\\Settings_Panel_2", mode);
		for (int num45 = 0; num45 < TextureAssets.XmasTree.Length; num45++)
		{
			TextureAssets.XmasTree[num45] = LoadAsset<Texture2D>("Images\\Xmas_" + num45, mode);
		}
		for (int num46 = 0; num46 < 6; num46++)
		{
			TextureAssets.Clothes[num46] = LoadAsset<Texture2D>("Images\\Clothes_" + num46, mode);
		}
		for (int num47 = 0; num47 < TextureAssets.Flames.Length; num47++)
		{
			TextureAssets.Flames[num47] = LoadAsset<Texture2D>("Images\\Flame_" + num47, mode);
		}
		for (int num48 = 0; num48 < 8; num48++)
		{
			TextureAssets.MapIcon[num48] = LoadAsset<Texture2D>("Images\\Map_" + num48, mode);
		}
		for (int num49 = 0; num49 < TextureAssets.Underworld.Length; num49++)
		{
			TextureAssets.Underworld[num49] = LoadAsset<Texture2D>("Images/Backgrounds/Underworld " + num49, (AssetRequestMode)0);
		}
		TextureAssets.Dest[0] = LoadAsset<Texture2D>("Images\\Dest1", mode);
		TextureAssets.Dest[1] = LoadAsset<Texture2D>("Images\\Dest2", mode);
		TextureAssets.Dest[2] = LoadAsset<Texture2D>("Images\\Dest3", mode);
		TextureAssets.Actuator = LoadAsset<Texture2D>("Images\\Actuator", mode);
		TextureAssets.Wire = LoadAsset<Texture2D>("Images\\Wires", mode);
		TextureAssets.Wire2 = LoadAsset<Texture2D>("Images\\Wires2", mode);
		TextureAssets.Wire3 = LoadAsset<Texture2D>("Images\\Wires3", mode);
		TextureAssets.Wire4 = LoadAsset<Texture2D>("Images\\Wires4", mode);
		TextureAssets.WireNew = LoadAsset<Texture2D>("Images\\WiresNew", mode);
		TextureAssets.FlyingCarpet = LoadAsset<Texture2D>("Images\\FlyingCarpet", mode);
		TextureAssets.Hb1 = LoadAsset<Texture2D>("Images\\HealthBar1", mode);
		TextureAssets.Hb2 = LoadAsset<Texture2D>("Images\\HealthBar2", mode);
		for (int num50 = 0; num50 < TextureAssets.NpcHead.Length; num50++)
		{
			TextureAssets.NpcHead[num50] = LoadAsset<Texture2D>("Images\\NPC_Head_" + num50, mode);
		}
		for (int num51 = 0; num51 < TextureAssets.NpcHeadBoss.Length; num51++)
		{
			TextureAssets.NpcHeadBoss[num51] = LoadAsset<Texture2D>("Images\\NPC_Head_Boss_" + num51, mode);
		}
		for (int num52 = 1; num52 < TextureAssets.BackPack.Length; num52++)
		{
			TextureAssets.BackPack[num52] = LoadAsset<Texture2D>("Images\\BackPack_" + num52, mode);
		}
		for (int num53 = 1; num53 < BuffID.Count; num53++)
		{
			TextureAssets.Buff[num53] = LoadAsset<Texture2D>("Images\\Buff_" + num53, mode);
		}
		Main.instance.LoadBackground(0);
		Main.instance.LoadBackground(49);
		TextureAssets.MinecartMount = LoadAsset<Texture2D>("Images\\Mount_Minecart", mode);
		for (int num54 = 0; num54 < TextureAssets.RudolphMount.Length; num54++)
		{
			TextureAssets.RudolphMount[num54] = LoadAsset<Texture2D>("Images\\Rudolph_" + num54, mode);
		}
		TextureAssets.BunnyMount = LoadAsset<Texture2D>("Images\\Mount_Bunny", mode);
		TextureAssets.PigronMount = LoadAsset<Texture2D>("Images\\Mount_Pigron", mode);
		TextureAssets.SlimeMount = LoadAsset<Texture2D>("Images\\Mount_Slime", mode);
		TextureAssets.TurtleMount = LoadAsset<Texture2D>("Images\\Mount_Turtle", mode);
		TextureAssets.UnicornMount = LoadAsset<Texture2D>("Images\\Mount_Unicorn", mode);
		TextureAssets.BasiliskMount = LoadAsset<Texture2D>("Images\\Mount_Basilisk", mode);
		TextureAssets.MinecartMechMount[0] = LoadAsset<Texture2D>("Images\\Mount_MinecartMech", mode);
		TextureAssets.MinecartMechMount[1] = LoadAsset<Texture2D>("Images\\Mount_MinecartMechGlow", mode);
		TextureAssets.CuteFishronMount[0] = LoadAsset<Texture2D>("Images\\Mount_CuteFishron1", mode);
		TextureAssets.CuteFishronMount[1] = LoadAsset<Texture2D>("Images\\Mount_CuteFishron2", mode);
		TextureAssets.MinecartWoodMount = LoadAsset<Texture2D>("Images\\Mount_MinecartWood", mode);
		TextureAssets.DesertMinecartMount = LoadAsset<Texture2D>("Images\\Mount_MinecartDesert", mode);
		TextureAssets.FishMinecartMount = LoadAsset<Texture2D>("Images\\Mount_MinecartMineCarp", mode);
		TextureAssets.BeeMount[0] = LoadAsset<Texture2D>("Images\\Mount_Bee", mode);
		TextureAssets.BeeMount[1] = LoadAsset<Texture2D>("Images\\Mount_BeeWings", mode);
		TextureAssets.UfoMount[0] = LoadAsset<Texture2D>("Images\\Mount_UFO", mode);
		TextureAssets.UfoMount[1] = LoadAsset<Texture2D>("Images\\Mount_UFOGlow", mode);
		TextureAssets.DrillMount[0] = LoadAsset<Texture2D>("Images\\Mount_DrillRing", mode);
		TextureAssets.DrillMount[1] = LoadAsset<Texture2D>("Images\\Mount_DrillSeat", mode);
		TextureAssets.DrillMount[2] = LoadAsset<Texture2D>("Images\\Mount_DrillDiode", mode);
		TextureAssets.DrillMount[3] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillRing", mode);
		TextureAssets.DrillMount[4] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillSeat", mode);
		TextureAssets.DrillMount[5] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillDiode", mode);
		TextureAssets.ScutlixMount[0] = LoadAsset<Texture2D>("Images\\Mount_Scutlix", mode);
		TextureAssets.ScutlixMount[1] = LoadAsset<Texture2D>("Images\\Mount_ScutlixEyes", mode);
		TextureAssets.ScutlixMount[2] = LoadAsset<Texture2D>("Images\\Mount_ScutlixEyeGlow", mode);
		for (int num55 = 0; num55 < TextureAssets.Gem.Length; num55++)
		{
			TextureAssets.Gem[num55] = LoadAsset<Texture2D>("Images\\Gem_" + num55, mode);
		}
		for (int num56 = 0; num56 < CloudID.Count; num56++)
		{
			TextureAssets.Cloud[num56] = LoadAsset<Texture2D>("Images\\Cloud_" + num56, mode);
		}
		for (int num57 = 0; num57 < 4; num57++)
		{
			TextureAssets.Star[num57] = LoadAsset<Texture2D>("Images\\Star_" + num57, mode);
		}
		for (int num58 = 0; num58 < 15; num58++)
		{
			TextureAssets.Liquid[num58] = LoadAsset<Texture2D>("Images\\Liquid_" + num58, mode);
			TextureAssets.LiquidSlope[num58] = LoadAsset<Texture2D>("Images\\LiquidSlope_" + num58, mode);
		}
		Main.instance.waterfallManager.LoadContent();
		TextureAssets.NpcToggle[0] = LoadAsset<Texture2D>("Images\\House_1", mode);
		TextureAssets.NpcToggle[1] = LoadAsset<Texture2D>("Images\\House_2", mode);
		TextureAssets.HbLock[0] = LoadAsset<Texture2D>("Images\\Lock_0", mode);
		TextureAssets.HbLock[1] = LoadAsset<Texture2D>("Images\\Lock_1", mode);
		TextureAssets.blockReplaceIcon[0] = LoadAsset<Texture2D>("Images\\UI\\BlockReplace_0", mode);
		TextureAssets.blockReplaceIcon[1] = LoadAsset<Texture2D>("Images\\UI\\BlockReplace_1", mode);
		TextureAssets.Grid = LoadAsset<Texture2D>("Images\\Grid", mode);
		TextureAssets.Trash = LoadAsset<Texture2D>("Images\\Trash", mode);
		TextureAssets.Cd = LoadAsset<Texture2D>("Images\\CoolDown", mode);
		TextureAssets.Logo = LoadAsset<Texture2D>("Images\\Logo", mode);
		TextureAssets.Logo2 = LoadAsset<Texture2D>("Images\\Logo2", mode);
		TextureAssets.Logo3 = LoadAsset<Texture2D>("Images\\Logo3", mode);
		TextureAssets.Logo4 = LoadAsset<Texture2D>("Images\\Logo4", mode);
		TextureAssets.Logo5 = LoadAsset<Texture2D>("Images\\Logo5", mode);
		TextureAssets.Logo6 = LoadAsset<Texture2D>("Images\\Logo6", mode);
		TextureAssets.Dust = LoadAsset<Texture2D>("Images\\Dust", mode);
		TextureAssets.Sun = LoadAsset<Texture2D>("Images\\Sun", mode);
		TextureAssets.Sun2 = LoadAsset<Texture2D>("Images\\Sun2", mode);
		TextureAssets.Sun3 = LoadAsset<Texture2D>("Images\\Sun3", mode);
		TextureAssets.BlackTile = LoadAsset<Texture2D>("Images\\Black_Tile", mode);
		TextureAssets.Heart = LoadAsset<Texture2D>("Images\\Heart", mode);
		TextureAssets.Heart2 = LoadAsset<Texture2D>("Images\\Heart2", mode);
		TextureAssets.Bubble = LoadAsset<Texture2D>("Images\\Bubble", mode);
		TextureAssets.Flame = LoadAsset<Texture2D>("Images\\Flame", mode);
		TextureAssets.Mana = LoadAsset<Texture2D>("Images\\Mana", mode);
		for (int num59 = 0; num59 < TextureAssets.Cursors.Length; num59++)
		{
			TextureAssets.Cursors[num59] = LoadAsset<Texture2D>("Images\\UI\\Cursor_" + num59, mode);
		}
		TextureAssets.CursorRadial = LoadAsset<Texture2D>("Images\\UI\\Radial", mode);
		TextureAssets.Ninja = LoadAsset<Texture2D>("Images\\Ninja", mode);
		TextureAssets.AntLion = LoadAsset<Texture2D>("Images\\AntlionBody", mode);
		TextureAssets.SpikeBase = LoadAsset<Texture2D>("Images\\Spike_Base", mode);
		TextureAssets.Wood[0] = LoadAsset<Texture2D>("Images\\Tiles_5_0", mode);
		TextureAssets.Wood[1] = LoadAsset<Texture2D>("Images\\Tiles_5_1", mode);
		TextureAssets.Wood[2] = LoadAsset<Texture2D>("Images\\Tiles_5_2", mode);
		TextureAssets.Wood[3] = LoadAsset<Texture2D>("Images\\Tiles_5_3", mode);
		TextureAssets.Wood[4] = LoadAsset<Texture2D>("Images\\Tiles_5_4", mode);
		TextureAssets.Wood[5] = LoadAsset<Texture2D>("Images\\Tiles_5_5", mode);
		TextureAssets.Wood[6] = LoadAsset<Texture2D>("Images\\Tiles_5_6", mode);
		TextureAssets.SmileyMoon = LoadAsset<Texture2D>("Images\\Moon_Smiley", mode);
		TextureAssets.PumpkinMoon = LoadAsset<Texture2D>("Images\\Moon_Pumpkin", mode);
		TextureAssets.SnowMoon = LoadAsset<Texture2D>("Images\\Moon_Snow", mode);
		for (int num60 = 0; num60 < TextureAssets.CageTop.Length; num60++)
		{
			TextureAssets.CageTop[num60] = LoadAsset<Texture2D>("Images\\CageTop_" + num60, mode);
		}
		for (int num61 = 0; num61 < TextureAssets.Moon.Length; num61++)
		{
			TextureAssets.Moon[num61] = LoadAsset<Texture2D>("Images\\Moon_" + num61, mode);
		}
		for (int num62 = 0; num62 < TextureAssets.TreeTop.Length; num62++)
		{
			TextureAssets.TreeTop[num62] = LoadAsset<Texture2D>("Images\\Tree_Tops_" + num62, mode);
		}
		for (int num63 = 0; num63 < TextureAssets.TreeBranch.Length; num63++)
		{
			TextureAssets.TreeBranch[num63] = LoadAsset<Texture2D>("Images\\Tree_Branches_" + num63, mode);
		}
		TextureAssets.ShroomCap = LoadAsset<Texture2D>("Images\\Shroom_Tops", mode);
		TextureAssets.InventoryBack = LoadAsset<Texture2D>("Images\\Inventory_Back", mode);
		TextureAssets.InventoryBack2 = LoadAsset<Texture2D>("Images\\Inventory_Back2", mode);
		TextureAssets.InventoryBack3 = LoadAsset<Texture2D>("Images\\Inventory_Back3", mode);
		TextureAssets.InventoryBack4 = LoadAsset<Texture2D>("Images\\Inventory_Back4", mode);
		TextureAssets.InventoryBack5 = LoadAsset<Texture2D>("Images\\Inventory_Back5", mode);
		TextureAssets.InventoryBack6 = LoadAsset<Texture2D>("Images\\Inventory_Back6", mode);
		TextureAssets.InventoryBack7 = LoadAsset<Texture2D>("Images\\Inventory_Back7", mode);
		TextureAssets.InventoryBack8 = LoadAsset<Texture2D>("Images\\Inventory_Back8", mode);
		TextureAssets.InventoryBack9 = LoadAsset<Texture2D>("Images\\Inventory_Back9", mode);
		TextureAssets.InventoryBack10 = LoadAsset<Texture2D>("Images\\Inventory_Back10", mode);
		TextureAssets.InventoryBack11 = LoadAsset<Texture2D>("Images\\Inventory_Back11", mode);
		TextureAssets.InventoryBack12 = LoadAsset<Texture2D>("Images\\Inventory_Back12", mode);
		TextureAssets.InventoryBack13 = LoadAsset<Texture2D>("Images\\Inventory_Back13", mode);
		TextureAssets.InventoryBack14 = LoadAsset<Texture2D>("Images\\Inventory_Back14", mode);
		TextureAssets.InventoryBack15 = LoadAsset<Texture2D>("Images\\Inventory_Back15", mode);
		TextureAssets.InventoryBack16 = LoadAsset<Texture2D>("Images\\Inventory_Back16", mode);
		TextureAssets.InventoryBack17 = LoadAsset<Texture2D>("Images\\Inventory_Back17", mode);
		TextureAssets.InventoryBack18 = LoadAsset<Texture2D>("Images\\Inventory_Back18", mode);
		TextureAssets.InventoryBack19 = LoadAsset<Texture2D>("Images\\Inventory_Back19", mode);
		TextureAssets.InventoryBack20 = LoadAsset<Texture2D>("Images\\Inventory_Back20", mode);
		TextureAssets.InventoryBack21 = LoadAsset<Texture2D>("Images\\Inventory_Back21", mode);
		TextureAssets.InventoryBack22 = LoadAsset<Texture2D>("Images\\Inventory_Back22", mode);
		TextureAssets.InventoryBack23 = LoadAsset<Texture2D>("Images\\Inventory_Back23", mode);
		TextureAssets.InventoryBack24 = LoadAsset<Texture2D>("Images\\Inventory_Back24", mode);
		TextureAssets.InventoryBack25 = LoadAsset<Texture2D>("Images\\Inventory_Back25", mode);
		TextureAssets.HairStyleBack = LoadAsset<Texture2D>("Images\\HairStyleBack", mode);
		TextureAssets.ClothesStyleBack = LoadAsset<Texture2D>("Images\\ClothesStyleBack", mode);
		TextureAssets.InventoryTickOff = LoadAsset<Texture2D>("Images\\Inventory_Tick_Off", mode);
		TextureAssets.InventoryTickOn = LoadAsset<Texture2D>("Images\\Inventory_Tick_On", mode);
		TextureAssets.TextBack = LoadAsset<Texture2D>("Images\\Text_Back", mode);
		TextureAssets.Chat = LoadAsset<Texture2D>("Images\\Chat", mode);
		TextureAssets.Chat2 = LoadAsset<Texture2D>("Images\\Chat2", mode);
		TextureAssets.ChatBack = LoadAsset<Texture2D>("Images\\Chat_Back", mode);
		TextureAssets.Team = LoadAsset<Texture2D>("Images\\Team", mode);
		PlayerDataInitializer.Load();
		TextureAssets.Chaos = LoadAsset<Texture2D>("Images\\Chaos", mode);
		TextureAssets.EyeLaser = LoadAsset<Texture2D>("Images\\Eye_Laser", mode);
		TextureAssets.BoneEyes = LoadAsset<Texture2D>("Images\\Bone_Eyes", mode);
		TextureAssets.BoneLaser = LoadAsset<Texture2D>("Images\\Bone_Laser", mode);
		TextureAssets.LightDisc = LoadAsset<Texture2D>("Images\\Light_Disc", mode);
		TextureAssets.Confuse = LoadAsset<Texture2D>("Images\\Confuse", mode);
		TextureAssets.Probe = LoadAsset<Texture2D>("Images\\Probe", mode);
		TextureAssets.SunOrb = LoadAsset<Texture2D>("Images\\SunOrb", mode);
		TextureAssets.SunAltar = LoadAsset<Texture2D>("Images\\SunAltar", mode);
		TextureAssets.XmasLight = LoadAsset<Texture2D>("Images\\XmasLight", mode);
		TextureAssets.Beetle = LoadAsset<Texture2D>("Images\\BeetleOrb", mode);
		for (int num64 = 0; num64 < ChainID.Count; num64++)
		{
			TextureAssets.Chains[num64] = LoadAsset<Texture2D>("Images\\Chains_" + num64, mode);
		}
		TextureAssets.Chain20 = LoadAsset<Texture2D>("Images\\Chain20", mode);
		TextureAssets.FishingLine = LoadAsset<Texture2D>("Images\\FishingLine", mode);
		TextureAssets.Chain = LoadAsset<Texture2D>("Images\\Chain", mode);
		TextureAssets.Chain2 = LoadAsset<Texture2D>("Images\\Chain2", mode);
		TextureAssets.Chain3 = LoadAsset<Texture2D>("Images\\Chain3", mode);
		TextureAssets.Chain4 = LoadAsset<Texture2D>("Images\\Chain4", mode);
		TextureAssets.Chain5 = LoadAsset<Texture2D>("Images\\Chain5", mode);
		TextureAssets.Chain6 = LoadAsset<Texture2D>("Images\\Chain6", mode);
		TextureAssets.Chain7 = LoadAsset<Texture2D>("Images\\Chain7", mode);
		TextureAssets.Chain8 = LoadAsset<Texture2D>("Images\\Chain8", mode);
		TextureAssets.Chain9 = LoadAsset<Texture2D>("Images\\Chain9", mode);
		TextureAssets.Chain10 = LoadAsset<Texture2D>("Images\\Chain10", mode);
		TextureAssets.Chain11 = LoadAsset<Texture2D>("Images\\Chain11", mode);
		TextureAssets.Chain12 = LoadAsset<Texture2D>("Images\\Chain12", mode);
		TextureAssets.Chain13 = LoadAsset<Texture2D>("Images\\Chain13", mode);
		TextureAssets.Chain14 = LoadAsset<Texture2D>("Images\\Chain14", mode);
		TextureAssets.Chain15 = LoadAsset<Texture2D>("Images\\Chain15", mode);
		TextureAssets.Chain16 = LoadAsset<Texture2D>("Images\\Chain16", mode);
		TextureAssets.Chain17 = LoadAsset<Texture2D>("Images\\Chain17", mode);
		TextureAssets.Chain18 = LoadAsset<Texture2D>("Images\\Chain18", mode);
		TextureAssets.Chain19 = LoadAsset<Texture2D>("Images\\Chain19", mode);
		TextureAssets.Chain20 = LoadAsset<Texture2D>("Images\\Chain20", mode);
		TextureAssets.Chain21 = LoadAsset<Texture2D>("Images\\Chain21", mode);
		TextureAssets.Chain22 = LoadAsset<Texture2D>("Images\\Chain22", mode);
		TextureAssets.Chain23 = LoadAsset<Texture2D>("Images\\Chain23", mode);
		TextureAssets.Chain24 = LoadAsset<Texture2D>("Images\\Chain24", mode);
		TextureAssets.Chain25 = LoadAsset<Texture2D>("Images\\Chain25", mode);
		TextureAssets.Chain26 = LoadAsset<Texture2D>("Images\\Chain26", mode);
		TextureAssets.Chain27 = LoadAsset<Texture2D>("Images\\Chain27", mode);
		TextureAssets.Chain28 = LoadAsset<Texture2D>("Images\\Chain28", mode);
		TextureAssets.Chain29 = LoadAsset<Texture2D>("Images\\Chain29", mode);
		TextureAssets.Chain30 = LoadAsset<Texture2D>("Images\\Chain30", mode);
		TextureAssets.Chain31 = LoadAsset<Texture2D>("Images\\Chain31", mode);
		TextureAssets.Chain32 = LoadAsset<Texture2D>("Images\\Chain32", mode);
		TextureAssets.Chain33 = LoadAsset<Texture2D>("Images\\Chain33", mode);
		TextureAssets.Chain34 = LoadAsset<Texture2D>("Images\\Chain34", mode);
		TextureAssets.Chain35 = LoadAsset<Texture2D>("Images\\Chain35", mode);
		TextureAssets.Chain36 = LoadAsset<Texture2D>("Images\\Chain36", mode);
		TextureAssets.Chain37 = LoadAsset<Texture2D>("Images\\Chain37", mode);
		TextureAssets.Chain38 = LoadAsset<Texture2D>("Images\\Chain38", mode);
		TextureAssets.Chain39 = LoadAsset<Texture2D>("Images\\Chain39", mode);
		TextureAssets.Chain40 = LoadAsset<Texture2D>("Images\\Chain40", mode);
		TextureAssets.Chain41 = LoadAsset<Texture2D>("Images\\Chain41", mode);
		TextureAssets.Chain42 = LoadAsset<Texture2D>("Images\\Chain42", mode);
		TextureAssets.Chain43 = LoadAsset<Texture2D>("Images\\Chain43", mode);
		TextureAssets.EyeLaserSmall = LoadAsset<Texture2D>("Images\\Eye_Laser_Small", mode);
		TextureAssets.BoneArm = LoadAsset<Texture2D>("Images\\Arm_Bone", mode);
		TextureAssets.PumpkingArm = LoadAsset<Texture2D>("Images\\PumpkingArm", mode);
		TextureAssets.PumpkingCloak = LoadAsset<Texture2D>("Images\\PumpkingCloak", mode);
		TextureAssets.BoneArm2 = LoadAsset<Texture2D>("Images\\Arm_Bone_2", mode);
		TextureAssets.BoneArm3 = LoadAsset<Texture2D>("Images\\Arm_Bone_3", mode);
		for (int num65 = 1; num65 < TextureAssets.GemChain.Length; num65++)
		{
			TextureAssets.GemChain[num65] = LoadAsset<Texture2D>("Images\\GemChain_" + num65, mode);
		}
		for (int num66 = 1; num66 < TextureAssets.Golem.Length; num66++)
		{
			TextureAssets.Golem[num66] = LoadAsset<Texture2D>("Images\\GolemLights" + num66, mode);
		}
		TextureAssets.GolfSwingBarFill = LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarFill", mode);
		TextureAssets.GolfSwingBarPanel = LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarPanel", mode);
		TextureAssets.SpawnPoint = LoadAsset<Texture2D>("Images\\UI\\SpawnPoint", mode);
		TextureAssets.SpawnBed = LoadAsset<Texture2D>("Images\\UI\\SpawnBed", mode);
		TextureAssets.MapPing = LoadAsset<Texture2D>("Images\\UI\\MapPing", mode);
		TextureAssets.GolfBallArrow = LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow", mode);
		TextureAssets.GolfBallArrowShadow = LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow_Shadow", mode);
		TextureAssets.GolfBallOutline = LoadAsset<Texture2D>("Images\\Misc\\GolfBallOutline", mode);
		TextureAssets.NpcPortraitBackground = LoadAsset<Texture2D>("Images\\TownNPCs\\Portraits\\Portrait_Window", mode);
		Main.ResourceSetsManager.LoadContent(mode);
		Main.MinimapFrameManagerInstance.LoadContent(mode);
		Main.AchievementAdvisor.LoadContent();
	}

	private static Asset<T> LoadAsset<T>(string assetName, AssetRequestMode mode) where T : class
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Main.Assets.Request<T>(assetName, mode);
	}
}
