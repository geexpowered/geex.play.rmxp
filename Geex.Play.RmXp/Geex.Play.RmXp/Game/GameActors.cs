namespace Geex.Play.Rpg.Game
{
    ///<summary>This class is a GameActor collection, loaded at the game starting up. 
    public partial class GameActors
    {
        #region Variables

        /// <summary>
        /// Actor list
        /// </summary>
        public GameActor[] data;

        #endregion

        #region Initialize

        ///<summary>Initialize GameActors</summary>
        public GameActors()
        {
            data = new GameActor[Data.Actors.Length];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Select an actor by its id. Replaces the ruby [] operator. 
        /// Use InGame.Actors.SoundEffectlect(actor_id)instead of $game_actors[i]
        /// </summary>
        ///<param Name="id">Actor id</param>
        public GameActor this[int id]
        {
            get
            {
                if (id > Data.Actors.Length || Data.Actors[id] == null)
                {
                    return null;
                }
                if (data[id] == null)
                {
                    data[id] = new GameActor(id);
                }
                return data[id];
            }
        }

        #endregion
    }
}
