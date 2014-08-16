using System;
using System.Collections.Generic;
using Geex.Edit;
using Geex.Play.Custom;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class deals with characters. It's used as a superclass for the GamePlayer and GameEvent classes
    /// </summary>
    public partial class GameCharacter
    {
        #region Variables
        /// <summary>
        /// Geex Effect for this picture
        /// </summary>
        public GeexEffect GeexEffect = new GeexEffect();

        /// <summary>
        /// el:animation variable to set Animation Priority 7 = above all
        /// </summary>
        public int AnimationPriority = 7;
        /// <summary>
        /// el:animation variable to set Animation pause in between 2 frame
        /// </summary>
        public int AnimationPause = 0;
        /// <summary>
        /// el:animation variable to set Animation zoom
        /// </summary>
        public int AnimationZoom = 0;
        /// <summary>
        /// Priority adjustment
        /// </summary>
        public int Priority = 0;
        /// <summary>
        /// Character X zoom
        /// </summary>
        public float ZoomX = 1f;
        /// <summary>
        /// Character Y zoom
        /// </summary>
        public float ZoomY = 1f;
        /// <summary>
        /// Character rotation
        /// </summary>
        public int Angle = 0;
        /// <summary>
        /// Reference to a Tag to be display with Character
        /// </summary>
        public Tag Tag = null;

        /// <summary>
        /// True is Antilag does apply, the event is not updated if out of screen
        /// </summary>
        public bool IsAntilag = GameOptions.IsAntilagOnByDefault;
        
        /// <summary>
        /// ID
        /// </summary>
        public int Id;
        /// <summary>
        /// Destination x-coordinate on map
        /// </summary>
        public int X;
        /// <summary>
        /// Destination y-coordinate on map
        /// </summary>
        public int Y;
        /// <summary>
        /// real x-coordinate (real)
        /// </summary>
        protected int RealX;
        /// <summary>
        /// real y-coordinate (real)
        /// </summary>
        protected int RealY;
        /// <summary>
        /// Tile ID (invalid if 0)
        /// </summary>
        public int TileId;
        /// <summary>
        /// Character file Name
        /// </summary>
        public string CharacterName;
        /// <summary>
        /// Character hue
        /// </summary>
        public int CharacterHue;
        /// <summary>
        /// opacity level
        /// </summary>
        public byte Opacity;
        /// <summary>
        /// blending method
        /// </summary>
        public int BlendType;
        /// <summary>
        /// direction
        /// </summary>
        public int Dir;
        /// <summary>
        /// Fix direction flag
        /// </summary>
        public bool IsDirectionFix;
        /// <summary>
        /// Lock flag
        /// </summary>
        public bool Locked;
        /// <summary>
        /// Character graphics pattern
        /// </summary>
        public int Pattern;
        /// <summary>
        /// forced move route flag
        /// </summary>
        public bool MoveRouteForcing;
        /// <summary>
        /// Ghost mode
        /// </summary>
        public bool Through;
        /// <summary>
        /// Character current Animation ID
        /// </summary>
        public int AnimationId;
        /// <summary>
        /// transparent flag
        /// </summary>
        public bool IsTransparent;
        /// <summary>
        /// Character move speed
        /// </summary>
        public int MoveSpeed;
        /// <summary>
        /// event starting flag
        /// </summary>
        public bool IsStarting;
        /// <summary>
        /// Character Width from its Graphic
        /// </summary>
        public int Cw = 0;
        /// <summary>
        /// Character Height from its Graphic
        /// </summary>
        public int Ch = 0;

        /// <summary>
        /// Character Size width
        /// </summary>
        public int CollisionWidth = 32;
        /// <summary>
        /// Character Size height
        /// </summary>
        public int CollisionHeight = 32;
        /// <summary>
        /// Temporarily Erased flag
        /// </summary>
        protected bool isErased;

        protected int WaitCount;
        protected int OriginalDirection;
        protected int OriginalPattern;
        /// <summary>
        /// Character move type 0: Stand still 1:Random moves 2:Follow hero 3: Move Route
        /// </summary>
        public int MoveType;
        protected int MoveFrequency;
        protected MoveRoute MoveRoute = new MoveRoute();
        protected int MoveRouteIndex;
        protected MoveRoute OriginalMoveRoute;
        protected int OriginalMoveRouteIndex;
        protected bool IsWalkAnim;
        protected bool IsStepAnime;
        protected bool IsAlwaysOnTop;
        protected float AnimeCount;
        protected int StopCount;
        protected int JumpCount;
        protected int JumpPeak;
        protected int PrelockDirection;
        #endregion

        #region Properties
        /// <summary>
        /// True if Character is erased
        /// </summary>
        public bool IsErased
        {
            get
            {
                return isErased;
            }
        }

        /// <summary>
        /// True if is passable in front of Game Character
        /// </summary>
        public bool IsFrontPassable
        {
            get
            {
                int offset = 2;
                //if (!GameOptions.IsCollisionMaskOn)
                    //offset = GeexEdit.TileSize;
                switch (Dir)
                {
                    case 2:
                        return InGame.Map.IsPassable(X, Y + offset, 2, this);
                    case 4:
                        return InGame.Map.IsPassable(X - offset, Y, 4, this);
                    case 6:
                        return InGame.Map.IsPassable(X + offset, Y, 6, this);
                    case 8:
                        return InGame.Map.IsPassable(X, Y - offset, 8, this);
                    default:
                        return InGame.Map.IsPassable(X, Y, Dir, this);
                }
            }
        }

        /// <summary>
        /// True if Event is Visible on screen
        /// </summary>
        public bool IsOnScreen
        {
            get
            {
                int width =  Math.Max(Cw,CollisionWidth);
                int height = Math.Max(Ch,CollisionHeight);
                return (ScreenX >= -GeexEdit.TileSize - width && ScreenX <= GeexEdit.GameWindowWidth + 32 + width && ScreenY < GeexEdit.GameWindowHeight + GeexEdit.TileSize + height && ScreenY >= -GeexEdit.TileSize-height);
            }
        }
        
        /// <summary>
        /// Determine if Jumping
        /// </summary>
        public bool IsJumping
        {
            get
            {
                // A jump is occurring if jump count is larger than 0
                return (JumpCount > 0);
            }
        }

        /// <summary>
        /// Determine if Moving
        /// </summary>
        public bool IsMoving
        {
            get
            {
                // If logical coordinates differ from real coordinates, movement is occurring.
                return (RealX != X | RealY != Y );
            }
        }

        /// <summary>
        /// Determine if Movement is locked
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return Locked;
            }
            set
            {
                Locked = value;
            }
        }

        /// <summary>
        /// Get Screen X-Coordinates
        /// </summary>
        public int ScreenX
        {
            get
            {
                // Get screen coordinates from real coordinates and map display position
                return RealX - InGame.Map.DisplayX;
            }
        }

        /// <summary>
        /// Get Screen Y-Coordinates
        /// </summary>
        public int ScreenY
        {
            get
            {
                int n;
                // Get screen coordinates from real coordinates and map display position
                int _y = RealY - InGame.Map.DisplayY;
                // Make y-coordinate smaller via jump count
                if (JumpCount >= JumpPeak)
                {
                    n = JumpCount - JumpPeak;
                }
                else
                {
                    n = JumpPeak - JumpCount;
                }
                return _y - (JumpPeak * JumpPeak - n * n) / 2;
            }
        }

        /// <summary>
        /// Get Thicket Depth
        /// </summary>
        public int BushDepth
        {
            get
            {
                // If Tile, or if display flag on the closest surface is ON
                if (TileId > 0 | IsAlwaysOnTop) return 0;
                // If element Tile other than jumping, then 12; anything else = 0
                if (JumpCount == 0 & InGame.Map.IsBush(X, Y))
                {
                    return 12;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Get Terrain Tag
        /// </summary>
        public int TerrainTag
        {
            get
            {
                return InGame.Map.TerrainTag(X, Y);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Character
        /// </summary>
        public GameCharacter()
        {
            Id = 0;
            X = 0;
            Y = 0;
            RealX = 0;
            RealY = 0;
            TileId = 0;
            CharacterName = "";
            CharacterHue = 0;
            Opacity = 255;
            BlendType = 0;
            Dir = 2;
            Pattern = 0;
            MoveRouteForcing = false;
            Through = false;
            AnimationId = 0;
            IsTransparent = false;
            OriginalDirection = 2;
            OriginalPattern = 0;
            MoveType = 0;
            MoveSpeed = 4;
            MoveFrequency = 6;
            MoveRoute = null;
            MoveRouteIndex = 0;
            OriginalMoveRoute = null;
            OriginalMoveRouteIndex = 0;
            IsWalkAnim = true;
            IsStepAnime = false;
            IsDirectionFix = false;
            IsAlwaysOnTop = false;
            AnimeCount = 0;
            StopCount = 0;
            JumpCount = 0;
            JumpPeak = 0;
            WaitCount = 0;
            Locked = false;
            PrelockDirection = 0;
        }
        #endregion

        #region Methods
        #region Methods - General
        /// <summary>
        /// Force Move Route
        /// </summary>
        /// <param Name="_move_route">new move route</param>
        public void ForceMoveRoute(MoveRoute _move_route)
        {
            // Save original move route
            if (OriginalMoveRoute == null)
            {
                OriginalMoveRoute = MoveRoute;
                OriginalMoveRouteIndex = MoveRouteIndex;
            }
            // Change move route
            MoveRoute = _move_route;
            MoveRouteIndex = 0;
            // Set forced move route flag
            MoveRouteForcing = true;
            // Clear prelock direction
            PrelockDirection = 0;
            // Clear wait count
            WaitCount = 0;
            // Move cutsom
            MoveTypeCustom();
         }
        /// <summary>
        /// Straighten Position
        /// </summary>
        public void Straighten()
        {
            // If moving Animation or stop Animation is ON
            if (IsWalkAnim | IsStepAnime)
            {
                // Set pattern to 0
                Pattern = 0;
            }
            // Clear Animation count
            AnimeCount = 0;
            // Clear prelock direction
            PrelockDirection = 0;
        }

        /// <summary>
        /// Determine if Passable
        /// </summary>
        /// <param Name="newX">x-coordinate</param>
        /// <param Name="newY">y-coordinate</param>
        /// <param Name="d">direction (0,2,4,6,8)</param>
        public virtual bool IsPassable(int newX, int newY, int d, bool autoMove)
        {
            // If through is ON
            if (Through) return true;
            // If Map is not passable
            if (!InGame.Map.IsPassable(newX, newY, d, this, autoMove)) return false;
            // If collide with an avent
            foreach (short i in InGame.Map.EventKeysToUpdate)
            {
                // If event coordinates are consistent with move destination
                if (!InGame.Map.Events[i].Through && InGame.Map.Events[i] != this && (InGame.Map.Events[i].CharacterName != "" || InGame.Map.Events[i].TileId>0) )
                {
                    if (IsCollidingWithEvent(InGame.Map.Events[i], newX, newY)) return false;
                }
            }
            // If collide with Player
            if (this != InGame.Player)
            {
                return (!IsCollidingWithPlayer(newX, newY));
            }
            // passable
            return true;
        }

        public virtual bool IsPassable(int newX, int newY, int d)
        {
            return IsPassable(newX, newY, d, false);
        }

        /// <summary>
        /// True if "this" event is colliding with ev
        /// </summary>
        /// <param Name="ev">Event to be tested</param>
        public bool IsCollidingWithEvent(GameEvent ev)
        {
            /*// Collisions with tile coordinates
            if (GameOptions.IsTileByTileMoving)
            {
                return ev.X / 32 == X / 32 && ev.Y / 32 == Y / 32;
            }
            // Collisions with character and event masks
            else
            {*/
                //if (Through || ev.Through) return false;
                Rectangle A = new Rectangle(X - CollisionWidth / 2, Y + 1 - CollisionHeight, CollisionWidth, CollisionHeight);
                Rectangle B = new Rectangle(ev.X - ev.CollisionWidth / 2, ev.Y - ev.CollisionHeight, ev.CollisionWidth, ev.CollisionHeight);
                int top = Math.Max(A.Top, B.Top);
                int bottom = Math.Min(A.Bottom, B.Bottom);
                int left = Math.Max(A.Left, B.Left);
                int right = Math.Min(A.Right, B.Right);
                if (top <= bottom && left <= right)
                {
                    return true;
                }
                return false;
            //}
        }

        /// <summary>
        /// True if "this" new position event is colliding with ev
        /// </summary>
        /// <param Name="ev">Event to be tested</param>
        /// <param Name="newX">new Event x coordinate</param>
        /// <param Name="newY">new Event y coordinate</param>
        public bool IsCollidingWithEvent(GameEvent ev, int newX, int newY)
        {
            /*// Collisions with tile coordinates
            if (GameOptions.IsTileByTileMoving)
            {
                return ev.X / 32 == newX / 32 && ev.Y / 32 == newY / 32;
            }
            // Collisions with character and event masks
            else
            {*/
                //if (Through || ev.Through) return false;
                Rectangle A = new Rectangle(newX - CollisionWidth / 2, newY + 1 - CollisionHeight, CollisionWidth, CollisionHeight);
                Rectangle B = new Rectangle(ev.X - ev.CollisionWidth / 2, ev.Y - ev.CollisionHeight, ev.CollisionWidth, ev.CollisionHeight);
                int top = Math.Max(A.Top, B.Top);
                int bottom = Math.Min(A.Bottom, B.Bottom);
                int left = Math.Max(A.Left, B.Left);
                int right = Math.Min(A.Right, B.Right);
                if (top < bottom && left < right)
                {
                    return true;
                }
                return false;
            //}
        }

        /// <summary>
        /// True is new event position colliding with Player
        /// </summary>
        /// <param Name="newX">new Event x coordinate</param>
        /// <param Name="newY">new Event y coordinate</param>
        public bool IsCollidingWithPlayer(int newX, int newY)
        {
            // Collisions with tile coordinates
            if (GameOptions.IsTileByTileMoving)
            {
                return InGame.Player.X / GeexEdit.TileSize == newX / GeexEdit.TileSize && (InGame.Player.Y + 1) / GeexEdit.TileSize == newY / GeexEdit.TileSize;
            }
            // Collisions with character and event masks
            else
            {
                Rectangle A = new Rectangle(
                    newX - CollisionWidth / 2,
                    newY - CollisionHeight,
                    CollisionWidth,
                    CollisionHeight);
                Rectangle B = new Rectangle(
                     InGame.Player.X - InGame.Player.CollisionWidth / 2,
                     InGame.Player.Y + 1 - InGame.Player.CollisionHeight,
                     InGame.Player.CollisionWidth,
                     InGame.Player.CollisionHeight);

                int top = Math.Max(A.Top, B.Top);
                int bottom = Math.Min(A.Bottom, B.Bottom);
                int left = Math.Max(A.Left, B.Left);
                int right = Math.Min(A.Right, B.Right);
                if (top <= bottom && left <= right)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// True is event colliding with Player
        /// </summary>
        public bool IsCollidingWithPlayer()
        {
            return IsCollidingWithPlayer(X, Y);
        }

        /// <summary>
        /// Lock
        /// </summary>
        public void ToLock()
        {
            // If already locked
            if (Locked) return;
            // Save prelock direction
            PrelockDirection = Dir;
            // Turn toward player
            TurnTowardPlayer();
            // Set locked flag
            Locked = true;
        }

        /// <summary>
        /// Unlock
        /// </summary>
        public void Unlock()
        {
            // If not locked
            if (!Locked) return;
            // Clear locked flag
            Locked = false;
            // If direction is not fixed
            if (!IsDirectionFix)
            {
                // If prelock direction is saved
                if (PrelockDirection != 0)
                {
                    // Restore prelock direction
                    Dir = PrelockDirection;
                }
            }
        }

        /// <summary>
        /// Move to Designated Position
        /// </summary>
        /// <param Name="_x">X coordinate</param> 
        /// <param Name="_y">Y coordinate</param> 
        public virtual void Moveto(int _x, int _y)
        {
            X = _x;// % (InGame.Map.width*32);
            Y = _y;//% (InGame.Map.height*32);
            RealX = X;
            RealY = Y;
            PrelockDirection = 0;
        }

        /// <summary>
        /// Get Screen Z-Coordinates
        /// </summary>
        /// <param Name="height">height : Character height</param>
        public int ScreenZ(int height)
        {
            // If display flag on closest surface is ON
            if (IsAlwaysOnTop) return 999;
            // Get screen coordinates from real coordinates and map display position
            int z = RealY - InGame.Map.DisplayY + Priority;
            // If Tile
            if (TileId > 0)
            {
              // Add Tile priority * 32
                return z + TileManager.Priorities[TileId];
            }
            // If Character
            else
            {
              // If height exceeds 32, then add 31
                if (height > 32)
                {
                    return z + 31;
                }
                else
                {
                    return z;
                }
            }
        }
        /// <summary>
        /// Get Screen Z-Coordinates
        /// </summary>
        public int ScreenZ()
        {
            return ScreenZ(0);
        }
        #endregion

        #region Methods - Updates
        /// <summary>
        /// Temporarily Erase
        /// </summary>
        public void Erase()
        {
            isErased = true;
            Refresh();
        }
        /// <summary>
        /// Generic method for refresh
        /// </summary>
        public virtual void Refresh()
        {}

        /// <summary>
        /// Frame Update
        /// </summary>
        public virtual void Update()
        {
            UpdateMovementType();
            UpdateAnimation();
            // If waiting
            if (WaitCount > 0)
            {
                // Reduce wait count
                WaitCount -= 1;
                return;
            }
            // If move route is forced then custom move
            if (MoveRouteForcing)
            {
                MoveTypeCustom();
                return;
            }
            // When waiting for event execution or locked, Not moving by self
            if (IsStarting | IsLocked) return;
            UpdateMovement();
        }

        /// <summary>
        /// Update Movement
        /// </summary>
        void UpdateMovement()
        {
            // If stop count exceeds a certain value (computed from move frequency)
            if (StopCount > (40 - MoveFrequency * 2) * (6 - MoveFrequency))
            {
                // Branch by move type
                switch (MoveType)
                {
                    case 1: // Random
                        MoveTypeRandom();
                        break;
                    case 2: //Approach
                        MoveTypeTowardPlayer();
                        break;
                    case 3: // Custom
                        MoveTypeCustom();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Update the right movement type
        /// </summary>
        void UpdateMovementType()
        {
            // Branch with jumping, moving, and stopping
            if (IsJumping)
            {
                UpdateJump();
            }
            else if (IsMoving)
            {
                UpdateMove();
            }
            else
            {
                UpdateStop();
            }
        }

        /// <summary>
        /// Update pattern Animation
        /// </summary>
        void UpdateAnimation()
        {
            // If Animation count exceeds maximum value
            // Maximum value is move speed * 1 taken from basic value 18
            if (AnimeCount > 18 - MoveSpeed * 2)
            {
                // If stop Animation is OFF when stopping
                if (!IsStepAnime & StopCount > 0)
                {
                    // Return to original pattern
                    Pattern = OriginalPattern;
                }
                // If stop Animation is ON when moving
                else
                {
                    //  Update pattern
                    Pattern = (Pattern + 1) % GameOptions.CharacterPatterns;
                }
                // Clear Animation count
                AnimeCount = 0;
            }
        }
        /// <summary>
        /// Frame Update for jumping
        /// </summary>
        public void UpdateJump()
        {
            // Reduce jump count by 1
            JumpCount -= 1;
            // Calculate new coordinates
            RealX = (RealX * JumpCount + X ) / (JumpCount + 1);
            RealY = (RealY * JumpCount + Y ) / (JumpCount + 1);
        }


        /// <summary>
        /// Frame Update for moving
        /// </summary>
        public void UpdateMove()
        {
            // Convert map coordinates from map move speed into move distance
            short distance = InGame.System.Speed[MoveSpeed];
            // If logical coordinates are further down than real coordinates
            // Move down
            if (Y > RealY) RealY = Math.Min(RealY + distance, Y);
            // Move left
            if (X < RealX) RealX = Math.Max(RealX - distance, X);
            // Move right
            if (X > RealX) RealX = Math.Min(RealX + distance, X);
            // Move up
            if (Y < RealY) RealY = Math.Max(RealY - distance, Y);
            // If move Animation is ON
            if (IsWalkAnim)
            {
              // Increase Animation count by 1.5
              AnimeCount += 1.5f;
            }
            // If move Animation is OFF, and stop Animation is ON
            else if (IsStepAnime)
            {
              // Increase Animation count by 1
                AnimeCount += 1;
            }
        }


        /// <summary>
        /// Frame Update (stop)
        /// </summary>
        public void UpdateStop()
        {
            // If stop Animation is ON
            if (IsStepAnime)
            {
                // Increase Animation count by 1
                AnimeCount += 1;
            }
            // If stop Animation is OFF, but current pattern is different from original
            else if (Pattern != OriginalPattern)
            {
                // Increase Animation count by 1.5
                AnimeCount += 1;
            }
            // When waiting for event execution, or not locked
            // If lock deals with event execution coming to a halt
            if (!(IsStarting | IsLocked))
            {
                // Increase stop count by 1
                StopCount += 1;
            }
        }
        #endregion

        #region Methods - Move Type
        /// <summary>
        /// Path finding to x,y coordination
        /// </summary>
        /// <param Name="x">X-map coordinate</param>
        /// <param Name="y">X-map coordinate</param>
        public void FindPath(int x, int y)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param Name="x"></param>
        /// <param Name="y"></param>
        public void JumpTo(int destX, int destY)
        {
            int x_plus=(destX-X)/GeexEdit.TileSize;
            int y_plus=(destY-Y)/GeexEdit.TileSize;
            // If plus value is not (0,0)
            if (x_plus != 0 || y_plus != 0)
            {
                // If horizontal distnace is longer
                if (Math.Abs(x_plus) > Math.Abs(y_plus))
                {
                    // Change direction to left or right
                    if (x_plus < 0)
                    {   TurnLeft();}
                    else
                    {   TurnRight();}
                // If vertical distance is longer, or equal
                }
                else
                {
                    // Change direction to up or down
                    if (y_plus < 0)
                    {   TurnUp();}
                    else
                    {   TurnDown();}
                }
            }
            // If plus value is (0,0) or jump destination is passable
            if ((x_plus == 0 && y_plus == 0) || IsPassable(X, Y, 0))
            {
                // Straighten position
                Straighten();
                // Update coordinates
                X = destX;
                Y = destY;
                // Calculate distance
                int distance = (int)Math.Sqrt(x_plus * x_plus + y_plus * y_plus);
                // Set jump count
                JumpPeak = 10 + distance - MoveSpeed;
                JumpCount = JumpPeak * 2;
                // Clear stop count
                StopCount = 0;
            }
        }

        /// <summary>
        /// Move Type : Custom
        /// </summary>
        public void MoveTypeCustom()
        {
            // Interrupt if not stopping
            if (MoveRoute==null || IsJumping || IsMoving || MoveRoute.List.Length == 0) return;
            // Loop until finally arriving at move command list
            while (MoveRouteIndex <= MoveRoute.List.Length)
            {
                // If command code is 0 (last part of list)
                #region Methods - move_type_custom = 0
                // Acquiring move command
                MoveCommand command = MoveRoute.List[0];
                if (MoveRouteIndex != MoveRoute.List.Length)
                {
                    command = MoveRoute.List[MoveRouteIndex];
                }
                if (MoveRouteIndex == MoveRoute.List.Length || command.Code == 0)
                {
                    // If [IsRepeated action] option is ON
                    if (MoveRoute.Repeat)
                    {
                        // First return to the move route index
                        MoveRouteIndex = 0;
                    }
                    else                    // If [IsRepeated action] option is OFF
                    {
                        // If move route is forcing
                        if (MoveRouteForcing && !MoveRoute.Repeat)
                        {
                            // Release forced move route
                            MoveRouteForcing = false;
                            // Restore original move route
                            MoveRoute = OriginalMoveRoute;
                            MoveRouteIndex = OriginalMoveRouteIndex;
                            OriginalMoveRoute = null;
                        }
                        // Clear stop count
                        StopCount = 0;
                    }
                    return;
                }
                #endregion
                // During move command (from move down to jump)
                #region Methods - move_type_custom <14
                if (command.Code <= 14)
                {
                    // Branch by command code
                    switch (command.Code)
                    {
                        case 1:
                            // Move down
                            MoveDown(true,GeexEdit.TileSize);
                            break;
                        case 2:
                            // Move left
                            MoveLeft(true, GeexEdit.TileSize);
                            break;
                        case 3:
                            // Move right
                            MoveRight(true, GeexEdit.TileSize);
                            break;
                        case 4:
                            // Move up
                            MoveUp(true, GeexEdit.TileSize);
                            break;
                        case 5:
                            // Move lower left
                            MoveLowerLeft(true,GeexEdit.TileSize);
                            break;
                        case 6:
                            // Move lower right
                            MoveLowerRight(true, GeexEdit.TileSize);
                            break;
                        case 7:
                            // Move upper left
                            MoveUpperLeft(true, GeexEdit.TileSize);
                            break;
                        case 8:
                            // Move upper right
                            MoveUpperRight(true, GeexEdit.TileSize);
                            break;
                        case 9:
                            // Move at random
                            MoveRandom();
                            break;
                        case 10:
                            // Move toward player
                            MoveTowardPlayer();
                            break;
                        case 11:
                            // Move away from player
                            MoveAwayFromPlayer();
                            break;
                        case 12:
                            // 1 step forward
                            MoveForward();
                            break;
                        case 13:
                            // 1 step backward
                            MoveBackward();
                            break;
                        case 14:
                            // Jump
                            Jump(command.IntParams[0]*GeexEdit.TileSize, command.IntParams[1]*GeexEdit.TileSize);
                            break;
                    }
                    // If movement failure occurs when [Ignore if can't move] option is OFF
                    if (!MoveRoute.Skippable && !IsMoving && !IsJumping) return;
                    MoveRouteIndex += 1;
                    return;
                }
                #endregion
                // If waiting
                #region Methods - move_type_custom = 15
                if (command.Code == 15)
                {
                    // Set wait count
                    WaitCount = command.IntParams[0] * 2 - 1;
                    MoveRouteIndex += 1;
                    return;
                }
                // If direction change command
                if (command.Code >= 16 & command.Code <= 26)
                {
                    // Branch by command code
                    switch (command.Code)
                    {
                        case 16:  // Turn down
                            TurnDown();
                            break;
                        case 17:  // Turn left
                            TurnLeft();
                            break;
                        case 18:  // Turn right
                            TurnRight();
                            break;
                        case 19:  // Turn up
                            TurnUp();
                            break;
                        case 20:  // Turn 90° right
                            TurnRight90();
                            break;
                        case 21:  // Turn 90° left
                            TurnLeft90();
                            break;
                        case 22:  // Turn 180°
                            Turn180();
                            break;
                        case 23:  // Turn 90° right or left
                            TurnRightOrLeft90();
                            break;
                        case 24:  // Turn at Random
                            TurnRandom();
                            break;
                        case 25:  // Turn toward player
                            TurnTowardPlayer();
                            break;
                        case 26:  // Turn away from player
                            TurnAwayFromPlayer();
                            break;
                    }
                    MoveRouteIndex += 1;
                    return;
                }
                #endregion
                // If other command
                #region Methods - move_type_custom >= 27
                if (command.Code >= 27)
                {
                    // Branch by command code
                    switch (command.Code)
                    {
                        case 27:  // Switch ON
                            InGame.Switches.Arr[(int)command.IntParams[0]] = true;
                            InGame.Map.IsNeedRefresh = true;
                            break;
                        case 28:  // Switch OFF
                            InGame.Switches.Arr[(int)command.IntParams[0]] = false;
                            InGame.Map.IsNeedRefresh = true;
                            break;
                        case 29: // Change speed
                            MoveSpeed = (int)command.IntParams[0];
                            break;
                        case 30:  // Change freq
                            MoveFrequency = (int)command.IntParams[0];
                            break;
                        case 31:  // Move Animation ON
                            IsWalkAnim = true;
                            break;
                        case 32:  // Move Animation OFF
                            IsWalkAnim = false;
                            break;
                        case 33:  // Stop Animation ON
                            IsStepAnime = true;
                            break;
                        case 34:  // Stop Animation OFF
                            IsStepAnime = false;
                            break;
                        case 35:  // Direction fix ON
                            IsDirectionFix = true;
                            break;
                        case 36:  // Direction fix OFF
                            IsDirectionFix = false;
                            break;
                        case 37:  // Through ON
                            Through = true;
                            break;
                        case 38:  // Through OFF
                            Through = false;
                            break;
                        case 39:  // Always on top ON
                            IsAlwaysOnTop = true;
                            break;
                        case 40:  // Always on top OFF
                            IsAlwaysOnTop = false;
                            break;
                        case 41:  // Change Graphic
                            TileId = 0;
                            CharacterName = command.StringParams;
                            CharacterHue = command.IntParams[0];
                            if (OriginalDirection != command.IntParams[1])
                            {
                                Dir = command.IntParams[1];
                                OriginalDirection = Dir;
                                PrelockDirection = 0;
                            }
                            if (OriginalPattern != command.IntParams[2])
                            {
                                Pattern = command.IntParams[2];
                                OriginalPattern = Pattern;
                            }
                            break;
                        case 42:  // Change Opacity
                            Opacity = (byte)command.IntParams[0];
                            break;
                        case 43:  // Change Blending
                            BlendType = command.IntParams[0];
                            break;
                        case 44:  // Play SE
                            InGame.System.SoundPlay(new AudioFile(command.StringParams, command.IntParams[0], command.IntParams[1]));
                            break;
                        case 45:  // Script
                            //result = eval(command.BaseParameters[0])
                            break;
                    }
                    MoveRouteIndex += 1;
                }
                #endregion
            }
        }

        /// <summary>
        /// Move Type : Random
        /// </summary>
        public void MoveTypeRandom()
        {
            int rnd = InGame.Rnd.Next(6);
            if (rnd<4)
            {
                MoveRandom();
                return;
            }
            if (rnd==4)
            {
                MoveForward();
                return;
            }
            if (rnd==5)
            {
                StopCount=0;
            }
        }

        /// <summary>
        /// Move Type : Toward Player
        /// </summary>
        public void MoveTypeTowardPlayer()
        {
            // Get difference in player coordinates
            int sx = X - InGame.Player.X;
            int sy = Y - InGame.Player.Y;
            // Get absolute value of difference
            int abs_sx = sx > 0 ? sx : -sx;
            int abs_sy = sy > 0 ? sy : -sy;
            // If separated by Map Width or more tiles matching up horizontally and vertically
            if (sx + sy >= GeexEdit.GameMapWidth*GeexEdit.TileSize)
            {
                // Random
                MoveRandom();
            }
            else if (abs_sx > 0 || abs_sy > 0)
            {
                MoveTowardPlayer();
            }
        }

        /// <summary>
        /// Move toward a GameCharacter
        /// </summary>
        /// <param Name="Character">Target GameCharacter</param>
        public void MoveTowardCharacter(GameCharacter character)
        {
            // Get difference in player coordinates
            int sx = X - character.X;
            int sy = Y - character.Y;
            // If coordinates are equal
            if (sx == 0 && sy == 0) return;
            // Get absolute value of difference
            int abs_sx = Math.Abs(sx);
            int abs_sy = Math.Abs(sy);

            // If horizontal and vertical distances are equal
            if (abs_sx == abs_sy)
            {
                // Increase one of them randomly
                if (InGame.Rnd.Next(2) == 0)
                {
                    abs_sx += InGame.System.Speed[MoveSpeed];
                }
                else
                {
                    abs_sy += InGame.System.Speed[MoveSpeed];
                }
            }
            // If horizontal distance is longer
            if (abs_sx > abs_sy)
            {
                // Move towards player, prioritize left and right directions
                if (sx > 0)
                {
                    MoveLeft(true, InGame.System.Speed[MoveSpeed], true);
                }
                else
                {
                    MoveRight(true, InGame.System.Speed[MoveSpeed], true);
                }
                if (!IsMoving & sy != 0)
                {
                    if (sy > 0) { MoveUp(true, InGame.System.Speed[MoveSpeed], true); } else { MoveDown(true, InGame.System.Speed[MoveSpeed], true); }
                }
            }
            // If vertical distance is longer
            else
            {
                // Move towards player, prioritize up and down directions
                if (sy > 0) { MoveUp(true, InGame.System.Speed[MoveSpeed], true); } else { MoveDown(true, InGame.System.Speed[MoveSpeed], true); }
                if (!IsMoving & sx != 0)
                {
                    if (sx > 0) { MoveLeft(true, InGame.System.Speed[MoveSpeed], true); } else { MoveRight(true, InGame.System.Speed[MoveSpeed], true); }
                }
            }
        }
        
        /// <summary>
        /// Move at Random
        /// </summary>
        public void MoveRandom()
        {
            switch (InGame.Rnd.Next(4))
            {
                case 0: // Move down
                    MoveDown(false,GeexEdit.TileSize);
                    break;
                case 1: // Move left
                    MoveLeft(false,GeexEdit.TileSize);
                    break;
                case 2: // Move right
                    MoveRight(false,GeexEdit.TileSize);
                    break;
                case 3:  // Move up
                    MoveUp(false,GeexEdit.TileSize);
                    break;
            }
        }

        /// <summary>
        /// Move toward Player
        /// </summary>
        public void MoveTowardPlayer()
        {
            MoveTowardCharacter(InGame.Player);
        }

        /// <summary>
        /// Move away from Player
        /// </summary>
        public void MoveAwayFromPlayer()
        {
            MoveAwayFromCharacter(InGame.Player);
        }
        /// Move away from GameCharacter
        /// </summary>
        /// <param Name="Character">Target GameCharacter</param>
        public void MoveAwayFromCharacter(GameCharacter character)
        {
            // Get difference in player coordinates
            int sx = X - character.X;
            int sy = Y - character.Y;
            // If coordinates are equal
            if (sx == 0 && sy == 0) return;
            // Get absolute value of difference
            int abs_sx = Math.Abs(sx);
            int abs_sy = Math.Abs(sy);
            // If horizontal and vertical distances are equal
            if (abs_sx == abs_sy)
            {
                // Increase one of them randomly by 32pixel
                if (InGame.Rnd.Next(2) == 0)
                {
                    abs_sx += GeexEdit.TileSize;
                    abs_sy += GeexEdit.TileSize;
                }
            }
            // If horizontal distance is longer
            if (abs_sx > abs_sy)
            {
                // Move away from player, prioritize left and right directions
                if (sx > 0)
                {
                    MoveRight(true, GeexEdit.TileSize);
                }
                else
                {
                    MoveLeft(true, GeexEdit.TileSize);
                }
                if (!IsMoving & sy != 0)
                {
                    if (sy > 0)
                    {
                        MoveDown(true, GeexEdit.TileSize);
                    }
                    else
                    {
                        MoveUp(true, GeexEdit.TileSize);
                    }
                }
            }
            // If vertical distance is longer
            else
            {
                // Move away from player, prioritize up and down directions
                if (sy > 0)
                {
                    MoveDown(true, GeexEdit.TileSize);
                }
                else
                {
                    MoveUp(true, GeexEdit.TileSize);
                }
                if (!IsMoving & sx != 0)
                {
                    if (sx > 0)
                    {
                        MoveRight(true, GeexEdit.TileSize);
                    }
                    else
                    {
                        MoveLeft(true, GeexEdit.TileSize);
                    }
                }
            }
        }
        /// <summary>
        /// 1 Step Forward
        /// </summary>
        public void MoveForward()
        {
            switch (Dir)
            {
                case 2:
                    MoveDown(false,GeexEdit.TileSize);
                    break;
                case 4:
                    MoveLeft(false,GeexEdit.TileSize);
                    break;
                case 6:
                    MoveRight(false,GeexEdit.TileSize);
                    break;
                case 8:
                    MoveUp(false,GeexEdit.TileSize);
                    break;
            }
        }

        /// <summary>
        /// 1 Step Backward
        /// </summary>
        public void MoveBackward()
        {
            // Remember direction fix situation
            bool last_direction_fix = IsDirectionFix;
            // Force direction fix
            IsDirectionFix = true;
            // Branch by direction
            switch (Dir)
            {
                case 2:  // Down
                    MoveUp(false,GeexEdit.TileSize);
                    break;
                case 4:  // Left
                    MoveRight(false,GeexEdit.TileSize);
                    break;
                case 6:  // Right
                    MoveLeft(false,GeexEdit.TileSize);
                    break;
                case 8:  // Up
                    MoveDown(false,GeexEdit.TileSize);
                    break;
            }
            // Return direction fix situation back to normal
            IsDirectionFix = last_direction_fix;
        }

        #endregion

        #region Methods - Move
        /// <summary>
        /// Jump
        /// </summary>
        /// <param Name="x_plus">x-coordinate plus value</param>
        /// <param Name="y_plus">y-coordinate plus value</param>
        public void Jump(int x_plus, int y_plus)
        {
            // If plus value is not (0,0)
            if (x_plus != 0 | y_plus != 0)
            {
                // If horizontal distnace is longer
                if (Math.Abs(x_plus) > Math.Abs(y_plus))
                {
                    // Change direction to left or right
                    if (x_plus < 0) { TurnLeft(); } else { TurnRight(); }
                }
                // If vertical distance is longer, or equal
                else
                {
                    // Change direction to up or down
                    if (y_plus < 0) { TurnUp(); } else { TurnDown(); }
                }
            }
            // Calculate new coordinates
            int new_x = X + x_plus;
            int new_y = Y + y_plus;
            // If plus value is (0,0) or jump destination is passable (10 is direction for jump)
            if ((x_plus == 0 & y_plus == 0) | IsPassable(new_x, new_y, 10))
            {
                // Straighten position
                Straighten();
                // Update coordinates
                X = new_x;
                Y = new_y;
                // Calculate distance
                //int distance = (int)Math.Sqrt((x_plus * x_plus + y_plus * y_plus) / GeexEdit.TileSize);
                int distance = (int)(Math.Max(Math.Abs(x_plus),Math.Abs(y_plus)) / GeexEdit.TileSize);
                // Set jump count
                JumpPeak = 10 + distance - MoveSpeed;
                JumpCount = JumpPeak * 2;
                // Clear stop count
                StopCount = 0;
            }
        }
        
        /// <summary>
        /// Increase Steps
        /// </summary>
        public virtual void IncreaseSteps()
        {
            // Clear stop count
            StopCount = 0;
        }

        protected virtual bool CheckEventTriggerTouchMove(int newY, int newX) 
        {
            return false;
        }
        
        /// <summary>
        /// Move Left
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveLeft(bool turn_enabled, short pixel, bool automove)
        {
            // Turn left
            if (turn_enabled) TurnLeft();
            // If passable
            if (IsPassable(X - pixel, Y, 4, automove))
            {
                // Update coordinates
                X -= pixel;
                // Increase steps
                IncreaseSteps();
            }
            else
            {
                CheckEventTriggerTouchMove(X - pixel, Y);
            }
        }

        public void MoveLeft(bool turn_enabled, short pixel)
        {
            MoveLeft(turn_enabled, pixel, false);
        }

        /// <summary>move right</summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveRight(bool turn_enabled, short pixel, bool automove)
        {
            // Turn right
            if (turn_enabled) TurnRight();
            // If passable
            if (IsPassable(X + pixel, Y, 6, automove))
            {
                // Update coordinates
                X += pixel;
                // Increase steps
                IncreaseSteps();
            }
            else
            {
                CheckEventTriggerTouchMove(X + pixel, Y);
            }
        }

        public void MoveRight(bool turn_enabled, short pixel)
        {
            MoveRight(turn_enabled, pixel, false);
        }

        /// <summary>
        /// move up
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveUp(bool turn_enabled, short pixel, bool automove)
        {
            // Turn up
            if (turn_enabled) TurnUp();
            // If passable
            if (IsPassable(X, Y - pixel, 8, automove))
            {
                // Update coordinates
                Y -= pixel;
                // Increase ste;ps
                IncreaseSteps();
            }
            else
            {
                CheckEventTriggerTouchMove(X, Y - pixel);
            }
        }

        public void MoveUp(bool turn_enabled, short pixel)
        {
            MoveUp(turn_enabled, pixel, false);
        }

        /// <summary>
        /// Move Down
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveDown(bool turn_enabled, short pixel, bool automove)
        {
            // Turn down
            if (turn_enabled) TurnDown();
            // If passable
            if (IsPassable(X, Y + pixel, 2, automove))
            {
                // Update coordinates
                Y += pixel;
                // Increase steps
                IncreaseSteps();
            }
            else
            {
                CheckEventTriggerTouchMove(X, Y + pixel);
            }
        }

        public void MoveDown(bool turn_enabled, short pixel)
        {
            MoveDown(turn_enabled, pixel, false);
        }

        /// <summary>
        /// Move Lower Left
        /// </summary>
        /// <param Name="turn_enabled">a flag permits direction change on that spot</param>
        /// <param Name="pixel">The pixel distance to move</param>
        public void MoveLowerLeft(bool turn_enabled, short pixel)
        {
            if (turn_enabled) TurnLowerLeft();
            // When a down to left or a left to down course is passable
            if (GameOptions.IsCollisionMaskOn)
            {
                if(IsPassable(X-pixel,Y+pixel, Dir))
                {
                    // Update coordinates
                    X -= pixel;
                    Y += pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X - pixel, Y + pixel);
                }
            }
            else
            {
                if ((IsPassable(X, Y, 2) && IsPassable(X, Y + pixel, 4)) || (IsPassable(X, Y, 4) && IsPassable(X - pixel, Y, 2)))
                {
                    // Update coordinates
                    X -= pixel;
                    Y += pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X - pixel, Y + pixel);
                }
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
            // If no direction fix
            if (!IsDirectionFix)
            {
                // Face right if facing left, and face down if facing up
                Dir = (Dir == 4 ? 6 : Dir == 8 ? 2 : Dir);
            }
            if (GameOptions.IsCollisionMaskOn)
            {
                if (IsPassable(X + pixel, Y + pixel, Dir))
                {
                    // Update coordinates
                    X += pixel;
                    Y += pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X + pixel, Y + pixel);
                }
            }
            else
            { 
                // When a down to right or a right to down course is passable
                if ((IsPassable(X, Y, 2) & IsPassable(X, Y + pixel, 6)) | (IsPassable(X, Y, 6) & IsPassable(X + pixel, Y, 2)))
                {
                    // Update coordinates
                    X += pixel;
                    Y += pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X + pixel, Y + pixel);
                }
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
            if (GameOptions.IsCollisionMaskOn)
            {
                if (IsPassable(X - pixel, Y - pixel, Dir))
                {
                    // Update coordinates
                    X -= pixel;
                    Y -= pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X - pixel, Y - pixel);
                }
            }
            else
            {
                // When an up to left or a left to up course is passable
                if ((IsPassable(X, Y, 8) && IsPassable(X, Y - pixel, 4)) || (IsPassable(X, Y, 4) && IsPassable(X - pixel, Y, 8)))
                {
                    // Update coordinates
                    X -= pixel;
                    Y -= pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X - pixel, Y - pixel);
                }
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
            if (GameOptions.IsCollisionMaskOn)
            {
                if (IsPassable(X + pixel, Y - pixel, Dir))
                {
                    // Update coordinates
                    // Update coordinates
                    X += pixel;
                    Y -= pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X + pixel, Y - pixel);
                }
            }
            else
            {
                // When an up to right or a right to up course is passable
                if ((IsPassable(X, Y, 8) && IsPassable(X, Y - pixel, 6)) || (IsPassable(X, Y, 6) && IsPassable(X + pixel, Y, 8)))
                {
                    // Update coordinates
                    X += pixel;
                    Y -= pixel;
                    // Increase steps
                    IncreaseSteps();
                }
                else
                {
                    CheckEventTriggerTouchMove(X + pixel, Y - pixel);
                }
            }
        }
        #endregion

        #region Methods - Turn
        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnUp()
        {
            if (!IsDirectionFix)
            {
                Dir = 8;
                StopCount = 0;
            }
        }

        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnRight()
        {
            if (!IsDirectionFix)
            {
                Dir = 6;
                StopCount = 0;
            }
        }

        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnLeft()
        {
            if (!IsDirectionFix)
            {
                Dir = 4;
                StopCount = 0;
            }
        }

        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnDown()
        {
            if (!IsDirectionFix)
            {
                Dir = 2;
                StopCount = 0;
            }
        }

        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnLowerLeft()
        {
            // If no direction fix
            if (!IsDirectionFix)
            {
                // Face down is facing right or up
                Dir = (Dir == 6 ? 4 : Dir == 8 ? 2 : Dir);
            }
        }

        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnLowerRight()
        {
            // If no direction fix
            if (!IsDirectionFix)
            {
                // Face right if facing left, and face down if facing up
                Dir = (Dir == 4 ? 6 : Dir == 8 ? 2 : Dir);
            }
        }

        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnUpperLeft()
        {
            // If no direction fix
            if (!IsDirectionFix)
            {
                // Face left if facing right, and face up if facing down
                Dir = (Dir == 6 ? 4 : Dir == 2 ? 8 : Dir);
            }
        }

        /// <summary>
        /// turn Character toward direction
        /// </summary>
        public void TurnUpperRight()
        {
            // If no direction fix
            if (!IsDirectionFix)
            {
                // Face right if facing left, and face up if facing down
                Dir = (Dir == 4 ? 6 : Dir == 2 ? 8 : Dir);
            }
        }
        /// <summary>
        /// Turn 90° Right
        /// </summary>
        public void TurnRight90()
        {
            switch (Dir
            )
            {
                case 2:
                    TurnLeft();
                    break;
                case 4:
                    TurnUp();
                    break;
                case 6:
                    TurnDown();
                    break;
                case 8:
                    TurnRight();
                    break;
            }
        }

        /// <summary>
        /// Turn 90° Left
        /// </summary>
        public void TurnLeft90()
        {
            switch (Dir
            )
            {
                case 2:
                    TurnRight();
                    break;
                case 4:
                    TurnDown();
                    break;
                case 6:
                    TurnUp();
                    break;
                case 8:
                    TurnLeft();
                    break;
            }
        }

        /// <summary>
        /// Turn 180°
        /// </summary>
        public void Turn180()
        {
            switch (Dir
            )
            {
                case 2:
                    TurnUp();
                    break;
                case 4:
                    TurnRight();
                    break;
                case 6:
                    TurnLeft();
                    break;
                case 8:
                    TurnDown();
                    break;
            }
        }

        /// <summary>
        /// Turn 90° Right or Left
        /// </summary>
        public void TurnRightOrLeft90()
        {
            if (InGame.Rnd.Next(2) == 0)
            {
                TurnRight90();
            }
            else
            {
                TurnLeft90();
            }
        }

        /// <summary>
        /// Turn at Random
        /// </summary>
        public void TurnRandom()
        {
            switch (InGame.Rnd.Next(4))
            {
                case 0:
                    TurnUp();
                    break;
                case 1:
                    TurnRight();
                    break;
                case 2:
                    TurnLeft();
                    break;
                case 3:
                    TurnDown();
                    break;
            }
        }

        /// <summary>
        /// Turn Towards Player
        /// </summary>
        public void TurnTowardPlayer()
        {
            // Get difference in player coordinates
            int sx = X - InGame.Player.X;
            int sy = Y - InGame.Player.Y;
            // If coordinates are equal
            if (sx == 0 & sy == 0) return;
            // If horizontal distance is longer
            if (Math.Abs(sx) > Math.Abs(sy))
            {
                // Turn to the right or left towards player
                if (sx > 0) { TurnLeft();} else { TurnRight();}
            }
            // If vertical distance is longer
            else
            {
                // Turn up or down towards player
                if (sy > 0) { TurnUp();} else { TurnDown();}
            }
            
        }

        /// <summary>
        /// Turn Away from Player
        /// </summary>
        public void TurnAwayFromPlayer()
        {
            // Get difference in player coordinates
            int sx = X - InGame.Player.X;
            int sy = Y - InGame.Player.Y;
            // If coordinates are equal
            if (sx == 0 & sy == 0) return;
            // If horizontal distance is longer
            if (Math.Abs(sx) > Math.Abs(sy))
            {
                // Turn to the right or left away from player
                if (sx > 0)
                {
                    TurnRight();
                }
                else
                {
                    TurnLeft();
                }
            }
            // If vertical distance is longer
            else
            {
            // Turn up or down away from player
                if (sy > 0)
                {
                    TurnDown();
                }
                else
                {
                    TurnUp();
                }
            }
        }
        #endregion

        #region Methods - test
        /// <summary>
        /// True if this is facing ev
        /// </summary>
        /// <param Name="ev">Character to be tested against</param>
        /// <returns></returns>
        public bool IsFacing(GameCharacter ev)
        {
            switch (Dir)
            {
                case 2:
                    return (Y<ev.Y);
                case 4:
                    return (X>ev.X);
                case 6:
                    return (X<ev.X);
                case 8:
                    return (Y>ev.Y);
            }
            return false;
        }
        /// <summary>
        /// True if this is facing one of the GameCharacter within list
        /// </summary>
        /// <param Name="list">List of GameCharacter to be tested against</param>
        public bool IsFacing(List<GameCharacter> list)
        {
            if (IsMoving) return false;
            foreach(GameCharacter character in list)
            {
                if (IsFacing(character)) return true;
            }
            return false;
        }

        /// <summary>
        /// True if this is facing ev
        /// </summary>
        /// <param Name="ev">Character to be tested against</param>
        /// <returns></returns>
        public bool IsFacingHard(GameCharacter ev)
        {
            int c = 0;
            int d = 0;
            switch (Dir)
            {
                case 2:
                    c = ev.Y - Y;
                    d = ev.X - X;
                    return (Y < ev.Y) && (Math.Abs(c/(Math.Sqrt(d * d + c * c))) >= 0.75);
                case 4:
                    c = X - ev.X;
                    d = ev.Y - Y;
                    return (X > ev.X) && (Math.Abs(c / (Math.Sqrt(d * d + c * c))) >= 0.75);
                case 6:
                    c = ev.X - X;
                    d = ev.Y - Y;
                    return (X < ev.X) && (Math.Abs(c / (Math.Sqrt(d * d + c * c))) >= 0.75);
                case 8:
                    c = Y - ev.Y;
                    d = ev.X - X;
                    return (Y > ev.Y) && (Math.Abs(c / (Math.Sqrt(d * d + c * c))) >= 0.75);
            }
            return false;
        }

        /// <summary>
        /// True if this is within radius range
        /// </summary>
        /// <param Name="with">Character to be tested against</param>
        /// <param Name="radius">Radius in pixel</param>
        /// <param Name="type">True if Square range, otherwise circle</param>
        public bool ViewRange(GameCharacter with, int radius, bool squareRange)
        {
            int d=999999;
            if (with == this) return false;
            if (squareRange)
            {
                d = (X - with.X) + (Y - with.Y);
            }
            else
            {
                d = (int)(Math.Sqrt( (X - with.X)*(X-with.X) + (Y - with.Y)*(Y - with.Y) ));
            }
            return d<=radius;
        }

        /// <summary>
        /// True if this "view" one Character in list
        /// </summary>
        /// <param Name="list">List of GameCharacter to be checked against</param>
        /// <param Name="radius">View range in pixel</param>
        /// <param Name="type">True if Square range, otherwise circle</param>
        public bool ViewRange(List<GameCharacter> list,int radius,bool squareRange)
        {
            foreach(GameCharacter character in list)
            {
                if (character == this) continue;
                if (ViewRange(character,radius,squareRange)) return true;
            }
            return false;
        }
        #endregion
        #endregion
    }
}
