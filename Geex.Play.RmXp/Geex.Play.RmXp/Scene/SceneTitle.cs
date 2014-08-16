using System.Collections.Generic;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;
using Geex.Play.Custom;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Scene
{
	/// <summary>
	/// This class performs Title screen processing.
	/// </summary>
	public partial class SceneTitle : SceneBase
	{
		#region Variables
		Sprite title;
		bool continueEnabled;
		WindowCommand commandWindow;
		#endregion

		#region Methods
		/// <summary>
		/// Initialize SceneTitle
		/// </summary>
		public override void LoadSceneContent()
		{
			// Load database
			Data.Actors = Cache.ActorData("actors");
			Data.Classes = Cache.ClassData("classes");
			Data.Skills = Cache.SkillData("skills");
			Data.Items = Cache.ItemData("items");
			Data.Weapons = Cache.WeaponData("weapons");
			Data.Armors = Cache.ArmorData("armors");
			Data.Npcs = Cache.NpcData("npcs");
			Data.Troops = Cache.TroopData("troops");
			Data.States = Cache.StateData("states");
			Data.Animations = Cache.AnimationData("animations");
			Data.CommonEvents = Cache.CommonEventData("commonevents");
			Data.System = Cache.SystemData("system");
            Cache.LoadIcons("icons", "IconsReach");
			InGame.System = new GameSystem();
			InGame.Switches = new GameSwitches();
			InGame.Variables = new GameVariables();
			InGame.Temp = new GameTemp();

			// Title
			title = new Sprite(Graphics.Background);
			title.Z = 0;
			title.Bitmap = Cache.Title(Data.System.TitleName);
			string s1 = "New Game";
			string s2 = "Continue";
			string s3 = "Shutdown";
			commandWindow = new WindowCommand(192, new List<string>() { s1, s2, s3 });
			commandWindow.BackOpacity = 160;
			commandWindow.X = (GeexEdit.GameWindowWidth - commandWindow.Width) / 2;
			commandWindow.Y = GeexEdit.GameWindowHeight - 192;
			commandWindow.Z = 500;
			continueEnabled = true;
			// Play title BGM
			Audio.SongPlay(Data.System.TitleMusicLoop);
			// Stop playing Song effect and background sound
			Audio.SongEffectStop();
			Audio.BackgroundSoundStop();
		}

		/// <summary>
		/// Dispose SceneTitle
		/// </summary>
		public override void Dispose()
		{
			commandWindow.Dispose();
			title.Dispose();
		}

		/// <summary>
		/// Update SceneTitle
		/// </summary>
		public override void Update()
		{
			commandWindow.Update();
			if (Input.RMTrigger.C)
			{
				switch (commandWindow.Index)
				{
					case 0:
						CommandNewGame();
						break;
					case 1:
						CommandContinue();
						break;
					case 2:
						CommandShutdown();
						break;
				}
			}
		}

		///<summary>Start a new game</summary>
		void CommandNewGame()
		{
			Graphics.Freeze();
			Audio.SoundEffectPlay(Data.System.DecisionSoundEffect);
			Audio.SongStop();
			// Set game key variables
			Graphics.FrameCount = 0;
			InGame.Screen = new GameScreen();
			InGame.Actors = new GameActors();
			InGame.Npcs = new GameBattler();
			InGame.Party = new GameParty();
			InGame.Player = new GamePlayer();
			InGame.Map = new GameMap();
			InGame.Troops = new GameTroop();
			InGame.Party.SetupStartingMembers();
			InGame.Map.Setup(Data.System.StartMapId);
            if (GameOptions.IsTileByTileMoving)
            {
                InGame.Player.Moveto(Data.System.StartX * 32 + 16, Data.System.StartY * 32 + 31);
            }
            else
            {
                InGame.Player.Moveto(Data.System.StartX * 32 + 16, Data.System.StartY * 32 + 32);
            }
			InGame.Player.Refresh();
			InGame.Map.Autoplay();
			Main.Scene = new SceneMap();
			InGame.Map.Update();
		}

		///<summary>Load a game</summary>
		void CommandContinue()
		{
			Graphics.Freeze();
			if (!continueEnabled)
			{
				Audio.SoundEffectPlay(Data.System.BuzzerSoundEffect);
				return;
			}
			Audio.SoundEffectPlay(Data.System.DecisionSoundEffect);
			Main.Scene = new SceneLoad();
		}

		///<summary>Exit game</summary>
		void CommandShutdown()
		{
			Audio.SoundEffectPlay(Data.System.DecisionSoundEffect);
			Audio.SongFadeOut(800);
			Audio.BackgroundSoundFadeOut(800);
			Audio.SongEffectFadeOut(800);
			Main.Scene = null;
		}
		#endregion
	}
}
