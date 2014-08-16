using System.Collections.Generic;
using Geex.Play.Custom;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays usable skills on the skill and battle screens.
    /// </summary>
    public partial class WindowSkill : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// Window actor
        /// </summary>
        GameActor actor;

        /// <summary>
        /// Skill list
        /// </summary>
        List<Skill> data = new List<Skill>();

        #endregion

        #region Properties

        /// <summary>
        /// Get current skill
        /// </summary>
        public Skill Skill
        {
            get
            {
                try
                {
                    return data[this.Index];
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="actor">actor</param>
        public WindowSkill(GameActor actor)
            : base(0, 128, GeexEdit.GameWindowWidth, GeexEdit.GameWindowHeight - 128)
        {
            Initialize(actor);
        }

        /// <summary>
        /// Window Initialization
        /// </summary>
        protected void Initialize(GameActor actor)
        {
            base.Initialize();
            // WindowSkill initialization
            this.actor = actor;
            columnMax = 2;
            Refresh();
            if (itemMax > 0)
            {
                this.Index = 0;
            }
            // If in battle, move window to center of screen
            // and make it semi-transparent
            if (InGame.Temp.IsInBattle)
            {
                this.Y = 64;
                this.Height = GeexEdit.GameWindowHeight - 224;
                this.BackOpacity = 160;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            if (this.Contents != null)
            {
                this.Contents.Dispose();
                this.Contents = null;
            }
            data.Clear();
            for (int i = 0 ; i<actor.Skills.Count ; i++)
            {
                Skill _skill = Data.Skills[actor.Skills[i]];
                if (_skill != null)
                {
                    data.Add(_skill);
                }
            }
            // If item count is not 0, make a bitmap and draw all items
            itemMax = data.Count;
            if (itemMax > 0)
            {
                this.Contents = new Bitmap(Width - 32, RowMax * 32);
                for (int i = 0 ; i<itemMax ; i++)
                {
                    DrawItem(i);
                }
            }
        }

        /// <summary>
        /// Draw item
        /// </summary>
        /// <param Name="index">item index</param>
        public void DrawItem(int index)
        {
            Skill skill = data[index];
            if (actor.IsSkillCanUse(skill.Id))
            {
              this.Contents.Font.Color = NormalColor;
            }
            else
            {
              this.Contents.Font.Color = DisabledColor;
            }
            int posX = 4 + index % 2 * (288 * GeexEdit.GameWindowWidth / 640 + 32);
            int posY = index / 2 * 32;
            Rectangle _rect = new Rectangle(posX, posY, this.Width / columnMax - 32, 32);
            this.Contents.FillRect(_rect, new Color(0, 0, 0, 0));
            byte _opacity = this.Contents.Font.Color == NormalColor ? (byte)255 : (byte)128;
            this.Contents.Blit(posX + index % 2 * 8, posY + 4, Cache.IconBitmap, Cache.IconSourceRect(skill.IconName), _opacity);
            this.Contents.DrawText(posX + 28 * GeexEdit.GameWindowWidth / 640, posY, 204, 32, skill.Name, 0);
            this.Contents.DrawText(posX + 232 * GeexEdit.GameWindowWidth / 640, posY, 48, 32, skill.SpCost.ToString(), 2);
        }

        /// <summary>
        /// Update help window
        /// </summary>
        public override void UpdateHelp()
        {
            HelpWindow.SetText(this.Skill == null ? "" : this.Skill.Description);
        }
        #endregion
    }
}
