using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Play.Custom;

namespace Geex.Play.Rpg.Spriting
{
    /// <summary>
    /// Sprite class for Tag
    /// </summary>
    public class SpriteTag : Geex.Run.Sprite
    {
        /// <summary>
        /// Get or set the Sprite's tag
        /// </summary>
        public Tag TagData;
        /// <summary>
        /// Create a Sprite Tag
        /// </summary>
        /// <param Name="spriteTag"></param>
        public SpriteTag(Tag spriteTag)
            : base(Graphics.Background)
        {
            TagData = spriteTag;
            X = spriteTag.Character.ScreenX;
            Y = spriteTag.Character.ScreenY;
        }
    }
}
