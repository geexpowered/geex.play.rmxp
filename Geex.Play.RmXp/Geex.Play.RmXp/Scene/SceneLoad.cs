using System.IO;
using System.Xml.Serialization;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;
using Microsoft.Xna.Framework.Storage;
using Geex.Play.Rpg.Utils;
using Geex.Play.Custom;
#if XBOX
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace Geex.Play.Rpg.Scene
{
	/// <summary>
	/// This class performs load screen processing.
	/// </summary>
	public partial class SceneLoad : SceneFile
	{
		#region Initialize

		/// <summary>
		/// Initialize
		/// </summary>
		public override void LoadSceneContent()
		{
#if XBOX
			// Set the request flag
			if ((!Guide.IsVisible))
			{
				syncResult = StorageDevice.BeginShowSelector(Pad.PlayerIndex, null, null);
			}
#else
            syncResult = StorageDevice.BeginShowSelector(Pad.ActivePlayer, null, null);
#endif
            isOpeningContainer = true;
            Graphics.Transition(40);
			base.Initialize("Which file would you like to load?");
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Dispose
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Frame Update
		/// </summary>
		public override void Update()
		{
			base.Update();
		}
	
		/// <summary>
		/// Load Game State from Device
		/// </summary>
		/// <param name="device"></param>
		void LoadFromDevice(StorageDevice device)
		{
			// Open the file.
			InGame.System.GameSelfSwitches.Clear();
			Stream stream = container.OpenFile(MakeFilename(fileIndex), FileMode.Open);
			XmlSerializer serializer = new XmlSerializer(typeof(SavedGame));
			savedGame = (SavedGame)serializer.Deserialize(stream);
			// Close the file.
			stream.Close();
			// Dispose the container.
			container.Dispose();
		}

        /// <summary>
        /// On processing
        /// </summary>
        public override void Processing()
        {
            // Load game
            if (isProcessing && storageDevice != null && storageDevice.IsConnected)
            {
                // Load database
                // Check to see whether the save exists.
                if (!container.FileExists(MakeFilename(fileIndex)))
                {
                    // If not, Dispose of the container and return.
                    container.Dispose();
                    OnCancel();
                    isProcessing = false;
                    return;
                }
                LoadFromDevice(storageDevice);
                RetrieveSavedDatas();
                // ReStart Game
                InGame.Map.Setup(InGame.Map.MapId);
                InGame.Player.Moveto(InGame.Player.X, InGame.Player.Y);
                InGame.Player.Center(InGame.Player.X, InGame.Player.Y);
                // Refresh party members
                InGame.Party.Refresh();
                SwitchScene();
                isProcessing = false;
            }
        }

		/// <summary>
		/// Decision Processing
		/// </summary>
		/// <param Name="filename">filename string</param>
		public override void OnDecision(string filename)
		{
			// Play load SE
			InGame.System.SoundPlay(Data.System.LoadSoundEffect);
            isProcessing = true;
		}

		/// <summary>
		/// Cancel Processing
		/// </summary>
		public override void OnCancel()
		{
			// Play cancel SE
			InGame.System.SoundPlay(Data.System.CancelSoundEffect);
			// Switch to title screen
            this.Dispose();
			Main.Scene = new SceneTitle();
            // Transition
            Graphics.Transition();
            // Reset graphics
            Graphics.Background.Tone = Tone.Empty;
		}

		/// <summary>
		/// Nothing for data loading
		/// </summary>
		public override void PrepareData()
		{
			// Do nothing
		}
		/// <summary>
		/// Load SavedGame data into game
		/// </summary>
		void RetrieveSavedDatas()
		{
			string[] character_names = savedGame.CharacterNames;
			int[] character_hues = savedGame.CharacterHues;
			Graphics.FrameCount = savedGame.FrameCount;
			InGame.System = savedGame.GameSystem;
			InGame.Switches = savedGame.GameSwitchesData;
			InGame.Variables = savedGame.GameVariablesData;
			InGame.Screen = savedGame.GameScreen;
			InGame.Actors = savedGame.GameActors;
			InGame.Party = savedGame.GameParty;
			InGame.Troops = savedGame.GameTroop;
			InGame.Map = savedGame.GameMap;
			InGame.Player = savedGame.GamePlayer;
		}

		/// <summary>
		/// Change scene
		/// </summary>
		public override void SwitchScene()
		{
			// Play load SE
			InGame.System.SoundPlay(Data.System.LoadSoundEffect);
            // Stop title song
            Audio.SongFadeOut(200);
			// Restore BGM and BGS
			// Run automatic change for BGM and BGS set on the map
			InGame.Map.Autoplay();
			// Switch to map screen
            this.Dispose();
            Graphics.Transition(40);
			Main.Scene = new SceneMap();
		}

		#endregion
	}
}
