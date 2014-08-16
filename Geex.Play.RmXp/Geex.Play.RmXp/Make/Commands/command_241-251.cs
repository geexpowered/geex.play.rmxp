using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Make
{
	/// <summary>
	/// This interpreter runs event commands. This class is used within the
	//  GameSystem class and the GameEvent class
	/// </summary>
	public sealed partial class Interpreter
	{
		/// <summary>
		/// Play Song
		/// </summary>	    
		bool Command241()
		{
			// Play Song
			AudioFile audio = new AudioFile(stringParams[0],intParams[0],intParams[1]);
			InGame.System.SongPlay(audio);
			return true;
		}
		
		/// <summary>
		/// Fade Out Song
		/// </summary>
		bool Command242()
		{
			// Fade out Song
			InGame.System.SongFade(intParams[0]);
			return true;
		}
		
		/// <summary>
		/// Play background Effect
		/// </summary>
		bool Command245()
		{
			// Play background Effect
			AudioFile audio = new AudioFile(stringParams[0],intParams[0],intParams[1]);
			InGame.System.BackgroundSoundPlay(audio);
			return true;
		}
		
		/// <summary>
		/// Fade Out background Effect
		/// </summary>	    
		bool Command246()
		{
			// Fade out background Effect
			InGame.System.BackgroundSoundFade(intParams[0]);
			return true;
		}
		
		/// <summary>
		/// Memorize Song/background Effect
		/// </summary>
		bool Command247()
		{
			// Memorize Song/background Effect
			InGame.System.SongMemorize();
			InGame.System.BackgroundSoundMemorize();
			return true;
		}
		
		/// <summary>
		/// Restore Song/background Effect
		/// </summary>	    
		bool Command248()
		{
			// Restore Song/background Effect
			InGame.System.SongRestore();
			InGame.System.BackgroundSoundRestore();
			return true;
		}
		
		/// <summary>
		/// Play Song Effect
		/// </summary>
		bool Command249()
		{
			// Play Song Effect
			AudioFile audio = new AudioFile(stringParams[0],intParams[0],intParams[1]);
			InGame.System.SongEffectPlay(audio);
			return true;
		}
		
		/// <summary>
		/// Play Sound Effect
		/// </summary>
		bool Command250()
		{
			AudioFile audio = new AudioFile(stringParams[0],intParams[0],intParams[1]);
			// Play Sound Effect
			InGame.System.SoundPlay(audio);
			return true;
		}
		
		/// <summary>
		/// Stop Sound Effect
		/// </summary>
		bool Command251()
		{
			// Stop Sound Effect
			Audio.SoundEffectStop();
			return true;
		}
	} 
}

