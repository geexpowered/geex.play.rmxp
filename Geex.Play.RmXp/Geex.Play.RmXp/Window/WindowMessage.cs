using System;
using System.Text.RegularExpressions;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays buyable goods on the shop screen.
    /// </summary>
    public partial class WindowMessage : WindowSelectable
    {
        #region Variables
        /// <summary>
        /// Start coordinate for text
        /// </summary>
        const int X_START = 0;

        /// <summary>
        /// True is the Mesage Window is open
        /// </summary>
        bool isOpen;

        /// <summary>
        /// Current Message Window Font Name
        /// </summary>
        public string WindowFontName = string.Empty;

        /// <summary>
        /// Current Message Window Font Size
        /// </summary>
        public short WindowFontSize = GameOptions.MessageFontSize;

        /// <summary>
        /// Current Message Window back opacity
        /// </summary>
        public byte WindowBackOpacity;

        /// <summary>
        /// Current Message Window opacity
        /// </summary>
        public byte WindowOpacity;

        /// <summary>
        /// Mesage Display Mode 0:all, 1: letter by letter, 2:Word by Word
        /// </summary>
        public short Mode = 0;


        /// <summary>
        /// x coordinate of next text to be displayed
        /// </summary>
        int textX;
        /// <summary>
        /// y coordinate of next text to be displayed
        /// </summary>
        int textY;

        /// <summary>
        /// True is Event updates must be locked during Window Message display
        /// </summary>
        public bool IsEventLocked = false;

        /// <summary>
        /// True in Keys are locked during Window Message Display
        /// </summary>
        public bool IsKeyLocked = false;

        /// <summary>
        /// Fading in
        /// </summary>
        bool isFadeIn;

        /// <summary>
        /// Fading out
        /// </summary>
        bool isFadeOut;

        /// <summary>
        /// Window's contents are showing
        /// </summary>
        bool contentsShowing = false;

        /// <summary>
        /// GeexMouse width
        /// </summary>
        int cursorWidth;

        /// <summary>
        /// Input number window
        /// </summary>
        WindowInputNumber inputNumberWindow = null;

        /// <summary>
        /// Gold window
        /// </summary>
        WindowGold goldWindow = null;

        /// <summary>
        /// Message Text to display
        /// </summary>
        string text;

        /// <summary>
        /// Width of char to be displayed
        /// </summary>
        int charWidth;
        /// <summary>
        /// Height of char to be displayed
        /// </summary>
        int charHeight;

        /// <summary>
        /// True is it's the end of message
        /// </summary>
        bool isMessageEnd;

        /// <summary>
        /// Amount of opacity to be added during fade
        /// </summary>
        int fadingSteps;

        /// <summary>
        /// Wait before going on
        /// </summary>
        int waitCount;

        /// <summary>
        /// Faceset
        /// </summary>
        Sprite faceset = null;

        #region Window colors
        /// <summary>
        /// Current Message Window Color table
        /// </summary>
        public Color[] ColorTable = new Color[8];
        /// <summary>
        /// Current Text Color
        /// </summary>
        int color;

        /// <summary>
        /// Store Color before change
        /// </summary>
        int rememberColor;

        /// <summary>
        /// True if next word only should be colorized
        /// </summary>
        bool isOneWordColor;
        #endregion

        #region title window
        /// <summary>
        /// Title Window
        /// </summary>
        Geex.Run.Window titleWindow;
        /// <summary>
        /// Title Window Width
        /// </summary>
        int titleWindowWidth;
        /// <summary>
        /// Title Window Height
        /// </summary>
        int titleWindowHeight;
        /// <summary>
        /// Title Window text
        /// </summary>
        public string TitleText = string.Empty;
        /// <summary>
        /// Title window text Color
        /// </summary>
        int titleWindowColor;
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Get window message Font Name
        /// </summary>
        string Font
        {
            get
            {
                return WindowFontName == string.Empty ? GeexEdit.DefaultFont : WindowFontName;
            }
        }

        /// <summary>
        /// get window message Font Size
        /// </summary>
        short FontSize
        {
            get
            {
                return WindowFontSize == 0 ? GeexEdit.DefaultFontSize : WindowFontSize;
            }
        }

        #endregion

        #region Initialize
        /// <summary>
        /// Constructor
        /// </summary>
        public WindowMessage()
            : base(GameOptions.MessageWindowRect.X, GameOptions.MessageWindowRect.Y, GameOptions.MessageWindowRect.Width, GameOptions.MessageWindowRect.Height)
        {
            InitVariables();
            InitMessageWindows();
            base.Initialize();
        }

        /// <summary>
        /// Initialize Window message variables
        /// </summary>
        void InitVariables()
        {
            X = GameOptions.MessageWindowRect.X;
            Y = GameOptions.MessageWindowRect.Y;
            text = null;
            isMessageEnd = false;
            isOpen = false;
            WindowBackOpacity = 200;
            WindowOpacity = 255;
            Mode = 0;
            waitCount = 0;
            fadingSteps = 10;
            isFadeIn = true;
            isFadeOut = false;
            IsEventLocked = false;
            IsKeyLocked = false;
            // blanc, bleu fonce,rouge,vert,bleu clair,violet,jaune,gris
            ColorTable[0] = GameOptions.MessageTextColor;
            ColorTable[1] = new Color(64, 64, 255, 255);
            ColorTable[2] = new Color(255, 64, 64, 255);
            ColorTable[3] = new Color(64, 255, 64, 255);
            ColorTable[4] = new Color(64, 128, 255, 255);
            ColorTable[5] = new Color(255, 64, 255, 255);
            ColorTable[6] = new Color(255, 255, 64, 255);
            ColorTable[7] = new Color(192, 192, 192, 255);
            color = 0;
            rememberColor = 0;
            isOneWordColor = false;
            titleWindowWidth = FontSize * 10 + 32;
            titleWindowHeight = FontSize + 32;
            TitleText = string.Empty;
            titleWindowColor = 0;
        }

        /// <summary>
        /// Initialize Windows
        /// </summary>
        void InitMessageWindows()
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.Contents.Font.Size = GameOptions.MessageFontSize;
            this.IsVisible = false;
            this.Z = 9995;
            contentsShowing = false;
            cursorWidth = 0;
            this.IsActive = false;
            this.Index = -1;
            titleWindow = new Geex.Run.Window();
            titleWindow.Contents = new Bitmap(32, 32);
        }
        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public new void Dispose()
        {
            TerminateMessage();
            InGame.Temp.IsMessageWindowShowing = false;
            if (inputNumberWindow != null)
            {
                inputNumberWindow.Dispose();
            }
            if (faceset != null)
                faceset.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Terminate Message
        /// </summary>
        void TerminateMessage()
        {
            isMessageEnd = false;
            isOpen = false;
            InGame.Temp.IsMessageWindowShowing = false;
            this.IsActive = false;
            this.IsPausing = false;
            this.Index = -1;
            this.IsVisible = false;
            this.Contents.Clear();
            titleWindow.Contents.Clear();
            titleWindow.IsVisible = false;
			TitleText = String.Empty;
            // Clear showing flag
            contentsShowing = false;
            // Call message callback
            if (InGame.Temp.MessageProc != null)
            {
                InGame.Temp.MessageProc();
            }
            // Clear variables related to text, choices, and number input
            InGame.Temp.MessageText = null;
            InGame.Temp.MessageProc = null;
            //InGame.Temp.MessageProc = null;
            InGame.Temp.ChoiceStart = 99;
            InGame.Temp.ChoiceMax = 0;
            InGame.Temp.ChoiceCancelType = 0;
            InGame.Temp.ChoiceProc = null;
            InGame.Temp.NumInputStart = 99;
            InGame.Temp.NumInputVariableId = 0;
            InGame.Temp.NumInputDigitsMax = 0;
            // Open gold window
            if (goldWindow != null)
            {
                goldWindow.Dispose();
                goldWindow = null;
            }
            if(faceset != null)
                faceset.Dispose();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Refresh
        /// </summary>
        void Refresh()
        {
            ResetWindow();
            this.Contents.Clear();
            textX = X_START;
            textY = (32 - charHeight) / 2 + 8;
            cursorWidth = GameOptions.MessageWindowRect.Width - 32;
            // Indent if choice
            if (InGame.Temp.ChoiceStart == 0)
            {
                textX = X_START + 8;
            }
            SetUp(InGame.Temp.MessageText);
            RefreshTitleWindow();
            // If choice
            if (InGame.Temp.ChoiceMax > 0)
            {
                itemMax = InGame.Temp.ChoiceMax;
                this.IsActive = true;
                this.Index = 0;
            }
            // If number input
            if (InGame.Temp.NumInputVariableId > 0 && inputNumberWindow == null)
            {
                int _digits_max = InGame.Temp.NumInputDigitsMax;
                int _number = (int)InGame.Variables.Arr[InGame.Temp.NumInputVariableId];
                inputNumberWindow = new WindowInputNumber(_digits_max);
                inputNumberWindow.Number = _number;
                inputNumberWindow.X = this.X + 8;
                inputNumberWindow.Y = this.Y + InGame.Temp.NumInputStart * 32;
            }

        }
        /// <summary>
        /// Refresh the Title Window
        /// </summary>
        void RefreshTitleWindow()
        {
            titleWindow.Z = this.Z;
            titleWindow.Windowskin = Cache.Windowskin(windowskinName);
            titleWindow.Opacity = WindowOpacity;
            titleWindow.BackOpacity = WindowBackOpacity;
            //@title_window.contents_opacity=@fade_in ? 0 : @window_opacity
            titleWindow.Ox = 0;
            titleWindow.Oy = 0;
            titleWindow.IsPausing = false;
            titleWindow.IsActive = false;
            titleWindow.CursorRect.Empty();
            if (TitleText != string.Empty)
            {
                // Se Title position and Size
                Rectangle titleSize = this.Contents.TextSize(TitleText);
                titleWindow.X = this.X;
                titleWindow.Y = this.Y - titleSize.Height - 32;
                titleWindow.Width = titleSize.Width + 32;
                titleWindow.Height = titleSize.Height + 32;
                titleWindow.Contents.Dispose();
                titleWindow.Contents = new Bitmap(titleWindow.Width - 32, titleWindow.Height - 32);
                titleWindow.Contents.Font.Name = Font;
                titleWindow.Contents.Font.Size = FontSize;
                titleWindow.Contents.Font.Color = ColorTable[titleWindowColor];
                titleWindow.Contents.DrawText(0, 0, titleSize.Width, titleSize.Height, TitleText);
            }
            titleWindow.IsVisible = (TitleText != string.Empty);
        }

        /// <summary>
        /// Set message text up
        /// </summary>
        /// <param Name="messageText">text to be displayed</param>
        void SetUp(string messageText)
        {
            waitCount = 0;
            isOpen = true;
            isMessageEnd = false;
            this.IsVisible = false;
            this.Z = 9995;
            text = messageText;
            this.IsVisible = true;
            InGame.Temp.IsMessageWindowShowing = true;
        }

        /// <summary>
        /// Set Window Position and Opacity Level
        /// </summary>
        void ResetWindow()
        {
            this.Contents.Font.Name = Font;
            this.Contents.Font.Size = FontSize;
            Rectangle charSize = this.Contents.TextSize("Q");
            charWidth = charSize.Width;
            charHeight = charSize.Height;
            if (InGame.Temp.IsInBattle)
            {
                this.X = (GeexEdit.GameWindowWidth - GameOptions.MessageWindowRect.Width) / 2;
                switch (InGame.System.MessagePosition)
                {
                    case 0:  // up
                        this.Y = 32;
                        break;
                    case 1: // middle
                        this.Y = GeexEdit.GameWindowCenterY - GameOptions.MessageWindowRect.Height / 2;
                        break;
                    case 2: // down
                        this.Y = GeexEdit.GameWindowHeight - GameOptions.MessageWindowRect.Height - 32;
                        break;
                }
            }
            else
            {
                switch (InGame.System.MessagePosition)
                {
                    case 0:  // up
                        this.Y = 32;
                        break;
                    case 1: // middle
                        this.Y = GeexEdit.GameWindowCenterY - GameOptions.MessageWindowRect.Height / 2;
                        break;
                    case 2: // down
                        this.Y = GeexEdit.GameWindowHeight - GameOptions.MessageWindowRect.Height - 32;
                        break;
                }
            }
            if (InGame.System.MessageFrame == 0)
            {
                this.Opacity = WindowOpacity;
                titleWindow.Opacity = WindowOpacity;
            }
            else
            {
                this.Opacity = 0;
                titleWindow.Opacity = 0;
            }
            this.BackOpacity = WindowBackOpacity;
            titleWindow.BackOpacity = WindowBackOpacity;
            if (isFadeIn)
            {
                this.ContentsOpacity = 0;
                titleWindow.ContentsOpacity = 0;
            }
            else
            {
                this.ContentsOpacity = WindowOpacity;
                titleWindow.ContentsOpacity = WindowOpacity;
            }
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            base.Update();
            if (InGame.Temp.ChoiceMax == 0) this.IsPausing = true;
            if (IsKeyLocked && isOpen && (text == null || text == string.Empty) && waitCount == 0) TerminateMessage();
            #region Hit C to accelerate message display
            if (Input.RMTrigger.C && isOpen && !IsKeyLocked)
            {
                if ((isMessageEnd || text == null || text == string.Empty) && InGame.Temp.NumInputVariableId == 0)
                {
                    if (InGame.Temp.ChoiceMax > 0)
                    {
                        InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                        InGame.Temp.ChoiceProc(this.Index);
                    }
                    TerminateMessage();
                }
                // Display all remaining chars
                while (text != string.Empty)
                {
                    AddOneLetter();
                }
                isMessageEnd = true;
                return;
            }
            #endregion
            #region Hit B to cancel
            if (Input.RMTrigger.B && isOpen && !IsKeyLocked)
            {
                if (InGame.Temp.ChoiceMax > 0 && InGame.Temp.ChoiceCancelType >= 0)
                {
                    InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                    InGame.Temp.ChoiceProc(InGame.Temp.ChoiceCancelType - 1);
                    TerminateMessage();
                }
            }
            #endregion
            // Next char display
            if (isOpen) AddNextLetter();
            #region If number input
            if (InGame.Temp.NumInputVariableId > 0  && inputNumberWindow == null)
            {
                int digitMax = InGame.Temp.NumInputDigitsMax;
                int number = InGame.Variables.Arr[InGame.Temp.NumInputVariableId];
                inputNumberWindow = new WindowInputNumber(digitMax);
                inputNumberWindow.Number = number;
                inputNumberWindow.X = this.X + 8;
                inputNumberWindow.Y = this.Y + InGame.Temp.NumInputStart * charHeight;
            }
            #endregion
            #region Fade In
            if (isFadeIn && this.ContentsOpacity < WindowOpacity)
            {
                this.ContentsOpacity = (byte)Math.Min(this.ContentsOpacity + fadingSteps, WindowOpacity);
                titleWindow.ContentsOpacity = this.ContentsOpacity;
                if (inputNumberWindow != null) inputNumberWindow.ContentsOpacity = (byte)Math.Min(this.ContentsOpacity + fadingSteps, WindowOpacity);
                if (this.ContentsOpacity == WindowOpacity) isFadeIn = false;
                return;
            }
            #endregion
            #region Inputting number
            if (inputNumberWindow != null)
            {
                inputNumberWindow.Update();
                // Confirm
                if (Input.RMTrigger.C && !IsKeyLocked)
                {
                    InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                    InGame.Variables.Arr[InGame.Temp.NumInputVariableId] = inputNumberWindow.Number;
                    InGame.Map.IsNeedRefresh = true;
                    // Dispose of number input window
                    inputNumberWindow.Dispose();
                    inputNumberWindow = null;
                    TerminateMessage();
                }
                return;
            }
            #endregion
            #region If display wait message or choice exists when not fading out
            if (!isFadeOut && InGame.Temp.MessageText != null && !isOpen)
            {
                contentsShowing = true;
                isFadeIn = true;
                //InGame.Temp.message_window_showing = true;
                if (!isOpen)
                {
                    Refresh();
                    // If choice
                    if (InGame.Temp.ChoiceMax > 0)
                    {
                        itemMax = InGame.Temp.ChoiceMax;
                        this.IsActive = true;
                        this.Index = 0;
                    }
                    // If number input
                    if (InGame.Temp.NumInputVariableId > 0 && inputNumberWindow == null)
                    {
                        int digitsMax = InGame.Temp.NumInputDigitsMax;
                        int number = InGame.Variables.Arr[InGame.Temp.NumInputVariableId];
                        inputNumberWindow = new WindowInputNumber(digitsMax);
                        inputNumberWindow.Number = number;
                        inputNumberWindow.X = this.X + 8;
                        inputNumberWindow.Y = this.Y + InGame.Temp.NumInputStart * 32;
                    }
                }
                this.IsVisible = true;
                this.ContentsOpacity = 0;
                if (inputNumberWindow != null) inputNumberWindow.ContentsOpacity = 0;
                return;
            }
            #endregion
        }

        /// <summary>
        /// Message Rectangle Update
        /// </summary>
        public override void UpdateCursorRect()
        {
            if (Index >= 0)
            {
                int _n = InGame.Temp.ChoiceStart + Index;
                this.CursorRect.Set(-4, _n * 26 + 9, cursorWidth, 28);
            }
            else
            {
                this.CursorRect.Empty();
            }
        }

        /// <summary>
        /// Make gold window
        /// </summary>
        void MakeGoldWindow()
        {
            if (goldWindow == null)
            {
                goldWindow = new WindowGold();
                goldWindow.X = GeexEdit.GameWindowWidth - goldWindow.Width;
                if (InGame.Temp.IsInBattle)
                {
                    goldWindow.Y = GameOptions.MessageGoldInBattleY;
                }
                else
                {
                    goldWindow.Y = this.Y + this.Height + GameOptions.MessageGold.Height > GeexEdit.GameWindowHeight ? 32 : GeexEdit.GameWindowHeight - GameOptions.MessageGold.Height;
                }
                goldWindow.Opacity = this.Opacity;
                goldWindow.BackOpacity = this.BackOpacity;
            }
        }

        /// <summary>
        /// Text display mode and print next text
        /// </summary>
        void AddNextLetter()
        {
            if (text == null || text == string.Empty) return;
            // wait_count
            if (waitCount > 0)
            {
                waitCount -= 1;
                return;
            }
            // test different Mode
            switch (Mode)
            {
                case 0:
                    // Letter by letter
                    AddOneLetter();
                    break;
                case 1:
                    // Word by word
                    string letter;
                    do
                    {
                        letter = AddOneLetter();
                    } while (!(letter == " " || letter == string.Empty));
                    break;
                case 2:
                    // Display all
                    do
                    {
                        letter = AddOneLetter();
                    } while (!(letter == string.Empty || letter == null));
                    break;
            }
        }

        /// <summary>
        /// Add next letter to message
        /// </summary>
        string AddOneLetter()
        {
            // If waiting for a message to be displayed
            if (text == null) return string.Empty;
            #region Analyse next Text
            #region Check all possible Text Code
            if (text[0] == '\\')
            {
                text = text.Remove(0, 1);
                switch (text[0])
                {
                    #region \v Insert Variables
                    case 'v':
                    case 'V':
                        int n = GetNumber(ref text);
                        text = Convert.ToString(InGame.Variables.Arr[n]) + text;//text.Insert(0, Convert.ToString(n));
                        return string.Empty;
                    #endregion
                    #region \c Change text Color
                    case 'c':
                    case 'C':
                        int color_index = GetNumber(ref text);
                        if (color_index >= 0 && color_index <= 7)
                        {
                            color = color_index;
                        }
                        return string.Empty;
                    #endregion
                    #region \g Make Gold Window
                    case 'g':
                    case 'G':
                        MakeGoldWindow();
                        return string.Empty;
                    #endregion
                    #region \i Display Item Icon
                    case 'i':
                    case 'I':
                        int itemId = GetNumber(ref text);
                        DrawIcon(Cache.IconSourceRect(Data.Items[itemId].IconName), textX, textY);
                        return string.Empty;
                    #endregion
                    #region \a Display Armor Icon
                    case 'a':
                    case 'A':
                        int armorId = GetNumber(ref text);
                        DrawIcon(Cache.IconSourceRect(Data.Armors[armorId].IconName), textX, textY);
                        return string.Empty;
                    #endregion
                    #region \w Display Weapon Icon
                    case 'w':
                    case 'W':
                        int weaponId = GetNumber(ref text);
                        DrawIcon(Cache.IconSourceRect(Data.Weapons[weaponId].IconName), textX, textY);
                        return string.Empty;
                    #endregion
                    #region \t Display Title Window
                    case 't':
                    case 'T':
                        TitleText = GetString(ref text);
                        RefreshTitleWindow();
                        return string.Empty;
                    #endregion
                    #region \o Change next word Color
                    case 'o':
                    case 'O':
                        rememberColor = color;
                        color = GetNumber(ref text);
                        isOneWordColor = true;
                        return string.Empty;
                    #endregion
                    #region \m Change Display Mode
                    case 'm':
                    case 'M':
                        Mode = (short)GetNumber(ref text);
                        return string.Empty;
                    #endregion
                    #region \n Display Hero's Name
                    case 'n':
                    case 'N':
                        int name = GetNumber(ref text);
                        text = Data.Actors[name - 1].Name + text;
                        return string.Empty;
                    #endregion
                    #region \p frame to pause message
                    case 'p':
                    case 'P':
                        waitCount = (int)(GetNumber(ref text) * GameOptions.AdjustFrameRate);
                        return string.Empty;
                    #endregion
                    #region \f Display faceset
                    case 'f':
                    case 'F':
                        string filename = GetString(ref text);
                        faceset = new Sprite(Graphics.Foreground);
                        faceset.Bitmap = Cache.Picture(filename);
                        faceset.X = this.X - faceset.Bitmap.Width + 30;
                        faceset.Y = this.Y - 37;
                        faceset.Z = this.Z + 1;
                        faceset.IsVisible = true;
                        return string.Empty;
                    #endregion
                    #region \s Insert Gold Sum
                    case 's':
                    case 'S':
                        text = Convert.ToString(InGame.Party.Gold) + text;
                        return string.Empty;
                    #endregion
                    #region \e Event Charset
                    case 'e':
                    case 'E':
                        int id = GetNumber(ref text);
                        int positionX = 0;
                        int positionY = 0;
                        Bitmap charset;
                        if (id == 0)
                        {
                            charset = Cache.Character(InGame.Player.CharacterName, InGame.Player.CharacterHue);
                            if (InGame.Player.IsDirectionFix)
                            {
                                positionX = InGame.Player.Pattern * charset.Width / 4;
                                positionY = (InGame.Player.Dir / 2 - 1) * charset.Height / 4;
                            }
                        }
                        else
                        {
                            GameEvent ev = InGame.Map.Events[(id == 1 ? InGame.Temp.MessageWindowEventID : id)];
                            charset = Cache.Character(ev.CharacterName, ev.CharacterHue);
                            if (ev.IsDirectionFix)
                            {
                                positionX = ev.Pattern * charset.Width / 4;
                                positionY = (ev.Dir / 2 - 1) * charset.Height / 4;
                            }
                        }
                        if (GameOptions.IsArpgCharacterOn && charset.Width == charset.Height)
                        {
                            this.Contents.Blit(0, 0, charset, new Rectangle(positionX + charset.Width / 12, positionY, charset.Width / 12, charset.Height / 6));
                        }
                        else
                        {
                            this.Contents.Blit(0, 0, charset, new Rectangle(positionX, positionY, charset.Width / 4, charset.Height / 4));
                        }
                        return string.Empty;
                    #endregion
                }
            }
            #endregion
            // If Line feed or carriage return
            if (text[0] == '\n' || text[0] == '\r')
            {
                text = text.Remove(0, 1);
                // Indent if choice
                if (textY >= InGame.Temp.ChoiceStart) textX = 8;
                // Add 1 line
                textY += charHeight;
                textX = X_START;
                // Indent if choice
                if (textY >= InGame.Temp.ChoiceStart * charHeight) textX = X_START;
                return string.Empty;
            }
            // Draw text
            string letter = text.Substring(0, 1);
            if (letter == null) return string.Empty;
            if (letter == " " && isOneWordColor)
            {
                color = rememberColor;
                isOneWordColor = false;
            }
            // Print shadowed letter
            this.Contents.Font.Color = ColorTable[color];
            this.Contents.DrawText(textX, textY, charWidth, charHeight, letter, true);
            textX += this.Contents.TextSize(letter).Width;
            text = text.Remove(0, 1);
            return letter;
            #endregion
        }

        /// <summary>
        /// Get the number from [[0-9]+]
        /// </summary>
        /// <param Name="text">original text</param>
        int GetNumber(ref string text)
        {
            text = text.Remove(0, 1);
            Regex regex = new Regex(@"\[[0-9]+\]");
            MatchCollection matches = regex.Matches(text);
            if (matches.Count == 0) return 0;
            text = text.Remove(0, matches[0].Length);
            string s = matches[0].Value.Substring(1, matches[0].Length - 2);
            return Convert.ToInt32(s);
        }

        /// <summary>
        /// Get the number from [[a-Z]+]
        /// </summary>
        /// <param Name="text">original text</param>
        string GetString(ref string text)
        {
            text = text.Remove(0, 1);
            Regex regex = new Regex(@"\[.+\]");
            MatchCollection matches = (regex.Matches(text));
            if (matches.Count==0) return string.Empty;
            string param = text.Substring(1, matches[0].Length - 2);
            text = text.Remove(0, matches[0].Length);
            return param;
        }

        /// <summary>
        /// Display Bitmap at x,y within Message Window
        /// </summary>
        /// <param Name="bitmap">Bitmap Icon</param>
        /// <param Name="x">x coordinate within Message Window</param>
        /// <param Name="y">y coordinate within Message Window</param>
        void DrawIcon(Rectangle rect, int x, int y)
        {
            this.Contents.Blit(x, y, Cache.IconBitmap, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
            textX += rect.Width;
        }

        #endregion
    }
}
