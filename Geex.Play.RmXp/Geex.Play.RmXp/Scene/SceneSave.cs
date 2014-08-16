using System.IO;
using System.Xml.Serialization;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;
using Geex.Play.Rpg.Utils;
using Microsoft.Xna.Framework.Storage;
#if XBOX
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace Geex.Play.Rpg.Scene
{
	/// <summary>
	/// This class performs save screen processing.
	/// </summary>
	public partial class SceneSave : SceneFile
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
			base.Initialize("Which file would you like to save to?");
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
		/// Save Game State to Device
		/// </summary>
		/// <param name="device"></param>
		void SaveToDevice()
		{
            string filename = MakeFilename(fileIndex);
			// Check to see whether the save exists.
			if (container.FileExists(filename))
			{
				// Delete it so that we can create one fresh.
				container.DeleteFile(filename);
			}
			Stream stream = container.CreateFile(filename);
			// Convert the object to XML data and put it in the stream.
			XmlSerializer serializer = new XmlSerializer(typeof(SavedGame));
			serializer.Serialize(stream, savedGame);
			// Close the file.
			stream.Close();
			// Dispose the container, to commit changes.
			container.Dispose();
		}

        /// <summary>
        /// On processing
        /// </summary>
        public override void Processing()
        {
            if (isProcessing && storageDevice != null && storageDevice.IsConnected)
            {
                PrepareData();
                SaveToDevice();
                SwitchScene();
                isProcessing = false;
            }
        }

		/// <summary>
		/// Decision Processing
		/// </summary>
		/// <param name="filename">filename string</param>
		public override void OnDecision(string filename)
		{
			// Play save SE
			InGame.System.SoundPlay(Data.System.SaveSoundEffect);
            // Save
            isProcessing = true;
		}

		/// <summary>
		/// Cancel Processing
		/// </summary>
		public override void OnCancel()
		{
			// Play cancel SE
			InGame.System.SoundPlay(Data.System.CancelSoundEffect);
			// If called from event
			if (InGame.Temp.IsCallingSave)
			{
				// Clear save call flag
				InGame.Temp.IsCallingSave = false;
				// Switch to map screen
				Main.Scene = new SceneMap();
				return;
			}
			// Switch to menu screen
			Main.Scene = new SceneMap();
		}

		/// <summary>
		/// Write data in SavedGame object
		/// </summary>
		public override void PrepareData()
		{
			// Make character data for drawing save file
			string[] _character_names = new string[InGame.Party.Actors.Count];
			int[] _character_hues = new int[InGame.Party.Actors.Count];
			for (int i = 0; i < InGame.Party.Actors.Count; i++)
			{
				_character_names[i] = InGame.Party.Actors[i].CharacterName;
				_character_hues[i] = InGame.Party.Actors[i].CharacterHue;
			}
			// Increase save count by 1
			InGame.System.SaveCount += 1;
			savedGame.CharacterNames = _character_names;
			savedGame.CharacterHues = _character_hues;
			savedGame.FrameCount = Graphics.FrameCount;
			savedGame.GameSystem = InGame.System;
			savedGame.GameSwitchesData = InGame.Switches;
			savedGame.GameVariablesData = InGame.Variables;
			savedGame.GameScreen = InGame.Screen;
			savedGame.GameActors = InGame.Actors;
			savedGame.GameParty = InGame.Party;
			savedGame.GameTroop = InGame.Troops;
			savedGame.GameMap = InGame.Map;
			savedGame.GamePlayer = InGame.Player;
		}
		
	    /// <summary>
	    /// Switch to SceneMap
	    /// </summary>
		public override void SwitchScene()
		{
			// If called from event
			if (InGame.Temp.IsCallingSave)
			{
				// Clear save call flag
				InGame.Temp.IsCallingSave = false;
				// Switch to map screen
			}
			// Switch to menu screen
			Main.Scene = new SceneMap();
		}

		#endregion
	}
}
