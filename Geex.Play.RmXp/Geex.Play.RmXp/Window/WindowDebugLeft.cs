using System.Text;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window designates switch and variable blocks on the debug screen.
    /// </summary>
    public partial class WindowDebugLeft : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// The number of groups of switches to show
        /// </summary>
        int switchMax;


        /// <summary>
        /// The number of groups of variables to show
        /// </summary>
        int variableMax;

        #endregion

        #region Properties

        /// <summary>
        /// Get Mode
        /// </summary>
        public int Mode
        {
            get
            {
                if (this.Index < switchMax)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Get ID Shown on Top
        /// </summary>
        public int TopId
        {
            get
            {
                if (this.Index < switchMax)
                {
                    return this.Index * 10 + 1;
                }
                else
                {
                    return (this.Index - switchMax) * 10 + 1;
                }
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowDebugLeft()
            : base(0, 0, 192, 480)
        {
            this.Index = 0;
            Refresh();
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
            //switch_max = (Data.System.switches.Size - 1 + 9) / 10;
            //variable_max = (Data.System.variables.Size - 1 + 9) / 10;
            itemMax = switchMax + variableMax;
            this.Contents = new Bitmap(Width - 32, itemMax * 32);
            for (int i = 0; i < switchMax; i++)
            {
                // Draw : S [xxxx-xxxx], with xxxx the switch index
                StringBuilder text = new StringBuilder("S [");
                text.Append(i * 10 + 1);
                text.Append("-");
                text.Append(i * 10 + 10);
                text.Append("]");
                this.Contents.DrawText(4, i * 32, 152, 32, text.ToString());
            }
            for (int i = 0; i < variableMax; i++)
            {
                // Draw : V [xxxx-xxxx], with xxxx the variable index
                StringBuilder text = new StringBuilder("V [");
                text.Append(i * 10 + 1);
                text.Append("-");
                text.Append(i * 10 + 10);
                text.Append("]");
                this.Contents.DrawText(4, (switchMax + i) * 32, 152, 32, text.ToString());
            }
        }

        #endregion
    }
}
