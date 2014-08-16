using System.Collections.Generic;
using Geex.Run;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class contains information about the monster group currently being fought, 
    /// and has methods for settings up the monster group and determining a random target for a monster
    /// </summary>
    public partial class GameTroop
    {
        #region Variables

        /// <summary>
        /// Troop's npcs list
        /// </summary>
        public List<GameNpc> Npcs = new List<GameNpc>();

        #endregion

        #region Initialize

        /// <summary>
        /// Empty constructor
        /// </summary>
        public GameTroop()
        {

        }

        /// <summary>Create an npc array</summary>
        /// <param Name="troop_id"></param>
        public void Setup(int troopId)
        {
            //Set array of enemies who are set as troops
            Npcs.Clear();
            Troop _troop = Data.Troops[troopId];
            for (int i = 0; i < _troop.Members.Length; i++)
            {
                Npc _npc = Data.Npcs[_troop.Members[i].NpcId];
                if (_npc != null)
                {
                    Npcs.Add(new GameNpc(troopId, i));
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>Random Selection of a Target npc</summary>
        /// <param Name="hp0">Limited to enemies with 0 HP</param>
        /// <returns>GameNpc : seleted npc</returns>
        public GameNpc RandomTargetNpc(bool hp0)
        {
            List<GameNpc> roulette = new List<GameNpc>();
            for (int i=0;i<Npcs.Count;i++)
            {
                //If it fits the conditions
                if ((!hp0 && Npcs[i].IsExist) || (hp0 && Npcs[i].IsHp0))
                {
                    //Add an npc to the roulette
                    roulette.Add(Npcs[i]);
                }
            }
            //If roulette Size is 0
            if (roulette.Count == 0)
            {
                return null;
            }
            //Spin the roulette, choose an npc
            return roulette[InGame.Rnd.Next(roulette.Count)];
        }

        /// <summary>Random Selection of a Target npc</summary>
        /// <returns>GameNpc : seleted npc</returns>
        public GameNpc RandomTargetNpc()
        {
            return RandomTargetNpc(false);
        }

        /// <summary>Random Selection of a Target npc (HP 0)</summary>
        /// <returns>GameNpc : seleted npc</returns>
        public GameNpc RandomTargetNpcHp0()
        {
            return RandomTargetNpc(true);
        }

        /// <summary>Smooth Selection of a Target npc</summary>
        /// <param Name="npc_index">The relative position in the monster group of the npc to check. </param>
        /// <returns>GameNpc : seleted npc</returns>
        public GameNpc SmoothTargetNpc(int npcIndex)
        {
            GameNpc _npc = null;
            // If an npc exists
            if (Npcs[npcIndex] != null && Npcs[npcIndex].IsExist)
            {
                _npc = Npcs[npcIndex];
            }
            else
            {
                // Loop
                for (int i = 0; i < Npcs.Count; i++)
                {
                    // If an npc exists
                    if (Npcs[i].IsExist)
                    {
                        _npc = Npcs[i];
                    }
                }
            }
            return _npc;
        }

        #endregion
    }
}
