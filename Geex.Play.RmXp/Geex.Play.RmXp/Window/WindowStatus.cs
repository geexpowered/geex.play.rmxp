using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays full status specs on the status screen.
    /// </summary>
    public partial class WindowStatus : WindowBase
    {
        #region Variables

        /// <summary>
        /// Actor whom status is displayed
        /// </summary>
        GameActor actor;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="actor">Displayed actor</param>
        public WindowStatus(GameActor actor)
            : base(0, 0, GeexEdit.GameWindowWidth, GeexEdit.GameWindowHeight)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.actor = actor;
            Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh window content
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            DrawActorGraphic(actor, 40, 112);
            DrawActorName(actor, 4, 0);
            draw_actor_class(actor, 4 + 144, 0);
            DrawActorLevel(actor, 96, 32);
            DrawActorState(actor, 96, 64);
            DrawActorHp(actor, 96, 112, 172);
            DrawActorSp(actor, 96, 144, 172);
            DrawActorParameter(actor, 96, 192, 0);
            DrawActorParameter(actor, 96, 224, 1);
            DrawActorParameter(actor, 96, 256, 2);
            DrawActorParameter(actor, 96, 304, 3);
            DrawActorParameter(actor, 96, 336, 4);
            DrawActorParameter(actor, 96, 368, 5);
            DrawActorParameter(actor, 96, 400, 6);
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(320, 48, 80, 32, "EXP");
            this.Contents.DrawText(320, 80, 80, 32, "NEXT");
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(320 + 80, 48, 84, 32, actor.ExpString, 2);
            this.Contents.DrawText(320 + 80, 80, 84, 32, actor.NextRestExpString, 2);
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(320, 160, 96, 32, "Equipment");
            DrawItemName(Data.Weapons[actor.WeaponId], 320 + 16, 208);
            DrawItemName(Data.Armors[actor.ArmorShield], 320 + 16, 256);
            DrawItemName(Data.Armors[actor.ArmorHelmet], 320 + 16, 304);
            DrawItemName(Data.Armors[actor.ArmorBody], 320 + 16, 352);
            DrawItemName(Data.Armors[actor.ArmorAccessory], 320 + 16, 400);
        }

        /// <summary>
        /// Dummy
        /// </summary>
        public void Dummy()
        {
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(320, 112, 96, 32, Data.System.Wordings.Weapon);
            this.Contents.DrawText(320, 176, 96, 32, Data.System.Wordings.Armor1);
            this.Contents.DrawText(320, 240, 96, 32, Data.System.Wordings.Armor2);
            this.Contents.DrawText(320, 304, 96, 32, Data.System.Wordings.Armor3);
            this.Contents.DrawText(320, 368, 96, 32, Data.System.Wordings.Armor4);
            DrawItemName(Data.Weapons[actor.WeaponId], 320 + 24, 144);
            DrawItemName(Data.Armors[actor.ArmorShield], 320 + 24, 208);
            DrawItemName(Data.Armors[actor.ArmorHelmet], 320 + 24, 272);
            DrawItemName(Data.Armors[actor.ArmorBody], 320 + 24, 336);
            DrawItemName(Data.Armors[actor.ArmorAccessory], 320 + 24, 400);
        }

        #endregion
    }
}
