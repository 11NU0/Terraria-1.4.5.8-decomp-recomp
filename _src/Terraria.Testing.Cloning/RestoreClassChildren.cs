namespace Terraria.Testing.Cloning;

public delegate void RestoreClassChildren<T>(T clone, T current, DeepCloneContext ctx) where T : class;
