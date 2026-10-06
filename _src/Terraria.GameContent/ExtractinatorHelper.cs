namespace Terraria.GameContent;

public class ExtractinatorHelper
{
	public static void RollExtractinatorDrop(int extractionMode, int extractinatorBlockType, out int itemType, out int stack)
	{
		int num = 5000;
		int num2 = 25;
		int num3 = 50;
		int num4 = -1;
		int num5 = -1;
		int num6 = -1;
		int num7 = 1;
		int num8 = -1;
		int num9 = -1;
		int num10 = -1;
		int num11 = -1;
		switch (extractionMode)
		{
		case -1:
			itemType = -1;
			stack = 1;
			return;
		case 1:
			num /= 3;
			num2 *= 2;
			num3 = 20;
			num4 = 10;
			break;
		case 2:
			num = -1;
			num2 = -1;
			num3 = -1;
			num4 = -1;
			num5 = 1;
			num7 = -1;
			break;
		case 3:
			num = -1;
			num2 = -1;
			num3 = -1;
			num4 = -1;
			num5 = -1;
			num7 = -1;
			num6 = 1;
			break;
		case 4:
			num = -1;
			num2 = -1;
			num3 = -1;
			num7 = -1;
			num9 = 50;
			num8 = 1;
			break;
		case 5:
			num = -1;
			num2 = -1;
			num3 = -1;
			num7 = -1;
			num11 = 1;
			break;
		case 6:
			num = -1;
			num2 = -1;
			num3 = -1;
			num7 = -1;
			num10 = 1;
			break;
		}
		itemType = -1;
		stack = 1;
		if (num4 != -1 && Main.rand.Next(num4) == 0)
		{
			itemType = 3380;
			if (Main.rand.Next(5) == 0)
			{
				stack += Main.rand.Next(2);
			}
			if (Main.rand.Next(10) == 0)
			{
				stack += Main.rand.Next(3);
			}
			if (Main.rand.Next(15) == 0)
			{
				stack += Main.rand.Next(4);
			}
		}
		else if (num7 != -1 && Main.rand.Next(2) == 0)
		{
			if (Main.rand.Next(12000) == 0)
			{
				itemType = 74;
				if (Main.rand.Next(14) == 0)
				{
					stack += Main.rand.Next(0, 2);
				}
				if (Main.rand.Next(14) == 0)
				{
					stack += Main.rand.Next(0, 2);
				}
				if (Main.rand.Next(14) == 0)
				{
					stack += Main.rand.Next(0, 2);
				}
			}
			else if (Main.rand.Next(800) == 0)
			{
				itemType = 73;
				if (Main.rand.Next(6) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(6) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(6) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(6) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(6) == 0)
				{
					stack += Main.rand.Next(1, 20);
				}
			}
			else if (Main.rand.Next(60) == 0)
			{
				itemType = 72;
				if (Main.rand.Next(4) == 0)
				{
					stack += Main.rand.Next(5, 26);
				}
				if (Main.rand.Next(4) == 0)
				{
					stack += Main.rand.Next(5, 26);
				}
				if (Main.rand.Next(4) == 0)
				{
					stack += Main.rand.Next(5, 26);
				}
				if (Main.rand.Next(4) == 0)
				{
					stack += Main.rand.Next(5, 25);
				}
			}
			else
			{
				itemType = 71;
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(10, 26);
				}
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(10, 26);
				}
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(10, 26);
				}
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(10, 25);
				}
			}
		}
		else if (num != -1 && Main.rand.Next(num) == 0)
		{
			itemType = 1242;
		}
		else if (num5 != -1)
		{
			if (Main.rand.Next(4) != 1)
			{
				itemType = 2674;
			}
			else if (Main.rand.Next(3) != 1)
			{
				itemType = 2006;
			}
			else if (Main.rand.Next(3) != 1)
			{
				itemType = 2002;
			}
			else
			{
				itemType = 2675;
			}
		}
		else if (num6 != -1 && extractinatorBlockType == 642)
		{
			if (Main.rand.Next(10) == 1)
			{
				itemType = Main.rand.Next(5);
				if (itemType == 0)
				{
					itemType = 4354;
				}
				else if (itemType == 1)
				{
					itemType = 4389;
				}
				else if (itemType == 2)
				{
					itemType = 4377;
				}
				else if (itemType == 3)
				{
					itemType = 5127;
				}
				else
				{
					itemType = 4378;
				}
			}
			else
			{
				itemType = Main.rand.Next(5);
				if (itemType == 0)
				{
					itemType = 4349;
				}
				else if (itemType == 1)
				{
					itemType = 4350;
				}
				else if (itemType == 2)
				{
					itemType = 4351;
				}
				else if (itemType == 3)
				{
					itemType = 4352;
				}
				else
				{
					itemType = 4353;
				}
			}
		}
		else if (num6 != -1)
		{
			itemType = Main.rand.Next(5);
			if (itemType == 0)
			{
				itemType = 4349;
			}
			else if (itemType == 1)
			{
				itemType = 4350;
			}
			else if (itemType == 2)
			{
				itemType = 4351;
			}
			else if (itemType == 3)
			{
				itemType = 4352;
			}
			else
			{
				itemType = 4353;
			}
		}
		else if (num9 != -1 && Main.rand.Next(num9) == 0)
		{
			itemType = Main.rand.Next(3);
			if (itemType == 0)
			{
				itemType = 62;
			}
			else if (itemType == 1)
			{
				itemType = 195;
			}
			else if (itemType == 2)
			{
				itemType = 194;
			}
		}
		else if (num8 > 0)
		{
			itemType = 2;
		}
		else if (num11 > 0)
		{
			itemType = 1125;
		}
		else if (num10 > 0)
		{
			itemType = 169;
		}
		else if (num2 != -1 && Main.rand.Next(num2) == 0)
		{
			itemType = Main.rand.Next(6);
			if (itemType == 0)
			{
				itemType = 181;
			}
			else if (itemType == 1)
			{
				itemType = 180;
			}
			else if (itemType == 2)
			{
				itemType = 177;
			}
			else if (itemType == 3)
			{
				itemType = 179;
			}
			else if (itemType == 4)
			{
				itemType = 178;
			}
			else
			{
				itemType = 182;
			}
			if (Main.rand.Next(20) == 0)
			{
				stack += Main.rand.Next(0, 2);
			}
			if (Main.rand.Next(30) == 0)
			{
				stack += Main.rand.Next(0, 3);
			}
			if (Main.rand.Next(40) == 0)
			{
				stack += Main.rand.Next(0, 4);
			}
			if (Main.rand.Next(50) == 0)
			{
				stack += Main.rand.Next(0, 5);
			}
			if (Main.rand.Next(60) == 0)
			{
				stack += Main.rand.Next(0, 6);
			}
		}
		else if (num3 != -1 && Main.rand.Next(num3) == 0)
		{
			itemType = 999;
			if (Main.rand.Next(20) == 0)
			{
				stack += Main.rand.Next(0, 2);
			}
			if (Main.rand.Next(30) == 0)
			{
				stack += Main.rand.Next(0, 3);
			}
			if (Main.rand.Next(40) == 0)
			{
				stack += Main.rand.Next(0, 4);
			}
			if (Main.rand.Next(50) == 0)
			{
				stack += Main.rand.Next(0, 5);
			}
			if (Main.rand.Next(60) == 0)
			{
				stack += Main.rand.Next(0, 6);
			}
		}
		else if (Main.rand.Next(3) == 0)
		{
			if (Main.rand.Next(5000) == 0)
			{
				itemType = 74;
				if (Main.rand.Next(10) == 0)
				{
					stack += Main.rand.Next(0, 3);
				}
				if (Main.rand.Next(10) == 0)
				{
					stack += Main.rand.Next(0, 3);
				}
				if (Main.rand.Next(10) == 0)
				{
					stack += Main.rand.Next(0, 3);
				}
				if (Main.rand.Next(10) == 0)
				{
					stack += Main.rand.Next(0, 3);
				}
				if (Main.rand.Next(10) == 0)
				{
					stack += Main.rand.Next(0, 3);
				}
			}
			else if (Main.rand.Next(400) == 0)
			{
				itemType = 73;
				if (Main.rand.Next(5) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(5) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(5) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(5) == 0)
				{
					stack += Main.rand.Next(1, 21);
				}
				if (Main.rand.Next(5) == 0)
				{
					stack += Main.rand.Next(1, 20);
				}
			}
			else if (Main.rand.Next(30) == 0)
			{
				itemType = 72;
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(5, 26);
				}
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(5, 26);
				}
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(5, 26);
				}
				if (Main.rand.Next(3) == 0)
				{
					stack += Main.rand.Next(5, 25);
				}
			}
			else
			{
				itemType = 71;
				if (Main.rand.Next(2) == 0)
				{
					stack += Main.rand.Next(10, 26);
				}
				if (Main.rand.Next(2) == 0)
				{
					stack += Main.rand.Next(10, 26);
				}
				if (Main.rand.Next(2) == 0)
				{
					stack += Main.rand.Next(10, 26);
				}
				if (Main.rand.Next(2) == 0)
				{
					stack += Main.rand.Next(10, 25);
				}
			}
		}
		else
		{
			itemType = RollOreEarlymode();
			if (extractinatorBlockType == 642 && Main.hardMode)
			{
				itemType = RollOreHardmode();
			}
			if (Main.rand.Next(20) == 0)
			{
				stack += Main.rand.Next(0, 2);
			}
			if (Main.rand.Next(30) == 0)
			{
				stack += Main.rand.Next(0, 3);
			}
			if (Main.rand.Next(40) == 0)
			{
				stack += Main.rand.Next(0, 4);
			}
			if (Main.rand.Next(50) == 0)
			{
				stack += Main.rand.Next(0, 5);
			}
			if (Main.rand.Next(60) == 0)
			{
				stack += Main.rand.Next(0, 6);
			}
		}
	}

	private static int RollOreHardmode()
	{
		return Main.rand.Next(14) switch
		{
			0 => 12, 
			1 => 11, 
			2 => 14, 
			3 => 13, 
			4 => 699, 
			5 => 700, 
			6 => 701, 
			7 => 702, 
			8 => 364, 
			9 => 1104, 
			10 => 365, 
			11 => 1105, 
			12 => 366, 
			_ => 1106, 
		};
	}

	private static int RollOreEarlymode()
	{
		return Main.rand.Next(8) switch
		{
			0 => 12, 
			1 => 11, 
			2 => 14, 
			3 => 13, 
			4 => 699, 
			5 => 700, 
			6 => 701, 
			_ => 702, 
		};
	}
}
