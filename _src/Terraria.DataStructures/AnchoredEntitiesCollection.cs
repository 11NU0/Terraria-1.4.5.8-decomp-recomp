using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public class AnchoredEntitiesCollection
{
	private struct IndexPointPair
	{
		public int index;

		public Point coords;
	}

	private List<IndexPointPair> _anchoredNPCs;

	private List<IndexPointPair> _anchoredPlayers;

	public int AnchoredPlayersAmount => _anchoredPlayers.Count;

	public AnchoredEntitiesCollection()
	{
		_anchoredNPCs = new List<IndexPointPair>();
		_anchoredPlayers = new List<IndexPointPair>();
	}

	public void ClearNPCAnchors()
	{
		_anchoredNPCs.Clear();
	}

	public void ClearPlayerAnchors()
	{
		_anchoredPlayers.Clear();
	}

	public void AddNPC(int npcIndex, Point coords)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_anchoredNPCs.Add(new IndexPointPair
		{
			index = npcIndex,
			coords = coords
		});
	}

	public int GetNextPlayerStackIndexInCoords(Point coords)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetEntitiesInCoords(coords);
	}

	public void AddPlayerAndGetItsStackedIndexInCoords(int playerIndex, Point coords, out int stackedIndexInCoords)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		stackedIndexInCoords = GetEntitiesInCoords(coords);
		_anchoredPlayers.Add(new IndexPointPair
		{
			index = playerIndex,
			coords = coords
		});
	}

	private int GetEntitiesInCoords(Point coords)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < _anchoredNPCs.Count; i++)
		{
			if (_anchoredNPCs[i].coords == coords)
			{
				num++;
			}
		}
		for (int j = 0; j < _anchoredPlayers.Count; j++)
		{
			if (_anchoredPlayers[j].coords == coords)
			{
				num++;
			}
		}
		return num;
	}
}
