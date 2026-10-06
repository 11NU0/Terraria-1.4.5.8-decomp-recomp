using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Graphics;

namespace Terraria.GameContent.Golf;

public class FancyGolfPredictionLine
{
	private class PredictionEntity : Entity
	{
	}

	private readonly List<Vector2> _positions;

	private readonly Entity _entity = new PredictionEntity();

	private readonly int _iterations;

	private readonly Color[] _colors = new Color[2]
	{
		Color.White,
		Color.Gray
	};

	private readonly BasicDebugDrawer _drawer = new BasicDebugDrawer(((Game)Main.instance).GraphicsDevice);

	private float _time;

	public FancyGolfPredictionLine(int iterations)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected Obj, but got Unknown
		_positions = new List<Vector2>(iterations * 2 + 1);
		_iterations = iterations;
	}

	public void Update(Entity golfBall, Vector2 impactVelocity, float roughLandResistance)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		bool flag = Main.tileSolid[379];
		Main.tileSolid[379] = false;
		_positions.Clear();
		_time += 1f / 60f;
		_entity.position = golfBall.position;
		_entity.width = golfBall.width;
		_entity.height = golfBall.height;
		GolfHelper.HitGolfBall(_entity, impactVelocity, roughLandResistance);
		_positions.Add(_entity.position);
		float angularVelocity = 0f;
		for (int i = 0; i < _iterations; i++)
		{
			GolfHelper.StepGolfBall(_entity, ref angularVelocity);
			_positions.Add(_entity.position);
		}
		Main.tileSolid[379] = flag;
	}

	public void Draw(Camera camera, SpriteBatch spriteBatch, float chargeProgress)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		_drawer.Begin(camera.GameViewMatrix.TransformationMatrix);
		_ = _positions.Count;
		Texture2D value = TextureAssets.Extra[33].Value;
		Vector2 val = new Vector2(3.5f, 3.5f);
		Vector2 val2 = value.Size() / 2f;
		Vector2 val3 = val - camera.UnscaledPosition;
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < _positions.Count - 1; i++)
		{
			GetSectionLength(i, out var length, out var _);
			if (length != 0f)
			{
				for (; num < num2 + length; num += 4f)
				{
					float num3 = (num - num2) / length + (float)i;
					Vector2 position = GetPosition((num - num2) / length + (float)i);
					Color color = GetColor2(num3);
					color *= MathHelper.Clamp(2f - 2f * num3 / (float)(_positions.Count - 1), 0f, 1f);
					spriteBatch.Draw(value, position + val3, (Rectangle?)null, color, 0f, val2, GetScale(num), (SpriteEffects)0, 0f);
				}
				num2 += length;
			}
		}
		_drawer.End();
	}

	private Color GetColor(float travelledLength)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		float num = travelledLength % 200f / 200f;
		num *= (float)_colors.Length;
		num -= _time * (float)Math.PI * 1.5f;
		num %= (float)_colors.Length;
		if (num < 0f)
		{
			num += (float)_colors.Length;
		}
		int num2 = (int)Math.Floor(num);
		int num3 = num2 + 1;
		num2 = Utils.Clamp(num2 % _colors.Length, 0, _colors.Length - 1);
		num3 = Utils.Clamp(num3 % _colors.Length, 0, _colors.Length - 1);
		float num4 = num - (float)num2;
		Color val = Color.Lerp(_colors[num2], _colors[num3], num4);
		val.A = 64;
		val *= 0.6f;
		return val;
	}

	private Color GetColor2(float index)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		float num = index * 0.5f - _time * (float)Math.PI * 1.5f;
		int num2 = (int)Math.Floor(num) % _colors.Length;
		if (num2 < 0)
		{
			num2 += _colors.Length;
		}
		int num3 = (num2 + 1) % _colors.Length;
		float num4 = num - (float)Math.Floor(num);
		Color val = Color.Lerp(_colors[num2], _colors[num3], num4);
		val.A = 64;
		val *= 0.6f;
		return val;
	}

	private float GetScale(float travelledLength)
	{
		return 0.2f + Utils.GetLerpValue(0.8f, 1f, (float)Math.Cos(travelledLength / 50f + _time * -(float)Math.PI) * 0.5f + 0.5f, clamped: true) * 0.15f;
	}

	private void GetSectionLength(int startIndex, out float length, out float rotation)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		int num = startIndex + 1;
		if (num >= _positions.Count)
		{
			num = _positions.Count - 1;
		}
		length = Vector2.Distance(_positions[startIndex], _positions[num]);
		rotation = (_positions[num] - _positions[startIndex]).ToRotation();
	}

	private Vector2 GetPosition(float indexProgress)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Math.Floor(indexProgress);
		int num2 = num + 1;
		if (num2 >= _positions.Count)
		{
			num2 = _positions.Count - 1;
		}
		float num3 = indexProgress - (float)num;
		return Vector2.Lerp(_positions[num], _positions[num2], num3);
	}
}
