using System;
using System.IO;
using System.Text;
using Geex.Run;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays save files on the save and load screens.
    /// </summary>
    public partial class WindowSaveFile : WindowBase
    {
        #region Variables

        /// <summary>
        /// Savefile index
        /// </summary>
        int fileIndex;

        /// <summary>
        /// Savefile Name
        /// </summary>
        string filename;

        /// <summary>
        /// Savefile Name width
        /// </summary>
        int nameWidth;

        /// <summary>
        /// Saved Character names
        /// </summary>
        string[] characterNames;

        /// <summary>
        /// Saved Character hues
        /// </summary>
        protected int[] characterHues;

        /// <summary>
        /// Save time-stamp
        /// </summary>
        DateTime timeStamp;

        /// <summary>
        /// Frame count
        /// </summary>
        int frameCount;

        /// <summary>
        /// Game duration in seconds
        /// </summary>
        long totalSec;

        /// <summary>
        /// File existence flag
        /// </summary>
        bool fileExist;

        /// <summary>
        /// Save container
        /// </summary>
        private StorageContainer container;
        #endregion

        #region Properties

        public bool IsSelected
        {
            get { return localSelected; }
            set
            {
                localSelected = value;
                UpdateCursorRect();
            }
        }
        bool localSelected;
        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="file_index">save file index (0-3)</param>
        /// <param Name="filename">file Name</param>
        public WindowSaveFile(int file_index, string filename, StorageContainer container)
            : base(0, 64 + file_index % 4 * 104, 640, 104)
        {
            this.container = container;
            this.Contents = new Bitmap(Width - 32, Height - 32);
            InitFilename(file_index, filename);
            InitFiledata(file_index);
            if (fileExist)
            {
                InitGamedata();
            }
            Refresh();
        }

        /// <summary>
        /// Object Initialization : Filename
        /// </summary>
        /// <param Name="file_index">save file index (0-3)</param>
        /// <param Name="filename">file Name</param>
        public void InitFilename(int fileIndex, string filename)
        {
            this.fileIndex = fileIndex;
            this.filename = filename;
        }

        /// <summary>
        /// Object Initialization : File Data
        /// </summary>
        /// <param Name="file_index">save file index (0-3)</param>
        public void InitFiledata(int fileIndex)
        {
            this.fileIndex = fileIndex;
            DateTime time_stamp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            FileInfo file = new FileInfo(filename);
            fileExist = container.FileExists(filename);
            IsSelected = false;
        }

        /// <summary>
        /// Object Initialization : Game Data
        /// </summary>
        public void InitGamedata()
        {
            //FileInfo _file = new FileInfo(filename);
            //FileStream _stream = File.Open(filename, FileMode.Open);
            //time_stamp = _file.LastWriteTimeUtc;
            /* GERER MARSHALL LOAD
             * 
            character_names = Marshal.Load(_stream);
            character_hues = Marshal.Load(_stream);
            FrameCount = Marshal.Load(_stream);
            // GameSystem = Marshal.Load(_stream);
            InGame.Switches.datas = Marshal.Load(_stream);
            InGame.game_variables = Marshal.Load(_stream);
            total_sec = FrameCount / Graphics.FrameRate;
            _stream.Close();
            */
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            // Draw file number
            this.Contents.Font.Color = NormalColor;
            string name = "";
            // If save file exists
            if (fileExist)
                name = "Save " + (fileIndex + 1);
            else
                name = "Empty";
            // Draw file name
            this.Contents.DrawText(4, 0, 600, 32, name);
            nameWidth = Contents.TextSize(name).Width;
            
            /*if (fileExist)
            {
                // Draw Character
                for (int i = 0; i < characterNames.Length; i++)
                {
                    Bitmap bitmap = Cache.Character(characterNames[i], characterHues[i]);
                    int cw = bitmap.Rect.Width / 4;
                    int ch = bitmap.Rect.Height / 4;
                    Rectangle src_rect = new Rectangle(0, 0, cw, ch);
                    X = 300 - characterNames.Length * 32 + i * 64 - cw / 2;
                    this.Contents.Blit(X, 68 - ch, bitmap, src_rect);
                }
                // Draw play time
                int hour = (int)totalSec / 60 / 60;
                int min = (int)totalSec / 60 % 60;
                int sec = (int)totalSec % 60;
                StringBuilder time_sb = new StringBuilder(hour);
                time_sb.Append(":");
                time_sb.Append(min);
                time_sb.Append(":");
                time_sb.Append(sec);
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(4, 8, 600, 32, time_sb.ToString(), 2);
                // Draw timestamp
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(4, 40, 600, 32, timeStamp.ToLongTimeString(), 2);
            }*/
        }

        /// <summary>
        /// GeexMouse Rectangle Update
        /// </summary>
        public void UpdateCursorRect()
        {
            if (IsSelected)
            {
                this.CursorRect.Set(0, 0, nameWidth + 8, 32);
            }
            else
            {
                this.CursorRect.Empty();
            }
        }

        #endregion
    }
}
