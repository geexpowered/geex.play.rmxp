using System;
using System.Collections.Generic;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Game
{
    ///<summary>This class handles the actor. It's used within the GameActors class InGame.Actors and refers
    ///to the GameParty class InGame.Party</summary>
    public partial class GameActor : GameBattler
    {
        #region Constants

        protected const int BATTLER_SCREEN_Y_COORDINATE = 464;      //Actor's Y-Coordinate on battle screen

        #endregion

        #region Variables
        /// <summary>
        /// Actor Id
        /// </summary>
        public int ActorId;

        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Character file Name
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// Character hue
        /// </summary>
        public int CharacterHue;

        /// <summary>
        /// class ID
        /// </summary>
        public int ClassId;

        /// <summary>
        /// weapon ID
        /// </summary>
        public int WeaponId;

        /// <summary>
        ///  shield ID
        /// </summary>
        public int ArmorShield;

        /// <summary>
        /// helmet ID
        /// </summary>
        public int ArmorHelmet;

        /// <summary>
        /// body armor ID
        /// </summary>
        public int ArmorBody;

        /// <summary>
        /// accessory ID
        /// </summary>
        public int ArmorAccessory;

        /// <summary>
        /// list of skill_id
        /// </summary>
        public List<int> Skills = new List<int>();

        /// <summary>
        /// XP list
        /// </summary>
        int[] expList =new int[101];

        #endregion

        #region Properties

        #region Xp & Levels

        /// <summary>
        /// Actor level
        /// </summary>
        public override int Level
        {
            get { return localLevel; }
            set
            {
                // Check up and down limits
                value = Math.Min(value, Data.Actors[ActorId].FinalLevel);
                value = Math.Max(value, 1);
                // Set local level
                localLevel = value;
                // Change EXP
                this.Exp = this.expList[localLevel];
            }
        }
        int localLevel;

        /// <summary>
        /// Get EXP String
        /// </summary>
        public string ExpString
        {
            get
            {
                return expList[Level + 1] > 0 ? Exp.ToString() : "-------";
            }
        }

        /// <summary>
        /// Get Next Level EXP String
        /// </summary>
        public string NextExpString
        {
            get
            {
                return expList[Level + 1] > 0 ? expList[Level + 1].ToString() : "-------";
            }
        }

        /// <summary>
        /// Get Until Next Level EXP String
        /// </summary>
        public string NextRestExpString
        {
            get
            {
                return expList[Level + 1] > 0 ?
                    (expList[Level + 1] - Exp).ToString() : "-------";
            }
        }

        #endregion

        #region Id & Index

        ///<summary>id property, handy for others classes</summary>
        public int Id
        {
            get { return ActorId; }
        }

        ///<summary>party index property</summary>
        public override int Index
        {
            get { return InGame.Party.Actors.IndexOf(this); }
        }

        #endregion

        #region States

        ///<summary>Get State Effectiveness</summary>
        public override List<short> StateRanks
        {
            get
            {
                return Data.Classes[ClassId].StateRanks;
            }
        }

        ///<summary>Get Normal Attack Element</summary> 
        public override List<short> ElementSet
        {
            get
            {
                Weapon _weapon = Data.Weapons[WeaponId];
                return _weapon != null ? _weapon.ElementSet : new List<short>();
            }
        }

        ///<summary>Get Normal Attack State Change (+)</summary> 
        public override List<short> PlusStateSet
        {
            get
            {
                Weapon _weapon = Data.Weapons[WeaponId];
                return _weapon != null ? _weapon.PlusStateSet : new List<short>();
            }
        }

        ///<summary>Get Normal Attack State Change (-)</summary> 
        public override List<short> MinusStateSet
        {
            get
            {
                Weapon _weapon = Data.Weapons[WeaponId];
                return _weapon != null ? _weapon.MinusStateSet : new List<short>();
            }
        }

        #endregion

        #region Statistics
        ///<summary>Get Basic Maximum HP</summary>
        public override int BaseMaxHp
        {
            get
            {
                return Data.Actors[ActorId].BaseParameters.MaxHpParameters[Level];
            }
        }

        ///<summary>Get Basic Maximum SP</summary>
        public override int BaseMaxSp
        {
            get
            {
                return Data.Actors[ActorId].BaseParameters.MaxSpParameters[Level];
            }
        }

        ///<summary>Get Basic Attack Power</summary>
        public override int BaseAtk
        {
            get
            {
                Weapon _weapon = Data.Weapons[WeaponId];
                return _weapon != null ? _weapon.Atk : 0;
            }
        }

        ///<summary>Get Basic Strength</summary>        
        public override int BaseStr
        {
            get
            {
                int _n = Data.Actors[ActorId].BaseParameters.StrenghtParameters[Level];
                Weapon weapon = Data.Weapons[WeaponId];
                Armor armor1 = Data.Armors[ArmorShield];
                Armor armor2 = Data.Armors[ArmorHelmet];
                Armor armor3 = Data.Armors[ArmorBody];
                Armor armor4 = Data.Armors[ArmorAccessory];
                _n += weapon != null ? weapon.StrPlus : 0;
                _n += armor1 != null ? armor1.StrPlus : 0;
                _n += armor2 != null ? armor2.StrPlus : 0;
                _n += armor3 != null ? armor3.StrPlus : 0;
                _n += armor4 != null ? armor4.StrPlus : 0;
                return Math.Min(Math.Max(_n, MIN_STAT), MAX_STAT);
            }
        }

        ///<summary>Get Basic Dexterity</summary>        
        public override int BaseDex
        {
            get
            {
                int _n = Data.Actors[ActorId].BaseParameters.DexterityParameters[Level];
                Weapon weapon = Data.Weapons[WeaponId];
                Armor armor1 = Data.Armors[ArmorShield];
                Armor armor2 = Data.Armors[ArmorHelmet];
                Armor armor3 = Data.Armors[ArmorBody];
                Armor armor4 = Data.Armors[ArmorAccessory];
                _n += weapon != null ? weapon.DexPlus : 0;
                _n += armor1 != null ? armor1.DexPlus : 0;
                _n += armor2 != null ? armor2.DexPlus : 0;
                _n += armor3 != null ? armor3.DexPlus : 0;
                _n += armor4 != null ? armor4.DexPlus : 0;
                return Math.Min(Math.Max(_n, MIN_STAT), MAX_STAT);
            }
        }

        ///<summary>Get Basic Agility</summary>        
        public override int BaseAgi
        {
            get
            {
                int _n = Data.Actors[ActorId].BaseParameters.AgilityParameters[Level];
                Weapon weapon = Data.Weapons[WeaponId];
                Armor armor1 = Data.Armors[ArmorShield];
                Armor armor2 = Data.Armors[ArmorHelmet];
                Armor armor3 = Data.Armors[ArmorBody];
                Armor armor4 = Data.Armors[ArmorAccessory];
                _n += weapon != null ? weapon.AgiPlus : 0;
                _n += armor1 != null ? armor1.AgiPlus : 0;
                _n += armor2 != null ? armor2.AgiPlus : 0;
                _n += armor3 != null ? armor3.AgiPlus : 0;
                _n += armor4 != null ? armor4.AgiPlus : 0;
                return Math.Min(Math.Max(_n, MIN_STAT), MAX_STAT);
            }
        }

        ///<summary>Get Basic Intelligence</summary>        
        public override int BaseInt
        {
            get
            {
                int _n = Data.Actors[ActorId].BaseParameters.IntelligenceParameters[Level];
                Weapon weapon = Data.Weapons[WeaponId];
                Armor armor1 = Data.Armors[ArmorShield];
                Armor armor2 = Data.Armors[ArmorHelmet];
                Armor armor3 = Data.Armors[ArmorBody];
                Armor armor4 = Data.Armors[ArmorAccessory];
                _n += weapon != null ? weapon.IntPlus : 0;
                _n += armor1 != null ? armor1.IntPlus : 0;
                _n += armor2 != null ? armor2.IntPlus : 0;
                _n += armor3 != null ? armor3.IntPlus : 0;
                _n += armor4 != null ? armor4.IntPlus : 0;
                return Math.Min(Math.Max(_n, MIN_STAT), MAX_STAT);
            }
        }

        ///<summary>Get Basic Physical Defense</summary>        
        public override int BasePdef
        {
            get
            {
                Weapon weapon = Data.Weapons[WeaponId];
                Armor armor1 = Data.Armors[ArmorShield];
                Armor armor2 = Data.Armors[ArmorHelmet];
                Armor armor3 = Data.Armors[ArmorBody];
                Armor armor4 = Data.Armors[ArmorAccessory];
                int pdef1 = weapon != null ? weapon.Pdef : 0;
                int pdef2 = armor1 != null ? armor1.Pdef : 0;
                int pdef3 = armor2 != null ? armor2.Pdef : 0;
                int pdef4 = armor3 != null ? armor3.Pdef : 0;
                int pdef5 = armor4 != null ? armor4.Pdef : 0;
                return pdef1 + pdef2 + pdef3 + pdef4 + pdef5;
            }
        }

        ///<summary>Get Basic Magical Defense</summary>        
        public override int BaseMdef
        {
            get
            {
                Weapon weapon = Data.Weapons[WeaponId];
                Armor armor1 = Data.Armors[ArmorShield];
                Armor armor2 = Data.Armors[ArmorHelmet];
                Armor armor3 = Data.Armors[ArmorBody];
                Armor armor4 = Data.Armors[ArmorAccessory];
                int mdef1 = weapon != null ? weapon.Mdef : 0;
                int mdef2 = armor1 != null ? armor1.Mdef : 0;
                int mdef3 = armor2 != null ? armor2.Mdef : 0;
                int mdef4 = armor3 != null ? armor3.Mdef : 0;
                int mdef5 = armor4 != null ? armor4.Mdef : 0;
                return mdef1 + mdef2 + mdef3 + mdef4 + mdef5;
            }
        }

        ///<summary>Get Basic Evasion Correction</summary>        
        public override int BaseEva
        {
            get
            {
                Armor armor1 = Data.Armors[ArmorShield];
                Armor armor2 = Data.Armors[ArmorHelmet];
                Armor armor3 = Data.Armors[ArmorBody];
                Armor armor4 = Data.Armors[ArmorAccessory];
                int eva1 = armor1 != null ? armor1.Eva : 0;
                int eva2 = armor2 != null ? armor2.Eva : 0;
                int eva3 = armor3 != null ? armor3.Eva : 0;
                int eva4 = armor4 != null ? armor4.Eva : 0;
                return eva1 + eva2 + eva3 + eva4;
            }
        }

        /// <summary>
        /// Actor EXP
        /// </summary>
        public int Exp
        {
            get { return localExp; }
            set
            {
                localExp = Math.Max(Math.Min(value, MAX_XP), 0);
                //Level up
                while (localExp >= this.expList[Level + 1] && this.expList[Level + 1] > 0)
                {
                    LevelUp();
                }
                //Level down
                while (localExp < this.expList[Level])
                {
                    LevelDown();
                }
                //Correction if exceeding current max HP and max SP
                Hp = Math.Min(Hp, MaxHp);
                Sp = Math.Min(Sp, this.MaxSp);
            }
        }
        int localExp;
        #endregion

        #region Animation

        /// <summary>
        /// Get Offensive Animation ID for Normal Attacks
        /// </summary>
        public override int Animation1Id
        {
            get
            {
                Weapon weapon = Data.Weapons[WeaponId];
                return weapon != null ? weapon.Animation1Id : 0;
            }
        }

        /// <summary>
        /// Get Target Animation ID for Normal Attacks
        /// </summary>
        public override int Animation2Id
        {
            get
            {
                Weapon weapon = Data.Weapons[WeaponId];
                return weapon != null ? weapon.Animation2Id : 0;
            }
        }

        #endregion

        #region Class


        /// <summary>
        /// Get Class localName
        /// </summary>
        public string ClassName
        {
            get
            {
                return Data.Classes[ClassId].Name;
            }
        }

        #endregion

        #region BattleScreenCoordinates

        ///<summary>Get Battle Screen X-Coordinate</summary>
        public override int ScreenX
        {
            get
            {
                // Return after calculating x-coordinate by order of members in party
                return this.Index * 160 + 80;//GeexEdit.GameWindowWidth / 2 - 160 * InGame.Party.Actors.Count / 2 + this.Index * 160 + 80;
            }
        }

        ///<summary>Get Battle Screen Y-Coordinate</summary>
        public override int ScreenY
        {
            get { return (464 * GeexEdit.GameWindowHeight/480); }
        }

        ///<summary>Get Battle Screen Z-Coordinate</summary>
        public override int ScreenZ
        {
            get
            {
                // Return after calculating z-coordinate by order of members in party
                //return 4 - this.Index;
                return 102;
            }
        }

        #endregion

        #endregion

        #region Initialize

        ///<summary>Initializes with the Battler constructor and launches setup(id)</summary>
        ///<param Name="id">Actor id</param>
        public GameActor(int id)
            : base()
        {
            Setup(id);
        }
        /// <summary>
        /// Empty constructor mandatory for game saving
        /// </summary>
        public GameActor()
        {
        }

        ///<summary>Setup GameActor</summary>
        ///<param Name="id">Actor id</param>
        public void Setup(int id)
        {
            //Variables setup
            Actor _actor = Data.Actors[id];
            ActorId = id;
            Name = _actor.Name;
            CharacterName = _actor.CharacterName;
            CharacterHue = _actor.CharacterHue;
            BattlerName = _actor.BattlerName;
            BattlerHue = _actor.BattlerHue;
            ClassId = _actor.ClassId;
            WeaponId = _actor.WeaponId;
            ArmorShield = _actor.Armor1Id;
            ArmorHelmet = _actor.Armor2Id;
            ArmorBody = _actor.Armor3Id;
            ArmorAccessory = _actor.Armor4Id;
            Level = _actor.InitialLevel;
            MakeExpList();
            Exp = expList[Level];
            Skills.Clear();
            Hp = MaxHp;
            Sp = MaxSp;            
            MaxhpPlus = 0;
            MaxspPlus = 0;
            StrPlus = 0;
            DexPlus = 0;
            AgiPlus = 0;
            IntPlus = 0;
            // Learnt skills
            for (int i = 1; i <= Level; i++)
            {
                for (int j = 0; j < Data.Classes[ClassId].Learnings.Count; j++)
                {
                    if (Data.Classes[ClassId].Learnings[j].Level == i)
                    {
                        LearnSkill(Data.Classes[ClassId].Learnings[j].SkillId);
                    }
                }
            }
            //Update auto state
            UpdateAutoState(null, Data.Armors[ArmorShield]);
            UpdateAutoState(null, Data.Armors[ArmorHelmet]);
            UpdateAutoState(null, Data.Armors[ArmorBody]);
            UpdateAutoState(null, Data.Armors[ArmorAccessory]);
        }

        #endregion

        #region Methods

        #region Xp

        /// <summary>
        /// Actor gains a level
        /// </summary>
        public void LevelUp()
        {
            Level += 1;
            //Learn skill
            for (int j = 0; j < Data.Classes[ClassId].Learnings.Count; j++)
            {
                if (Data.Classes[ClassId].Learnings[j].Level == Level)
                {
                    LearnSkill(Data.Classes[ClassId].Learnings[j].SkillId);
                }
            }
        }

        /// <summary>
        /// Actor loses a level
        /// </summary>
        public void LevelDown()
        {
            Level -= 1;
        }

        ///<summary>Calculate EXP</summary>
        public void MakeExpList()
        {
            Actor _actor = Data.Actors[ActorId];
            expList[1] = 0;
            double _pow_i = 2.4 + _actor.ExpInflation / 100.0;
            for (int i = 2; i < 100; i++)
            {
                if (i > _actor.FinalLevel)
                {
                    expList[i] = 0;
                }
                else
                {
                    double n = _actor.ExpBasis * Math.Pow(i + 3, _pow_i) / Math.Pow(5, _pow_i);
                    expList[i] = expList[i - 1] + (int)n;
                }
            }
        }

        #endregion

        #region State & Element

        ///<summary>Get Element Revision Value</summary>
        ///<param Name="element_id">Element ID</param>
        ///<returns>Modified element rate</returns>
        public override int ElementRate(short element_id)
        {
            //Get values corresponding to element effectiveness
            int[] _table = { 0, 200, 150, 100, 50, 0, -100 };
            int _result = _table[Data.Classes[ClassId].ElementRanks[element_id]];
            //If this element is protected by armor, then it's reduced by half
            Armor _armor;
            int[] _current_armors = { ArmorShield, ArmorHelmet, ArmorBody, ArmorAccessory };
            for (int i = 0; i < _current_armors.Length; i++)
            {
                _armor = Data.Armors[_current_armors[i]];
                if (_armor != null && _armor.GuardElementSet.Contains(element_id))
                {
                    _result /= 2;
                }
            }
            //If this element is protected by states, then it's reduced by half
            for (int i = 0; i < states.Count; i++)
            {
                if (Data.States[states[i]].GuardElementSet.Contains(element_id))
                {
                    _result /= 2;
                }
            }
            //End Method
            return _result;
        }

        ///<summary>Determine State Guard</summary>
        ///<param Name="state_id">state ID</param>
        ///<returns>True if actor is guarding</returns>
        public override bool IsStateGuard(short state_id)
        {
            int[] _armors = { ArmorShield, ArmorHelmet, ArmorBody, ArmorAccessory };
            Armor _armor;
            foreach (int i in _armors)
            {
                _armor = Data.Armors[i];
                if (_armor != null)
                {
                    if (_armor.GuardStateSet.Contains(state_id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        ///<summary>
        ///Update Auto State
        ///</summary>
        ///<param Name="old_armor">unequipped armor</param>
        ///<param Name="new_armor">equipped armor</param>
        public void UpdateAutoState(Armor old_armor, Armor new_armor)
        {
            // Forcefully remove unequipped armor's auto state
            if (old_armor != null && old_armor.AutoStateId != 0)
            {
                base.RemoveState(old_armor.AutoStateId, true);
            }
            // Forcefully add unequipped armor's auto state
            if (new_armor != null && new_armor.AutoStateId != 0)
            {
                base.AddState(new_armor.AutoStateId, true);
            }
        }
        #endregion

        #region Equipment

        /// <summary>
        /// Determine Fixed Equipment
        /// </summary>
        /// <param Name="equip_type">type of equipment</param>
        /// <returns>True if an equipment object of the selected type is equipped</returns>
        public bool IsEquipFix(int equip_type)
        {
            switch (equip_type)
            {
                case 0: // Weapon
                    return Data.Actors[ActorId].WeaponFix;
                case 1: // Shield
                    return Data.Actors[ActorId].Armor1Fix;
                case 2:// Head
                    return Data.Actors[ActorId].Armor2Fix;
                case 3: // Body
                    return Data.Actors[ActorId].Armor3Fix;
                case 4: // Accessory
                    return Data.Actors[ActorId].Armor4Fix;
                default:
                    return false;
            }
        }

        ///<summary>Change equipement</summary> 
        ///<param name="equip_type">type of equipment</param>
        ///<param name="id">weapon or armor ID (If 0, remove equipment)</param>
        public void Equip(int equip_type, int id)
        {
            switch (equip_type)
            {
                case 0: // Weapon
                    if (id == 0 || InGame.Party.WeaponNumber(id) > 0)
                    {
                        InGame.Party.GainWeapon(WeaponId, 1);
                        WeaponId = id;
                        InGame.Party.LoseWeapon(id, 1);
                    }
                    break;
                case 1: // Shield
                    if (id == 0 || InGame.Party.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[ArmorShield], Data.Armors[id]);
                        InGame.Party.GainArmor(ArmorShield, 1);
                        ArmorShield = id;
                        InGame.Party.LoseArmor(id, 1);
                    }
                    break;
                case 2: // Head
                    if (id == 0 || InGame.Party.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[ArmorHelmet], Data.Armors[id]);
                        InGame.Party.GainArmor(ArmorHelmet, 1);
                        ArmorHelmet = id;
                        InGame.Party.LoseArmor(id, 1);
                    }
                    break;
                case 3: // Body
                    if (id == 0 || InGame.Party.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[ArmorBody], Data.Armors[id]);
                        InGame.Party.GainArmor(ArmorBody, 1);
                        ArmorBody = id;
                        InGame.Party.LoseArmor(id, 1);
                    }
                    break;
                case 4: // Accessory
                    if (id == 0 || InGame.Party.ArmorNumber(id) > 0)
                    {
                        UpdateAutoState(Data.Armors[ArmorAccessory], Data.Armors[id]);
                        InGame.Party.GainArmor(ArmorAccessory, 1);
                        ArmorAccessory = id;
                        InGame.Party.LoseArmor(id, 1);
                    }
                    break;
                default:
                    break;//throw new Wrong_Equipment_Type_Exception("GameActor.Equip " + equip_type + " " + id);
            }
        }

        /// <summary>
        /// Determine if Carriable Equippable
        /// </summary>
        /// <param Name="item">Carriable item</param>
        /// <returns>True if carriable parameter is equippable</returns>
        public bool IsEquippable(Carriable item)
        {
            switch (item.GetType().Name.ToString())
            {
                case "Weapon":
                    return IsEquippable((Weapon)item);
                case "Armor":
                    return IsEquippable((Armor)item);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determine if Equippable
        /// </summary>
        /// <param Name="item">Weapon item</param>
        /// <returns>True if weapon parameter is equippable</returns>
        public bool IsEquippable(Weapon item)
        {
            // If included among equippable weapons in current class
            if (Data.Classes[ClassId].WeaponSet.Contains(item.Id))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determine if Equippable
        /// </summary>
        /// <param Name="item">Armor item</param>
        /// <returns>True if armor parameter is equippable</returns>
        public bool IsEquippable(Armor item)
        {
            // If included among equippable armor in current class
            if (Data.Classes[ClassId].ArmorSet.Contains(item.Id))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Skill
        ///<summary>Learn Skill</summary>
        ///<param name="skill_id">skill ID</param>
        public void LearnSkill(int skill_id)
        {
            if (skill_id > 0 && !IsSkillLearn(skill_id))
            {
                Skills.Add(skill_id);
                //Sort byusing the default comparer
                Skills.Sort();
            }
        }

        ///<summary>Forget Skill</summary>
        ///<param name="skill_id">skill ID</param>
        public void ForgetSkill(int skill_id)
        {
            Skills.Remove(skill_id);
            //InGame.Party.skills.delete(skill_id)
        }

        ///<summary>Determine if Finished Learning Skill</summary>
        ///<param Name="skill_id">skill ID</param>
        ///<returns>True if selected skill is learnt</returns>
        public bool IsSkillLearn(int skill_id)
        {
            return Skills.Contains(skill_id);
        }

        ///<summary>Determine if Skill can be Used</summary>
        ///<param Name="skill_id">skill ID</param>
        ///<returns>True if selected skill can be used</returns>
        public override bool IsSkillCanUse(int skill_id)
        {
            if (!IsSkillLearn(skill_id))
            {
                return false;
            }
            return base.IsSkillCanUse(skill_id);
        }

        #endregion

        #region Graphics

        ///<summary>Change Actor Graphics</summary>
        ///<param Name="character_name">new Character file Name</param>
        ///<param Name="character_hue">new Character hue</param>
        ///<param Name="battler_name">new Battler file Name</param>
        ///<param Name="battler_hue">new Battler hue</param>
        public void SetGraphic(string character_name, int character_hue, string battler_name, int battler_hue)
        {
            this.CharacterName = character_name;
            this.CharacterHue = character_hue;
            this.BattlerName = battler_name;
            this.BattlerHue = battler_hue;
        }

        #endregion

        #endregion

    }
}
