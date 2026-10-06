using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Terraria.Utilities;

namespace Terraria.Testing;

public static class GitStatus
{
	private static string _gitSHA = "";

	private static bool _init = true;

	public static string GitSHA
	{
		get
		{
			Init();
			return _gitSHA;
		}
	}

	private static void Init()
	{
		if (_init)
		{
			return;
		}
		_init = true;
		if (!HasGitFolder())
		{
			return;
		}
		try
		{
			_gitSHA = GitRevParse();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "git command failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	private static string GitRevParse()
	{
		using Process process = new Process();
		process.StartInfo = new ProcessStartInfo("git", "rev-parse HEAD")
		{
			UseShellExecute = false,
			RedirectStandardOutput = true,
			CreateNoWindow = true
		};
		process.Start();
		string text = process.StandardOutput.ReadToEnd().Trim();
		if (!Regex.IsMatch(text, "^[0-9a-f]+$"))
		{
			throw new Exception(text);
		}
		return text;
	}

	private static bool HasGitFolder()
	{
		try
		{
			string directoryName = Path.GetDirectoryName(Path.GetFullPath("."));
			do
			{
				if (Directory.Exists(Path.Combine(directoryName, ".git")))
				{
					return true;
				}
			}
			while ((directoryName = Path.GetDirectoryName(directoryName)) != null);
		}
		catch (Exception)
		{
		}
		return false;
	}
}
