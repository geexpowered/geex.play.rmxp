using Geex.Edit;
using Geex.Run;
using Microsoft.Xna.Framework;
using System;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class represents the game screen for handling : shaking, tone, Color, weather,
    // pictures display
    /// </summary>
    public partial class GameScreen
    {
        #region Variables

        /// <summary>
        /// Color tone
        /// </summary>
        public Tone ColorTone;

        /// <summary>
        /// flash Color
        /// </summary>
        public Color FlashColor;

        /// <summary>
        /// shake positioning
        /// </summary>
        public int Shake;

        /// <summary>
        /// in-game pictures
        /// </summary>
        public GamePicture[] Pictures = new GamePicture[GeexEdit.NumberOfPictures];

        /// <summary>
        /// in-battle pictures
        /// </summary>
        public GamePicture[] BattlePictures = new GamePicture[GeexEdit.NumberOfPictures];

        /// <summary>
        /// weather type
        /// </summary>
        public int WeatherType;

        /// <summary>
        /// max number of weather sprites
        /// </summary>
        public int WeatherMax;

        /// <summary>
        /// tone target
        /// </summary>
        Tone toneTarget;

        /// <summary>
        /// tone change duration
        /// </summary>
        int toneDuration;

        /// <summary>
        /// flash duration
        /// </summary>
        int flashDuration;

        /// <summary>
        /// shake power
        /// </summary>
        int shakePower;

        /// <summary>
        /// shake speed
        /// </summary>
        int shakeSpeed;

        /// <summary>
        /// shake duration
        /// </summary>
        int shakeDuration;

        /// <summary>
        /// shake direction
        /// </summary>
        int shakeDirection;

        /// <summary>
        /// weather type target
        /// </summary>
        int weatherTypeTarget;

        /// <summary>
        /// max number of weather sprites target
        /// </summary>
        int weatherMaxTarget;

        /// <summary>
        /// weather change duration
        /// </summary>
        int weatherDuration;
        /// <summary>
        /// Zoom X to target
        /// </summary>
        float zoomXTarget = 0f;
        /// <summary>
        /// Zoom Y to target
        /// </summary>
        float zoomYTarget = 0f;
        /// <summary>
        /// Zoom transformation duration
        /// </summary>
        int zoomDuration = 0;

        #endregion

        ///<summary>Constructor</summary>
        public GameScreen()
        {
            //Screentone initialization
            ColorTone = new Tone(0, 0, 0, 0);
            toneTarget = new Tone(0, 0, 0, 0);
            toneDuration = 0;

            //Screenflash initialization
            FlashColor = new Color(0, 0, 0, 0);
            flashDuration = 0;

            //Shaking initialization
            shakePower = 0;
            shakeSpeed = 0;
            shakeDuration = 0;
            shakeDirection = 1;
            Shake = 0;

            //Pictures initialization
            for (int i = 0; i < GeexEdit.NumberOfPictures; i++)
            {
                Pictures[i] = new GamePicture(i);
                BattlePictures[i] = new GamePicture(i);
            }

            //Weather initialization
            WeatherType = 0;
            WeatherMax = 0;
            weatherTypeTarget = 0;
            weatherMaxTarget = 0;
            weatherDuration = 0;
        }

        ///<summary>Starts screentone change</summary>
        ///<param Name="tone">target tone</param Name>
        ///<param Name="duration">change duration</param Name>
        public void StartToneChange(Tone tone, int duration)
        {
            toneTarget = tone;
            toneDuration = duration;
            if (toneDuration == 0)
            {
                this.ColorTone = tone;
            }
        }

        ///<summary>Starts screenflash</summary>
        ///<param Name="Color">flash Color</param Name>
        ///<param Name="duration">flash duration</param Name>
        public void StartFlash(Color color, int duration)
        {
            FlashColor = new Color(color.R, color.G, color.B, 255); //255 is the alpha value
            flashDuration = duration;
            Graphics.Background.Flash(FlashColor, duration);
            Graphics.Foreground.Flash(FlashColor, duration);
        }

        ///<summary>Starts screen shaking</summary>
        ///<param Name="power">shaking power</param Name>
        ///<param Name="speed">shaking speed</param Name>
        ///<param Name="duration">shaking duration</param Name>
        public void StartShake(int power, int speed, int duration)
        {
            shakePower = power;
            shakeSpeed = speed;
            shakeDuration = duration;
            Pad.VibrateLeft(duration, power / 9f, true);
            Pad.VibrateRight(duration, power / 9f, true);
        }

        /// <summary>
        /// Start Screen zoom
        /// </summary>
        /// <param name="zoomX">zoom X target</param>
        /// <param name="zoomY">zoom Y target</param>
        /// <param name="duration">zoom duration in frame</param>
        public void StartZoom(float zoomX, float zoomY, int duration)
        {
            zoomXTarget = zoomX;
            zoomYTarget = zoomY;
            zoomDuration= duration;
        }

        ///<summary>Change weather</summary>
        ///<param Name="type">weather type</param Name>
        ///<param Name="power">weather power</param Name>
        ///<param Name="duration">Transition duration</param Name>
        public void Weather(int type, int power, int duration)
        {
            weatherTypeTarget = type;
            //If weather changes
            if (weatherTypeTarget != 0)
            {
                WeatherType = weatherTypeTarget;
            }
            //If no weather Effect
            if (weatherTypeTarget == 0)
            {
                weatherMaxTarget = 0;
            }
            //else set weather_max_target 
            else
            {
                weatherMaxTarget = (power + 1) * 4;
            }
            weatherDuration = duration;
            //If no duration, jump to chosen weather
            if (weatherDuration == 0)
            {
                WeatherType = weatherTypeTarget;
                WeatherMax = weatherMaxTarget;
            }
        }

        ///<summary>Updates screen</summary>
        public void Update()
        {
            //Tone Update with duration discount
            if (toneDuration >= 1)
            {
                int _d = toneDuration;
                ColorTone.Red = (ColorTone.Red * (_d - 1) + toneTarget.Red) / _d;
                ColorTone.Green = (ColorTone.Green * (_d - 1) + toneTarget.Green) / _d;
                ColorTone.Blue = (ColorTone.Blue * (_d - 1) + toneTarget.Blue) / _d;
                ColorTone.Gray = (ColorTone.Gray * (_d - 1) + toneTarget.Gray) / _d;
                toneDuration -= 1;
            }
            //Flash Update with duration discount
            if (flashDuration >= 1)
            {
                int _d = flashDuration;
                FlashColor.A = (byte)(FlashColor.A * ((_d - 1) / _d));
                flashDuration -= 1;
            }

            //Shaking Update with duration discount
            if (shakeDuration >= 1 || Shake != 0)
            {
                //Screen move (from previous state) calculation : _delta
                float _delta = (shakePower * shakeSpeed * shakeDirection) / 10;
                //If shaking time is up
                if (shakeDuration <= 1 && Shake * (Shake + _delta) < 0)
                {
                    Shake = 0;
                }
                //Else screen moves of _delta
                else
                {
                    Shake += (int)_delta;
                }
                //Screen shaking direction fonctions of its power
                if (Shake > shakePower * 2)
                {
                    shakeDirection = -1;
                }
                if (Shake < -shakePower * 2)
                {
                    shakeDirection = 1;
                }
                //Duration discount
                if (shakeDuration >= 1)
                {
                    shakeDuration -= 1;
                }
            }

            //Weather Update and duration discount
            if (weatherDuration >= 1)
            {
                int _d = weatherDuration;
                WeatherMax = (WeatherMax * (_d - 1) + weatherMaxTarget) / _d;
                weatherDuration -= 1;
                if (weatherDuration == 0)
                {
                    WeatherType = weatherTypeTarget;
                }
            }
            // Zoom change
            if (zoomDuration > 0)
            {
                TileManager.ZoomCenter = new Vector2(
                GeexEdit.GameWindowCenterX - (Math.Max(GeexEdit.GameWindowWidth * (1 - TileManager.Zoom.X) / (2 * TileManager.Zoom.X), Math.Min(GeexEdit.GameWindowWidth * (TileManager.Zoom.X - 1) / (2 * TileManager.Zoom.X), GeexEdit.GameWindowCenterX - InGame.Player.ScreenX))),
                GeexEdit.GameWindowCenterY - (Math.Max(GeexEdit.GameWindowHeight * (1 - TileManager.Zoom.Y) / (2 * TileManager.Zoom.Y), Math.Min(GeexEdit.GameWindowHeight * (TileManager.Zoom.Y - 1) / (2 * TileManager.Zoom.Y), GeexEdit.GameWindowCenterY - InGame.Player.ScreenY))));
                TileManager.Zoom.X = (TileManager.Zoom.X * (zoomDuration - 1) + zoomXTarget) / zoomDuration;
                TileManager.Zoom.Y = (TileManager.Zoom.Y * (zoomDuration - 1) + zoomYTarget) / zoomDuration;
                zoomDuration--;
            }
            //Pictures Update : in battle...
            if (InGame.Temp.IsInBattle)
            {
                foreach (GamePicture pic in BattlePictures)
                {
                    pic.Update();
                }
            }
            //...or on the map
            else
            {
                foreach(GamePicture pic in Pictures)
                {
                    pic.Update();
                }
            }
        }
    }
}
