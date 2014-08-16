#region Usings
using System.Collections.Generic;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;
#endregion

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs menu screen processing.
    /// </summary>
    public class SceneMenu : SceneBase
    {
        #region Variables

        /// <summary>
        /// Menu index
        /// </summary>
        int menuIndex=0;

        /// <summary>
        /// Command window
        /// </summary>
        WindowCommand commandWindow;

        /// <summary>
        /// Playtime window
        /// </summary>
        WindowPlayTime playtimeWindow;

        /// <summary>
        /// Steps window
        /// </summary>
        WindowSteps stepsWindow;

        /// <summary>
        /// Gold window
        /// </summary>
        WindowGold goldWindow;

        /// <summary>
        /// Status window
        /// </summary>
        WindowMenuStatus statusWindow;

        #endregion

        #region Initialize
        /// <summary>
        /// Menu with index
        /// </summary>
        /// <param name="index"></param>
        public SceneMenu(int index): base()
        {
            menuIndex = index;
        }
        public SceneMenu() : base()
        {
        }
        /// <summary>
        /// Initialization (default : menu_index = 0)
        /// </summary>
        public override void LoadSceneContent()
        {
            InitializeWindow();
        }

        /// <summary>
        /// Initialize window
        /// </summary>
        void InitializeWindow()
        {
            // Make command window
            List<string> Commandlist = new List<string>();
            Commandlist.Add(Data.System.Wordings.Item);
            Commandlist.Add(Data.System.Wordings.Skill);
            Commandlist.Add(Data.System.Wordings.Equip);
            Commandlist.Add("Status");
            Commandlist.Add("Save");
            Commandlist.Add("End Game");
            commandWindow = new WindowCommand(GameOptions.MenuCommandListWidth, Commandlist);
            commandWindow.Index = menuIndex;
            // If number of party members is 0
            if (InGame.Party.Actors.Count == 0)
            {
                // Disable items, skills, equipment, and status
                commandWindow.DisableItem(0);
                commandWindow.DisableItem(1);
                commandWindow.DisableItem(2);
                commandWindow.DisableItem(3);
            }
            // If save is forbidden
            if (InGame.System.IsSaveDisabled)
            {
                // Disable save
                commandWindow.DisableItem(4);
            }
            // Make play time window
            playtimeWindow = new WindowPlayTime();
            // Make steps window
            stepsWindow = new WindowSteps();
            // Make gold window
            goldWindow = new WindowGold();
            // Make status window
            statusWindow = new WindowMenuStatus();

        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Prepare for transition
            //Graphics.Freeze();
            // Dispose of windows
            commandWindow.Dispose();
            playtimeWindow.Dispose();
            stepsWindow.Dispose();
            goldWindow.Dispose();
            statusWindow.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame update
        /// </summary>
        public override void Update()
        {
            // Update windows
            commandWindow.Update();
            playtimeWindow.Update();
            stepsWindow.Update();
            goldWindow.Update();
            statusWindow.Update();
            // If command window is active
            if (commandWindow.IsActive)
            {
                UpdateCommand();
                return;
            }
            // If status window is active
            else if (statusWindow.IsActive)
            {
                UpdateStatus();
                return;
            }
        }

        /// <summary>
        /// Frame Update (when command window is active)
        /// </summary>
        void UpdateCommand()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Switch to map screen
                Main.Scene = new SceneMap();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // If command other than save or end game, and party members = 0
                if (InGame.Party.Actors.Count == 0 && commandWindow.Index < 4)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Branch by command window cursor position
                switch (commandWindow.Index)
                {
                    case 0:  // item
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Switch to item screen
                        Main.Scene = new SceneItem();
                        break;
                    case 1:  // skill
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Make status window active
                        commandWindow.IsActive = false;
                        statusWindow.IsActive = true;
                        statusWindow.Index = 0;
                        break;
                    case 2:  // equipment
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Make status window active
                        commandWindow.IsActive = false;
                        statusWindow.IsActive = true;
                        statusWindow.Index = 0;
                        break;
                    case 3:  // status
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Make status window active
                        commandWindow.IsActive = false;
                        statusWindow.IsActive = true;
                        statusWindow.Index = 0;
                        break;
                    case 4:  // save
                        // If saving is forbidden
                        if (InGame.System.IsSaveDisabled)
                        {
                            // Play buzzer SE
                            InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                            return;
                        }
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Switch to save screen
                        Main.Scene = new SceneSave();
                        break;
                    case 5:  // end game
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Switch to end game screen
                        Main.Scene = new SceneEnd();
                        break;
                }
                return;
            }
        }

        /// <summary>
        /// Frame Update (when status window is active)
        /// </summary>
        void UpdateStatus()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Make command window active
                commandWindow.IsActive = true;
                statusWindow.IsActive = false;
                statusWindow.Index = -1;
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Branch by command window cursor position
                switch (commandWindow.Index)
                {
                    case 1:  // skill
                        // If this actor's action limit is 2 or more
                        if (InGame.Party.Actors[statusWindow.Index].Restriction >= 2)
                        {
                            // Play buzzer SE
                            InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                            return;
                        }
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Switch to skill screen
                        Main.Scene = new SceneSkill(statusWindow.Index);
                        break;
                    case 2:  // equipment
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Switch to equipment screen
                        Main.Scene = new SceneEquip(statusWindow.Index);
                        break;
                    case 3:  // status
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        // Switch to status screen
                        Main.Scene = new SceneStatus(statusWindow.Index);
                        break;
                }
                return;
            }
        }


        #endregion
    }
}
