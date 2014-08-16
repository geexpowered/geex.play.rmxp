using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;

namespace Geex.Play.Rpg.Scene
{
	/// <summary>
	/// This class performs status screen processing.
	/// </summary>
	public partial class SceneStatus : SceneBase
	{
		#region Variables

		/// <summary>
		/// Actor index
		/// </summary>
		int actorIndex=0;

		/// <summary>
		/// Status window actor
		/// </summary>
		GameActor actor;

		/// <summary>
		/// Managed status window
		/// </summary>
		WindowStatus statusWindow;

		#endregion

		#region Initialize

		/// <summary>
		/// Constuctor for actor browsing
		/// </summary>
		/// <param Name="_index">actor index in party</param>
		public SceneStatus(int _index)
			: base()
		{
			actorIndex = _index;
			//initialize(actor_index, 0);
		}

		/// <summary>
		/// Initialize (default : actor_index = 0, equip_index = 0)
		/// </summary>
		public override void LoadSceneContent()		{
			SetUp(actorIndex, 0);
		}

		/// <summary>
		/// Initialize
		/// </summary>
		/// <param Name="actor_index">actor index</param>
		/// <param Name="equip_index">equip index</param>
		void SetUp(int actor_index, int equip_index)
		{
			this.actorIndex = actor_index;
			// Get actor
			actor = InGame.Party.Actors[actor_index];
			// Make status window
			statusWindow = new WindowStatus(actor);
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Dispose
		/// </summary>
		public override void Dispose()
		{
			// Prepare for Transition
			//Graphics.Freeze();
			// Dispose of windows
			statusWindow.Dispose();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Frame Update
		/// </summary>
		public override void Update()
		{
			// If B button was pressed
			if (Input.RMTrigger.B)
			{
				// Play cancel SE
				InGame.System.SoundPlay(Data.System.CancelSoundEffect);
				// Switch to menu screen
				Main.Scene = new SceneMenu(3);
				return;
			}
			// If R button was pressed
			if (Input.RMTrigger.R)
			{
				// Play cursor SE
				InGame.System.SoundPlay(Data.System.CursorSoundEffect);
				// To next actor
				actorIndex += 1;
				actorIndex %= InGame.Party.Actors.Count;
				// Switch to different status screen
				Main.Scene = new SceneStatus(actorIndex);
				return;
			}
			// If L button was pressed
			if (Input.RMTrigger.L)
			{
				// Play cursor SE
				InGame.System.SoundPlay(Data.System.CursorSoundEffect);
				// To previous actor
				actorIndex += InGame.Party.Actors.Count - 1;
				actorIndex %= InGame.Party.Actors.Count;
				// Switch to different status screen
				Main.Scene = new SceneStatus(actorIndex);
				return;
			}
		}

		#endregion
	}
}
