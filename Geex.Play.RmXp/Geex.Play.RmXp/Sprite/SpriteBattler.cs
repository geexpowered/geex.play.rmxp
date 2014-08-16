using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Spriting
{
    /// <summary>
    /// This window displays buyable goods on the shop screen.
    /// </summary>
    public partial class SpriteBattler : SpriteRpg
    {
        #region Variables

        /// <summary>
        /// Battler object associated with this sprite
        /// </summary>
        public GameBattler battler;

        /// <summary>
        /// True if Battler sprite is visible
        /// </summary>
        bool isBattlerVisible;

        /// <summary>
        /// Current Battler Name
        /// </summary>
        string battlerName;

        /// <summary>
        /// Current Battler hue
        /// </summary>
        int battlerHue;

        /// <summary>
        /// Current state Animation id
        /// </summary>
        int stateAnimationId;

        /// <summary>
        /// Sprite width
        /// </summary>
        int width;

        /// <summary>
        /// Sprite height
        /// </summary>
        int height;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="viewport">viewport</param>
        /// <param Name="Battler">associated GameBattler</param>
        public SpriteBattler(Viewport viewport, GameBattler battler)
            : base(viewport)
        {
            this.battler = battler;
            isBattlerVisible = false;
            Z = 101;
        }

        /// <summary>
        /// Constructor (Battler = null)
        /// </summary>
        /// <param Name="viewport">viewport on which sprite is displayed</param>
        public SpriteBattler(Viewport viewport)
            : base(viewport)
        {
            this.battler = null;
            isBattlerVisible = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            // If Battler is null
            if (battler == null)
            {
                RemoveBattler();
                return;
            }
            // If file Name or hue are different than current ones
            RedrawBattler();
            // If Animation ID is different than current one
            LoopAnim();
            // If actor which should be displayed
            AdjustActorOpacity();
            // Blink
            AdjustBlink();
            // If invisible
            AdjustVisibility();
            // If visible
            if (isBattlerVisible)
            {
                // Escape
                SpriteEscape();
                // White flash
                SpriteWhiteFlash();
                // Animation
                SpriteAnimation();
                // Damage
                SpriteDamage();
                // Collapse
                SpriteCollapse();
            }
            // Set sprite coordinates
            SpritePosition();
        }

        /// <summary>
        /// Remove Battler
        /// </summary>
        void RemoveBattler()
        {
            this.Bitmap = null;
            LoopAnimation(null);
        }

        /// <summary>
        /// Redraw Battler
        /// </summary>
        void RedrawBattler()
        {
            if (battler.BattlerName != battlerName ||
                battler.BattlerHue != battlerHue)
            {
                // Get and set bitmap
                battlerName = battler.BattlerName;
                battlerHue = battler.BattlerHue;
                this.Bitmap = Cache.Battler(battlerName, battlerHue);
                width = Bitmap.Width;
                height = Bitmap.Height;
                this.Ox = width / 2;
                this.Oy = height;
                // Change opacity level to 0 when dead or hidden
                if (battler.IsDead || battler.IsHidden)
                {
                    this.Opacity = 0;
                }
            }
        }

        /// <summary>
        /// Loop Sprite Animation
        /// </summary>
        void LoopAnim()
        {
            if (battler.StateAnimationId != stateAnimationId)
            {
                stateAnimationId = battler.StateAnimationId;
                LoopAnimation(Data.Animations[stateAnimationId]);
            }
        }

        /// <summary>
        /// Adjust Actor Opacity
        /// </summary>
        void AdjustActorOpacity()
        {
            if (battler.GetType().Name.ToString() == "Geex.Play.Rpg.Game.GameActor" && isBattlerVisible)
            {
                // Bring opacity level down a bit when not in main phase
                if (InGame.Temp.BattleMainPhase)
                {
                    if (this.Opacity < 255)
                    { this.Opacity += 3; }
                }
                else
                {
                    if (this.Opacity > 207)
                    { this.Opacity -= 3; }
                }
            }
        }

        /// <summary>
        /// Adjust Blink
        /// </summary>
        void AdjustBlink()
        {
            if (battler.IsBlink)
            {
                BlinkOn();
            }
            else
            {
                BlinkOff();
            }
        }

        /// <summary>
        /// Adjust Visibility
        /// </summary>
        void AdjustVisibility()
        {
            if (!isBattlerVisible)
            {
                // Appear
                if (!battler.IsHidden && !battler.IsDead &&
                   (battler.Damage == null || battler.Damage == "" || battler.IsDamagePop))
                {
                    Appear();
                    isBattlerVisible = true;
                }
            }
        }
        /// <summary>
        /// Escape
        /// </summary>
        void SpriteEscape()
        {
            if (battler.IsHidden)
            {
                Audio.SoundEffectPlay(Data.System.EscapeSoundEffect);
                Escape();
                isBattlerVisible = false;
            }
        }

        /// <summary>
        /// Sprite: White Flash
        /// </summary>
        void SpriteWhiteFlash()
        {
            if (battler.IsWhiteFlash)
            {
                Whiten();
                battler.IsWhiteFlash = false;
            }
        }

        /// <summary>
        /// Sprite: Animation
        /// </summary>
        void SpriteAnimation()
        {
            if (battler.AnimationId != 0)
            {
                mAnimation = Data.Animations[battler.AnimationId];
                this.animation(mAnimation, battler.IsAnimationHit, 0, 7, 0);
                battler.AnimationId = 0;
            }
        }

        /// <summary>
        /// Sprite: Damage
        /// </summary>
        void SpriteDamage()
        {
            if (battler.IsDamagePop)
            {
                Damage(battler.Damage, battler.IsCritical);
                battler.Damage = null;
                battler.IsCritical = false;
                battler.IsDamagePop = false;
            }
        }

        /// <summary>
        /// Sprite: Collapse
        /// </summary>
        void SpriteCollapse()
        {
            if (battler.Damage == null && battler.IsDead)
            {
                if (battler.GetType().Name.ToString() == "GameNpc")
                {
                    Audio.SoundEffectPlay(Data.System.EnemyCollapseSoundEffect);
                }
                else
                {
                    Audio.SoundEffectPlay(Data.System.ActorCollapseSoundEffect);
                }
                Collapse();
                isBattlerVisible = false;
            }
        }

        /// <summary>
        /// Sprite: Position
        /// </summary>
        void SpritePosition()
        {
            this.X = battler.ScreenX;
            this.Y = battler.ScreenY;
            this.Z = battler.ScreenZ;
        }

        #endregion
    }
}
