using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays actor parameter changes on the equipment screen.
    /// </summary>
    public partial class WindowEquipLeft : WindowBase
    {
        #region Variables

        /// <summary>
        /// Equipped actor
        /// </summary>
        GameActor actor;

        /// <summary>
        /// New ATK
        /// </summary>
        int? newAtk;

        /// <summary>
        /// New Physical Defense
        /// </summary>
        int? newPdef;

        /// <summary>
        /// New Magical Defense
        /// </summary>
        int? newMdef;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="actor">Equipped actor</param>
        public WindowEquipLeft(GameActor actor)
            : base(0, 64, 272, 192)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.actor = actor;
            Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh, draw new actor parameters
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            DrawActorName(actor, 4, 0);
            DrawActorLevel(actor, 4, 32);
            DrawActorParameter(actor, 4, 64, 0);
            DrawActorParameter(actor, 4, 96, 1);
            DrawActorParameter(actor, 4, 128, 2);
            if (newAtk != null)
            {
                this.Contents.Font.Color = SystemColor;
                this.Contents.DrawText(160, 64, 40, 32, "->", 1);
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(200, 64, 36, 32, newAtk.ToString(), 2);
            }
            if (newPdef != null)
            {
                this.Contents.Font.Color = SystemColor;
                this.Contents.DrawText(160, 96, 40, 32, "->", 1);
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(200, 96, 36, 32, newPdef.ToString(), 2);
            }
            if (newMdef != null)
            {
                this.Contents.Font.Color = SystemColor;
                this.Contents.DrawText(160, 128, 40, 32, "->", 1);
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(200, 128, 36, 32, newMdef.ToString(), 2);
            }
        }

        /// <summary>
        /// Set parameters after changing equipment
        /// </summary>
        /// <param Name="new_atk">attack power after changing equipment</param>
        /// <param Name="new_pdef">physical defense after changing equipment</param>
        /// <param Name="new_mdef">magic defense after changing equipment</param>
        public void SetNewParameters(int? new_atk, int? new_pdef, int? new_mdef)
        {
            if (this.newAtk != new_atk || this.newPdef != new_pdef || this.newMdef != new_mdef)
            {
                this.newAtk = new_atk;
                this.newPdef = new_pdef;
                this.newMdef = new_mdef;
                Refresh();
            }
        }

        #endregion
    }
}
