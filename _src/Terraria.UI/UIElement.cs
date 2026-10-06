using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent.UI.Elements;

namespace Terraria.UI;

public class UIElement : IComparable
{
	public delegate void MouseEvent(UIMouseEvent evt, UIElement listeningElement);

	public delegate void ScrollWheelEvent(UIScrollWheelEvent evt, UIElement listeningElement);

	public delegate void ElementEvent(UIElement affectedElement);

	public delegate void DrawEvent(UIElement affectedElement, SpriteBatch sb);

	public delegate void UIElementAction(UIElement element);

	protected readonly List<UIElement> Elements = new List<UIElement>();

	public StyleDimension Top;

	public StyleDimension Left;

	public StyleDimension Width;

	public StyleDimension Height;

	public StyleDimension MaxWidth = StyleDimension.Fill;

	public StyleDimension MaxHeight = StyleDimension.Fill;

	public StyleDimension MinWidth = StyleDimension.Empty;

	public StyleDimension MinHeight = StyleDimension.Empty;

	private bool _isInitialized;

	public bool IgnoresMouseInteraction;

	public bool PassThroughMouseInteraction;

	public bool OverflowHidden;

	public SamplerState OverrideSamplerState;

	public float PaddingTop;

	public float PaddingLeft;

	public float PaddingRight;

	public float PaddingBottom;

	public float MarginTop;

	public float MarginLeft;

	public float MarginRight;

	public float MarginBottom;

	public float HAlign;

	public float VAlign;

	private CalculatedStyle _innerDimensions;

	private CalculatedStyle _dimensions;

	private CalculatedStyle _outerDimensions;

	private static readonly RasterizerState OverflowHiddenRasterizerState = new RasterizerState
	{
		CullMode = (CullMode)0,
		ScissorTestEnable = true
	};

	public bool UseImmediateMode;

	private SnapPoint _snapPoint;

	private static int _idCounter = 0;

	public UIElement Parent { get; private set; }

	public int UniqueId { get; private set; }

	public IEnumerable<UIElement> Children => Elements;

	public bool IsMouseHovering { get; private set; }

	public event MouseEvent OnLeftMouseDown;

	public event MouseEvent OnLeftMouseUp;

	public event MouseEvent OnLeftClick;

	public event MouseEvent OnLeftDoubleClick;

	public event MouseEvent OnRightMouseDown;

	public event MouseEvent OnRightMouseUp;

	public event MouseEvent OnRightClick;

	public event MouseEvent OnRightDoubleClick;

	public event MouseEvent OnMouseOver;

	public event MouseEvent OnMouseOut;

	public event ScrollWheelEvent OnScrollWheel;

	public event ElementEvent OnUpdate;

	public event DrawEvent OnDraw;

	public UIElement()
	{
		UniqueId = _idCounter++;
	}

	public void SetSnapPoint(string name, int id, Vector2? anchor = null, Vector2? offset = null)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!anchor.HasValue)
		{
			anchor = new Vector2(0.5f);
		}
		if (!offset.HasValue)
		{
			offset = Vector2.Zero;
		}
		_snapPoint = new SnapPoint(name, id, anchor.Value, offset.Value);
	}

	public bool GetSnapPoint(out SnapPoint point)
	{
		point = _snapPoint;
		if (_snapPoint != null)
		{
			_snapPoint.Calculate(this);
		}
		return _snapPoint != null;
	}

	public virtual void ExecuteRecursively(UIElementAction action)
	{
		action(this);
		foreach (UIElement element in Elements)
		{
			element.ExecuteRecursively(action);
		}
	}

	protected virtual void DrawSelf(SpriteBatch spriteBatch)
	{
	}

	protected virtual void DrawChildren(SpriteBatch spriteBatch)
	{
		foreach (UIElement element in Elements)
		{
			element.Draw(spriteBatch);
		}
	}

	public void Append(UIElement element)
	{
		element.Remove();
		element.Parent = this;
		Elements.Add(element);
		element.Recalculate();
	}

	public void Remove()
	{
		if (Parent != null)
		{
			Parent.RemoveChild(this);
		}
	}

	public void RemoveChild(UIElement child)
	{
		Elements.Remove(child);
		child.Parent = null;
	}

	public void RemoveAllChildren()
	{
		foreach (UIElement element in Elements)
		{
			element.Parent = null;
		}
		Elements.Clear();
	}

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		if (OnDraw != null)
		{
			OnDraw(this, spriteBatch);
		}
		bool overflowHidden = OverflowHidden;
		bool useImmediateMode = UseImmediateMode;
		RasterizerState rasterizerState = ((GraphicsResource)spriteBatch).GraphicsDevice.RasterizerState;
		Rectangle scissorRectangle = ((GraphicsResource)spriteBatch).GraphicsDevice.ScissorRectangle;
		SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;
		if (useImmediateMode || OverrideSamplerState != null)
		{
			spriteBatch.End();
			spriteBatch.Begin((SpriteSortMode)(useImmediateMode ? 1 : 0), BlendState.AlphaBlend, (OverrideSamplerState != null) ? OverrideSamplerState : anisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, (Effect)null, Main.UIScaleMatrix);
			DrawSelf(spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, (Effect)null, Main.UIScaleMatrix);
		}
		else
		{
			DrawSelf(spriteBatch);
		}
		if (overflowHidden)
		{
			spriteBatch.End();
			Rectangle clippingRectangle = GetClippingRectangle(spriteBatch);
			((GraphicsResource)spriteBatch).GraphicsDevice.ScissorRectangle = clippingRectangle;
			((GraphicsResource)spriteBatch).GraphicsDevice.RasterizerState = OverflowHiddenRasterizerState;
			spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, (Effect)null, Main.UIScaleMatrix);
		}
		DrawChildren(spriteBatch);
		if (overflowHidden)
		{
			spriteBatch.End();
			((GraphicsResource)spriteBatch).GraphicsDevice.ScissorRectangle = scissorRectangle;
			((GraphicsResource)spriteBatch).GraphicsDevice.RasterizerState = rasterizerState;
			spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, (Effect)null, Main.UIScaleMatrix);
		}
	}

	public virtual void Update(GameTime gameTime)
	{
		if (OnUpdate != null)
		{
			OnUpdate(this);
		}
		foreach (UIElement element in Elements)
		{
			element.Update(gameTime);
		}
	}

	public Rectangle GetClippingRectangle(SpriteBatch spriteBatch)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2(_innerDimensions.X, _innerDimensions.Y);
		Vector2 val2 = new Vector2(_innerDimensions.Width, _innerDimensions.Height) + val;
		val = Vector2.Transform(val, Main.UIScaleMatrix);
		val2 = Vector2.Transform(val2, Main.UIScaleMatrix);
		Rectangle val3 = new Rectangle((int)val.X, (int)val.Y, (int)(val2.X - val.X), (int)(val2.Y - val.Y));
		int num = (int)((float)Main.screenWidth * Main.UIScale);
		int num2 = (int)((float)Main.screenHeight * Main.UIScale);
		val3.X = Utils.Clamp(val3.X, 0, num);
		val3.Y = Utils.Clamp(val3.Y, 0, num2);
		val3.Width = Utils.Clamp(val3.Width, 0, num - val3.X);
		val3.Height = Utils.Clamp(val3.Height, 0, num2 - val3.Y);
		Rectangle scissorRectangle = ((GraphicsResource)spriteBatch).GraphicsDevice.ScissorRectangle;
		int num3 = Utils.Clamp(val3.Left, scissorRectangle.Left, scissorRectangle.Right);
		int num4 = Utils.Clamp(val3.Top, scissorRectangle.Top, scissorRectangle.Bottom);
		int num5 = Utils.Clamp(val3.Right, scissorRectangle.Left, scissorRectangle.Right);
		int num6 = Utils.Clamp(val3.Bottom, scissorRectangle.Top, scissorRectangle.Bottom);
		return new Rectangle(num3, num4, num5 - num3, num6 - num4);
	}

	public virtual List<SnapPoint> GetSnapPoints()
	{
		List<SnapPoint> list = new List<SnapPoint>();
		if (GetSnapPoint(out var point))
		{
			list.Add(point);
		}
		foreach (UIElement element in Elements)
		{
			list.AddRange(element.GetSnapPoints());
		}
		return list;
	}

	public virtual void Recalculate()
	{
		CalculatedStyle parentDimensions = ((Parent == null) ? UserInterface.ActiveInstance.GetDimensions() : Parent.GetInnerDimensions());
		if (Parent != null && Parent is UIList)
		{
			parentDimensions.Height = float.MaxValue;
		}
		CalculatedStyle calculatedStyle = (_outerDimensions = GetDimensionsBasedOnParentDimensions(parentDimensions));
		calculatedStyle.X += MarginLeft;
		calculatedStyle.Y += MarginTop;
		calculatedStyle.Width -= MarginLeft + MarginRight;
		calculatedStyle.Height -= MarginTop + MarginBottom;
		_dimensions = calculatedStyle;
		calculatedStyle.X += PaddingLeft;
		calculatedStyle.Y += PaddingTop;
		calculatedStyle.Width -= PaddingLeft + PaddingRight;
		calculatedStyle.Height -= PaddingTop + PaddingBottom;
		_innerDimensions = calculatedStyle;
		RecalculateChildren();
	}

	private CalculatedStyle GetDimensionsBasedOnParentDimensions(CalculatedStyle parentDimensions)
	{
		CalculatedStyle result = default;
		result.X = Left.GetValue(parentDimensions.Width) + parentDimensions.X;
		result.Y = Top.GetValue(parentDimensions.Height) + parentDimensions.Y;
		float value = MinWidth.GetValue(parentDimensions.Width);
		float value2 = MaxWidth.GetValue(parentDimensions.Width);
		float value3 = MinHeight.GetValue(parentDimensions.Height);
		float value4 = MaxHeight.GetValue(parentDimensions.Height);
		result.Width = MathHelper.Clamp(Width.GetValue(parentDimensions.Width), value, value2);
		result.Height = MathHelper.Clamp(Height.GetValue(parentDimensions.Height), value3, value4);
		result.Width += MarginLeft + MarginRight;
		result.Height += MarginTop + MarginBottom;
		result.X += parentDimensions.Width * HAlign - result.Width * HAlign;
		result.Y += parentDimensions.Height * VAlign - result.Height * VAlign;
		return result;
	}

	public UIElement GetElementAt(Vector2 point)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = null;
		for (int num = Elements.Count - 1; num >= 0; num--)
		{
			UIElement uIElement2 = Elements[num];
			if (!uIElement2.IgnoresMouseInteraction && uIElement2.ContainsPoint(point))
			{
				uIElement = uIElement2;
				if (!uIElement2.PassThroughMouseInteraction)
				{
					break;
				}
			}
		}
		if (uIElement != null)
		{
			return uIElement.GetElementAt(point);
		}
		if (IgnoresMouseInteraction)
		{
			return null;
		}
		if (ContainsPoint(point))
		{
			return this;
		}
		return null;
	}

	public virtual bool ContainsPoint(Vector2 point)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = _dimensions.ToRectangle();
		return val.Contains(point.ToPoint());
	}

	public virtual Rectangle GetViewCullingArea()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return _dimensions.ToRectangle();
	}

	public void SetPadding(float pixels)
	{
		PaddingBottom = pixels;
		PaddingLeft = pixels;
		PaddingRight = pixels;
		PaddingTop = pixels;
	}

	public virtual void RecalculateChildren()
	{
		foreach (UIElement element in Elements)
		{
			element.Recalculate();
		}
	}

	public CalculatedStyle GetInnerDimensions()
	{
		return _innerDimensions;
	}

	public CalculatedStyle GetDimensions()
	{
		return _dimensions;
	}

	public CalculatedStyle GetOuterDimensions()
	{
		return _outerDimensions;
	}

	public void CopyStyle(UIElement element)
	{
		Top = element.Top;
		Left = element.Left;
		Width = element.Width;
		Height = element.Height;
		PaddingBottom = element.PaddingBottom;
		PaddingLeft = element.PaddingLeft;
		PaddingRight = element.PaddingRight;
		PaddingTop = element.PaddingTop;
		HAlign = element.HAlign;
		VAlign = element.VAlign;
		MinWidth = element.MinWidth;
		MaxWidth = element.MaxWidth;
		MinHeight = element.MinHeight;
		MaxHeight = element.MaxHeight;
		Recalculate();
	}

	public virtual void LeftMouseDown(UIMouseEvent evt)
	{
		if (OnLeftMouseDown != null)
		{
			OnLeftMouseDown(evt, this);
		}
		if (Parent != null)
		{
			Parent.LeftMouseDown(evt);
		}
	}

	public virtual void LeftMouseUp(UIMouseEvent evt)
	{
		if (OnLeftMouseUp != null)
		{
			OnLeftMouseUp(evt, this);
		}
		if (Parent != null)
		{
			Parent.LeftMouseUp(evt);
		}
	}

	public virtual void LeftClick(UIMouseEvent evt)
	{
		if (OnLeftClick != null)
		{
			OnLeftClick(evt, this);
		}
		if (Parent != null)
		{
			Parent.LeftClick(evt);
		}
	}

	public virtual void LeftDoubleClick(UIMouseEvent evt)
	{
		if (OnLeftDoubleClick != null)
		{
			OnLeftDoubleClick(evt, this);
		}
		if (Parent != null)
		{
			Parent.LeftDoubleClick(evt);
		}
	}

	public virtual void RightMouseDown(UIMouseEvent evt)
	{
		if (OnRightMouseDown != null)
		{
			OnRightMouseDown(evt, this);
		}
		if (Parent != null)
		{
			Parent.RightMouseDown(evt);
		}
	}

	public virtual void RightMouseUp(UIMouseEvent evt)
	{
		if (OnRightMouseUp != null)
		{
			OnRightMouseUp(evt, this);
		}
		if (Parent != null)
		{
			Parent.RightMouseUp(evt);
		}
	}

	public virtual void RightClick(UIMouseEvent evt)
	{
		if (OnRightClick != null)
		{
			OnRightClick(evt, this);
		}
		if (Parent != null)
		{
			Parent.RightClick(evt);
		}
	}

	public virtual void RightDoubleClick(UIMouseEvent evt)
	{
		if (OnRightDoubleClick != null)
		{
			OnRightDoubleClick(evt, this);
		}
		if (Parent != null)
		{
			Parent.RightDoubleClick(evt);
		}
	}

	public virtual void MouseOver(UIMouseEvent evt)
	{
		IsMouseHovering = true;
		if (OnMouseOver != null)
		{
			OnMouseOver(evt, this);
		}
		if (Parent != null)
		{
			Parent.MouseOver(evt);
		}
	}

	public virtual void MouseOut(UIMouseEvent evt)
	{
		IsMouseHovering = false;
		if (OnMouseOut != null)
		{
			OnMouseOut(evt, this);
		}
		if (Parent != null)
		{
			Parent.MouseOut(evt);
		}
	}

	public virtual void ScrollWheel(UIScrollWheelEvent evt)
	{
		if (OnScrollWheel != null)
		{
			OnScrollWheel(evt, this);
		}
		if (Parent != null)
		{
			Parent.ScrollWheel(evt);
		}
	}

	public void Activate()
	{
		if (!_isInitialized)
		{
			Initialize();
		}
		OnActivate();
		foreach (UIElement element in Elements)
		{
			element.Activate();
		}
	}

	public virtual void OnActivate()
	{
	}

	[Conditional("DEBUG")]
	public void DrawDebugHitbox(BasicDebugDrawer drawer, float colorIntensity = 0f)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		if (IsMouseHovering)
		{
			colorIntensity += 0.1f;
		}
		Color val = Main.hslToRgb(colorIntensity, colorIntensity, 0.5f);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		drawer.DrawLine(innerDimensions.Position(), innerDimensions.Position() + new Vector2(innerDimensions.Width, 0f), 2f, val);
		drawer.DrawLine(innerDimensions.Position() + new Vector2(innerDimensions.Width, 0f), innerDimensions.Position() + new Vector2(innerDimensions.Width, innerDimensions.Height), 2f, val);
		drawer.DrawLine(innerDimensions.Position() + new Vector2(innerDimensions.Width, innerDimensions.Height), innerDimensions.Position() + new Vector2(0f, innerDimensions.Height), 2f, val);
		drawer.DrawLine(innerDimensions.Position() + new Vector2(0f, innerDimensions.Height), innerDimensions.Position(), 2f, val);
		foreach (UIElement element in Elements)
		{
			_ = element;
		}
	}

	public void Deactivate()
	{
		OnDeactivate();
		foreach (UIElement element in Elements)
		{
			element.Deactivate();
		}
	}

	public virtual void OnDeactivate()
	{
	}

	public void Initialize()
	{
		OnInitialize();
		_isInitialized = true;
	}

	public virtual void OnInitialize()
	{
	}

	public virtual int CompareTo(object obj)
	{
		return 0;
	}

	static UIElement()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected Obj, but got Unknown
	}
}
