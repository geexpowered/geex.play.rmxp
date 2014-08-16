using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window is used to select text characters on the input Name screen.
    /// </summary>
    public partial class WindowNameInput : WindowBase
    {
        #region Constants

        public readonly string[] CharacterTable = {
            "A","B","C","D","E",
            "F","G","H","I","J",
            "K","L","M","N","O",
            "P","Q","R","S","T",
            "U","V","W","X","Y",
            "Z"," "," "," "," ",
            "+","-","*","/","!",
            "1","2","3","4","5",
            "" ,"" ,"" ,"" ,"" ,
            "a","b","c","d","e",
            "f","g","h","i","j",
            "k","l","m","n","o",
            "p","q","r","s","t",
            "u","v","w","x","y",
            "z"," "," "," "," ",
            "#","$","%","&","",
            "6","7","8","9","0",
            "" ,"" ,"" ,"" ,""
        };

        #endregion

        #region Variables

        /// <summary>
        /// index
        /// </summary>
        int index;

        #endregion

        #region Properties

        /// <summary>
        /// Text Character Acquisition
        /// </summary>
        public string Character
        {
            get
            {
                return CharacterTable[index];
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowNameInput()
            : base(0, 128, 640, 352)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            index = 0;
            Refresh();
            UpdateCursorRect();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            for (int i = 0; i < 90; i++)
            {
                X = 140 + i / 5 / 9 * 180 + i % 5 * 32;
                Y = i / 5 % 9 * 32;
                this.Contents.DrawText(X, Y, 32, 32, CharacterTable[i], 1);
            }
            this.Contents.DrawText(428, 9 * 32, 48, 32, "OK", 1);
        }

        /// <summary>
        /// GeexMouse Rectangle Update
        /// </summary>
        public void UpdateCursorRect()
        {
            // If cursor is positioned on [OK]
            if (index >= 90)
            {
                this.CursorRect.Set(428, 9 * 32, 48, 32);
                // If cursor is positioned on anything other than [OK]
            }
            else
            {
                X = 140 + index / 5 / 9 * 180 + index % 5 * 32;
                Y = index / 5 % 9 * 32;
                this.CursorRect.Set(X, Y, 32, 32);
            }
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            base.Update();
            // If cursor is positioned on [OK]
            if (index >= 90)
            {
                // GeexMouse down
                if (Input.RMTrigger.Down)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    index -= 90;
                }
                // GeexMouse up
                if (Input.RMRepeat.Up)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    index -= 90 - 40;
                }
            }
            // If cursor is positioned on anything other than [OK]
            else
            {
                // If right directional button is pushed
                if (Input.RMRepeat.Right)
                {
                    // If directional button pressed down is not a IsRepeated, or
                    // cursor is not positioned on the right edge
                    if (Input.RMTrigger.Right || index / 45 < 3 || index % 5 < 4)
                    {
                        // Move cursor to right
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        if (index % 5 < 4)
                        {
                            index += 1;
                        }
                        else
                        {
                            index += 45 - 4;
                        }
                        if (index >= 90)
                        {
                            index -= 90;
                        }
                    }
                }
                // If left directional button is pushed
                if (Input.RMRepeat.Left)
                {
                    // If directional button pressed down is not a IsRepeated, or
                    // cursor is not positioned on the left edge
                    if (Input.RMTrigger.Left || index / 45 > 0 || index % 5 > 0)
                    {
                        // Move cursor to left
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        if (index % 5 > 0)
                        {
                            index -= 1;
                        }
                        else
                        {
                            index -= 45 - 4;
                        }
                        if (index < 0)
                        {
                            index += 90;
                        }
                    }
                }
                // If down directional button is pushed
                if (Input.RMRepeat.Down)
                {
                    // Move cursor down
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    if (index % 45 < 40)
                    {
                        index += 5;
                    }
                    else
                    {
                        index += 90 - 40;
                    }
                }
                // If up directional button is pushed
                if (Input.RMRepeat.Up)
                {
                    // If directional button pressed down is not a IsRepeated, or
                    // cursor is not positioned on the upper edge
                    if (Input.RMTrigger.Up || index % 45 >= 5)
                    {
                        // Move cursor up
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        if (index % 45 >= 5)
                        {
                            index -= 5;
                        }
                        else
                        {
                            index += 90;
                        }
                    }
                }
                // If L or R button was pressed
                if (Input.RMRepeat.L || Input.RMRepeat.R)
                {
                    // Move capital / small
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    if (index < 45)
                    {
                        index += 45;
                    }
                    else
                    {
                        index -= 45;
                    }
                }
            }
            UpdateCursorRect();
        }

        #endregion

    }
}
