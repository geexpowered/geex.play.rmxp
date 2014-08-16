using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays switches and variables separately on the debug screen.
    /// </summary>
    public partial class WindowDebugRight : WindowSelectable
    {
        #region Properties

        /// <summary>
        /// mode (0: switch, 1: variable)
        /// </summary>
        public int Mode
        {
            get { return localMode; }
            set
            {
                if (this.localMode != value)
                {
                    this.localMode = value;
                    Refresh();
                }
            }
        }
        int localMode;

        /// <summary>
        /// ID shown on top
        /// </summary>
        public int TopId
        {
            get { return localTopId; }
            set
            {
                if (localTopId != value)
                {
                    localTopId = value;
                    Refresh();
                }
            }
        }
        int localTopId;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowDebugRight()
            : base(192, 0, 448, 352)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.Index = -1;
            this.IsActive = false;
            itemMax = 10;
            Mode = 0;
            TopId = 1;
            Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            string name = "";
            string status = "";
            for (int i = 0; i < 9; i++)
            {
                if (Mode == 0)
                {
                    //Name = Data.System.switches[top_id + i];
                    //status = InGame.Switches.datas[top_id + i] ? "[ON]" : "[OFF]";
                }
                else
                {
                    //Name = Data.System.variables[top_id + i];
                    //status = InGame.game_variables[top_id + i].ToString();
                }
                if (name == null)
                {
                    name = "";
                }
                string id_text = (TopId + i).ToString();
                Width = this.Contents.TextSize(id_text).Width;
                this.Contents.DrawText(4, i * 32, Width, 32, id_text);
                this.Contents.DrawText(12 + Width, i * 32, 296 - Width, 32, name);
                this.Contents.DrawText(312, i * 32, 100, 32, status, 2);
            }
        }

        #endregion
    }
}
