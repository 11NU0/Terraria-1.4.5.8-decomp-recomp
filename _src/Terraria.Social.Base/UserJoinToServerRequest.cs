using System;

namespace Terraria.Social.Base;

public abstract class UserJoinToServerRequest
{
	internal string UserDisplayName { get; private set; }

	internal string UserFullIdentifier { get; private set; }

	public event Action OnAccepted;

	public event Action OnRejected;

	public UserJoinToServerRequest(string userDisplayName, string fullIdentifier)
	{
		UserDisplayName = userDisplayName;
		UserFullIdentifier = fullIdentifier;
	}

	public void Accept()
	{
		if (OnAccepted != null)
		{
			OnAccepted();
		}
	}

	public void Reject()
	{
		if (OnRejected != null)
		{
			OnRejected();
		}
	}

	public abstract bool IsValid();

	public abstract string GetUserWrapperText();
}
