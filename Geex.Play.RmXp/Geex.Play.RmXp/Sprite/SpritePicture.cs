using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Spriting
{
    /// <summary>
    /// This sprite is used to display the Picture.It observes the GameCharacter
    /// class and automatically changes sprite conditions.
    /// </summary>
    public partial class SpritePicture : Sprite
    {
        #region Variables

        /// <summary>
        /// Picture associated with the sprite
        /// </summary>
        protected GamePicture picture;

        /// <summary>
        /// Current Picture Name
        /// </summary>
        protected string pictureName;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="port">viewport on which the sprite is displayed</param>
        /// <param Name="Picture">associated GamePicture</param>
        public SpritePicture(Viewport port, GamePicture picture)
            : base(port)
        {
            this.picture = picture;
            Update();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            // If Picture file Name is different from current one
            if (pictureName != picture.Name)
            {
                // Remember file Name to instance variables
                pictureName = picture.Name;
                // If file Name is not empty
                if (pictureName != "")
                {
                    // Get Picture graphic
                    this.Bitmap = Cache.Picture(pictureName);
                    this.SourceRect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
                }
            }
            // If file Name is empty
            if (pictureName == "")
            {
                // Set sprite to invisible
                this.IsVisible = false;
                return;
            }
            // Set sprite to visible
            this.IsVisible = true;
            // Set transfer starting point
            if (picture.Origin == 0)
            {
                this.Ox = 0;
                this.Oy = 0;
            }
            else
            {
                this.Ox = this.Bitmap.Width / 2;
                this.Oy = this.Bitmap.Height / 2;
            }
            // Set sprite coordinates
            // Locked in map option
            if (picture.IsLocked)
            {
                this.X = picture.X - InGame.Map.DisplayX;
                this.Y = picture.Y - InGame.Map.DisplayY;
            }
            else
            {
                this.X = picture.X;
                this.Y = picture.Y;
            }
            this.Z = picture.Number;
            if (picture.IsBackground)
            {
                this.Viewport = Graphics.Background;
                this.Z = 1;
            }
            else
            {
                this.Z = picture.Number + 1000;
            }
            // Set zoom rate, opacity level, and blend method
            this.ZoomX = picture.ZoomX / 100f;
            this.ZoomY = picture.ZoomY / 100f;
            this.Opacity = picture.Opacity;
            this.BlendType = picture.BlendType;
            // Set rotation angle and Color tone
            this.Angle = picture.Angle;
            this.Tone = picture.ColorTone;
            this.GeexEffect = picture.GeexEffect;
        }

        #endregion
    }
}
