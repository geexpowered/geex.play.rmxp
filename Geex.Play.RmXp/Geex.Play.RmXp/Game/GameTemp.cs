using System;
using System.Collections.Generic;
using Geex.Play.Make;
using Geex.Run;
using Geex.Play.Rpg.Window;
using Geex.Play.Custom;

namespace Geex.Play.Rpg.Game
{
    ///<summary>This class handles temporary data that is not included with save data</summary>
    public partial class GameTemp
    {
        /// <summary>
        /// map music loop (for battle memory)
        /// </summary>
        public AudioFile MapSong;

        #region Message Window
        /// <summary>
        /// Reference to current Message Window
        /// </summary>
        public WindowMessage MessageWindow;
        /// <summary>
        /// message text
        /// </summary>
        public string MessageText = null;
        /// <summary>
        /// show choices: opening line
        /// </summary>
        public int ChoiceStart;
        /// <summary>
        /// show choices: number of items
        /// </summary>
        public int ChoiceMax;
        /// <summary>
        /// show choices: cancel
        /// </summary>
        public int ChoiceCancelType;
        /// <summary>
        /// show choices: callback (Proc)
        /// </summary>
        public Interpreter.ProcInt ChoiceProc = null;
        /// <summary>
        /// show choices : indent value
        /// </summary>
        public int ChoiceProcCurrentIndent;
        /// <summary>
        /// input number: opening line
        /// </summary>
        public int NumInputStart;
        /// <summary>
        /// input number: variable ID
        /// </summary>
        public int NumInputVariableId;
        /// <summary>
        /// input number: digit amount
        /// </summary>
        public int NumInputDigitsMax;
        /// <summary>
        /// message window showing
        /// </summary>
        public bool IsMessageWindowShowing = false;
        #endregion

        /// <summary>
        /// common event ID
        /// </summary>
        public int CommonEventId = 0;
        /// <summary>
        /// map event interpreter
        /// </summary>
        public Interpreter MapInterpreter = new Interpreter(0, true);

        /// <summary>
        /// battle event interpreter
        /// </summary>
        public Interpreter BattleInterpreter = new Interpreter(0, false);
        #region Battle
        /// <summary>
        /// in-battle flag
        /// </summary>
        public bool IsInBattle = false;
        /// <summary>
        /// battle calling flag
        /// </summary>
        public bool IsCallingBattle = false;
        /// <summary>
        /// battle troop ID
        /// </summary>
        public int BattleTroopId = 0;
        /// <summary>
        /// battle flag: escape possible
        /// </summary>
        public bool IsBattleCanEscape= false;
        /// <summary>
        /// battle flag: losing possible
        /// </summary>
        public bool IsBattleCanLose = false;
        /// <summary>
        /// battle callback (Proc)
        /// </summary>
        public Interpreter.ProcInt BattleProc = null;
        /// <summary>
        /// number of battle turns
        /// </summary>
        public int BattleTurn = 0;
        /// <summary>
        /// battle event flags: completed
        /// </summary>
        public Dictionary<int, bool> BattleEventFlags = new Dictionary<int, bool>();
        /// <summary>
        /// battle flag: interrupt
        /// </summary>
        public bool BattleAbort = false;
        /// <summary>
        /// battle flag: main phase
        /// </summary>
        public bool BattleMainPhase = false;
        /// <summary>
        /// Battleback file Name
        /// </summary>
        public string BattlebackName = "";
        #endregion
        /// <summary>
        /// Battler being forced into action
        /// </summary>
        public GameBattler ForcingBattler;
        /// <summary>
        /// shop calling flag
        /// </summary>
        public bool IsCallingShop = false;
        /// <summary>
        /// list of shop goods
        /// </summary>
        public List<int[]> ShopGoods = new List<int[]>();
        /// <summary>
        /// Shop ID
        /// </summary>
        public int ShopId = 00;
        /// <summary>
        /// Name input: calling flag
        /// </summary>
        public bool IsCallingName = false;
        /// <summary>
        /// Name input: actor ID
        /// </summary>
        public int NameActorId = 0;
        /// <summary>
        /// Name input: max Character count
        /// </summary>
        public int NameMaxChar = 0;
        /// <summary>
        /// menu calling flag
        /// </summary>
        public bool IsCallingMenu = false;
        /// <summary>
        /// save calling flag
        /// </summary>
        public bool IsCallingSave = false;
        /// <summary>
        /// debug calling flag
        /// </summary>
        public bool IsCallingDebug = false;
        /// <summary>
        /// player place movement flag
        /// </summary>
        public bool IsTransferringPlayer = false;
        /// <summary>
        /// player destination: map ID
        /// </summary>
        public int PlayerNewMapId = 0;
        /// <summary>
        /// player destination: x-coordinate
        /// </summary>
        public int PlayerNewX = 0;
        /// <summary>
        /// player destination: y-coordinate
        /// </summary>
        public int PlayerNewY = 0;
        /// <summary>
        /// player destination: direction
        /// </summary>
        public int PlayerNewDirection = 0;
        /// <summary>
        /// Transition processing flag
        /// </summary>
        public bool IsProcessingTransition = false;
        /// <summary>
        /// Transition file Name
        /// </summary>
        public string TransitionName = "";
        /// <summary>
        /// game over flag
        /// </summary>
        public bool IsGameover = false;
        /// <summary>
        /// return to title screen flag
        /// </summary>
        public bool ToTitle = false;
        /// <summary>
        /// last save file no.
        /// </summary>
        public int LastFileIndex = 0;
        /// <summary>
        /// menu: play sound Effect flag
        /// </summary>
        public bool IsMenuBeep=false;
        /// <summary>
        /// debug screen: for saving conditions
        /// </summary>
        public int DebugTopRow = 0;
        /// <summary>
        /// debug screen: for saving conditions
        /// </summary>
        public int DebugIndex = 0;
        /// <summary>
        /// Delegate to message_waiting = false
        /// </summary>
        public Interpreter.ProcEmpty MessageProc;

        /// <summary>
        /// Reference to the event that opens a Message Window
        /// </summary>
        public int MessageWindowEventID=0;
    }
}
