using System;
using System.Collections.Generic;
using System.Text;
using Geex.Run;
using Geex.Play.Make;
using Geex.Play.Rpg.Window;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class handles the game system data
    /// </summary>
    public class GameSystem
    {
        #region Variables
        /// <summary>
        /// Power of 2 speeds
        /// </summary>
        public short[] Speed = new short[7] { 0, 1, 2, 3, 4, 6, 8 };
        
        /// <summary>
        /// Game self switches
        /// </summary>
        public GameSelfSwitches GameSelfSwitches = new GameSelfSwitches(); 

        /// <summary>
        /// Timer
        /// </summary>
        public int Timer = 0;

        /// <summary>
        /// Timer working flag
        /// </summary>
        public bool IsTimerWorking = false;

        /// <summary>
        /// True if save/load is forbidden
        /// </summary>
        public bool IsSaveDisabled = false;

        /// <summary>
        /// True if menu is forbidden
        /// </summary>
        public bool IsMenuDisabled = false;

        /// <summary>
        /// encounter forbidden
        /// </summary>
        public bool IsEncounterDisabled = false;

        /// <summary>
        /// mode game frozen
        /// </summary>
        public bool IsFreezeMode = false;

        /// <summary>
        /// text option: positioning
        /// </summary>
        public int MessagePosition = 2;

        /// <summary>
        /// text option: window frame
        /// </summary>
        public int MessageFrame;

        /// <summary>
        /// save count
        /// </summary>
        public int SaveCount = 0;

        /// <summary>
        /// magic number
        /// </summary>
        public int MagicNumber = 0;

        /// <summary>
        /// Get Music Loop
        /// </summary>
        public AudioFile PlayingSong;

        /// <summary>
        /// Remember last Song
        /// </summary>
        AudioFile lastSong;

        /// <summary>
        /// Play Sound Loop
        /// </summary>
        public AudioFile PlayingBackgroundSound;

        /// <summary>
        /// Sound Loop Memory
        /// </summary>
        AudioFile lastBackgroundSound;

        #endregion

        #region Properties
        /// <summary>
        /// Get or Set Battle Background Music
        /// </summary>
        public AudioFile BattleSong
        {
            get
            {
                if (localBattleSong == null)
                {
                    return Data.System.BattleMusicLoop;
                }
                else
                {
                    return localBattleSong;
                }
            }
            set
            {
                localBattleSong = value;
            }
        }
        /// <summary>
        /// Reference to battle_bgm property
        /// </summary>
         AudioFile localBattleSong = null;

        /// <summary>
        /// Get or Set Windowskin File localName
        /// </summary>
        public string WindowskinName
        {
            get
            {
                if (localWindowSkinName == "")
                {
                    return Data.System.WindowskinName;
                }
                else
                {
                    return localWindowSkinName;
                }
            }
            set
            {
                localWindowSkinName = value;
            }
        }
        /// <summary>
        /// Reference to windowskin_name property
        /// </summary>
         string localWindowSkinName = "";

        /// <summary>
        /// Get or Set Music Effect for Battle Ending
        /// </summary>
        public  AudioFile BattleEndSongEffect
        {
            get
            {
                if (LocalBattleEndSongEffect == null)
                {
                    return Data.System.BattleEndMusicEffect;
                }
                else
                {
                    return LocalBattleEndSongEffect;
                }
            }
            set
            {
                LocalBattleEndSongEffect = value;
            }
        }
        AudioFile LocalBattleEndSongEffect = null;
        #endregion

        #region Constructor
        /// <summary>
        /// This class handles the game system data
        /// </summary>
        public GameSystem()
        {}
        #endregion

        #region Methods

         /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            if (IsTimerWorking & Timer > 0) Timer -= 1;
        }

        #region Methods - Songs

        /// <summary>
        /// Play Background Music
        /// </summary>
        /// <param Name="Song">background music to be played</param>
        public void SongPlay(AudioFile Song)
        {
            PlayingSong = Song;
            if (Song != null)
            {
                if (Song.Name != "")
                {
                    Audio.SongPlay(Song);
                }
            }
            else
            {
                Audio.SongStop();
            }
        }
           

        /// <summary>
        /// Stop Song
        /// </summary>
        public void SongStop()
        {
            Audio.SongStop();
        }

        /// <summary>
        /// Fade Out Song
        /// </summary>
        /// <param Name="second"> fade-out time (in seconds)</param>
        public  void SongFade(int second)
        {
            PlayingSong = null;
            Audio.SongFadeOut(second * 1000);
        }

        /// <summary>
        /// Remember Song
        /// </summary>
        public void SongMemorize()
        {
            lastSong = PlayingSong;
        }

        /// <summary>
        /// Restore Song
        /// </summary>
        public void SongRestore()
        {
            SongPlay(lastSong);
        }
        #endregion

        #region Methods - Background Effects

        /// <summary>
        /// Play Sound Loop
        /// </summary>
        /// <param Name="soundLoop">background to be played</param>
        public void BackgroundSoundPlay(AudioFile soundLoop)
        {
            PlayingBackgroundSound = soundLoop;
            if (soundLoop != null && soundLoop.Name != "")
            {
                Audio.BackgroundSoundPlay(soundLoop);
            }
            else
            {
                Audio.BackgroundSoundStop();
            }
        }

        /// <summary>
        /// Fade Out Sound Loop
        /// </summary>
        /// <param Name="second">fade-out time (in seconds)</param>
        public void BackgroundSoundFade(int second)
        {
            PlayingBackgroundSound = null;
            Audio.BackgroundSoundFadeOut(second * 1000);
        }

        /// <summary>
        /// Sound Loop Memory
        /// </summary>
        public void BackgroundSoundMemorize()
        {
            lastBackgroundSound = PlayingBackgroundSound;
        }

        /// <summary>
        /// Restore Sound Loop
        /// </summary>
        public void BackgroundSoundRestore()
        {
            BackgroundSoundPlay(lastBackgroundSound);
        }

        #endregion

        #region Methods - SongEffect

        /// <summary>
        /// Play Music Effect
        /// </summary>
        /// <param Name="musicEffect">music Effect to be played</param>
        public void SongEffectPlay(AudioFile musicEffect)
        {
            if (musicEffect != null & musicEffect.Name != "")
            {
                Audio.SongEffectPlay(musicEffect);
            }
            else
            {
                Audio.SongEffectStop();
            }
        }

        /// <summary>
        /// Play Music Effect
        /// </summary>
        /// <param Name="musicEffect">music Effect to be played</param>
        public void SongEffectStop()
        {
            if (Audio.IsSongEffectPlaying)
            {
                Audio.SongEffectStop();
            }
        }

        #endregion

        #region Methods - SoundEffect

        /// <summary>
        /// Play Sound Effect
        /// </summary>
        /// <param Name="se">sound Effect to be played</param>
        public void SoundPlay(AudioFile se)
        {
            if (se != null & se.Name != "")
            {
                Audio.SoundEffectPlay(se);
            }
        }

        /// <summary>
        /// Stop Sound Effect
        /// </summary>
        public void SoundStop()
        {
            Audio.SoundEffectStop();
        }

        #endregion

        #region Methods - Conditions

        /// <summary>
        /// Conditions for event are met
        /// </summary>
        /// <param Name="map_id">map ID</param>
        /// <param Name="event_id">event ID</param>
        /// <param Name="condition">analysed condition</param>
        /// <returns>true if conditions are met</returns>
        public bool IsEventConditionsMet(int map_id, int event_id, Event.Page.Condition condition)
        {
            // Switch 1 condition confirmation
            if (condition.Switch1Valid && !InGame.Switches.Arr[condition.Switch1Id])
            {
                return false;
            }
            // Switch 2 condition confirmation
            if (condition.Switch2Valid && !InGame.Switches.Arr[condition.Switch2Id])
            {
                return false;
            }
            // Variable condition confirmation
            if (condition.VariableValid && InGame.Variables.Arr[condition.VariableId] < condition.VariableValue)
            {
                return false;
            }
            // Self switch condition confirmation
            if (condition.SelfSwitchValid)
            {
                GameSwitch sw = new GameSwitch(map_id, event_id, condition.SelfSwitchCh);
                if (InGame.System.GameSelfSwitches[sw])
                {
                    return InGame.System.GameSelfSwitches[sw];
                }
                return false;
            }
            // Returns True
            return true;
        }

        /// <summary>
        /// Condition for troop are met
        /// </summary>
        /// <param Name="page_index">troop event's page index</param>
        /// <param Name="condition">analysed condition</param>
        /// <returns>true if conditions are met</returns>
        public bool IsTroopConditionsMet(int page_index, Troop.Page.Condition condition)
        {
            // Return False if no conditions appointed
            if (!(condition.TurnValid || condition.NpcValid || condition.ActorValid || condition.SwitchValid))
            {
                return false;
            }
            // Return False if page already completed
            if ((bool)InGame.Temp.BattleEventFlags[page_index])
            {
                return false;
            }
            // Confirm turn conditions
            if (condition.TurnValid)
            {
                int n = InGame.Temp.BattleTurn;
                int a = condition.TurnA;
                int b = condition.TurnB;
                if ((b == 0 && n != a) || (b > 0 && (n < 1 || n < a || n % b != a % b)))
                {
                    return false;
                }
            }
            // Confirm enemy conditions
            if (condition.NpcValid)
            {
                GameNpc enemy = InGame.Troops.Npcs[condition.NpcIndex];
                if (enemy == null || enemy.Hp * 100.0 / enemy.MaxHp > condition.NpcHp)
                {
                    return false;
                }
            }
            // Confirm actor conditions
            if (condition.ActorValid)
            {
                GameActor actor = InGame.Actors[condition.ActorId-1];
                if (actor == null || actor.Hp * 100.0 / actor.MaxHp > condition.ActorHp)
                {
                    return false;
                }
            }
            // Confirm switch conditions
            if (condition.SwitchValid)
            {
                if (InGame.Switches.Arr[condition.SwitchId])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // Return True
            return true;
        }

        #endregion

        #endregion
    }
}
