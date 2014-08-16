using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This class is for all in-game windows.
    /// </summary>
    public partial class WindowBase : Geex.Run.Window
    {
        #region Variables

        /// <summary>
        /// Windowskin Name
        /// </summary>
        protected string windowskinName;

        #endregion

        #region Properties

        /// <summary>
        /// Get Normal Color
        /// </summary>
        public Color NormalColor
        {
            get { return new Color(255, 255, 255, 255); }
        }

        /// <summary>
        /// Get Disabled Color
        /// </summary>
        public Color DisabledColor
        {
            get { return new Color(255, 255, 255, 128); }
        }

        /// <summary>
        /// Get System Color
        /// </summary>
        public Color SystemColor
        {
            get { return new Color(192, 224, 255, 255); }
        }

        /// <summary>
        /// Get Crisis Color
        /// </summary>
        public Color CrisisColor
        {
            get { return new Color(255, 255, 64, 255); }
        }

        /// <summary>
        /// Get Knock-Out Color
        /// </summary>
        public Color KnockoutColor
        {
            get { return new Color(255, 64, 0, 255); }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="x">window x-coordinate</param>
        /// <param Name="y">window y-coordinate</param>
        /// <param Name="_width">window width</param>
        /// <param Name="_height">window height</param>
        public WindowBase(Viewport port, int _x, int _y, int _width, int _height)
            : base(port)
        {
            windowskinName = InGame.System.WindowskinName;
            this.Windowskin = Cache.Windowskin(windowskinName);
            this.X = _x;
            this.Y = _y;
            this.Width = _width;
            this.Height = _height;
            this.Z = 100;
        }
        public WindowBase(int _x, int _y, int _width, int _height)
            : this(Graphics.Foreground, _x, _y, _width, _height)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get Text Color
        /// </summary>
        /// <param Name="n">text Color number (0-7)</param>
        /// <returns>Text Color</returns>
        public Color TextColor(int n)
        {
            switch (n)
            {
                case 0:
                    return GameOptions.MessageTextColor;
                case 1:
                    return new Color(128, 128, 255, 255);
                case 2:
                    return new Color(255, 128, 128, 255);
                case 3:
                    return new Color(128, 255, 128, 255);
                case 4:
                    return new Color(128, 255, 255, 255);
                case 5:
                    return new Color(255, 128, 255, 255);
                case 6:
                    return new Color(255, 255, 128, 255);
                case 7:
                    return new Color(192, 192, 192, 255);
                default:
                    return NormalColor;
            }
        }

        #region Update

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            //Reset if Windowskin was changed
            if (InGame.System.WindowskinName != windowskinName)
            {
                windowskinName = InGame.System.WindowskinName;
                this.Windowskin = Cache.Windowskin(windowskinName);
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw Graphic
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawActorGraphic(GameActor actor, int x, int y)
        {
            Bitmap bitmap = Cache.Character(actor.CharacterName);
            int _cw = bitmap.Width / 4;
            int _ch = GameOptions.IsArpgCharacterOn && bitmap.Height==bitmap.Width ? bitmap.Height / 6 : bitmap.Height / 4;
            Rectangle _src_rect = new Rectangle(0, 0, _cw, _ch);
            this.Contents.Blit(x - _cw / 2, y - _ch, bitmap, _src_rect);
        }

        /// <summary>
        /// Draw localName
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawActorName(GameActor actor, int x, int y)
        {
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(x, y, 120, 32, actor.Name);
        }

        /// <summary>
        /// Draw Class
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void draw_actor_class(GameActor actor, int x, int y)
        {
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(x, y, 236, 32, actor.ClassName);
        }

        /// <summary>
        /// Draw Level
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawActorLevel(GameActor actor, int x, int y)
        {
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(x, y, 32, 32, "Lv");
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(x + 32, y, 24, 32, actor.Level.ToString(), 2);
        }

        /// <summary>
        /// Make State Text String for Drawing
        /// </summary>
        /// <param Name="Battler">actor</param>
        /// <param Name="width">draw spot width</param>
        /// <param Name="need_normal">Whether or not [normal] is needed (true / false)</param>
        /// <returns></returns>
        public string MakeBattlerStateText(GameBattler battler, int width, bool need_normal)
        {
            // Get width of brackets
            int _brackets_width = this.Contents.TextSize("[]").Width;
            // Make text string for state names
            string _text = "";
            foreach (int i in battler.states)
            {
                if (Data.States[i].Rating >= 1)
                {
                    if (_text == "")
                    {
                        _text = Data.States[i].Name;
                    }
                    else
                    {
                        string new_text = _text + "/" + Data.States[i].Name;
                        int text_width = this.Contents.TextSize(new_text).Width;
                        if (text_width > width - _brackets_width)
                        {
                            break;
                        }
                        _text = new_text;
                    }
                }
            }
            // If text string for state names is empty, make it [normal]
            if (_text == "")
            {
                if (need_normal)
                {
                    _text = "[Normal]";
                }
            }
            else
            {
                // Attach brackets
                _text = "[" + _text + "]";
            }
            // Return completed text string
            return _text;
        }

        /// <summary>
        /// Draw State
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        /// <param Name="width">draw spot wdth</param>
        public void DrawActorState(GameActor actor, int x, int y, int width)
        {
            string _text = MakeBattlerStateText(actor, width, true);
            this.Contents.Font.Color = actor.Hp == 0 ? KnockoutColor : NormalColor;
            this.Contents.DrawText(x, y, width, 32, _text);
        }

        /// <summary>
        /// Draw State (Default width = 120)
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawActorState(GameActor actor, int x, int y)
        {
            DrawActorState(actor, x, y, 120);
        }

        /// <summary>
        /// Draw Xp
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawActorExp(GameActor actor, int x, int y)
        {
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(x, y, 24, 32, "E");
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(x + 24, y, 84, 32, actor.ExpString, 2);
            this.Contents.DrawText(x + 108, y, 12, 32, "/", 1);
            this.Contents.DrawText(x + 120, y, 84, 32, actor.NextExpString);
        }

        /// <summary>
        /// Draw Actor HP
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        /// <param Name="width">draw spot width</param>
        public void DrawActorHp(GameActor actor, int x, int y, int width)
        {
            int _hp_x = 0;              //HP x-coordinates
            bool _flag = false;         //If true draw MaxHp
            // Draw "HP" text string
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(x, y, 32, 32, Data.System.Wordings.Hp);
            // Calculate if there is draw space for MaxHp
            if (width - 32 >= 108)
            {
                _hp_x = x + width - 108;
                _flag = true;
            }
            else if (width - 32 >= 48)
            {
                _hp_x = x + width - 48;
                _flag = false;
            }
            // Draw HP
            this.Contents.Font.Color = actor.Hp == 0 ? KnockoutColor :
              actor.Hp <= actor.MaxHp / 4 ? CrisisColor : NormalColor;
            this.Contents.DrawText(_hp_x, y, 48, 32, actor.Hp.ToString(), 2);
            // Draw MaxHp
            if (_flag)
            {
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(_hp_x + 48, y, 12, 32, "/", 1);
                this.Contents.DrawText(_hp_x + 60, y, 48, 32, actor.MaxHp.ToString());
            }
        }

        /// <summary>
        /// Draw Actor HP (Default width = 144)
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawActorHp(GameActor actor, int x, int y)
        {
            DrawActorHp(actor, x, y, 144);
        }

        /// <summary>
        /// Draw Actor SP
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        /// <param Name="width">draw spot width</param>
        public void DrawActorSp(GameActor actor, int x, int y, int width)
        {
            int _sp_x = 0;              //SP x-coordinates
            bool _flag = false;         //If true draw MaxSp
            // Draw "SP" text string
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(x, y, 32, 32, Data.System.Wordings.Sp);
            // Calculate if there is draw space for MaxHp
            if (width - 32 >= 108)
            {
                _sp_x = x + width - 108;
                _flag = true;
            }
            else if (width - 32 >= 48)
            {
                _sp_x = x + width - 48;
                _flag = false;
            }
            // Draw SP
            this.Contents.Font.Color = actor.Sp == 0 ? KnockoutColor :
              actor.Sp <= actor.MaxSp / 4 ? CrisisColor : NormalColor;
            this.Contents.DrawText(_sp_x, y, 48, 32, actor.Sp.ToString(), 2);
            // Draw MaxSp
            if (_flag)
            {
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(_sp_x + 48, y, 12, 32, "/", 1);
                this.Contents.DrawText(_sp_x + 60, y, 48, 32, actor.MaxSp.ToString());
            }
        }

        /// <summary>
        /// Draw Actor SP (Default width = 144)
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawActorSp(GameActor actor, int x, int y)
        {
            DrawActorSp(actor, x, y, 144);
        }

        /// <summary>
        /// Draw Parameter
        /// </summary>
        /// <param Name="actor">actor</param>
        /// <param Name="_x">draw spot x-coordinate</param>
        /// <param Name="_y">draw spot y-coordinate</param>
        /// <param Name="type">parameter type (0-6)</param>
        public void DrawActorParameter(GameActor actor, int _x, int _y, int type)
        {
            string parameter_name = "";         //Parameter Name string
            int parameter_value = 0;            //Parameter value
            switch (type)
            {
                case 0:
                    parameter_name = Data.System.Wordings.Atk;
                    parameter_value = actor.Atk;
                    break;
                case 1:
                    parameter_name = Data.System.Wordings.Pdef;
                    parameter_value = actor.Pdef;
                    break;
                case 2:
                    parameter_name = Data.System.Wordings.Mdef;
                    parameter_value = actor.Mdef;
                    break;
                case 3:
                    parameter_name = Data.System.Wordings.Str;
                    parameter_value = actor.Str;
                    break;
                case 4:
                    parameter_name = Data.System.Wordings.Dex;
                    parameter_value = actor.Dex;
                    break;
                case 5:
                    parameter_name = Data.System.Wordings.Agi;
                    parameter_value = actor.Agi;
                    break;
                case 6:
                    parameter_name = Data.System.Wordings.Intel;
                    parameter_value = actor.Intel;
                    break;
            }
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(_x, _y, 120, 32, parameter_name);
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(_x + 120, _y, 36, 32, parameter_value.ToString(), 1);
        }

        /// <summary>
        /// Draw Item localName
        /// </summary>
        /// <param Name="item">item</param>
        /// <param Name="x">draw spot x-coordinate</param>
        /// <param Name="y">draw spot y-coordinate</param>
        public void DrawItemName(Carriable item, int x, int y)
        {
            if (item == null)
            {
                return;
            }
            this.Contents.Blit(x, y + 4, Cache.IconBitmap, Cache.IconSourceRect(item.IconName));
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(x + 28, y, 212, 32, item.Name);
        }

        #endregion

        #endregion
    }
}
