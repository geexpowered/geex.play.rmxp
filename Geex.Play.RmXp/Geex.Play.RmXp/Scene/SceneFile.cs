using System;
using System.Text;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Utils;
using Geex.Play.Rpg.Window;
using Geex.Run;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace Geex.Play.Rpg.Scene
{
	/// <summary>
	/// This is a superclass for the save screen and load screen.
	/// </summary>
	public partial class SceneFile : SceneBase
	{
		#region Variables
		/// <summary>
		/// Text string shown in the help window
		/// </summary>
		string helpText;

		/// <summary>
		/// Managed help window
		/// </summary>
		WindowHelp helpWindow;

		/// <summary>
		/// Managed savefile windows array
		/// </summary>
		WindowSaveFile[] savefileWindows;

		/// <summary>
		/// File index
		/// </summary>
		protected int fileIndex;
		
        /// <summary>
        /// True if system can load a game
        /// </summary>
        bool isLoadingReady;

		/// <summary>
		/// Async result for save selection
		/// </summary>
		protected IAsyncResult syncResult;
		/// <summary>
		/// Actual game storage device
		/// </summary>
		protected StorageContainer container;
		/// <summary>
		/// Actual game storage device
		/// </summary>
		protected StorageDevice storageDevice;
		/// <summary>
		/// Get Saved game datas
		/// </summary>
		protected SavedGame savedGame = new SavedGame();

        protected bool isOpeningContainer = false;
        protected bool isContainerOpened = false;
        protected bool isProcessing = false;
		#endregion

		#region Initialize

		/// <summary>
		/// Initialize
		/// </summary>
		public override void LoadSceneContent()
		{
			Initialize("");
            isLoadingReady = false;
		}

		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="helpText">text string shown in the help window</param>
		public void Initialize(string helpText)
		{   
            // Set help text
			this.helpText = helpText;
			// Initialize SavegGame object
			savedGame = new SavedGame();
            // Forbid loading until windows set
            isLoadingReady = false;
            // Windows are initialised in daughter class
            // when container is open
		}

        /// <summary>
        /// Initialize savegame windows
        /// </summary>
		public void InitializeWindows()
		{
			// Make help window
			helpWindow = new WindowHelp();
			helpWindow.SetText(helpText);
			// Make save file window
			savefileWindows = new WindowSaveFile[4];
			for (int i = 0; i < 4; i++)
			{
				savefileWindows[i] = new WindowSaveFile(i, MakeFilename(i), container);
			}
			// Select last file to be operated
			fileIndex = InGame.Temp.LastFileIndex;
			savefileWindows[fileIndex].IsSelected = true;
            // Loading is now possible
            isLoadingReady = true;
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Dispose
		/// </summary>
		public override void Dispose()
		{
			// Prepare for transition
			Graphics.Freeze();
			// Dispose of windows
			DisposeWindows();
		}
		
        /// <summary>
        /// Dispose save windows
        /// </summary>
		void DisposeWindows()
		{
			helpWindow.Dispose();
			foreach (WindowSaveFile window in savefileWindows)
			{
				window.Dispose();
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Frame update
		/// </summary>
		public override void Update()
		{
            // Save management
            OpeningContainer();
            RetrievingSave();
            Processing();
			// If C button was pressed
            if (Geex.Run.Input.RMTrigger.C && isLoadingReady)
			{
				// Call method: on_decision (defined by the subclasses)
                OnDecision(MakeFilename(fileIndex));
				InGame.Temp.LastFileIndex = fileIndex;
				return;
			}
			// If B button was pressed
            if (Geex.Run.Input.RMTrigger.B)
			{
				// Call method: on_cancel (defined by the subclasses)
				OnCancel();
				return;
			}
			// If the down directional button was pressed
            if (Geex.Run.Input.RMTrigger.Down)
			{
				// Play cursor SE
				InGame.System.SoundPlay(Data.System.CursorSoundEffect);
				// Move cursor down
				savefileWindows[fileIndex].IsSelected = false;
				fileIndex = (fileIndex + 1) % 4;
				savefileWindows[fileIndex].IsSelected = true;
				return;
			}
			// If the up directional button was pressed
            if (Geex.Run.Input.RMTrigger.Up)
			{
				// Play cursor SE
				InGame.System.SoundPlay(Data.System.CursorSoundEffect);
				// Move cursor up
				savefileWindows[fileIndex].IsSelected = false;
				fileIndex = (fileIndex + 3) % 4;
				savefileWindows[fileIndex].IsSelected = true;
				return;
			}
		}


        /// <summary>
        /// On opening container
        /// </summary>
        public void OpeningContainer()
        {
            // Loading
            if (isOpeningContainer && syncResult.IsCompleted)
            {
                storageDevice = StorageDevice.EndShowSelector(syncResult);
                syncResult = storageDevice.BeginOpenContainer("GeexStorage", null, null);
                syncResult.AsyncWaitHandle.WaitOne();
                container = storageDevice.EndOpenContainer(syncResult);
                syncResult.AsyncWaitHandle.Close();
                isOpeningContainer = false;
                isContainerOpened = true;
            }
        }

        /// <summary>
        /// On retrieving saves
        /// </summary>
        public void RetrievingSave()
        {
            // When container opened, initialize windows
            if (isContainerOpened && storageDevice != null && storageDevice.IsConnected)
            {
                InitializeWindows();
                isContainerOpened = false;
            }
        }

        /// <summary>
        /// On opening container
        /// </summary>
        public virtual void Processing()
        {

        }

		/// <summary>
		/// Make File Name
		/// </summary>
		/// <param name="file_index">save file index (0-3)</param>
		public string MakeFilename(int file_index)
		{
			StringBuilder filename = new StringBuilder("Save");
			filename.Append(file_index + 1);
			filename.Append(".geexsave");
			return filename.ToString();
		}

		/// <summary>
		/// On decision (defined by subclasses)
		/// </summary>
		/// <param name="filename">save file index (0-3)</param>
		public virtual void OnDecision(string filename)
		{

		}

		/// <summary>
		/// On cancel (defined by subclasses)
		/// </summary>
		public virtual void OnCancel()
		{

		}
		
		/// <summary>
		/// Prepare data for saving/loading
		/// </summary>
		public virtual void PrepareData()
		{

		}
		
		/// <summary>
		/// Serialize or deserialize data
		/// </summary>
		public virtual void ManageSerialization(StorageDevice device)
		{

		}
		
		/// <summary>
		/// Proceed scene switching
		/// </summary>
		public virtual void SwitchScene()
		{

		}

		#endregion
	}
}
