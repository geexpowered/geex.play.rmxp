using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Play.Custom;
using Microsoft.Xna.Framework;
using Geex.Edit;
using System;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays items in possession for selling on the shop screen.
    /// </summary>
    public partial class WindowShopSell : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// Array containing the items to be drawn.
        /// </summary>
        List<Carriable> data = new List<Carriable>();

        #endregion

        #region Properties

        /// <summary>
        /// Get a specific item
        /// </summary>
        public Carriable Item
        {
            get
            {
                if (this.Index == -1)
                {
                    return null;
                }
                return data[this.Index];
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowShopSell()
            : base(0, 128, GeexEdit.GameWindowWidth, GeexEdit.GameWindowHeight - 128)
        {
            Initialize();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            columnMax = 2;
            Refresh();
            if (itemMax > 0)
            {
                this.Index = 0;
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
            for (int i = 1; i < Data.Items.Length; i++)
            {
                if (InGame.Party.ItemNumber(i) > 0)
                {
                    data.Add(Data.Items[i]);
                }
            }
            for (int i = 1; i < Data.Weapons.Length; i++)
            {
                if (InGame.Party.WeaponNumber(i) > 0)
                {
                    data.Add(Data.Weapons[i]);
                }
            }
            for (int i = 1; i < Data.Armors.Length; i++)
            {
                if (InGame.Party.ArmorNumber(i) > 0)
                {
                    data.Add(Data.Armors[i]);
                }
            }
            // If item count is not 0, make a bitmap and draw all items
            itemMax = data.Count;
            if (itemMax > 0)
            {
                this.Contents = new Bitmap(Width - 32, RowMax * 32);
                for (int i = 0; i < itemMax; i++)
                {
                    DrawItem(i);
                }
            }
            // Refresh index
            this.Index = Math.Min(this.Index, itemMax - 1);
        }

        /// <summary>
        /// Draw a specific item
        /// </summary>
        /// <param Name="index">item index</param>
        public void DrawItem(int index)
        {
            Carriable item = data[index];
            int number = 0;
            switch (item.GetType().Name.ToString())
            {
                case "Item":
                    number = InGame.Party.ItemNumber(item.Id);
                    break;
                case "Weapon":
                    number = InGame.Party.WeaponNumber(item.Id);
                    break;
                case "Armor":
                    number = InGame.Party.ArmorNumber(item.Id);
                    break;
            }
            // If items are sellable, set to valid text Color. If not, set to invalid
            // text Color.
            if (item.Price > 0)
            {
                this.Contents.Font.Color = NormalColor;
            }
            else
            {
                this.Contents.Font.Color = DisabledColor;
            }
            int _x = 4 + index % 2 * ((288 + 32) * GeexEdit.GameWindowWidth / 640) ;
            int _y = index / 2 * 32;
            Rectangle _rect = new Rectangle(_x, _y, this.Width / columnMax - 32, 32);
            this.Contents.FillRect(_rect, new Color(0, 0, 0, 0));
            byte _opacity = this.Contents.Font.Color == NormalColor ? (byte)255 : (byte)128;
            this.Contents.Blit(_x, _y + 4, Cache.IconBitmap, Cache.IconSourceRect(item.IconName), _opacity);
            this.Contents.DrawText(_x + 28 * GeexEdit.GameWindowWidth / 640, _y, 212, 32, item.Name, 0);
            this.Contents.DrawText(_x + 240 * GeexEdit.GameWindowWidth / 640, _y, 16, 32, ":", 1);
            this.Contents.DrawText(_x + 256 * GeexEdit.GameWindowWidth / 640, _y, 24, 32, number.ToString(), 2);
        }

        /// <summary>
        /// Help Text Update
        /// </summary>
        public override void UpdateHelp()
        {
            if (this.itemMax == 0)
            {
                HelpWindow.SetText("");
            }
            else
            {
                HelpWindow.SetText(this.Item == null ? "" : this.Item.Description);
            }
        }

        #endregion
    }
}
