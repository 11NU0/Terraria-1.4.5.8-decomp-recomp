using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class VertexStrip
{
	public delegate Color StripColorFunction(float progressOnStrip);

	public delegate float StripHalfWidthFunction(float progressOnStrip);

	private struct CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord) : IVertexType
	{
		public Vector2 Position = position;

		public Color Color = color;

		public Vector3 TexCoord = texCoord;

		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
		{
			new VertexElement(0, (VertexElementFormat)1, (VertexElementUsage)0, 0),
			new VertexElement(8, (VertexElementFormat)4, (VertexElementUsage)1, 0),
			new VertexElement(12, (VertexElementFormat)2, (VertexElementUsage)2, 0)
		});

		public VertexDeclaration VertexDeclaration => _vertexDeclaration;

		static CustomVertexInfo()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected Obj, but got Unknown
		}
	}

	private CustomVertexInfo[] _vertices = new CustomVertexInfo[1];

	private int _vertexAmountCurrentlyMaintained;

	private short[] _indices = new short[1];

	private int _indicesAmountCurrentlyMaintained;

	private List<Vector2> _temporaryPositionsCache = new List<Vector2>();

	private List<float> _temporaryRotationsCache = new List<float>();

	public void Reset(int expectedVertexCount = 0)
	{
		_vertexAmountCurrentlyMaintained = 0;
		_indicesAmountCurrentlyMaintained = 0;
		if (_vertices.Length < expectedVertexCount)
		{
			Array.Resize(ref _vertices, expectedVertexCount);
		}
	}

	public void PrepareStrip(Vector2[] positions, float[] rotations, StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), int? expectedVertexPairsAmount = null, bool includeBacksides = false)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		int num = positions.Length;
		Reset(num * 2);
		int num2 = num;
		if (expectedVertexPairsAmount.HasValue)
		{
			num2 = expectedVertexPairsAmount.Value;
		}
		for (int i = 0; i < num && !(positions[i] == Vector2.Zero); i++)
		{
			Vector2 pos = positions[i] + offsetForAllPositions;
			float rot = MathHelper.WrapAngle(rotations[i]);
			float progressOnStrip = (float)i / (float)(num2 - 1);
			AddVertexPair(colorFunction, widthFunction, pos, rot, progressOnStrip);
		}
		PrepareIndices(includeBacksides);
	}

	public void PrepareStripWithProceduralPadding(Vector2[] positions, float[] rotations, StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), bool includeBacksides = false, bool tryStoppingOddBug = true)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		_temporaryPositionsCache.Clear();
		_temporaryRotationsCache.Clear();
		for (int i = 0; i < positions.Length && !(positions[i] == Vector2.Zero); i++)
		{
			Vector2 val = positions[i];
			float num = MathHelper.WrapAngle(rotations[i]);
			_temporaryPositionsCache.Add(val);
			_temporaryRotationsCache.Add(num);
			if (i + 1 >= positions.Length || !(positions[i + 1] != Vector2.Zero))
			{
				continue;
			}
			Vector2 val2 = positions[i + 1];
			float num2 = MathHelper.WrapAngle(rotations[i + 1]);
			int num3 = (int)(Math.Abs(MathHelper.WrapAngle(num2 - num)) / ((float)Math.PI / 12f));
			if (num3 == 0)
			{
				continue;
			}
			float num4 = val.Distance(val2);
			Vector2 val3 = val + num.ToRotationVector2() * num4;
			Vector2 val4 = val2 + num2.ToRotationVector2() * (0f - num4);
			int num5 = num3 + 2;
			float num6 = 1f / (float)num5;
			Vector2 target = val;
			for (float num7 = num6; num7 < 1f; num7 += num6)
			{
				Vector2 val5 = Vector2.CatmullRom(val3, val, val2, val4, num7);
				float num8 = MathHelper.WrapAngle(val5.DirectionTo(target).ToRotation());
				if (float.IsNaN(num8))
				{
					num8 = _temporaryRotationsCache.Last();
				}
				_temporaryPositionsCache.Add(val5);
				_temporaryRotationsCache.Add(num8);
				target = val5;
			}
		}
		Reset(_temporaryPositionsCache.Count * 2);
		int count = _temporaryPositionsCache.Count;
		Vector2 zero = Vector2.Zero;
		for (int j = 0; j < count && (!tryStoppingOddBug || !(_temporaryPositionsCache[j] == zero)); j++)
		{
			Vector2 pos = _temporaryPositionsCache[j] + offsetForAllPositions;
			float rot = _temporaryRotationsCache[j];
			float progressOnStrip = (float)j / (float)(count - 1);
			AddVertexPair(colorFunction, widthFunction, pos, rot, progressOnStrip);
		}
		PrepareIndices(includeBacksides);
	}

	public void PrepareIndices(bool includeBacksides)
	{
		int num = _vertexAmountCurrentlyMaintained / 2 - 1;
		int num2 = 6 + includeBacksides.ToInt() * 6;
		int num3 = (_indicesAmountCurrentlyMaintained = num * num2);
		if (_indices.Length < num3)
		{
			Array.Resize(ref _indices, num3);
		}
		for (short num4 = 0; num4 < num; num4++)
		{
			short num5 = (short)(num4 * num2);
			int num6 = num4 * 2;
			_indices[num5] = (short)num6;
			_indices[num5 + 1] = (short)(num6 + 1);
			_indices[num5 + 2] = (short)(num6 + 2);
			_indices[num5 + 3] = (short)(num6 + 2);
			_indices[num5 + 4] = (short)(num6 + 1);
			_indices[num5 + 5] = (short)(num6 + 3);
			if (includeBacksides)
			{
				_indices[num5 + 6] = (short)(num6 + 2);
				_indices[num5 + 7] = (short)(num6 + 1);
				_indices[num5 + 8] = (short)num6;
				_indices[num5 + 9] = (short)(num6 + 2);
				_indices[num5 + 10] = (short)(num6 + 3);
				_indices[num5 + 11] = (short)(num6 + 1);
			}
		}
	}

	public void AddVertexPair(StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 pos, float rot, float progressOnStrip)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Color vertexColor = colorFunction(progressOnStrip);
		float num = widthFunction(progressOnStrip);
		Vector2 val = MathHelper.WrapAngle(rot - (float)Math.PI / 2f).ToRotationVector2() * num;
		AddVertexPair(pos + val, pos - val, progressOnStrip, vertexColor);
	}

	public void AddVertexPair(Vector2 a, Vector2 b, Vector3 uvA, Vector3 uvB, Color vertexColor)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		while (_vertexAmountCurrentlyMaintained + 1 >= _vertices.Length)
		{
			Array.Resize(ref _vertices, _vertices.Length * 2);
		}
		Vector2.Distance(a, b);
		_vertices[_vertexAmountCurrentlyMaintained].Position = a;
		_vertices[_vertexAmountCurrentlyMaintained + 1].Position = b;
		_vertices[_vertexAmountCurrentlyMaintained].TexCoord = uvA;
		_vertices[_vertexAmountCurrentlyMaintained + 1].TexCoord = uvB;
		_vertices[_vertexAmountCurrentlyMaintained].Color = vertexColor;
		_vertices[_vertexAmountCurrentlyMaintained + 1].Color = vertexColor;
		_vertexAmountCurrentlyMaintained += 2;
	}

	public void AddVertexPair(Vector2 a, Vector2 b, float uv_x, Color vertexColor)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		while (_vertexAmountCurrentlyMaintained + 1 >= _vertices.Length)
		{
			Array.Resize(ref _vertices, _vertices.Length * 2);
		}
		float num = Vector2.Distance(a, b);
		_vertices[_vertexAmountCurrentlyMaintained].Position = a;
		_vertices[_vertexAmountCurrentlyMaintained + 1].Position = b;
		_vertices[_vertexAmountCurrentlyMaintained].TexCoord = new Vector3(uv_x, num, num);
		_vertices[_vertexAmountCurrentlyMaintained + 1].TexCoord = new Vector3(uv_x, 0f, num);
		_vertices[_vertexAmountCurrentlyMaintained].Color = vertexColor;
		_vertices[_vertexAmountCurrentlyMaintained + 1].Color = vertexColor;
		_vertexAmountCurrentlyMaintained += 2;
	}

	public void AddVertexPair(Vector2 v1, Vector2 v2, float uv_x, Color color1, Color color2)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		while (_vertexAmountCurrentlyMaintained + 1 >= _vertices.Length)
		{
			Array.Resize(ref _vertices, _vertices.Length * 2);
		}
		float num = Vector2.Distance(v1, v2);
		_vertices[_vertexAmountCurrentlyMaintained++] = new CustomVertexInfo(v1, color1, new Vector3(uv_x, num, num));
		_vertices[_vertexAmountCurrentlyMaintained++] = new CustomVertexInfo(v2, color2, new Vector3(uv_x, 0f, num));
	}

	public void DrawTrail()
	{
		if (_vertexAmountCurrentlyMaintained >= 3)
		{
			GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
			VertexBufferBinding[] vertexBuffers = graphicsDevice.GetVertexBuffers();
			IndexBuffer indices = graphicsDevice.Indices;
			graphicsDevice.DrawUserIndexedPrimitives<CustomVertexInfo>((PrimitiveType)0, _vertices, 0, _vertexAmountCurrentlyMaintained, _indices, 0, _indicesAmountCurrentlyMaintained / 3);
			graphicsDevice.SetVertexBuffers(vertexBuffers);
			graphicsDevice.Indices = indices;
		}
	}
}
