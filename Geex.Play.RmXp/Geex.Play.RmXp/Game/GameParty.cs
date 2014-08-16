using System;
using System.Collections.Generic;
using Geex.Play.Make;
using Geex.Run;
using Microsoft.Xna.Framework;
using Geex.Play.Custom;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class represents the characters party.
    /// </summary>
    public partial class GameParty
    {
        #region Constants

        public const int MAX_GOLD = 9999999;
        public const int MAX_STEPS = 9999999;
        public const int MAX_ITEM_NUMBER = 99;

        #endregion

        #region Variables

        /// <summary>
        /// Party's actors
        /// </summary>
        public List<GameActor> Actors = new List<GameActor>();

        /// <summary>
        /// Party's gold
        /// </summary>
        public int Gold;

        /// <summary>
        /// Party's steps number
        /// </summary>
        public int Steps;

        /// <summary>
        /// Item / Number
        /// </summary>
        public GeexDictionary<int, int> Items = new GeexDictionary<int, int>();

        /// <summary>
        /// Weapon / Number
        /// </summary>
        public GeexDictionary<int, int> Weapons = new GeexDictionary<int, int>();

        /// <summary>
        /// Armor / Number
        /// </summary>
        public GeexDictionary<int, int> Armors = new GeexDictionary<int, int>();

        /// <summary>
        /// List of GameCharacter Tag
        /// </summary>
        public List<Tag> Tags = new List<Tag>();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the maximum level of the party's characters
        /// </summary>
        public int MaxLevel
        {
            get
            {
                //If no Character in the party, returns 0
                if (Actors.Count == 0)
                {
                    return 0;
                }
                //Max level calculation
                int _level = 0;
                for (int i=0;i<Actors.Count;i++)
                {
                    if (_level < Actors[i].Level)
                    {
                        _level = Actors[i].Level;
                    }
                }
                return _level;
            }
        }

        ///<summary>Returns true if an actor is inputable</summary>
        ///<returns>True if any actor of the party is inputable</returns>
        public bool IsInputable
        {
            get
            {
                for (int i = 0; i < Actors.Count; i++)
                {
                    if (Actors[i].IsInputable)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        ///<summary>Returns true if each member of a non-null party is dead</summary>
        ///<returns>True if every actor of the party is dead</returns>
        public bool IsAllDead
        {
            get
            {
                if (InGame.Party.Actors.Count == 0)
                {
                    return false;
                }
                for (int i = 0; i < Actors.Count; i++)
                {
                    if (Actors[i].Hp > 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion

        #region Initialize

        ///<summary>Constructor</summary>
        public GameParty()
        {
            Gold = 0;
            Steps = 0;
        }

        #endregion

        #region Methods

        ///<summary>Defines starting party</summary>
        public void SetupStartingMembers()
        {
            Actors.Clear();
            for (int i=0;i<Data.System.PartyMembers.Length;i++)
            {
                //Add actors with the index from the party_members list
                Actors.Add(InGame.Actors[Data.System.PartyMembers[i]-1]);
            }
        }

        ///<summary>Refresh the actors list</summary>
        public void Refresh()
        {
            List<GameActor> _new_actors = new List<GameActor>();
            for (int i = 0; i < Actors.Count; i++)
            {
                if (Data.Actors[Actors[i].Id] != null)
                {
                    _new_actors.Add(InGame.Actors[Actors[i].Id]);
                }
            }
            Actors = _new_actors;
        }

        ///<summary>Adds an actor to the party</summary>
        ///<param Name="actor_id">actor id in database</param>
        public void AddActor(int actor_id)
        {
            GameActor _actor = InGame.Actors[actor_id];
            //Add the actor if it isn't in the party, 
            if (Actors.Count < 4 && !Actors.Contains(_actor)) //TEST : utilisation de contains, l'objet est-il le bon ?
            {
                Actors.Add(_actor);
                InGame.Player.Refresh();
            }
        }

        ///<summary>Removes an actor from the party</summary>
        ///<param Name="actor_id">actor id in database</param>
        public void RemoveActor(int actor_id)
        {
            Actors.Remove(InGame.Actors[actor_id]);
            InGame.Player.Refresh();
        }

        ///<summary>Substracts n to gold</summary>
        ///<param Name="n">number of gold pieces</param>
        public void LoseGold(int n)
        {
            GainGold((-1) * n);
        }

        ///<summary>Adds one step</summary>
        public void IncreaseSteps()
        {
            //Add one step, except if it exceeds Max_Steps
            Steps = Math.Min(Steps + 1, MAX_STEPS);
        }

        ///<summary>Returns the number of a specific item</summary>
        ///<param Name="item_id">item id</param>
        ///<returns>Number of specified item</returns>
        public int ItemNumber(int item_id)
        {
            //Returns either the number of the specified item, or 0
            return Items.ContainsKey(item_id) ? Items[item_id] : 0;
        }

        ///<summary>Returns the number of a specific weapon</summary>
        ///<param Name="weapon_id">weapon id</param>
        ///<returns>Number of specified weapon</returns>
        public int WeaponNumber(int weapon_id)
        {
            //Returns either the number of the specified weapon, or 0
            return Weapons.ContainsKey(weapon_id) ? Weapons[weapon_id] : 0;
        }

        ///<summary>Returns the number of a specific armor</summary>
        ///<param Name="armor_id">armor id</param>
        ///<returns>Number of specified armor</returns>
        public int ArmorNumber(int armor_id)
        {
            //Returns either the number of the specified armor, or 0
            return Armors.ContainsKey(armor_id) ? Armors[armor_id] : 0;
        }

        ///<summary>Adds n to gold</summary>
        ///<param name="n">number of gold pieces</param>
        public void GainGold(int n)
        {
            //Add n to gold, except if it exceeds Max_Gold
            Gold = Math.Min(Math.Max(Gold + n, 0), MAX_GOLD);
        }

        ///<summary>Adds n specific items in the inventory</summary>
        ///<param name="item_id">item id</param>
        ///<param name="n">number of items</param>
        public void GainItem(int item_id, int n)
        {
            if (item_id > 0)
            {
                //Add n items if their number does not exceed Max_Item_Number
                Items[item_id] = Math.Min(Math.Max(ItemNumber(item_id) + n, 0), MAX_ITEM_NUMBER);
            }
        }

        ///<summary>Adds n specific weapons in the inventory</summary>
        ///<param name="weapon_id">weapon id</param>
        ///<param name="n">number of weapons</param>
        public void GainWeapon(int weapon_id, int n)
        {
            if (weapon_id > 0)
            {
                //Add n weapons if their number does not exceed Max_Item_Number
                Weapons[weapon_id] = Math.Min(Math.Max(WeaponNumber(weapon_id) + n, 0), MAX_ITEM_NUMBER);
            }
        }

        ///<summary>Adds n specific armors in the inventory</summary>
        ///<param name="armor_id">armor id</param>
        ///<param name="n">number of armors</param>
        public void GainArmor(int armor_id, int n)
        {
            if (armor_id > 0)
            {
                //Add n armors if their number does not exceed Max_Item_Number
                Armors[armor_id] = Math.Min(Math.Max(ArmorNumber(armor_id) + n, 0), MAX_ITEM_NUMBER);
            }
        }

        ///<summary>Discards n specific items from the inventory</summary>
        ///<param Name="item_id">item id</param>
        ///<param Name="n">number of items</param>
        public void LoseItem(int item_id, int n)
        {
            GainItem(item_id, (-1) * n);
        }

        ///<summary>Discards n specific weapons from the inventory</summary>
        ///<param Name="weapon_id">weapon id</param>
        ///<param Name="n">number of weapons</param>
        public void LoseWeapon(int weapon_id, int n)
        {
            GainWeapon(weapon_id, (-1) * n);
        }

        ///<summary>Discards n specific armors from the inventory</summary>
        ///<param Name="armor_id">armor id</param>
        ///<param Name="n">number of armors</param>
        public void LoseArmor(int armor_id, int n)
        {
            GainArmor(armor_id, (-1) * n);
        }

        ///<summary>Whether the party can use a specific item or not</summary>
        ///<param Name="item_id"item id</param>
        ///<returns>True if the party can use the specified item</returns>
        public bool IsItemCanUse(int item_id)
        {
            //Return false if no item
            if (ItemNumber(item_id) == 0)
            {
                return false;
            }
            //Gets the use occasions from datas
            int _occasion = Data.Items[item_id].Occasion;
            //If in battle
            if (InGame.Temp.IsInBattle)
            {
                return (_occasion == 0 || _occasion == 1);
            }
            //Else, the party is on the map :
            return (_occasion == 0 || _occasion == 2);
        }

        ///<summary>Clears the party members current action</summary>
        public void ClearActions()
        {
            for (int i=0;i<Actors.Count;i++)
            {
                Actors[i].CurrentAction.Clear();
            }
        }

        ///<summary>Inflicts slip damages, flashes the screen, kill actors and possibly sets Gameover off</summary>
        public void CheckMapSlipDamage()
        {
            for (int i = 0; i < Actors.Count; i++)
            {
                if (Actors[i].Hp > 0 && Actors[i].IsSlipDamage)
                {
                    Actors[i].Hp -= Math.Max(Actors[i].MaxHp / 100, 1);
                    if (Actors[i].Hp == 0)
                    {
                        Audio.SoundEffectPlay(Data.System.ActorCollapseSoundEffect);
                    }
                    InGame.Screen.StartFlash(new Color(255, 0, 0, 128), 4);
                    InGame.Temp.IsGameover = InGame.Party.IsAllDead;
                }
            }
        }

        ///<summary>Sets a random actor of the party as a target</summary>
        ///<param Name="hp0">hp0 is true if actor's hp==0</param>
        ///<returns>Target actor</returns>
        public GameActor RandomTargetActor(bool hp0)
        {
            List<GameActor> roulette = new List<GameActor>();
            //Calculate chances to be Hit for each actor,
            for (int i = 0; i < Actors.Count; i++)
            {
                if ((!hp0 && Actors[i].IsExist) || (hp0 && Actors[i].IsHp0))
                {
                    //Gets the classe's position : front(0), middle(1), back(2)
                    int _position = Data.Classes[Actors[i].ClassId].Position;
                    //n value will be 4(front position), 3(middle position) or 2(back position)
                    int _n = 4 - _position;
                    //Front position classes have more chances to be Hit than back position classes
                    for (int j = 0; j < _n; j++)
                    {
                        roulette.Add(Actors[i]);
                    }
                }
            }
            if (roulette.Count == 0)
            {
                return null;
            }
            return roulette[InGame.Rnd.Next(roulette.Count)];
        }

        ///<summary>Sets a random actor of the party as a target (default : hp0 == false)</summary>
        ///<returns>Target actor</returns>
        public GameActor RandomTargetActor() //default : hp0 == false
        {
            // No default parameter in C#, let's be crafty
            return RandomTargetActor(false);
        }

        ///<summary>Sets a random actor of the party as a target, whom hp == 0</summary>
        ///<returns>Target actor</returns>
        public GameActor RandomTargetActorHp0()
        {
            return RandomTargetActor(true);
        }

        ///<summary>Select a specific actor from the party if it is valid, if not a random actor</summary>
        ///<param Name="actor_index">actor index</param>
        ///<returns>Target actor</returns>
        public GameActor SmoothTargetActor(int actor_index)
        {
            //Actor selection
            GameActor _actor = Actors[actor_index];
            //If this actor is a valid target, returns it
            if (_actor != null && _actor.IsExist)
            {
                return _actor;
            }

            //If not, take any valid target
            _actor = null;
            //_actor is nullified, to return null if there is no valid target
            //Note : normally, if there is no valid target, there is no more fight. TODO : ckeck it.
            for (int i = 0; i < Actors.Count; i++)
            {
                if (Actors[i].IsExist)
                {
                    _actor = Actors[i];
                }
            }
            return _actor;
        }

        #endregion
    }
}
