using System.Collections.Generic;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Spriting;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Custom
{
    /// <summary>
    /// Create a structure for GameCharacter Tag. A Tag is a image or text displayed above GameCharacter
    /// </summary>
    public class Tag
    {
        #region Variables
        /// <summary>
        /// Get teh Character on which Tag must be drawn
        /// </summary>
        public GameCharacter Character;
        /// <summary>
        /// Text to be displayed with Tag
        /// </summary>
        public string Text="";
        /// <summary>
        /// Icon to be displayed with Tag
        /// </summary>
        public string Icon="";
        /// <summary>
        /// True if Icon should be displayed below GameCharacter
        /// </summary>
        public bool IsIconDown=false;
        /// <summary>
        /// True if Icon should fade out
        /// </summary>
        public bool IsIconFading=false;
        /// <summary>
        /// Position 0: Center, 1:On the right
        /// </summary>
        public byte Position=0;
        /// <summary>
        /// Color of Tag displayed
        /// </summary>
        public Color TagColor=Color.White;
        /// <summary>
        /// Tag duration
        /// </summary>
        public int Duration=0;
        /// <summary>
        /// Count the number of frame elapsed
        /// </summary>
        public int FrameCounter = 0;
        #endregion

        #region Properties
        /// <summary>
        /// True if Tag is Empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Duration == 0;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a structure for GameCharacter Tag. A Tag is a image or text displayed above GameCharacter
        /// </summary>
        /// <param Name="tagCharacter">GameCharacter to be Tagged</param>
        /// <param Name="tagText">Text to Tag</param>
        /// <param Name="tagIcon">Icon Name to Tag</param>
        /// <param Name="tagDuration">Duration of Tag</param>
        /// <param Name="tagFade">True if Tag should fade out</param>
        /// <param Name="tagIconDown">True if Tag should be display below Character</param>
        public Tag(GameCharacter tagCharacter, string tagText, string tagIcon, int tagDuration, bool tagFade, bool tagIconDown)
        {
            Character = tagCharacter;
            Text = tagText;
            Icon = tagIcon;
            Duration = tagDuration;
            IsIconFading = tagFade;
            IsIconDown = tagIconDown;
        }
        public Tag(GameCharacter tagCharacter, string tagText, string tagIcon, int tagDuration, bool tagFade, bool tagIconDown, Color tagColor, byte tagPosition)
        {
            Character = tagCharacter;
            Text = tagText;
            Icon = tagIcon;
            Duration = tagDuration;
            IsIconFading = tagFade;
            IsIconDown = tagIconDown;
            Position = tagPosition;
            TagColor = tagColor;
        }

        /// <summary>
        /// Create an empty structure for GameCharacter Tag Saving
        /// </summary>
        public Tag() : this(null, "", "", 0, true, false)
        { }
        #endregion
    }

    /// <summary>
    /// Manage the game tag. A Tag is a image or text displayed above GameCharacter
    /// </summary>
    public class Tags
    {
        #region Variables
        /// <summary>
        /// List of Tag to draw
        /// </summary>
        public List<Tag> TagList = new List<Tag>();
        /// <summary>
        /// List of Tag Sprite to be updated
        /// </summary>
        List<SpriteTag> tagSpriteList = new List<SpriteTag>();
        #endregion

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            // Update Tag Sprite list
            List<SpriteTag> toDelete = new List<SpriteTag>();
            foreach (SpriteTag sprite in tagSpriteList)
            {
                UpdateSpriteTag(sprite);
                // terminate Sprite
                if (sprite.TagData.FrameCounter == 0)
                {
                    toDelete.Add(sprite);
                }
            }
            // Delete terminated tag sprites
            foreach (SpriteTag sprite in toDelete)
            {
                sprite.Dispose();
                tagSpriteList.Remove(sprite);
            }
            // Read next Tag
            if (TagList.Count==0) return;
            // Create nex Tag
            SpriteTag tagSprite = new SpriteTag(TagList[0]);
            tagSprite.Bitmap = new Bitmap(GameOptions.IconSize, GameOptions.IconSize);
            tagSprite.Center();
            // Add Icon
            if (TagList[0].Icon != "")
            {
                tagSprite.Bitmap.Blit(0, 0, Cache.IconBitmap, Cache.IconSourceRect(TagList[0].Icon));
            }
            // Add Text
            tagSprite.Bitmap.Font.Size = 16;
            if (TagList[0].Text != "")
            {
                int size = tagSprite.Bitmap.TextSize(TagList[0].Text).Width / 2;
                tagSprite.Bitmap.Font.Color = TagList[0].TagColor;
                switch (TagList[0].Position)
                {
                    case 0:
                        tagSprite.Bitmap.DrawText(0, 0, TagList[0].Text.Length * 16, GameOptions.IconSize, TagList[0].Text, 0, true);
                        break;
                    case 1:
                        tagSprite.Bitmap.DrawText(GameOptions.IconSize, 0, TagList[0].Text.Length * 16, GameOptions.IconSize, TagList[0].Text, 0, true);
                        break;
                }
            }
            TagList[0].FrameCounter = TagList[0].Duration;
            if (TagList[0].IsIconFading) tagSprite.Opacity = 0;
            else tagSprite.Opacity = 255;
            tagSpriteList.Add(tagSprite);
            TagList.RemoveAt(0);
            UpdateSpriteTag(tagSprite);
        }

        /// <summary>
        /// Update Sprite Coordinate
        /// </summary>
        /// <param Name="sprite"></param>
        void UpdateSpriteTag(SpriteTag sprite)
        {
            GameCharacter character = sprite.TagData.Character;
            sprite.X=character.ScreenX;
            if (sprite.TagData.IsIconDown)
            {
                sprite.Y = character.ScreenY + character.CollisionHeight + sprite.Opacity / 20;
            }
            else
            {
                sprite.Y = character.ScreenY - 32-character.CollisionHeight - sprite.Opacity / 20;
            }
            sprite.Ox = GameOptions.IconSize/2;
            sprite.Oy = GameOptions.IconSize/2;
            sprite.Z = character.ScreenZ() + 96;
            // Fade in
            if (sprite.TagData.IsIconFading && sprite.TagData.FrameCounter >= 40 && sprite.Opacity!=255)
            {
                if (sprite.TagData.Duration > 80)
                {
                    if (sprite.Opacity > 245)
                    {
                        sprite.Opacity = 255;
                    }
                    else
                    {
                        sprite.Opacity += 7;
                    }
                }
                else
                {
                    if (sprite.TagData.FrameCounter <= 40) sprite.Opacity = 255;
                    else sprite.Opacity = (byte)((sprite.TagData.Duration - sprite.TagData.FrameCounter) * 255 / (sprite.TagData.Duration - 40));
                }
            }
            // Fade Out on thelast 40 frames
            if (sprite.TagData.IsIconFading && sprite.TagData.FrameCounter<40)
            {
                if (sprite.Opacity < 7)
                {
                    sprite.Opacity = 0;
                }
                else
                {
                    sprite.Opacity -= 7;
                }
            }
            sprite.TagData.FrameCounter-=1;
         }

        /// <summary>
        /// Dispose Tags
        /// </summary>
        public void Dispose()
        {
            foreach (SpriteTag sprite in tagSpriteList)
            {
                sprite.Dispose();
            }
        }
    }
}