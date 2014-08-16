using System;
using System.Collections.Generic;
using Geex.Edit;
using Geex.Run;
using Microsoft.Xna.Framework;
using Geex.Play.Custom;
using Geex.Play.Rpg.Game;

namespace Geex.Play.Rpg.Spriting
{
    /// <summary>
    /// This class manages SpriteEngine around Sprite.
    /// </summary>
    public partial class SpriteRpg : Sprite
    {
        #region Variables
        int animationPause = 0;
        int animationPriority = 224;
        float animationZoom = 0f;
        /// <summary>
        /// The Bitmap for any Sprite Animation
        /// </summary>
        Bitmap animationBitmap;
        public bool blink;
        int blink_count;

        int whitenDuration;
        int appearDuration;
        int escapeDuration;
        int collapseDuration;
        int damageDuration;
        int animationDuration;

        protected Animation mAnimation;
        bool animationHit;
        Animation mLoopAnimation;
        int loopAnimationIndex;
        public Sprite damageSprite;
        List<Animation> animations = new List<Animation>();
        protected List<Sprite> animationSprites = new List<Sprite>();
        protected List<Sprite> loopAnimationSprites = new List<Sprite>();

        Dictionary<Bitmap, int> referenceCount = new Dictionary<Bitmap, int>();

        #endregion

        #region Properties

        public bool IsEffect
        {
            get
            {
                return whitenDuration > 0 || appearDuration > 0 || escapeDuration > 0
                    || collapseDuration > 0 || damageDuration > 0 || animationDuration > 0;
            }
        }


        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="viewport">Sprites viewport</param>
        public SpriteRpg(Viewport viewport)
            : base(viewport)
        {
            whitenDuration = 0;
            appearDuration = 0;
            escapeDuration = 0;
            collapseDuration = 0;
            damageDuration = 0;
            animationDuration = 0;
            blink = false;
        }

        /// <summary>
        /// Constructor (with null viewport)
        /// </summary>
        public SpriteRpg()
            : base()
        {
            whitenDuration = 0;
            appearDuration = 0;
            escapeDuration = 0;
            collapseDuration = 0;
            damageDuration = 0;
            animationDuration = 0;
            blink = false;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            DisposeDamage();
            DisposeAnimation();
            DisposeLoopAnimation();
            base.Dispose();
        }

        void DisposeDamage()
        {
            if (damageSprite != null)
            {
                damageSprite.Bitmap.Dispose();
                damageSprite.Dispose();
                damageSprite = null;
                damageDuration = 0;
            }
        }

        /// <summary>
        /// Dispose Animation
        /// </summary>
        void DisposeAnimation()
        {
            if (animationSprites != null && animationSprites.Count != 0)
            {
                Sprite sprite = animationSprites[0];
                if (sprite != null)
                {
                    referenceCount[sprite.Bitmap] -= 1;

                    if (referenceCount[sprite.Bitmap] == 0)
                    {
                        sprite.Bitmap.Dispose();
                    }
                }
                foreach (Sprite disposed_sprite in animationSprites)
                {
                    disposed_sprite.Dispose();
                }
                animationSprites.Clear();
                animationSprites = null;
            }
            mAnimation = null;
        }

        /// <summary>
        /// Dispose Loop Animation
        /// </summary>
        void DisposeLoopAnimation()
        {
            if (loopAnimationSprites != null && loopAnimationSprites.Count != 0)
            {
                Geex.Run.Sprite sprite = loopAnimationSprites[0];
                if (sprite != null)
                {
                    referenceCount[sprite.Bitmap] -= 1;

                    if (referenceCount[sprite.Bitmap] == 0)
                    {
                        sprite.Bitmap.Dispose();
                    }
                }
                foreach (Geex.Run.Sprite _disposedSprite in loopAnimationSprites)
                {
                    _disposedSprite.Dispose();
                }
                loopAnimationSprites.Clear();
                loopAnimationSprites = null;
            }
            mLoopAnimation = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Whiten
        /// </summary>
        public void Whiten()
        {
            //this.BlendType = 0;
            //this.Color = new Color(255, 255, 255, 128);
            //this.Opacity = 255;
            //whitenDuration = 16;
            Flash(new Color(255, 255, 255, 128), 16);
        }

        /// <summary>
        /// Appear
        /// </summary>
        public void Appear()
        {
            this.BlendType = 0;
            //this.Color = new Color(0, 0, 0, 0);
            this.Opacity = 0;
            appearDuration = 16;
        }

        /// <summary>
        /// Escape
        /// </summary>
        public void Escape()
        {
            this.BlendType = 0;
           // this.Color = new Color(0, 0, 0, 0);
            this.Opacity = 255;
            escapeDuration = 32;
        }

        /// <summary>
        /// Collapse
        /// </summary>
        public void Collapse()
        {
            this.BlendType = 1;
            this.Color = new Color(255, 64, 64, 255);
            this.Opacity = 255;
            collapseDuration = 48;
        }

        /// <summary>
        /// Damage
        /// </summary>
        /// <param Name="value">Damage value (int)</param>
        /// <param Name="critical">True if critical</param>
        public void Damage(int value, bool critical)
        {
            DisposeDamage();
            string damage_string = (Math.Abs(value)).ToString();
            Bitmap bitmap = DamageDrawBitmap(damage_string);
            if (value < 0)
            {
                bitmap.Font.Color = new Color(176, 255, 144);
            }
            DamageDrawSprite(damage_string, critical, bitmap);
        }

        /// <summary>
        /// Damage
        /// </summary>
        /// <param Name="value">Damage value (string)</param>
        /// <param Name="critical">critical flag</param>
        public void Damage(string value, bool critical)
        {
            DisposeDamage();
            string damage_string = value;
            Bitmap bitmap = DamageDrawBitmap(damage_string);
            bitmap.Font.Color = new Color(255, 255, 255);
            DamageDrawSprite(damage_string, critical, bitmap);
        }

        /// <summary>
        /// Draw Damage Bitmap
        /// </summary>
        /// <param Name="damage_string">damage string</param>
        /// <returns>damage bitmap</returns>
        Bitmap DamageDrawBitmap(string damage_string)
        {
            Bitmap bitmap = new Bitmap(160, 48);
            bitmap.Font.Name = GeexEdit.DefaultFont;
            bitmap.Font.Size = GeexEdit.DefaultFontSize + 10;
            bitmap.Font.Color = new Color(0, 0, 0);
            bitmap.DrawText(-1, 12 - 1, 160, 36, damage_string, 1);
            bitmap.DrawText(+1, 12 - 1, 160, 36, damage_string, 1);
            bitmap.DrawText(-1, 12 + 1, 160, 36, damage_string, 1);
            bitmap.DrawText(+1, 12 + 1, 160, 36, damage_string, 1);
            return bitmap;
        }

        /// <summary>
        /// Damage Draw Sprite
        /// </summary>
        /// <param Name="damage_string">damage string</param>
        /// <param Name="critical">critical flag</param>
        /// <param Name="bitmap">damage bitmap</param>
        void DamageDrawSprite(string damage_string, bool critical, Bitmap bitmap)
        {
            bitmap.DrawText(0, 12, 160, 36, damage_string, 1);
            if (critical)
            {
                bitmap.Font.Size = GeexEdit.DefaultFontSize;
                bitmap.Font.Color = new Color(0, 0, 0);
                bitmap.DrawText(-1, -1, 160, 20, "CRITICAL", 1);
                bitmap.DrawText(+1, -1, 160, 20, "CRITICAL", 1);
                bitmap.DrawText(-1, +1, 160, 20, "CRITICAL", 1);
                bitmap.DrawText(+1, +1, 160, 20, "CRITICAL", 1);
                bitmap.Font.Color = new Color(255, 255, 255);
                bitmap.DrawText(0, 0, 160, 20, "CRITICAL", 1);
            }
            damageSprite = new Geex.Run.Sprite(this.Viewport);
            damageSprite.Bitmap = bitmap;
            damageSprite.Ox = 80;
            damageSprite.Oy = 20;
            damageSprite.X = this.X;
            damageSprite.Y = (int)(this.Y - this.Oy / 2);
            damageSprite.Z = 3000;
            damageDuration = 40;
        }

        /// <summary>
        /// Set Animation
        /// </summary>
        /// <param Name="Animation">Animation</param>
        /// <param Name="Hit">true if Hit</param>
        protected void animation(Animation animation, bool hit)
        {
            DisposeAnimation();
            this.mAnimation = animation;
            if (animation == null)
            {
                return;
            }
            animationHit = hit;
            animationDuration = animation.FrameMax + 1;
            string animation_name = animation.AnimationName;
            int animation_hue = animation.AnimationHue;
            animationBitmap = Cache.Animation(animation_name, animation_hue);
            if (referenceCount != null && referenceCount.ContainsKey(animationBitmap))
            {
                referenceCount[animationBitmap] += 1;
            }
            else
            {
                referenceCount[animationBitmap] = 1;
            }
            if (animationSprites != null) animationSprites.Clear();
            if (animation.Position != 3 || !animations.Contains(animation))
            {
                for (int i = 0; i < animation.Frames[0].CellMax; i++)
                {
                    Geex.Run.Sprite sprite = new Geex.Run.Sprite(this.Viewport);
                    sprite.Bitmap = animationBitmap;
                    sprite.IsVisible = false;
                    if (animationSprites == null) animationSprites = new List<Run.Sprite>();
                    animationSprites.Add(sprite);
                }
                if (animations == null || !animations.Contains(animation))
                {
                    animations.Add(animation);
                }
            }
        }

        protected void animation(Animation animation, bool hit, int pause, int priority, int zoom)
        {
            animationPause = pause + 2;
            animationPriority = priority * 32;
            animationZoom = zoom / 100f;
            this.animation(animation, hit);
        }

        /// <summary>
        /// Set Loop Animation
        /// </summary>
        /// <param Name="Animation">Animation</param>
        protected void LoopAnimation(Animation animation)
        {
            // If Id is 0, animation is to be disposed
            if (animation != null && animation.Id == 0)
            {
                DisposeLoopAnimation();
                return;
            }
            if (animation == mLoopAnimation || animation.Frames == null)
            {
                return;
            }
            DisposeLoopAnimation();
            mLoopAnimation = animation;
            if (mLoopAnimation == null || mLoopAnimation.Frames == null)
            {
                return;
            }
            loopAnimationIndex = 0;
            string animation_name = mLoopAnimation.AnimationName;
            int animation_hue = mLoopAnimation.AnimationHue;
            Bitmap bitmap = Cache.Animation(animation_name, animation_hue);
            if (referenceCount.ContainsKey(bitmap))
            {
                referenceCount[bitmap] += 1;
            }
            else
            {
                referenceCount[bitmap] = 1;
            }
            if (loopAnimationSprites != null)
            {
                loopAnimationSprites.Clear();
            }
            else
            {
                loopAnimationSprites = new List<Sprite>();
            }
            for (int i = 0; i < animation.Frames[0].CellMax; i++)
            {
                Geex.Run.Sprite sprite = new Geex.Run.Sprite(this.Viewport);
                sprite.Bitmap = bitmap;
                sprite.IsVisible = false;
                loopAnimationSprites.Add(sprite);
            }
        }

        /// <summary>
        /// Blink On
        /// </summary>
        public void BlinkOn()
        {
            if (!blink)
            {
                blink = true;
                blink_count = 0;
            }
        }

        /// <summary>
        /// Blink Off
        /// </summary>
        public void BlinkOff()
        {
            if (blink)
            {
                blink = false;
                this.Color = new Color(255, 255, 255, 255);
            }
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public virtual void Update()
        {
            UpdateWhiten();                       // Update Whiten
            UpdateAppear();                       // Update Appear
            UpdateEscape();                       // Update Escape
            UpdateCollapse();                     // Update Collapse
            UpdateDamage();                       // Update Damage
            UpdateAnimationDuration();           // Update Animation Duration
            UpdateLoopAnimationIndex();         // Update Loop Animation
            UpdateBlink();                        // Update Blink
            if (animations != null) animations.Clear();
        }

        /// <summary>
        /// Update Whiten
        /// </summary>
        void UpdateWhiten()
        {
            if (whitenDuration > 0)
            {
                whitenDuration -= 1;
                Color temp_color = Color;
                temp_color.A = (byte)(128 - (16 - whitenDuration) * 10);
                this.Color = temp_color;
            }
        }

        /// <summary>
        /// Update Appear
        /// </summary>
        void UpdateAppear()
        {
            if (appearDuration > 0)
            {
                appearDuration -= 1;
                this.Opacity = (byte)(Math.Min(255,(16 - appearDuration) * 16));
            }
        }

        /// <summary>
        /// Update Escape
        /// </summary>
        void UpdateEscape()
        {

            if (escapeDuration > 0)
            {
                escapeDuration -= 1;
                this.Opacity = (byte)(Math.Min(255,(256 - (32 - escapeDuration) * 10)));
            }
        }

        /// <summary>
        /// Update Collapse
        /// </summary>
        void UpdateCollapse()
        {
            if (collapseDuration > 0)
            {
                collapseDuration -= 1;
                if (collapseDuration != 0 && this.Opacity > 6)
                {
                    this.Opacity = (byte)(256 - (48 - collapseDuration) * 6);
                }
                else
                {
                    this.Opacity = 0;
                    this.IsVisible = false;
                }
            }
        }

        /// <summary>
        /// Update Damage
        /// </summary>
        void UpdateDamage()
        {
            if (damageDuration > 0)
            {
                damageDuration -= 1;
                switch (damageDuration)
                {
                    case 38:
                    case 39:
                        damageSprite.Y -= 4;
                        break;
                    case 36:
                    case 37:
                        damageSprite.Y -= 2;
                        break;
                    case 34:
                    case 35:
                        damageSprite.Y += 2;
                        break;
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                        damageSprite.Y += 4;
                        break;
                }
                if (damageDuration != 0 && damageSprite.Opacity > 16)
                {
                    damageSprite.Opacity = (byte)(damageSprite.Opacity - damageDuration / 4);
                }
                else
                {
                    damageSprite.Opacity = 0;
                }

                if (damageDuration == 0 || damageSprite.Opacity == 0)
                {
                    DisposeDamage();
                }
            }
        }

        /// <summary>
        /// Update Animation Duration
        /// </summary>
        void UpdateAnimationDuration()
        {
            if (mAnimation != null && (Graphics.FrameCount % Math.Max(animationPause, 1) == 0))
            {
                animationDuration -= 1;
                UpdateAnimation();
            }
        }

        /// <summary>
        /// Update Loop Animation Index
        /// </summary>
        void UpdateLoopAnimationIndex()
        {
            if (mLoopAnimation != null && (Graphics.FrameCount % Math.Max(animationPause, 1) == 0))
            {
                UpdateLoopAnimation();
                loopAnimationIndex += 1;
                loopAnimationIndex %= mLoopAnimation.FrameMax;
            }
        }

        /// <summary>
        /// Update Animation
        /// </summary>
        void UpdateAnimation()
        {
            if (animationDuration > 0)
            {
                int frame_index = mAnimation.FrameMax - animationDuration;
                int[,] cell_data = new int[mAnimation.FrameMax, 8];
                for (int i = 0; i < mAnimation.Frames[frame_index].CellDataPattern.Length; i++)
                {
                    cell_data[i, 0] = mAnimation.Frames[frame_index].CellDataPattern[i];
                    cell_data[i, 1] = mAnimation.Frames[frame_index].CellDataXcoordinate[i];
                    cell_data[i, 2] = mAnimation.Frames[frame_index].CellDataYcoordinate[i];
                    cell_data[i, 3] = mAnimation.Frames[frame_index].CellDataZoomLevel[i];
                    cell_data[i, 4] = mAnimation.Frames[frame_index].CellDataAngle[i];
                    cell_data[i, 5] = mAnimation.Frames[frame_index].CellDataHorizontalflip[i];
                    cell_data[i, 6] = mAnimation.Frames[frame_index].CellDataOpacity[i];
                    cell_data[i, 7] = mAnimation.Frames[frame_index].CellDataBlend[i];
                }
                int position = mAnimation.Position;
                AnimationSetSprites(animationSprites, cell_data, position);
                foreach (Animation.Timing timing in mAnimation.Timings)
                {
                    if (timing.Frame == frame_index)
                    {
                        AnimationProcessTiming(timing, animationHit);
                    }
                }
            }
            else
            {
                DisposeAnimation();
            }
        }

        /// <summary>
        /// Update Loop Animation
        /// </summary>
        void UpdateLoopAnimation()
        {
            // TODO distinction entre mLoopAnimation et mAnimation

            int frame_index = loopAnimationIndex;
            int[,] cell_data = new int[mLoopAnimation.FrameMax, 8];
            for (int i = 0; i < mLoopAnimation.Frames[frame_index].CellDataPattern.Length; i++)
            {
                cell_data[i, 0] = mLoopAnimation.Frames[frame_index].CellDataPattern[i];
                cell_data[i, 1] = mLoopAnimation.Frames[frame_index].CellDataXcoordinate[i];
                cell_data[i, 2] = mLoopAnimation.Frames[frame_index].CellDataYcoordinate[i];
                cell_data[i, 3] = mLoopAnimation.Frames[frame_index].CellDataZoomLevel[i];
                cell_data[i, 4] = mLoopAnimation.Frames[frame_index].CellDataAngle[i];
                cell_data[i, 5] = mLoopAnimation.Frames[frame_index].CellDataHorizontalflip[i];
                cell_data[i, 6] = mLoopAnimation.Frames[frame_index].CellDataOpacity[i];
                cell_data[i, 7] = mLoopAnimation.Frames[frame_index].CellDataBlend[i];
            }
            int anim_position = mLoopAnimation.Position;
            AnimationSetSprites(loopAnimationSprites, cell_data, anim_position);
            foreach (Animation.Timing timing in mLoopAnimation.Timings)
            {
                if (timing.Frame == frame_index)
                {
                    AnimationProcessTiming(timing, true);
                }
            }
        }

        /// <summary>
        /// Update Blink
        /// </summary>
        void UpdateBlink()
        {
            if (blink)
            {
                blink_count = (blink_count + 1) % 32;

                int alpha;
                if (blink_count < 16)
                {
                    alpha = (16 - blink_count) * 12;
                }
                else
                {
                    alpha = (blink_count - 16) * 12;
                }
                this.Color = new Color(255, 255, 255, alpha);
            }
        }

        /// <summary>
        /// Set Animation Sprites
        /// </summary>
        /// <param Name="sprites"></param>
        /// <param Name="cell_data"></param>
        /// <param Name="position"></param>
        void AnimationSetSprites(List<Geex.Run.Sprite> sprites, int[,] cell_data, int position)
        {
            if (sprites == null) return;
            for (int i = 0; i < sprites.Count; i++)
            {
                Geex.Run.Sprite sprite = sprites[i];
                int pattern = cell_data[i, 0];

                if (sprite == null || pattern == -1)
                {
                    if (sprite != null)
                    {
                        sprite.IsVisible = false;
                    }
                    continue;
                }
                sprite.IsVisible = true;
                sprite.SourceRect = new Rectangle(pattern % 5 * 192, pattern / 5 * 192, 192, 192);
                if (position == 3)
                {
                    if (this.Viewport != null)
                    {
                        sprite.X = TileManager.Rect.Width / 2;
                        sprite.Y = TileManager.Rect.Height - 160;
                    }
                    else
                    {
                        sprite.X = 320;
                        sprite.Y = 240;
                    }
                }
                else
                {
                    sprite.X = this.X - this.Ox + this.SourceRect.Width / 2;
                    sprite.Y = this.Y - this.Oy + this.SourceRect.Height / 2;
                    if (position == 0)
                    {
                        sprite.Y -= this.SourceRect.Height / 4;
                    }
                    if (position == 2)
                    {
                        sprite.Y += this.SourceRect.Height / 4;
                    }
                }
                sprite.X += cell_data[i, 1];
                sprite.Y += cell_data[i, 2];
                sprite.Z = sprite.Y + animationPriority;
                sprite.Ox = 96;
                sprite.Oy = 96;
                sprite.ZoomX = (cell_data[i, 3] / 100f) + animationZoom;
                sprite.ZoomY = (cell_data[i, 3] / 100f) + animationZoom;
                sprite.Angle = cell_data[i, 4];
                sprite.Mirror = (cell_data[i, 5] == 1);
                sprite.Opacity = (byte)(cell_data[i, 6] * this.Opacity / 255);
                sprite.BlendType = cell_data[i, 7];
            }
        }

        /// <summary>
        /// Animation Process Timing
        /// </summary>
        /// <param Name="timing">animatgion timing</param>
        /// <param Name="Hit">true if Hit</param>
        void AnimationProcessTiming(Animation.Timing timing, bool hit)
        {
            if ((timing.Condition == 0) || (timing.Condition == 1 && hit == true)
                || (timing.Condition == 2 && hit == false))
            {
                if (timing.SoundEffect.Name != "")
                {
                    AudioFile se = timing.SoundEffect;
                    Audio.SoundEffectPlay(se.Name, se.Volume, se.Pitch);
                }
                Color color = new Color(timing.FlashColorRed, timing.FlashColorGreen, timing.FlashColorBlue, timing.FlashColorAlpha);
                switch (timing.FlashScope)
                {
                    case 1:
                        Flash(color, timing.FlashDuration);
                        break;
                    case 2:
                        if (this.Viewport != null)
                        {
                            Viewport.Flash(color, timing.FlashDuration);
                        }
                        break;
                    case 3:
                        Flash(new Color(), timing.FlashDuration);
                        break;
                }
            }
        }

        #endregion
    }
}
