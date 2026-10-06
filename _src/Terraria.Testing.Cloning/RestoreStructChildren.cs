namespace Terraria.Testing.Cloning;

public delegate void RestoreStructChildren<T>(ref T clone, ref T current, DeepCloneContext ctx) where T : struct;
