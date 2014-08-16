using Geex.Play.Rpg.Game;
using Geex.Play.Custom;

namespace Geex.Run
{
    public partial class SavedGame
    {
        public string[] CharacterNames;
        public int[] CharacterHues;
        public int FrameCount;
        public GameSystem GameSystem; 
        public GameSwitches GameSwitchesData;
        public GameVariables GameVariablesData;
        public GameScreen GameScreen;
        public GameActors GameActors;
        public GameParty GameParty;
        public GameTroop GameTroop;
        public GameMap GameMap;
        public GamePlayer GamePlayer;
    }
}
