using System;

namespace Terraria.Testing.Cloning;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class CloneByReference : Attribute
{
}
