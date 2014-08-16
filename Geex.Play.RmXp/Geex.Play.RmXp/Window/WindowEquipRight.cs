using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays items the actor is currently equipped with on the
    /// equipment screen.
    /// </summary>
    public partial class WindowEquipRight : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// Equipped actor
        /// </summary>
        GameActor actor;

        /// <summary>
        /// Equipment list
        /// </summary>
        List<Carriable> data = new List<Carriable>();

        #endregion

        #region Properties

        /// <summary>
        /// Item Acquisition
        /// </summary>
        public Carriable Item
        {
            get
            {
                return data[this.Index];
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="actor">Equipped actor</param>
        public WindowEquipRight(GameActor actor)
            : base(272, 64, GeexEdit.GameWindowWidth - 272, 192)
        {
            Initialize(actor);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="actor">Equipped actor</param>
        protected void Initialize(GameActor actor)
        {
            base.Initialize();
            // WindowEquipRight initialization
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.actor = actor;
            Refresh();
            this.Index = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh, draw actor equipement
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            data.Clear();
            data.Add(Data.Weapons[actor.WeaponId]);
            data.Add(Data.Armors[actor.ArmorShield]);
            data.Add(Data.Armors[actor.ArmorHelmet]);
            data.Add(Data.Armors[actor.ArmorBody]);
            data.Add(Data.Armors[actor.ArmorAccessory]);
            itemMax = data.Count;
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(4, 32 * 0, 92, 32, Data.System.Wordings.Weapon);
            this.Contents.DrawText(4, 32 * 1, 92, 32, Data.System.Wordings.Armor1);
            this.Contents.DrawText(4, 32 * 2, 92, 32, Data.System.Wordings.Armor2);
            this.Contents.DrawText(4, 32 * 3, 92, 32, Data.System.Wordings.Armor3);
            this.Contents.DrawText(5, 32 * 4, 92, 32, Data.System.Wordings.Armor4);
            DrawItemName(data[0], 92 * GeexEdit.GameWindowWidth / 640, 32 * 0);
            DrawItemName(data[1], 92 * GeexEdit.GameWindowWidth / 640, 32 * 1);
            DrawItemName(data[2], 92 * GeexEdit.GameWindowWidth / 640, 32 * 2);
            DrawItemName(data[3], 92 * GeexEdit.GameWindowWidth / 640, 32 * 3);
            DrawItemName(data[4], 92 * GeexEdit.GameWindowWidth / 640, 32 * 4);
        }

        /// <summary>
        /// Help Text Update
        /// </summary>
        public override void UpdateHelp()
        {
            HelpWindow.SetText(this.Item == null ? "" : this.Item.Description);
        }

        #endregion
    }
}
