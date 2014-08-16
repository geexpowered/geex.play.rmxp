using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays the status of all party members on the battle screen.
    /// </summary>
    public partial class WindowBattleStatus : WindowBase
    {
        #region Variables

        /// <summary>
        /// A flag is up if a party member got his level up
        /// </summary>
        bool[] levelUpFlags = new bool[4];

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowBattleStatus()
            : base(0, GeexEdit.GameWindowHeight - 160, GeexEdit.GameWindowWidth, 160)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            for (int i = 0; i < levelUpFlags.Length; i++)
            {
                levelUpFlags[i] = false;
            }
            Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Put selected actor's level up flag up
        /// </summary>
        /// <param Name="actor_index">actor's party index</param>
        public void LevelUp(int actor_index)
        {
            levelUpFlags[actor_index] = true;
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
            //item_max = InGame.Party.actors.Count;
            for (int i = 0; i < InGame.Party.Actors.Count; i++)
            {
                GameActor _actor = InGame.Party.Actors[i];
                int _actor_x = i * 160 + 4;//GeexEdit.GameWindowWidth/2 - 160*InGame.Party.Actors.Count/2 + i * 160 + 4;
                DrawActorName(_actor, _actor_x, 0);
                DrawActorHp(_actor, _actor_x, 20, 120);
                DrawActorSp(_actor, _actor_x, 40, 120);
                if (levelUpFlags[i])
                {
                    this.Contents.Font.Color = NormalColor;
                    this.Contents.DrawText(_actor_x, 96, 120, 32, "LEVEL UP!");
                }
                else
                {
                    DrawActorState(_actor, _actor_x, 96);
                }
            }
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            base.Update();
            // Slightly lower opacity level during main phase
            if (InGame.Temp.BattleMainPhase)
            {
                if (this.ContentsOpacity > 191)
                {
                    this.ContentsOpacity -= 4;
                }
            }
            else
            {
                if (this.ContentsOpacity < 255)
                {
                    this.ContentsOpacity += 4;
                }
            }
        }

        #endregion
    }
}
