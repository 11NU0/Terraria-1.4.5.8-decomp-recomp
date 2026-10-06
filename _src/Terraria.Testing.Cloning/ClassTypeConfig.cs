using System;

namespace Terraria.Testing.Cloning;

internal sealed class ClassTypeConfig
{
	public readonly Type Type;

	public bool CloneByReference;

	public bool PrivateFieldsUnique;

	private ClassTypeInfo _frozen;

	public bool IsFrozen => _frozen != null;

	public ClassTypeConfig(Type type)
	{
		Type = type;
	}

	public ClassTypeInfo Freeze()
	{
		if (_frozen == null)
		{
			_frozen = new ClassTypeInfo(Type, this, ref _frozen);
		}
		return _frozen;
	}
}
