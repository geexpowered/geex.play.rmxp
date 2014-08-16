using System;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Play.Custom;
using Geex.Play.Rpg.Spriteset;
using Geex.Play.Rpg.Window;
using Geex.Run;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Geex.Play.Rpg.Scene
{
    ///<summary>This class performs map screen processing.</summary>
    partial class SceneMap : SceneBase
    {
        #region Variables
        /// <summary>
        /// Determine what transfer phase currently running
        /// </summary>
        short transferPhase = 0;
        /// <summary>
        /// Transfer phases currently running
        /// </summary>
        bool isOperatingTransfer = false;

        /// <summary>
        /// Make sprite set
        /// </summary>
        SpritesetMap spriteset;

        /// <summary>
        /// Show interactive messages
        /// </summary>
        WindowMessage messageWindow;
        #endregion

        #region Debug
        bool debug;
        Sprite spriteVarDebug;
        List<int> debugVars = new List<int>();
        List<int> debugVarValues = new List<int>();
        List<int> debugVarOldValues = new List<int>();

        void InitializeDebug(int[] vars)
        {
            debug = true;
            // Initialize debug variables
            for (int j = 0; j < vars.Length; j++)
            {
                debugVars.Add(vars[j]);
            }
            // Init values
            foreach (int var in debugVars)
            {
                debugVarValues.Add(InGame.Variables.Arr[var]);
                debugVarOldValues.Add(InGame.Variables.Arr[var]);
            }
            // Display debug variables
            spriteVarDebug = new Sprite(Graphics.Background);
            spriteVarDebug.X = 100;
            spriteVarDebug.Y = 50;
            spriteVarDebug.Z = 1000;
            spriteVarDebug.Bitmap = new Bitmap(200, vars.Length * 30);
            spriteVarDebug.Bitmap.Font.Name = "Arial";
            int i = 0;
            foreach (int var in debugVars)
            {
                spriteVarDebug.Bitmap.DrawText(0, 30 * i, 200, 30, var.ToString() + ": " + InGame.Variables.Arr[var].ToString());
                i++;
            }
            spriteVarDebug.Visible = true;
        }

        void UpdateDebug()
        {
            int i = 0;

            // Update each variable display if needed
            foreach (int var in debugVars)
            {
                debugVarValues[i] = InGame.Variables.Arr[var];
                if (debugVarValues[i] != debugVarOldValues[i])
                {
                    debugVarOldValues[i] = debugVarValues[i];
                    spriteVarDebug.Bitmap.DrawText(0, i * 30, 200, 30, var.ToString() + ": " + InGame.Variables.Arr[var].ToString());
                }
                i++;
            }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize Scenemap - Automatically called when Scene Object is created
        /// </summary>
        public override void LoadSceneContent()
        {
            InGame.Tags = new Tags();
            spriteset = new SpritesetMap();
            messageWindow = new WindowMessage();
            // Pass to Game_system for reference
            InGame.Temp.MessageWindow = messageWindow;
            Graphics.Transition(60);
            InGame.Map.Update();
            if (Graphics.SplashTexture != null) Graphics.SplashTexture.Dispose();

            //InitializeDebug(new int[2]{2,3});
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose Scenemap
        /// <summary>
        public override void Dispose()
        {
            InGame.Tags.Dispose();
            // Dispose of sprite set
            spriteset.Dispose();
            // Dispose of message window
            messageWindow.Dispose();
            // If switching to title screen
            if (Main.Scene.GetType() == Type.GetType("SceneGameover"))
            {
                Graphics.Transition(240);
            }
            if (Main.Scene.GetType() == Type.GetType("SceneTitle"))
            {
                // Fade out screen
                Graphics.Transition();
            }
            if (Main.Scene.GetType() == Type.GetType("SceneTitle"))
            {
                // Fade out screen
                Graphics.Transition();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Scene Map Update
        /// </summary>
        public override void Update()
        {
            // Debug
            if (debug) { UpdateDebug(); }

            if (!messageWindow.IsVisible) UpdateMenuCall();
            if (!InGame.Temp.IsTransferringPlayer && !Graphics.IsTransitioning)
            {
                InGame.Tags.Update();
                InGame.Map.Update();
                InGame.Temp.MapInterpreter.Update();
                InGame.Player.Update();
                // Update system (Timer), screen
                InGame.System.Update();
                InGame.Screen.Update();
            }
            // Abort loop if player isn't place moving
            if (InGame.Temp.IsTransferringPlayer)
            {
                if (isOperatingTransfer)
                {
                    PlayerTransferring();
                }
                else
                {
                    transferPhase = 1;
                    isOperatingTransfer = true;
                    PlayerTransferring();
                }
                return;
            }
            // Update sprite set
            spriteset.Update();
            // Update message window
            messageWindow.Update();
            // If returning to title screen
            if (InGame.Temp.ToTitle)
            {
                // Change to title screen
                Main.Scene = new SceneTitle();
                return;
            }
            // If Transition processing
            if (InGame.Temp.IsProcessingTransition)
            {
                // Clear Transition processing flag
                InGame.Temp.IsProcessingTransition = false;
                // Execute Transition
                if (InGame.Temp.TransitionName == "") Graphics.Transition(20);
                else
                {
                    Graphics.Transition(40, InGame.Temp.TransitionName);
                }
            }
            // If showing message window
            if (InGame.Temp.IsMessageWindowShowing) return;
            // If encounter list isn't empty, and encounter count is 0
            if (InGame.Player.EncounterCount == 0 && InGame.Map.EncounterList.Length != 0)
            {
                // If event is running or encounter is not forbidden
                if (!(InGame.Temp.MapInterpreter.IsRunning || InGame.System.IsEncounterDisabled))
                {
                    // Confirm troop
                    Random random = new Random();
                    int n = random.Next(InGame.Map.EncounterList.Length);
                    int troop_id = InGame.Map.EncounterList[n];
                    // If troop is valid
                    if (Data.Npcs[troop_id] != null)
                    {
                        // Set battle calling flag
                        InGame.Temp.IsCallingBattle = true;
                        InGame.Temp.BattleTroopId = troop_id;
                        InGame.Temp.IsBattleCanEscape = true;
                        InGame.Temp.IsBattleCanLose = false;
                        InGame.Temp.BattleProc = null;
                    }
                }
            }
            UpdateCalls();
        }
        /// <summary>
        /// Apply transfer and transition
        /// </summary>
        void PlayerTransferring()
        {
            switch (transferPhase)
            {
                case 0: // transfer
                    return;
                case 1: // Fade out
                    Graphics.Transition(40, Data.System.BattleTransition);
                    transferPhase = 2;
                    TransferClean();
                    transferPhase = 3;
                    return;
                case 3:
                    if (!Graphics.IsTransitioning)
                    {
                        MapEdgeTransferPlayer();
                        ResetMap();
                        Graphics.SplashTexture = null;
                        Graphics.Transition(40, Data.System.BattleTransition);
                        transferPhase = 0;
                        isOperatingTransfer = false;
                        InGame.Temp.IsProcessingTransition = false;
                        InGame.Temp.IsTransferringPlayer = false;
                        return;
                    }
                    return;
            }
        }

        /// <summary>
        /// Operate transfer and clean SpritesetMap
        /// </summary>
        void TransferClean()
        {
            spriteset.Dispose();
        }

        /// <summary>
        /// Transfer the player to another map when he reches the edge of the map
        /// </summary>
        void MapEdgeTransferPlayer()
        {
            bool isEdgeTransfer = false;
            int dir = 0;
            if (InGame.Map.MapId != InGame.Temp.PlayerNewMapId)
            {
                InGame.Player.EdgeTransferList[2] = 0;
                InGame.Player.EdgeTransferList[4] = 0;
                InGame.Player.EdgeTransferList[6] = 0;
                InGame.Player.EdgeTransferList[8] = 0;
                dir = InGame.Player.EdgeTransferDirection;
                isEdgeTransfer = InGame.Player.IsEdgeTransferring;
                InGame.Player.IsEdgeTransferring = false;
                InGame.Player.EdgeTransferDirection = 0;
                // Clean Picture test
                if (GameOptions.IsDeletingPicturesAfterTransfer)
                {
                    foreach (GamePicture picture in InGame.Screen.Pictures) picture.Erase();
                }
                if (GameOptions.IsDeletingWeatherAfterTransfer)
                {
                    InGame.Screen.Weather(0, 0, 0);
                }
                // Clean Tone change
                if (!GameOptions.IsScreenToneCleanedAtEveryFrame) InGame.Screen.ColorTone = Tone.Empty;
                // Set up a new map
                CheckResetSelfSwitches();
                InGame.Map.Setup(InGame.Temp.PlayerNewMapId);
            }

            // Edge Transfer
            if (isEdgeTransfer)
            {
                switch (dir)
                {
                    case 2:
                        InGame.Temp.PlayerNewX = InGame.Player.X;
                        InGame.Temp.PlayerNewY = InGame.Player.CollisionHeight + 2;
                        break;
                    case 4:
                        InGame.Temp.PlayerNewX = InGame.Map.Width * 32 - (InGame.Player.CollisionWidth / 2 + 2);
                        InGame.Temp.PlayerNewY = InGame.Player.Y;
                        break;
                    case 6:
                        InGame.Temp.PlayerNewX = InGame.Player.CollisionWidth / 2 + 2;
                        InGame.Temp.PlayerNewY = InGame.Player.Y;
                        break;
                    case 8:
                        InGame.Temp.PlayerNewX = InGame.Player.X;
                        InGame.Temp.PlayerNewY = InGame.Map.Height * 32 - 2;
                        break;
                }
                InGame.Player.Moveto(InGame.Temp.PlayerNewX, InGame.Temp.PlayerNewY);

            }
            else
            {
                // Set up player position
                InGame.Player.Moveto(InGame.Temp.PlayerNewX * 32 + 16, InGame.Temp.PlayerNewY * 32 + 32);
            }
            // Set player direction
            switch (InGame.Temp.PlayerNewDirection)
            {
                case 2: // down
                    InGame.Player.TurnDown();
                    break;
                case 4: // left
                    InGame.Player.TurnLeft();
                    break;
                case 6: // right
                    InGame.Player.TurnRight();
                    break;
                case 8: // up
                    InGame.Player.TurnUp();
                    break;
            }
            // Straighten player position
            InGame.Player.Straighten();
        }

        /// <summary>
        /// Reset the new map
        /// </summary>
        void ResetMap()
        {
            spriteset = new SpritesetMap();
            // Update map (run parallel process event)
            InGame.Map.Update();
            // Run automatic change for BGM and BGS set on the map
            InGame.Map.Autoplay();
            // Clean the Garbage collector
            GC.Collect();
        }

        /// <summary>
        /// Update  Menu Call
        /// </summary>
        void UpdateMenuCall()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // If event is running, or menu is not forbidden
                if (!(InGame.Temp.MapInterpreter.IsRunning | InGame.System.IsMenuDisabled))
                {
                    // Set menu calling flag or beep flag
                    InGame.Temp.IsCallingMenu = true;
                    InGame.Temp.IsMenuBeep = true;
                }
            }
        }

        /// <summary>
        /// Transfer the player to another map
        /// </summary>
        public void TransferPlayer()
        {
            // Execute Transition
            InGame.Temp.IsProcessingTransition = false;
            // Clear player place move call flag
            InGame.Temp.IsTransferringPlayer = false;
            // If move destination is different than current map
            bool isEdgeTransfer = false;
            int dir = 0;
            if (InGame.Map.MapId != InGame.Temp.PlayerNewMapId)
            {
                InGame.Player.EdgeTransferList[2] = 0;
                InGame.Player.EdgeTransferList[4] = 0;
                InGame.Player.EdgeTransferList[6] = 0;
                InGame.Player.EdgeTransferList[8] = 0;
                dir = InGame.Player.EdgeTransferDirection;
                isEdgeTransfer = InGame.Player.IsEdgeTransferring;
                InGame.Player.IsEdgeTransferring = false;
                InGame.Player.EdgeTransferDirection = 0;
                CheckResetSelfSwitches();
                // Clean Picture test
                if (GameOptions.IsDeletingPicturesAfterTransfer)
                {
                    foreach (GamePicture picture in InGame.Screen.Pictures) picture.Erase();
                }
                if (GameOptions.IsDeletingWeatherAfterTransfer)
                {
                    InGame.Screen.Weather(0, 0, 0);
                }
                // Clean Tone change
                if (!GameOptions.IsScreenToneCleanedAtEveryFrame) InGame.Screen.ColorTone = Tone.Empty;
                // Set up a new map
                InGame.Map.Setup(InGame.Temp.PlayerNewMapId);
            }
            // Edge Transfer
            if (isEdgeTransfer)
            {
                switch (dir)
                {
                    case 2:
                        InGame.Temp.PlayerNewX = InGame.Player.X;
                        InGame.Temp.PlayerNewY = (GameOptions.GamePlayerHeight / 2 + 2);
                        break;
                    case 4:
                        InGame.Temp.PlayerNewX = InGame.Map.Width * 32 - (GameOptions.GamePlayerWidth / 2 + 2);
                        InGame.Temp.PlayerNewY = InGame.Player.Y;
                        break;
                    case 6:
                        InGame.Temp.PlayerNewX = (GameOptions.GamePlayerWidth / 2 + 2);
                        InGame.Temp.PlayerNewY = InGame.Player.Y;
                        break;
                    case 8:
                        InGame.Temp.PlayerNewX = InGame.Player.X;
                        InGame.Temp.PlayerNewY = InGame.Map.Height * 32 - 2;
                        break;
                }
                InGame.Player.Moveto(InGame.Temp.PlayerNewX, InGame.Temp.PlayerNewY - 1);

            }
            else
            {
                // Set up player position
                InGame.Player.Moveto(InGame.Temp.PlayerNewX * 32 + 16, InGame.Temp.PlayerNewY * 32 + 32 - 1);
            }
            // Set player direction
            switch (InGame.Temp.PlayerNewDirection)
            {
                case 2: // down
                    InGame.Player.TurnDown();
                    break;
                case 4: // left
                    InGame.Player.TurnLeft();
                    break;
                case 6: // right
                    InGame.Player.TurnRight();
                    break;
                case 8: // up
                    InGame.Player.TurnUp();
                    break;
            }
            // Straighten player position
            InGame.Player.Straighten();
            // Remake sprite set
            spriteset.Dispose();
            spriteset = new SpritesetMap();
            // Update map (run parallel process event)
            InGame.Map.Update();
            // Run automatic change for BGM and BGS set on the map
            InGame.Map.Autoplay();
        }

        /// <summary>
        /// Call Scene
        /// </summary>
        void UpdateCalls()
        {
            // If player is not moving
            if (!InGame.Player.IsMoving)
            {
                // Run calling of each screen
                if (InGame.Temp.IsCallingBattle) CallBattle();
                else if (InGame.Temp.IsCallingShop) CallShop();
                else if (InGame.Temp.IsCallingName) CallName();
                else if (InGame.Temp.IsCallingMenu) CallMenu();
                else if (InGame.Temp.IsCallingSave) CallSave();
            }
        }

        #region Calls
        /// <summary>
        /// Call  battle Scene
        /// </summary>
        void CallBattle()
        {
            // Clear battle calling flag
            InGame.Temp.IsCallingBattle = false;
            // Clear menu calling flag
            InGame.Temp.IsCallingMenu = false;
            InGame.Temp.IsMenuBeep = false;
            // Make encounter count
            InGame.Player.MakeEncounterCount();
            // Memorize map BGM and stop BGM
            InGame.Temp.MapSong = InGame.System.PlayingSong;
            Audio.SongStop();
            // Play battle start SE
            Audio.SoundEffectPlay(Data.System.BattleStartSoundEffect);
            // Play battle BGM
            Audio.SongPlay(InGame.System.BattleSong);
            // Straighten player position
            InGame.Player.Straighten();
            // Switch to battle screen
            Main.Scene = new SceneBattle();
        }

        /// <summary>
        /// Call  shop Scene
        /// </summary>
        void CallShop()
        {
            // Clear shop call flag
            InGame.Temp.IsCallingShop = false;
            // Straighten player position
            InGame.Player.Straighten();
            // Switch to shop screen
            Main.Scene = new SceneShop();
        }

        /// <summary>
        /// Call  Name input Scene
        /// </summary>
        void CallName()
        {
            // Clear Name input call flag
            InGame.Temp.IsCallingName = false;
            // Straighten player position
            InGame.Player.Straighten();
            // Switch to Name input screen
            Main.Scene = new SceneName();
        }

        /// <summary>
        /// Call  Menu Scene
        /// </summary>
        void CallMenu()
        {
            // Clear menu call flag
            InGame.Temp.IsCallingMenu = false;
            // If menu beep flag is set
            if (InGame.Temp.IsMenuBeep)
            {
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Clear menu beep flag
                InGame.Temp.IsMenuBeep = false;
            }
            // Straighten player position
            InGame.Player.Straighten();
            // Switch to menu screen
            Main.Scene = new SceneMenu();
        }

        /// <summary>
        /// Call  Scene save
        /// </summary>
        void CallSave()
        {
            // Straighten player position
            InGame.Player.Straighten();
            // Switch to save screen
            Main.Scene = new SceneSave();
        }
        #endregion

        /// <summary>
        /// Check if there are events whose self switches need to be reset
        /// </summary>
        void CheckResetSelfSwitches()
        {
            foreach (GameEvent ev in InGame.Map.Events)
            {
                if (ev != null && !ev.IsEmpty && ev.isResetSelfSwitches)
                {
                    foreach (GameSwitch key in InGame.System.GameSelfSwitches.Keys)
                    {
                        if (key.MapID == InGame.Map.MapId && key.EventID == ev.Id)
                        {
                            InGame.System.GameSelfSwitches[key] = false;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
