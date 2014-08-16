using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Play.Custom;
using Microsoft.Xna.Framework;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays number of items in possession and the actor's equipment
    /// on the shop screen.
    /// </summary>
    public partial class WindowShopStatus : WindowBase
    {
        #region Properties

        /// <summary>
        /// The item selected in the window listing the items that can be bought. 
        /// </summary>
        public Carriable Item
        {
            get { return localItem; }
            set
            {
                if (localItem != value)
                {
                    localItem = value;
                    Refresh();
                }
            }
        }
        Carriable localItem;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowShopStatus()
            : base(368, 128, GeexEdit.GameWindowWidth - 368, GeexEdit.GameWindowHeight - 128)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.Z = 200;
            //Item = null;
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
            if (Item == null)
            {
                return;
            }
            int number = 0;
            switch (Item.GetType().Name.ToString())
            {
                case "Item":
                    number = InGame.Party.ItemNumber(Item.Id);
                    break;
                case "Weapon":
                    number = InGame.Party.WeaponNumber(Item.Id);
                    break;
                case "Armor":
                    number = InGame.Party.ArmorNumber(Item.Id);
                    break;
            }
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(4, 0, 200, 32, "Number in possession");
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(204, 0, 32, 32, number.ToString(), 2);
            if (Item.GetType().Name.ToString() == "Item")
            {
                return;
            }
            // Equipment adding information
            for (int i = 0; i < InGame.Party.Actors.Count; i++)
            {
                // Get actor
                GameActor actor = InGame.Party.Actors[i];
                // If equippable, then set to normal text Color. If not, set to
                // invalid text Color.
                if (actor.IsEquippable(Item))
                {
                    this.Contents.Font.Color = NormalColor;
                }
                else
                {
                    this.Contents.Font.Color = DisabledColor;
                }
                // Draw actor's Name
                this.Contents.DrawText(4, 64 + 64 * i, 120, 32, actor.Name);
                //Flag : draw item or not
                bool flagDraw = false;
                // Get current equipment
                Carriable item1 = null;
                if (Item.GetType().Name.ToString() == "Weapon")
                {
                    item1 = WeaponChangeDrawer(actor,(Weapon)Item, i); 
                }
                else
                {
                    item1 = ArmorChangeDrawer(actor, (Armor)Item, i); 
                }
                // Is current equipment to be drawn ?
                if (item1 != null)
                {
                    flagDraw = true;
                }
                // Draw item
                if (flagDraw)
                {
                    int x = 4;
                    int y = 64 + 64 * i + 32;
                    byte opacity = this.Contents.Font.Color == NormalColor ? (byte)255 : (byte)128;
                    this.Contents.Blit(x, y + 4, Cache.IconBitmap, Cache.IconSourceRect(item1.IconName), opacity);
                    this.Contents.DrawText(x + 28, y, 212, 32, item1.Name);
                }
            }
        }

        /// <summary>
        /// Manage drawing relative to weapon changing
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="weapon">selected item in the window</param>
        /// <param Name="i">iterator</param>
        /// <returns>current weapon</returns>
        public Weapon WeaponChangeDrawer(GameActor actor, Weapon weapon, int i)
        {
            Weapon _current_weapon = null;
            _current_weapon = Data.Weapons[actor.WeaponId];
            // If equippable
            if (actor.IsEquippable(weapon))
            {
                int change = 0;
                int atk1 = _current_weapon != null ? _current_weapon.Atk : 0;
                int atk2 = weapon != null ? weapon.Atk : 0;
                // Create change string
                change = atk2 - atk1;
                string changeString = "";
                changeString = change > 0 ? "+" + change.ToString() : change.ToString();
                // Draw parameter change values
                this.Contents.DrawText(124, 64 + 64 * i, 112, 32, changeString, 2);
            }
            return _current_weapon;
            
        }

        /// <summary>
        /// Manage drawing relative to armor changing
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="armor">selected item in the window</param>
        /// <param Name="i">iterator</param>
        /// <returns>current armor</returns>
        public Armor ArmorChangeDrawer(GameActor actor, Armor armor, int i)
        {
            Armor _current_armor = null;
            if (armor.Kind == 0)
            {
                _current_armor = Data.Armors[actor.ArmorShield];
            }
            else if (armor.Kind == 1)
            {
                _current_armor = Data.Armors[actor.ArmorHelmet];
            }
            else if (armor.Kind == 2)
            {
                _current_armor = Data.Armors[actor.ArmorBody];
            }
            else
            {
                _current_armor = Data.Armors[actor.ArmorAccessory];
            }
            // If equippable
            if (actor.IsEquippable(armor))
            {
                int change = 0;
                int _pdef1 = _current_armor != null ? _current_armor.Pdef : 0;
                int _mdef1 = _current_armor != null ? _current_armor.Mdef : 0;
                int _pdef2 = armor != null ? armor.Pdef : 0;
                int _mdef2 = armor != null ? armor.Mdef : 0;
                // Create change string
                change = _pdef2 - _pdef1 + _mdef2 - _mdef1;
                string changeString = "";
                changeString = change > 0 ? "+" + change.ToString() : change.ToString();
                // Draw parameter change values
                this.Contents.DrawText(124, 64 + 64 * i, 112, 32, changeString, 2);
            }
            return _current_armor;
        }

        #endregion
    }
}
