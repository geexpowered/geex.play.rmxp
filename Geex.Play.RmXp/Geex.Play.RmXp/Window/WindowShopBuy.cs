using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Play.Custom;
using Microsoft.Xna.Framework;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays buyable goods on the shop screen.
    /// </summary>
    public partial class WindowShopBuy : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// Shop goods list. List of arrays. In the array, Index 0 is a number representing 
        /// the type of item (0 = Item, 1 = Weapon, 2 = Armor), index 1 is the ID number of that item.
        /// </summary>
        List<int[]> shopGoods = new List<int[]>();

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
                return data[this.Index];
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="shop_goods">Shop goods list</param>
        public WindowShopBuy(List<int[]> shopGoods)
            : base(0, 128, GeexEdit.GameWindowWidth - 272, GeexEdit.GameWindowHeight - 128)
        {
            Initialize(shopGoods);
        }

        protected void Initialize(List<int[]> shopGoods)
        {
            base.Initialize();
            // WindowShopBuy initialization
            this.shopGoods = shopGoods;
            Refresh();
            this.Index = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh, choose items to draw
        /// </summary>
        public void Refresh()
        {
            if (this.Contents != null)
            {
                this.Contents.Dispose();
                this.Contents = null;
            }
            data.Clear();
            Carriable _item = null;
            foreach (int[] goods_item in shopGoods)
            {
                switch (goods_item[0])
                {
                    case 0:
                        _item = Data.Items[goods_item[1]];
                        break;
                    case 1:
                        _item = Data.Weapons[goods_item[1]];
                        break;
                    case 2:
                        _item = Data.Armors[goods_item[1]];
                        break;
                }
                if (_item != null)
                {
                    data.Add(_item);
                }
            }
            // If item count is not 0, make a bit map and draw all items
            itemMax = data.Count;
            if (itemMax > 0)
            {
                this.Contents = new Bitmap(Width - 32, RowMax * 32);
                for (int i = 0; i < itemMax; i++)
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
            Carriable item = data[index];
            // Get items in possession
            int _number = 0;
            switch (item.GetType().Name.ToString())
            {
                case "Item":
                    _number = InGame.Party.ItemNumber(item.Id);
                    break;
                case "Weapon":
                    _number = InGame.Party.WeaponNumber(item.Id);
                    break;
                case "Armor":
                    _number = InGame.Party.ArmorNumber(item.Id);
                    break;
            }
            // If price is less than money in possession, and amount in possession is
            // not GameParty.MAX_ITEM_NUMBER, then set to normal text Color. Otherwise 
            // set to disabled Color
            if (item.Price <= InGame.Party.Gold && _number < GameParty.MAX_ITEM_NUMBER)
            {
                this.Contents.Font.Color = NormalColor;
            }
            else
            {
                this.Contents.Font.Color = DisabledColor;
            }
            int _x = 4;
            int _y = index * 32;
            Rectangle _rect = new Rectangle(_x, _y, this.Width - 32, 32);
            this.Contents.FillRect(_rect, new Color(0, 0, 0, 0));
            byte _opacity = this.Contents.Font.Color == NormalColor ? (byte)255 : (byte)128;
            this.Contents.Blit(_x, _y + 4, Cache.IconBitmap, Cache.IconSourceRect(item.IconName), _opacity);
            this.Contents.DrawText(_x + 28, _y, 212, 32, item.Name, 0);
            this.Contents.DrawText(_x + 240, _y, 88, 32, item.Price.ToString(), 2);
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
