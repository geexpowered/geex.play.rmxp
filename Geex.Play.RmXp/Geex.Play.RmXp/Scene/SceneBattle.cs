using System.Collections.Generic;
using Geex.Play.Rpg.Arrow;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Spriteset;
using Geex.Play.Rpg.Window;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs battle screen processing.
    /// </summary>
    public partial class SceneBattle : SceneBase
    {
        #region Variables

        /// <summary>
        /// Managed command window
        /// </summary>
        WindowCommand actorCommandWindow;

        /// <summary>
        /// Managed party command
        /// </summary>
        WindowPartyCommand partyCommandWindow;

        /// <summary>
        /// Managed window help
        /// </summary>
        WindowHelp helpWindow;

        /// <summary>
        /// Managed window status
        /// </summary>
        WindowBattleStatus statusWindow;

        /// <summary>
        /// Managed message window
        /// </summary>
        WindowMessage messageWindow;

        /// <summary>
        /// Managed skill window
        /// </summary>
        WindowSkill skillWindow;

        /// <summary>
        /// Managed item window
        /// </summary>
        WindowItem itemWindow;

        /// <summary>
        /// Managed battle results window
        /// </summary>
        WindowBattleResult resultWindow;

        /// <summary>
        /// Spriteset battle
        /// </summary>
        Spriteset_Battle spriteset;

        /// <summary>
        /// Wait counter
        /// </summary>
        int waitCount;

        /// <summary>
        /// Battle troop id
        /// </summary>
        int troopId;

        /// <summary>
        /// Current battle phase
        /// </summary>
        int phase;

        /// <summary>
        /// Actor index
        /// </summary>
        int actorIndex;

        /// <summary>
        /// Active Battler
        /// </summary>
        GameBattler activeBattler;

        /// <summary>
        /// Current skill
        /// </summary>
        Skill skill;

        /// <summary>
        /// Current item
        /// </summary>
        Item item;

        /// <summary>
        /// Npc arrow 
        /// </summary>
        ArrowNpc npcArrow;

        /// <summary>
        /// Actor arrow
        /// </summary>
        ArrowActor actorArrow;

        /// <summary>
        /// Phase 4 detail
        /// </summary>
        int phase4Step;

        /// <summary>
        /// Action battlers
        /// </summary>
        List<GameBattler> actionBattlers = new List<GameBattler>();

        /// <summary>
        /// Action battlers
        /// </summary>
        List<GameBattler> targetBattlers = new List<GameBattler>();

        /// <summary>
        /// Current Animation 1 id
        /// </summary>
        int animation1Id = 0;

        /// <summary>
        /// Current Animation 2 id
        /// </summary>
        int animation2Id = 0;

        /// <summary>
        /// Current common event id
        /// </summary>
        int commonEventId = 0;


        /// <summary>
        /// Phase 5 wait count
        /// </summary>
        int phase5WaitCount;

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize
        /// </summary>
        public override void LoadSceneContent()
        {
            InitializeVariable();
            InitializeSpriteset();
            InitializeWindow();
            InitializeTransition();
        }

        /// <summary>
        /// Main Processing : Variable Initialization
        /// </summary>
        void InitializeVariable()
        {
            InitializeBattledata();         // Setup Battle Temp Data & Interpreter
            InitializeTroopdata();          // Setup Troop Data
            InitializeBattleEventFlags();   // Setup BattleEvent Flags
            // Initialize wait count
            waitCount = 0;
        }

        /// <summary>
        /// Main Processing : Battle Data Initialization
        /// </summary>
        void InitializeBattledata()
        {
            // Initialize each kind of temporary battle data
            InGame.Temp.IsInBattle = true;
            InGame.Temp.BattleTurn = 0;
            InGame.Temp.BattleAbort = false;
            InGame.Temp.BattleMainPhase = false;
            InGame.Temp.BattlebackName = InGame.Map.BattlebackName;
            InGame.Temp.ForcingBattler = null;
            // Initialize battle event interpreter
            InGame.Temp.BattleInterpreter.Setup(null, 0);
        }

        /// <summary>
        /// Main Processing : Troop Data Initialization
        /// </summary>
        void InitializeTroopdata()
        {
            // Prepare troop
            troopId = InGame.Temp.BattleTroopId;
            InGame.Troops.Setup(troopId);
        }

        /// <summary>
        /// Main Processing : Battle Events Flags Initialization
        /// Possible only after Troopdata initialization
        /// </summary>
        void InitializeBattleEventFlags()
        {
            InGame.Temp.BattleEventFlags.Clear();
            for (int index = 0; index < Data.Troops[troopId].Pages.Length; index++)
            {
                InGame.Temp.BattleEventFlags.Add(index, false);
            }
        }

        /// <summary>
        /// Main Processing : Spriteset Initialization
        /// </summary>
        void InitializeSpriteset()
        {
            // Make sprite set
            spriteset = new Spriteset_Battle();
        }

        /// <summary>
        /// Main Processing : Window Initialization
        /// </summary>
        void InitializeWindow()
        {
            // Make actor command window
            List<string> Commandlist = new List<string>();
            Commandlist.Add(Data.System.Wordings.Attack);
            Commandlist.Add(Data.System.Wordings.Skill);
            Commandlist.Add(Data.System.Wordings.Guard);
            Commandlist.Add(Data.System.Wordings.Item);
            actorCommandWindow = new WindowCommand(160, Commandlist);
            actorCommandWindow.Y = GeexEdit.GameWindowHeight -  320;
            actorCommandWindow.BackOpacity = 160;
            actorCommandWindow.IsActive = false;
            actorCommandWindow.IsVisible = false;
            // Make other windows
            partyCommandWindow = new WindowPartyCommand();
            helpWindow = new WindowHelp();
            helpWindow.BackOpacity = 160;
            helpWindow.IsVisible = false;
            statusWindow = new WindowBattleStatus();
            messageWindow = new WindowMessage();
        }

        /// <summary>
        /// Main Processing : Transition
        /// </summary>
        void InitializeTransition()
        {
            // Execute Transition
            if (Data.System.BattleTransition == "")
            {
                Graphics.Transition(20);
            }
            else
            {
                Graphics.Transition(40, Data.System.BattleTransition);
            }
            // Start pre-battle phase
            StartPhase1();
        }

        /// <summary>
        /// Battle Event Setup
        /// </summary>
        void SetupBattleEvent()
        {
            // If battle event is running
            if (InGame.Temp.BattleInterpreter.IsRunning)
            {
                return;
            }
            // Search for all battle event pages
            for (int index = 0; index < Data.Troops[troopId].Pages.Length; index++)
            {
                // Get event pages
                Troop.Page page = Data.Troops[troopId].Pages[index];
                // Make event conditions possible for reference with c
                Troop.Page.Condition _condition = page.PageCondition;
                // Continue if Page Condition Not Met
                if (! InGame.System.IsTroopConditionsMet(index, _condition))
                {
                    continue;
                }
                // Set up event
                InGame.Temp.BattleInterpreter.Setup(page.List, 0);
                // If this page span is [battle] or [turn]
                if (page.Span <= 1)
                {
                    // Set action completed flag
                    InGame.Temp.BattleEventFlags[index] = true;
                }
                return;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Dispose windows
            DisposeWindows();
            // Dispose sprites
            DisposeSpriteset();

            // Refresh map
            InGame.Map.Refresh();
            // If switching to title screen
            if (Main.Scene.IsA("SceneTitle"))
            {
                // Fade out screen
                Graphics.Transition();
                Graphics.Freeze();
            }
        }

        /// <summary>
        /// Dispose battle windows
        /// </summary>
        private void DisposeWindows()
        {
            actorCommandWindow.Dispose();
            partyCommandWindow.Dispose();
            helpWindow.Dispose();
            statusWindow.Dispose();
            messageWindow.Dispose();
            if (resultWindow != null)
            {
                resultWindow.Dispose();
            }
        }

        /// <summary>
        /// Dispose spriteset
        /// </summary>
        private void DisposeSpriteset()
        {
            spriteset.Dispose();
        }

        #endregion

        #region Methods

        #region Methods - Win/Lose

        /// <summary>
        /// Determine Battle Win/Loss Results
        /// </summary>
        /// <returns>true if lose</returns>
        bool IsJudged()
        {
            // If all dead determinant is true, or number of members in party is 0
            if (InGame.Party.IsAllDead || InGame.Party.Actors.Count == 0)
            {
                // If possible to lose
                if (InGame.Temp.IsBattleCanLose)
                {
                    // Return to BGM before battle starts
                    InGame.System.SongPlay(InGame.Temp.MapSong);
                    // Battle ends
                    BattleEnd(2);
                    // Return true
                    return true;
                }
                // Set game over flag
                InGame.Temp.IsGameover = true;
                // Return true
                return true;
            }
            // Return false if even 1 enemy exists
            foreach (GameNpc npc in InGame.Troops.Npcs)
            {
                if (npc.IsExist)
                {
                    return false;
                }
            }
            // Start after battle phase (win)
            StartPhase5();
            // Return true
            return true;
        }

        /// <summary>
        /// Battle Ends
        /// </summary>
        /// <param Name="result">results (0:win 1:lose 2:escape)</param>
        void BattleEnd(short result)
        {
            // Clear in battle flag
            InGame.Temp.IsInBattle = false;
            // Clear entire party actions flag
            InGame.Party.ClearActions();
            // Remove battle states
            foreach (GameActor actor in InGame.Party.Actors)
            {
                actor.RemoveStatesBattle();
            }
            // Clear enemies
            InGame.Troops.Npcs.Clear();
            // Call battle callback
            if (InGame.Temp.BattleProc != null)
            {
                InGame.Temp.BattleProc(result);
                InGame.Temp.BattleProc = null;
            }
            // Switch to map screen
            InGame.System.SongEffectStop();
            Main.Scene = new SceneMap();
        }

        #endregion

        #region Methods - Update

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            UpdateInterpreter();               // Update Battle Interpreter
            UpdateSystems();                   // Update Screen & Timer
            UpdateTransition();                // Update Transition
            UpdateSpriteset();
            UpdateWindow();
            if (UpdateMessage()) return;       // Update Message Test
            if (UpdateSpriteSetEffect()) return;      // Update Spriteset Effect Test
            if (UpdateGameover()) return;      // Update Gameover Test
            if (UpdateTitle()) return;         // Update Title Test
            if (UpdateAbort()) return;         // Update Abort Test
            if (UpdateWait()) return;          // Update Wait Test
            if (UpdateForcing()) return;       // Update Forcing Test
            UpdateBattlephase();               // Update Battle Phase
        }

        /// <summary>
        /// Frame Update : Interpreter
        /// </summary>
        void UpdateInterpreter()
        {
            // If battle event is running
            if (InGame.Temp.BattleInterpreter.IsRunning)
            {
                // Update interpreter
                InGame.Temp.BattleInterpreter.Update();
                // If a Battler which is forcing actions doesn't exist
                if (InGame.Temp.ForcingBattler == null)
                {
                    // If battle event has finished running
                    if (!InGame.Temp.BattleInterpreter.IsRunning)
                    {
                        // Rerun battle event set up if battle continues
                        if (!IsJudged())
                        {
                            SetupBattleEvent();
                        }
                    }
                    // If not after battle phase
                    if (phase != 5)
                    {
                        // Refresh status window
                        statusWindow.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// Frame Update : Systems
        /// </summary>
        public void UpdateSystems()
        {
            // Update system (Timer) and screen
            InGame.System.Update();
            InGame.Screen.Update();
            // If Timer has reached 0
            if (InGame.System.IsTimerWorking && InGame.System.Timer == 0)
            {
                // Abort battle
                InGame.Temp.BattleAbort = true;
            }
        }

        /// <summary>
        /// Frame Update : Transition
        /// </summary>
        public void UpdateTransition()
        {
            // If Transition is processing
            if (InGame.Temp.IsProcessingTransition)
            {
                // Clear Transition processing flag
                InGame.Temp.IsProcessingTransition = false;
                // Execute Transition
                if (InGame.Temp.TransitionName == "")
                {
                    Graphics.Transition(20);
                }
                else
                {
                    Graphics.Transition(40, InGame.Temp.TransitionName);
                }
            }
        }

        /// <summary>
        /// Update battle spriteset
        /// </summary>
        private void UpdateSpriteset()
        {
            spriteset.Update();
        }

        /// <summary>
        /// Update battle windows
        /// </summary>
        private void UpdateWindow()
        {
            helpWindow.Update();
            partyCommandWindow.Update();
            actorCommandWindow.Update();
            statusWindow.Update();
            messageWindow.Update();
            if (skillWindow != null)
            {
                skillWindow.Update();
            }
            if (itemWindow != null)
            {
                itemWindow.Update();
            }
        }

        /// <summary>
        /// Frame Update : Message Test
        /// </summary>
        /// <returns>true if message_window_showing</returns>
        public bool UpdateMessage()
        {
            // If message window is showing
            return InGame.Temp.IsMessageWindowShowing;
        }

        /// <summary>
        /// Frame Update : Spriteset Effect Test
        /// </summary>
        /// <returns>true if Effect</returns>
        bool UpdateSpriteSetEffect()
        {
            // If Effect is showing
            return spriteset.IsEffect;
        }

        /// <summary>
        /// Frame Update : Gameover Test
        /// </summary>
        /// <returns>true if Gameover</returns>
        bool UpdateGameover()
        {
            // If game over
            if (InGame.Temp.IsGameover)
            {
                // Switch to game over screen
                Main.Scene = new SceneGameover();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Frame Update : Title Test
        /// </summary>
        /// <returns>true if current Scene is SceneTitle</returns>
        bool UpdateTitle()
        {
            // If returning to title screen
            if (InGame.Temp.ToTitle)
            {
                // Switch to title screen
                Main.Scene = new SceneTitle();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Frame Update : Abort Test
        /// </summary>
        /// <returns>true if battle is aborted</returns>
        bool UpdateAbort()
        {
            // If battle is aborted
            if (InGame.Temp.BattleAbort)
            {
                // Return to BGM used before battle started
                InGame.System.SongPlay(InGame.Temp.MapSong);
                // Battle ends
                BattleEnd(1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Frame Update : Wait Test
        /// </summary>
        /// <returns>true if wait counter > 0</returns>
        bool UpdateWait()
        {
            // If waiting
            if (waitCount > 0)
            {
                // Decrease wait count
                waitCount -= 1;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Frame Update : Forcing Test
        /// </summary>
        /// <returns>true if Battler forcing an action doesn't exist,
        /// and battle event is running</returns>
        bool UpdateForcing()
        {
            // If Battler forcing an action doesn't exist,
            // and battle event is running
            if (InGame.Temp.ForcingBattler == null &&
               InGame.Temp.BattleInterpreter.IsRunning)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Frame Update : Battle Phase
        /// </summary>
        void UpdateBattlephase()
        {
            // Branch according to phase
            switch (phase)
            {
                case 1:  // pre-battle phase
                    UpdatePhase1();
                    break;
                case 2:  // party command phase
                    UpdatePhase2();
                    break;
                case 3:  // actor command phase
                    UpdatePhase3();
                    break;
                case 4:  // main phase
                    UpdatePhase4();
                    break;
                case 5:  // after battle phase
                    UpdatePhase5();
                    break;
            }
        }

        #endregion

        #region Methods - Phase 1 (Pre-battle Phase)

        /// <summary>
        /// Start Pre-Battle Phase
        /// </summary>
        void StartPhase1()
        {
            // Shift to phase 1
            phase = 1;
            // Clear all party member actions
            InGame.Party.ClearActions();
            // Set up battle event
            SetupBattleEvent();
        }

        /// <summary>
        /// Frame Update (pre-battle phase)
        /// </summary>
        void UpdatePhase1()
        {
            // Determine win/loss situation
            if (IsJudged())
            {
                // If won or lost: end method
                return;
            }
            // Start party command phase
            StartPhase2();
        }

        #endregion

        #region Methods - Phase 2 (Party Command Phase)

        /// <summary>
        /// Start Party Command Phase
        /// </summary>
        void StartPhase2()
        {
            // Shift to phase 2
            phase = 2;
            // Set actor to non-selecting
            actorIndex = -1;
            activeBattler = null;
            // Enable party command window
            partyCommandWindow.IsActive = true;
            partyCommandWindow.IsVisible = true;
            // Disable actor command window
            actorCommandWindow.IsActive = false;
            actorCommandWindow.IsVisible = false;
            // Clear main phase flag
            InGame.Temp.BattleMainPhase = false;
            // Clear all party member actions
            InGame.Party.ClearActions();
            // If impossible to input command
            if (!InGame.Party.IsInputable)
            {
                // Start main phase
                StartPhase4();
            }
        }

        /// <summary>
        /// Frame Update (party command phase)
        /// </summary>
        void UpdatePhase2()
        {
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Branch by party command window cursor position
                switch (partyCommandWindow.Index)
                {
                    case 0:  // fight
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Start actor command phase
                        StartPhase3();
                        break;
                    case 1:  // escape
                        // If it's not possible to escape
                        if (!InGame.Temp.IsBattleCanEscape)
                        {
                            // Play buzzer SE
                            InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                            return;
                        }
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Escape processing
                        UpdatePhase2Escape();
                        break;
                }
                return;
            }
        }

        /// <summary>
        /// Frame Update (party command phase: escape)
        /// </summary>
        void UpdatePhase2Escape()
        {
            // Calculate enemy agility average
            int _npcs_agi = 0;
            int _npcs_number = 0;
            foreach (GameNpc npc in InGame.Troops.Npcs)
            {
                if (npc.IsExist)
                {
                    _npcs_agi += npc.Agi;
                    _npcs_number += 1;
                }
            }
            if (_npcs_number > 0)
            {
                _npcs_agi /= _npcs_number;
            }
            // Calculate actor agility average
            int actors_agi = 0;
            int actors_number = 0;
            foreach (GameActor actor in InGame.Party.Actors)
            {
                if (actor.IsExist)
                {
                    actors_agi += actor.Agi;
                    actors_number += 1;
                }
            }
            if (actors_number > 0)
            {
                actors_agi /= actors_number;
            }
            // Determine if escape is successful
            bool success = InGame.Rnd.Next(100) < 50 * actors_agi / _npcs_agi;
            // If escape is successful
            if (success)
            {
                // Play escape SE
                InGame.System.SoundPlay(Data.System.EscapeSoundEffect);
                // Return to BGM before battle started
                InGame.System.SongPlay(InGame.Temp.MapSong);
                // Battle ends
                BattleEnd(1);
                // If escape is failure
            }
            else
            {
                // Clear all party member actions
                InGame.Party.ClearActions();
                // Start main phase
                StartPhase4();
            }
        }

        #endregion

        #region Methods - Phase 3 (Actor Command Phase)

        /// <summary>
        /// Start Actor Command Phase
        /// </summary>
        void StartPhase3()
        {
            // Shift to phase 3
            phase = 3;
            // Set actor as unselectable
            actorIndex = -1;
            activeBattler = null;
            // Go to command input for next actor
            Phase3NextActor();
        }

        /// <summary>
        /// Go to Command Input for Next Actor
        /// </summary>
        void Phase3NextActor()
        {
            // Loop
            do
            {
                // Actor blink Effect OFF
                if (activeBattler != null)
                {
                    activeBattler.IsBlink = false;
                }
                // If last actor
                if (actorIndex == InGame.Party.Actors.Count - 1)
                {
                    // Start main phase
                    StartPhase4();
                    return;
                }
                // Advance actor index
                actorIndex += 1;
                activeBattler = InGame.Party.Actors[actorIndex];
                activeBattler.IsBlink = true;
                // Once more if actor refuses command input
            } while (!activeBattler.IsInputable);
            // Set up actor command window
            Phase3SetupCommandwindow();
        }

        /// <summary>
        /// Go to Command Input of Previous Actor
        /// </summary>
        void Phase3PriorActor()
        {
            // Loop
            do
            {
                // Actor blink Effect OFF
                if (activeBattler != null)
                {
                    activeBattler.IsBlink = false;
                }
                // If first actor
                if (actorIndex == 0)
                {
                    // Start party command phase
                    StartPhase2();
                    return;
                }
                // Return to actor index
                actorIndex -= 1;
                activeBattler = InGame.Party.Actors[actorIndex];
                activeBattler.IsBlink = true;
                // Once more if actor refuses command input
            } while (!activeBattler.IsInputable);
            // Set up actor command window
            Phase3SetupCommandwindow();
        }

        /// <summary>
        /// Actor Command Window Setup
        /// </summary>
        void Phase3SetupCommandwindow()
        {
            // Disable party command window
            partyCommandWindow.IsActive = false;
            partyCommandWindow.IsVisible = false;
            // Enable actor command window
            actorCommandWindow.IsActive = true;
            actorCommandWindow.IsVisible = true;
            // Set actor command window position
            actorCommandWindow.X = actorIndex * 160; //GeexEdit.GameWindowWidth / 2 - 160 * InGame.Party.Actors.Count / 2 + actorIndex * 160;
            // Set index to 0
            actorCommandWindow.Index = 0;
        }

        /// <summary>
        /// Frame Update (actor command phase)
        /// </summary>
        void UpdatePhase3()
        {
            // If enemy arrow is enabled
            if (npcArrow != null)
            {
                npcArrow.Update();
                UpdatePhase3EnemySelect();
            }
            // If actor arrow is enabled
            else if (actorArrow != null)
            {
                actorArrow.Update();
                UpdatePhase3ActorSelect();
            }
            // If skill window is enabled
            else if (skillWindow != null)
            {
                UpdatePhase3SkillSelect();
            }
            // If item window is enabled
            else if (itemWindow != null)
            {
                UpdatePhase3ItemSelect();
            }
            // If actor command window is enabled
            else if (actorCommandWindow.IsActive)
            {
                UpdatePhase3BasicCommand();
            }
        }

        /// <summary>
        /// Frame Update (actor command phase : basic command)
        /// </summary>
        void UpdatePhase3BasicCommand()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Go to command input for previous actor
                Phase3PriorActor();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Branch by actor command window cursor position
                switch (actorCommandWindow.Index)
                {
                    case 0:  // attack
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Set action
                        activeBattler.CurrentAction.kind = 0;
                        activeBattler.CurrentAction.basic = 0;
                        // Start enemy selection
                        StartNpcSelect();
                        break;
                    case 1:  // skill
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Set action
                        activeBattler.CurrentAction.kind = 1;
                        // Start skill selection
                        StartSkillSelect();
                        break;
                    case 2:  // guard
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Set action
                        activeBattler.CurrentAction.kind = 0;
                        activeBattler.CurrentAction.basic = 1;
                        // Go to command input for next actor
                        Phase3NextActor();
                        break;
                    case 3:  // item
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Set action
                        activeBattler.CurrentAction.kind = 2;
                        // Start item selection
                        StartItemSelect();
                        break;
                }
                return;
            }
        }

        /// <summary>
        /// Frame Update (actor command phase : skill selection)
        /// </summary>
        void UpdatePhase3SkillSelect()
        {
            // Make skill window visible
            skillWindow.IsVisible = true;
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // End skill selection
                EndSkillSelect();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Get currently selected data on the skill window
                skill = skillWindow.Skill;
                // If it can't be used
                if (skill == null || !activeBattler.IsSkillCanUse(skill.Id))
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Set action
                activeBattler.CurrentAction.SkillId = skill.Id;
                // Make skill window invisible
                skillWindow.IsVisible = false;
                skillWindow.IsActive = false;
                // If Effect scope is single enemy
                if (skill.Scope == 1)
                {
                    // Start enemy selection
                    StartNpcSelect();
                }
                // If Effect scope is single ally
                else if (skill.Scope == 3 || skill.Scope == 5)
                {
                    // Start actor selection
                    StartActorSelect();
                    // If Effect scope is not single
                }
                else
                {
                    // End skill selection
                    EndSkillSelect();
                    // Go to command input for next actor
                    Phase3NextActor();
                }
                return;
            }
        }

        /// <summary>
        /// Frame Update (actor command phase : item selection)
        /// </summary>
        void UpdatePhase3ItemSelect()
        {
            // Make item window visible
            itemWindow.IsVisible = true;
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // End item selection
                EndItemSelect();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Get currently selected data on the item window
                item = (Item)itemWindow.Item;
                // If it can't be used
                if (item == null || !InGame.Party.IsItemCanUse(item.Id))
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Set action
                activeBattler.CurrentAction.ItemId = item.Id;
                // Make item window invisible
                itemWindow.IsVisible = false;
                itemWindow.IsActive = false;

                // If Effect scope is single enemy
                if (item.Scope == 1)
                {
                    // Start enemy selection
                    StartNpcSelect();
                }
                // If Effect scope is single ally
                else if (item.Scope == 3 || item.Scope == 5)
                {
                    // Start actor selection
                    StartActorSelect();
                }
                // If Effect scope is not single
                else
                {
                    // End item selection
                    EndItemSelect();
                    // Go to command input for next actor
                    Phase3NextActor();
                }
                return;
            }
        }

        /// <summary>
        /// Frame Update (actor command phase : enemy selection)
        /// </summary>
        void UpdatePhase3EnemySelect()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // End enemy selection
                EndNpcSelect();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Set action
                activeBattler.CurrentAction.TargetIndex = npcArrow.index;
                // End enemy selection
                EndNpcSelect();
                // If skill window is showing
                if (skillWindow != null)
                {
                    // End skill selection
                    EndSkillSelect();
                }
                // If item window is showing
                if (itemWindow != null)
                {
                    // End item selection
                    EndItemSelect();
                }
                // Go to command input for next actor
                Phase3NextActor();
            }
        }

        /// <summary>
        /// Frame Update (actor command phase : actor selection)
        /// </summary>
        void UpdatePhase3ActorSelect()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // End actor selection
                EndActorSelect();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Set action
                activeBattler.CurrentAction.TargetIndex = actorArrow.index;
                // End actor selection
                EndActorSelect();
                // If skill window is showing
                if (skillWindow != null)
                {
                    // End skill selection
                    EndSkillSelect();
                }
                // If item window is showing
                if (itemWindow != null)
                {
                    // End item selection
                    EndItemSelect();
                }
                // Go to command input for next actor
                Phase3NextActor();
            }
        }

        /// <summary>
        /// Start Enemy Selection
        /// </summary>
        void StartNpcSelect()
        {
            // Make npc arrow
            npcArrow = new ArrowNpc(Graphics.Foreground);
            // Associate help window
            npcArrow.HelpWindow = helpWindow;
            // Disable actor command window
            actorCommandWindow.IsActive = false;
            actorCommandWindow.IsVisible = false;
        }

        /// <summary>
        /// End Enemy Selection
        /// </summary>
        void EndNpcSelect()
        {
            // Dispose of npc arrow
            npcArrow.Dispose();
            npcArrow = null;
            // If command is [fight]
            if (actorCommandWindow.Index == 0)
            {
                // Enable actor command window
                actorCommandWindow.IsActive = true;
                actorCommandWindow.IsVisible = true;
                // Hide help window
                helpWindow.IsVisible = false;
            }
            // If Skill Window Exist
            if (!(skillWindow == null))
            {
                // Enable Skill Window
                skillWindow.IsActive = true;
            }
            // If Item Window Exist
            if (!(itemWindow == null))
            {
                // Enable Skill Window
                itemWindow.IsActive = true;
            }
        }

        /// <summary>
        /// Start Actor Selection
        /// </summary>
        void StartActorSelect()
        {
            // Make actor arrow
            actorArrow = new ArrowActor(Graphics.Foreground);
            actorArrow.index = actorIndex;
            // Associate help window
            actorArrow.HelpWindow = helpWindow;
            // Disable actor command window
            actorCommandWindow.IsActive = false;
            actorCommandWindow.IsVisible = false;
        }

        /// <summary>
        /// End Actor Selection
        /// </summary>
        void EndActorSelect()
        {
            // Dispose of actor arrow
            actorArrow.Dispose();
            actorArrow = null;
            // If Skill Window Exist
            if (!(skillWindow == null))
            {
                // Enable Skill Window
                skillWindow.IsActive = true;
            }
            // If Item Window Exist
            if (!(itemWindow == null))
            {
                // Enable Skill Window
                itemWindow.IsActive = true;
            }
        }

        /// <summary>
        /// Start Skill Selection
        /// </summary>
        void StartSkillSelect()
        {
            // Make skill window
            skillWindow = new WindowSkill((GameActor)activeBattler);
            // Associate help window
            skillWindow.HelpWindow = helpWindow;
            // Disable actor command window
            actorCommandWindow.IsActive = false;
            actorCommandWindow.IsVisible = false;
        }

        /// <summary>
        /// End Skill Selection
        /// </summary>
        void EndSkillSelect()
        {
            // Dispose of skill window
            skillWindow.Dispose();
            skillWindow = null;
            // Hide help window
            helpWindow.IsVisible = false;
            // Enable actor command window
            actorCommandWindow.IsActive = true;
            actorCommandWindow.IsVisible = true;
        }

        /// <summary>
        /// Start Item Selection
        /// </summary>
        void StartItemSelect()
        {
            // Make item window
            itemWindow = new WindowItem();
            // Associate help window
            itemWindow.HelpWindow = helpWindow;
            // Disable actor command window
            actorCommandWindow.IsActive = false;
            actorCommandWindow.IsVisible = false;
        }

        /// <summary>
        /// End Item Selection
        /// </summary>
        void EndItemSelect()
        {
            // Dispose of item window
            itemWindow.Dispose();
            itemWindow = null;
            // Hide help window
            helpWindow.IsVisible = false;
            // Enable actor command window
            actorCommandWindow.IsActive = true;
            actorCommandWindow.IsVisible = true;
        }

        #endregion

        #region Methods - Phase 4 (Main Phase)

        /// <summary>
        /// Start Main Phase
        /// </summary>
        void StartPhase4()
        {
            // Shift to phase 4
            phase = 4;
            // Turn count
            InGame.Temp.BattleTurn += 1;
            // Search all battle event pages
            for (int index = 0; index < Data.Troops[troopId].Pages.Length; index++)
            {
                // Get event page
                Troop.Page _page = Data.Troops[troopId].Pages[index];
                // If this page span is [turn]
                if (_page.Span == 1)
                {
                    // Clear action completed flags
                    InGame.Temp.BattleEventFlags[index] = false;
                }
            }
            // Set actor as unselectable
            actorIndex = -1;
            activeBattler = null;
            // Enable party command window
            partyCommandWindow.IsActive = false;
            partyCommandWindow.IsVisible = false;
            // Disable actor command window
            actorCommandWindow.IsActive = false;
            actorCommandWindow.IsVisible = false;
            // Set main phase flag
            InGame.Temp.BattleMainPhase = true;
            // Make enemy action
            foreach (GameNpc npc in InGame.Troops.Npcs)
            {
                npc.MakeAction();
            }
            // Make action orders
            MakeActionOrders();
            // Shift to step 1
            phase4Step = 1;
        }

        /// <summary>
        /// Make Action Orders
        /// </summary>
        void MakeActionOrders()
        {
            // Initialize action_battlers array
            actionBattlers.Clear();
            // Add enemy to action_battlers array
            foreach (GameNpc npc in InGame.Troops.Npcs)
            {
                actionBattlers.Add(npc);
            }
            // Add actor to action_battlers array
            foreach (GameActor actor in InGame.Party.Actors)
            {
                actionBattlers.Add(actor);
            }
            // Decide action speed for all
            foreach (GameBattler battler in actionBattlers)
            {
                battler.MakeActionSpeed();
            }
            // Line up action speed in order from greatest to least
            actionBattlers.Sort(InGame.SpeedComparer);
        }

        /// <summary>
        /// Frame Update (main phase)
        /// </summary>
        void UpdatePhase4()
        {
            switch (phase4Step)
            {
                case 1:
                    UpdatePhase4Step1();
                    break;
                case 2:
                    UpdatePhase4Step2();
                    break;
                case 3:
                    UpdatePhase4Step3();
                    break;
                case 4:
                    UpdatePhase4Step4();
                    break;
                case 5:
                    UpdatePhase4Step5();
                    break;
                case 6:
                    UpdatePhase4Step6();
                    break;
            }
        }

        /// <summary>
        /// Frame Update (main phase step 1 : action preparation)
        /// </summary>
        void UpdatePhase4Step1()
        {
            // Hide help window
            helpWindow.IsVisible = false;
            // Determine win/loss
            if (IsJudged())
            {
                // If won, or if lost : end method
                return;
            }
            // If an action forcing Battler doesn't exist
            if (InGame.Temp.ForcingBattler == null)
            {
                // Set up battle event
                SetupBattleEvent();
                // If battle event is running
                if (InGame.Temp.BattleInterpreter.IsRunning)
                {
                    return;
                }
            }
            // If an action forcing Battler exists
            if (InGame.Temp.ForcingBattler != null)
            {
                // Add to head, or move
                actionBattlers.Remove(InGame.Temp.ForcingBattler);
                actionBattlers.Insert(0, InGame.Temp.ForcingBattler);
            }
            // If no actionless battlers exist (all have performed an action)
            if (actionBattlers.Count == 0)
            {
                // Start party command phase
                StartPhase2();
                return;
            }
            // Initialize Animation ID and common event ID
            animation1Id = 0;
            animation2Id = 0;
            commonEventId = 0;
            // Shift from head of actionless battlers
            activeBattler = actionBattlers[0];
            actionBattlers.RemoveAt(0);
            // If already removed from battle
            /*
            if (active_battler.index == null)
            {
                return;
            }*/
            // Slip damage
            if (activeBattler.Hp > 0 && activeBattler.IsSlipDamage)
            {
                activeBattler.SlipDamageEffect();
                activeBattler.IsDamagePop = true;
            }
            // Natural removal of states
            activeBattler.RemoveStatesAuto();
            // Refresh status window
            statusWindow.Refresh();
            // Shift to step 2
            phase4Step = 2;
        }

        /// <summary>
        /// Frame Update (main phase step 2 : start action)
        /// </summary>
        void UpdatePhase4Step2()
        {
            // If not a forcing action
            if (!activeBattler.CurrentAction.IsForcing)
            {
                // If restriction is [normal attack npc] or [normal attack ally]
                if (activeBattler.Restriction == 2 || activeBattler.Restriction == 3)
                {
                    // Set attack as an action
                    activeBattler.CurrentAction.kind = 0;
                    activeBattler.CurrentAction.basic = 0;
                }
                // If restriction is [cannot perform action]
                if (activeBattler.Restriction == 4)
                {
                    // Clear Battler being forced into action
                    InGame.Temp.ForcingBattler = null;
                    // Shift to step 1
                    phase4Step = 1;
                    return;
                }
            }
            // Clear target battlers
            targetBattlers.Clear();
            // Branch according to each action
            switch (activeBattler.CurrentAction.kind)
            {
                case 0:  // basic
                    MakeBasicActionResult();
                    break;
                case 1:  // skill
                    MakeSkillActionResult();
                    break;
                case 2:  // item
                    MakeItemActionResult();
                    break;
            }
            // Shift to step 3
            if (phase4Step == 2)
            {
                phase4Step = 3;
            }
        }

        /// <summary>
        /// Make Basic Action Results
        /// </summary>
        void MakeBasicActionResult()
        {
            // If attack
            if (activeBattler.CurrentAction.basic == 0)
            {
                // Set anaimation ID
                animation1Id = activeBattler.Animation1Id;
                animation2Id = activeBattler.Animation2Id;
                //Initilize target and index
                GameBattler target = null;
                int index = 0;
                // If action Battler is npc
                if (activeBattler.IsA("GameNpc"))
                {
                    if (activeBattler.Restriction == 3)
                        target = InGame.Troops.RandomTargetNpc();
                    else if (activeBattler.Restriction == 2)
                        target = InGame.Party.RandomTargetActor();
                    else
                    {
                        index = activeBattler.CurrentAction.TargetIndex;
                        target = InGame.Party.SmoothTargetActor(index);
                    }
                }
                // If action Battler is actor
                if (activeBattler.IsA("GameActor"))
                {
                    if (activeBattler.Restriction == 3)
                        target = InGame.Party.RandomTargetActor();
                    else if (activeBattler.Restriction == 2)
                        target = InGame.Troops.RandomTargetNpc();
                    else
                    {
                        index = activeBattler.CurrentAction.TargetIndex;
                        target = InGame.Troops.SmoothTargetNpc(index);
                    }
                }
                // Set array of targeted battlers
                targetBattlers.Clear();
                targetBattlers.Add(target);
                // Apply normal attack results
                foreach (GameBattler attacked_target in targetBattlers)
                {
                    attacked_target.AttackEffect(activeBattler);
                }
                return;
            }
            // If guard
            if (activeBattler.CurrentAction.basic == 1)
            {
                // Display "Guard" in help window
                helpWindow.SetText(Data.System.Wordings.Guard, 1);
                return;
            }
            // If escape
            if (activeBattler.IsA("GameNpc") &&
               activeBattler.CurrentAction.basic == 2)
            {
                // Display "Escape" in help window
                helpWindow.SetText("Escape", 1);
                // Escape
                activeBattler.Escape();
                return;
            }
            // If doing nothing
            if (activeBattler.CurrentAction.basic == 3)
            {
                // Clear Battler being forced into action
                InGame.Temp.ForcingBattler = null;
                // Shift to step 1
                phase4Step = 1;
                return;
            }
        }

        /// <summary>
        /// Set Targeted Battler for Skill or Item
        /// </summary>
        /// <param Name="scope">Effect scope for skill or item</param>
        void SetTargetBattlers(int scope)
        {
            int index = 0;
            // If Battler performing action is npc
            if (activeBattler.IsA("GameNpc"))
            {
                // Branch by Effect scope
                switch (scope)
                {
                    case 1:  // single enemy
                        index = activeBattler.CurrentAction.TargetIndex;
                        targetBattlers.Add(InGame.Party.SmoothTargetActor(index));
                        break;
                    case 2:  // all enemies
                        foreach (GameActor actor in InGame.Party.Actors)
                        {
                            if (actor.IsExist)
                            {
                                targetBattlers.Add(actor);
                            }
                        }
                        break;
                    case 3:  // single ally
                        index = activeBattler.CurrentAction.TargetIndex;
                        targetBattlers.Add(InGame.Troops.SmoothTargetNpc(index));
                        break;
                    case 4:  // all allies
                        foreach (GameNpc npc in InGame.Troops.Npcs)
                        {
                            if (npc.IsExist)
                            {
                                targetBattlers.Add(npc);
                            }
                        }
                        break;
                    case 5:  // single ally (HP 0) 
                        index = activeBattler.CurrentAction.TargetIndex;
                        GameNpc _npc = InGame.Troops.Npcs[index];
                        if (_npc != null && _npc.IsHp0)
                        {
                            targetBattlers.Add(_npc);
                        }
                        break;
                    case 6:  // all allies (HP 0) 
                        foreach (GameNpc npc in InGame.Troops.Npcs)
                        {
                            if (npc != null && npc.IsHp0)
                            {
                                targetBattlers.Add(npc);
                            }
                        }
                        break;
                    case 7:  // user
                        targetBattlers.Add(activeBattler);
                        break;
                }
            }
            // If Battler performing action is actor
            if (activeBattler.IsA("GameActor"))
            {
                // Branch by Effect scope
                switch (scope)
                {
                    case 1:  // single enemy
                        index = activeBattler.CurrentAction.TargetIndex;
                        targetBattlers.Add(InGame.Troops.SmoothTargetNpc(index));
                        break;
                    case 2:  // all enemies
                        foreach (GameNpc npc in InGame.Troops.Npcs)
                        {
                            if (npc.IsExist)
                                targetBattlers.Add(npc);
                        }
                        break;
                    case 3:  // single ally
                        index = activeBattler.CurrentAction.TargetIndex;
                        targetBattlers.Add(InGame.Party.SmoothTargetActor(index));
                        break;
                    case 4:  // all allies
                        foreach (GameActor actor in InGame.Party.Actors)
                        {
                            if (actor.IsExist)
                                targetBattlers.Add(actor);
                        }
                        break;
                    case 5:  // single ally (HP 0) 
                        index = activeBattler.CurrentAction.TargetIndex;
                        GameActor _actor = InGame.Party.Actors[index];
                        if (_actor != null && _actor.IsHp0)
                            targetBattlers.Add(_actor);
                        break;
                    case 6:  // all allies (HP 0) 
                        foreach (GameActor actor in InGame.Party.Actors)
                        {
                            if (actor != null && actor.IsHp0)
                            {
                                targetBattlers.Add(actor);
                            }
                        }
                        break;
                    case 7:  // user
                        targetBattlers.Add(activeBattler);
                        break;
                }
            }
        }

        /// <summary>
        /// Make Skill Action Results
        /// </summary>
        void MakeSkillActionResult()
        {
            // Get skill
            skill = Data.Skills[activeBattler.CurrentAction.SkillId];
            // If not a forcing action
            if (!activeBattler.CurrentAction.IsForcing)
            {
                // If unable to use due to SP running out
                if (!activeBattler.IsSkillCanUse(skill.Id))
                {
                    // Clear Battler being forced into action
                    InGame.Temp.ForcingBattler = null;
                    // Shift to step 1
                    phase4Step = 1;
                    return;
                }
            }
            // Use up SP
            activeBattler.Sp -= skill.SpCost;
            // Refresh status window
            statusWindow.Refresh();
            // Show skill Name on help window
            helpWindow.SetText(skill.Name, 1);
            // Set Animation ID
            animation1Id = skill.Animation1Id;
            animation2Id = skill.Animation2Id;
            // Set command event ID
            commonEventId = skill.CommonEventId;
            // Set target battlers
            SetTargetBattlers(skill.Scope);
            // Apply skill Effect
            foreach (GameBattler target in targetBattlers)
            {
                target.SkillEffect(activeBattler, skill);
            }
        }

        /// <summary>
        /// Make Item Action Results
        /// </summary>
        void MakeItemActionResult()
        {
            // Get item
            item = Data.Items[activeBattler.CurrentAction.ItemId];
            // If unable to use due to items running out
            if (!InGame.Party.IsItemCanUse(item.Id))
            {
                // Shift to step 1
                phase4Step = 1;
                return;
            }
            // If consumable
            if (item.Consumable)
            {
                // Decrease used item by 1
                InGame.Party.LoseItem(item.Id, 1);
            }
            // Display item Name on help window
            helpWindow.SetText(item.Name, 1);
            // Set Animation ID
            animation1Id = item.Animation1Id;
            animation2Id = item.Animation2Id;
            // Set common event ID
            commonEventId = item.CommonEventId;
            // Set targeted battlers
            SetTargetBattlers(item.Scope);
            // Apply item Effect
            foreach (GameBattler target in targetBattlers)
            {
                target.ItemEffect(item);
            }
        }

        /// <summary>
        /// Frame Update (main phase step 3 : Animation for action performer)
        /// </summary>
        void UpdatePhase4Step3()
        {
            // Animation for action performer (if ID is 0, then white flash)
            if (animation1Id == 0)
            {
                activeBattler.IsWhiteFlash = true;
            }
            else
            {
                activeBattler.AnimationId = animation1Id;
                activeBattler.IsAnimationHit = true;
            }
            // Shift to step 4
            phase4Step = 4;
        }

        /// <summary>
        /// Frame Update (main phase step 4 : Animation for target)
        /// </summary>
        void UpdatePhase4Step4()
        {
            // Animation for target
            foreach (GameBattler target in targetBattlers)
            {
                target.AnimationId = animation2Id;
                target.IsAnimationHit = (target.Damage != "Miss");
            }
            // Animation has at least 8 frames, regardless of its length
            waitCount = 8;
            // Shift to step 5
            phase4Step = 5;
        }

        /// <summary>
        /// Frame Update (main phase step 5 : damage display)
        /// </summary>
        void UpdatePhase4Step5()
        {
            // Hide help window
            helpWindow.IsVisible = false;
            // Refresh status window
            statusWindow.Refresh();
            // Display damage
            foreach (GameBattler target in targetBattlers)
            {
                if (target.Damage != null)
                {
                    target.IsDamagePop = true;
                }
            }
            // Animation has at least 8 frames, regardless of its length
            waitCount = 8;
            // Shift to step 6
            phase4Step = 6;
        }

        /// <summary>
        /// Frame Update (main phase step 6 : refresh)
        /// </summary>
        void UpdatePhase4Step6()
        {
            // Clear Battler being forced into action
            InGame.Temp.ForcingBattler = null;
            // If common event ID is valid
            if (commonEventId > 0)
            {
                // Set up event
                CommonEvent _common_event = Data.CommonEvents[commonEventId];
                InGame.Temp.BattleInterpreter.Setup(_common_event.List, 0);
            }
            // Shift to step 1
            phase4Step = 1;
        }

        #endregion

        #region Methods - Phase 5 (After Battle Phase)

        /// <summary>
        /// Start After Battle Phase
        /// </summary>
        void StartPhase5()
        {
            // Shift to phase 5
            phase = 5;
            // Play battle end ME
            InGame.System.SongEffectPlay(InGame.System.BattleEndSongEffect);
            // Return to BGM before battle started
            InGame.System.SongPlay(InGame.Temp.MapSong);
            // Initialize EXP, amount of gold, and treasure
            int exp = 0;
            int gold = 0;
            List<Carriable> treasures = new List<Carriable>();
            // Loop
            foreach (GameNpc npc in InGame.Troops.Npcs)
            {
                // If npc is not hidden
                if (!npc.IsHidden)
                {
                    // Add EXP and amount of gold obtained
                    exp += npc.Exp;
                    gold += npc.Gold;
                    // Determine if treasure appears
                    if (InGame.Rnd.Next(100) < npc.TreasureProb)
                    {
                        if (npc.ItemId > 0)
                            treasures.Add(Data.Items[npc.ItemId]);
                        if (npc.WeaponId > 0)
                            treasures.Add(Data.Weapons[npc.WeaponId]);
                        if (npc.ArmorId > 0)
                            treasures.Add(Data.Armors[npc.ArmorId]);
                    }
                }
            }
            // Treasure is limited to a maximum of 6 items
            if (treasures.Count > 6)
            {
                for (int i = 6; i < treasures.Count; i++)
                {
                    treasures.RemoveAt(i);
                }
            }
            // Obtaining EXP
            for (int i = 0; i < InGame.Party.Actors.Count; i++)
            {
                GameActor actor = InGame.Party.Actors[i];
                if (!actor.IsCanGetExp)
                {
                    int last_level = actor.Level;
                    actor.Exp += exp;
                    if (actor.Level > last_level)
                    {
                        statusWindow.LevelUp(i);
                    }
                }
            }
            // Obtaining gold
            InGame.Party.GainGold(gold);
            // Obtaining treasure
            foreach (Carriable item in treasures)
            {
                switch (item.GetType().Name)
                {
                    case "Item":
                        InGame.Party.GainItem(item.Id, 1);
                        break;
                    case "Weapon":
                        InGame.Party.GainWeapon(item.Id, 1);
                        break;
                    case "Armor":
                        InGame.Party.GainArmor(item.Id, 1);
                        break;
                }
            }
            // Make battle result window
            resultWindow = new WindowBattleResult(exp, gold, treasures);
            // Set wait count
            phase5WaitCount = 100;
        }

        /// <summary>
        /// Frame Update (after battle phase)
        /// </summary>
        void UpdatePhase5()
        {
            // If wait count is larger than 0
            if (phase5WaitCount > 0)
            {
                // Decrease wait count
                phase5WaitCount -= 1;
                // If wait count reaches 0
                if (phase5WaitCount == 0)
                {
                    // Show result window
                    resultWindow.IsVisible = true;
                    // Clear main phase flag
                    InGame.Temp.BattleMainPhase = false;
                    // Refresh status window
                    statusWindow.Refresh();
                }
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Battle ends
                BattleEnd(0);
            }
        }

        #endregion

        #endregion
    }
}

