using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;

namespace Geex.Play.Rpg.Scene
{
	/// <summary>
	/// This class performs game end screen processing.
	/// </summary>
	public partial class SceneEnd : SceneBase
	{
		#region Variables

		/// <summary>
		/// Managed command window
		/// </summary>
		WindowCommand commandwindow;

		#endregion

		#region Initialize

		/// <summary>
		/// Initialize
		/// </summary>
		public override void LoadSceneContent()
		{
			InitializeWindows();
		}

		/// <summary>
		/// Windows initialization
		/// </summary>
		void InitializeWindows()
		{
			List<string> Commandlist = new List<string>();
			Commandlist.Add("To Title");
			Commandlist.Add("Shutdown");
			Commandlist.Add("Cancel");
			commandwindow = new WindowCommand(192, Commandlist);
			commandwindow.X = 320 - commandwindow.Width / 2;
			commandwindow.Y = 240 - commandwindow.Height / 2;
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Dispose
		/// </summary>
		public override void Dispose()
		{
			// Dispose of window
			commandwindow.Dispose();
			// If switching to title screen
			if (Main.Scene.IsA("SceneTitle"))
			{
				// Fade out screen
				Graphics.Transition();
				InGame.Screen.ColorTone = Tone.Empty;
				Graphics.Background.Tone = Tone.Empty;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Frame Update
		/// </summary>
		public override void Update()
		{
			// Update command window
			commandwindow.Update();
			// If B button was pressed
			if (Input.RMTrigger.B)
			{
				// Play cancel SE
				InGame.System.SoundPlay(Data.System.CancelSoundEffect);
				// Switch to menu screen
			    Main.Scene = new SceneMenu(5);
			    return;
			}
			// If C button was pressed
			if (Input.RMTrigger.C)
			{
				// Branch by command window cursor position
				switch (commandwindow.Index)
				{
					case 0:  // to title
						CommandToTitle();
						break;
					case 1:  // shutdown
						CommandShutdown();
						break;
					case 2:  // quit
						CommandCancel();
						break;
				}
				return;
			}
		}

		/// <summary>
		/// Process When Choosing [To Title] Command
		/// </summary>
		void CommandToTitle()
		{
			// Play decision SE
			InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
			// Fade out BGM, BGS, and ME
			Audio.SongFadeOut(800);
			Audio.BackgroundSoundFadeOut(800);
			Audio.SongEffectFadeOut(800);
			// Switch to title screen
			Main.Scene = new SceneTitle();
		}

		/// <summary>
		/// Process When Choosing [Shutdown] Command
		/// </summary>
		void CommandShutdown()
		{
			// Play decision SE
			InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
			// Fade out BGM, BGS, and ME
			Audio.SongFadeOut(800);
			Audio.BackgroundSoundFadeOut(800);
			Audio.SongEffectFadeOut(800);
			// Shutdown
			Main.Scene = null;
		}

		/// <summary>
		/// Process When Choosing [Cancel] Command
		/// </summary>
		public void CommandCancel()
		{
			// Play decision SE
			InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
			// Switch to menu screen
			Main.Scene = new SceneMenu(5);
		}

		#endregion
	}
}
