using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs Name input screen processing.
    /// </summary>
    public partial class SceneName : SceneBase
    {
        #region Variables

        /// <summary>
        /// Actor whose Name is edited
        /// </summary>
        GameActor actor;

        /// <summary>
        /// Managed edit window
        /// </summary>
        WindowNameEdit editWindow;

        /// <summary>
        /// Managed input window
        /// </summary>
        WindowNameInput inputWindow;

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize
        /// </summary>
        public override void LoadSceneContent()
        {
            // Get actor
            actor = InGame.Actors[InGame.Temp.NameActorId];
            // Make windows
            editWindow = new WindowNameEdit(actor, InGame.Temp.NameMaxChar);
            inputWindow = new WindowNameInput();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Prepare for Transition
            Graphics.Freeze();
            // Dispose of windows
            editWindow.Dispose();
            inputWindow.Dispose();
        }


        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            // If B button was pressed
            if (Input.RMRepeat.B)
            {
                // If cursor position is at 0
                if (editWindow.Index == 0)
                {
                    return;
                }
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Delete text
                editWindow.Back();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // If cursor position is at [OK]
                if (inputWindow.Character == null)
                {
                    // If Name is empty
                    if (editWindow.Name == "")
                    {
                        // Return to default Name
                        editWindow.RestoreDefault();
                        // If Name is empty
                        if (editWindow.Name == "")
                        {
                            // Play buzzer SE
                            InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                            return;
                        }
                        // Play decision SE
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        return;
                    }
                    // Change actor Name
                    actor.Name = editWindow.Name;
                    // Play decision SE
                    InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                    // Switch to map screen
                    Main.Scene = new SceneMap();
                    return;
                }
                // If cursor position is at maximum
                if (editWindow.Index == InGame.Temp.NameMaxChar)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // If text Character is empty
                if (inputWindow.Character == "")
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Add text Character
                editWindow.Add(inputWindow.Character);
                return;
            }
        }

        #endregion
    }
}
