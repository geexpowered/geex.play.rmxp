using System;
using System.Collections.Generic;
using Geex.Edit;
using Geex.Play.Make;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class handles the map. It includes scrolling and passable determining functions
    /// </summary>
    public partial class GameMap
    {
        #region Variables
        /// <summary>
        /// localName of current Chipset
        /// </summary>
        public string ChipsetName;
        /// <summary>
        /// Tileset ID in data_tileset array
        /// </summary>
        int tilesetID = 1;

        /// <summary>
        /// Map passages table
        /// </summary>
        byte[] passages;

        /// <summary>
        /// Map priority table
        /// </summary>
        byte[] priorities;

        /// <summary>
        /// Map terrain_tags table
        /// </summary>
        byte[] terrainTags;

        /// <summary>
        /// Get Map ID
        /// </summary>
        public int MapId = 0;

        /// <summary>
        /// Get Map Width in tiles
        /// </summary>
        public short Width = GeexEdit.GameMapWidth;

        /// <summary>
        /// Get Map Height in tiles
        /// </summary>
        public short Height = GeexEdit.GameMapHeight;

        /// <summary>
        /// Get Encounter List
        /// </summary>
        public short[] EncounterList;

        /// <summary>
        /// Get Encounter Steps
        /// </summary>
        public short EncounterStep;

        /// <summary>
        /// Change direction (0,2,4,6,8,10) to obstacle bit (0,1,2,4,8,0)
        /// </summary>
        byte[] ObstableBit = new Byte[11] { 0, 0, 1, 0, 2, 0, 4, 0, 8, 0, 0 };

        /// <summary>
        /// Panorama Pixel Shader effect
        /// </summary>
        public GeexEffect PanoramaGeexEffect = new GeexEffect();

        /// <summary>
        /// Current Map Panorama Name
        /// </summary>
        public string PanoramaName = string.Empty;

        /// <summary>
        /// Current Map Panorama hue
        /// </summary>
        public int PanoramaHue = 0;

        /// <summary>
        /// Current Map Fog Name
        /// </summary>
        public string FogName = string.Empty;

        /// <summary>
        /// Current Map Fog hue
        /// </summary>
        public int FogHue = 0;

        /// <summary>
        /// Current Map Fog blend type 0:normal 1:additive 2:Substract
        /// </summary>
        public short FogBlendType = 0;

        /// <summary>
        /// Current Map Fog zoom 1.0 = 100%
        /// </summary>
        public float FogZoom = 1f;

        /// <summary>
        /// Current Map opacity
        /// </summary>
        public byte FogOpacity = 255;

        /// <summary>
        /// Current Map Fog X-scroll
        /// </summary>
        public int FogSx = 0;

        /// <summary>
        /// urrent Map Fog Y-scroll
        /// </summary>
        public int FogSy = 0;

        /// <summary>
        /// Current Map Battleback Name
        /// </summary>
        public string BattlebackName = string.Empty;

        /// <summary>
        /// Display x-coordinate
        /// </summary>
        public int DisplayX = 0;

        /// <summary>
        /// Display y-coordinate
        /// </summary>
        public int DisplayY = 0;

        /// <summary>
        /// Refresh request flag
        /// </summary>
        public bool IsNeedRefresh;

        /// <summary>
        /// Current Map events
        /// </summary>
        public GameEvent[] Events;

        /// <summary>
        /// Store all the events that needs to be Update per frame. Event out of screen are not taken into account unless antilag flag is not set
        /// </summary>
        public List<short> EventKeysToUpdate;

        /// <summary>
        /// Current Map common events
        /// </summary>
        GameCommonEvent[] commonEvents;

        /// <summary>
        /// Fog Geex Effect
        /// </summary>
        public GeexEffect FogGeexEffect = new GeexEffect();

        /// <summary>
        /// Fog x-coordinate starting point
        /// </summary>
        public float FogOx;

        /// <summary>
        /// Fog y-coordinate starting point
        /// </summary>
        public float FogOy;

        /// <summary>
        /// Current Fog tone
        /// </summary>
        Tone fogTone;

        /// <summary>
        /// Target Fog tone
        /// </summary>
        Tone fogToneTarget;

        /// <summary>
        /// Fog tone change duration
        /// </summary>
        int fogToneDuration;

        /// <summary>
        /// Fog opacity change duration
        /// </summary>
        int fogOpacityDuration;

        /// <summary>
        /// Target Fog opacity
        /// </summary>
        byte fogOpacityTarget;

        /// <summary>
        /// Scroll direction
        /// </summary>
        short scrollDirection;

        /// <summary>
        /// Scroll rest
        /// </summary>
        int scrollRest;

        /// <summary>
        /// Real scroll rest
        /// </summary>
        double realScrollRest;

        /// <summary>
        /// Scroll speed
        /// </summary>
        short scrollSpeed;
        #region Audio
        /// <summary>
        /// The map BGM
        /// </summary>
        AudioFile Song;
        /// <summary>
        /// The Map BGS
        /// </summary>
        AudioFile BackgroundSound;
        /// <summary>
        /// True if BGGM must be played
        /// </summary>
        bool isAutoSong;
        /// <summary>
        /// True if BGS must be played
        /// </summary>
        bool isAutoBackgroundSound;
        #endregion

        /// <summary>
        /// List of game Particles
        /// </summary>
        public List<GameParticle> Particles = new List<GameParticle>();
        #endregion

        #region Properties
        /// <summary>
        /// Determine if Scrolling
        /// </summary>
        public bool IsScrolling
        {
            get
            {
                return scrollRest > 0;
            }
        }

        public byte[] Passages
        {
            get { return passages; }
        }

        #endregion

        #region Initialize
        /// <summary>
        /// Create a new game map instance
        /// </summary>
        public GameMap()
        {
        }

        #region Initialize - SetUp
        /// <summary>Map Setup</summary>
        /// <param Name="id">map ID</param>
        public void Setup(int id)
        {
            SetupParticles();
            SetupMapId(id);
            SetupLoad();
            SetupDisplay();
            SetupRefresh();
            SetupCommonEvents();
            SetupFog();
            SetupScroll();
        }

        /// <summary>
        /// Setup Map ID
        /// </summary>
        /// <param Name="map_id">map ID</param>
        public void SetupMapId(int id)
        {
            MapId = id;
        }

        /// <summary>Load Map Data</summary>
        /// <param Name="map_id">map ID</param>
        void SetupLoad()
        {
            // Load map from file and set map
            Map map = Cache.MapData("map" + MapId.ToString());
            // Setup Map
            TileManager.MapData = map.Data;
            Width = map.Width;
            Height = map.Height;
            EncounterList = map.EncounterList;
            EncounterStep = map.EncounterStep;
            tilesetID = map.TilesetId;
            isAutoSong = map.AutoplayMusicLoop;
            isAutoBackgroundSound = map.AutoplaySoundLoop;
            Song = map.MusicLoop;
            BackgroundSound = map.SoundLoop;
            // Select Fogdata from Map instead of Tileset
            if (map.FogName != string.Empty)
            {
                FogName = map.FogName;
                FogHue = map.FogHue;
                FogOpacity = map.FogOpacity;
                FogBlendType = map.FogBlendType;
                FogZoom = map.FogZoom / 100f;
                FogSx = map.FogSx;
                FogSy = map.FogSy;
            }
            // Select Panorama data from Map instead of Tileset
            if (map.PanoramaName != string.Empty)
            {
                PanoramaName = map.PanoramaName;
                PanoramaHue = map.PanoramaHue;
            }
            // Setup TileManager
            SetupTileset();
            SetupEvents(map.Events);
        }

        /// <summary>
        /// Setup Tileset
        /// </summary>
        void SetupTileset()
        {
            // set Tile set information in opening instance variables
            Tileset tileset = Cache.TilesetData("tileset" + tilesetID);
            ChipsetName = tileset.TilesetName;
            PanoramaName = tileset.PanoramaName;
            PanoramaHue = tileset.PanoramaHue;
            // mandatory for pixel collision - Mask must be present in Masks folders
            TileManager.AutotileAnimations = tileset.AutotileAnimations;
            passages = new byte[tileset.Passages.Length];
            priorities = new byte[tileset.Priorities.Length];
            terrainTags = new byte[tileset.TerrainTags.Length];
            TileManager.Priorities = new byte[tileset.Passages.Length];
            for (int i = 0; i < tileset.Passages.Length; i++)
            {
                passages[i] = (byte)tileset.Passages[i];
                priorities[i] = (byte)tileset.Priorities[i];
                terrainTags[i] = (byte)tileset.TerrainTags[i];
                TileManager.Priorities[i] = (byte)tileset.Priorities[i];
            }
            // Set Add blending tiles
            for (int i = 0; i < passages.Length; i++)
                TileManager.IsAddBlend[i] = passages[i] == 0x40;
            // Select fog/panorama data from Tileset by default
            // Select Fogdata from Tileset by default
            if (FogName != string.Empty)
            {
                FogName = tileset.FogName;
                FogHue = tileset.FogHue;
                FogOpacity = tileset.FogOpacity;
                FogBlendType = tileset.FogBlendType;
                FogZoom = tileset.FogZoom / 100f;
                FogSx = tileset.FogSx;
                FogSy = tileset.FogSy;
            }
            // Select Panorama data from Tileset by default
            if (PanoramaName != string.Empty)
            {
                PanoramaName = tileset.PanoramaName;
                PanoramaHue = tileset.PanoramaHue;
            }
            BattlebackName = tileset.BattlebackName;
        }

        /// <summary>
        /// Setup Display
        /// </summary>
        void SetupDisplay()
        {
            // Initialize displayed coordinates
            DisplayX = 0;
            DisplayY = 0;
        }

        /// <summary>
        /// Setup Refresh
        /// </summary>
        void SetupRefresh()
        {
            // Clear refresh request flag
            IsNeedRefresh = false;
        }

        /// <summary>
        /// Setup Map Events
        /// </summary>
        void SetupEvents(Event[] mapEvents)
        {
            // Set map event
            Events = new GameEvent[mapEvents.Length];
            for (int i = 0; i < mapEvents.Length; i++)
            {
                if (!mapEvents[i].IsEmpty)
                {
                    Events[i] = new GameEvent(mapEvents[i]);
                    Events[i].CheckEventOptions();
                }
                else
                {
                    Events[i] = null;
                }
            }
        }

        /// <summary>
        /// Setup common event TileManager.MapData
        /// </summary>
        void SetupCommonEvents()
        {
            // Set common event TileManager.MapData
            commonEvents = new GameCommonEvent[Data.CommonEvents.Length];
            for (int i = 1; i < Data.CommonEvents.Length; i++)
            {
                commonEvents[i] = new GameCommonEvent(Data.CommonEvents[i].Id);
            }
        }

        /// <summary>Setup Fog TileManager.MapData</summary>
        void SetupFog()
        {
            // Initialize all Fog information
            FogOx = 0;
            FogOy = 0;
            fogTone = new Tone(0, 0, 0, 0);
            fogToneTarget = new Tone(0, 0, 0, 0);
            fogToneDuration = 0;
            fogOpacityDuration = 0;
            fogOpacityTarget = 0;
        }

        /// <summary>
        /// Setup scroll datas
        /// </summary>
        void SetupScroll()
        {
            // Initialize scroll information
            scrollDirection = 2;
            scrollRest = 0;
            realScrollRest = 0;
            scrollSpeed = 4;
        }

        /// <summary>
        /// Setup the Game Player Size
        /// </summary>
        void SetupEventsSize()
        {
            InGame.Player.CollisionWidth = GameOptions.GamePlayerWidth;
            InGame.Player.CollisionHeight = GameOptions.GamePlayerHeight;
        }

        /// <summary>
        /// Setup particles data
        /// </summary>
        void SetupParticles()
        {
            Particles = new List<GameParticle>();
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            // If map ID is effective
            if (MapId > 0)
            {
                foreach (GameEvent ev in Events)
                {
                    if (ev == null) continue;
                    ev.Refresh();
                }
                // Refresh all common events
                for (int i = 1; i < commonEvents.Length; i++)
                {
                    commonEvents[i].Refresh();
                }
            }
            // Clear refresh request flag
            IsNeedRefresh = false;
        }

        /// <summary>
        /// Automatically Change Background Music and Backround Sound
        /// </summary>
        public void Autoplay()
        {
            if (isAutoSong)
            {
                InGame.System.SongPlay(Song);
            }
            if (isAutoBackgroundSound)
            {
                InGame.System.BackgroundSoundPlay(BackgroundSound);
            }
        }

        #region Map Scrolling
        /// <summary>
        /// Scroll Down
        /// </summary>
        /// <param Name="distance">scroll distance</param>
        public void ScrollDown(int distance)
        {
            DisplayY = Math.Min(DisplayY + distance, (Math.Max(0, Height - GeexEdit.GameMapHeight)) * 32);
        }

        /// <summary>
        /// Scroll Left
        /// t</summary>
        /// <param Name="distance">scroll distance</param>
        public void ScrollLeft(int distance)
        {
            DisplayX = Math.Max(DisplayX - distance, 0);
        }

        /// <summary>
        /// Scroll Right
        /// </summary>
        /// <param Name="distance">scroll distance</param>
        public void ScrollRight(int distance)
        {
            DisplayX = Math.Min(DisplayX + distance, (Math.Max(0, Width - GeexEdit.GameMapWidth)) * 32);
        }

        /// <summary>
        /// Scroll Up
        /// </summary>
        /// <param Name="distance">scroll distance</param>
        public void ScrollUp(int distance)
        {
            DisplayY = Math.Max(DisplayY - distance, 0);
        }
        #endregion

        #region Passability
        /// <summary>
        /// True is coordinate are within Map Size
        /// </summary>
        /// <param Name="x">X coordinate</param>
        /// <param Name="y">Y coordinate</param>
        public bool IsValid(int x, int y)
        {
            return (x >= 0 & x < Width * 32 & y >= 0 & y < Height * 32);
        }

        /// <summary>
        /// True  if Map is Passable. Game Option isCollisionMaskOn determines is pixel collision in on, if not Tile passability applies
        /// </summary>
        /// <param Name="x">X coordinate</param>
        /// <param Name="y">Y coordinate</param>
        /// <param Name="d">direction (0,2,4,6,8,10)</param>
        /// <param Name="self_event">Self (If event is determined passable)</param>
        public bool IsPassable(int x, int y, int d, GameCharacter self_event, bool automove)
        {
            // Event passability
            byte bit = ObstableBit[d];
            foreach(GameEvent e in Events)
            {
                if (e != null)
                {
                    if (e.TileId >= 0 && e != self_event &&
                        e.X / 32 == x / 32 && (e.Y - 1) / 32 == (y - 1) / 32 && !e.Through)
                    {
                        if ((passages[e.TileId] & bit) != 0)
                            return false;
                        else if ((passages[e.TileId] & 0x0f) == 0x0f)
                            return false;
                        else if (priorities[e.TileId] == 0)
                            return true;
                    }
                }
            }   

            // If coordinates given are outside of the map
            if (!IsValid(x, y - 1)) return false;
            if (GameOptions.IsCollisionMaskOn)
            {
                return IsPixelPassable(x, y, d, self_event);
            }
            else
            {
                if (GameOptions.IsTileByTileMoving)
                {
                    // Move for automatic character move
                    if (automove)
                    {
                        return IsTilePassable(x + 15, y + 20, d, self_event);
                    }
                    // Player and other moves
                    else
                    {
                        // If unable to enter move Tile in designated direction
                        return IsTilePassable(x, y, d, self_event);
                    }
                }
                else
                {
                    // If unable to enter move Tile in designated direction
                    return IsTilePassable(x, y + 20, d, self_event);
                }
            }
        }

        public bool IsPassable(int x, int y, int d, GameCharacter self_event)
        {
            return IsPassable(x, y, d, self_event, false);
        }

        /// <summary>
        /// True if Map is pixel Passable
        /// </summary>
        /// <param Name="x">X coordinate</param>
        /// <param Name="y">Y coordinate</param>
        /// <param Name="d">direction (0,2,4,6,8,10)</param>
        /// <param Name="self_event">Self (If event is determined passable)</param>
        bool IsPixelPassable(int x, int y, int d, GameCharacter self_event)
        {
            // Calculate position rectange
            return TileManager.MapCollision(new Rectangle(x - self_event.CollisionWidth / 2, y - self_event.CollisionHeight, self_event.CollisionWidth, self_event.CollisionHeight));
        }

        /// <summary>
        /// True if Tiles are passable
        /// </summary>
        /// <param Name="x">X coordinate</param>
        /// <param Name="y">Y coordinate</param>
        /// <param Name="d">direction (0,2,4,6,8,10)</param>
        /// <param Name="self_event">Self (If event is determined passable)</param>
        bool IsTilePassable(int x, int y, int d, GameCharacter self_event)
        {
            // Result
            bool isTilePassable = false;

            // Collision width
            int collisionWidth = 0;
            if(self_event != null) {
                collisionWidth = self_event.CollisionWidth;
            }
            // Direction and start/end tile
            int xStart = 0; int xEnd = 0; int yStart = 0; int yEnd = 0;
            switch (d)
            {
                // Neutral
                case 0:
                    xStart = Math.Max(0, (x - collisionWidth / 2) / 32);
                    xEnd = xStart;
                    yStart = Math.Max(0, (y - 31) / 32);
                    yEnd = yStart;
                    break;
                // Jump
                case 10:
                    xEnd = Math.Max(0, (x - collisionWidth / 2) / 32);
                    yEnd = Math.Max(0, (y - 31) / 32);
                    break;
                // Down
                case 2:
                    xStart = Math.Max(0, (x - collisionWidth / 2) / 32);
                    xEnd = xStart;
                    yStart = Math.Max(0, (y - 53) / 32);
                    yEnd = Math.Min(yStart + 1, Height-1);
                    break;
                // Left
                case 4:
                    xStart = Math.Max(0, (x - collisionWidth / 2) / 32) + 1;
                    xEnd = Math.Max(xStart - 1, 0);
                    yStart = Math.Max(0, (y - 31) / 32);
                    yEnd = yStart;
                    break;
                // Right
                case 6:
                    xStart = Math.Max(0, (x + collisionWidth / 2) / 32 - 1);
                    xEnd = Math.Min(xStart + 1, Width-1);
                    yStart = Math.Max(0, (y - 31) / 32);
                    yEnd = yStart;
                    break;
                // Up
                case 8:
                    xStart = Math.Max(0, (x - collisionWidth / 2) / 32);
                    xEnd = xStart;
                    yStart = Math.Max(0, (y + 1) / 32);
                    yEnd = Math.Max(0, yStart - 1);
                    break;
            }

            // Jump passability
            if (d == 10)
            {
                // Test are done for the end of the move
                int w = xEnd;
                int h = yEnd;

                // Plain: can enter this tile for it is allowed
                // Passability by layer
                bool[] plainPassability = { true, true, true };
                // Check if tiles are passable
                // Loop searches in order from top of layer
                for (int i = 2; i >= 0; i--)
                {
                    int tileId = TileManager.MapData[w][h][i];
                    // If obstacle bit is set or  If obstacle bit is set in all directions
                    if (passages[tileId] == 15)
                        plainPassability[i] = false;
                }
                // Return passability in function of tile supersition;
                isTilePassable = testLayerPassability(plainPassability, w, h);
            }
            // Move passability
            else
            {
                // Test are done on the start tile of the move
                int w = xStart;
                int h = yStart;
                // Change direction (0,2,4,6,8,10) to obstacle bit (0,1,2,4,8,0)
                byte bit = ObstableBit[d];

                if (GameOptions.IsTileByTileMoving)
                {
                    // Local passability
                    bool[] localPassability = { true, true, true };
                    // Loop searches in order from top of layer
                    for (int i = 2; i >= 0; i--)
                    {
                        int tileId = TileManager.MapData[w][h][i];
                        // If obstacle bit is set
                        if ((passages[tileId] & bit) != 0)
                            localPassability[i] = false;
                    }

                    // Check if test is passed
                    isTilePassable = testLayerPassability(localPassability, w, h);
                }
                else
                {
                    isTilePassable = true;
                }


                // If tile can be quit, check if future tile can be entered
                if (isTilePassable)
                {
                    // Test are done for the end of the move
                    w = xEnd;
                    h = yEnd;

                    // Plain: can enter this tile for it is allowed
                    // Passability by layer
                    bool[] plainPassability = { true, true, true };
                    // Check if tiles are passable
                    // Loop searches in order from top of layer
                    for (int i = 2; i >= 0; i--)
                    {
                        int tileId = TileManager.MapData[w][h][i];
                        // If obstacle bit is set or  If obstacle bit is set in all directions
                        if (passages[tileId] == 15)
                            plainPassability[i] = false;
                    }
                    // Return passability in function of tile supersition;
                    isTilePassable = testLayerPassability(plainPassability, w, h);

                    // Front : if it is allowed, can enter this tile by this direction
                    if (isTilePassable)
                    {
                        bool[] frontPassability = { true, true, true };
                        // Opposite direction to obstacle bit
                        bit = ObstableBit[10 - d];
                        // Loop searches in order from top of layer
                        for (int i = 2; i >= 0; i--)
                        {
                            int tileId = TileManager.MapData[w][h][i];
                            // If obstacle bit is set
                            if ((passages[tileId] & bit) != 0)
                                frontPassability[i] = false;
                        }

                        // Check if test is passed
                        isTilePassable = testLayerPassability(frontPassability, w, h);
                    }
                }
            }

            // Passable
            return isTilePassable;
        }

        /// <summary>
        /// Test passability in function of layer superposition
        /// </summary>
        /// <param name="layerPassability"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private bool testLayerPassability(bool[] layerPassability, int w, int h)
        {
            bool passable = false;
            // Get tile id
            int tileIdLayer0 = TileManager.MapData[w][h][0];
            int tileIdLayer1 = TileManager.MapData[w][h][1];
            int tileIdLayer2 = TileManager.MapData[w][h][2];
            // All layer passable
            if (layerPassability[0] && layerPassability[1] && layerPassability[2])
                passable = true;
            // Layer 0 not passable but layer 1 is passable 
            else if (!layerPassability[0]
                && (layerPassability[1] && priorities[tileIdLayer1] == 0) && !(tileIdLayer1 == 0))
            {
                // If layer 2 passable
                if((layerPassability[2] && priorities[tileIdLayer2] == 0)
                        || (layerPassability[2] && priorities[tileIdLayer2] > 0 && tileIdLayer2 != 0)
                        || tileIdLayer2 == 0)
                    passable = true;
                // Else, not passable
            }
            // Layer 0 or 1 not passable but layer 2 passable 
            else if ((!layerPassability[0] || !layerPassability[1])
                && (layerPassability[2] && priorities[tileIdLayer2] == 0) && tileIdLayer2 != 0)
                passable = true;
            // Result
            return passable;
        }

        #endregion
        /// <summary>
        /// Determine Thicket
        /// </summary>
        /// <param Name="x">x-coordinate</param>
        /// <param Name="y">y-coordinate</param>
        /// <returns></returns>
        public bool IsBush(int x, int y)
        {
            if (!IsValid(x, y)) return false;
            if (MapId != 0)
            {
                for (int i = 2; i >= 0; i--)
                {
                    int _tile_id = TileManager.MapData[x / 32][y / 32][i];
                    if (passages[(int)_tile_id] == 0x40)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determine Counter
        /// </summary>
        /// <param Name="x">x-coordinate</param>
        /// <param Name="y">y-coordinate</param>
        /// <returns></returns>
        public bool IsCounter(int x, int y)
        {
            if (!IsValid(x, y)) return false;
            if (MapId != 0)
            {
                for (int i = 2; i >= 0; i--)
                {
                    int? _tile_id = TileManager.MapData[x / 32][(y - 1) / 32][i]; // The counter check is one tile too low
                    if (_tile_id == null)
                    {
                        return false;
                    }
                    else if ((passages[(int)_tile_id] & 0x80) == 0x80)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Get Terrain Tag
        /// </summary>
        /// <param Name="x">x-coordinate</param>
        /// <param Name="y">y-coordinate</param>
        /// <returns></returns>
        public short TerrainTag(int x, int y)
        {
            if (!IsValid(x, y)) return 0;
            if (MapId != 0)
            {
                for (int i = 2; i >= 0; i--)
                {
                    int? _tile_id = TileManager.MapData[x / 32][y / 32][i];
                    if (_tile_id == null)
                    {
                        return 0;
                    }
                    else if (terrainTags[(int)_tile_id] > 0)
                    {
                        return terrainTags[(int)_tile_id];
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// True if Terrain Tag(x,y) is terrainTag
        /// </summary>
        /// <param Name="x">x-coordinate</param>
        /// <param Name="y">y-coordinate</param>
        /// <param Name="terrainTag">terrain tag to test</param>
        /// <returns></returns>
        public bool IsTerrainTag(int x, int y, short terrainTag)
        {
            if (!IsValid(x, y)) return false;
            if (MapId != 0)
            {
                for (int i = 2; i >= 0; i--)
                {
                    int? _tile_id = TileManager.MapData[x / 32][y / 32][i];
                    if (terrainTags[(int)_tile_id] == terrainTag)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// True if tileId terrain tag is terrainTag
        /// </summary>
        /// <param Name="tileId">tileId</param>
        /// <param Name="terrainTag">terrain tag to test</param>
        /// <returns></returns>
        public bool IsTerrainTagTile(int tileId, short terrainTag)
        {
            return terrainTags[tileId] == terrainTag;
        }
        /// <summary>
        /// Start Scroll
        /// </summary>
        /// <param Name="direction">scroll direction</param>
        /// <param Name="distance">scroll distance</param>
        /// <param Name="speed">scroll speed</param>
        public void StartScroll(short direction, int distance, short speed)
        {
            scrollDirection = direction;
            scrollRest = distance * 32;
            realScrollRest = 0;
            scrollSpeed = speed;
        }

        /// <summary>
        /// Start Changing Fog Color Tone
        /// </summary>
        /// <param Name="tone">Color tone</param>
        /// <param Name="duration">time</param>
        public void StartFogToneChange(Tone tone, int duration)
        {
            fogToneTarget = tone.Clone;
            fogToneDuration = duration;
            if (fogToneDuration == 0)
            {
                fogTone = fogToneTarget.Clone;
            }
        }

        /// <summary>
        /// Start Changing Fog Opacity Level
        /// </summary>
        /// <param Name="opacity">opacity level</param>
        /// <param Name="duration">time</param>
        public void StartFogOpacityChange(byte opacity, int duration)
        {
            fogOpacityTarget = opacity;
            fogOpacityDuration = duration;
            if (fogOpacityDuration == 0)
            {
                FogOpacity = fogOpacityTarget;
            }
        }

        #region Methods - Update

        /// <summary>
        /// Update Map
        /// </summary>
        public void Update()
        {
            UpdateRefresh();
            UpdateScrolling();
            UpdateEvents();
            UpdateCommonEvents();
            UpdateFogScroll();
            UpdateFogColour();
            UpdateFog();
        }

        /// <summary>
        /// Refresh Game Map
        /// </summary>
        void UpdateRefresh()
        {
            // Refresh map if necessary
            if (InGame.Map.IsNeedRefresh) Refresh();
        }

        /// <summary>
        /// Update Scrolling
        /// </summary>
        void UpdateScrolling()
        {
            // If scrolling
            if (scrollRest > 0)
            {
                int distance = 0;
                // Change from scroll speed to distance in map coordinates
                double realDistance = (double)InGame.System.Speed[scrollSpeed] / GameOptions.MapScrollSpeedDiviser;
                // Either calculate distance or move
                if (realScrollRest + realDistance >= 1)
                {
                    distance = (int)Math.Floor(realScrollRest + realDistance);
                    realScrollRest = Math.Max(0, realDistance + realScrollRest - 1);
                }
                else
                {
                    realScrollRest += realDistance;
                }               
                // Execute scrolling
                switch (scrollDirection)
                {
                    case 2: //down
                        ScrollDown(distance);
                        break;
                    case 4: //left
                        ScrollLeft(distance);
                        break;
                    case 6:
                        ScrollRight(distance);
                        break;
                    case 8:
                        ScrollUp(distance);
                        break;
                }
                // Subtract distance scrolled
                scrollRest -= distance;
            }
        }

        /// <summary>
        /// Update Events
        /// </summary>
        void UpdateEvents()
        {
            // Update map event. Optimisation, get only events on screen or with no antilag
            EventKeysToUpdate = new List<short>();
            for (short i = 0; i < Events.Length; i++)
            {
                if (Events[i] != null && !Events[i].IsEmpty && !Events[i].IsErased && (Events[i].IsOnScreen || !Events[i].IsAntilag))
                {
                    EventKeysToUpdate.Add(i);
                }
            }
            foreach (short i in EventKeysToUpdate)
            {

                Events[i].Update();
            }
        }

        /// <summary>
        /// Update Common Events
        /// </summary>
        void UpdateCommonEvents()
        {
            // Update common event
            for (int i = 1; i < commonEvents.Length; i++)
            {
                commonEvents[i].Update();
            }
        }

        /// <summary>
        /// Update Fog Scroll
        /// </summary>
        void UpdateFogScroll()
        {
            // Manage Fog scrolling
            FogOx -= FogSx / 8f;
            FogOy -= FogSy / 8f;
        }

        /// <summary>
        /// Update Fog Color
        /// </summary>
        void UpdateFogColour()
        {
            // Manage change in Fog Color tone
            if (fogToneDuration >= 1)
            {
                int d = fogToneDuration;
                Tone target = fogToneTarget;
                fogTone.Red = (fogTone.Red * (d - 1) + target.Red) / d;
                fogTone.Green = (fogTone.Green * (d - 1) + target.Green) / d;
                fogTone.Blue = (fogTone.Blue * (d - 1) + target.Blue) / d;
                fogTone.Gray = (fogTone.Gray * (d - 1) + target.Gray) / d;
                fogToneDuration -= 1;
            }
        }

        /// <summary>
        /// Update Fog
        /// </summary>
        void UpdateFog()
        {
            // Manage change in Fog opacity level
            if (fogOpacityDuration >= 1)
            {
                int d = fogOpacityDuration;
                FogOpacity = (byte)((FogOpacity * (d - 1) + fogOpacityTarget) / d);
                fogOpacityDuration -= 1;
            }
        }

        #endregion

        #endregion
    }
}