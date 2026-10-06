using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria.GameContent.Creative;

public interface ICreativeItemSortStep : IEntrySortStep<Item>, IComparer<Item>
{
}
