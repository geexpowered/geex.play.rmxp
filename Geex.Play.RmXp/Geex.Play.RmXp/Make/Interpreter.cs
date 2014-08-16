using System.Collections.Generic;
using Geex.Edit;
using Geex.Play.Rpg;
using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        #region Structures
        /// <summary>
        /// Condition and value for branch condition
        /// </summary>
        struct Branch
        {
            public int Val;
            public bool Result;
            bool isDeleted;
            /// <summary>
            /// True if branch is deleted
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    return isDeleted;
                }
            }
            public Branch(int v, bool b)
            {
                Val = v;
                Result = b;
                isDeleted = false;
            }

            /// <summary>
            /// Empty the Branch
            /// </summary>
            public void Empty()
            {
                Val = 0;
                Result = false;
                isDeleted = true;
            }

            public void Reset()
            {
                Val = 0;
                Result = false;
                isDeleted = false;
            }
        }
        #endregion

        #region Variables
        /// <summary>
        /// nest depth
        /// </summary>
        int depth;
        /// <summary>
        /// main flag
        /// </summary>
        bool main;
        /// <summary>
        /// map ID when starting up
        /// </summary>
        int mapId;
        /// <summary>
        /// event ID
        /// </summary>
        int eventId;
        /// <summary>
        /// waiting for message to end
        /// </summary>
        bool isWaitingMessage;
        /// <summary>
        /// waiting for move completion
        /// </summary>
        bool isWaitingMoveRoute;
        /// <summary>
        /// button input variable ID
        /// </summary>
        int buttonInputVariableId;
        /// <summary>
        /// wait count
        /// </summary>
        int waitCount;
        /// <summary>
        /// child interpreter
        /// </summary>
        Interpreter childInterpreter;
        /// <summary>
        /// branch data
        /// </summary>
        Branch[] branch = new Branch[GeexEdit.MaxNumberfOfBranch];
        //Dictionary<int, Branch> branch = new Dictionary<int, Branch>(); 
        /// <summary>
        /// list of event commands
        /// </summary>
        EventCommand[] list;

        /// <summary>
        /// Interpreter index
        /// </summary>
        int index;
        /// <summary>
        /// Count Interpreter loop
        /// </summary>
        int loopCount;
        /// <summary>
        /// Class variable that store list[index].intParams from execute_command
        /// </summary>
        short[] intParams;
        /// <summary>
        /// Class variable that store list[index].stringParams from execute_command
        /// </summary>
        string[] stringParams;
        #endregion

        #region Properties
        /// <summary>
        /// Determine if Running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return list != null;
            }
        }
        #endregion

        #region Delegates
        /// <summary>
        /// Delegate to a method(int n)
        /// </summary>
        /// <param Name="n">Value of int</param>
        public delegate void ProcInt(int n);

        /// <summary>
        /// Delegate to a method()
        /// </summary>
        public delegate void ProcEmpty();
        #endregion

        #region Constructors
        /// <summary>
        /// Runs event commands
        /// </summary>
        /// <param Name="_depth">nest depth</param>
        /// <param Name="_main">main flag</param>
        public Interpreter(int _depth, bool _main)
        {
            depth = _depth;
            main = _main;
            // Depth goes up to level 100
            if (_depth > 100) return;
            // Clear inner situation of interpreter
            Clear();
        }
        public Interpreter() : this(0, false) { }
        public Interpreter(int _depth) : this(_depth, false) { }
        #endregion

        #region Update
        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            // Initialize loop count
            loopCount = 0;
            // Loop
            do
            {
                // Add 1 to loop count
                loopCount += 1;
                // If 100 event commands ran
                if (loopCount > 100)
                {
                    loopCount = 0;
                    break;
                }
                // If map is different than event startup time
                if (InGame.Map.MapId != mapId)
                {
                    // Change event ID to 0
                    eventId = 0;
                }
                // If a child interpreter exists
                if (childInterpreter != null)
                {
                    // Update child interpreter
                    childInterpreter.Update();
                    // If child interpreter is finished running
                    if (!childInterpreter.IsRunning)
                    {
                        // Delete child interpreter
                        childInterpreter = null;
                    }
                    // If child interpreter still exists
                    if (childInterpreter != null) return;
                }
                // If waiting for message to end
                if (isWaitingMessage) return;
                // If waiting for move to end
                if (isWaitingMoveRoute)
                {
                    // If player is forcing move route
                    if (InGame.Player.MoveRouteForcing) return;
                    // Loop (map events)
                    foreach (GameEvent ev in InGame.Map.Events)
                    {
                        // If this event is forcing move route
                        if (ev!=null && ev.MoveRouteForcing) 
                            return;
                    }
                    // Clear move end waiting flag
                    isWaitingMoveRoute = false;
                }
                // If waiting for button input
                if (buttonInputVariableId > 0)
                {
                    // Run button input processing
                    InputButton();
                    return;
                }
                // If waiting
                if (waitCount > 0)
                {
                    waitCount -= 1;
                    return;
                }
                // If an action forcing Battler exists
                if (InGame.Temp.ForcingBattler != null) return;
                // If a call flag is set for each type of screen
                if (InGame.Temp.IsCallingBattle ||
                    InGame.Temp.IsCallingShop ||
                    InGame.Temp.IsCallingName ||
                    InGame.Temp.IsCallingSave ||
                    InGame.Temp.IsCallingName ||
                    InGame.Temp.IsGameover) return;
                // If list of event commands is empty
                if (list == null)
                {
                    // If main map event
                    if (main)
                    {
                        SetupStartingEvent();
                    }
                    // If nothing was set up
                    if (list==null) return;
                }
                // If return value is false when trying to execute event command
                if (!ExecuteCommand()) return;
                // Advance index
                index += 1;
            } while (true);
        }
        #endregion

        #region Execute commands
        /// <summary>
        /// Event Command Execution
        /// </summary>
        bool ExecuteCommand()
        {
            if (index >= list.Length)
            {
                // End event
                CommandEnd();
                return true;
            }
            // If last to arrive for list of event commands
            if (list[index] == null)
            {
                // End event
                CommandEnd();
                return true;
            }
            // Make event command parameters available for reference via parameters
            intParams = list[index].IntParams;
            stringParams = list[index].StringParams;
            // Branch by command code
            switch (list[index].Code)
            {
                #region Command - Tab 1
                case 101:  // Show Text
                    return Command101();
                case 102:  // Show Choices
                    return Command102();
                case 402:  // When [**]
                    return Command402();
                case 403:  // When Cancel
                    return Command403();
                case 103:  // Input Number
                    return Command103();
                case 104:  // Change Text Options
                    return Command104();
                case 105:  // Button Input Processing
                    return Command105();
                case 106:  // Wait
                    return Command106();
                case 111:  // Conditional Branch
                    return Command111();
                case 411:  // Else
                    return Command411();
                case 112:  // Loop
                    return Command112();
                case 413:  // Repeat Above
                    return Command413();
                case 113:  // Break Loop
                    return Command113();
                case 115:  // Exit Event Processing
                    return Command115();
                case 116:  // Erase Event
                    return Command116();
                case 117:  // Call Common Event
                    return Command117();
                case 118:  // Label
                    return Command118();
                case 119:  // Jump to Label
                    return Command119();
                case 121:  // Control Switches
                    return Command121();
                case 122:  // Control Variables
                    return Command122();
                case 123:  // Control Self Switch
                    return Command123();
                case 124:  // Control Timer
                    return Command124();
                case 125:  // Change Gold
                    return Command125();
                case 126:  // Change Items
                    return Command126();
                case 127:  // Change Weapons
                    return Command127();
                case 128:  // Change Armor
                    return Command128();
                case 129:  // Change Party Member
                    return Command129();
                case 131:  // Change Windowskin
                    return Command131();
                case 132:  // Change Battle BGM
                    return Command132();
                case 133:  // Change Battle End ME
                    return Command133();
                case 134:  // Change Save Access
                    return Command134();
                case 135:  // Change Menu Access
                    return Command135();
                case 136:  // Change Encounter
                    return Command136();
                #endregion
                #region Command - Tab 2
                case 201:  // Transfer Player
                    return Command201();
                case 202:  // Set Event Location
                    return Command202();
                case 203:  // Scroll Map
                    return Command203();
                case 204:  // Change Map Settings
                    return Command204();
                case 205: // Change Fog Color Tone
                    return Command205();
                case 206:  // Change Fog Opacity
                    return Command206();
                case 207:  // Show Animation
                    return Command207();
                case 208:  // Change Transparent Flag
                    return Command208();
                case 209:  // Set Move Route
                    return Command209();
                case 210:  // Wait for Move's Completion
                    return Command210();
                case 221:  // Prepare for Transition
                    return Command221();
                case 222:  // Execute Transition
                    return Command222();
                case 223:  // Change Screen Color Tone
                    return Command223();
                case 224:  // Screen Flash
                    return Command224();
                case 225:  // Screen Shake
                    return Command225();
                case 231:  // Show Picture
                    return Command231();
                case 232:  // Move Picture
                    return Command232();
                case 233:  // Rotate Picture
                    return Command233();
                case 234:  // Change Picture Color Tone
                    return Command234();
                case 235:  // Erase Picture
                    return Command235();
                case 236:  // Set Weather Effects
                    return Command236();
                case 241:  // Play BGM
                    return Command241();
                case 242:  // Fade Out BGM
                    return Command242();
                case 245:  // Play BGS
                    return Command245();
                case 246:  // Fade Out BGS
                    return Command246();
                case 247:  // Memorize BGM/BGS
                    return Command247();
                case 248:  // Restore BGM/BGS
                    return Command248();
                case 249:  // Play ME
                    return Command249();
                case 250:  // Play SE
                    return Command250();
                case 251:  // Stop SE
                    return Command251();
                #endregion
                #region Command - Tab 3
                case 301:  // Battle Processing
                    return Command301();
                case 601:  // If Win
                    return Command601();
                case 602:  // If Escape
                    return Command602();
                case 603:  // If Lose
                    return Command603();
                case 302:  // Shop Processing
                    return Command302();
                case 303:  // localName Input Processing
                    return Command303();
                case 311:  // Change HP
                    return Command311();
                case 312:  // Change SP
                    return Command312();
                case 313:  // Change State
                    return Command313();
                case 314:  // Recover All
                    return Command314();
                case 315:  // Change EXP
                    return Command315();
                case 316:  // Change Level
                    return Command316();
                case 317:  // Change Parameters
                    return Command317();
                case 318:  // Change Skills
                    return Command318();
                case 319:  // Change Equipment
                    return Command319();
                case 320:  // Change Actor localName
                    return Command320();
                case 321:  // Change Actor Class
                    return Command321();
                case 322:  // Change Actor Graphic
                    return Command322();
                case 331:  // Change Enemy HP
                    return Command331();
                case 332:  // Change Enemy SP
                    return Command332();
                case 333:  // Change Enemy State
                    return Command333();
                case 334:  // Enemy Recover All
                    return Command334();
                case 335:  // Enemy Appearance
                    return Command335();
                case 336:  // Enemy Transform
                    return Command336();
                case 337:  // Show Battle Animation
                    return Command337();
                case 338:  // Deal Damage
                    return Command338();
                case 339:  // Force Action
                    return Command339();
                case 340:  // Abort Battle
                    return Command340();
                case 351:  // Call Menu Screen
                    return Command351();
                case 352:  // Call Save Screen
                    return Command352();
                case 353:  // Game Over
                    return Command353();
                case 354:  // Return to Title Screen
                    return Command354();
                case 355: // Script Geex Make
                    return SetupMakeCommand();
                case 0:   // Command End
                    CommandEnd();
                    return false;
                #endregion
                default:    // Other including 256 (eloption) and 357 (elobject)
                    return true;
            }
        }
        #endregion

        #region Tools
        /// <summary>
        /// EL Script, structure is :
        /// stringParams[0] = command Name
        /// stringParams[1] = command type (command,option,object)
        /// stringParam[2..n] = command and parameters
        /// </summary>
        bool SetupMakeCommand()
        {
            // Init Geex Make Command
            MakeCommand.Initialize(stringParams);
            MakeCommand.MapId = mapId;
            MakeCommand.EventId = eventId;
            // Do not read option or object during game running
            // if (MakeCommand.type != MakeCommandType.Command) return true;
            // jump to command Name
            MakeCommand.Start();
            return true;
        }
        /// <summary>
        /// Clear
        /// </summary>
        void Clear()
        {
            mapId = 0;
            eventId = 0;
            isWaitingMessage = false;
            isWaitingMoveRoute = false;
            buttonInputVariableId = 0;
            waitCount = 0;
            childInterpreter = null;
            branch = new Branch[GeexEdit.MaxNumberfOfBranch];
        }

        /// <summary>
        /// Event Setup
        /// </summary>
        /// <param Name="_list">list of event commands</param>
        /// <param Name="_event_id">event ID</param>
        public void Setup(EventCommand[] _list, int _event_id)
        {
            // Clear inner situation of interpreter
            Clear();
            // Remember map ID
            mapId = InGame.Map.MapId;
            // Remember event ID
            eventId = _event_id;
            // Remember list of event commands
            list = _list;
            // Initialize index
            index = 0;
        }

        /// <summary>
        /// Position the cursor at beginning of Interpreter
        /// </summary>
        public void Reset(EventCommand[] _list)
        {
            for(int i=0; i<branch.Length; i++)
            {
                branch[i].Reset();
            }
            list = _list;
            index = 0;
        }

        /// <summary>
        /// Starting Event Setup
        /// </summary>
        public void SetupStartingEvent()
        {
            // Refresh map if necessary
            if (InGame.Map.IsNeedRefresh)
                InGame.Map.Refresh();
            // If common event call is reserved
            if (InGame.Temp.CommonEventId > 0)
            {
                // Set up event
                Setup(Data.CommonEvents[InGame.Temp.CommonEventId].List, 0);
                // Release reservation
                InGame.Temp.CommonEventId = 0;
                return;
            }
            // Loop (map events)
            foreach (short i in InGame.Map.EventKeysToUpdate)
            {
                // If running event is found
                if (InGame.Map.Events[i] != null && InGame.Map.Events[i].IsStarting && !InGame.Map.Events[i].IsErased)
                {
                    // If not auto run
                    if (InGame.Map.Events[i].Trigger < 3)
                    {
                        // Clear starting flag
                        InGame.Map.Events[i].ClearStarting();
                        // Lock
                        InGame.Map.Events[i].ToLock();
                    }
                    // Set up event
                    Setup(InGame.Map.Events[i].List(), (InGame.Map.Events[i].Id));
                    return;
                }
            }
            // Loop (common events)
            foreach (CommonEvent common_event in Data.CommonEvents)
            {
                // If trigger is auto run, and condition switch is ON
                if (common_event.Trigger == 1 && InGame.Switches.Arr[common_event.SwitchId] == true)
                {
                    // Set up event
                    Setup(common_event.List, 0);
                    return;
                }
            }
        }

        /// <summary>
        /// Assign the triggered key to Game Variables
        /// </summary>
        void InputButton()
        {
            // Determine pressed button
            int n = 0;
            for (int i = 2; i <= 18; i++)
            {
                if (Input.IsTriggered(i)) n = i;
            }
            // If button was pressed
            if (n > 0)
            {
                // Change value of variables
                InGame.Variables.Arr[buttonInputVariableId] = n;
                InGame.Map.IsNeedRefresh = true;
                // End button input
                buttonInputVariableId = 0;
            }
        }
        // * 

        /// <summary>
        /// Setup Choices
        /// </summary>
        /// <param Name="parameters">[0]=Choice Cancel type, [1]=choice 1, [2]=choice 2 etc</param>
        void SetupChoices(int cancelType, string[] choicesList)
        {
            // Set choice item count to choice_max
            InGame.Temp.ChoiceMax = choicesList.Length;
            // Set choice to message_text
            for (int i = 0; i < choicesList.Length; i++)
            {
                InGame.Temp.MessageText += choicesList[i] + "\n";
            }
            // Set cancel processing
            InGame.Temp.ChoiceCancelType = cancelType;
            // Set callback
            InGame.Temp.ChoiceProcCurrentIndent = list[index].Indent;
            InGame.Temp.ChoiceProc = new ProcInt(ProcAssignBranch);
        }

        /// <summary>
        /// Actor Iterator (consider all party members)
        /// </summary>
        /// <param Name="parameter">if 1 or more, ID; if 0, all</param>
        List<GameActor> IterateActor(int parameter)
        {
            // If entire party
            if (parameter == 0)
            {
                // Loop for entire party
                return InGame.Party.Actors;
            }
            // If single actor
            else
            {
                // Get actor
                GameActor actor = InGame.Actors[parameter-1];
                // Evaluate block
                if (actor != null) return new List<GameActor>() { actor };
            }
            // Empty
            return new List<GameActor>();
        }

        /// <summary>
        /// Enemy Iterator (consider all troop members)
        /// </summary>
        /// <param Name="parameter">If 0 or above, index; if -1, all</param>
        List<GameNpc> IterateEnemy(int parameter)
        {
            // If entire troop
            if (parameter == -1)
            {
                return InGame.Troops.Npcs;
            }
            // If single enemy
            else
            {
                // Get enemy
                GameNpc enemy = InGame.Troops.Npcs[parameter];
                // Evaluate block
                if (enemy != null) return new List<GameNpc>() { enemy };
            }
            return new List<GameNpc>();
        }

        // * Battler Iterator (consider entire troop and entire party)
        //     parameter1 : If 0, enemy; if 1, actor
        //     parameter2 : If 0 or above, index; if -1, all

        public List<GameActor> IterateActor(int parameter1, int parameter2)
        {
            List<GameActor> list = new List<GameActor>();
            // If enemy
            if (parameter1 == 0)
            {
                return null;
            }
            // If actor
            else
            {
                // If entire party
                if (parameter2 == -1)
                {
                    // Loop for entire party
                    return InGame.Party.Actors;
                }
                // If single actor (N exposed)
                else
                {
                    // Get actor
                    list.Add(InGame.Party.Actors[parameter2]);
                }
            }
            return list;
        }

        /// <summary>
        /// Get GameEvent)
        /// </summary>
        /// <param Name="parameter">parameter: -1 GamePlayer, 0 self, else GameEvent(parameter)</param>
        GameCharacter GetCharacter(int parameter)
        {
            // Branch by parameter
            switch (parameter)
            {
                case -1:  // player
                    return InGame.Player;
                case 0:  // this event
                    return InGame.Map.Events == null ? null : InGame.Map.Events[eventId];
                default:  // specific event
                    return InGame.Map.Events == null ? null : InGame.Map.Events[parameter];
            }
        }

        /// <summary>
        /// Change GameEvent according to intParams
        /// </summary>
        /// <param Name="Character"></param>
        int ChangeGameEvent(ref GameCharacter character)
        {
            if (character != null)
            {
                switch (intParams[5])
                {
                    case 0:  // x-coordinate
                        return character.X / GeexEdit.TileSize;
                    case 1:  // y-coordinate
                        return (character.Y - InGame.Player.CollisionHeight / 2) / GeexEdit.TileSize;
                    case 2:  // direction
                        return character.Dir;
                    case 3:  // screen x-coordinate
                        return character.ScreenX;
                    case 4:  // screen y-coordinate
                        return character.ScreenY;
                    case 5:  // terrain tag
                        return character.TerrainTag;
                }
            }
            return 0;
        }

        /// <summary>
        /// Change GamePlayer according to intParams
        /// </summary>
        int ChangeGamePlayer()
        {
            switch (intParams[5])
            {
                case 0:  // x-coordinate
                    return InGame.Player.X / GeexEdit.TileSize;
                case 1:  // y-coordinate
                    return (InGame.Player.Y - InGame.Player.CollisionHeight / 2) / GeexEdit.TileSize;
                case 2:  // direction
                    return InGame.Player.Dir;
                case 3:  // screen x-coordinate
                    return InGame.Player.ScreenX;
                case 4:  // screen y-coordinate
                    return InGame.Player.ScreenY;
                case 5:  // terrain tag
                    return InGame.Player.TerrainTag;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Calculate Operated Value
        /// </summary>
        /// <param Name="operation">operation</param>
        /// <param Name="operand_type">operand type (0: invariable 1: variable)</param>
        /// <param Name="operand">operand (number or variable ID)</param>
        public int OperateValue(int operation, int operand_type, int operand)
        {
            int value;
            // Get operand
            if (operand_type == 0)
            {
                value = operand;
            }
            else
            {
                value = InGame.Variables.Arr[operand];
            }
            // Reverse sign of integer if operation is [decrease]
            if (operation == 1)
            {
                value = -value;
            }
            // Return value
            return value;
        }

        /// <summary>
        /// End Event
        /// </summary>
        void CommandEnd()
        {
            // Clear list of event commands
            list = null;
            // If main map event and event ID are valid
            if (main && (eventId > 0))
            {
                // Unlock event
                InGame.Map.Events[eventId].Unlock();
            }
        }

        /// <summary>
        ///  Command Skip
        /// </summary>
        /// <returns></returns>
        bool CommandSkip()
        {
            // Get indent
            int indent = list[index].Indent;
            // Loop
            while (list[index + 1].Indent != indent) // Roys->Test Case : Put a condition in a loop
            {
                // Advance index
                index += 1;
            }
            return true;
        }

        public void Wait(short time)
        {
            // Set wait count
            waitCount = (int)(time * 2 * GameOptions.AdjustFrameRate);
        }
        #endregion

        #region Procs
        /// <summary>
        /// InGame.Temp.choice_proc "|n| @branch[current_indent] = n" => branch[InGame.Temp.choice_proc] = call( n )
        /// </summary>
        /// <param Name="n"></param>
        void ProcAssignBranch(int n)
        {
            Branch b = new Branch(n, true);
            branch[InGame.Temp.ChoiceProcCurrentIndent] = b;
        }

        /// <summary>
        /// InGame.Temp.MessageProc "message_waiting = false"
        /// </summary>
        void ProcMessageWaiting()
        {
            isWaitingMessage = false;
        }
        #endregion
    }
}