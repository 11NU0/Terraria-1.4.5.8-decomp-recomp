using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Skies.CreditsRoll;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Skies;

public class CreditsRollSky : CustomSky
{
	private int _endTime;

	private int _currentTime;

	private CreditsRollComposer _composer = new CreditsRollComposer();

	private List<IAnimationSegment> _segmentsInGame = new List<IAnimationSegment>();

	private List<IAnimationSegment> _segmentsInMainMenu = new List<IAnimationSegment>();

	private bool _isActive;

	private bool _wantsToBeSeen;

	private float _opacity;

	public int AmountOfTimeNeededForFullPlay => _endTime;

	public CreditsRollSky()
	{
		EnsureSegmentsAreMade();
	}

	public override void Update(GameTime gameTime)
	{
		if (FocusHelper.PauseSkies)
		{
			return;
		}
		_currentTime++;
		float num = 1f / 120f;
		if (Main.gameMenu)
		{
			num = 1f / 15f;
		}
		_opacity = MathHelper.Clamp(_opacity + num * (float)_wantsToBeSeen.ToDirectionInt(), 0f, 1f);
		if (_opacity == 0f && !_wantsToBeSeen)
		{
			_isActive = false;
			return;
		}
		bool flag = true;
		if (!Main.CanPlayCreditsRoll())
		{
			flag = false;
		}
		if (_currentTime >= _endTime)
		{
			flag = false;
		}
		if (!flag)
		{
			SkyManager.Instance.Deactivate("CreditsRoll");
		}
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		if (!(num < minDepth) && !(num > maxDepth))
		{
			Vector2 anchorPositionOnScreen = Main.ScreenSize.ToVector2() / 2f;
			if (Main.gameMenu)
			{
				anchorPositionOnScreen.Y = 300f * Main.UIScaleMatrix.M22 / Main.BackgroundViewMatrix.Zoom.Y;
			}
			GameAnimationSegment info = new GameAnimationSegment
			{
				SpriteBatch = spriteBatch,
				AnchorPositionOnScreen = anchorPositionOnScreen,
				TimeInAnimation = _currentTime,
				DisplayOpacity = _opacity
			};
			List<IAnimationSegment> list = _segmentsInGame;
			if (Main.gameMenu)
			{
				list = _segmentsInMainMenu;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Draw(ref info);
			}
		}
	}

	public override bool IsActive()
	{
		return _isActive;
	}

	public override void Reset()
	{
		_currentTime = 0;
		EnsureSegmentsAreMade();
		_isActive = false;
		_wantsToBeSeen = false;
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		_isActive = true;
		_wantsToBeSeen = true;
		if (_opacity == 0f)
		{
			EnsureSegmentsAreMade();
			_currentTime = 0;
		}
	}

	private void EnsureSegmentsAreMade()
	{
		if (_segmentsInMainMenu.Count <= 0 || _segmentsInGame.Count <= 0)
		{
			_segmentsInGame.Clear();
			_composer.FillSegments(_segmentsInGame, out _endTime, inGame: true);
			_segmentsInMainMenu.Clear();
			_composer.FillSegments(_segmentsInMainMenu, out _endTime, inGame: false);
		}
	}

	public override void Deactivate(params object[] args)
	{
		_wantsToBeSeen = false;
	}
}
