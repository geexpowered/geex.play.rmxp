using Geex.Play.Make;
using Geex.Run;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This window displays buyable goods on the shop screen.
    /// </summary>
    public partial class GameCommonEvent
    {
        #region Variables
        /// <summary>
        /// Common event ID
        /// </summary>
        int commonEventId;

        /// <summary>
        /// Associated interpreter
        /// </summary>
        Interpreter interpreter;

        #endregion

        #region Properties
        /// <summary>
        /// Get IsTriggered
        /// </summary>
        int IsTrigger
        {
            get
            {
                return Data.CommonEvents[commonEventId].Trigger;
            }
        }

        /// <summary>
        /// Get Condition Switch ID
        /// </summary>
        int SwitchId
        {
            get
            {
                return Data.CommonEvents[commonEventId].SwitchId;
            }
        }

        /// <summary>
        /// Get List of Event Commands
        /// </summary>
        EventCommand[] list
        {
            get
            {
                return Data.CommonEvents[commonEventId].List;
            }
        }
        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="common_event_id">common event ID</param>
        public GameCommonEvent(int id)
        {
            commonEventId = id;
            interpreter = null;
            Refresh();
        }
        /// <summary>
        /// Empty constructor mandatory for game saving
        /// </summary>
        public GameCommonEvent()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            // Create an interpreter for parallel process if necessary
            if (IsTrigger == 2 && InGame.Switches.Arr[SwitchId])
            {
                if (interpreter == null)
                {
                    interpreter = new Interpreter();
                    interpreter.Setup(list, 0);
                }
            }
            else
            {
                interpreter = null;
            }
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            // If parallel process is valid
            if (interpreter != null)
            {
                // If not running
                if (!interpreter.IsRunning)
                {
                    // Set up event
                    interpreter.Reset(list);
                    //interpreter.SoundEffecttup(list, 0);
                }
                // Update interpreter
                interpreter.Update();
            }
        }

        #endregion
    }
}
