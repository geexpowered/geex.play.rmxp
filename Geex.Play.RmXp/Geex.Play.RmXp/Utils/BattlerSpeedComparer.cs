using System.Collections.Generic;
using Geex.Play.Rpg.Game;

namespace Geex.Play.Rpg.Utils
{
    /// <summary>
    /// Function to sort GameBattler in function of speed (descending order).
    /// </summary>
    public partial class BattlerSpeedComparer : IComparer<GameBattler>
    {
        public int Compare(GameBattler a, GameBattler b)
        {
            int _action_speed_a = a.CurrentAction.speed;
            int _action_speed_b = b.CurrentAction.speed;

            if (_action_speed_a > _action_speed_b)
            {
                return -1;
            }
            else if (_action_speed_a < _action_speed_b)
            {
                return +1;
            }
            else
            {
                return 0;
            }
        }
    }
}
