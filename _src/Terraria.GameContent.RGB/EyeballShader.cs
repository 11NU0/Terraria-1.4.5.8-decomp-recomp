using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using Terraria.Utilities;

namespace Terraria.GameContent.RGB;

public class EyeballShader : ChromaShader
{
	private struct Ring(Vector4 color, float distance)
	{
		public readonly Vector4 Color = color;

		public readonly float Distance = distance;
	}

	private enum EyelidState
	{
		Closed,
		Opening,
		Open,
		Closing
	}

	private static readonly Ring[] Rings;

	private readonly Vector4 _eyelidColor;

	private float _eyelidProgress;

	private Vector2 _pupilOffset;

	private Vector2 _targetOffset;

	private readonly UnifiedRandom _random;

	private float _timeUntilPupilMove;

	private float _eyelidStateTime;

	private readonly bool _isSpawning;

	private EyelidState _eyelidState;

	public EyeballShader(bool isSpawning)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color(108, 110, 75);
		_eyelidColor = val.ToVector4();
		_pupilOffset = Vector2.Zero;
		_targetOffset = Vector2.Zero;
		_random = new UnifiedRandom();
		_isSpawning = isSpawning;
	}

	public override void Update(float elapsedTime)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		UpdateEyelid(elapsedTime);
		bool flag = _timeUntilPupilMove <= 0f;
		_pupilOffset = (_targetOffset + _pupilOffset) * 0.5f;
		_timeUntilPupilMove -= elapsedTime;
		if (flag)
		{
			float num = (float)_random.NextDouble() * ((float)Math.PI * 2f);
			float num2;
			if (_isSpawning)
			{
				_timeUntilPupilMove = (float)_random.NextDouble() * 0.4f + 0.3f;
				num2 = (float)_random.NextDouble() * 0.7f;
			}
			else
			{
				_timeUntilPupilMove = (float)_random.NextDouble() * 0.4f + 0.6f;
				num2 = (float)_random.NextDouble() * 0.3f;
			}
			_targetOffset = new Vector2((float)Math.Cos(num), (float)Math.Sin(num)) * num2;
		}
	}

	private void UpdateEyelid(float elapsedTime)
	{
		float num = 0.5f;
		float num2 = 6f;
		if (_isSpawning)
		{
			if (NPC.MoonLordCountdown >= NPC.MaxMoonLordCountdown - 10)
			{
				_eyelidStateTime = 0f;
				_eyelidState = EyelidState.Closed;
			}
			num = (float)NPC.MoonLordCountdown / (float)NPC.MaxMoonLordCountdown * 10f + 0.5f;
			num2 = 2f;
		}
		_eyelidStateTime += elapsedTime;
		switch (_eyelidState)
		{
		case EyelidState.Closed:
			_eyelidProgress = 0f;
			if (_eyelidStateTime > num)
			{
				_eyelidStateTime = 0f;
				_eyelidState = EyelidState.Opening;
			}
			break;
		case EyelidState.Opening:
			_eyelidProgress = _eyelidStateTime / 0.4f;
			if (_eyelidStateTime > 0.4f)
			{
				_eyelidStateTime = 0f;
				_eyelidState = EyelidState.Open;
			}
			break;
		case EyelidState.Open:
			_eyelidProgress = 1f;
			if (_eyelidStateTime > num2)
			{
				_eyelidStateTime = 0f;
				_eyelidState = EyelidState.Closing;
			}
			break;
		case EyelidState.Closing:
			_eyelidProgress = 1f - _eyelidStateTime / 0.4f;
			if (_eyelidStateTime > 0.4f)
			{
				_eyelidStateTime = 0f;
				_eyelidState = EyelidState.Closed;
			}
			break;
		}
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2(1.5f, 0.5f);
		Vector2 val2 = val + _pupilOffset;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector2 val3 = canvasPositionOfIndex - val;
			Vector4 val4 = Vector4.One;
			Vector2 val5 = val2 - canvasPositionOfIndex;
			float num = val5.Length();
			for (int j = 1; j < Rings.Length; j++)
			{
				Ring ring = Rings[j];
				Ring ring2 = Rings[j - 1];
				if (num < ring.Distance)
				{
					val4 = Vector4.Lerp(ring2.Color, ring.Color, (num - ring2.Distance) / (ring.Distance - ring2.Distance));
					break;
				}
			}
			float num2 = (float)Math.Sqrt(1f - 0.4f * val3.Y * val3.Y) * 5f;
			float num3 = Math.Abs(val3.X) - num2 * (1.1f * _eyelidProgress - 0.1f);
			if (num3 > 0f)
			{
				val4 = Vector4.Lerp(val4, _eyelidColor, Math.Min(1f, num3 * 10f));
			}
			fragment.SetColor(i, val4);
		}
	}

	static EyeballShader()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		Ring[] array = new Ring[5];
		Color val = Color.Black;
		array[0] = new Ring(val.ToVector4(), 0f);
		val = Color.Black;
		array[1] = new Ring(val.ToVector4(), 0.4f);
		val = new Color(17, 220, 237);
		array[2] = new Ring(val.ToVector4(), 0.5f);
		val = new Color(17, 120, 237);
		array[3] = new Ring(val.ToVector4(), 0.6f);
		array[4] = new Ring(Vector4.One, 0.65f);
		Rings = array;
	}
}
