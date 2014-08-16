using System;
using System.Collections.Generic;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Spriting;
using Geex.Play.Rpg.Utils;
using Geex.Run;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Geex.Play.Custom;


namespace Geex.Play.Rpg.Spriteset
{
    /// <summary>
    /// This class brings together map screen sprites, tilemaps... It's used within the SceneMap class
    /// </summary>
    public partial class SpritesetMap
    {
        #region Constants
        /// <summary>
        /// Max value of Zoom
        /// </summary>
        const float ZOOM_MAX =1.5f;
        const float ZOOM_STEP = 0.01f;
        #endregion

        #region Variables
        /// <summary>
        /// List of Sprite Particles
        /// </summary>
        List<SpriteParticle> spriteParticles;
        /// <summary>
        /// List of inGame Characters
        /// </summary>
        List<SpriteCharacter> characterSprites;
        /// <summary>
        /// Game Map Panorama
        /// </summary>
        Geex.Run.Plane panorama;
        /// <summary>
        /// localName of Panorama
        /// </summary>
        string panoramaName;
        /// <summary>
        /// Hue of Panorama
        /// </summary>
        int panoramaHue;
        /// <summary>
        /// Game Map Fog
        /// </summary>
        Geex.Run.Plane fog;
        /// <summary>
        /// localName of Fog
        /// </summary>
        string fogName;
        /// <summary>
        /// Hue of Fog
        /// </summary>
        int fogHue;
        /// <summary>
        /// Game Map Weather
        /// </summary>
        Weather weather;
        /// <summary>
        /// Sprite for pictures
        /// </summary>
        SpritePicture[] pictureSprites;
        /// <summary>
        /// Sprite for Timer
        /// </summary>
        SpriteTimer timerSprite;
        #endregion

        /// <summary>
        /// This class brings together all types of game sprites. It's used within the SceneMap class
        /// </summary>
        public SpritesetMap()
        {
            InitTilemap();
            InitPanorama();
            InitFog();
            InitPlayer(); 
            InitCharacters();
            InitWeather();
            InitPictures();
            InitTimer();
            InitParticles();
            Update(); 
        }

        #region Initialize
        /// <summary>
        /// Make Panorama plane
        /// </summary>
        void InitPanorama()
        {
            panorama = new Geex.Run.Plane(Graphics.Background);
            panorama.Z = -1000;
            panorama.IsVisible = false;
        }

        /// <summary>
        /// Make Fog plane
        /// </summary>
        void InitFog()
        {
            fog = new Geex.Run.Plane(Graphics.Background);
            fog.Z = 3000;
            fog.IsVisible = false;
        }

         /// <summary>
         /// Make Character sprites
         /// </summary>
        void InitCharacters()
        {
            foreach (GameEvent ev in InGame.Map.Events)
            {
                // Check if event graphics Geex Option is triggered
                if (ev!=null && ev.IsGraphicVisible)
                {
                    characterSprites.Add(new SpriteCharacter(Graphics.Background, ev));
                }
            }
        }

        /// <summary>
        /// Initialize player sprites
        /// </summary>
        void InitPlayer()
        {
            characterSprites = new List<SpriteCharacter>();
            characterSprites.Add(new SpriteCharacter(Graphics.Background, InGame.Player));
        }

        /// <summary>
        /// Make weather
        /// </summary>
        void InitWeather()
        {
            weather = new Weather(Graphics.Background);
        }

        /// <summary>
        /// Make Picture sprites
        /// </summary>
        void InitPictures()
        {
            // TO OPTIMIZE : A recoder pour ne pas générer 50 pictures upfront
            pictureSprites = new SpritePicture[GeexEdit.NumberOfPictures];
            for (int i=0;i<GeexEdit.NumberOfPictures;i++)
            {
                pictureSprites[i] = new SpritePicture(Graphics.Foreground, InGame.Screen.Pictures[i]);
            }
        }

        /// <summary>
        /// Create and set TileManager
        /// </summary>
        void InitTilemap()
        {
            // mandatory for pixel collision - Mask must be present in Masks folders
            TileManager.ChipsetName = InGame.Map.ChipsetName;
        }
        /// <summary>
        /// Make Timer sprite
        /// </summary>
        void InitTimer()
        {    
            timerSprite = new SpriteTimer();
        }

        /// <summary>
        /// Initialize sprite particles list
        /// </summary>
        void InitParticles()
        {
            spriteParticles = new List<SpriteParticle>();
            // Check if there's any new particle to create
            foreach (GameParticle particle in InGame.Map.Particles)
            {
                CreateParticle(particle.FromEvent, particle.Effect);
            }
        }
        #endregion

        #region Methods

        #region Methods - Dispose
        /// <summary>
        /// Dispose of object
        /// </summary>
        public void Dispose()
        {
            DisposePanorama();
            DisposeFog();
            DisposeCharacters();
            DisposeWeather();
            DisposePictures();
            DisposeTimer();
            DisposeParticles();
            TileManager.ZoomCenter = new Vector2(GeexEdit.GameWindowCenterX, GeexEdit.GameWindowCenterY);
            TileManager.Dispose();
            InGame.Tags.Dispose();
        }

        /// <summary>
        /// Dispose of Panorama plane
        /// </summary>
        void DisposePanorama()
        {
            panorama.Dispose();
        }

        /// <summary>
        /// Dispose of Fog plane
        /// </summary>
        void DisposeFog()
        {
            fog.Dispose();
        }

        /// <summary>
        /// Dispose of Character sprites
        /// </summary>
        void DisposeCharacters()
        {
            foreach (SpriteCharacter cs in  characterSprites)
            {
                cs.Dispose();
            }
        }

        /// <summary>
        /// Dispose of Weather
        /// </summary>
        void DisposeWeather()
        {
            weather.Dispose();
        }

        /// <summary>
        /// Dispose of Picture sprites
        /// </summary>
        void DisposePictures()
        {
            foreach(SpritePicture sp in pictureSprites)
            {
                sp.Dispose();
            }
        }

        /// <summary>
        /// Dispose of Timer sprite
        /// </summary>
        void DisposeTimer()
        {
            timerSprite.Dispose();
        }

        /// <summary>
        /// Dispose all particles
        /// </summary>
        void DisposeParticles()
        {
            foreach (SpriteParticle sprite in spriteParticles) sprite.Dispose();
        }
        #endregion

        #region Methods - Update
        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            UpdateTilemap();
            UpdatePanorama();
            UpdateFog();
            UpdateCharacterSprites();
            UpdateWeather();
            UpdatePictureSprites();
            UpdateTimerSprite();
            UpdateParticle();
        }

        /// <summary>
        /// Update Game Particles
        /// </summary>
        void UpdateParticle()
        {
            // Check if there's any new particle to create
            for (int i = spriteParticles.Count; i < InGame.Map.Particles.Count;i++)
            {
                CreateParticle(InGame.Map.Particles[i].FromEvent, InGame.Map.Particles[i].Effect);
            }
            List<SpriteParticle> toDelete = new List<SpriteParticle>();
            foreach (SpriteParticle sprite in spriteParticles)
            {
                sprite.Update();
                if (!sprite.Ev.IsParticleTriggered)
                {
                    toDelete.Add(sprite);
                }
            }
            foreach (SpriteParticle sp in toDelete)
            {
                spriteParticles.Remove(sp);
                sp.Dispose();
            }
        }
        /// <summary>
        /// Update tilemap
        /// </summary>
        void UpdateTilemap()
        {
            // Set screen Color tone && shake position
            Graphics.Background.Tone = InGame.Screen.ColorTone;
            if (GameOptions.IsScreenToneCleanedAtEveryFrame) InGame.Screen.ColorTone = Tone.Empty;
            // Zoom
            TileManager.ZoomCenter = new Vector2(
                GeexEdit.GameWindowCenterX - (Math.Max(GeexEdit.GameWindowWidth * (1 - TileManager.Zoom.X) / (2 * TileManager.Zoom.X), Math.Min(GeexEdit.GameWindowWidth * (TileManager.Zoom.X - 1) / (2 * TileManager.Zoom.X), GeexEdit.GameWindowCenterX - InGame.Player.ScreenX))),
                GeexEdit.GameWindowCenterY - (Math.Max(GeexEdit.GameWindowHeight * (1 - TileManager.Zoom.Y) / (2 * TileManager.Zoom.Y), Math.Min(GeexEdit.GameWindowHeight * (TileManager.Zoom.Y - 1) / (2 * TileManager.Zoom.Y), GeexEdit.GameWindowCenterY - InGame.Player.ScreenY))));
            if (Pad.RightStickDir4 == Direction.Up || Geex.Run.Input.IsPressed(Keys.Add))
            {

                TileManager.Zoom.X = Math.Min(ZOOM_MAX, TileManager.Zoom.X + ZOOM_STEP);
                TileManager.Zoom.Y = Math.Min(ZOOM_MAX, TileManager.Zoom.Y + ZOOM_STEP);
            }
            // Zoom out control
            if (Pad.RightStickDir4 == Direction.Down || Geex.Run.Input.IsPressed(Keys.Subtract))
            {
                TileManager.Zoom.X = Math.Max(1f, TileManager.Zoom.X - ZOOM_STEP);
                TileManager.Zoom.Y = Math.Max(1f, TileManager.Zoom.Y - ZOOM_STEP);
            }
            // Apply shaking Effect
            Graphics.Background.Ox = InGame.Screen.Shake;
            // Apply scroll with Zoom
            TileManager.Ox = InGame.Map.DisplayX;
            TileManager.Oy = InGame.Map.DisplayY;
        }

        /// <summary>
        /// Update Panorama plane
        /// </summary>
        void UpdatePanorama()
        {
            // If Panorama is different from current one
            if (panoramaName != InGame.Map.PanoramaName || panoramaHue != InGame.Map.PanoramaHue)
            {
                panoramaName = InGame.Map.PanoramaName;
                panoramaHue = InGame.Map.PanoramaHue;
                if (panoramaName == "")
                {
                    panorama.Bitmap.Dispose();
                }
                else
                {
                    panorama.Bitmap = Cache.Panorama(panoramaName, panoramaHue);
                    panorama.IsVisible = true;
                }
            } 
            panorama.Ox = InGame.Map.DisplayX / 2;
            panorama.Oy = InGame.Map.DisplayY / 2;
            // Geex Effect
            panorama.GeexEffect = InGame.Map.PanoramaGeexEffect;
        }

        /// <summary>
        /// Update Fog plane
        /// </summary>
        void UpdateFog()
        {
            // If Fog is different than current Fog
            if (fogName != InGame.Map.FogName || fogHue != InGame.Map.FogHue)
            {
                fogName = InGame.Map.FogName;
                fogHue = InGame.Map.FogHue;
                if (fogName == "")
                {
                    fog.Bitmap.Dispose();
                }
                else
                {
                    fog.Bitmap = Cache.Fog(fogName, fogHue);
                    fog.IsVisible = true;
                }
            } 
            fog.ZoomX = InGame.Map.FogZoom;
            fog.ZoomY = InGame.Map.FogZoom;
            fog.Opacity = InGame.Map.FogOpacity;
            fog.BlendType = InGame.Map.FogBlendType;
            fog.Ox = InGame.Map.DisplayX  + (int)InGame.Map.FogOx;
            fog.Oy = InGame.Map.DisplayY + (int)InGame.Map.FogOy;
            double time = Graphics.FrameCount / 300.0;
            // Geex Effect
            fog.GeexEffect = InGame.Map.FogGeexEffect;

        }

        /// <summary>
        /// Update character sprites
        /// </summary>
        void UpdateCharacterSprites()
        {
            for (int i=0;i<characterSprites.Count;i++)
            {
                characterSprites[i].Update();
            }
        }

        /// <summary>
        /// Update weather graphic
        /// </summary>
        void UpdateWeather()
        {
            weather.Type = InGame.Screen.WeatherType;
            weather.Max = InGame.Screen.WeatherMax;
            weather.Ox = InGame.Map.DisplayX;
            weather.Oy = InGame.Map.DisplayY;
            weather.Update();
        }

        /// <summary>
        /// Update Picture sprites
        /// </summary>
        void UpdatePictureSprites()
        {
            foreach (SpritePicture sp in pictureSprites)
            {
                sp.Update();
            }
        }

        /// <summary>
        /// Update Timer sprite
        /// </summary>
        void UpdateTimerSprite()
        {
            timerSprite.Update();

        }
        #endregion

        /// <summary>
        /// Create a new particle
        /// </summary>
        /// <param Name="ev">Event where the particle is created</param>
        /// <param Name="type">The Type of Particle</param>
        public void CreateParticle(GameEvent ev,ParticleEffect type)
        {
            switch (type)
            {
                case ParticleEffect.Flame:
                    spriteParticles.Add(new Flame(ev, Graphics.Background));
                    break;
                case ParticleEffect.Teleport:
                    spriteParticles.Add(new Teleport(ev, Graphics.Background));
                    break;
                case ParticleEffect.Spirit:
                    spriteParticles.Add(new Spirit(ev, Graphics.Background));
                    break;
                case ParticleEffect.Aura:
                    spriteParticles.Add(new Aura(ev, Graphics.Background));
                    break;
                case ParticleEffect.Soot:
                    spriteParticles.Add(new Soot(ev, Graphics.Background));
                    break;
                case ParticleEffect.Fireplace:
                    //spriteParticles.Add(new Fireplace(ev, viewport1));
                    break;
                case ParticleEffect.Smoke:
                    spriteParticles.Add(new Smoke(ev, Graphics.Background));
                    break;
                case ParticleEffect.EventBase:
                    spriteParticles.Add(new EventBase(ev, Graphics.Background));
                    break;
                case ParticleEffect.Splash:
                    //spriteParticles.Add(new Splash(ev, viewport1));
                    break;
                case ParticleEffect.Light:
                    //spriteParticles.Add(new Light(ev, viewport1));
                    break;
                case ParticleEffect.Flare:
                    //spriteParticles.Add(new Flare(ev, viewport1));
                    break;
                default:
                    return;
            }
        }
        #endregion
    }
}