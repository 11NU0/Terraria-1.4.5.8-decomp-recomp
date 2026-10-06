using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using ReLogic.OS;
using ReLogic.Peripherals.RGB;
using ReLogic.Peripherals.RGB.Corsair;
using ReLogic.Peripherals.RGB.Logitech;
using ReLogic.Peripherals.RGB.Razer;
using ReLogic.Peripherals.RGB.SteelSeries;
using SteelSeries.GameSense;
using SteelSeries.GameSense.DeviceZone;
using Terraria.GameContent.RGB;
using Terraria.GameInput;
using Terraria.IO;

namespace Terraria.Initializers;

public static class ChromaInitializer
{
	public struct EventLocalization
	{
		public string DefaultDisplayName;

		public Dictionary<string, string> LocalizedNames;
	}

	private static ChromaEngine _engine;

	private const string GAME_NAME_ID = "TERRARIA";

	private static float _rgbUpdateRate = 45f;

	private static bool _useRazer = true;

	private static bool _useCorsair = true;

	private static bool _useLogitech = true;

	private static bool _useSteelSeries = true;

	private static VendorColorProfile _razerColorProfile = new VendorColorProfile(new Vector3(1f, 0.765f, 0.568f));

	private static VendorColorProfile _corsairColorProfile = new VendorColorProfile();

	private static VendorColorProfile _logitechColorProfile = new VendorColorProfile();

	private static VendorColorProfile _steelSeriesColorProfile = new VendorColorProfile();

	private static Dictionary<string, EventLocalization> _localizedEvents = new Dictionary<string, EventLocalization>
	{
		{
			"KEY_MOUSELEFT",
			new EventLocalization
			{
				DefaultDisplayName = "Left Mouse Button"
			}
		},
		{
			"KEY_MOUSERIGHT",
			new EventLocalization
			{
				DefaultDisplayName = "Right Mouse Button"
			}
		},
		{
			"KEY_UP",
			new EventLocalization
			{
				DefaultDisplayName = "Up"
			}
		},
		{
			"KEY_DOWN",
			new EventLocalization
			{
				DefaultDisplayName = "Down"
			}
		},
		{
			"KEY_LEFT",
			new EventLocalization
			{
				DefaultDisplayName = "Left"
			}
		},
		{
			"KEY_RIGHT",
			new EventLocalization
			{
				DefaultDisplayName = "Right"
			}
		},
		{
			"KEY_JUMP",
			new EventLocalization
			{
				DefaultDisplayName = "Jump"
			}
		},
		{
			"KEY_THROW",
			new EventLocalization
			{
				DefaultDisplayName = "Throw"
			}
		},
		{
			"KEY_INVENTORY",
			new EventLocalization
			{
				DefaultDisplayName = "Inventory"
			}
		},
		{
			"KEY_GRAPPLE",
			new EventLocalization
			{
				DefaultDisplayName = "Grapple"
			}
		},
		{
			"KEY_SMARTSELECT",
			new EventLocalization
			{
				DefaultDisplayName = "Smart Select"
			}
		},
		{
			"KEY_SMARTCURSOR",
			new EventLocalization
			{
				DefaultDisplayName = "Smart Cursor"
			}
		},
		{
			"KEY_QUICKMOUNT",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Mount"
			}
		},
		{
			"KEY_QUICKHEAL",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Heal"
			}
		},
		{
			"KEY_QUICKMANA",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Mana"
			}
		},
		{
			"KEY_QUICKBUFF",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Buff"
			}
		},
		{
			"KEY_MAPZOOMIN",
			new EventLocalization
			{
				DefaultDisplayName = "Map Zoom In"
			}
		},
		{
			"KEY_MAPZOOMOUT",
			new EventLocalization
			{
				DefaultDisplayName = "Map Zoom Out"
			}
		},
		{
			"KEY_MAPALPHAUP",
			new EventLocalization
			{
				DefaultDisplayName = "Map Transparency Up"
			}
		},
		{
			"KEY_MAPALPHADOWN",
			new EventLocalization
			{
				DefaultDisplayName = "Map Transparency Down"
			}
		},
		{
			"KEY_MAPFULL",
			new EventLocalization
			{
				DefaultDisplayName = "Map Full"
			}
		},
		{
			"KEY_MAPSTYLE",
			new EventLocalization
			{
				DefaultDisplayName = "Map Style"
			}
		},
		{
			"KEY_HOTBAR1",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 1"
			}
		},
		{
			"KEY_HOTBAR2",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 2"
			}
		},
		{
			"KEY_HOTBAR3",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 3"
			}
		},
		{
			"KEY_HOTBAR4",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 4"
			}
		},
		{
			"KEY_HOTBAR5",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 5"
			}
		},
		{
			"KEY_HOTBAR6",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 6"
			}
		},
		{
			"KEY_HOTBAR7",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 7"
			}
		},
		{
			"KEY_HOTBAR8",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 8"
			}
		},
		{
			"KEY_HOTBAR9",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 9"
			}
		},
		{
			"KEY_HOTBAR10",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 10"
			}
		},
		{
			"KEY_HOTBARMINUS",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar Minus"
			}
		},
		{
			"KEY_HOTBARPLUS",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar Plus"
			}
		},
		{
			"KEY_DPADRADIAL1",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 1"
			}
		},
		{
			"KEY_DPADRADIAL2",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 2"
			}
		},
		{
			"KEY_DPADRADIAL3",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 3"
			}
		},
		{
			"KEY_DPADRADIAL4",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 4"
			}
		},
		{
			"KEY_RADIALHOTBAR",
			new EventLocalization
			{
				DefaultDisplayName = "Radial Hotbar"
			}
		},
		{
			"KEY_RADIALQUICKBAR",
			new EventLocalization
			{
				DefaultDisplayName = "Radial Quickbar"
			}
		},
		{
			"KEY_DPADSNAP1",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 1"
			}
		},
		{
			"KEY_DPADSNAP2",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 2"
			}
		},
		{
			"KEY_DPADSNAP3",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 3"
			}
		},
		{
			"KEY_DPADSNAP4",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 4"
			}
		},
		{
			"KEY_MENUUP",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Up"
			}
		},
		{
			"KEY_MENUDOWN",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Down"
			}
		},
		{
			"KEY_MENULEFT",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Left"
			}
		},
		{
			"KEY_MENURIGHT",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Right"
			}
		},
		{
			"KEY_LOCKON",
			new EventLocalization
			{
				DefaultDisplayName = "Lock On"
			}
		},
		{
			"KEY_VIEWZOOMIN",
			new EventLocalization
			{
				DefaultDisplayName = "Zoom In"
			}
		},
		{
			"KEY_VIEWZOOMOUT",
			new EventLocalization
			{
				DefaultDisplayName = "Zoom Out"
			}
		},
		{
			"KEY_TOGGLECREATIVEMENU",
			new EventLocalization
			{
				DefaultDisplayName = "Toggle Creative Menu"
			}
		},
		{
			"DO_RAINBOWS",
			new EventLocalization
			{
				DefaultDisplayName = "Theme"
			}
		},
		{
			"ZONE1",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 1"
			}
		},
		{
			"ZONE2",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 2"
			}
		},
		{
			"ZONE3",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 3"
			}
		},
		{
			"ZONE4",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 4"
			}
		},
		{
			"ZONE5",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 5"
			}
		},
		{
			"ZONE6",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 6"
			}
		},
		{
			"ZONE7",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 7"
			}
		},
		{
			"ZONE8",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 8"
			}
		},
		{
			"ZONE9",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 9"
			}
		},
		{
			"ZONE10",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 10"
			}
		},
		{
			"ZONE11",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 11"
			}
		},
		{
			"ZONE12",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 12"
			}
		},
		{
			"LIFE",
			new EventLocalization
			{
				DefaultDisplayName = "Life Percent"
			}
		},
		{
			"MANA",
			new EventLocalization
			{
				DefaultDisplayName = "Mana Percent"
			}
		},
		{
			"BREATH",
			new EventLocalization
			{
				DefaultDisplayName = "Breath Percent"
			}
		}
	};

	public static IntRgbGameValueTracker Event_LifePercent;

	public static IntRgbGameValueTracker Event_ManaPercent;

	public static IntRgbGameValueTracker Event_BreathPercent;

	public static void BindTo(Preferences preferences)
	{
		preferences.OnSave += Configuration_OnSave;
		preferences.OnLoad += Configuration_OnLoad;
	}

	private static void Configuration_OnLoad(Preferences obj)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected Obj, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected Obj, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected Obj, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected Obj, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected Obj, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected Obj, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected Obj, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected Obj, but got Unknown
		_useRazer = obj.Get("UseRazerRGB", defaultValue: true);
		_useCorsair = obj.Get("UseCorsairRGB", defaultValue: true);
		_useLogitech = obj.Get("UseLogitechRGB", defaultValue: true);
		_useSteelSeries = obj.Get("UseSteelSeriesRGB", defaultValue: true);
		_razerColorProfile = obj.Get<VendorColorProfile>("RazerColorProfile", new VendorColorProfile(new Vector3(1f, 0.765f, 0.568f)));
		_corsairColorProfile = obj.Get<VendorColorProfile>("CorsairColorProfile", new VendorColorProfile());
		_logitechColorProfile = obj.Get<VendorColorProfile>("LogitechColorProfile", new VendorColorProfile());
		_steelSeriesColorProfile = obj.Get<VendorColorProfile>("SteelSeriesColorProfile", new VendorColorProfile());
		if (_razerColorProfile == null)
		{
			_razerColorProfile = new VendorColorProfile(new Vector3(1f, 0.765f, 0.568f));
		}
		if (_corsairColorProfile == null)
		{
			_corsairColorProfile = new VendorColorProfile();
		}
		if (_logitechColorProfile == null)
		{
			_logitechColorProfile = new VendorColorProfile();
		}
		if (_steelSeriesColorProfile == null)
		{
			_steelSeriesColorProfile = new VendorColorProfile();
		}
		_rgbUpdateRate = obj.Get("RGBUpdatesPerSecond", 45f);
		if (_rgbUpdateRate <= 1E-07f)
		{
			_rgbUpdateRate = 45f;
		}
	}

	private static void Configuration_OnSave(Preferences preferences)
	{
		preferences.Put("RGBUpdatesPerSecond", _rgbUpdateRate);
		preferences.Put("UseRazerRGB", _useRazer);
		preferences.Put("RazerColorProfile", _razerColorProfile);
		preferences.Put("UseCorsairRGB", _useCorsair);
		preferences.Put("CorsairColorProfile", _corsairColorProfile);
		preferences.Put("UseLogitechRGB", _useLogitech);
		preferences.Put("LogitechColorProfile", _logitechColorProfile);
		preferences.Put("UseSteelSeriesRGB", _useSteelSeries);
		preferences.Put("SteelSeriesColorProfile", _steelSeriesColorProfile);
	}

	private static void AddDevices()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected Obj, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected Obj, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected Obj, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected Obj, but got Unknown
		_engine.AddDeviceGroup("Razer", (RgbDeviceGroup)new RazerDeviceGroup(_razerColorProfile));
		_engine.AddDeviceGroup("Corsair", (RgbDeviceGroup)new CorsairDeviceGroup(_corsairColorProfile));
		_engine.AddDeviceGroup("Logitech", (RgbDeviceGroup)new LogitechDeviceGroup(_logitechColorProfile));
		_engine.AddDeviceGroup("SteelSeries", (RgbDeviceGroup)new SteelSeriesDeviceGroup(_steelSeriesColorProfile, "TERRARIA", "Terraria", (IconColor)3));
		_engine.FrameTimeInSeconds = 1f / _rgbUpdateRate;
		if (_useRazer)
		{
			_engine.EnableDeviceGroup("Razer");
		}
		if (_useCorsair)
		{
			_engine.EnableDeviceGroup("Corsair");
		}
		if (_useLogitech)
		{
			_engine.EnableDeviceGroup("Logitech");
		}
		if (_useSteelSeries)
		{
			_engine.EnableDeviceGroup("SteelSeries");
		}
		LoadSpecialRulesForDevices();
		AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
		if (Platform.IsWindows)
		{
			Application.ApplicationExit += OnProcessExit;
		}
	}

	private static void LoadSpecialRulesForDevices()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected Obj, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected Obj, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected Obj, but got Unknown
		Event_LifePercent = new IntRgbGameValueTracker
		{
			EventName = "LIFE"
		};
		Event_ManaPercent = new IntRgbGameValueTracker
		{
			EventName = "MANA"
		};
		Event_BreathPercent = new IntRgbGameValueTracker
		{
			EventName = "BREATH"
		};
		LoadSpecialRulesFor_GameSense();
	}

	public static void UpdateEvents()
	{
		if (Main.gameMenu)
		{
			((ARgbGameValueTracker<int>)(object)Event_LifePercent).Update(0, false);
			((ARgbGameValueTracker<int>)(object)Event_ManaPercent).Update(0, false);
			((ARgbGameValueTracker<int>)(object)Event_BreathPercent).Update(0, false);
			return;
		}
		Player localPlayer = Main.LocalPlayer;
		int num = (int)Utils.Clamp((float)localPlayer.statLife / (float)localPlayer.statLifeMax2 * 100f, 0f, 100f);
		((ARgbGameValueTracker<int>)(object)Event_LifePercent).Update(num, true);
		int num2 = (int)Utils.Clamp((float)localPlayer.statMana / (float)localPlayer.statManaMax2 * 100f, 0f, 100f);
		((ARgbGameValueTracker<int>)(object)Event_ManaPercent).Update(num2, true);
		int num3 = (int)Utils.Clamp((float)localPlayer.breath / (float)localPlayer.breathMax * 100f, 0f, 100f);
		((ARgbGameValueTracker<int>)(object)Event_BreathPercent).Update(num3, true);
	}

	private static void LoadSpecialRulesFor_GameSense()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected Obj, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected Obj, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected Obj, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected Obj, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected Obj, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected Obj, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected Obj, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected Obj, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected Obj, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected Obj, but got Unknown
		GameSenseSpecificInfo val = new GameSenseSpecificInfo();
		List<Bind_Event> eventsToBind = (val.EventsToBind = new List<Bind_Event>());
		LoadSpecialRulesFor_GameSense_Keyboard(eventsToBind);
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE1", "zone1", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("one"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE2", "zone2", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("two"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE3", "zone3", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("three"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE4", "zone4", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("four"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE5", "zone5", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("five"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE6", "zone6", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("six"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE7", "zone7", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("seven"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE8", "zone8", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("eight"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE9", "zone9", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("nine"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE10", "zone10", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("ten"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE11", "zone11", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("eleven"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE12", "zone12", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("twelve"));
		AddGameplayEvents(eventsToBind);
		val.MiscEvents = new List<ARgbGameValueTracker>
		{
			(ARgbGameValueTracker)(object)Event_LifePercent,
			(ARgbGameValueTracker)(object)Event_ManaPercent,
			(ARgbGameValueTracker)(object)Event_BreathPercent
		};
		foreach (Bind_Event item in val.EventsToBind)
		{
			if (_localizedEvents.TryGetValue(item.eventName, out var value))
			{
				item.defaultDisplayName = value.DefaultDisplayName;
				item.localizedDisplayNames = value.LocalizedNames;
			}
		}
		_engine.LoadSpecialRules((object)val);
	}

	private static void AddGameplayEvents(List<Bind_Event> eventsToBind)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected Obj, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected Obj, but got Unknown
		eventsToBind.Add(new Bind_Event("TERRARIA", ((ARgbGameValueTracker)Event_LifePercent).EventName, 0, 100, (EventIconId)1, new AbstractHandler[0]));
		eventsToBind.Add(new Bind_Event("TERRARIA", ((ARgbGameValueTracker)Event_ManaPercent).EventName, 0, 100, (EventIconId)14, new AbstractHandler[0]));
		eventsToBind.Add(new Bind_Event("TERRARIA", ((ARgbGameValueTracker)Event_BreathPercent).EventName, 0, 100, (EventIconId)11, new AbstractHandler[0]));
	}

	private static void LoadSpecialRulesFor_SecondaryDevice(List<Bind_Event> eventsToBind, string eventName, string contextFrameKey, AbstractIlluminationDevice_Zone zone)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected Obj, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected Obj, but got Unknown
		Bind_Event item = new Bind_Event("TERRARIA", eventName, 0, 10, (EventIconId)0, new AbstractHandler[1] { (AbstractHandler)new ContextColorEventHandlerType
		{
			ContextFrameKey = contextFrameKey,
			DeviceZone = zone
		} });
		eventsToBind.Add(item);
	}

	private static void LoadSpecialRulesFor_GameSense_Keyboard(List<Bind_Event> eventsToBind)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected Obj, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected Obj, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected Obj, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected Obj, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected Obj, but got Unknown
		Dictionary<string, byte> xnaKeyNamesToSteelSeriesKeyIndex = HIDCodes.XnaKeyNamesToSteelSeriesKeyIndex;
		Color white = Color.White;
		foreach (KeyValuePair<string, List<string>> item3 in PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus)
		{
			string key = item3.Key;
			List<string> value = item3.Value;
			List<byte> list = new List<byte>();
			foreach (string item4 in value)
			{
				if (xnaKeyNamesToSteelSeriesKeyIndex.TryGetValue(item4, out var value2))
				{
					list.Add(value2);
				}
			}
			RGBPerkeyZoneCustom deviceZone = new RGBPerkeyZoneCustom(list.ToArray());
			new ColorStatic
			{
				red = white.R,
				green = white.G,
				blue = white.B
			};
			Bind_Event item = new Bind_Event("TERRARIA", "KEY_" + key.ToUpper(), 0, 10, (EventIconId)0, new AbstractHandler[1] { (AbstractHandler)new ContextColorEventHandlerType
			{
				ContextFrameKey = key,
				DeviceZone = (AbstractIlluminationDevice_Zone)(object)deviceZone
			} });
			eventsToBind.Add(item);
		}
		Bind_Event item2 = new Bind_Event("TERRARIA", "DO_RAINBOWS", 0, 10, (EventIconId)0, new AbstractHandler[1] { (AbstractHandler)new PartialBitmapEventHandlerType
		{
			EventsToExclude = eventsToBind.Select((Bind_Event x) => x.eventName).ToArray()
		} });
		eventsToBind.Add(item2);
	}

	public static void DisableAllDeviceGroups()
	{
		if (_engine != null)
		{
			_engine.DisableAllDeviceGroups();
		}
	}

	private static void OnProcessExit(object sender, EventArgs e)
	{
		DisableAllDeviceGroups();
	}

	public static void Load()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
		_engine = Main.Chroma;
		AddDevices();
		Color val = new Color(46, 23, 12);
		RegisterShader("Base", (ChromaShader)(object)new SurfaceBiomeShader(Color.Green, val), CommonConditions.InMenu, (ShaderLayer)9);
		RegisterShader("Surface Mushroom", (ChromaShader)(object)new SurfaceBiomeShader(Color.DarkBlue, new Color(33, 31, 27)), CommonConditions.DrunkMenu, (ShaderLayer)9);
		RegisterShader("Sky", (ChromaShader)(object)new SkyShader(new Color(34, 51, 128), new Color(5, 5, 5)), CommonConditions.Depth.Sky, (ShaderLayer)1);
		RegisterShader("Surface", (ChromaShader)(object)new SurfaceBiomeShader(Color.Green, val), CommonConditions.Depth.Surface, (ShaderLayer)1);
		RegisterShader("Vines", (ChromaShader)(object)new VineShader(), CommonConditions.Depth.Vines, (ShaderLayer)1);
		RegisterShader("Underground", (ChromaShader)(object)new CavernShader(new Color(122, 62, 32), new Color(25, 13, 7), 0.5f), CommonConditions.Depth.Underground, (ShaderLayer)1);
		RegisterShader("Caverns", (ChromaShader)(object)new CavernShader(val, new Color(25, 25, 25), 0.5f), CommonConditions.Depth.Caverns, (ShaderLayer)1);
		RegisterShader("Magma", (ChromaShader)(object)new CavernShader(new Color(181, 17, 0), new Color(25, 25, 25), 0.5f), CommonConditions.Depth.Magma, (ShaderLayer)1);
		RegisterShader("Underworld", (ChromaShader)(object)new UnderworldShader(Color.Red, new Color(1f, 0.5f, 0f), 1f), CommonConditions.Depth.Underworld, (ShaderLayer)1);
		RegisterShader("Surface Desert", (ChromaShader)(object)new SurfaceBiomeShader(new Color(84, 49, 0), new Color(245, 225, 33)), CommonConditions.SurfaceBiome.Desert, (ShaderLayer)2);
		RegisterShader("Surface Jungle", (ChromaShader)(object)new SurfaceBiomeShader(Color.Green, Color.Teal), CommonConditions.SurfaceBiome.Jungle, (ShaderLayer)2);
		RegisterShader("Surface Ocean", (ChromaShader)(object)new SurfaceBiomeShader(Color.SkyBlue, Color.Blue), CommonConditions.SurfaceBiome.Ocean, (ShaderLayer)2);
		RegisterShader("Surface Snow", (ChromaShader)(object)new SurfaceBiomeShader(new Color(0, 10, 50), new Color(0.5f, 0.75f, 1f)), CommonConditions.SurfaceBiome.Snow, (ShaderLayer)2);
		RegisterShader("Surface Mushroom", (ChromaShader)(object)new SurfaceBiomeShader(Color.DarkBlue, new Color(33, 31, 27)), CommonConditions.SurfaceBiome.Mushroom, (ShaderLayer)2);
		RegisterShader("Surface Hallow", (ChromaShader)(object)new HallowSurfaceShader(), CommonConditions.SurfaceBiome.Hallow, (ShaderLayer)3);
		RegisterShader("Surface Crimson", (ChromaShader)(object)new CorruptSurfaceShader(Color.Red, new Color(25, 25, 40)), CommonConditions.SurfaceBiome.Crimson, (ShaderLayer)3);
		RegisterShader("Surface Corruption", (ChromaShader)(object)new CorruptSurfaceShader(new Color(73, 0, 255), new Color(15, 15, 27)), CommonConditions.SurfaceBiome.Corruption, (ShaderLayer)3);
		RegisterShader("Hive", (ChromaShader)(object)new DrippingShader(new Color(0.05f, 0.01f, 0f), new Color(255, 150, 0), 0.5f), CommonConditions.UndergroundBiome.Hive, (ShaderLayer)3);
		RegisterShader("Underground Mushroom", (ChromaShader)(object)new UndergroundMushroomShader(), CommonConditions.UndergroundBiome.Mushroom, (ShaderLayer)2);
		RegisterShader("Underground Corrutpion", (ChromaShader)(object)new UndergroundCorruptionShader(), CommonConditions.UndergroundBiome.Corrupt, (ShaderLayer)2);
		RegisterShader("Underground Crimson", (ChromaShader)(object)new DrippingShader(new Color(0.05f, 0f, 0f), new Color(255, 0, 0)), CommonConditions.UndergroundBiome.Crimson, (ShaderLayer)2);
		RegisterShader("Underground Hallow", (ChromaShader)(object)new UndergroundHallowShader(), CommonConditions.UndergroundBiome.Hallow, (ShaderLayer)2);
		RegisterShader("Meteorite", (ChromaShader)(object)new MeteoriteShader(), CommonConditions.MiscBiome.Meteorite, (ShaderLayer)3);
		RegisterShader("Temple", (ChromaShader)(object)new TempleShader(), CommonConditions.UndergroundBiome.Temple, (ShaderLayer)3);
		RegisterShader("Dungeon", (ChromaShader)(object)new DungeonShader(), CommonConditions.UndergroundBiome.Dungeon, (ShaderLayer)3);
		RegisterShader("Granite", (ChromaShader)(object)new CavernShader(new Color(14, 19, 46), new Color(5, 0, 30), 0.5f), CommonConditions.UndergroundBiome.Granite, (ShaderLayer)3);
		RegisterShader("Marble", (ChromaShader)(object)new CavernShader(new Color(100, 100, 100), new Color(20, 20, 20), 0.5f), CommonConditions.UndergroundBiome.Marble, (ShaderLayer)3);
		Color secondaryColor = new Color(25, 25, 25);
		Vector4[] array = new Vector4[7];
		Color val2 = Color.White;
		array[0] = val2.ToVector4();
		val2 = Color.Yellow;
		array[1] = val2.ToVector4();
		val2 = Color.Orange;
		array[2] = val2.ToVector4();
		val2 = Color.Red;
		array[3] = val2.ToVector4();
		val2 = Color.Green;
		array[4] = val2.ToVector4();
		val2 = Color.Blue;
		array[5] = val2.ToVector4();
		val2 = Color.Purple;
		array[6] = val2.ToVector4();
		RegisterShader("Gem Cave", (ChromaShader)(object)new GemCaveShader(val, secondaryColor, array)
		{
			CycleTime = 100f,
			ColorRarity = 20f,
			TimeRate = 0.25f
		}, CommonConditions.UndergroundBiome.GemCave, (ShaderLayer)3);
		Vector4[] array2 = new Vector4[12];
		for (int i = 0; i < array2.Length; i++)
		{
			int num = i;
			val2 = Main.hslToRgb((float)i / (float)array2.Length, 1f, 0.5f);
			array2[num] = val2.ToVector4();
		}
		RegisterShader("Shimmer", (ChromaShader)(object)new GemCaveShader(Color.Silver * 0.5f, new Color(125, 55, 125), array2)
		{
			CycleTime = 2f,
			ColorRarity = 4f,
			TimeRate = 0.5f
		}, CommonConditions.UndergroundBiome.Shimmer, (ShaderLayer)3);
		RegisterShader("Underground Jungle", (ChromaShader)(object)new JungleShader(), CommonConditions.UndergroundBiome.Jungle, (ShaderLayer)2);
		RegisterShader("Underground Ice", (ChromaShader)(object)new IceShader(new Color(0, 10, 50), new Color(0.5f, 0.75f, 1f)), CommonConditions.UndergroundBiome.Ice, (ShaderLayer)2);
		RegisterShader("Corrupt Ice", (ChromaShader)(object)new IceShader(new Color(5, 0, 25), new Color(152, 102, 255)), CommonConditions.UndergroundBiome.CorruptIce, (ShaderLayer)3);
		RegisterShader("Crimson Ice", (ChromaShader)(object)new IceShader(new Color(0.1f, 0f, 0f), new Color(1f, 0.45f, 0.4f)), CommonConditions.UndergroundBiome.CrimsonIce, (ShaderLayer)3);
		RegisterShader("Hallow Ice", (ChromaShader)(object)new IceShader(new Color(0.2f, 0f, 0.1f), new Color(1f, 0.7f, 0.7f)), CommonConditions.UndergroundBiome.HallowIce, (ShaderLayer)3);
		RegisterShader("Underground Desert", (ChromaShader)(object)new DesertShader(new Color(60, 10, 0), new Color(255, 165, 0)), CommonConditions.UndergroundBiome.Desert, (ShaderLayer)2);
		RegisterShader("Corrupt Desert", (ChromaShader)(object)new DesertShader(new Color(15, 0, 15), new Color(116, 103, 255)), CommonConditions.UndergroundBiome.CorruptDesert, (ShaderLayer)3);
		RegisterShader("Crimson Desert", (ChromaShader)(object)new DesertShader(new Color(20, 10, 0), new Color(195, 0, 0)), CommonConditions.UndergroundBiome.CrimsonDesert, (ShaderLayer)3);
		RegisterShader("Hallow Desert", (ChromaShader)(object)new DesertShader(new Color(29, 0, 56), new Color(255, 221, 255)), CommonConditions.UndergroundBiome.HallowDesert, (ShaderLayer)3);
		RegisterShader("Pumpkin Moon", (ChromaShader)(object)new MoonShader(new Color(13, 0, 26), Color.Orange), CommonConditions.Events.PumpkinMoon, (ShaderLayer)4);
		RegisterShader("Blood Moon", (ChromaShader)(object)new MoonShader(new Color(10, 0, 0), Color.Red, Color.Red, new Color(255, 150, 125)), CommonConditions.Events.BloodMoon, (ShaderLayer)4);
		RegisterShader("Frost Moon", (ChromaShader)(object)new MoonShader(new Color(0, 4, 13), new Color(255, 255, 255)), CommonConditions.Events.FrostMoon, (ShaderLayer)4);
		RegisterShader("Solar Eclipse", (ChromaShader)(object)new MoonShader(new Color(0.02f, 0.02f, 0.02f), Color.Orange, Color.Black), CommonConditions.Events.SolarEclipse, (ShaderLayer)4);
		RegisterShader("Pirate Invasion", (ChromaShader)(object)new PirateInvasionShader(new Color(173, 173, 173), new Color(101, 101, 255), Color.Blue, Color.Black), CommonConditions.Events.PirateInvasion, (ShaderLayer)4);
		RegisterShader("DD2 Event", (ChromaShader)(object)new DD2Shader(new Color(222, 94, 245), Color.White), CommonConditions.Events.DD2Event, (ShaderLayer)4);
		RegisterShader("Goblin Army", (ChromaShader)(object)new GoblinArmyShader(new Color(14, 0, 79), new Color(176, 0, 144)), CommonConditions.Events.GoblinArmy, (ShaderLayer)4);
		RegisterShader("Frost Legion", (ChromaShader)(object)new FrostLegionShader(Color.White, new Color(27, 80, 201)), CommonConditions.Events.FrostLegion, (ShaderLayer)4);
		RegisterShader("Martian Madness", (ChromaShader)(object)new MartianMadnessShader(new Color(64, 64, 64), new Color(64, 113, 122), new Color(255, 255, 0), new Color(3, 3, 18)), CommonConditions.Events.MartianMadness, (ShaderLayer)4);
		RegisterShader("Solar Pillar", (ChromaShader)(object)new PillarShader(Color.Red, Color.Orange), CommonConditions.Events.SolarPillar, (ShaderLayer)4);
		RegisterShader("Nebula Pillar", (ChromaShader)(object)new PillarShader(new Color(255, 144, 209), new Color(100, 0, 76)), CommonConditions.Events.NebulaPillar, (ShaderLayer)4);
		RegisterShader("Vortex Pillar", (ChromaShader)(object)new PillarShader(Color.Green, Color.Black), CommonConditions.Events.VortexPillar, (ShaderLayer)4);
		RegisterShader("Stardust Pillar", (ChromaShader)(object)new PillarShader(new Color(46, 63, 255), Color.White), CommonConditions.Events.StardustPillar, (ShaderLayer)4);
		RegisterShader("Eater of Worlds", (ChromaShader)(object)new WormShader(new Color(14, 0, 15), new Color(47, 51, 59), new Color(20, 25, 11)), CommonConditions.Boss.EaterOfWorlds, (ShaderLayer)5);
		RegisterShader("Eye of Cthulhu", (ChromaShader)(object)new EyeOfCthulhuShader(new Color(145, 145, 126), new Color(138, 0, 0), new Color(3, 3, 18)), CommonConditions.Boss.EyeOfCthulhu, (ShaderLayer)5);
		RegisterShader("Skeletron", (ChromaShader)(object)new SkullShader(new Color(110, 92, 47), new Color(36, 32, 51), new Color(0, 0, 0)), CommonConditions.Boss.Skeletron, (ShaderLayer)5);
		RegisterShader("Brain Of Cthulhu", (ChromaShader)(object)new BrainShader(new Color(54, 0, 0), new Color(186, 137, 139)), CommonConditions.Boss.BrainOfCthulhu, (ShaderLayer)5);
		RegisterShader("Empress of Light", (ChromaShader)(object)new EmpressShader(), CommonConditions.Boss.Empress, (ShaderLayer)5);
		RegisterShader("Queen Slime", (ChromaShader)(object)new QueenSlimeShader(new Color(72, 41, 130), new Color(126, 220, 255)), CommonConditions.Boss.QueenSlime, (ShaderLayer)5);
		RegisterShader("King Slime", (ChromaShader)(object)new KingSlimeShader(new Color(41, 70, 130), Color.White), CommonConditions.Boss.KingSlime, (ShaderLayer)5);
		RegisterShader("Queen Bee", (ChromaShader)(object)new QueenBeeShader(new Color(5, 5, 0), new Color(255, 235, 0)), CommonConditions.Boss.QueenBee, (ShaderLayer)5);
		RegisterShader("Wall of Flesh", (ChromaShader)(object)new WallOfFleshShader(new Color(112, 48, 60), new Color(5, 0, 0)), CommonConditions.Boss.WallOfFlesh, (ShaderLayer)5);
		RegisterShader("Destroyer", (ChromaShader)(object)new WormShader(new Color(25, 25, 25), new Color(192, 0, 0), new Color(10, 0, 0)), CommonConditions.Boss.Destroyer, (ShaderLayer)5);
		RegisterShader("Skeletron Prime", (ChromaShader)(object)new SkullShader(new Color(110, 92, 47), new Color(79, 0, 0), new Color(255, 29, 0)), CommonConditions.Boss.SkeletronPrime, (ShaderLayer)5);
		RegisterShader("The Twins", (ChromaShader)(object)new TwinsShader(new Color(145, 145, 126), new Color(138, 0, 0), new Color(138, 0, 0), new Color(20, 20, 20), new Color(65, 140, 0), new Color(3, 3, 18)), CommonConditions.Boss.TheTwins, (ShaderLayer)5);
		RegisterShader("Duke Fishron", (ChromaShader)(object)new DukeFishronShader(new Color(0, 0, 122), new Color(100, 254, 194)), CommonConditions.Boss.DukeFishron, (ShaderLayer)5);
		RegisterShader("Deerclops", (ChromaShader)(object)new BlizzardShader(new Vector4(1f, 1f, 1f, 1f), new Vector4(0.15f, 0.1f, 0.4f, 1f), -0.1f, 0.4f), CommonConditions.Boss.Deerclops, (ShaderLayer)5);
		RegisterShader("Plantera", (ChromaShader)(object)new PlanteraShader(new Color(255, 0, 220), new Color(0, 255, 0), new Color(12, 4, 0)), CommonConditions.Boss.Plantera, (ShaderLayer)5);
		RegisterShader("Golem", (ChromaShader)(object)new GolemShader(new Color(255, 144, 0), new Color(255, 198, 0), new Color(10, 10, 0)), CommonConditions.Boss.Golem, (ShaderLayer)5);
		RegisterShader("Cultist", (ChromaShader)(object)new CultistShader(), CommonConditions.Boss.Cultist, (ShaderLayer)5);
		RegisterShader("Moon Lord", (ChromaShader)(object)new EyeballShader(isSpawning: false), CommonConditions.Boss.MoonLord, (ShaderLayer)5);
		RegisterShader("Rain", (ChromaShader)(object)new RainShader(), CommonConditions.Weather.Rain, (ShaderLayer)6);
		RegisterShader("Snowstorm", (ChromaShader)(object)new BlizzardShader(new Vector4(1f, 1f, 1f, 1f), new Vector4(0.1f, 0.1f, 0.3f, 1f), 0.35f, -0.35f), CommonConditions.Weather.Blizzard, (ShaderLayer)6);
		RegisterShader("Sandstorm", (ChromaShader)(object)new SandstormShader(), CommonConditions.Weather.Sandstorm, (ShaderLayer)6);
		RegisterShader("Slime Rain", (ChromaShader)(object)new SlimeRainShader(), CommonConditions.Weather.SlimeRain, (ShaderLayer)6);
		RegisterShader("Drowning", (ChromaShader)(object)new DrowningShader(), CommonConditions.Alert.Drowning, (ShaderLayer)7);
		RegisterShader("Keybinds", (ChromaShader)(object)new KeybindsMenuShader(), CommonConditions.Alert.Keybinds, (ShaderLayer)7);
		RegisterShader("Lava Indicator", (ChromaShader)(object)new LavaIndicatorShader(Color.Black, Color.Red, new Color(255, 188, 0)), CommonConditions.Alert.LavaIndicator, (ShaderLayer)7);
		RegisterShader("Moon Lord Spawn", (ChromaShader)(object)new EyeballShader(isSpawning: true), CommonConditions.Alert.MoonlordComing, (ShaderLayer)7);
		RegisterShader("Low Life", (ChromaShader)(object)new LowLifeShader(), CommonConditions.CriticalAlert.LowLife, (ShaderLayer)8);
		RegisterShader("Death", (ChromaShader)(object)new DeathShader(new Color(36, 0, 10), new Color(158, 28, 53)), CommonConditions.CriticalAlert.Death, (ShaderLayer)8);
	}

	private static void RegisterShader(string name, ChromaShader shader, ChromaCondition condition, ShaderLayer layer)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_engine.RegisterShader(shader, condition, layer);
	}

	[Conditional("DEBUG")]
	private static void AddDebugDraw()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		new BasicDebugDrawer(Main.instance.GraphicsDevice);
	}

	static ChromaInitializer()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected Obj, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected Obj, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected Obj, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected Obj, but got Unknown
	}
}
