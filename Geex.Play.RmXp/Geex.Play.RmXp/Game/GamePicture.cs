using Geex.Edit;
using Geex.Run;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class represents an in-game drawn Picture
    /// </summary>
    public class GamePicture
    {
        #region Variables
        /// <summary>
        /// Geex Effect for this picture
        /// </summary>
        public GeexEffect GeexEffect = new GeexEffect();

        /// <summary>
        /// True if picture must be background
        /// </summary>
        public bool IsBackground = false;
        /// <summary>
        /// True is Picture should be locked in the map. Pictures move according to map movement
        /// </summary>
        public bool IsLocked = false;

        /// <summary>
        /// Picture number
        /// </summary>
        public int Number;

        /// <summary>
        /// file Name
        /// </summary>
        public string Name;

        /// <summary>
        /// starting point
        /// </summary>
        public int Origin;

        /// <summary>
        /// x-coordinate
        /// </summary>
        public int X;

        /// <summary>
        /// y-coordinate
        /// </summary>
        public int Y;

        /// <summary>
        /// x directional zoom rate (0 to 100)
        /// </summary>
        public float ZoomX;
        /// <summary>
        /// y directional zoom rate (0 to 100)
        /// </summary>
        public float ZoomY;

        /// <summary>
        /// opacity level
        /// </summary>
        public byte Opacity;
        float startOpacity = 0;


        /// <summary>
        /// blend method
        /// </summary>
        public int BlendType;
        
        /// <summary>
        /// Color tone
        /// </summary>
        public Tone ColorTone;

        /// <summary>
        /// rotation angle
        /// </summary>
        public int Angle;

        /// <summary>
        /// change duration
        /// </summary>
        int duration;

        /// <summary>
        /// Target x-coordinates
        /// </summary>
        float targetX;

        /// <summary>
        /// Start X coordinate for Picture move
        /// </summary>
        float startX=0;

        /// <summary>
        /// Target y-coordinates
        /// </summary>
        float targetY;

        /// <summary>
        /// Start Y coordinate for Picture move
        /// </summary>
        float startY=0;

        /// <summary>
        /// Target x-zoom (0 to 100)
        /// </summary>
        float targetZoomX;

        /// <summary>
        /// Target y-zoom (0 to 100)
        /// </summary>
        float targetZoomY;

        /// <summary>
        /// Target opacity
        /// </summary>
        float targetOpacity;

        /// <summary>
        /// target tone
        /// </summary>
        Tone toneTarget;
        float startToneRed = 0;
        float startToneGeen = 0;
        float startToneBlue = 0;
        float startToneGray = 0;

        /// <summary>
        /// tone change duration
        /// </summary>
        int toneDuration;

        /// <summary>
        /// rotation speed
        /// </summary>
        int rotateSpeed;

        #endregion

        #region Initialize

        ///<summary>Constructor</summary>
        ///<param Name="number">Picture id</param Name>
        public GamePicture(int number)
        {
            this.Number = number;
            Name = "";
            Origin = 0;
            X = 0;
            Y = 0;
            ZoomX = 100;
            ZoomY = 100;
            Opacity = 255;
            BlendType = 1;
            duration = 0;
            targetX = X;
            targetY = Y;
            targetZoomX = ZoomX;
            targetZoomY = ZoomY;
            targetOpacity = Opacity;
            ColorTone = new Tone(0, 0, 0, 0);
            toneTarget = new Tone(0, 0, 0, 0);
            toneDuration = 0;
            Angle = 0;
            rotateSpeed = 0;
        }
        /// <summary>
        /// Empty constructor mandatory for game saving
        /// </summary>
        public GamePicture()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the Picture
        /// </summary>
        /// <param Name="_name">Picture's Name</param Name>
        /// <param Name="_origin">Origin</param Name>
        /// <param Name="showX">x-axis</param Name>
        /// <param Name="showY">y-axis</param Name>
        /// <param Name="showZoomX">zoom x-axis</param Name>
        /// <param Name="showZoomY">zoom y-axis</param Name>
        /// <param Name="showOpacity">opacity</param Name>
        /// <param Name="showBlendType">blend</param Name>
        /// <param Name="showLocked">true is Picture is locked to the map</param Name>
        public void Show(string _name, int _origin, int showX, int showY, int showZoomX, int showZoomY, byte showOpacity, int showBlendType, bool showLocked)
        {
            this.Name = _name;
            //showX = showX * 800 / 640;
            //showY = showY * 600 / 480;
            this.Origin = _origin;
            this.X = showX;
            startX = showX;
            this.Y = showY;
            startY = showY;
            this.ZoomX = showZoomX;
            this.ZoomY = showZoomY;
            this.Opacity = showOpacity;
            startOpacity = showOpacity;
            this.BlendType = showBlendType;
            duration = 0;
            targetX = showX;
            targetY = showY;
            targetZoomX = showZoomX;
            targetZoomY = showZoomY;
            targetOpacity = showOpacity;
            ColorTone = new Tone(0, 0, 0, 0);
            toneTarget = new Tone(0, 0, 0, 0);
            toneDuration = 0;
            Angle = 0;
            rotateSpeed = 0;
            IsLocked = showLocked;
        }

        /// <summary>
        /// Shows the Picture
        /// </summary>
        /// <param Name="_name">Picture's Name</param Name>
        /// <param Name="_origin">Origin</param Name>
        /// <param Name="showX">x-axis</param Name>
        /// <param Name="showY">y-axis</param Name>
        /// <param Name="showZoomX">zoom x-axis</param Name>
        /// <param Name="showZoomY">zoom y-axis</param Name>
        /// <param Name="showOpacity">opacity</param Name>
        /// <param Name="showBlendType">blend</param Name>
        /// <param Name="showLocked">true is Picture is locked to the map</param Name>
        /// <param Name="reCalc">True if needs recalculation</param>
        public void Show(string _name, int _origin, int showX, int showY, int showZoomX, int showZoomY, byte showOpacity, int showBlendType, bool showLocked, bool reCalc)
        {
            Show(_name, _origin, showX, showY, showZoomX, showZoomY, showOpacity, showBlendType, showLocked, reCalc, false);
        }
        public void Show(string _name, int _origin, int showX, int showY, int showZoomX, int showZoomY, byte showOpacity, int showBlendType, bool showLocked, bool reCalc, bool background)
        {
            this.Name = _name;
            if (reCalc)
            {
                showX = showX * 800 / 640;
                showY = showY * 600 / 480;
            }
            this.Origin = _origin;
            this.X = showX;
            startX = showX;
            this.Y = showY;
            startY = showY;
            this.ZoomX = showZoomX;
            this.ZoomY = showZoomY;
            this.Opacity = showOpacity;
            startOpacity = showOpacity;
            this.BlendType = showBlendType;
            duration = 0;
            targetX = showX;
            targetY = showY;
            targetZoomX = showZoomX;
            targetZoomY = showZoomY;
            targetOpacity = showOpacity;
            ColorTone = new Tone(0, 0, 0, 0);
            toneTarget = new Tone(0, 0, 0, 0);
            toneDuration = 0;
            Angle = 0;
            rotateSpeed = 0;
            IsLocked = showLocked;
            IsBackground = background;
        }

        /// <summary>
        /// Shows the Picture
        /// </summary>
        /// <param Name="Name">Picture's Name</param Name>
        /// <param Name="Origin">Origin</param Name>
        /// <param Name="showX">x-axis</param Name>
        /// <param Name="showYy">y-axis</param Name>
        /// <param Name="showZoomX">zoom x-axis</param Name>
        /// <param Name="showZoomY">zoom y-axis</param Name>
        /// <param Name="showOpacity">opacity</param Name>
        /// <param Name="showBlendType">blend</param Name>
        public void Show(string name, int origin, int showX, int showY, int showZoomX, int showZoomY, byte showOpacity, int showBlendType)
        {
            Show(name, origin, showX, showY, showZoomX, showZoomY, showOpacity, showBlendType, false);
        }

        /// <summary>
        /// Moves the Picture
        /// </summary>
        /// <param Name="MoveDuration">move duration</param Name>
        /// <param Name="MoveOrigin">Picture Origin</param Name>
        /// <param Name="moveX">x-axis</param Name>
        /// <param Name="moveY">y-axis</param Name>
        /// <param Name="moveZoomX">zoom x-axis</param Name>
        /// <param Name="moveZoomY">zoom y-axis</param Name>
        /// <param Name="moveOpacity">opacity</param Name>
        /// <param Name="MoveBlendType">blend</param Name>
        /// <param Name="moveAngle">rotation angle</param Name>
        public void Move(int duration, int origin, int moveX, int moveY, float moveZoomX, float moveZoomY, byte moveOpacity, int MoveBlendType, int moveAngle)
        {
            //moveX = moveX * 800 / 640;
            //moveY = moveY * 600 / 480;
            this.duration = duration;
            this.Origin = origin;
            this.targetX = moveX;
            this.targetY = moveY;
            this.targetZoomX = moveZoomX;
            this.targetZoomY = moveZoomY;
            this.targetOpacity = moveOpacity;
            this.BlendType = MoveBlendType;
            this.Angle = moveAngle;
        }

        /// <summary>
        /// Moves the Picture with position being recalculated or not (because of resolution change from 640*480 to 800*600)
        /// </summary>
        /// <param Name="MoveDuration">move duration</param Name>
        /// <param Name="MoveOrigin">Picture Origin</param Name>
        /// <param Name="moveX">x-axis</param Name>
        /// <param Name="moveY">y-axis</param Name>
        /// <param Name="moveZoomX">zoom x-axis</param Name>
        /// <param Name="moveZoomY">zoom y-axis</param Name>
        /// <param Name="moveOpacity">opacity</param Name>
        /// <param Name="MoveBlendType">blend</param Name>
        /// <param Name="moveAngle">rotation angle</param Name>
        /// <param Name="reCalc">True if needs recalculation</param>
        public void Move(int duration, int origin, int moveX, int moveY, float moveZoomX, float moveZoomY, byte moveOpacity, int MoveBlendType,bool reCalc)
        {
            if (reCalc)
            {
                moveX = moveX * 800 / 640;
                moveY = moveY * 600 / 480;
            }

            this.duration = duration;
            this.Origin = origin;
            this.targetX = moveX;
            this.targetY = moveY;
            this.targetZoomX = moveZoomX;
            this.targetZoomY = moveZoomY;
            this.targetOpacity = moveOpacity;
            this.BlendType = MoveBlendType;
            this.Angle = 0;
        }

        /// <summary>
        /// Moves the Picture
        /// </summary>
        /// <param Name="MoveDuration">move duration</param Name>
        /// <param Name="MoveOrigin">Picture Origin</param Name>
        /// <param Name="moveX">x-axis</param Name>
        /// <param Name="moveY">y-axis</param Name>
        /// <param Name="moveZoomX">zoom x-axis</param Name>
        /// <param Name="moveZoomY">zoom y-axis</param Name>
        /// <param Name="moveOpacity">opacity</param Name>
        /// <param Name="MoveBlendType">blend</param Name>
        public void Move(int duration, int origin, int x, int y, float zoom_x, float zoom_y, byte opacity, int blend_type)
        {
            Move(duration, origin, x, y, zoom_x, zoom_y, opacity, blend_type, 0);
        }

        /// <summary>
        /// Rotates the Picture
        /// </summary>
        /// <param Name="speed">rotation speed</param Name>
        public void Rotate(int speed)
        {
            rotateSpeed = speed;
        }

        ///<summary>Starts the Picture's tone change</summary>
        ///<param Name="changeTone">target tone</param Name>
        ///<param Name="duration">change duration</param Name>
        public void StartToneChange(Tone changeTone, int duration)
        {
            toneTarget = changeTone.Clone;
            toneDuration = (int)(duration * GameOptions.AdjustFrameRate);
            startToneRed = ColorTone.Red;
            startToneGeen = ColorTone.Green;
            startToneBlue= ColorTone.Blue;
            startToneGray = ColorTone.Gray;
            if (toneDuration == 0)
            {
                ColorTone = toneTarget.Clone;
            }
        }

        /// <summary>
        /// Erases the Picture
        /// </summary>
        public void Erase()
        {
            Name = "";
        }

        /// <summary>
        /// Updates the Picture
        /// </summary>
        public void Update()
        {
            if (duration >= 1)
            {
                startX = (startX * (duration - 1) + targetX) / duration;
                X = (int)startX;
                startY = (startY * (duration - 1) + targetY) / duration;
                Y = (int)startY;
                ZoomX = (ZoomX * (duration - 1) + targetZoomX) / duration;
                ZoomY = (ZoomY * (duration - 1) + targetZoomY) / duration;
                startOpacity = (startOpacity * (duration - 1) + targetOpacity) / duration;
                Opacity = (byte)startOpacity;
                duration -= 1;
            }
            if (toneDuration >= 1)
            {
                startToneRed = (startToneRed * (toneDuration - 1) + toneTarget.Red) / toneDuration;
                ColorTone.Red=(int)startToneRed;
                startToneGeen = (startToneGeen * (toneDuration - 1) + toneTarget.Green) / toneDuration;
                ColorTone.Green = (int)startToneGeen;
                startToneBlue = (startToneBlue * (toneDuration - 1) + toneTarget.Blue) / toneDuration;
                ColorTone.Blue = (int)startToneBlue;
                startToneGray = (startToneGray * (toneDuration - 1) + toneTarget.Gray) / toneDuration;
                ColorTone.Gray = (int)startToneGray;
                toneDuration -= 1;
            }
            if (rotateSpeed != 0)
            {
                Angle += rotateSpeed / 2;
                while (Angle < 0)
                {
                    Angle += 360;
                }
                Angle %= 360;
            }
        }
        #endregion
    }
}