using System;
using System.Collections.Generic;
using Geex.Edit;
using Geex.Run;
using Microsoft.Xna.Framework.Input;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class handles the player. Its functions include event starting determinants and map scrolling. Refer to "$game_player" for the one instance of this class
    /// </summary>
    public partial class GamePlayer : GameCharacter
    {
        #region Constants
        /// <summary>
        /// Number of pixel for slide around obstacle movement
        /// </summary>
        const int SLIDE_SENSITIVITY = 16;
        #endregion

        #region Variables
        /// <summary>
        /// Remember last Event Id touched
        /// </summary>
        int lastEventTouched = 0;
        /// <summary>
        /// Get Encounter Count
        /// </summary>
        public int EncounterCount;
        /// <summary>
        /// Remember Last player where player touches an avent
        /// </summary>
        int lastTouchX = 0;
        int lastTouchY= 0;
        /// <summary>
        /// Remember Last player position
        /// </summary>
        int lastRealX = 0;
        int lastRealY = 0;
        /// <summary>
        /// Set each map id transfer for each direction
        /// </summary>
        public short[] EdgeTransferList = new short[9];
        /// <summary>
        /// True if player is at the edge of a map and should transfer
        /// </summary>
        public bool IsEdgeTransferring = false;
        /// <summary>
        /// Direction to which transfer the player
        /// </summary>
        public int EdgeTransferDirection = 0;
		/// <summary>        
		/// When movement is tile by tile, counter to wait between input detection
        /// </summary>
        short waitingMoveCompletionCounter = 0;
        #endregion

        #region Properties
        /// <summary>
        /// Determine if Moving
        /// </summary>
        public new bool IsMoving
        {
            get
            {
                // If logical coordinates differ from real coordinates, movement is occurring.
                return Geex.Run.Input.Direction8 != Direction.None || Pad.LeftStickDir8 != Direction.None;
            }
        }
        /// <summary>
        /// Waiting for move completion
        /// </summary>
        bool IsWaitingMovingCompletion { get; set; }

        /// <summary>
        /// When move is Tile by Tile, number of frame to skip between input detection
        /// </summary>
        short WaitingMoveCompletionTime
        {
            get
            {
                if (MoveSpeed == 1)
                    return 32;
                else if (MoveSpeed == 2)
                    return 16;
                else if (MoveSpeed == 3)
                    return 11;
                else if (MoveSpeed == 4)
                    return 8;
                else if (MoveSpeed == 5)
                    return 6;
                else if (MoveSpeed == 6)
                    return 4;
                else
                    return 8;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new player
        /// </summary>
        public GamePlayer() : base()
        {
            CollisionWidth = GameOptions.GamePlayerWidth;
            CollisionHeight = GameOptions.GamePlayerHeight;
            EdgeTransferList[2] = 0;
            EdgeTransferList[4] = 0;
            EdgeTransferList[6] = 0;
            EdgeTransferList[8] = 0;
        }
        #endregion

        #region Movement
        /// <summary>
        /// Move Left
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        /// <param Name="slide">True if Player should slide around obstacle</param>
        public void MoveLeft(bool turn_enabled, short pixel, bool slide)
        {
            // Turn left
            if (turn_enabled) TurnLeft();
            // If passable
            int pass = Passable(X - pixel, Y, 4);
            if (pass == 2)
            {
                // Update coordinates
                X -= pixel;
                // Increase steps
                IncreaseSteps();
            }
            else 
            {
                // Event determinant is via touch of same position event
                CheckEventTriggerTouchMove();
            }
            if (pass==0)
            {
                if (!slide) return;
                if (Passable(X - pixel, Y - SLIDE_SENSITIVITY, 4)==2)
                {
                    MoveUp(false, pixel, false);
                }
                else if (Passable(X - pixel, Y + SLIDE_SENSITIVITY, 4)==2)
                {
                    MoveDown(false, pixel, false);
                }
            }
        }
        public new void MoveLeft(bool turn_enabled, short pixel)
        {
            MoveLeft(turn_enabled, pixel, GameOptions.IsSlidingIfColliding);
        }

        /// <summary>move right</summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        /// <param Name="slide">True if Player should slide around obstacle</param>
        public void MoveRight(bool turn_enabled, short pixel, bool slide)
        {
            // Turn right
            if (turn_enabled) TurnRight();
            // If passable
            int pass = Passable(X + pixel, Y, 6);
            if (pass == 2)
            {
                // Update coordinates
                X += pixel;
                // Increase steps
                IncreaseSteps();
            }
            else
            {
                // Event determinant is via touch of same position event
                CheckEventTriggerTouchMove();
            }
            if (pass==0)
            {
                if (!slide) return;
                if (Passable(X + pixel, Y - SLIDE_SENSITIVITY, 6)==2)
                {
                    MoveUp(false, pixel, false);
                }
                else if (Passable(X + pixel, Y + SLIDE_SENSITIVITY, 6)==2)
                {
                    MoveDown(false, pixel, false);
                }
            }
        }
        public new void MoveRight(bool turn_enabled, short pixel)
        {
            MoveRight(turn_enabled, pixel, GameOptions.IsSlidingIfColliding);
        }
        /// <summary>
        /// move up
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        /// <param Name="slide">True if Player should slide around obstacle</param>
        public void MoveUp(bool turn_enabled, short pixel, bool slide)

        {
            // Turn up
            if (turn_enabled) TurnUp();
            // If passable
            int pass = Passable(X, Y - pixel, 8);
            if (pass == 2)
            {
                // Update coordinates
                Y -= pixel;
                // Increase ste;ps
                IncreaseSteps();
            }
            else
            {
                // Event determinant is via touch of same position event
                CheckEventTriggerTouchMove();
            }
            if (pass==0)
            {
                if (!slide) return;
                if (Passable(X - SLIDE_SENSITIVITY, Y - pixel, 8)==2)
                {
                    MoveLeft(false, pixel, false);
                }
                else if (Passable(X + SLIDE_SENSITIVITY, Y - pixel, 8)==2)
                {
                    MoveRight(false, pixel, false);
                }
            }
        }
        public new void MoveUp(bool turn_enabled, short pixel)
        {
            MoveUp(turn_enabled, pixel, GameOptions.IsSlidingIfColliding);
        }

        /// <summary>
        /// Move Down
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        /// <param Name="slide">True if Player should slide around obstacle</param>
        public void MoveDown(bool turn_enabled, short pixel, bool slide)
        {
            // Turn down
            if (turn_enabled) TurnDown();
            // If passable
            int pass=Passable(X, Y+pixel, 2);
            if (pass == 2)
            {
                // Update coordinates
                Y += pixel;
                // Increase steps
                IncreaseSteps();
            }
            else
            {
                // Event determinant is via touch of same position event
                CheckEventTriggerTouchMove();
            }
            if (pass==0)
            {
                if (!slide) return;
                if (Passable(X - SLIDE_SENSITIVITY, Y + pixel, 2) == 2)
                {
                    MoveLeft(false, pixel, false);
                }
                else if (Passable(X + SLIDE_SENSITIVITY, Y + pixel, 2)==2)
                {
                    MoveRight(false, pixel, false);
                }
            }
        }
        public new void MoveDown(bool turn_enabled, short pixel)
        {
            MoveDown(turn_enabled, pixel, GameOptions.IsSlidingIfColliding);
        }

        /// <summary>
        /// Move Lower Left
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveLowerLeft(bool turn_enabled, short pixel)
        {
            if (turn_enabled) TurnLowerLeft();
            if (Dir == 8 || Dir == 2)
            {
                MoveLeft(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveDown(false, pixel, GameOptions.IsSlidingIfColliding);
            }
            else
            {
                MoveDown(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveLeft(false, pixel, GameOptions.IsSlidingIfColliding);
            }
        }

        /// <summary>
        /// Move Lower Right
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveLowerRight(bool turn_enabled, short pixel)
        {
            if (turn_enabled) TurnLowerRight();
            if (Dir == 8 || Dir == 2)
            {
                MoveRight(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveDown(false, pixel, GameOptions.IsSlidingIfColliding);
            }
            else
            {
                MoveDown(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveRight(false, pixel, GameOptions.IsSlidingIfColliding);
            }

        }

        /// <summary>
        /// Move Upper Left
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveUpperLeft(bool turn_enabled, short pixel)
        {
            if (turn_enabled) TurnUpperLeft();
            if (Dir == 8 || Dir == 2)
            {
                MoveLeft(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveUp(false, pixel, GameOptions.IsSlidingIfColliding);
            }
            else
            {
                MoveUp(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveLeft(false, pixel, GameOptions.IsSlidingIfColliding);
            }
        }

        /// <summary>
        /// Move Upper right
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveUpperRight(bool turn_enabled, short pixel)
        {
            if (turn_enabled) TurnUpperRight();
            if (Dir == 4 || Dir == 6)
            {
                MoveUp(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveRight(false, pixel, GameOptions.IsSlidingIfColliding);
            }
            else
            {
                MoveRight(false, pixel, GameOptions.IsSlidingIfColliding);
                MoveUp(false, pixel, GameOptions.IsSlidingIfColliding);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Passable Determinants
        /// </summary>
        /// <param Name="x">x-coordinate</param>
        /// <param Name="y">y-coordinate</param>
        /// <param Name="d">direction (0,2,4,6,8) 0 = Determines if all directions are impassable (for jumping)</param>
        public int Passable(int _x, int _y, int d)
        {
            // If coordinates are outside of map
            if (!InGame.Map.IsValid(_x, _y-1)) return 0;
            // Passable If debug mode is ON and ctrl key was pressed
            #if DEBUG
            if (Geex.Run.Input.IsPressed(Keys.RightControl) || Pad.IsPressed(Buttons.LeftShoulder)) return 2;
            #endif
            // If through is ON
            if (Through) return 2;
            // If Map is not passable
            if (!IsEventPassable(_x,_y,d)) return 1;
            if (!InGame.Map.IsPassable(_x, _y, d, this)) return 0;
            return 2;
        }

        /// <summary>
        /// Check if collides with some events
        /// </summary>
        bool IsEventPassable(int newX,int newY, int d)
        {
            // If collide with an avent
            foreach (short i in InGame.Map.EventKeysToUpdate)
            {
                // If event coordinates are consistent with move destination
                if (!InGame.Map.Events[i].Through && (InGame.Map.Events[i].CharacterName != "" || InGame.Map.Events[i].TileId > 0)
                    // and if event tile is null or not passable
                    && (InGame.Map.Events[i].TileId == 0 || !IsEventTilePassable(InGame.Map.Events[i].TileId)))
                {
                    if (IsCollidingWithEvent(InGame.Map.Events[i], newX, newY)) return false;
                }
            }
            // passable
            return true;
        }

        /// <summary>
        /// Check if event tile is passable
        /// </summary>
        /// <param name="tileId">tile id</param>
        /// <returns>true if event is passable</returns>
        bool IsEventTilePassable(int tileId)
        {
            if(GameOptions.IsCollisionMaskOn) return true;
            if (InGame.Map.Passages[tileId] == 0x0f) return false;
            return true;
        }

        /// <summary>
        /// Set Map Display Position to Center of Screen
        /// </summary>
        /// <param Name="x">x-coordinate</param>
        /// <param Name="y">y-coordinate</param>
        public void Center(int _x, int _y)
        {
            int max_x = (InGame.Map.Width - GeexEdit.GameMapWidth) * GeexEdit.TileSize;
            int max_y = (InGame.Map.Height - GeexEdit.GameMapHeight) * GeexEdit.TileSize;
            InGame.Map.DisplayX = Math.Max(0, Math.Min(_x - GeexEdit.GameWindowCenterX, max_x));
            InGame.Map.DisplayY = Math.Max(0, Math.Min(_y - GeexEdit.GameWindowCenterY, max_y));
        }

        /// <summary>
        /// Move to Designated Position
        /// </summary>
        /// <param Name="x">x-coordinate</param>
        /// <param Name="y">y-coordinate</param>
        public override void Moveto(int _x, int _y)
        {
            base.Moveto(_x, _y);
            // Centering
            Center(_x, _y);
            // Make encounter count
            MakeEncounterCount();
        }

        /// <summary>
        /// Increaase Steps
        /// </summary>
        public override void IncreaseSteps()
        {
            base.IncreaseSteps();
            // If move route is not forcing
            if (!MoveRouteForcing)
            {
                // Increase steps
                InGame.Party.IncreaseSteps();
                // Number of steps are an even number
                if (InGame.Party.Steps % 2 == 0)
                {
                    // Slip damage check
                    InGame.Party.CheckMapSlipDamage();
                }
            }
        }

        /// <summary>
        /// Make Encounter Count
        /// </summary>
        public void MakeEncounterCount()
        {
            // Image of two dice rolling
            if (InGame.Map.MapId != 0)
            {
                int n = InGame.Map.EncounterStep;
                EncounterCount = InGame.Rnd.Next(n) + InGame.Rnd.Next(n) + 1;
            }
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public override void Refresh()
        {
            if (InGame.Party.Actors.Count == 0)
            {
                // Clear Character file Name and hue
                CharacterName = "";
                CharacterHue = 0;
                return;
            }
            // Get lead actor
            GameActor actor = InGame.Party.Actors[0];
            // Set Character file Name and hue
            CharacterName = actor.CharacterName;
            CharacterHue = actor.CharacterHue;
            // Initialize opacity level and blending method
            Opacity = 255;
            BlendType = 0;
        }

        #region Methods - Check Event
        /// <summary>
        /// Same Position Starting Determinant
        /// </summary>
        /// <param Name="triggers">List of checked triggers</param>
        /// <param Name="waitMoveCompletion">True if event starts after move completion</param>
        /// <returns>True if event starts</returns>
        public bool CheckEventTriggerHere(List<int> triggers, bool waitMoveCompletion)
        {
            if (waitMoveCompletion)
                InGame.Temp.MapInterpreter.Wait(3);
            bool result = false;
            // If event is running
            if (InGame.Temp.MapInterpreter.IsRunning) return result;
            // All event loops
            foreach (short i in InGame.Map.EventKeysToUpdate)
            {
                // If event coordinates and triggers are consistent
                if (triggers.Contains(InGame.Map.Events[i].Trigger) && InGame.Map.Events[i].IsCollidingWithPlayer())
                {
                    // If starting determinant is same position event (other than jumping)
                    if (!InGame.Map.Events[i].IsJumping && InGame.Map.Events[i].IsOverTrigger)
                    {
                        InGame.Map.Events[i].Start();
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Same Position Starting Determinant, don't wait for move completion
        /// </summary>
        /// <param name="triggers">List of checked triggers</param>
        /// <returns>True if event starts</returns>
        public bool CheckEventTriggerHere(List<int> triggers)
        {
            return CheckEventTriggerHere(triggers, false);
        }

        /// <summary>
        /// Front Event Starting Determinant
        /// </summary>
        /// <param Name="triggers">List of Triggers</param>
        public bool CheckEventTriggerThere(List<int> triggers)
        {
            bool result = false;
            // If event is running
            if (InGame.Temp.MapInterpreter.IsRunning) return result;
            // Calculate front event coordinates
            short distance = 0;
            if (GameOptions.IsTileByTileMoving)
            {
                distance = GeexEdit.TileSize;
            }
            else
            {
                distance = InGame.System.Speed[MoveSpeed];
            }
            int new_x = X + (Dir == 6 ? distance : Dir == 4 ? -distance : 0);
            int new_y = Y + (Dir == 2 ? distance : Dir == 8 ? -distance : 0);
            // All event loops
            foreach(short i in InGame.Map.EventKeysToUpdate)
            {
                // If event coordinates and triggers are consistent
                if  (triggers.Contains(InGame.Map.Events[i].Trigger) && IsCollidingWithEvent(InGame.Map.Events[i],new_x,new_y) )
                {
                    // If starting determinant is front event (other than jumping) 
                    if (!InGame.Map.Events[i].IsJumping
                        // and if this event has no character or his tile is not passable or is not passable
                        && !(InGame.Map.Events[i].CharacterName == "" && IsEventTilePassable(InGame.Map.Events[i].TileId) && InGame.Map.IsPassable(new_x, new_y, Dir, this)))
                    {
                        InGame.Map.Events[i].Start();
                        result = true;
                    }
                }
            }
            

            // If fitting event is not found
            if (result == false)
            {
                // If front Tile is a counter
                if (InGame.Map.IsCounter(new_x,new_y))
                {
                    // Calculate 1 Tile inside coordinates
                    new_x += (Dir == 6 ? GeexEdit.TileSize : Dir == 4 ? -GeexEdit.TileSize : 0);
                    new_y += (Dir == 2 ? GeexEdit.TileSize : Dir == 8 ? -GeexEdit.TileSize : 0);
                    // All event loops
                    foreach (short i in InGame.Map.EventKeysToUpdate)
                    {
                        // If event coordinates and triggers are consistent
                        if ( triggers.Contains(InGame.Map.Events[i].Trigger) && IsCollidingWithEvent(InGame.Map.Events[i],new_x,new_y))
                        {
                            // If starting determinant is front event (other than jumping)
                            if (!InGame.Map.Events[i].IsJumping & !InGame.Map.Events[i].IsOverTrigger)
                            {
                                InGame.Map.Events[i].Start();
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Touch Event Starting Determinant
        /// </summary>
        /// <returns>True if event start</returns>
        bool CheckEventTriggerTouchMove()
        {
            if (InGame.Temp.MapInterpreter.IsRunning) return false;
            int newX = X;
            int newY = Y;
            // Check front position
            int distance = (GameOptions.IsTileByTileMoving ? 32 : InGame.System.Speed[MoveSpeed]);
            switch (Dir)
            {
                case 2:
                    newY += distance;
                    break;
                case 4:
                    newX -= distance;
                    break;
                case 6:
                    newX += distance;
                    break;
                case 8:
                    newY -= distance;
                    break;
            }
            bool result = false;
            // If event is running
            if (InGame.Temp.MapInterpreter.IsRunning) return result;
            // All event loops
            foreach (short i in InGame.Map.EventKeysToUpdate)
            {
                //if (InGame.Map.Events[i].Id == lastEventTouched) continue;
                if (InGame.Map.Events[i].Through || (InGame.Map.Events[i].Trigger != 1 && InGame.Map.Events[i].Trigger != 2 && InGame.Map.Events[i].CharacterName == "" && (InGame.Map.Events[i].TileId == 0 || InGame.Map.Events[i].TileId == null))) continue;
                // If event coordinates and triggers are consistent
                if ((InGame.Map.Events[i].Trigger == 1 | InGame.Map.Events[i].Trigger == 2) && IsCollidingWithEvent(InGame.Map.Events[i], newX, newY))
                {
                    // If starting determinant is front event (other than jumping)
                    if (!InGame.Map.Events[i].IsJumping && !InGame.Map.Events[i].IsOverTrigger)
                    {
                        lastEventTouched = i;
                        InGame.Map.Events[i].Start();
                        result = true;
                    }
                }
            }
            return result;
        }
        #endregion

        #region Methods - Update
        /// <summary>
        /// Updade Player move
        /// </summary>
        public new void UpdatePlayerMove()
        {
            if (!IsLocked)
            {
                short distance = 0;
                //
                if (GameOptions.IsTileByTileMoving)
                {
                    distance = GeexEdit.TileSize;
                }
                else
                {
                    // If moving, event running, move route forcing, and message window
                    // display are all not occurring
                    if (Pad.LeftStickPower != 0 && Pad.LeftStickPower < 0.75f && !MoveRouteForcing && MoveSpeed > 1)
                    {
                        distance = InGame.System.Speed[MoveSpeed - 2];
                    }
                    else
                    {
                        distance = InGame.System.Speed[MoveSpeed];
                    }
                }
                // Move player in the direction the directional button/pad is being pressed
                Direction dir = Geex.Run.Input.Direction8;
                Direction dirPad = Pad.LeftStickDir8;
                if (!(InGame.Temp.MapInterpreter.IsRunning || MoveRouteForcing || InGame.Temp.IsMessageWindowShowing))
                {
                    if (GameOptions.IsTileByTileMoving)
                    {
                        if (!IsWaitingMovingCompletion)
                        {
                            if (dir == Direction.Down || dirPad == Direction.Down)
                            {
                                MoveDown(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                            if (dir == Direction.Left || dirPad == Direction.Left)
                            {
                                MoveLeft(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                            if (dir == Direction.Right || dirPad == Direction.Right)
                            {
                                MoveRight(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                            if (dir == Direction.Up || dirPad == Direction.Up)
                            {
                                MoveUp(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                            if (dir == Direction.LowerLeft || dirPad == Direction.LowerLeft)
                            {
                                MoveLowerLeft(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                            if (dir == Direction.LowerRight || dirPad == Direction.LowerRight)
                            {
                                MoveLowerRight(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                            if (dir == Direction.UpperLeft || dirPad == Direction.UpperLeft)
                            {
                                MoveUpperLeft(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                            if (dir == Direction.UpperRight || dirPad == Direction.UpperRight)
                            {
                                MoveUpperRight(!IsDirectionFix, distance);
                                IsWaitingMovingCompletion = true;
                            }
                        }
                        if (IsWaitingMovingCompletion)
                        {
                            waitingMoveCompletionCounter++;
                            if (waitingMoveCompletionCounter >= WaitingMoveCompletionTime)
                            {
                                waitingMoveCompletionCounter = 0;
                                IsWaitingMovingCompletion = false;
                            }
                        }
                    }
                    else
                    {
                        if (dir == Direction.Down || dirPad == Direction.Down) MoveDown(!IsDirectionFix, distance);
                        if (dir == Direction.Left || dirPad == Direction.Left) MoveLeft(!IsDirectionFix, distance);
                        if (dir == Direction.Right || dirPad == Direction.Right) MoveRight(!IsDirectionFix, distance);
                        if (dir == Direction.Up || dirPad == Direction.Up) MoveUp(!IsDirectionFix, distance);
                        if (dir == Direction.LowerLeft || dirPad == Direction.LowerLeft) MoveLowerLeft(!IsDirectionFix, distance);
                        if (dir == Direction.LowerRight || dirPad == Direction.LowerRight) MoveLowerRight(!IsDirectionFix, distance);
                        if (dir == Direction.UpperLeft || dirPad == Direction.UpperLeft) MoveUpperLeft(!IsDirectionFix, distance);
                        if (dir == Direction.UpperRight || dirPad == Direction.UpperRight) MoveUpperRight(!IsDirectionFix, distance);
                    }
                }
            }
            if (RealY > lastRealY && RealY - InGame.Map.DisplayY > GeexEdit.GameWindowCenterY) InGame.Map.ScrollDown(RealY - lastRealY);
            // If Character moves left and is positioned more let on-screen than center
            if (RealX < lastRealX && RealX - InGame.Map.DisplayX < GeexEdit.GameWindowCenterX) InGame.Map.ScrollLeft(lastRealX - RealX);
            // If Character moves right and is positioned more right on-screen than center
            if (RealX > lastRealX && RealX - InGame.Map.DisplayX > GeexEdit.GameWindowCenterX) InGame.Map.ScrollRight(RealX - lastRealX);
            // If Character moves up and is positioned higher than the center of the screen
            if (RealY < lastRealY && RealY - InGame.Map.DisplayY < GeexEdit.GameWindowCenterY) InGame.Map.ScrollUp(lastRealY - RealY);
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            lastRealX = RealX;
            lastRealY = RealY;
            base.Update();
            UpdatePlayerMove();
            // Check to avoid repeated starting event 
            if (lastTouchX != X / GeexEdit.TileSize || lastTouchY != Y / GeexEdit.TileSize)
            {
                lastEventTouched = 0;
                lastTouchX = X / GeexEdit.TileSize;
                lastTouchY = Y / GeexEdit.TileSize;
                // Event determinant is via touch of same position event
                if (!CheckEventTriggerHere(new List<int>() { 1, 2 }, true)) 
                {
                    // Encounter countdown
                    if (EncounterCount > 0) EncounterCount -= 1;
                }
            }
            // If C button was pressed (ESCAPE or A button)
            if (Geex.Run.Input.RMTrigger.C || Pad.IsTriggered(Buttons.A))
            {
                // Same position and front event determinant
                CheckEventTriggerHere(new List<int>() { 0 });
                CheckEventTriggerThere(new List<int>() { 0, 1, 2 });
            }
            // test if on the edge of a Map
            if (EdgeTransferList[Dir] != 0) ApplyTransfer();
        }
        #endregion

        /// <summary>
        /// Apply the right transfer to player
        /// </summary>
        void ApplyTransfer()
        {
            if (X >= InGame.Map.Width * GeexEdit.TileSize - GameOptions.GamePlayerWidth * 2 && Dir == 6) SetTransfer(6);
            if (Y >= InGame.Map.Height * GeexEdit.TileSize - GameOptions.GamePlayerHeight * 2 && Dir == 2) SetTransfer(2);
            if (Y <= GameOptions.GamePlayerHeight * 2 && Dir == 8) SetTransfer(8);
            if (X <= GameOptions.GamePlayerWidth * 2 && Dir == 4) SetTransfer(4);
        }

        /// <summary>
        /// Set all transfer value
        /// </summary>
        /// <param Name="dir">Transfer direction</param>
        void SetTransfer(int dir)
        {
            IsEdgeTransferring=true;
            EdgeTransferDirection=dir;
            InGame.Temp.IsTransferringPlayer =true;
            InGame.Temp.PlayerNewMapId=EdgeTransferList[dir];
            InGame.Temp.IsProcessingTransition = true;
        }
        #endregion
    }
}