using System;

namespace Terraria.Utilities;

public static class PlatformUtilities
{
	public static void SavePng(string path, int width, int height, byte[] data)
	{
		throw new NotSupportedException("Use Bitmap to save png images on windows");
	}
}
