using System;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Utils;
using Geex.Run;
using Geex.Play.Custom;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// Manage Game Global variables
    /// </summary>
    public static class Data
    {
        /// <summary>
        /// Geex Edit datas for troops
        /// </summary>
        public static Troop[] Troops;
        /// <summary>
        /// Geex Edit datas for actors
        /// </summary>
        public static Actor[] Actors;
        /// <summary>
        /// Geex Edit datas for classes
        /// </summary>
        public static Class[] Classes;
        /// <summary>
        /// Geex Edit datas for skills
        /// </summary>
        public static Skill[] Skills;
        /// <summary>
        /// Geex Edit datas for items
        /// </summary>
        public static Item[] Items;
        /// <summary>
        /// Geex Edit datas for weapons
        /// </summary>
        public static Weapon[] Weapons;
        /// <summary>
        /// Geex Edit datas for armors
        /// </summary>
        public static Armor[] Armors;
        /// <summary>
        /// Geex Edit datas for Npcs
        /// </summary>
        public static Npc[] Npcs;
        /// <summary>
        /// Geex Edit datas for states
        /// </summary>
        public static State[] States;
        /// <summary>
        /// Geex Edit datas for animations
        /// </summary>
        public static Animation[] Animations;
        /// <summary>
        /// Geex Edit datas for Common events
        /// </summary>
        public static CommonEvent[] CommonEvents;
        /// <summary>
        /// Geex Edit datas for system
        /// </summary>
        public static SystemData System = new SystemData();
    }

}