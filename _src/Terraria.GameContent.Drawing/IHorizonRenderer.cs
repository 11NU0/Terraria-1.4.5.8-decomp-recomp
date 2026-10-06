using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing;

public interface IHorizonRenderer
{
	void DrawHorizon();

	void ModifyHorizonLight(ref Color color);

	void DrawSun(Vector2 sunPosition);

	void CloudsStart();

	void DrawCloud(float globalCloudAlpha, Cloud theCloud, int cloudPass, float cY);

	void CloudsEnd();

	void DrawSurfaceLayer(int layerIndex);

	void DrawLensFlare();
}
