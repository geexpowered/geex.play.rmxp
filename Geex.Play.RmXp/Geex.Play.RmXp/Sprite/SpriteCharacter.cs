using System;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Spriting
{

    /// <summary>
    /// This sprite is used to display the Character.It observes the GameCharacter class and
    /// automatically changes sprite conditions
    /// </summary>
    public partial class SpriteCharacter : SpriteRpg
    {
        #region Variables
        /// <summary>
        /// Sprite Character Name
        /// </summary>
        string characterName;
        /// <summary>
        /// Reference to GameCharacter
        /// </summary>
        public GameCharacter Character;
        /// <summary>
        /// Tile id is Sprite Character is a Tile
        /// </summary>
        int tileId;
        /// <summary>
        /// Character width within graphic
        /// </summary>
        int cw;
        /// <summary>
        /// Character height within graphic
        /// </summary>
        int ch;
        /// <summary>
        /// Character Hue
        /// </summary>
        int characterHue;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Sprite_Character Initialization
        /// </summary>
        /// <param Name="port">viewport</param>
        /// <param Name="_character">Character (GameCharacter)</param>
        public SpriteCharacter(Viewport port, GameCharacter _character) : base(port)
        {
            Character = _character;
            // Set the Character Size
            if (Character.TileId >= 384)
            {
                Character.Cw = 32;
                Character.Ch = 32;
            }
#if XBOX    // As Loading is slower on xbox you have to preload character's bitmap
            UpdateBitmap();
            UpdateCharacter();
#endif
        }
        #endregion

        /// <summary>
        /// Update Sprite Character Bitmap
        /// </summary>
        void UpdateBitmap()
        {
            // If Tile ID, file Name, or hue are different from current ones
            if (tileId != Character.TileId || characterName != Character.CharacterName || characterHue != Character.CharacterHue)
            {
                // Remember Tile ID, file Name, and hue
                tileId = Character.TileId;
                characterHue = Character.CharacterHue;
                // If Tile ID value is valid
                if (tileId >= 384)
                {
                    SetAsTile(tileId);
                    this.Ox = 16;
                    this.Oy = 32;
                    Character.Cw = 32;
                    Character.Ch = 32;
                }
                else
                {
                    if (characterName != Character.CharacterName)
                    {
                        this.Bitmap = Cache.Character(Character.CharacterName, Character.CharacterHue);
                    }
                    cw = Bitmap.Width / GameOptions.CharacterPatterns;
                    ch = Bitmap.Height / GameOptions.CharacterDirections;
                    this.Oy = ch;
                    this.Ox = cw / 2;
                    Character.Cw = cw;
                    Character.Ch = ch;
                }
                characterName = Character.CharacterName;
            }
        }

        /// <summary>
        /// Dispose Sprite Character
        /// </summary>
        public void Dispose()
        {
            this.Bitmap.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Update Character variables
        /// </summary>
        void UpdateCharacter()
        {
            // Set visible situation
            this.IsVisible = !(Character.IsTransparent || Character.IsErased);
            // If graphic is Character
            if (tileId == 0)
            {
                // Set rectangular transfer
                this.SourceRect.X = Character.Pattern * cw;
                this.SourceRect.Y = (Character.Dir - 2) / 2 * ch;
                this.SourceRect.Width = cw;
                this.SourceRect.Height = ch;
            }
            // Set sprite coordinates
            this.X = Character.ScreenX;
            this.Y = Character.ScreenY;
            this.Z = Character.ScreenZ(ch);
            // Z adjustment fo GamePlayer
            if(this.Character.GetType().Name.ToString()=="GamePlayer")
                this.Z++;
            this.ZoomX = Character.ZoomX;
            this.ZoomY = Character.ZoomY;
            this.Angle = Character.Angle;
            // Set opacity level, blend method, and bush depth
            this.Opacity = Character.Opacity;
            this.BlendType = Character.BlendType;
            this.BushDepth = Character.BushDepth;
            // Animation
            if (Character.AnimationId != 0)
            {
                Animation animation = Data.Animations[Character.AnimationId];
                this.animation(animation, true, Character.AnimationPause, Character.AnimationPriority, Character.AnimationZoom);
                Character.AnimationId = 0;
            }
            GeexEffect = Character.GeexEffect;
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            // Do not refresh if out of screen
            if (!Character.IsOnScreen && Character.IsAntilag) return;
            // refresh
            base.Update();
            UpdateBitmap();
            UpdateCharacter();
        }
    }
}
