using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Utils
{
    /// <summary>
    /// Function to sort states used in "AddState" function of GameBattler
    /// </summary>
    public partial class BattlerStateComparer : IComparer<short>
    {
        public int Compare(short a, short b)
        {
            State _state_a = Data.States[a];
            State _state_b = Data.States[b];

            if (_state_a.Rating > _state_b.Rating)
            {
                return -1;
            }
            else if (_state_a.Rating < _state_b.Rating)
            {
                return +1;
            }
            else if (_state_a.Restriction > _state_b.Restriction)
            {
                return -1;
            }
            else if (_state_a.Restriction < _state_b.Restriction)
            {
                return +1;
            }
            else
            {
                return (a == b) ? 1 : 0;
            }
        }
    }
}
