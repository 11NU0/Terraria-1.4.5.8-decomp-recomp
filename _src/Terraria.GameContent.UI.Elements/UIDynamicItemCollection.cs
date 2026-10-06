using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public abstract class UIDynamicItemCollection : UIElement
{
	public const string SnapPointName_ItemSlot = "DynamicItemCollectionSlot";
}
public abstract class UIDynamicItemCollection<TEntry> : UIDynamicItemCollection
{
	private List<TEntry> _contents = new List<TEntry>();

	private int _itemsPerLine;

	private const int sizePerEntryX = 44;

	private const int sizePerEntryY = 44;

	private List<SnapPoint> _dummySnapPoints = new List<SnapPoint>();

	public int Count => _contents.Count;

	public UIDynamicItemCollection()
	{
		Width = new StyleDimension(0f, 1f);
		HAlign = 0.5f;
		UpdateSize();
	}

	protected abstract Item GetItem(TEntry entry);

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		Main.inventoryScale = 3465f / 4095f;
		GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
		int num = _itemsPerLine;
		Point val = UserInterface.ActiveInstance.MousePosition.ToPoint();
		for (int i = startItemIndex; i < endItemIndex; i++)
		{
			TEntry entry = _contents[i];
			Rectangle itemSlotHitbox = GetItemSlotHitbox(startX, startY, startItemIndex, i);
			if ((int)TextureAssets.Item[GetItem(entry).type].State == 0)
			{
				num--;
			}
			bool hovering = IsMouseHovering && itemSlotHitbox.Contains(val) && !PlayerInput.IgnoreMouseInterface;
			DrawSlot(spriteBatch, entry, itemSlotHitbox.TopLeft(), hovering);
			if (num <= 0)
			{
				break;
			}
		}
		for (int j = 0; j < _contents.Count; j++)
		{
			if (num <= 0)
			{
				break;
			}
			Item item = GetItem(_contents[(j + endItemIndex) % _contents.Count]);
			if ((int)TextureAssets.Item[item.type].State == 0)
			{
				Main.instance.LoadItem(item.type);
				num -= 4;
			}
		}
	}

	protected abstract void DrawSlot(SpriteBatch spriteBatch, TEntry entry, Vector2 pos, bool hovering);

	private Rectangle GetItemSlotHitbox(int startX, int startY, int startItemIndex, int i)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		int num = i - startItemIndex;
		int num2 = num % _itemsPerLine;
		int num3 = num / _itemsPerLine;
		return new Rectangle(startX + num2 * 44, startY + num3 * 44, 44, 44);
	}

	private void GetGridParameters(out int startX, out int startY, out int startItemIndex, out int endItemIndex)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = GetDimensions().ToRectangle();
		Rectangle viewCullingArea = Parent.GetViewCullingArea();
		int x = val.Center.X;
		startX = x - (int)((float)(44 * _itemsPerLine) * 0.5f);
		startY = val.Top;
		startItemIndex = 0;
		endItemIndex = _contents.Count;
		int num = (Math.Min(viewCullingArea.Top, val.Top) - viewCullingArea.Top) / 44;
		startY += -num * 44;
		startItemIndex += -num * _itemsPerLine;
		int num2 = (int)Math.Ceiling((float)viewCullingArea.Height / 44f) * _itemsPerLine;
		if (endItemIndex > num2 + startItemIndex + _itemsPerLine)
		{
			endItemIndex = num2 + startItemIndex + _itemsPerLine;
		}
	}

	public override void Recalculate()
	{
		base.Recalculate();
		UpdateSize();
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (IsMouseHovering)
		{
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	public void SetContentsToShow(List<TEntry> itemsToShow)
	{
		_contents.Clear();
		_contents.AddRange(itemsToShow);
		UpdateSize();
	}

	public int GetItemsPerLine()
	{
		return _itemsPerLine;
	}

	public override List<SnapPoint> GetSnapPoints()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		List<SnapPoint> list = new List<SnapPoint>();
		GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
		Rectangle viewCullingArea = Parent.GetViewCullingArea();
		int num = endItemIndex - startItemIndex;
		while (_dummySnapPoints.Count < num)
		{
			_dummySnapPoints.Add(new SnapPoint("DynamicItemCollectionSlot", 0, Vector2.Zero, Vector2.Zero));
		}
		int num2 = 0;
		Vector2 val = GetDimensions().Position();
		Vector2 val2 = TextureAssets.InventoryBack.Size() * Main.inventoryScale;
		viewCullingArea.Y += (int)(val2.Y * 0.25f);
		viewCullingArea.Height -= (int)(val2.Y * 0.25f);
		for (int i = startItemIndex; i < endItemIndex; i++)
		{
			Vector2 val3 = GetItemSlotHitbox(startX, startY, startItemIndex, i).TopLeft() + val2 * 0.75f;
			if (viewCullingArea.Contains(val3.ToPoint()))
			{
				SnapPoint snapPoint = _dummySnapPoints[num2];
				snapPoint.ThisIsAHackThatChangesTheSnapPointsInfo(Vector2.Zero, val3 - val, i);
				snapPoint.Calculate(this);
				num2++;
				list.Add(snapPoint);
			}
		}
		foreach (UIElement element in Elements)
		{
			list.AddRange(element.GetSnapPoints());
		}
		return list;
	}

	public void UpdateSize()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		int num = (_itemsPerLine = GetDimensions().ToRectangle().Width / 44);
		int num2 = (int)Math.Ceiling((float)_contents.Count / (float)num);
		MinHeight.Set(44 * num2, 0f);
	}
}
