using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Testing.Cloning;

namespace Terraria.Testing;

public class StateSnapshotDefinition
{
	public abstract class Component
	{
		private readonly string Name;

		protected Component(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}

		public abstract object Backup(DeepCloneContext ctx);

		public abstract void Restore(object clone, DeepCloneContext ctx);
	}

	public class ReferenceComponent<T> : Component where T : class
	{
		private readonly Func<T> _get;

		private readonly Action<T> _set;

		public ReferenceComponent(string name, Func<T> get, Action<T> set)
			: base(name)
		{
			_get = get;
			_set = set;
		}

		public override object Backup(DeepCloneContext ctx)
		{
			return DeepCloning.Clone(_get(), ctx);
		}

		public void Restore(T clone, DeepCloneContext ctx)
		{
			_set(DeepCloning.Restore(clone, _get(), ctx));
		}

		public override void Restore(object clone, DeepCloneContext ctx)
		{
			Restore((T)clone, ctx);
		}
	}

	public class ValueComponent<T> : Component where T : struct
	{
		private readonly Func<T> _get;

		private readonly Action<T> _set;

		public ValueComponent(string name, Func<T> get, Action<T> set)
			: base(name)
		{
			_get = get;
			_set = set;
		}

		public override object Backup(DeepCloneContext ctx)
		{
			return DeepCloning.CloneStruct(_get(), ctx);
		}

		public override void Restore(object clone, DeepCloneContext ctx)
		{
			_set(DeepCloning.RestoreStruct((T)clone, _get(), ctx));
		}
	}

	public class ListComponent<T> : Component
	{
		private readonly List<T> _list;

		public ListComponent(string name, List<T> list)
			: base(name)
		{
			_list = list;
		}

		public override object Backup(DeepCloneContext ctx)
		{
			T[] array = _list.ToArray();
			DeepCloning.CloneChildren(array, ctx);
			return array;
		}

		public override void Restore(object clone, DeepCloneContext ctx)
		{
			Restore((T[])clone, ctx);
		}

		private void Restore(T[] clone, DeepCloneContext ctx)
		{
			_list.Clear();
			_list.AddRange(DeepCloning.Restore(clone, null, ctx));
		}
	}

	public class ArrayComponent : Component
	{
		private readonly Array _array;

		public ArrayComponent(string name, Array array)
			: base(name)
		{
			_array = array;
		}

		public override object Backup(DeepCloneContext ctx)
		{
			return DeepCloning.Clone(_array, ctx);
		}

		public override void Restore(object clone, DeepCloneContext ctx)
		{
			Restore((Array)clone, ctx);
		}

		private void Restore(Array clone, DeepCloneContext ctx)
		{
			Array.Copy(DeepCloning.Restore(clone, _array, ctx), _array, clone.Length);
		}
	}

	private readonly List<Component> _components = new List<Component>();

	public void AddRef<T>(string name, Func<T> get, Action<T> set) where T : class
	{
		Add(new ReferenceComponent<T>(name, get, set));
	}

	public void AddVal<T>(string name, Func<T> get, Action<T> set) where T : struct
	{
		Add(new ValueComponent<T>(name, get, set));
	}

	public void AddCollection<T>(string name, List<T> inst)
	{
		Add(new ListComponent<T>(name, inst));
	}

	public void AddCollection(string name, Array inst)
	{
		Add(new ArrayComponent(name, inst));
	}

	public void Add(Component c)
	{
		_components.Add(c);
	}

	public StateSnapshot Capture()
	{
		DeepCloneContext ctx = new DeepCloneContext(isBackup: true);
		StateSnapshot result = new StateSnapshot(this, _components.Select((Component c) => c.Backup(ctx)).ToArray());
		ctx.DumpMap();
		return result;
	}

	public void Restore(object[] clones)
	{
		DeepCloneContext deepCloneContext = new DeepCloneContext();
		for (int i = 0; i < _components.Count; i++)
		{
			_components[i].Restore(clones[i], deepCloneContext);
		}
		deepCloneContext.DumpMap();
	}
}
