using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam;

public class Lobby
{
	private HashSet<CSteamID> _usersSeen = new HashSet<CSteamID>();

	private byte[] _messageBuffer = new byte[1024];

	public CSteamID Id = CSteamID.Nil;

	public CSteamID Owner = CSteamID.Nil;

	public LobbyState State;

	private CallResult<LobbyEnter_t> _lobbyEnter;

	private CallResult<LobbyEnter_t>.APIDispatchDelegate _lobbyEnterExternalCallback;

	private CallResult<LobbyCreated_t> _lobbyCreated;

	private CallResult<LobbyCreated_t>.APIDispatchDelegate _lobbyCreatedExternalCallback;

	public Lobby()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		_lobbyEnter = CallResult<LobbyEnter_t>.Create((CallResult<LobbyEnter_t>.APIDispatchDelegate)OnLobbyEntered);
		_lobbyCreated = CallResult<LobbyCreated_t>.Create((CallResult<LobbyCreated_t>.APIDispatchDelegate)OnLobbyCreated);
	}

	public void Create(bool inviteOnly, CallResult<LobbyCreated_t>.APIDispatchDelegate callResult)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		SteamAPICall_t val = SteamMatchmaking.CreateLobby((inviteOnly ? ELobbyType.k_ELobbyTypePrivate : ELobbyType.k_ELobbyTypePublic), 256);
		_lobbyCreatedExternalCallback = callResult;
		_lobbyCreated.Set(val, (CallResult<LobbyCreated_t>.APIDispatchDelegate)null);
		State = LobbyState.Creating;
	}

	public void OpenInviteOverlay()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Inactive)
		{
			SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(Main.LobbyId));
		}
		else
		{
			SteamFriends.ActivateGameOverlayInviteDialog(Id);
		}
	}

	public void Join(CSteamID lobbyId, CallResult<LobbyEnter_t>.APIDispatchDelegate callResult)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Inactive)
		{
			State = LobbyState.Connecting;
			_lobbyEnterExternalCallback = callResult;
			SteamAPICall_t val = SteamMatchmaking.JoinLobby(lobbyId);
			_lobbyEnter.Set(val, (CallResult<LobbyEnter_t>.APIDispatchDelegate)null);
		}
	}

	public byte[] GetMessage(int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		CSteamID val = default;
		EChatEntryType val2 = default;
		int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(Id, index, out val, _messageBuffer, _messageBuffer.Length, out val2);
		byte[] array = new byte[lobbyChatEntry];
		Array.Copy(_messageBuffer, array, lobbyChatEntry);
		return array;
	}

	public int GetUserCount()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return SteamMatchmaking.GetNumLobbyMembers(Id);
	}

	public CSteamID GetUserByIndex(int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return SteamMatchmaking.GetLobbyMemberByIndex(Id, index);
	}

	public bool SendMessage(byte[] data)
	{
		return SendMessage(data, data.Length);
	}

	public bool SendMessage(byte[] data, int length)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (State != LobbyState.Active)
		{
			return false;
		}
		return SteamMatchmaking.SendLobbyChatMsg(Id, data, length);
	}

	public void Set(CSteamID lobbyId)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Id = lobbyId;
		State = LobbyState.Active;
		Owner = SteamMatchmaking.GetLobbyOwner(lobbyId);
	}

	public void SetPlayedWith(CSteamID userId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (!_usersSeen.Contains(userId))
		{
			SteamFriends.SetPlayedWith(userId);
			_usersSeen.Add(userId);
		}
	}

	public void Leave()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Active)
		{
			SteamMatchmaking.LeaveLobby(Id);
		}
		State = LobbyState.Inactive;
		_usersSeen.Clear();
	}

	private void OnLobbyEntered(LobbyEnter_t result, bool failure)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Connecting)
		{
			if (failure)
			{
				State = LobbyState.Inactive;
			}
			else
			{
				State = LobbyState.Active;
			}
			Id = new CSteamID(result.m_ulSteamIDLobby);
			Owner = SteamMatchmaking.GetLobbyOwner(Id);
			_lobbyEnterExternalCallback.Invoke(result, failure);
		}
	}

	private void OnLobbyCreated(LobbyCreated_t result, bool failure)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Creating)
		{
			if (failure)
			{
				State = LobbyState.Inactive;
			}
			else
			{
				State = LobbyState.Active;
			}
			Id = new CSteamID(result.m_ulSteamIDLobby);
			Owner = SteamMatchmaking.GetLobbyOwner(Id);
			_lobbyCreatedExternalCallback.Invoke(result, failure);
		}
	}
}
