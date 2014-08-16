namespace Geex.Play.Rpg.Game
{
    ///<summary>This class handles actions in battle. It's used within the GameBattler class</summary>
    public partial class GameBattleAction
    {
        #region Variable

        /// <summary>
        /// Speed
        /// </summary>
        public int speed;

        /// <summary>
        /// Kind (basic / skill / item)
        /// </summary>
        public int kind;

        /// <summary>
        /// Basic (attack / guard / escape)
        /// </summary>
        public int basic;

        /// <summary>
        /// Skill Id
        /// </summary>
        public int SkillId;

        /// <summary>
        /// Item Id
        /// </summary>
        public int ItemId;

        /// <summary>
        /// Target Id
        /// </summary>
        public int TargetIndex;

        /// <summary>
        /// Forcing flag
        /// </summary>
        public bool IsForcing;

        #endregion

        #region Initialize

        ///<summary>Initializes GameBattleAction, running the Clear() method.</summary>
        public GameBattleAction()
        {
            Clear();
        }

        #endregion

        #region Methods

        /// <summary>Set GameBattleAction to default</summary>
        public void Clear()
        {
            speed = 0;
            kind = 0;
            basic = 3;
            SkillId = 0;
            ItemId = 0;
            TargetIndex = -1;
            IsForcing = false;
        }

        ///<summary>Determine Validity</summary>
        ///<returns>True if valid</returns>
        public bool IsValid()
        {
            return (!(kind == 0 && basic == 3));
        }

        ///<summary>Determine if for One Ally</summary>        
        ///<returns>True if the action is for one friend</returns>
        public bool IsForOneFriend()
        {
            // If kind = skill, and Effect scope is for ally (including 0 HP)
            if (kind == 1 && (Data.Skills[SkillId].Scope == 3 || Data.Skills[SkillId].Scope == 5))
            {
                return true;
            }
            // If kind = item, and Effect scope is for ally (including 0 HP)
            if (kind == 2 && (Data.Items[ItemId].Scope == 3 || Data.Items[ItemId].Scope == 5))
            {
                return true;
            }

            return false;
        }

        ///<summary>Determine if for One Ally (HP 0)</summary>
        ///<returns>True if the action is for one friend with no HP</returns>
        public bool IsForOneFriendHp0()
        {
            // If kind = skill, and Effect scope is for ally (only 0 HP)
            if (kind == 1 && Data.Skills[SkillId].Scope == 5)
            {
                return true;
            }
            // If kind = item, and Effect scope is for ally (only 0 HP)
            if (kind == 2 && Data.Items[ItemId].Scope == 5)
            {
                return true;
            }
            return false;
        }

        ///<summary>Random Target (for Actor)</summary>       
        public void DecideRandomTargetForActor()
        {
            // Diverge with Effect scope
            GameBattler _battler = null;
            if (IsForOneFriendHp0())
            {
                _battler = InGame.Party.RandomTargetActorHp0();
            }
            else if (IsForOneFriend())
            {
                _battler = InGame.Party.RandomTargetActor();
            }
            else
            {
                _battler = InGame.Troops.RandomTargetNpc();
            }
            // If a target exists, get an index, and if a target doesn't exist,
            // Clear the action
            if (_battler != null)
            {
                TargetIndex = _battler.Index;
            }
            else
            {
                Clear();
            }
        }

        ///<summary>Random Target (for Enemy)</summary>       
        public void DecideRandomTargetforEnemy()
        {
            // Diverge with Effect scope
            GameBattler _battler = null;
            if (IsForOneFriendHp0())
            {
                _battler = InGame.Troops.RandomTargetNpcHp0();
            }
            else if (IsForOneFriend())
            {
                _battler = InGame.Troops.RandomTargetNpc();
            }
            else
            {
                _battler = InGame.Party.RandomTargetActor();
            }
            // If a target exists, get an index, and if a target doesn't exist,
            // Clear the action
            if (_battler != null)
            {
                TargetIndex = _battler.Index;
            }
            else
            {
                Clear();
            }
        }

        /// <summary>
        /// Last Target (for Actor)
        /// </summary>
        public void DecideLastTargetForActor()
        {
            GameBattler _battler = null;
            // If Effect scope is ally, then it's an actor, anything else is an enemy
            if (TargetIndex == -1)
            {
                _battler = null;
            }
            else if (IsForOneFriend())
            {
                _battler = InGame.Party.Actors[TargetIndex];
            }
            else
            {
                _battler = InGame.Troops.Npcs[TargetIndex];
            }
            // Clear action if no target exists
            if (_battler == null || !_battler.IsExist)
            {
                Clear();
            }
        }

        /// <summary>
        /// Last Target (for Enemy)
        /// </summary>
        public void DecideLastTargetForEnemy()
        {
            GameBattler _battler = null;
            // If Effect scope is ally, then it's an actor, anything else is an enemy
            if (TargetIndex == -1)
            {
                _battler = null;
            }
            else if (IsForOneFriend())
            {
                _battler = InGame.Troops.Npcs[TargetIndex];
            }
            else
            {
                _battler = InGame.Party.Actors[TargetIndex];
            }
            // Clear action if no target exists
            if (_battler == null || !_battler.IsExist)
            {
                Clear();
            }
        }

        #endregion

    }

}