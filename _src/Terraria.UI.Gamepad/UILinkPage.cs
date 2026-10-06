using System;
using System.Collections.Generic;

namespace Terraria.UI.Gamepad;

public class UILinkPage
{
	public int ID;

	public int PageOnLeft = -1;

	public int PageOnRight = -1;

	public int DefaultPoint;

	public int CurrentPoint;

	public Dictionary<int, UILinkPoint> LinkMap = new Dictionary<int, UILinkPoint>();

	public event Action<int, int> ReachEndEvent;

	public event Action TravelEvent;

	public event Action LeaveEvent;

	public event Action EnterEvent;

	public event Action UpdateEvent;

	public event Func<bool> IsValidEvent;

	public event Func<bool> CanEnterEvent;

	public event Action<int> OnPageMoveAttempt;

	public event Func<string> OnSpecialInteracts;

	public event Func<string> OnSpecialInteractsLate;

	public UILinkPage()
	{
	}

	public UILinkPage(int id)
	{
		ID = id;
	}

	public void Update()
	{
		if (UpdateEvent != null)
		{
			UpdateEvent();
		}
	}

	public void Leave()
	{
		if (LeaveEvent != null)
		{
			LeaveEvent();
		}
	}

	public void Enter()
	{
		if (EnterEvent != null)
		{
			EnterEvent();
		}
	}

	public bool IsValid()
	{
		if (IsValidEvent != null)
		{
			return IsValidEvent();
		}
		return true;
	}

	public bool CanEnter()
	{
		if (CanEnterEvent != null)
		{
			return CanEnterEvent();
		}
		return true;
	}

	public void TravelUp()
	{
		Travel(LinkMap[CurrentPoint].Up);
	}

	public void TravelDown()
	{
		Travel(LinkMap[CurrentPoint].Down);
	}

	public void TravelLeft()
	{
		Travel(LinkMap[CurrentPoint].Left);
	}

	public void TravelRight()
	{
		Travel(LinkMap[CurrentPoint].Right);
	}

	public void SwapPageLeft()
	{
		if (OnPageMoveAttempt != null)
		{
			OnPageMoveAttempt(-1);
		}
		UILinkPointNavigator.ChangePage(PageOnLeft);
	}

	public void SwapPageRight()
	{
		if (OnPageMoveAttempt != null)
		{
			OnPageMoveAttempt(1);
		}
		UILinkPointNavigator.ChangePage(PageOnRight);
	}

	private void Travel(int next)
	{
		if (next < 0)
		{
			if (ReachEndEvent != null)
			{
				ReachEndEvent(CurrentPoint, next);
				if (TravelEvent != null)
				{
					TravelEvent();
				}
			}
		}
		else
		{
			UILinkPointNavigator.ChangePoint(next);
			if (TravelEvent != null)
			{
				TravelEvent();
			}
		}
	}

	public string SpecialInteractions()
	{
		if (OnSpecialInteracts != null)
		{
			return OnSpecialInteracts();
		}
		return string.Empty;
	}

	public string SpecialInteractionsLate()
	{
		if (OnSpecialInteractsLate != null)
		{
			return OnSpecialInteractsLate();
		}
		return string.Empty;
	}
}
