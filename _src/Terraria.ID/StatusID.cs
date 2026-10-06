using ReLogic.Reflection;

namespace Terraria.ID;

public class StatusID
{
	public static readonly int Ok = 0;

	public static readonly int LaterVersion = 1;

	public static readonly int UnknownError = 2;

	public static readonly int EmptyFile = 3;

	public static readonly int DecryptionError = 4;

	public static readonly int BadSectionPointer = 5;

	public static readonly int BadFooter = 6;

	public static readonly IdDictionary Search = IdDictionary.Create<StatusID, int>();
}
