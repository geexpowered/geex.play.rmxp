using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;
using LocalSprite = Geex.Run.Sprite;

namespace Geex.Play.Rpg.Spriting
{
    /// <summary>
    /// This class manages Sprite used to display the particles
    /// </summary>
    public class SpriteParticle
    {
        #region Variables
        /// <summary>
        /// Folder of particle
        /// </summary>
        protected string particleFolder;
        /// <summary>
        /// Filename of particle
        /// </summary>
        protected string particleName;
        /// <summary>
        /// Opacity of starting particle
        /// </summary>
        protected byte originalOpacity;
        /// <summary>
        /// Number of particles per Effect
        /// </summary>
        protected int maxParticles;
        /// <summary>
        /// Offset for the particles x
        /// </summary>
        protected int xOffset;
        /// <summary>
        /// offset for the particles y 
        /// </summary>
        protected int yOffset;
        /// <summary>
        /// Hue transformation of particles
        /// </summary>
        protected int hue;
        /// <summary>
        /// Slow Down parameter of particles
        /// </summary>
        //protected float slowdown;
        /// <summary>
        /// Gravity for x axis (goes by tens)
        /// </summary>
        protected float xGravity;
        /// <summary>
        /// Gravity for y axis (goes by tens)
        /// </summary>
        protected float yGravity;
        /// <summary>
        /// Reference to the Game Event managing the particle
        /// </summary>
        public GameEvent Ev;
        /// <summary>
        /// True if particle should fade away
        /// </summary>
        protected bool isFading;
        /// <summary>
        /// True if particle should radomly move left right
        /// </summary>
        protected bool isLeftRight;
        /// <summary>
        /// True if particle should change hue randomly
        /// </summary>
        protected bool isRandomHue;
        /// <summary>
        /// Top half of the screen pixel
        /// </summary>
        protected int ytop = 0;
        /// <summary>
        /// Screen Height
        /// </summary>
        protected int ybottom = GeexEdit.GameWindowHeight;
        /// <summary>
        /// First pixel to the left
        /// </summary>
        protected int xleft = 0;
        /// <summary>
        /// Screen Width
        /// </summary>
        protected int xright = GeexEdit.GameWindowWidth;
        /// <summary>
        /// Array of sprite particles
        /// </summary>
        protected LocalSprite[] particles;
        /// <summary>
        /// Array of Particles opacity
        /// </summary>
        protected int[] opacity;
        /// <summary>
        /// Value of opacity to be decreased for fade away
        /// </summary>
        protected byte fadingOpacity;
        /// <summary>
        /// Particule starting x position
        /// </summary>
        protected int startingX;
        /// <summary>
        /// Value for particle X start is spreading between - spreadingOverX/2 and +spreadingOverX/2
        /// </summary>
        protected int spreadingOverX;

        /// <summary>
        /// Particule starting y position
        /// </summary>
        protected int startingY;
        /// <summary>
        /// Particule x position on screen
        /// </summary>
        protected int screenX;
        /// <summary>
        /// Particule y position on screen
        /// </summary>
        protected int screenY;
        /// <summary>
        /// Blend option for sprite particles
        /// </summary>
        protected int blendValue;
        #endregion

        /// <summary>
        /// Setup particle sprites and position
        /// </summary>
        /// <param Name="fromEvent">Event managing the particle</param>
        /// <param Name="fromViewport">Viewport displaying particle</param>
        /// <param Name="folder">Folder of the particle graphic</param>
        /// <param Name="filename">filename of particle texture</param>
        protected void Setup(GameEvent fromEvent, Viewport fromViewport, string folder, string filename)
        {
            Ev = fromEvent;
            particleName = filename;
            particles = new LocalSprite[maxParticles];
            opacity = new int[maxParticles];
            startingX = Ev.ScreenX + xOffset;
            startingY = Ev.ScreenY + yOffset;
            screenX = Ev.ScreenX;
            screenY = Ev.ScreenY;
            // Create particle Sprites
            for (int i = 0; i < maxParticles; i++)
            {
                particles[i] = new LocalSprite(fromViewport);
                particles[i].Bitmap = Cache.LoadBitmap(folder, particleName, hue);
                particles[i].BlendType = blendValue;
                particles[i].Y = startingY;
                particles[i].X = startingX;
                particles[i].Z = Ev.ScreenZ();
                opacity[i] = 250;
            }
        }

        /// <summary>
        /// Particle Frame Update
        /// </summary>
        public virtual void Update()
        {
            startingX = Ev.ScreenX + xOffset + (spreadingOverX == 0 ? 0 : InGame.Rnd.Next(spreadingOverX) - spreadingOverX / 2);
            startingY = Ev.ScreenY + yOffset;
            int offsetx = Ev.ScreenX - screenX;
            screenX = Ev.ScreenX;
            int offsety = Ev.ScreenY - screenY;
            screenY = Ev.ScreenY;
            for (int i = 0; i < maxParticles; i++)
            {
                //particles[i].sourceRect=new Rectangle()
                if (particles[i].Y <= 0)
                {
                    particles[i].Y = startingY + yOffset;
                    particles[i].X = startingX + xOffset;
                }
                if (particles[i].X <= 0)
                {
                    particles[i].Y = startingY + yOffset;
                    particles[i].X = startingX + xOffset;
                }
                if (particles[i].Y >= GeexEdit.GameWindowHeight)
                {
                    particles[i].Y = startingY + yOffset;
                    particles[i].X = startingX + xOffset;
                }
                if (particles[i].X >= GeexEdit.GameWindowWidth)
                {
                    particles[i].Y = startingY + yOffset;
                    particles[i].X = startingX + xOffset;
                }
                // Fading away
                if (isFading)
                {
                    if (opacity[i] <= 0)
                    {
                        opacity[i] = originalOpacity;
                        particles[i].Y = startingY + yOffset;
                        particles[i].X = startingX + xOffset;
                    }
                }
                else
                {
                    if (opacity[i] <= 0)
                    {
                        opacity[i] = 250;
                        particles[i].Y = startingY + yOffset;
                        particles[i].X = startingX + xOffset;
                    }
                }
                // Random hue rotation
                if (isRandomHue)
                {
                    if (hue >= 360) hue = 0;
                    if (Graphics.FrameCount % 2 == 0)
                    {
                        hue += 1;
                        particles[i].Bitmap = Cache.LoadBitmap(particleFolder, particleName, hue);
                    }
                }
                int minus = InGame.Rnd.Next(fadingOpacity);
                if (opacity[i] < minus) opacity[i] = 0;
                else opacity[i] -= minus;
                // Random left right movement
                if (isLeftRight)
                {
                    if (InGame.Rnd.Next(2) == 1)
                    {
                        particles[i].X = (int)(particles[i].X - xGravity + offsetx);
                    }
                    else
                    {
                        particles[i].X = (int)(particles[i].X + xGravity + offsetx);
                    }
                }
                particles[i].Y = (int)(particles[i].Y - yGravity + offsety);
                particles[i].Opacity = (byte)opacity[i];
                particles[i].Z = Ev.ScreenY + Ev.ScreenZ(32) - particles[i].Y;
            }
        }

        /// <summary>
        /// Dispose all particles
        /// </summary>
        public void Dispose()
        {
            foreach (LocalSprite sprite in particles) sprite.Dispose();
        }
    }

    /// <summary>
    /// Create an Aura particle Effect from a Character
    /// </summary>
    public class Aura : SpriteParticle
    {
        /// <summary>
        /// Create an Aura particle Effect from a Character
        /// </summary>
        /// <param Name="fromEvent">Game Event that manages the particle</param>
        /// <param Name="fromViewport">Viewport of sprite particle</param>
        public Aura(GameEvent fromEvent, Viewport fromViewport)
        {
            // Setup particle variables
            maxParticles = 10;
            isFading = true;
            isLeftRight = true;
            isRandomHue = false;
            blendValue = 1;
            originalOpacity = 250;
            fadingOpacity = 30;
            xOffset = 0;
            spreadingOverX = 0;
            yOffset = 0;
            hue = 0;
            // Graphics divided by slowdown
            xGravity = 0.5f / 0.8f;
            yGravity = 0.8f / 0.8f;
            // Create particles
            Setup(fromEvent, fromViewport, "Graphics/Characters/", fromEvent.CharacterName);
        }
        /// <summary>
        /// Setup particle sprites and position
        /// </summary>
        /// <param Name="fromEvent">Event managing the particle</param>
        /// <param Name="fromViewport">Viewport displaying particle</param>
        /// <param Name="folder">Folder of the particle graphic</param>
        /// <param Name="filename">filename of particle texture</param>
        protected new void Setup(GameEvent fromEvent, Viewport fromViewport, string folder, string filename)
        {
            particleFolder = folder;
            Ev = fromEvent;
            particles = new LocalSprite[maxParticles];
            opacity = new int[maxParticles];
            startingX = Ev.ScreenX + xOffset;
            startingY = Ev.ScreenY + yOffset;
            screenX = Ev.ScreenX;
            screenY = Ev.ScreenY;
            // Create particle Sprites
            for (int i = 0; i < maxParticles; i++)
            {
                particles[i] = new LocalSprite(fromViewport);
                particles[i].Bitmap = Cache.LoadBitmap(folder, filename, hue);
                int cw = particles[i].Bitmap.Width / GameOptions.CharacterPatterns;
                int ch = particles[i].Bitmap.Height / GameOptions.CharacterDirections;
                particles[i].Oy = GameOptions.IsArpgCharacterOn && particles[i].Bitmap.Height == particles[i].Bitmap.Width ? 2 * ch / 3 : ch;
                particles[i].Ox = cw / 2;
                particles[i].SourceRect = new Rectangle(Ev.Pattern * cw, (Ev.Dir - 2) / 2 * ch, cw, ch);
                particles[i].BlendType = blendValue;
                particles[i].Y = startingY;
                particles[i].X = startingX;
                particles[i].Z = Ev.ScreenZ() - 1;
                opacity[i] = Ev.Opacity;
            }
        }

        /// <summary>
        /// Particle Frame Update
        /// </summary>
        public override void Update()
        {
            originalOpacity = Ev.Opacity;
            // Update particle Sprites
            for (int i = 0; i < maxParticles; i++)
            {
                int cw = particles[i].Bitmap.Width / GameOptions.CharacterPatterns;
                int ch = particles[i].Bitmap.Height / GameOptions.CharacterDirections;
                particles[i].Oy = GameOptions.IsArpgCharacterOn && particles[i].Bitmap.Height == particles[i].Bitmap.Width ? 2 * ch / 3 : ch;
                particles[i].Ox = cw / 2;
                particles[i].SourceRect = new Rectangle(Ev.Pattern * cw, (Ev.Dir - 2) / 2 * ch, cw, ch);
                particles[i].BlendType = blendValue;
            }
            base.Update();
            // Update particle z
            for (int i = 0; i < maxParticles; i++)
            {
                particles[i].Z = Ev.ScreenZ() - 1;
            }
        }
    }

    /// <summary>
    /// Create a Flame Effect particle
    /// </summary>
    public class Flame : SpriteParticle
    {
        /// <summary>
        /// Create a Flame particle
        /// </summary>
        /// <param Name="fromEvent">Game Event that manages the particle</param>
        /// <param Name="fromViewport">Viewport of sprite particle</param>
        public Flame(GameEvent fromEvent, Viewport fromViewport)
        {
            // Setup particle variables
            maxParticles = 20;
            isFading = true;
            isLeftRight = true;
            isRandomHue = false;
            blendValue = 1;
            originalOpacity = 250;
            fadingOpacity = 30;
            xOffset = -5;
            spreadingOverX = 0;
            yOffset = -13;
            hue = 0;
            // Graphics divided by slowdown
            xGravity = 0.5f / 0.5f;
            yGravity = 0.10f / 0.5f;
            // Create particles
            Setup(fromEvent, fromViewport, "Graphics/Particles/", "Flame");
        }
    }

    /// <summary>
    /// Create a Smoke Effect particle
    /// </summary>
    public class Smoke : SpriteParticle
    {
        /// Create a Smoke Effect particle
        /// </summary>
        /// <param Name="fromEvent">Game Event that manages the particle</param>
        /// <param Name="fromViewport">Viewport of sprite particle</param>
        public Smoke(GameEvent fromEvent, Viewport fromViewport)
        {
            // Setup particle variables
            maxParticles = 12;
            isFading = true;
            isLeftRight = true;
            isRandomHue = false;
            blendValue = 1;
            originalOpacity = 80;
            fadingOpacity = 5;
            xOffset = -5;
            spreadingOverX = 0;
            yOffset = -13;
            hue = 40;
            xGravity = 0.5f / 0.5f;
            yGravity = 0.1f / 0.5f;
            // Create particles
            Setup(fromEvent, fromViewport, "Graphics/Particles/", "Smoke");
        }

    }

    /// <summary>
    /// Create a Teleport particle
    /// </summary>
    public class Teleport : SpriteParticle
    {
        /// Create a Smoke Effect particle
        /// </summary>
        /// <param Name="fromEvent">Game Event that manages the particle</param>
        /// <param Name="fromViewport">Viewport of sprite particle</param>
        public Teleport(GameEvent fromEvent, Viewport fromViewport)
        {
            // Setup particle variables
            maxParticles = 20;
            isFading = true;
            isLeftRight = true;
            isRandomHue = false;
            blendValue = fromEvent.BlendType;
            originalOpacity = 250;
            fadingOpacity = 20;
            xOffset = 0;
            spreadingOverX = fromEvent.CollisionWidth;
            yOffset = -13;
            hue = 0;
            xGravity = 0.2f / 0.2f;
            yGravity = 0.10f / 0.2f;
            // Create particles
            Setup(fromEvent, fromViewport, "Graphics/Particles/", "teleport");
        }

    }

    /// <summary>
    /// Create a Spirit particle
    /// </summary>
    public class Spirit : SpriteParticle
    {
        /// Create a Spirit Effect particle
        /// </summary>
        /// <param Name="fromEvent">Game Event that manages the particle</param>
        /// <param Name="fromViewport">Viewport of sprite particle</param>
        public Spirit(GameEvent fromEvent, Viewport fromViewport)
        {
            // Setup particle variables
            maxParticles = 12;
            isFading = true;
            isLeftRight = true;
            isRandomHue = true;
            blendValue = fromEvent.BlendType;
            originalOpacity = 250;
            fadingOpacity = 30;
            xOffset = -5;
            spreadingOverX = 0;
            yOffset = -13;
            hue = 40;
            xGravity = 0.5f / 0.5f;
            yGravity = 0.10f / 0.5f;
            // Create particles
            Setup(fromEvent, fromViewport, "Graphics/Particles/", "Particle");
        }

    }

    /// <summary>
    /// Create a Particle based on Event localName as the source sprite
    /// </summary>
    public class EventBase : SpriteParticle
    {
        /// <summary>
        /// Create a Particle based on Event localName as the source sprite
        /// </summary>
        /// <param Name="fromEvent">Game Event that manages the particle</param>
        /// <param Name="fromViewport">Viewport of sprite particle</param>
        public EventBase(GameEvent fromEvent, Viewport fromViewport)
        {
            // Setup particle variables
            maxParticles = 20;
            isFading = true;
            isLeftRight = true;
            isRandomHue = false;
            blendValue = fromEvent.BlendType;
            originalOpacity = 250;
            fadingOpacity = 20;
            xOffset = 0;
            spreadingOverX = fromEvent.CollisionWidth;
            yOffset = -13;
            hue = 0;
            xGravity = 0.2f / 0.2f;
            yGravity = 0.10f / 0.2f;
            // Create particles
            Setup(fromEvent, fromViewport, "Graphics/Particles/", fromEvent.EventName);
        }
        /// <summary>
        /// Setup particle sprites and position
        /// </summary>
        /// <param Name="fromEvent">Event managing the particle</param>
        /// <param Name="fromViewport">Viewport displaying particle</param>
        /// <param Name="folder">Folder of the particle graphic</param>
        /// <param Name="filename">filename of particle texture</param>
        protected void Setup(GameEvent fromEvent, Viewport fromViewport, string folder, string filename)
        {
            Ev = fromEvent;
            particleName = filename;
            particles = new LocalSprite[maxParticles];
            opacity = new int[maxParticles];
            startingX = Ev.ScreenX + xOffset;
            startingY = Ev.ScreenY + yOffset;
            screenX = Ev.ScreenX;
            screenY = Ev.ScreenY;
            // Create particle Sprites
            for (int i = 0; i < maxParticles; i++)
            {
                particles[i] = new LocalSprite(fromViewport);
                particles[i].Bitmap = Cache.LoadBitmap(folder, particleName, hue);
                particles[i].BlendType = blendValue;
                particles[i].Y = startingY;
                particles[i].X = startingX;
                particles[i].Z = Ev.ScreenZ();
                opacity[i] = 250;
            }
        }

    }

    /// <summary>
    /// Create an Soot particle Effect from a Character
    /// </summary>
    public class Soot : SpriteParticle
    {
        /// <summary>
        /// Create an Aura particle Effect from a Character
        /// </summary>
        /// <param Name="fromEvent">Game Event that manages the particle</param>
        /// <param Name="fromViewport">Viewport of sprite particle</param>
        public Soot(GameEvent fromEvent, Viewport fromViewport)
        {
            // Setup particle variables
            maxParticles = 20;
            isFading = true;
            isLeftRight = true;
            isRandomHue = false;
            blendValue = 0;
            originalOpacity = 250;
            fadingOpacity = 20;
            xOffset = -5;
            spreadingOverX = 0;
            yOffset = -8;
            hue = 0;
            // Graphics divided by slowdown
            xGravity = 0.5f / 0.5f;
            yGravity = 0.1f / 0.5f;
            // Create particles
            Setup(fromEvent, fromViewport, "Graphics/Particles/", "black");
        }
    }
}