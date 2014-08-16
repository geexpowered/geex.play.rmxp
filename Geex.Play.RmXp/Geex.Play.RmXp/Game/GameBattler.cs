using System;
using System.Collections.Generic;
using Geex.Run;


namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class deals with battlers. It's used as a superclass for the GameActor and GameNpc classes.
    /// </summary>
    public partial class GameBattler
    {
        #region Constants

        /// <summary>
        /// Max STR, DEX, AGI and INT
        /// </summary>
        protected const int MAX_STAT = 999;

        /// <summary>
        /// Min STR, DEX, AGI and INT
        /// </summary>
        protected const int MIN_STAT = 1;

        /// <summary>
        /// Max SP and HP
        /// </summary>
        protected const int MAX_STAT_POINTS = 99999;

        /// <summary>
        /// Max XP
        /// </summary>
        protected const int MAX_XP = 9999999;

        #endregion

        #region Variables

        #region Public variables

        /// <summary>
        /// States
        /// </summary>
        public List<short> states = new List<short>();

        /// <summary>
        /// Battler file Name
        /// </summary>
        public string BattlerName;

        /// <summary>
        /// Battler hue
        /// </summary>
        public int BattlerHue;

        /// <summary>
        /// Animation ID
        /// </summary>
        public int AnimationId;

        /// <summary>
        /// Animation Hit flag
        /// </summary>
        public bool IsAnimationHit;

        /// <summary>
        /// White flash flag
        /// </summary>
        public bool IsWhiteFlash;

        /// <summary>
        /// Damage display flag
        /// </summary>
        public bool IsDamagePop;

        /// <summary>
        /// damage value
        /// </summary>
        public string Damage;

        /// <summary>
        /// Critical flag
        /// </summary>
        public bool IsCritical;

        /// <summary>
        /// Hidden flag
        /// </summary>
        public bool IsHidden;

        /// <summary>
        /// Immortal flag
        /// </summary>
        public bool IsImmortal;

        /// <summary>
        /// Blink flag
        /// </summary>
        public bool IsBlink;

        /// <summary>
        /// Current action
        /// </summary>
        public GameBattleAction CurrentAction;

        /// <summary>
        /// Maintenance turns
        /// </summary>
        public GeexDictionary<short, short> StatesTurn = new GeexDictionary<short, short>();

        /// <summary>
        /// Damage value
        /// </summary>
        protected int DamageValue;

        /// <summary>
        /// State changed flag
        /// </summary>
        protected bool IsStateChanged;

        /// <summary>
        /// Max HP Bonus
        /// </summary>
        public int MaxhpPlus;

        /// <summary>
        /// Max SP Bonus
        /// </summary>
        public int MaxspPlus;

        /// <summary>
        /// Strenght Bonus
        /// </summary>
        public int StrPlus;

        /// <summary>
        /// Dexterity Bonus
        /// </summary>
        public int DexPlus;

        /// <summary>
        /// Agility Bonus
        /// </summary>
        public int AgiPlus;

        /// <summary>
        /// Intelligence Bonus
        /// </summary>
        public int IntPlus;

        #endregion

        #endregion

        #region Properties
        /// <summary>Battler index</summary>
        public virtual int Level { get; set; }

        /// <summary>Battler index</summary>
        public virtual int Index { get; set; }

        ///<summary>Get Battle Screen X-Coordinate</summary>
        public virtual int ScreenX { get; set; }

        ///<summary>Get Battle Screen Y-Coordinate</summary>
        public virtual int ScreenY { get; set; }

        ///<summary>Get Battle Screen Z-Coordinate</summary>
        public virtual int ScreenZ { get; set; }

        ///<summary>Get State Effectiveness</summary>
        public virtual List<short> StateRanks { get { return new List<short>(); } }

        ///<summary>Get Normal Attack Element</summary> 
        public virtual List<short> ElementSet { get { return new List<short>(); } }

        ///<summary>Get Normal Attack State Change (+)</summary> 
        public virtual List<short> PlusStateSet { get { return new List<short>(); } }

        ///<summary>Get Normal Attack State Change (-)</summary> 
        public virtual List<short> MinusStateSet { get { return new List<short>(); } }

        ///<summary>Get Offensive Animation ID for Normal Attacks</summary> 
        public virtual int Animation1Id { get { return 0; } }

        ///<summary>Get Target Animation ID for Normal Attacks</summary> 
        public virtual int Animation2Id { get { return 0; } }

        ///<summary>Get Base Max HP</summary> 
        public virtual int BaseMaxHp { get { return 0; } }

        ///<summary>Get Base Max SP</summary> 
        public virtual int BaseMaxSp { get { return 0; } }

        ///<summary>Get Base Strenght</summary> 
        public virtual int BaseStr { get { return 0; } }

        ///<summary>Get Base Dexterity</summary> 
        public virtual int BaseDex { get { return 0; } }

        ///<summary>Get Base Agility</summary> 
        public virtual int BaseAgi { get { return 0; } }

        ///<summary>Get Base Intelligence</summary> 
        public virtual int BaseInt { get { return 0; } }

        ///<summary>Get Base Attack Power</summary> 
        public virtual int BaseAtk { get { return 0; } }

        ///<summary>Get Base Physical Defense</summary> 
        public virtual int BasePdef { get { return 0; } }

        ///<summary>Get Base Magickal Defense</summary> 
        public virtual int BaseMdef { get { return 0; } }

        ///<summary>Get Base Evasion Chances</summary> 
        public virtual int BaseEva { get { return 0; } }

        /// <summary>
        /// Max Hp
        /// </summary>
        public virtual int MaxHp
        {
            get
            {
                double n = Math.Min(Math.Max(BaseMaxHp + MaxhpPlus, 1), 99999);
                for (int i = 0; i < states.Count; i++)
                {
                    n *= Data.States[states[i]].MaxhpRate / 100;
                }
                n = Math.Min(Math.Max(n, 1), MAX_STAT_POINTS);
                return (int)n; ;
            }
            /*set
            {
                maxhp_plus += value - MaxHp;
                maxhp_plus = Math.Min(Math.Max(maxhp_plus, -9999), 9999);
                hp = Math.Min(hp, MaxHp);
            }*/
        }

        /// <summary>
        /// Max Sp
        /// </summary>
        public virtual int MaxSp
        {
            get
            {
                double _n = Math.Min(Math.Max(BaseMaxSp + MaxspPlus, 1), MAX_STAT_POINTS);
                for (int i = 0; i < states.Count; i++)
                {
                    _n *= Data.States[states[i]].MaxspRate / 100.0;
                }
                _n = Math.Min(Math.Max(Math.Round(_n), 1), MAX_STAT_POINTS);
                return (int)_n;
            }
            /*set
            {
                maxsp_plus += value - MaxSp;
                maxsp_plus = Math.Min(Math.Max(maxsp_plus, -9999), 9999);
                sp = Math.Min(sp, this.MaxSp);
            }*/
        }

        /*/// <summary>
        /// HP
        /// </summary>
        public int hp
        {
            get { return localHp; }
            set
            {
                localHp = Math.Max(Math.Min(value, MaxHp), 0);
                // add or exclude incapacitation
                for (short i = 0; i < Data.States.Length; i++)
                {
                    if (Data.States[i].zero_hp)
                    {
                        if (this.is_dead)
                        {
                            add_state(i);
                        }
                        else
                        {
                            remove_state(i);
                        }
                    }
                }
            }
        }*/
        public int Hp
        {
            get
            {
                return Math.Max(Math.Min(localHp, MaxHp), 0);
            }
            set
            {
                localHp = value;
                for (short i = 0; i < Data.States.Length; i++)
                {
                    if (Data.States[i].ZeroHp)
                    {
                        if (this.IsDead)
                        {
                            AddState(i);
                        }
                        else
                        {
                            RemoveState(i);
                        }
                    }
                }
            }
        }
        int localHp;

        /// <summary>
        /// SP
        /// </summary>
        public int Sp
        {
            get
            {
                return Math.Max(Math.Min(localSp, MaxSp), 0);
            }
            set
            {
                localSp = value;
            }
        }
        int localSp;
        /// <summary>
        /// Hit chances
        /// </summary>
        public int Hit
        {
            get
            {
                double n = 100;
                for (int i = 0; i < states.Count; i++)
                {
                    n *= Data.States[states[i]].HitRate / 100.0;
                }
                return (int)Math.Round(n);
            }
            set { localHit = value; }
        }
        int localHit;

        /// <summary>
        /// Attack power
        /// </summary>
        public int Atk
        {
            get
            {
                double n = BaseAtk;
                for (int i = 0; i < states.Count; i++)
                {
                    n *= Data.States[states[i]].AtkRate / 100.0;
                }
                return (int)Math.Round(n);
            }
            set { localAtk = value; }
        }
        int localAtk;

        /// <summary>
        /// Physical defense power
        /// </summary>
        public int Pdef
        {
            get
            {
                double n = BasePdef;
                for (int i = 0; i < states.Count; i++)
                {
                    n *= Data.States[states[i]].PdefRate / 100.0;
                }
                return (int)Math.Round(n);
            }
            set { localPdef = value; }
        }
        int localPdef;

        /// <summary>
        /// Magickal defense power
        /// </summary>
        public int Mdef
        {
            get
            {
                double n = BaseMdef;
                for (int i = 0; i < states.Count; i++)
                {
                    n *= Data.States[states[i]].MdefRate / 100.0;
                }
                return (int)Math.Round(n);
            }
            set { localMdef = value; }
        }
        int localMdef;

        /// <summary>
        /// Evasion
        /// </summary>
        public int Eva
        {
            get
            {
                int n = BaseEva;
                for (int i = 0; i < states.Count; i++)
                {
                    n += Data.States[states[i]].Eva;
                }
                return n;
            }
            set { localEva = value; }
        }
        int localEva;

        /// <summary>
        /// Strenght
        /// </summary>
        public int Str
        {
            get
            {
                double _n = Math.Min(Math.Max(BaseStr + StrPlus, MIN_STAT), MAX_STAT);
                for (int i = 0; i < states.Count; i++)
                {
                    _n *= Data.States[states[i]].StrRate / 100.0;
                }
                _n = Math.Min(Math.Max(Math.Round(_n), MIN_STAT), MAX_STAT);
                return (int)_n;
            }
            set
            {
                StrPlus += value - this.Str;
                StrPlus = Math.Min(Math.Max(StrPlus, -999), 999);
            }
        }

        /// <summary>
        /// Dexterity
        /// </summary>
        public int Dex
        {
            get
            {
                double _n = Math.Min(Math.Max(BaseDex + DexPlus, MIN_STAT), MAX_STAT);
                for (int i=0;i<states.Count;i++)
                {
                    _n *= Data.States[states[i]].DexRate / 100.0;
                }
                _n = Math.Min(Math.Max(Math.Round(_n), MIN_STAT), MAX_STAT);
                return (int)_n;
            }
            set
            {
                DexPlus += value - this.Dex;
                DexPlus = Math.Min(Math.Max(DexPlus, -999), 999);
            }
        }

        /// <summary>
        /// Agility
        /// </summary>
        public int Agi
        {
            get
            {
                double _n = Math.Min(Math.Max(BaseAgi + AgiPlus, MIN_STAT), MAX_STAT);
                for (int i = 0; i < states.Count; i++)
                {
                    _n *= Data.States[states[i]].AgiRate / 100.0;
                }
                _n = Math.Min(Math.Max(Math.Round(_n), MIN_STAT), MAX_STAT);
                return (int)_n;
            }
            set
            {
                AgiPlus += value - this.Agi;
                AgiPlus = Math.Min(Math.Max(AgiPlus, -999), 999);
            }
        }

        /// <summary>
        /// Intelligence
        /// </summary>
        public int Intel
        {
            get
            {
                double _n = Math.Min(Math.Max(BaseInt + IntPlus, MIN_STAT), MAX_STAT);
                for (int i = 0; i < states.Count; i++)
                {
                    _n *= Data.States[states[i]].IntRate / 100.0;
                }
                _n = Math.Min(Math.Max(Math.Round(_n), MIN_STAT), MAX_STAT);
                return (int)_n;
            }
            set
            {
                IntPlus += value - this.Intel;
                IntPlus = Math.Min(Math.Max(IntPlus, -999), 999);
            }
        }

        /// <summary>
        /// Decide if there are slip damage
        /// </summary>
        public bool IsSlipDamage
        {
            get
            {
                for (int i = 0; i < states.Count; i++)
                {
                    if (Data.States[states[i]].SlipDamage)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Decide Incapacitation
        /// </summary>
        public bool IsDead
        {
            get
            {
                return (Hp == 0 && !IsImmortal);
            }
        }

        /// <summary>
        /// Decide if Command is Inputable
        /// </summary>
        public bool IsInputable
        {
            get
            {
                return (!IsHidden && Restriction <= 1);
            }
        }

        /// <summary>
        /// Decide if Action is Possible
        /// </summary>
        public bool IsMovable
        {
            get
            {
                return (!IsHidden && Restriction < 4);
            }
        }

        /// <summary>
        /// Decide if Guarding
        /// </summary>
        public bool IsGuarding
        {
            get
            {
                return (CurrentAction.kind == 0 && CurrentAction.basic == 1);
            }
        }

        /// <summary>
        /// Decide if Resting
        /// </summary>
        public bool IsResting
        {
            get
            {
                return (CurrentAction.kind == 0 && CurrentAction.basic == 3);
            }
        }
        
        /// <summary>
        /// Decide Existance
        /// </summary>
        public bool IsExist
        {
            get
            {
                return (!IsHidden && (Hp > 0 || IsImmortal));
            }
        }

        /// <summary>
        /// Decide HP 0
        /// </summary>
        public bool IsHp0
        {
            get
            {
                return (!IsHidden && Hp == 0);
            }
        }

        /// <summary>
        /// Get the maximum restriction
        /// </summary>
        public int Restriction
        {
            get
            {
                int _restriction_max = 0;
                for (int i = 0; i < states.Count; i++)
                {
                    if (Data.States[states[i]].Restriction >= _restriction_max)
                    {
                        _restriction_max = Data.States[states[i]].Restriction;
                    }
                }
                return _restriction_max;
            }
        }

        /// <summary>
        /// Decide if can't gain exp 
        /// </summary>
        public bool IsCanGetExp
        {
            get
            {
                for (int i = 0; i < states.Count; i++)
                {
                    if (Data.States[states[i]].CantGetExp)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Decide if can't evade
        /// </summary>
        public bool IsCantEvade
        {
            get
            {
                for (int i = 0; i < states.Count; i++)
                {
                    if (Data.States[states[i]].CantEvade)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Get state Animation ID
        /// </summary>
        /// <returns>state Animation ID</returns>
        public int StateAnimationId
        {
            get
            {
                // If no states are added
                if (states.Count == 0)
                {
                    return 0;
                }
                // Return state Animation ID with maximum rating
                return Data.States[states[0]].AnimationId;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public GameBattler()
        {
            CurrentAction = new GameBattleAction();
            BattlerName = "";
            BattlerHue = 0;
            localHp = 0;
            localSp = 0;
            states.Clear();
            StatesTurn.Clear();
            MaxhpPlus = 0;
            MaxspPlus = 0;
            StrPlus = 0;
            DexPlus = 0;
            AgiPlus = 0;
            IntPlus = 0;
            IsHidden = false;
            IsImmortal = false;
            IsDamagePop = false;
            Damage = "";
            IsCritical = false;
            AnimationId = 0;
            IsAnimationHit = false;
            IsWhiteFlash = false;
            IsBlink = false;
        }

        #endregion

        #region Method

        /// <summary>
        /// Action Escape. The monster's hidden flag is set, and 
        /// its current action is cleared. Implemented by GameNpc.
        /// </summary>
        public virtual void Escape()
        { 
        
        }

        /// <summary>
        /// Recover All
        /// </summary>
        public void RecoverAll()
        {
            Hp = MaxHp;
            Sp = MaxSp;
            List<short> _states_clone = new List<short>(states);
            for (int i=0;i<_states_clone.Count;i++)
            {
                RemoveState(_states_clone[i]);
            }
        }

        /// <summary>
        /// Determine Action Speed
        /// </summary>
        public void MakeActionSpeed()
        {
            CurrentAction.speed = Agi + InGame.Rnd.Next(10 + Agi / 4);
        }

        /// <summary>
        /// Check if the Battler is under a certain state
        /// </summary>
        /// <param Name="state_id">state id</param>
        /// <returns>true if the applicable state is added</returns>
        public bool IsState(short state_id)
        {
            return states.Contains(state_id);
        }

        /// <summary>
        /// Determine if a state is full or not.
        /// </summary>
        /// <param Name="state_id">state ID</param>
        /// <returns>true if the number of maintenance turns is equal to the
        /// lowest number of natural removal turns.</returns>
        public bool IsStateFull(short state_id)
        {
            // Return false if the applicable state is not added.
            if (!this.IsState(state_id))
            {
                return false;
            }
            // Return true if the number of maintenance turns is -1 (auto state).
            if (StatesTurn[state_id] == -1)
            {
                return true;
            }
            // Return true if the number of maintenance turns is equal to the
            // lowest number of natural removal turns.
            return StatesTurn[state_id] == Data.States[state_id].HoldTurn;
        }

        /// <summary>
        /// Add State
        /// </summary>
        /// <param Name="state_id">state ID</param>
        /// <param Name="force">forcefully added flag (used to deal with auto state)</param>
        public void AddState(short state_id, bool force)
        {
            // For an ineffective state
            if (Data.States[state_id] == null)
            {
                // End Method
                return;
            }
            // If not forcefully added
            if (!force)
            {
                // A state loop already in existance
                for (int i=0;i<states.Count;i++)
                {
                    // If a new state is included in the state change (-) of an existing
                    // state, and that state is not included in the state change (-) of
                    // a new state (example: an attempt to add poison during dead)
                    if (Data.States[states[i]].MinusStateSet.Contains(state_id) &&
                       !Data.States[state_id].MinusStateSet.Contains(states[i]))
                    {
                        // End Method
                        return;
                    }
                }
            }
            // If this state is not added
            if (!IsState(state_id))
            {
                // Add state ID to states array
                states.Add(state_id);
                StatesTurn.Add(state_id, 0);
                // If option [regarded as HP 0]is effective
                if (Data.States[state_id].ZeroHp)
                {
                    // Change HP to 0
                    Hp = 0;
                }
                // All state loops
                for (short i = 0; i < Data.States.Length; i++)
                {
                    // Dealing with a state change (+)
                    if (Data.States[state_id].PlusStateSet.Contains(i))
                    {
                        AddState(i);
                    }
                    // Dealing with a state change (-)
                    if (Data.States[state_id].MinusStateSet.Contains(i))
                    {
                        RemoveState(i);
                    }
                }
                // line change to a large rating order (if value is the same, then a
                // strong restriction order)
                states.Sort(InGame.StateComparer);
            }
            // If added forcefully
            if (force)
            {
                // Set the natural removal's lowest number of turns to -1
                StatesTurn[state_id] = -1;
            }
            // If not added forcefully
            if (!(StatesTurn[state_id] == -1))
            {
                // Set the natural removal's lowest number of turns
                StatesTurn[state_id] = Data.States[state_id].HoldTurn;
            }
            // If unable to move
            if (!IsMovable)
            {
                // Clear action
                CurrentAction.Clear();
            }
            // Check the maximum value of HP and SP
            if (Hp > this.MaxHp)
            {
                Hp = this.MaxHp;
            }
            if (Sp > this.MaxSp)
            {
                Sp = this.MaxSp;
            }
        }

        /// <summary>
        /// Add State
        /// </summary>
        /// <param Name="state_id">state ID</param>
        public void AddState(short state_id)
        {
            AddState(state_id, false);
        }

        /// <summary>
        /// Remove State
        /// </summary>
        /// <param Name="state_id">state ID</param>
        /// <param Name="force">forcefully removed flag (used to deal with auto state)</param>
        public void RemoveState(short state_id, bool force)
        {
            // If this state is added
            if (IsState(state_id))
            {
                // If a forcefully added state is not forcefully removed
                if (StatesTurn[state_id] == -1 && !force)
                {
                    // End Method
                    return;
                }
                // If current HP is at 0 and options are effective [regarded as HP 0]
                if (Hp == 0 && Data.States[state_id].ZeroHp)
                {
                    // Determine if there's another state [regarded as HP 0] or not
                    bool zero_hp = false;
                    for (int i = 0; i < states.Count; i++)
                    {
                        if (i != state_id && Data.States[states[i]].ZeroHp)
                        {
                            zero_hp = true;
                        }
                    }
                    // Change HP to 1 if OK to remove incapacitation.
                    if (zero_hp == false)
                    {
                        Hp = 1;
                    }
                }
                // Delete state ID from states and states_turn hash array
                states.Remove(state_id);
                StatesTurn.Remove(state_id);
            }
            // Check maximum value for HP and SP
            if (Hp > this.MaxHp)
            {
                Hp = this.MaxHp;
            }
            if (Sp > this.MaxSp)
            {
                Sp = this.MaxSp;
            }
        }

        /// <summary>
        /// Remove State
        /// </summary>
        /// <param Name="state_id">state ID</param>
        public void RemoveState(short state_id)
        {
            RemoveState(state_id, false);
        }

        /// <summary>
        /// Remove Battle States (called up during end of battle)
        /// </summary>
        public void RemoveStatesBattle()
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (Data.States[states[i]].BattleOnly)
                {
                    RemoveState(states[i], false);
                }
            }
        }

        /// <summary>
        /// Natural Removal of States (called up each turn)
        /// </summary>
        public void RemoveStatesAuto()
        {
            // Array used to mark states to remove
            bool[] toRemove = new bool[StatesTurn.Keys.Count];
            short[] toRemoveKeys = new short[StatesTurn.Keys.Count];
            // Array used to mark states turns to decrease
            bool[] toDecrease = new bool[StatesTurn.Keys.Count];
            short[] toDecreaseKeys = new short[StatesTurn.Keys.Count];
            // Index
            short index = 0;
            // Set remove marks
            foreach (short i in StatesTurn.Keys)
            {
                if (StatesTurn[i] > 0)
                {
                    toDecrease[index] = true;
                    toDecreaseKeys[index] = i;
                    //StatesTurn[i]--;
                }
                else if (InGame.Rnd.Next(100) < Data.States[i].AutoReleaseProb)
                {
                    toRemove[index] = true;
                    toRemoveKeys[index] = i;
                    //RemoveState(i, false);
                }
                index++;
            }
            // Modify states and avoid modification of dictionary during iteration
            for (short j = 0; j < index; j++)
            {
                if (toRemove[j])
                {
                    RemoveState(toRemoveKeys[j], false);
                }
                if (toDecrease[j])
                {
                    StatesTurn[toDecreaseKeys[j]]--;
                }
            }

        }

        /// <summary>
        /// State Removed by Shock (called up each time physical damage occurs)
        /// </summary>
        public void RemoveStatesShock()
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (InGame.Rnd.Next(100) < Data.States[states[i]].ShockReleaseProb)
                {
                    RemoveState(states[i], false);
                }
            }
        }

        public virtual bool IsStateGuard(short state_id)
        {
            return false;
        }

        /// <summary>
        /// Decide and add the states which are now effectives
        /// </summary>
        /// <param Name="plus_state_set">State Change (+)</param>
        /// <returns>true if effective</returns>
        public bool StatesPlus(List<short> plus_state_set)
        {
            bool _effective = false;

            for (short i = 0; i < plus_state_set.Count; i++)
            {
                if (!IsStateGuard(plus_state_set[i]))
                {
                    _effective |= (IsStateFull(plus_state_set[i]) == false);
                    if (Data.States[plus_state_set[i]].Nonresistance)
                    {
                        IsStateChanged = true;
                        AddState(plus_state_set[i], false);
                    }
                    else if (!IsStateFull(plus_state_set[i]))
                    {
                        int[] n = { 0, 100, 80, 60, 40, 20, 0 };
                        if (InGame.Rnd.Next(100) < n[StateRanks[plus_state_set[i]]])
                        {
                            IsStateChanged = true;
                            AddState(plus_state_set[i], false);
                        }
                    }
                }
            }
            return _effective;
        }

        /// <summary>
        /// Decide and remove the states which are not effective anymore
        /// </summary>
        /// <param Name="minus_state_set"></param>
        /// <returns>true </returns>
        public bool StatesMinus(List<short> minus_state_set)
        {
            bool _effective = true;
            foreach (short i in minus_state_set)
            {
                _effective |= IsState(i);
                IsStateChanged = true;
                RemoveState(i, false);
            }
            return _effective;
        }

        /// <summary>
        /// Decide if specified skill can be used
        /// </summary>
        /// <param Name="skill_id">skill ID</param>
        /// <returns>true if skill can be used</returns>
        public virtual bool IsSkillCanUse(int skill_id)
        {
            // If there's not enough SP, the skill cannot be used.
            if (Data.Skills[skill_id].SpCost > Sp)
            {
                return false;
            }
            // Unusable if incapacitated
            if (IsDead) { return false; }
            // If silent, only physical skills can be used
            if (Data.Skills[skill_id].AtkF == 0 && Restriction == 1)
            {
                return false;
            }
            // Get usable time
            int occasion = Data.Skills[skill_id].Occasion;
            // If in battle
            if (InGame.Temp.IsInBattle)
            {
                // Usable with [Normal] and [Only Battle]
                return (occasion == 0 || occasion == 1);
            }
            else
            {
                // Usable with [Normal] and [Only Menu]
                return (occasion == 0 || occasion == 2);
            }
        }

        /// <summary>
        /// Calculating Element Correction
        /// </summary>
        /// <param Name="element_set">element</param>
        /// <returns>weakest element rate</returns>
        public int ElementsCorrect(List<short> element_set)
        {
            // If not an element
            if (element_set.Count == 0)
            {
                // Return 100
                return 100;
            }
            // Return the weakest object among the elements given
            // * "element_rate" method is defined by GameActor and GameNpc classes,
            //    which inherit from this class.
            int weakest = -100;
            foreach (short i in element_set)
            {
                weakest = Math.Max(weakest, this.ElementRate(i));
            }
            return weakest;
        }

        public virtual int ElementRate(short i)
        {
            return 100;
        }

        #region Attack effects

        /// <summary>
        /// Applying Normal Attack Effects
        /// </summary>
        /// <param Name="attacker">attacker</param>
        /// <returns>true if executed</returns>
        public bool AttackEffect(GameBattler attacker)
        {
            // Setup Attack Effect
            AttackEffectSetup();
            // First Hit Detection
            bool hit_result = AttackEffectFirstHitResult(attacker);
            // If Hit occurs
            if (hit_result)
            {
                // Calculate Basic Damage
                AttackEffectBaseDamage(attacker);
                // Element Correction
                AttackEffectElementCorrection(attacker);
                // If damage value is strictly positive
                if (this.DamageValue > 0)
                {
                    // Critical correction
                    AttackEffectCriticalCorrection(attacker);
                    // Guard correction
                    AttackEffectGuardCorrection();
                }
                // Dispersion
                AttackEffectDispersion();
                // Second Hit Detection
                hit_result = attack_effect_second_hit_result(attacker);
            }
            // If second Hit occurs
            if (hit_result)
            {
                // State Removed by Shock
                RemoveStatesShock();
                // Substract damage from HP and create the damage string from damage_value
                attack_effect_damage();
                // State change
                IsStateChanged = false;
                StatesPlus(attacker.PlusStateSet);
                StatesMinus(attacker.MinusStateSet);
            }
            // When missing
            else
            {
                // Apply Miss Results and create the damage string with the "Miss" value
                attack_effect_miss();
            }
            // End Method
            return true;
        }

        /// <summary>
        /// Attack Effect : Setup
        /// </summary>
        public void AttackEffectSetup()
        {
            this.IsCritical = false;
        }

        /// <summary>
        /// Attack Effect : First Hit Detection
        /// </summary>
        /// <param Name="attacker">attacker</param>
        /// <returns>true if Hit</returns>
        public bool AttackEffectFirstHitResult(GameBattler attacker)
        {
            return (InGame.Rnd.Next(100) < attacker.Hit);
        }

        /// <summary>
        /// Attack Effect : Base Damage
        /// </summary>
        /// <param Name="attacker">attacker</param>
        public void AttackEffectBaseDamage(GameBattler attacker)
        {
            int _atk = Math.Max(attacker.Atk - this.Pdef / 2, 0);
            this.DamageValue = (_atk * (20 + attacker.Str) / 20);
        }

        /// <summary>
        /// Attack Effect : Element Correction
        /// </summary>
        /// <param Name="attacker">attacker</param>
        public void AttackEffectElementCorrection(GameBattler attacker)
        {
            this.DamageValue *= ElementsCorrect(attacker.ElementSet);
            this.DamageValue /= 100;
        }

        /// <summary>
        /// Attack Effect : Critical Correction
        /// </summary>
        /// <param Name="attacker">attacker</param>
        public void AttackEffectCriticalCorrection(GameBattler attacker)
        {
            if (InGame.Rnd.Next(100) < 4 * attacker.Dex / this.Agi)
            {
                this.DamageValue *= 2;
                this.IsCritical = true;
            }
        }

        /// <summary>
        /// Attack Effect : Guard Correction
        /// </summary>
        public void AttackEffectGuardCorrection()
        {
            if (this.IsGuarding)
            {
                this.DamageValue /= 2;
            }
        }

        /// <summary>
        /// Attack Effect : Dispersion
        /// </summary>
        public void AttackEffectDispersion()
        {
            if (Math.Abs(this.DamageValue) > 0)
            {
                int _amp = Math.Max(Math.Abs(this.DamageValue) * 15 / 100, 1);
                this.DamageValue += InGame.Rnd.Next(_amp + 1) + InGame.Rnd.Next(_amp + 1) - _amp;
            }
        }

        /// <summary>
        /// Attack Effect : Second Hit Detection
        /// </summary>
        /// <param Name="attacker">attacker</param>
        /// <returns>true if this Battler is Hit</returns>
        public bool attack_effect_second_hit_result(GameBattler attacker)
        {
            Eva = 8 * this.Agi / attacker.Dex + this.Eva;
            Hit = this.DamageValue < 0 ? 100 : 100 - Eva;
            Hit = this.IsCantEvade ? 100 : Hit;
            return (InGame.Rnd.Next(100) < Hit);
        }

        public void attack_effect_damage()
        {
            this.Damage = DamageValue.ToString();
            this.Hp -= this.DamageValue;
        }

        /// <summary>
        /// Attack Effect : Miss
        /// </summary>
        public void attack_effect_miss()
        {
            this.Damage = "Miss";
            this.IsCritical = false;
        }

        #endregion

        #region Skill Effect

        /// <summary>
        /// Apply Skill Effects
        /// </summary>
        /// <param Name="user">skill user</param>
        /// <param Name="skill">skill</param>
        /// <returns>true if skill is effective</returns>
        public bool SkillEffect(GameBattler user, Skill skill)
        {
            // Skill Effects Setup
            SkillEffectSetup();
            // Return False If Out of Scope
            if (SkillEffectScope(skill))
            {
                return false;
            }
            // Setup Effective
            bool effective = SkillEffectEffectiveSetup(skill);
            // First Hit detection
            this.Hit = SkillEffectFirstHitResult(user, skill);
            bool hit_result = (InGame.Rnd.Next(100) < Hit);
            // Set effective flag if skill is uncertain
            effective = SkillEffectEffectiveCorrection(effective, Hit);
            // If Hit occurs
            if (hit_result)
            {
                // Calculate power
                int power = SkillEffectPower(user, skill);
                // Calculate rate
                int rate = SkillEffectRate(user, skill);
                // Calculate basic damage
                SkillEffectBaseDamage(power, rate);
                // Element correction
                SkillEffectElementCorrection(skill);
                // If damage value is strictly positive
                if (this.DamageValue > 0)
                {
                    // Guard correction
                    SkillEffectGuardCorrection();
                }
                // Dispersion
                SkillEffectDisperation(skill);
                // Second Hit detection
                Hit = SkillEffectSecondHitResult(user, skill);
                hit_result = (InGame.Rnd.Next(100) < Hit);
                // Set effective flag if skill is uncertain
                effective = SkillEffectEffectiveCorrection(effective, Hit);
            }
            // If Hit occurs
            if (hit_result)
            {
                // Physical Hit Detection
                if (SkillEffectPhysicalHitResult(skill))
                {
                    effective = true;
                }
                // Deal Damage
                effective = SkillEffectDamage();
                // State change
                IsStateChanged = false;
                effective |= StatesPlus(skill.PlusStateSet);
                effective |= StatesMinus(skill.MinusStateSet);
                // Skill Effect 0 Power Test
                SkillEffectPower0(skill);
            }
            // If miss occurs
            else
            {
                // Apply Miss Effects
                SkillEffectMiss();
            }
            // Skill Effect Damage Fix
            SkillEffectDamagefix();
            // End Method
            return effective;
        }

        /// <summary>
        /// Skill Effect : Setup
        /// </summary>
        public void SkillEffectSetup()
        {
            this.IsCritical = false;
        }

        /// <summary>
        /// Skill Effect : Scope Test
        /// </summary>
        /// <param Name="skill">skill</param>
        /// <returns>true if scope is for ally with 1+ HP, and your own HP = 0,
        /// or skill scope is for ally with 0, and your own HP = 1+</returns>
        public bool SkillEffectScope(Skill skill)
        {
            // If skill scope is for ally with 1 or more HP, and your own HP = 0,
            // or skill scope is for ally with 0, and your own HP = 1 or more
            return (((skill.Scope == 3 || skill.Scope == 4) && this.Hp == 0) ||
                    ((skill.Scope == 5 || skill.Scope == 6) && this.Hp >= 1));
        }

        /// <summary>
        /// Skill Effect : Effective Setup
        /// </summary>
        /// <param Name="skill">skill</param>
        /// <returns>true if common_event_id associated with skill exist</returns>
        public bool SkillEffectEffectiveSetup(Skill skill)
        {
            bool _effective = false;
            return _effective |= skill.CommonEventId > 0;
        }

        /// <summary>
        /// Skill Effect : First Hit Result
        /// </summary>
        /// <param Name="user">skill user</param>
        /// <param Name="skill">skill</param>
        /// <returns>Hit value</returns>
        public int SkillEffectFirstHitResult(GameBattler user, Skill skill)
        {
            int _hit_temp = skill.Hit;
            if (skill.AtkF > 0)
            {
                _hit_temp *= user.Hit / 100;
            }
            return _hit_temp;
        }

        /// <summary>
        /// Skill Effect : Skill Effective
        /// </summary>
        /// <param Name="effective">effective value</param>
        /// <param Name="Hit">Hit value</param>
        /// <returns>true if skill is effective</returns>
        public bool SkillEffectEffectiveCorrection(bool effective, int hit)
        {
            return effective |= hit < 100;
        }

        /// <summary>
        /// Skill Effect : Power
        /// </summary>
        /// <param Name="user">skill user</param>
        /// <param Name="skill">skill</param>
        /// <returns>power value</returns>
        public int SkillEffectPower(GameBattler user, Skill skill)
        {
            int _power = skill.Power + user.Atk * skill.AtkF / 100;
            if (_power > 0)
            {
                _power -= this.Pdef * skill.PdefF / 200;
                _power -= this.Mdef * skill.MdefF / 200;
                _power = Math.Max(_power, 0);
            }
            return _power;
        }

        /// <summary>
        /// Skill Effect : Rate
        /// </summary>
        /// <param Name="user">skill user</param>
        /// <param Name="skill">skill</param>
        /// <returns>rate value</returns>
        public int SkillEffectRate(GameBattler user, Skill skill)
        {
            // Calculate rate
            int _rate = 20;
            _rate += (user.Str * skill.StrF / 100);
            _rate += (user.Dex * skill.DexF / 100);
            _rate += (user.Agi * skill.AgiF / 100);
            _rate += (user.Intel * skill.IntF / 100);
            // Return Rate
            return _rate;
        }

        /// <summary>
        /// Skill Effect : Base Damage
        /// </summary>
        /// <param Name="power">power value</param>
        /// <param Name="rate">rate value</param>
        public void SkillEffectBaseDamage(int power, int rate)
        {
            this.DamageValue = power * rate / 20;
        }

        /// <summary>
        /// Skill Effect : Element Correction
        /// </summary>
        /// <param Name="skill">skill</param>
        public void SkillEffectElementCorrection(Skill skill)
        {
            this.DamageValue *= ElementsCorrect(skill.ElementSet);
            this.DamageValue /= 100;
        }

        /// <summary>
        /// Skill Effect : Guard Correction
        /// </summary>
        public void SkillEffectGuardCorrection()
        {
            if (this.IsGuarding)
            {
                this.DamageValue /= 2;
            }
        }

        /// <summary>
        /// Skill Effect : Disperation
        /// </summary>
        /// <param Name="skill"></param>
        public void SkillEffectDisperation(Skill skill)
        {
            if (skill.Variance > 0 && Math.Abs(this.DamageValue) > 0)
            {
                int amp = Math.Max(Math.Abs(this.DamageValue) * skill.Variance / 100, 1);
                this.DamageValue += InGame.Rnd.Next(amp + 1) + InGame.Rnd.Next(amp + 1) - amp;
            }
        }

        /// <summary>
        /// Skill Effect : Second Hit Detection
        /// </summary>
        /// <param Name="user">skill user</param>
        /// <param Name="skill">skill</param>
        /// <returns>Hit value</returns>
        public int SkillEffectSecondHitResult(GameBattler user, Skill skill)
        {
            Eva = 8 * this.Agi / user.Dex + this.Eva;
            int hit_temp = this.DamageValue < 0 ? 100 : 100 - Eva * skill.EvaF / 100;
            hit_temp = this.IsCantEvade ? 100 : Hit;
            return hit_temp;
        }

        /// <summary>
        /// Skill Effect : Physical Hit Result
        /// </summary>
        /// <param Name="skill">skill</param>
        /// <returns>true if physical attack has power other than 0</returns>
        public bool SkillEffectPhysicalHitResult(Skill skill)
        {
            // If physical attack has power other than 0
            if (skill.Power != 0 && skill.AtkF > 0)
            {
                // State Removed by Shock
                RemoveStatesShock();
                // Return True
                return true;
            }
            // Return False
            return false;
        }

        /// <summary>
        /// Skill Effect : Damage
        /// </summary>
        /// <returns>true if damage decreases HP</returns>
        public bool SkillEffectDamage()
        {
            // Substract damage from HP
            int _last_hp = this.Hp;
            this.Hp -= this.DamageValue;
            this.Damage = DamageValue.ToString();
            return (this.Hp != _last_hp);
        }

        /// <summary>
        /// Skill Effect : Power 0 Test
        /// </summary>
        /// <param Name="skill">skill</param>
        public void SkillEffectPower0(Skill skill)
        {
            // If power is 0
            if (skill.Power == 0)
            {
                // Set damage to an empty string
                this.Damage = "";
                // If state is unchanged
                if (!IsStateChanged)
                {
                    // Set damage to "Miss"
                    this.Damage = "Miss";
                }
            }
        }

        /// <summary>
        /// Skill Effect : Miss
        /// </summary>
        public void SkillEffectMiss()
        {
            this.Damage = "Miss";
        }

        /// <summary>
        /// Skill Effect : Damage Fix
        /// </summary>
        public void SkillEffectDamagefix()
        {
            if (!InGame.Temp.IsInBattle)
            {
                this.Damage = null;
            }
        }

        #endregion

        #region Item effects

        /// <summary>
        /// Application of Item Effects
        /// </summary>
        /// <param Name="item">used item</param>
        /// <returns>true if effective</returns>
        public bool ItemEffect(Item item)
        {
            // Item Effect Setup
            ItemEffectSetup();
            // Return False If Out of Scope
            if (ItemEffectScope(item))
            {
                return false;
            }
            // Setup Effective
            bool effective = ItemEffectEffectiveSetup(item);
            // First Hit detection
            bool hit_result = ItemEffectHitResult(item);
            // Set effective flag if skill is uncertain
            effective = ItemEffectEffectiveCorrection(effective, item);
            // If Hit occurs
            if (hit_result)
            {
                // Calculate amount of recovery {hp_recovery, sp_recovery}
                int[] recovery = ItemEffectRecovery(item);
                // Element correction
                recovery = ItemEffectElementCorrection(recovery[0], recovery[1], item);
                // If recovery code is negative (hp<0)
                if (recovery[0] < 0)
                {
                    // Guard correction
                    recovery[0] = ItemEffectGuardCorrection(recovery[0]);
                }
                // Dispersion
                recovery = ItemEffectDispersion(recovery[0], recovery[1], item);
                // Damage & Recovery
                effective = ItemEffectDamage(recovery[0], recovery[1], effective);
                // State change
                IsStateChanged = false;
                effective |= StatesPlus(item.PlusStateSet);
                effective |= StatesMinus(item.MinusStateSet);
                // If parameter value increase is effective
                if (item.ParameterType > 0 && item.ParameterPoints != 0)
                {
                    // Item Effect Parameter Points
                    ItemEffectParameterPoints(item);
                    // Set to effective flag
                    effective = true;
                }
                // Item Effect no recovery
                ItemEffectNoRecovery(item);
                // If miss occurs
            }
            else
            {
                // Item Effect miss
                ItemEffectMiss();
            }
            // Item Effect damage fix
            ItemEffectDamagefix();
            // End Method
            return effective;
        }

        /// <summary>
        /// Item Effect : Setup
        /// </summary>
        public void ItemEffectSetup()
        {
            this.IsCritical = false;
        }

        /// <summary>
        /// Item Effect : Scope
        /// </summary>
        /// <param Name="item">item</param>
        /// <returns>true if </returns>
        public bool ItemEffectScope(Item item)
        {
            return (((item.Scope == 3 || item.Scope == 4) && this.Hp == 0) ||
                    ((item.Scope == 5 || item.Scope == 6) && this.Hp >= 1));
        }

        /// <summary>
        /// Item Effect : Effective
        /// </summary>
        /// <param Name="item">item</param>
        /// <returns>true if no common event associated with the item</returns>
        public bool ItemEffectEffectiveSetup(Item item)
        {
            bool _effective = false;
            return _effective |= item.CommonEventId > 0;
        }

        /// <summary>
        /// Item Effect : Hit Result
        /// </summary>
        /// <param Name="item">item</param>
        /// <returns>true if Hit</returns>
        public bool ItemEffectHitResult(Item item)
        {
            return (InGame.Rnd.Next(100) < item.Hit);
        }

        /// <summary>
        /// Item Effect : Item Effective
        /// </summary>
        /// <param Name="effective">item effectiveness</param>
        /// <param Name="item">item</param>
        /// <returns>true if effective</returns>
        public bool ItemEffectEffectiveCorrection(bool effective, Item item)
        {
            return effective |= item.Hit < 100;
        }

        /// <summary>
        /// Item Effect : Recovery
        /// </summary>
        /// <param Name="item">item</param>
        public int[] ItemEffectRecovery(Item item)
        {
            int recover_hp = MaxHp * item.RecoverHpRate / 100 + item.RecoverHp;
            int recover_sp = MaxSp * item.RecoverSpRate / 100 + item.RecoverSp;
            if (recover_hp < 0)
            {
                recover_hp += this.Pdef * item.PdefF / 20;
                recover_hp += this.Mdef * item.MdefF / 20;
                recover_hp = Math.Min(recover_hp, 0);
            }
            int[] _table = { recover_hp, recover_sp };
            return _table;
        }

        /// <summary>
        /// Item Effect : Element Correction
        /// </summary>
        /// <param Name="recover_hp">HP recovered</param>
        /// <param Name="recover_sp">SP recovered</param>
        /// <param Name="item">item</param>
        public int[] ItemEffectElementCorrection(int recover_hp, int recover_sp, Item item)
        {
            // Element correction
            recover_hp *= ElementsCorrect(item.ElementSet);
            recover_hp /= 100;
            recover_sp *= ElementsCorrect(item.ElementSet);
            recover_sp /= 100;
            int[] _table = { recover_hp, recover_sp };
            return _table; ;
        }

        /// <summary>
        /// Item Effect : Guard Correction
        /// </summary>
        /// <param Name="recover_hp">HP recovered</param>
        public int ItemEffectGuardCorrection(int recover_hp)
        {
            return this.IsGuarding ? recover_hp /= 2 : recover_hp;
        }

        /// <summary>
        /// Item Effect : Disperation
        /// </summary>
        /// <param Name="recover_hp">HP recovered</param>
        /// <param Name="recover_sp">SP recovered</param>
        /// <param Name="item">item</param>
        /// <returns></returns>
        public int[] ItemEffectDispersion(int recover_hp, int recover_sp, Item item)
        {
            int amp;
            if (item.Variance > 0 && Math.Abs(recover_hp) > 0)
            {
                amp = Math.Max(Math.Abs(recover_hp) * item.Variance / 100, 1);
                recover_hp += InGame.Rnd.Next(amp + 1) + InGame.Rnd.Next(amp + 1) - amp;
            }
            if (item.Variance > 0 && Math.Abs(recover_sp) > 0)
            {
                amp = Math.Max(Math.Abs(recover_sp) * item.Variance / 100, 1);
                recover_sp += InGame.Rnd.Next(amp + 1) + InGame.Rnd.Next(amp + 1) - amp;
            }
            int[] _table = { recover_hp, recover_sp };
            return _table;
        }

        /// <summary>
        /// Item Effect : Damage
        /// </summary>
        /// <param Name="recover_hp">HP recovered</param>
        /// <param Name="recover_sp">SP recovered</param>
        /// <param Name="effective">true if effective</param>
        public bool ItemEffectDamage(int recover_hp, int recover_sp, bool effective)
        {
            // Set Damage
            this.Damage = (-recover_hp).ToString();
            // HP and SP recovery
            int _last_hp = this.Hp;
            int _last_sp = this.Sp;
            this.Hp += recover_hp;
            this.Sp += recover_sp;
            effective |= this.Hp != _last_hp;
            effective |= this.Sp != _last_sp;
            // Return Effectiveness
            return effective;
        }

        /// <summary>
        /// Item Effect : Parameter Points
        /// </summary>
        /// <param Name="item">item</param>
        public void ItemEffectParameterPoints(Item item)
        {
            // Branch by parameter
            switch (item.ParameterType)
            {
                case 1:  // Max HP
                    MaxhpPlus += item.ParameterPoints;//MaxHp += item.parameter_points;//
                    break;
                case 2:  // Max SP
                    MaxspPlus += item.ParameterPoints;
                    break;
                case 3:  // Strength
                    StrPlus += item.ParameterPoints;
                    break;
                case 4:  // Dexterity
                    DexPlus += item.ParameterPoints;
                    break;
                case 5:  // Agility
                    AgiPlus += item.ParameterPoints;
                    break;
                case 6:  // Intelligence
                    IntPlus += item.ParameterPoints;
                    break;
            }
        }

        /// <summary>
        /// Item Effect : No Recovery
        /// </summary>
        /// <param Name="item">item</param>
        public void ItemEffectNoRecovery(Item item)
        {
            // If HP recovery rate and recovery amount are 0
            if (item.RecoverHpRate == 0 && item.RecoverHp == 0)
            {
                // Set damage to empty string
                this.Damage = "";
                // If SP recovery rate / recovery amount are 0, and parameter increase
                // value is ineffective.
                if (item.RecoverSpRate == 0 && item.RecoverSp == 0 &&
                   (item.ParameterType == 0 || item.ParameterPoints == 0))
                {
                    // If state is unchanged
                    if (!IsStateChanged)
                    {
                        // Set damage to "Miss"
                        this.Damage = "Miss";
                    }
                }
            }
        }

        /// <summary>
        /// Item Effect : Miss
        /// </summary>
        public void ItemEffectMiss()
        {
            this.Damage = "Miss";
        }

        /// <summary>
        /// Item Effect : Damage Fix
        /// </summary>
        public void ItemEffectDamagefix()
        {
            if (!InGame.Temp.IsInBattle)
            {
                this.Damage = null;
            }
        }

        #endregion

        #region Slip damage

        /// <summary>
        /// Application of Slip Damage Effects
        /// </summary>
        /// <returns>true if effective</returns>
        public bool SlipDamageEffect()
        {
            // Set damage
            SlipDamageEffectBaseDamage();
            // Dispersion
            SlipDamageEffectDispersion();
            // Subtract damage from HP
            SlipDamageEffectDamage();
            // End Method
            return true;
        }

        /// <summary>
        /// Slip Damage Effects : Base Damage
        /// </summary>
        public void SlipDamageEffectBaseDamage()
        {
            this.DamageValue = this.MaxHp / 10;
        }

        /// <summary>
        /// Slip Damage Effects : Dispersion
        /// </summary>
        public void SlipDamageEffectDispersion()
        {
            // Dispersion
            if (Math.Abs(this.DamageValue) > 0)
            {
                int _amp = Math.Max(Math.Abs(this.DamageValue) * 15 / 100, 1);
                this.DamageValue += InGame.Rnd.Next(_amp + 1) + InGame.Rnd.Next(_amp + 1) - _amp;
            }
        }

        /// <summary>
        /// Slip Damage Effects : Damage
        /// </summary>
        public void SlipDamageEffectDamage()
        {
            this.Hp -= this.DamageValue;
            this.Damage = DamageValue.ToString();
        }

        #endregion

        #region Tools

        /// <summary>
        /// Check if class is the right type
        /// </summary>
        /// <param Name="type_name">class Name</param>
        /// <returns>true if type_name is the right class Name</returns>
        public bool IsA(string type_name)
        {
            return (this.GetType().Name == type_name);
        }

        #endregion

        #endregion
    }
}
