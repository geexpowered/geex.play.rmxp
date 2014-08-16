using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays items in possession on the item and battle screens.
    /// </summary>
    public partial class WindowItem : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// Window Items List
        /// </summary>
        List<Carriable> data = new List<Carriable>();

        #endregion

        #region Properties

        /// <summary>
        /// Get Selected Item
        /// </summary>
        public Carriable Item
        {
            get
            {
                return data.Count == 0 ? null : data[this.Index];
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowItem()
            : base(0, 64, GeexEdit.GameWindowWidth, GeexEdit.GameWindowHeight - 64)
        {
            this.Initialize();
        }

        /// <summary>
        /// Window Initialization
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            // WindowItem initialization
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
        /// Refresh Window, draw items
        /// </summary>
        public void Refresh()
        {
            if (this.Contents != null)
            {
                this.Contents.Dispose();
                this.Contents = null;
            }
            data.Clear();
            // Add item
            for (int i=1; i<=Data.Items.Length ; i++)
            {
                if (InGame.Party.ItemNumber(i) > 0)
                {
                    data.Add(Data.Items[i]);
                }
            }
            // Also add weapons and armors if outside of battle
            if(! InGame.Temp.IsInBattle)
            {
                for (int i=1; i<=Data.Weapons.Length ; i++)
                {
                    if (InGame.Party.WeaponNumber(i) > 0)
                    {
                        data.Add(Data.Weapons[i]);
                    }
                }
                for (int i=1; i<=Data.Armors.Length ; i++)
                {
                    if (InGame.Party.ArmorNumber(i) > 0)
                    {
                        data.Add(Data.Armors[i]);
                    }
                }
            }
            // If item count is not 0, make a bit map and draw all items
            itemMax = data.Count;
            if (itemMax > 0)
            {
                this.Contents = new Bitmap(Width - 32, RowMax * 32);
                for (int i=0 ; i<itemMax ; i++)
                {
                    DrawItem(i);
                }
            }
        }

        /// <summary>
        /// Draw selected item
        /// </summary>
        /// <param Name="index">item index</param>
        public void DrawItem(int index)
        {
            Carriable _item = data[index];
            int _number = 0;
            //Whether the carriable is an item, an armor or a weapon
            switch(_item.GetType().Name.ToString())
            {
                case "Item":
                    _number = InGame.Party.ItemNumber(_item.Id);
                    break;
                case "Weapon":
                    _number = InGame.Party.WeaponNumber(_item.Id);
                    break;
                case "Armor":
                    _number = InGame.Party.ArmorNumber(_item.Id);
                    break;
            }
            // Draw in normal Color usable items, in disabled Color other carriables
            if ((_item.GetType().Name.ToString() == "Item") && InGame.Party.IsItemCanUse(_item.Id))
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
            this.Contents.Blit(posX + index % 2 * 8, posY + 4, Cache.IconBitmap, Cache.IconSourceRect(_item.IconName), _opacity);
            this.Contents.DrawText(posX + 28 * GeexEdit.GameWindowWidth / 640, posY, 212, 32, _item.Name, 0);
            this.Contents.DrawText(posX + 240 * GeexEdit.GameWindowWidth / 640, posY, 16, 32, ":", 1);
            this.Contents.DrawText(posX + 256 * GeexEdit.GameWindowWidth / 640, posY, 24, 32, _number.ToString(), 2);
        }

        /// <summary>
        /// Update help window
        /// </summary>
        public override void UpdateHelp()
        {
            HelpWindow.SetText(this.Item == null ? "" : this.Item.Description);
        }

        #endregion
    }
}
