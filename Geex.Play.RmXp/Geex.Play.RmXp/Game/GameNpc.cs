using System.Collections.Generic;
using Geex.Run;

namespace Geex.Play.Rpg.Game
{
    ///<summary>This class handles an Non-playable-Character.</summary>
    public partial class GameNpc : GameBattler
    {
        #region Variables

        /// <summary>
        /// Troop id
        /// </summary>
        int troopId;

        /// <summary>
        /// Member index in the troop
        /// </summary>
        int memberIndex;

        /// <summary>
        /// Npc Id
        /// </summary>
        int npcId;

        #endregion

        #region Properties
        /// <summary>
        /// Get Npc id
        /// </summary> 
        public int Id
        {
            get
            {
                return npcId;
            }
        }

        /// <summary>
        /// Get member index
        /// </summary> 
        public override int Index
        {
            get
            {
                return memberIndex;
            }
        }

        /// <summary>
        /// Npc Name from data
        /// </summary> 
        public string Name
        {
            get
            {
                return Data.Npcs[npcId].Name;
            }
        }

        /// <summary>
        /// Get base max hp
        /// </summary> 
        public override int BaseMaxHp
        {
            get
            {
                return Data.Npcs[npcId].MaxHp;
            }
        }

        /// <summary>
        /// Get base max sp
        /// </summary> 
        public override int BaseMaxSp
        {
            get
            {
                return Data.Npcs[npcId].MaxSp;
            }
        }

        /// <summary>
        /// Get base str</summary> 
        public override int BaseStr
        {
            get
            {
                return Data.Npcs[npcId].Str;
            }
        }

        ///<summary>Get base dex</summary> 
        public override int BaseDex
        {
            get
            {
                return Data.Npcs[npcId].Dex;
            }
        }

        ///<summary>Get base agi</summary> 
        public override int BaseAgi
        {
            get
            {
                return Data.Npcs[npcId].Agi;
            }
        }

        ///<summary>Get base int</summary> 
        public override int BaseInt
        {
            get{
                return Data.Npcs[npcId].Intel;
            }
        }

        ///<summary>Get base atk</summary> 
        public override int BaseAtk
        {
            get
            {
                return Data.Npcs[npcId].Atk;
            }
        }

        ///<summary>Get base pdef</summary> 
        public override int BasePdef
        {
            get
            {
                return Data.Npcs[npcId].Pdef;
            }
        }

        ///<summary>Get base mdef</summary> 
        public override int BaseMdef
        {
            get
            {
                return Data.Npcs[npcId].Mdef;
            }
        }

        ///<summary>Get base eva</summary> 
        public override int BaseEva
        {
            get
            {
                return Data.Npcs[npcId].Eva;
            }
        }

        ///<summary>Get Offensive Animation ID for Normal Attacks</summary> 
        public override int Animation1Id
        {
            get
            {
                return Data.Npcs[npcId].Animation1Id;
            }
        }

        ///<summary>Get Target Animation ID for Normal Attacks</summary> 
        public override int Animation2Id
        {
            get
            {
                return Data.Npcs[npcId].Animation2Id;
            }
        }

        ///<summary>Get State Effectiveness</summary>
        public override List<short> StateRanks
        {
            get
            {
                return Data.Npcs[npcId].StateRanks;
            }
        }

        ///<summary>Get Normal Attack Element</summary> 
        public override List<short> ElementSet
        {
            get
            {
                return new List<short>();
            }
        }

        ///<summary>Get Normal Attack State Change (+)</summary> 
        public override List<short> PlusStateSet
        {
            get
            {
                return new List<short>();
            }
        }

        ///<summary>Get Normal Attack State Change (-)</summary> 
        public override List<short> MinusStateSet
        {
            get
            {
                return new List<short>();
            }
        }

        ///<summary>Get Npc actions from data</summary> 
        public Npc.Action[] Actions
        {
            get
            {
                return Data.Npcs[npcId].Actions;
            }
        }

        ///<summary>Get experience provided by Npc killing</summary> 
        public int Exp
        {
            get
            {
                return Data.Npcs[npcId].Exp;
            }
        }

        ///<summary>Get gold provided by Npc killing</summary>  
        public int Gold
        {
            get
            {
                return Data.Npcs[npcId].Gold;
            }
        }

        ///<summary>Get id of the item provided by Npc killing</summary> 
        public int ItemId
        {
            get
            {
                return Data.Npcs[npcId].ItemId;
            }
        }

        ///<summary>Get id of the weapon provided by Npc killing</summary>    
        public int WeaponId
        {
            get
            {
                return Data.Npcs[npcId].WeaponId;
            }
        }

        ///<summary>Get id of the armor provided by Npc killing</summary>   
        public int ArmorId
        {
            get
            {
                return Data.Npcs[npcId].ArmorId;
            }
        }

        ///<summary>Get the probability of treasure appearance</summary> 
        public int TreasureProb
        {
            get
            {
                return Data.Npcs[npcId].TreasureProb;
            }
        }

        ///<summary>Get Battle Screen X-Coordinate</summary>
        public override int ScreenX
        {
            get
            {
                return Data.Troops[troopId].Members[memberIndex].X;
            }
        }

        ///<summary>Get Battle Screen Y-Coordinate</summary>
        public override int ScreenY
        {
            get
            {
                return Data.Troops[troopId].Members[memberIndex].Y;
            }
        }

        ///<summary>Get Battle Screen Z-Coordinate</summary>
        public override int ScreenZ
        {
            get
            {
                return ScreenY;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="troop_id">Troop id</param>
        /// <param Name="member_index">Member index in the troop</param>
        public GameNpc(int troop_id, int member_index)
            : base()
        {
            this.troopId = troop_id;
            this.memberIndex = member_index;
            Troop _troop = Data.Troops[troop_id];
            npcId = _troop.Members[member_index].NpcId;
            Npc _npc = Data.Npcs[npcId];
            BattlerName = _npc.BattlerName;
            BattlerHue = _npc.BattlerHue;
            IsHidden = _troop.Members[member_index].Hidden;
            IsImmortal = _troop.Members[member_index].Immortal;
            Hp = MaxHp;
            Sp = MaxSp;
        }
        /// <summary>
        /// Parameterless constructor for load/save
        /// </summary>
        public GameNpc()
        {
        }

        #endregion

        #region Methods


        ///<summary>Get Element Revision Value</summary>
        ///<param Name="element_id">Element ID</param>
        ///<returns>Modified element rate</returns>
        public override int ElementRate(short element_id)
        {
            //Get values corresponding to element effectiveness
            int[] _table = { 0, 200, 150, 100, 50, 0, -100 };
            int _result = _table[Data.Npcs[npcId].ElementRanks[element_id]];
            bool _f = false;
            //If this element is protected by states, then it's reduced by half
            for (short i=0;i<states.Count;i++)
            {
                _f |= Data.States[states[i]].GuardElementSet.Contains(element_id);
            }
            if (_f)
            {
                _result /= 2;
            }
            return _result;
        }

        ///<summary>Determine State Guard - No state Guard for enemies</summary>
        ///<param Name="state_id">state ID</param>
        ///<returns>True if Npc state is guard (never the case)</returns>
        public override bool IsStateGuard(short state_id)
        {
            //No state Guard for enemies
            return false;
        }

        ///<summary>
        ///Action Escape. The monster's hidden flag is set, and 
        ///its current action is cleared.
        ///</summary>
        public override void Escape()
        {
            IsHidden = true;
            this.CurrentAction.Clear();
        }

        ///<summary>
        ///This method resolves the Effect of the command "Transform Monster". 
        ///A call to make_action is made, which causes the monster to determine a new 
        ///action in accordance with its new identity.
        ///</summary>
        ///<param Name="npc_id">Id of the target monster</param>
        public void Transform(int npc_id)
        {
            this.npcId = npc_id;
            BattlerName = Data.Npcs[npc_id].BattlerName;
            BattlerHue = Data.Npcs[npc_id].BattlerHue;
            MakeAction();
        }

        ///<summary>
        ///This method determines which action the monster will take next. 
        ///</summary>
        public void MakeAction()
        {
            //First, if the monster already has an action set, it is cleared.
            this.CurrentAction.Clear();
            //Checks if monster isn't paralyzed by status. If it is, return
            //without giving the monster an action.
            if (!this.IsMovable)
            {
                return;
            }
            //Determine which subset of the monster's actions meet their preconditions 
            //as defined in the database.
            List<Npc.Action> available_actions = new List<Npc.Action>();
            int rating_max = 0;
            for (int i=0;i<this.Actions.Length;i++)
            {
                int n = InGame.Temp.BattleTurn;
                //Absolute turn value specified
                int a = this.Actions[i].ConditionTurnA;
                //Turn multiple specified
                int b = this.Actions[i].ConditionTurnB;
                //If the turn requirement is not met, continue
                if ((b == 0 && n != a) || (b > 0 && (n < 1 || n < a || n % b != a % b)))
                {
                    continue;
                }
                //Checks the HP percentage precondition
                if (this.Hp * 100.0 / this.MaxHp > this.Actions[i].ConditionHp)
                {
                    continue;
                }
                //Checks highest-level precondition
                if (InGame.Party.MaxLevel < this.Actions[i].ConditionLevel)
                {
                    continue;
                }
                //if any switch is required
                int switch_id = this.Actions[i].ConditionSwitchId;
                if (switch_id > 0 && !( InGame.Switches.Arr[switch_id] ))
                {
                    continue;
                }
                //If passes all of these tests, then action is added to the available_actions
                available_actions.Add(this.Actions[i]);
                if (this.Actions[i].Rating > rating_max)
                {
                    rating_max = this.Actions[i].Rating;
                }
            }

            //Now determines each action's relative chance of executing by adjusting their ratings 
            //based on the highest rating in the array of available actions. If an action's 
            //unadjusted rating is 3 or more points below the rating of the highest-rated action 
            //in the array of available actions, that action will never be chosen.

            int ratings_total = 0;
            //Raitings total calculation
            for (int i=0;i<available_actions.Count;i++)
            {
                if (available_actions[i].Rating > rating_max - 3)
                {
                    ratings_total += available_actions[i].Rating - (rating_max - 3);
                }
            }
            if (ratings_total > 0)
            {
                //Makes the determination of which action to execute
                int value = InGame.Rnd.Next(ratings_total);
                for (int i=0;i<available_actions.Count;i++)
                {
                    if (available_actions[i].Rating > rating_max - 3)
                    {
                        //Chose this action if value < rating, determines target
                        if (value < available_actions[i].Rating - (rating_max - 3))
                        {
                            this.CurrentAction.kind = available_actions[i].Kind;
                            this.CurrentAction.basic = available_actions[i].Basic;
                            this.CurrentAction.SkillId = available_actions[i].SkillId;
                            this.CurrentAction.DecideRandomTargetforEnemy();
                            return;
                        }
                        //Else value is decremented until an action is chosen
                        else
                        {
                            value -= available_actions[i].Rating - (rating_max - 3);
                        }
                    }
                }
            }
        }

        #endregion

    }
}
