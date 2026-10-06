using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.Graphics.Renderers;

public class ItemTransferParticle : IPooledParticle, IParticle
{
	private Vector2 StartPosition;

	private Vector2 EndPosition;

	private Vector2 StartOffset;

	private Vector2 EndOffset;

	private Vector2 BezierHelper1;

	private Vector2 BezierHelper2;

	private bool TransitionIn;

	private bool Fullbright;

	private bool InInventory;

	private Item _itemInstance;

	private int _lifeTimeCounted;

	private int _lifeTimeTotal;

	public bool ShouldBeRemovedFromRenderer { get; private set; }

	public bool IsRestingInPool { get; private set; }

	public ItemTransferParticle()
	{
		_itemInstance = new Item();
	}

	public void Update(ref ParticleRendererSettings settings)
	{
		if (++_lifeTimeCounted >= _lifeTimeTotal)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public void Prepare(int itemType, int lifeTimeTotal, Vector2 startPosition, Vector2 endPosition, Vector2 offsetStart, Vector2 offsetEnd, bool transitionIn, bool fullbright, bool inInventory, int stack = 1)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		_itemInstance.SetDefaults(itemType);
		_itemInstance.stack = stack;
		_lifeTimeTotal = lifeTimeTotal;
		StartPosition = startPosition;
		StartOffset = offsetStart;
		EndPosition = endPosition;
		EndOffset = offsetEnd;
		TransitionIn = transitionIn;
		Fullbright = fullbright;
		InInventory = inInventory;
		Vector2 val = (EndPosition - StartPosition).SafeNormalize(Vector2.UnitY).RotatedBy(1.5707963705062866);
		bool flag = val.Y < 0f;
		bool flag2 = val.Y == 0f;
		if (!flag || (flag2 && Main.rand.Next(2) == 0))
		{
			val *= -1f;
		}
		val = new Vector2(0f, -1f);
		float num = Vector2.Distance(EndPosition, StartPosition);
		BezierHelper1 = val * num + Main.rand.NextVector2Circular(32f, 32f);
		BezierHelper2 = -val * num + Main.rand.NextVector2Circular(32f, 32f);
	}

	public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)_lifeTimeCounted / (float)_lifeTimeTotal;
		float toMin = Utils.Remap(num, 0.1f, 0.5f, 0f, 0.85f);
		toMin = Utils.Remap(num, 0.5f, 0.9f, toMin, 1f);
		Vector2 val = default;
		Vector2.Hermite(ref StartPosition, ref BezierHelper1, ref EndPosition, ref BezierHelper2, toMin, out val);
		Vector2 zero = Vector2.Zero;
		if (num <= 0.15f)
		{
			zero = Vector2.Lerp(Vector2.Zero, StartOffset, num / 0.15f);
		}
		else if (num <= 0.5f)
		{
			zero = Vector2.Lerp(StartOffset, EndOffset, (num - 0.15f) / 0.35f);
		}
		else
		{
			zero = ((!(num <= 0.85f)) ? Vector2.Lerp(EndOffset, Vector2.Zero, Utils.Remap(num, 0.85f, 0.95f, 0f, 1f)) : EndOffset);
		}
		val += zero;
		float num2 = Utils.Remap(num, 0f, 0.15f, (!TransitionIn) ? 1 : 0, 1f) * Utils.Remap(num, 0.85f, 0.95f, 1f, 0f);
		Color val2 = (Fullbright ? Color.White : Lighting.GetColor(val.ToTileCoordinates()));
		int context = 31;
		int num3 = 32;
		if (InInventory)
		{
			num3 = 32;
			num2 = 1f;
			float num4 = num;
			num4 *= num4;
			val = Vector2.Lerp(StartPosition - new Vector2(26f, 26f) * Main.inventoryScale, EndPosition - new Vector2(26f, 26f) * Main.inventoryScale, num4);
			context = 14;
		}
		if (InInventory)
		{
			ItemSlot.Draw(spritebatch, ref _itemInstance, context, settings.AnchorPosition + val, val2);
		}
		else
		{
			ItemSlot.DrawItemIcon(_itemInstance, context, Main.spriteBatch, settings.AnchorPosition + val, _itemInstance.scale * num2, num3, val2);
		}
	}

	public void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void FetchFromPool()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		_lifeTimeCounted = 0;
		_lifeTimeTotal = 0;
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
		StartPosition = (EndPosition = (BezierHelper1 = (BezierHelper2 = Vector2.Zero)));
	}
}
