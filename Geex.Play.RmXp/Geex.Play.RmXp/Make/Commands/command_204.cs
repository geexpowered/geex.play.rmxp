using Geex.Play.Rpg.Game;
using Geex.Play.Rpg;

namespace Geex.Play.Make
{
	/// <summary>
	/// This interpreter runs event commands. This class is used within the
	//  GameSystem class and the GameEvent class
	/// </summary>
	public sealed partial class Interpreter
	{
		/// <summary>
		/// Change Map Settings
		/// </summary>
		bool Command204()
		{
			switch (intParams[0])
			{
				case 0:    // Panorama
					InGame.Map.PanoramaName = stringParams[0];
					InGame.Map.PanoramaHue = intParams[1];
					break;
				case 1:    // Fog
					InGame.Map.FogName = stringParams[0];
					InGame.Map.FogHue = intParams[1];
					InGame.Map.FogOpacity = (byte)intParams[2];
					InGame.Map.FogBlendType = intParams[3];
					InGame.Map.FogZoom = intParams[4]/100f;
					InGame.Map.FogSx = intParams[5];
					InGame.Map.FogSy = intParams[6];
					break;
				case 2:    // Battleback
					InGame.Map.BattlebackName = stringParams[0];
					InGame.Temp.BattlebackName = stringParams[1];
					break;
			}
			return true;
		}
	}
}

