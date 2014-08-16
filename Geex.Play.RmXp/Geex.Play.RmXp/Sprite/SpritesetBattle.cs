using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Spriting;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Spriteset
{
    /// <summary>
    /// This class brings together battle screen sprites. It's used within
    /// the SceneBattle class.</summary>
    public partial class Spriteset_Battle
    {
        #region Variables
        /// <summary>
        /// Battleback sprite
        /// </summary>
        protected Geex.Run.Sprite battlebackSprite;

        /// <summary>
        /// Battleback Name
        /// </summary>
        protected string battlebackName;

        /// <summary>
        /// Npc sprites
        /// </summary>
        protected List<SpriteBattler> enemySprites = new List<SpriteBattler>();

        /// <summary>
        /// Actor sprites
        /// </summary>
        protected List<SpriteBattler> actorSprites = new List<SpriteBattler>();

        /// <summary>
        /// Weather sprite
        /// </summary>
        protected Weather weather;

        /// <summary>
        /// Picture sprites
        /// </summary>
        protected List<SpritePicture> pictureSprites = new List<SpritePicture>();

        /// <summary>
        /// Timer sprite
        /// </summary>
        protected SpriteTimer timerSprite;

        #endregion

        #region Properties

        /// <summary>
        /// Determine if Effects are Displayed
        /// </summary>
        public bool IsEffect
        {
            get
            {
                foreach (SpriteBattler sprite in enemySprites)
                {
                    if (sprite.IsEffect)
                    {
                        return true;
                    }
                }
                foreach (SpriteBattler sprite in actorSprites)
                {
                    if (sprite.IsEffect)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public Spriteset_Battle()
        {
            InitializeBattleback();                           // Initialize Battleback
            InitializeEnemySprites();                         // Initialize Enemy Sprites
            InitializeActorSprites();                         // Initialize Actor Sprites
            InitializePictureSprites();                       // Initialize Picture Sprites
            InitializeWeather();                              // Initialize Weather
            InitializeTimer();                                // Initialize Timer
            Update();                                    // Frame Update
        }


        /// <summary>
        /// Object Initialization : Battleback Sprite
        /// </summary>
        public void InitializeBattleback()
        {
            // Make Battleback sprite
            battlebackSprite = new Geex.Run.Sprite(Graphics.Background);
        }

        /// <summary>
        /// Object Initialization : Npc Sprites
        /// </summary>
        public void InitializeEnemySprites()
        {
            enemySprites.Clear();
            // Reverse npc list
            List<GameNpc> _npc_list = InGame.Troops.Npcs;
            //_npc_list.Reverse();
            // Make npc sprites
            foreach (GameNpc npc in _npc_list)
            {
                enemySprites.Add(new SpriteBattler(Graphics.Background, npc));
            }
        }

        /// <summary>
        /// Object Initialization : Actor Sprites
        /// </summary>
        public void InitializeActorSprites()
        {
            // Make actor sprites
            actorSprites.Clear();
            actorSprites.Add(new SpriteBattler(Graphics.Foreground));
            actorSprites.Add(new SpriteBattler(Graphics.Foreground));
            actorSprites.Add(new SpriteBattler(Graphics.Foreground));
            actorSprites.Add(new SpriteBattler(Graphics.Foreground));
        }

        /// <summary>
        /// Object Initialization : Pictures
        /// </summary>
        public void InitializePictureSprites()
        {
            // Make Picture sprites
            pictureSprites.Clear();
            foreach (GamePicture pic in InGame.Screen.BattlePictures)
            {
                pictureSprites.Add(new SpritePicture(Graphics.Foreground, pic));
            }
        }

        /// <summary>
        /// Object Initialization : Weather
        /// </summary>
        public void InitializeWeather()
        {
            // Make weather
            weather = new Weather(Graphics.Background);
        }

        /// <summary>
        /// Object Initialization : Timer
        /// </summary>
        public void InitializeTimer()
        {
            // Make Timer sprite
            timerSprite = new SpriteTimer();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // If Battleback bit map exists, Dispose of it
            if (battlebackSprite.Bitmap != null)
            {
                battlebackSprite.Bitmap.Dispose();
            }
            // Disposal of Battleback sprite
            battlebackSprite.Dispose();
            // Disposal of enemy sprites and actor sprites
            foreach (SpriteBattler sprite in enemySprites)
            {
                sprite.Dispose();
            }
            foreach (SpriteBattler sprite in actorSprites)
            {
                sprite.Dispose();
            }
            // Disposal of weather
            if (weather != null)
            {
                weather.Dispose();
            }
            // Disposal of Picture sprites
            foreach (SpritePicture sprite in pictureSprites)
            {
                sprite.Dispose();
            }
            // Disposal of Timer sprite
            if (timerSprite != null)
            {
                timerSprite.Dispose();
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            UpdateBattleback();                         // Update Battleback
            UpdateBattlers();                           // Update Actor Battlers
            UpdateSprites();                            // Update Sprites
            UpdateWeather();                            // Update Weather
            UpdateTimer();                              // Update Timer
            UpdateViewports();                          // Update Viewports
        }

        /// <summary>
        /// Frame Update : Battleback
        /// </summary>
        public void UpdateBattleback()
        {
            // If Battleback file Name is different from current one
            if (battlebackName != InGame.Temp.BattlebackName)
            {
                battlebackName = InGame.Temp.BattlebackName;
                if (battlebackSprite.Bitmap != null)
                {
                    battlebackSprite.Bitmap.Dispose();
                }
                battlebackSprite.Bitmap = Cache.Battleback(battlebackName);
            }
        }

        /// <summary>
        /// Frame Update : Actor Battlers
        /// </summary>
        void UpdateBattlers()
        {
            // Battler index
            short i = 0;
            // Update actor sprite contents (corresponds with actor switching)
            foreach (GameActor actor in InGame.Party.Actors)
            {
                actorSprites[i].battler = actor;
                i++;
            }
        }

        /// <summary>
        /// Frame Update : Sprites
        /// </summary>
        void UpdateSprites()
        {
            // Update Battler sprites
            foreach (SpriteBattler sprite in enemySprites)
            {
                sprite.Update();
            }
            foreach (SpriteBattler sprite in actorSprites)
            {
                sprite.Update();
            }
            foreach (SpritePicture sprite in pictureSprites)
            {
                sprite.Update();
            }
        }

        /// <summary>
        /// Frame Update : Weather
        /// </summary>
        void UpdateWeather()
        {
            // Update weather graphic
            weather.Type = InGame.Screen.WeatherType;
            weather.Max = InGame.Screen.WeatherMax;
            weather.Update();
        }

        /// <summary>
        /// Frame Update : Timer
        /// </summary>
        void UpdateTimer()
        {
            // Update Timer sprite
            timerSprite.Update();
        }

        /// <summary>
        /// Frame Update : Viewports
        /// </summary>
        void UpdateViewports()
        {
            // Set screen Color tone and shake position
            Graphics.Background.Tone = InGame.Screen.ColorTone;
            Graphics.Background.Ox = (int)InGame.Screen.Shake;
            // Set screen flash Color
            //Graphics.Background.Color = InGame.Screen.FlashColor;
        }

        #endregion
    }
}
