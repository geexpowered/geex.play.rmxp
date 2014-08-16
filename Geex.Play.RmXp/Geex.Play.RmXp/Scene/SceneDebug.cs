using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs debug screen processing.
    /// </summary>
    public partial class SceneDebug : SceneBase
    {
        #region Variables

        /// <summary>
        /// Managed left debug window
        /// </summary>
        WindowDebugLeft leftWindow;

        /// <summary>
        /// Managed right debug window
        /// </summary>
        WindowDebugRight rightWindow;

        /// <summary>
        /// Managed help window
        /// </summary>
        WindowBase helpWindow;

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize
        /// </summary>
        public override void LoadSceneContent()
        {
            InitializeWindows();
        }

        /// <summary>
        /// Windows initialization
        /// </summary>
        void InitializeWindows()
        {
            // Make windows
            leftWindow = new WindowDebugLeft();
            rightWindow = new WindowDebugRight();
            helpWindow = new WindowBase(192, 352, 448, 128);
            helpWindow.Contents = new Bitmap(406, 96);
            // Restore previously selected item
            leftWindow.TopRow = InGame.Temp.DebugTopRow;
            leftWindow.Index = InGame.Temp.DebugIndex;
            rightWindow.Mode = leftWindow.Mode;
            rightWindow.TopId = leftWindow.TopId;
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            // Refresh map
            InGame.Map.Refresh();
            // Prepare for Transition
            Graphics.Freeze();
            // Dispose of windows
            leftWindow.Dispose();
            rightWindow.Dispose();
            helpWindow.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            // Update windows
            rightWindow.Mode = leftWindow.Mode;
            rightWindow.TopId = leftWindow.TopId;
            leftWindow.Update();
            rightWindow.Update();
            // Memorize selected item
            InGame.Temp.DebugTopRow = leftWindow.TopRow;
            InGame.Temp.DebugIndex = leftWindow.Index;
            // If left window is active: call update_left
            if (leftWindow.IsActive)
            {
                UpdateLeft();
                return;
            }
            // If right window is active: call update_right
            else if (rightWindow.IsActive)
            {
                UpdateRight();
                return;
            }
        }

        /// <summary>
        /// Frame Update (when left window is active)
        /// </summary>
        void UpdateLeft()
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
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Display help
                string _text1 = "";
                if (leftWindow.Mode == 0)
                {
                    _text1 = "C (Enter) : ON / OFF";
                    helpWindow.Contents.DrawText(4, 0, 406, 32, _text1);
                }
                _text1 = "Left : -1   Right : +1";
                string _text2 = "L (Pageup) : -10";
                string _text3 = "R (Pagedown) : +10";
                helpWindow.Contents.DrawText(4, 0, 406, 32, _text1);
                helpWindow.Contents.DrawText(4, 32, 406, 32, _text2);
                helpWindow.Contents.DrawText(4, 64, 406, 32, _text3);
            }
            // Activate right window
            leftWindow.IsActive = false;
            rightWindow.IsActive = true;
            rightWindow.Index = 0;
            return;
        }

        /// <summary>
        /// Frame Update (when right window is active)
        /// </summary>
        void UpdateRight()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Activate left window
                leftWindow.IsActive = true;
                rightWindow.IsActive = false;
                rightWindow.Index = -1;
                // Erase help
                helpWindow.Contents.Clear();
                return;
            }
            // Get selected switch / variable ID
            int _current_id = rightWindow.TopId + rightWindow.Index;
            // If switch
            if (rightWindow.Mode == 0)
            {
                // If C button was pressed
                if (Input.RMTrigger.C)
                {
                    // Play decision SE
                    InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                    // Reverse ON / OFF
                    InGame.Switches.Arr[_current_id] = (!InGame.Switches.Arr[_current_id]);
                    rightWindow.Refresh();
                    return;
                }
            }
            // If variable
            if (rightWindow.Mode == 1)
            {
                // If right button was pressed
                if (Input.RMRepeat.Right)
                {
                    // Play cursor SE
                    InGame.System.SoundPlay(Data.System.CursorSoundEffect);
                    // Increase variables by 1
                    InGame.Variables.Arr[_current_id] += 1;
                    // Maximum limit check
                    if (InGame.Variables.Arr[_current_id] > 99999999)
                    {
                        InGame.Variables.Arr[_current_id] = 99999999;
                    }
                    rightWindow.Refresh();
                    return;
                }
                // If left button was pressed
                if (Input.RMRepeat.Left)
                {
                    // Play cursor SE
                    InGame.System.SoundPlay(Data.System.CursorSoundEffect);
                    // Decrease variables by 1
                    InGame.Variables.Arr[_current_id] -= 1;
                    // Minimum limit check
                    if (InGame.Variables.Arr[_current_id] < -99999999)
                    {
                        InGame.Variables.Arr[_current_id] = -99999999;
                    }
                    rightWindow.Refresh();
                    return;
                }
                // If R button was pressed
                if (Input.RMRepeat.R)
                {
                    // Play cursor SE
                    InGame.System.SoundPlay(Data.System.CursorSoundEffect);
                    // Increase variables by 10
                    InGame.Variables.Arr[_current_id] += 10;
                    // Maximum limit check
                    if (InGame.Variables.Arr[_current_id] > 99999999)
                    {
                        InGame.Variables.Arr[_current_id] = 99999999;
                    }
                    rightWindow.Refresh();
                    return;
                }
                // If L button was pressed
                if (Input.RMRepeat.L)
                {
                    // Play cursor SE
                    InGame.System.SoundPlay(Data.System.CursorSoundEffect);
                    // Decrease variables by 10
                    InGame.Variables.Arr[_current_id] -= 10;
                    // Minimum limit check
                    if (InGame.Variables.Arr[_current_id] < -99999999)
                    {
                        InGame.Variables.Arr[_current_id] = -99999999;
                    }
                    rightWindow.Refresh();
                    return;
                }
            }
        }

        #endregion
    }
}
