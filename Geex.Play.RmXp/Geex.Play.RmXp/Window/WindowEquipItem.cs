using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Play.Custom;
using Microsoft.Xna.Framework;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays choices when opting to change equipment on the
    /// equipment screen.
    /// </summary>
    public partial class WindowEquipItem : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// Equipped actor
        /// </summary>
        GameActor actor;

        /// <summary>
        /// Equip region (0-3)
        /// </summary>
        int equipType;

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
        /// <param Name="actor">equipped actor</param>
        /// <param Name="equip_type">equip region (0-3)</param>
        public WindowEquipItem(GameActor actor, int equipType)
            : base(0, 256, GeexEdit.GameWindowWidth, GeexEdit.GameWindowHeight - 256)
        {
            Initialize(actor, equipType);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="actor">equipped actor</param>
        /// <param name="equipType">equip region (0-3)</param>
        protected void Initialize(GameActor actor, int equipType)
        {
            base.Initialize();
            // WindowEquipItem initialization
            this.actor = actor;
            this.equipType = equipType;
            columnMax = 2;
            Refresh();
            this.IsActive = false;
            this.Index = -1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh, draw window content
        /// </summary>
        public void Refresh()
        {
            if (this.Contents != null)
            {
                this.Contents.Dispose();
                this.Contents = null;
            }
            data.Clear();
            // Add equippable weapons
            if (equipType == 0)
            {
                List<short> weapon_set = Data.Classes[actor.ClassId].WeaponSet;
                for (short i = 1; i <= Data.Weapons.Length; i++)
                {
                    if (InGame.Party.WeaponNumber(i) > 0 && weapon_set.Contains(i))
                    {
                        data.Add(Data.Weapons[i]);
                    }
                }
            }
            // Add equippable armor
            if (equipType != 0)
            {
                List<short> armor_set = Data.Classes[actor.ClassId].ArmorSet;
                for (short i = 1; i <= Data.Armors.Length; i++)
                {
                    if (InGame.Party.ArmorNumber(i) > 0 && armor_set.Contains(i))
                    {
                        //Armor kind = equip type - 1
                        if (Data.Armors[i].Kind == equipType - 1)
                        {
                            data.Add(Data.Armors[i]);
                        }
                    }
                }
            }
            // Add blank page
            data.Add(null);
            // Make a bitmap and draw all items
            itemMax = data.Count;
            this.Contents = new Bitmap(Width - 32, RowMax * 32);
            for (int i = 0; i <= itemMax - 1; i++)
            {
                DrawItem(i);
            }
        }

        /// <summary>
        /// Draw Item
        /// </summary>
        /// <param Name="index">item index</param>
        public void DrawItem(int index)
        {
            int number = 0;
            Carriable item = data[index];
            if (item == null) return;
            int posx = 4 + index % 2 * (288 * GeexEdit.GameWindowWidth / 640 + 32);
            int posy = index / 2 * 32;
            switch (item.GetType().Name.ToString())
            {
                case ("Weapon"):
                    number = InGame.Party.WeaponNumber(item.Id);
                    break;
                case ("Armor"):
                    number = InGame.Party.ArmorNumber(item.Id);
                    break;
            }
            this.Contents.Blit(posx + index % 2 * 8, posy + 4, Cache.IconBitmap, Cache.IconSourceRect(item.IconName));
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(posx + 28 * GeexEdit.GameWindowWidth / 640, posy, 212, 32, item.Name, 0);
            this.Contents.DrawText(posx + 240 * GeexEdit.GameWindowWidth / 640, posy, 16, 32, ":", 1);
            this.Contents.DrawText(posx + 256 * GeexEdit.GameWindowWidth / 640, posy, 24, 32, number.ToString(), 2);
        }

        /// <summary>
        /// Help Text Update
        /// </summary>
        public override void UpdateHelp()
        {
            HelpWindow.SetText(this.Item == null ? "" : this.Item.Description);
        }

        /// <summary>
        /// Disable Update
        /// </summary>
        /// <returns>True is Update is disabled (which is always the case)</returns>
        public bool IsDisableUpdate()
        {
            return true;
        }

        #endregion
    }
}
