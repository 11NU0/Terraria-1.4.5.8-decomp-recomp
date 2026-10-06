namespace Terraria.Testing.Cloning;

public delegate void CloneStructChildren<T>(ref T clone, DeepCloneContext ctx) where T : struct;
