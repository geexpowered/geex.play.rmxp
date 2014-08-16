using Geex.Play.Make;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// GameEvent handles commands and event content including event page, switching via condition determinants, and running parallel process events.
    /// GameEvent is refreshed when visible on screen. To make it refresh at every frame you can add eloption:antilag in the event commands
    /// </summary>
    public partial class GameEvent : GameCharacter
    {
        #region Variables
        /// <summary>
        /// Current event page id
        /// </summary>
        int currentPageId = -1;
        /// <summary>
        /// localName of Event in the Editor
        /// </summary>
        public string EventName;
        /// <summary>
        /// True if particle is already triggered for this event
        /// </summary>
        public bool IsParticleTriggered = false;
        /// <summary>
        /// True if Event Self Switches must be reset after transfer
        /// </summary>
        public bool isResetSelfSwitches = false;

        /// <summary>
        /// True is Event Graphic is Visible
        /// </summary>
        public bool IsGraphicVisible = true;
        /// <summary>
        /// trigger
        /// </summary>
        public int Trigger;

        /// <summary>
        /// Event pages
        /// </summary>
        Event.Page[] pages;

        /// <summary>
        /// Event Command Interpreter
        /// </summary>
        Interpreter interpreter;
        #endregion

        #region Properties
        /// <summary>
        /// Determine if Over trigger
        /// (whether or not same position is starting condition)
        /// </summary>
        public bool IsOverTrigger
        {
            get
            {
                // If not through situation with Character as graphic
                if (CharacterName != "" && !Through)
                {
                    // Starting determinant is face
                    return false;
                }
                // If this position on the map is not passable
                if (!InGame.Map.IsPassable(X, Y, 0, this))
                {
                    // Starting determinant is face
                    return false;
                }
                // Starting determinant is same position
                return true;
            }
        }

        /// <summary>
        /// True is Event is Empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (Id == 0 || currentPageId == -1);
            }
        }


        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="_event">handled event</param>
        public GameEvent(Event _event)
            : base()
        {
            EventName = _event.Name;
            Id = _event.Id;
            pages = _event.Pages;
            isErased = false;
            IsStarting = false;
            Through = true;
            X = _event.X;
            Y = _event.Y;
            // Move to starting position
            Moveto(X * 32 + 16, Y * 32 + 32);
            Refresh();
        }
        /// <summary>
        /// Empty constructor mandatory for game saving
        /// </summary>
        public GameEvent()
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set up Geex Option and Geex Object
        /// </summary>
        public void CheckEventOptions()
        {
            int pageId = currentPageId == -1 ? 0 : currentPageId;
            if (this != null && pages[pageId].List != null)
            {
                int index = 0;
                while (index < pages[pageId].List.Length)
                {
                    EventCommand line = pages[pageId].List[index];
                    if (line != null)
                    {
                        // Check Geex Make Option or Object
                        if (line.Code == 356 || line.Code == 357)
                        {
                            // Create the Geex Make script
                            MakeCommand.Initialize(line.StringParams);
                            MakeCommand.MapId = InGame.Map.MapId;
                            MakeCommand.EventId = Id;
                            MakeCommand.Start();
                        }
                    }
                    index++;
                }
            }
        }
        /// <summary>
        /// Return the list of command of current page
        /// </summary>
        /// <returns></returns>
        public EventCommand[] List()
        {
            return pages[currentPageId].List;
        }
        /// <summary>
        /// Reset the type of an event page
        /// </summary>
        /// <param Name="pageId">page id to reset</param>
        /// <param Name="type">type to set</param>
        public void ResetType(int pageId, int type)
        {
            pages[pageId].Trigger = type;
            currentPageId = -1;//page=null;
            Refresh();
        }

        #region Methods - Refresh
        /// <summary>
        /// Refresh
        /// </summary>
        public override void Refresh()
        {
            int newPageId = RefreshNewPage();               // Get New Page
            if (IsRefreshPageChange(newPageId))  // Return if No Page Change
            {
                return;
            }
            ClearStarting();                            // Clear starting flag
            if (IsRefreshPageReset())             // Return if null Page Reset
            {
                return;
            }
            RefreshSetPage();                           // Set page variables
            RefreshCheckProcess();                      // Check parallel processing
            CheckEventTriggerAuto();                   // Auto event start determinant
        }

        /// <summary>
        /// Refresh : New Page
        /// </summary>
        int RefreshNewPage()
        {
            return isErased ? -1 : RefreshTriggerConditions();
        }

        /// <summary>
        /// Refresh trigger Conditions
        /// </summary>
        int RefreshTriggerConditions()
        {
            // Check in order of large event pages
            for (int i = pages.Length - 1; i >= 0; i--)
            {
                // Skips If Page Conditions Not Met
                if (!InGame.System.IsEventConditionsMet(InGame.Map.MapId, Id, pages[i].PageCondition))
                {
                    continue;
                }
                // Set local variable: new_page
                return i;

            }
            if (InGame.System.IsEventConditionsMet(InGame.Map.MapId, Id, pages[0].PageCondition))
            {
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// Refresh : Page Change
        /// </summary>
        bool IsRefreshPageChange(int newPageId)
        {
            // If event page is the same as last time
            if (newPageId == currentPageId)
            {
                // End method
                return true;
            }
            // Set page as current event page
            currentPageId = newPageId;
            return false;
        }

        /// <summary>
        /// Refresh : Page Reset
        /// </summary>
        bool IsRefreshPageReset()
        {
            // If no page fulfills conditions
            if (currentPageId == -1)
            {
                // Reset values
                RefreshReset();
                // End method
                return true;
            }
            return false;
        }

        /// <summary>
        /// Refresh Reset
        /// </summary>
        void RefreshReset()
        {
            // Set each instance variable
            TileId = 0;
            CharacterName = "";
            CharacterHue = 0;
            MoveType = 0;
            Through = true;
            Trigger = 0;
            //list = null;
            interpreter = null;
        }

        /// <summary>
        /// Refresh Set Page
        /// </summary>
        void RefreshSetPage()
        {
            // Set each instance variable
            TileId = pages[currentPageId].PageGraphic.TileId;
            CharacterName = pages[currentPageId].PageGraphic.CharacterName;
            CharacterHue = pages[currentPageId].PageGraphic.CharacterHue;
            if (OriginalDirection != pages[currentPageId].PageGraphic.Direction)
            {
                Dir = pages[currentPageId].PageGraphic.Direction;
                OriginalDirection = Dir;
                PrelockDirection = 0;
            }
            if (OriginalPattern != pages[currentPageId].PageGraphic.Pattern)
            {
                Pattern = pages[currentPageId].PageGraphic.Pattern;
                OriginalPattern = Pattern;
            }
            Opacity = pages[currentPageId].PageGraphic.Opacity;
            BlendType = pages[currentPageId].PageGraphic.BlendType;
            MoveType = pages[currentPageId].MoveType;
            MoveSpeed = pages[currentPageId].MoveSpeed;
            MoveFrequency = pages[currentPageId].MoveFrequency;
            MoveRoute = pages[currentPageId].MoveRoute;
            MoveRouteIndex = 0;
            MoveRouteForcing = false;
            IsWalkAnim = pages[currentPageId].WalkAnime;
            IsStepAnime = pages[currentPageId].StepAnime;
            IsDirectionFix = pages[currentPageId].DirectionFix;
            Through = pages[currentPageId].Through;
            IsAlwaysOnTop = pages[currentPageId].AlwaysOnTop;
            Trigger = pages[currentPageId].Trigger;
            //list = pages[currentPageId].List;
            interpreter = null;
        }

        /// <summary>
        /// Refresh Check Process
        /// </summary>
        public void RefreshCheckProcess()
        {
            // If trigger is [parallel process]
            if (Trigger == 4)
            {
                // Create parallel process interpreter
                interpreter = new Interpreter();
                interpreter.Setup(pages[currentPageId].List, Id);
            }
        }

        #endregion

        /// <summary>
        /// Clear Starting Flag
        /// </summary>
        public void ClearStarting()
        {
            IsStarting = false;
        }

        /// <summary>
        /// Start Event
        /// </summary>
        public void Start()
        {
            // If list of event commands is not empty
            if (!Locked && pages[currentPageId].List != null && pages[currentPageId].List.Length > 0)
            {
                IsStarting = true;
            }
        }

        /// <summary>
        /// Automatic Event Starting Determinant
        /// </summary>
        void CheckEventTriggerAuto()
        {
            // If trigger is [touch from event] and consistent with player coordinates
            if (Trigger == 2 && this.IsCollidingWithPlayer())
            {
                // If starting determinant other than jumping is same position event
                if (!IsJumping && IsOverTrigger)
                {
                    Start();
                }
            }
            // If trigger is [auto run]
            if (Trigger == 3)
            {
                Start();
            }
        }

        /// <summary>
        /// Touch Event Starting Determinant
        /// </summary>
        /// <returns>True if event start</returns>
        protected override bool CheckEventTriggerTouchMove(int newX, int newY)
        {
            // If event is running
            if (InGame.Temp.MapInterpreter.IsRunning) return false;
            // Default result
            bool result = false;
            // Event start
            if (this.Trigger == 2 && IsCollidingWithPlayer(newX, newY))
            {
                if(!this.IsJumping && !this.IsOverTrigger)
                {
                    this.Start();
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            // No Update if Event out of screen and Antilag flag is true
            if (!IsOnScreen && IsAntilag) return;
            RefreshUpdate();
        }

        /// <summary>
        /// Update Event without antilag check
        /// </summary>
        void RefreshUpdate()
        {
            // OR is Evetns locked by Message window
            if (InGame.Temp.MessageWindow != null)
            {
                if (InGame.Temp.MessageWindow.IsEventLocked) return;
            }
            //if (IsEmpty) return;
            base.Update();
            // Automatic event starting determinant
            CheckEventTriggerAuto();
            // If parallel process is valid
            if (interpreter != null)
            {
                // If not running
                if (!interpreter.IsRunning)
                {
                    // Set up event
                    //interpreter.SoundEffecttup(pages[currentPageId].List, Id);
                    interpreter.Reset(pages[currentPageId].List);
                }
                // Update interpreter
                interpreter.Update();
            }
        }
        #endregion
    }
}
