using System;
using System.Text;
using BCrypt.Net;

namespace Terraria.Utilities;

public static class Secrets
{
	private static readonly byte[] _salt;

	static Secrets()
	{
		_salt = Convert.FromBase64String("fT2JQQzNMJl2NRoMbo9RjA==");
	}

	public static string ToSecret(string plainInput)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainInput);
		bytes = new BCrypt.Net.BCrypt().CryptRaw(bytes, _salt, 4);
		for (int i = 0; i < 1000; i++)
		{
			int num = i % bytes.Length;
			int num2 = bytes[num] % bytes.Length;
			Utils.Swap(ref bytes[num], ref bytes[num2]);
		}
		bytes = new BCrypt.Net.BCrypt().CryptRaw(bytes, _salt, 4);
		return Convert.ToBase64String(bytes);
	}
}
