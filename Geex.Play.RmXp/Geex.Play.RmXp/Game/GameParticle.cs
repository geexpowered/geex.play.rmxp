using Geex.Play.Make;
using Geex.Play.Rpg.Utils;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class holds the game particles data
    /// </summary>
    public class GameParticle
    {
        #region Variables
        /// <summary>
        /// The GameEvent where the particle is created
        /// </summary>
        public GameEvent FromEvent;
        /// <summary>
        /// The Particle Effect
        /// </summary>
        public ParticleEffect Effect;
        #endregion
        /// <summary>
        /// Initiliaze a new Game Particle
        /// </summary>
        /// <param Name="ev">The GameEvent where the particle is created</param>
        /// <param Name="particleEffect">The Particle Effect</param>
        public GameParticle(GameEvent ev, ParticleEffect particleEffect)
        {
            FromEvent = ev;
            Effect = particleEffect;
        }
        /// <summary>
        /// Parameterless constructor for load/save
        /// </summary>
        public GameParticle() : this(null,ParticleEffect.None)
        {}
    }
}
