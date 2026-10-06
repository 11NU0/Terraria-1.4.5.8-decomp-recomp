using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.Testing;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct UIGamepadHelper
{
	public UILinkPoint[,] CreateUILinkPointGrid(ref int currentID, List<SnapPoint> pointsForGrid, int pointsPerLine, UILinkPoint topLinkPoint, UILinkPoint leftLinkPoint, UILinkPoint rightLinkPoint, UILinkPoint bottomLinkPoint)
	{
		int num = (int)Math.Ceiling((float)pointsForGrid.Count / (float)pointsPerLine);
		UILinkPoint[,] array = new UILinkPoint[pointsPerLine, num];
		for (int i = 0; i < pointsForGrid.Count; i++)
		{
			int num2 = i % pointsPerLine;
			int num3 = i / pointsPerLine;
			array[num2, num3] = MakeLinkPointFromSnapPoint(currentID++, pointsForGrid[i]);
		}
		for (int j = 0; j < array.GetLength(0); j++)
		{
			for (int k = 0; k < array.GetLength(1); k++)
			{
				UILinkPoint uILinkPoint = array[j, k];
				if (uILinkPoint == null)
				{
					continue;
				}
				if (j < array.GetLength(0) - 1)
				{
					UILinkPoint uILinkPoint2 = array[j + 1, k];
					if (uILinkPoint2 != null)
					{
						PairLeftRight(uILinkPoint, uILinkPoint2);
					}
				}
				if (k < array.GetLength(1) - 1)
				{
					UILinkPoint uILinkPoint3 = array[j, k + 1];
					if (uILinkPoint3 != null)
					{
						PairUpDown(uILinkPoint, uILinkPoint3);
					}
				}
				if (leftLinkPoint != null && j == 0)
				{
					uILinkPoint.Left = leftLinkPoint.ID;
				}
				if (topLinkPoint != null && k == 0)
				{
					uILinkPoint.Up = topLinkPoint.ID;
				}
				if (rightLinkPoint != null && j == pointsPerLine - 1)
				{
					uILinkPoint.Right = rightLinkPoint.ID;
				}
				if (bottomLinkPoint != null && k == num - 1)
				{
					uILinkPoint.Down = bottomLinkPoint.ID;
				}
			}
		}
		return array;
	}

	public void LinkVerticalStrips(UILinkPoint[] stripOnLeft, UILinkPoint[] stripOnRight, int leftStripStartOffset)
	{
		if (stripOnLeft == null || stripOnRight == null)
		{
			return;
		}
		int num = Math.Max(stripOnLeft.Length, stripOnRight.Length);
		int num2 = Math.Min(stripOnLeft.Length, stripOnRight.Length);
		for (int i = 0; i < leftStripStartOffset; i++)
		{
			PairLeftRight(stripOnLeft[i], stripOnRight[0]);
		}
		for (int j = 0; j < num2; j++)
		{
			PairLeftRight(stripOnLeft[j + leftStripStartOffset], stripOnRight[j]);
		}
		for (int k = num2; k < num; k++)
		{
			if (stripOnLeft.Length > k)
			{
				stripOnLeft[k].Right = stripOnRight[stripOnRight.Length - 1].ID;
			}
			if (stripOnRight.Length > k)
			{
				stripOnRight[k].Left = stripOnLeft[stripOnLeft.Length - 1].ID;
			}
		}
	}

	public void LinkVerticalStripRightSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
	{
		if (strip == null || theSingle == null)
		{
			return;
		}
		int num = Math.Max(strip.Length, 1);
		int num2 = Math.Min(strip.Length, 1);
		for (int i = 0; i < num2; i++)
		{
			PairLeftRight(strip[i], theSingle);
		}
		for (int j = num2; j < num; j++)
		{
			if (strip.Length > j)
			{
				strip[j].Right = theSingle.ID;
			}
		}
	}

	public void RemovePointsOutOfView(List<SnapPoint> pts, UIElement containerPanel, SpriteBatch spriteBatch)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f / Main.UIScale;
		Rectangle clippingRectangle = containerPanel.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num;
		Vector2 maximum = clippingRectangle.BottomRight() * num;
		for (int i = 0; i < pts.Count; i++)
		{
			if (!pts[i].Position.Between(minimum, maximum))
			{
				pts.Remove(pts[i]);
				i--;
			}
		}
	}

	public void LinkHorizontalStripBottomSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
	{
		if (strip != null && theSingle != null)
		{
			for (int num = strip.Length - 1; num >= 0; num--)
			{
				PairUpDown(strip[num], theSingle);
			}
		}
	}

	public void LinkHorizontalStripUpSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
	{
		if (strip != null && theSingle != null)
		{
			for (int num = strip.Length - 1; num >= 0; num--)
			{
				PairUpDown(theSingle, strip[num]);
			}
		}
	}

	public void LinkVerticalStripBottomSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
	{
		if (strip != null && theSingle != null)
		{
			PairUpDown(strip[strip.Length - 1], theSingle);
		}
	}

	public UILinkPoint[] CreateUILinkStripVertical(ref int currentID, List<SnapPoint> currentStrip)
	{
		UILinkPoint[] array = new UILinkPoint[currentStrip.Count];
		for (int i = 0; i < currentStrip.Count; i++)
		{
			array[i] = MakeLinkPointFromSnapPoint(currentID++, currentStrip[i]);
		}
		for (int j = 0; j < currentStrip.Count - 1; j++)
		{
			PairUpDown(array[j], array[j + 1]);
		}
		return array;
	}

	public UILinkPoint[] CreateUILinkStripHorizontal(ref int currentID, List<SnapPoint> currentStrip)
	{
		UILinkPoint[] array = new UILinkPoint[currentStrip.Count];
		for (int i = 0; i < currentStrip.Count; i++)
		{
			array[i] = MakeLinkPointFromSnapPoint(currentID++, currentStrip[i]);
		}
		for (int j = 0; j < currentStrip.Count - 1; j++)
		{
			PairLeftRight(array[j], array[j + 1]);
		}
		return array;
	}

	public void TryMovingBackIntoCreativeGridIfOutOfIt(int start, int currentID)
	{
		List<UILinkPoint> list = new List<UILinkPoint>();
		for (int i = start; i < currentID; i++)
		{
			list.Add(UILinkPointNavigator.Points[i]);
		}
		if (PlayerInput.UsingGamepadUI && UILinkPointNavigator.CurrentPoint >= currentID)
		{
			MoveToVisuallyClosestPoint(list);
		}
	}

	public void MoveToVisuallyClosestPoint(List<UILinkPoint> lostrefpoints)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_ = UILinkPointNavigator.Points;
		Vector2 mouseScreen = Main.MouseScreen;
		UILinkPoint uILinkPoint = null;
		foreach (UILinkPoint lostrefpoint in lostrefpoints)
		{
			if (uILinkPoint == null || Vector2.Distance(mouseScreen, uILinkPoint.Position) > Vector2.Distance(mouseScreen, lostrefpoint.Position))
			{
				uILinkPoint = lostrefpoint;
			}
		}
		if (uILinkPoint != null)
		{
			UILinkPointNavigator.ChangePoint(uILinkPoint.ID);
		}
	}

	public List<SnapPoint> GetOrderedPointsByCategoryName(List<SnapPoint> pts, string name)
	{
		return (from x in pts
			where x.Name == name
			orderby x.Id
			select x).ToList();
	}

	public void PairLeftRight(UILinkPoint leftSide, UILinkPoint rightSide)
	{
		if (leftSide != null)
		{
			leftSide.Right = rightSide?.ID ?? (-1);
		}
		if (rightSide != null)
		{
			rightSide.Left = leftSide?.ID ?? (-1);
		}
	}

	public void PairUpDown(UILinkPoint upSide, UILinkPoint downSide)
	{
		if (upSide != null)
		{
			upSide.Down = downSide?.ID ?? (-1);
		}
		if (downSide != null)
		{
			downSide.Up = upSide?.ID ?? (-1);
		}
	}

	public UILinkPoint MakeLinkPointFromSnapPoint(int id, SnapPoint snap)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.SetPosition(id, snap.Position);
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[id];
		uILinkPoint.Unlink();
		return uILinkPoint;
	}

	public UILinkPoint GetLinkPoint(int id, UIElement element)
	{
		if (element.GetSnapPoint(out var point))
		{
			return MakeLinkPointFromSnapPoint(id, point);
		}
		return null;
	}

	public UILinkPoint TryMakeLinkPoint(ref int id, SnapPoint snap)
	{
		if (snap == null)
		{
			return null;
		}
		return MakeLinkPointFromSnapPoint(id++, snap);
	}

	public UILinkPoint[] GetVerticalStripFromCategoryName(ref int currentID, List<SnapPoint> pts, string categoryName)
	{
		List<SnapPoint> orderedPointsByCategoryName = GetOrderedPointsByCategoryName(pts, categoryName);
		UILinkPoint[] result = null;
		if (orderedPointsByCategoryName.Count > 0)
		{
			result = CreateUILinkStripVertical(ref currentID, orderedPointsByCategoryName);
		}
		return result;
	}

	public void MoveToVisuallyClosestPoint(int idRangeStartInclusive, int idRangeEndExclusive)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (UILinkPointNavigator.CurrentPoint >= idRangeStartInclusive && UILinkPointNavigator.CurrentPoint < idRangeEndExclusive)
		{
			return;
		}
		Dictionary<int, UILinkPoint> points = UILinkPointNavigator.Points;
		Vector2 mouseScreen = Main.MouseScreen;
		UILinkPoint uILinkPoint = null;
		for (int i = idRangeStartInclusive; i < idRangeEndExclusive; i++)
		{
			if (!points.TryGetValue(i, out var value))
			{
				return;
			}
			if (uILinkPoint == null || Vector2.Distance(mouseScreen, uILinkPoint.Position) > Vector2.Distance(mouseScreen, value.Position))
			{
				uILinkPoint = value;
			}
		}
		if (uILinkPoint != null)
		{
			UILinkPointNavigator.ChangePoint(uILinkPoint.ID);
		}
	}

	public void CullPointsOutOfElementArea(SpriteBatch spriteBatch, List<SnapPoint> pointsAtMiddle, UIElement container)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f / Main.UIScale;
		Rectangle clippingRectangle = container.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num;
		Vector2 maximum = clippingRectangle.BottomRight() * num;
		for (int i = 0; i < pointsAtMiddle.Count; i++)
		{
			if (!pointsAtMiddle[i].Position.Between(minimum, maximum))
			{
				pointsAtMiddle.Remove(pointsAtMiddle[i]);
				i--;
			}
		}
	}

	public void KeepCurrentPointInRangeAfterGridScroll(UILinkPoint[,] grid)
	{
		int length = grid.GetLength(0);
		int length2 = grid.GetLength(1);
		if (length == 0 || length2 == 0 || grid[0, 0] == null)
		{
			return;
		}
		int iD = grid[0, 0].ID;
		int num = UILinkPointNavigator.CurrentPoint - iD;
		if (num < 0 || num >= length * (length2 + 1))
		{
			return;
		}
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[UILinkPointNavigator.CurrentPoint];
		_ = uILinkPoint.Page;
		UILinkPoint uILinkPoint2 = ((num < length * length2) ? grid[num % length, num / length] : null);
		if (uILinkPoint2 != null)
		{
			Invariant.Assert(uILinkPoint2 == uILinkPoint, "Grid point mismatch");
			return;
		}
		Invariant.Assert(uILinkPoint.Page == grid[0, 0].Page, "Page mismatch");
		if (uILinkPoint.Page == grid[0, 0].Page && (uILinkPoint.Up >= 0 || uILinkPoint.Up < iD || uILinkPoint.Up >= UILinkPointNavigator.CurrentPoint))
		{
			UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[uILinkPoint.Up];
			Invariant.Assert(uILinkPoint.Page == uILinkPoint3.Page, "Page mismatch 2");
			UILinkPointNavigator.ChangePoint(uILinkPoint3.ID);
		}
	}
}
