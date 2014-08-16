using System;
using Geex.Edit;
using Geex.Play.Custom;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Utils;
using Geex.Run;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public static partial class MakeCommand
    {
        #region Constants
        /// <summary>
        /// Minimum number of frame duration for Tags
        /// </summary>
        const int TAG_MIN_FRAME_DURATION = 40;
        #endregion

        #region Commands
        /// <summary>
        /// el: animation
        /// [who "character"]=self
        /// animation"integer"
        /// [option priority:"integer" pause:"integer" zoom:"integer"]
        /// </summary>
        static void Animation()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("who"))
            {
                ev = Command("who").ToEvent();
            }
            ev.AnimationId = Command("animation").ToInteger();
            if (Optional("option"))
            {
                Category("option");
                ev.AnimationPriority = Parameter("priority").ToInteger();
                ev.AnimationPause = Parameter("pause").ToInteger();
                ev.AnimationZoom = Parameter("zoom").ToInteger();
            }
        }

        /// <summary>
        /// eloption: antilag
        /// [event "Character"] = "self"
        /// [status "true|false"] = "false"
        /// </summary>
        static void Antilag()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToEvent();
            }
            bool status = false;
            if (Optional("status"))
            {
                status = Command("status").ToBoolean();
            }
            ev.IsAntilag = status;
        }

        /// <summary>
        /// el: collide
        /// [who "Character"]=player
        /// [collide "Character"]=self
        /// [melee]=true
        /// [skill "integer"]
        /// </summary>
        /// <returns></returns>
        static bool Collide()
        {
            GameCharacter who = InGame.Player;
            if (Optional("who")) who = Command("who").ToCharacter();
            GameEvent charaWith = InGame.Map.Events[EventId];
            if (Optional("collide")) charaWith = Command("collide").ToEvent();
            bool melee = true;
            int range = 2;    // Minimum range of 2, otherwise no collision
            return who.IsCollidingWithEvent(charaWith, range, range) & melee;
        }

        /// <summary>
        /// gm: effect
        /// [type "Distortion|fog|blur|none"]=none
        /// [values x:"int" y:"int" z:"int"]={0,0,0}
        /// [target type:"picture|event|map|fog|panorama" id:"int"]="map"
        /// texture "folder/filename"
        /// </summary>
        static void Effect()
        {
            GeexEffectType type = Run.GeexEffectType.None;
            if (Optional("type"))
            {
                switch (Command("type").ToString().ToLower())
                {
                    case "distortion":
                        type = Run.GeexEffectType.Distortion;
                        break;
                    case "fog":
                        type = Run.GeexEffectType.Fog;
                        break;
                    case "blur":
                        type = Run.GeexEffectType.RadialBlur;
                        break;
                    default:
                        return;
                }
            }
            int x = 0, y = 0, z = 0;
            if (Optional("values"))
            {
                Category("values");
                x = Parameter("x").ToInteger();
                y = Parameter("y").ToInteger();
                z = Parameter("z").ToInteger();
            }
            string targetType = "map";
            int id = 0;
            if (Optional("target"))
            {
                Category("target");
                targetType = Parameter("type").ToString().ToLower();
                id = Parameter("id").ToInteger();
            }
            Texture2D texture = null;
            if (Optional("texture"))
                texture = Cache.LoadTexture(Command("texture").ToString());
            float xf = 0f, yf = 0f, zf = 0f;
            switch (type)
            {
                case Run.GeexEffectType.Distortion:
                    xf = (x % texture.Width) / (float)texture.Width;
                    yf = (y % texture.Height) / (float)texture.Height;
                    zf = z / 100f;
                    break;
                case Run.GeexEffectType.Fog:
                    xf = (x % texture.Width) / (float)texture.Width;
                    yf = (y % texture.Height) / (float)texture.Height;
                    break;
                case Run.GeexEffectType.RadialBlur:
                    xf = x / 100f;
                    yf = y / 100f;
                    zf = z / 100f;
                    break;
            }
            switch (targetType)
            {
                case "map":
                    TileManager.GeexEffect.EffectTexture = texture;
                    TileManager.GeexEffect.EffectType = type;
                    TileManager.GeexEffect.EffectValue = new Vector3(xf, yf, zf);
                    return;
                case "picture":
                    InGame.Screen.Pictures[id].GeexEffect.EffectTexture = texture;
                    InGame.Screen.Pictures[id].GeexEffect.EffectType = type;
                    InGame.Screen.Pictures[id].GeexEffect.EffectValue = new Vector3(xf, yf, zf);
                    return;
                case "event":
                    InGame.Map.Events[id].GeexEffect.EffectTexture = texture;
                    InGame.Map.Events[id].GeexEffect.EffectType = type;
                    InGame.Map.Events[id].GeexEffect.EffectValue = new Vector3(xf, yf, zf);
                    return;
                case "fog":
                    InGame.Map.FogGeexEffect.EffectTexture = texture;
                    InGame.Map.FogGeexEffect.EffectType = type;
                    InGame.Map.FogGeexEffect.EffectValue = new Vector3(xf, yf, zf);
                    return;
                case "panorama":
                    InGame.Map.PanoramaGeexEffect.EffectTexture = texture;
                    InGame.Map.PanoramaGeexEffect.EffectType = type;
                    InGame.Map.PanoramaGeexEffect.EffectValue = new Vector3(xf, yf, zf);
                    return;
            }

        }

        /// <summary>
        /// eloption: eventgraphic
        /// [event "Character"] = "self"
        /// [status "true|false"] = "false"
        /// </summary>
        static void EventGraphic()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToEvent();
            }
            bool status = false;
            if (Optional("status"))
            {
                status = Command("status").ToBoolean();
            }
            ev.IsGraphicVisible = status;
        }

        /// <summary>
        /// el: eventchange
        /// [event  "Character"] = "self"
        /// [trigger type:"Action|Touch|Event|Auto|Parallel" page:"int"]
        /// movetype "integer"
        /// [Name "string"]
        /// </summary>
        static void EventChange()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToEvent();
            }
            if (Optional("trigger"))
            {
                Category("trigger");
                int type = Parameter("type").ToInteger();
                int page = 0;
                if (Optional("page"))
                {
                    page = Parameter("page").ToInteger();
                }
                ev.ResetType(page,type);
            }
            if (Optional("opacity"))
            { ev.Opacity = Command("opacity").ToByte(); }
            if (Optional("movetype"))
            { ev.MoveType = Command("movetype").ToInteger(); }
            if (Optional("name"))
            { ev.EventName = Command("name").ToString(); }
        }

        /// <summary>
        /// el: face
        /// who "Character"
        /// with "Character"
        /// </summary>
        static bool FaceCommand()
        {
            GameCharacter who = Command("who").ToCharacter();
            GameCharacter with = Command("with").ToCharacter();
            if (Optional("hard"))
                return who.IsFacingHard(with);
            return who.IsFacing(with);
        }

        /// <summary>
        /// elobject: fog
        /// id "integer"
        /// file "string"
        /// opacity "0-255"
        /// blend "normal|add|sub"
        /// move x:"integer" y:"integer" pause:"pause"
        /// </summary>
        static void Fog()
        {
            int fogId= Command("id").ToInteger();
            string fogName = Command("file").ToString();
            byte fogOpacity = Command("opacity").ToByte();
            short fogBlend = Command("blend").ToShort();
            Category("move");
            int fogMoveX = Parameter("x").ToInteger();
            int fogMoveY= Parameter("y").ToInteger();
            int fogMovePause = Parameter("pause").ToInteger();
        }

        /// <summary>
        /// el: font
        /// name "name"
        /// size "integer"
        /// color r:"integer" g:"integer" b:"integer"
        /// </summary>
        static void Font()
        {
            if (Optional("name"))
            {
                GeexEdit.DefaultFont = Command("name").ToString();
            }
            if (Optional("size"))
            {
                GeexEdit.DefaultFontSize = Command("size").ToShort();
            }
            if (Optional("color"))
            {
                Category("color");
                GeexEdit.DefaultFontColor = new Color(Parameter("r").ToByte(), Parameter("g").ToByte(), Parameter("b").ToByte());
            }
        }
        /// <summary>
        /// el: message window
        /// [title "text"]
        /// [mode "integer"]
        /// [opacity "integer"]
        /// [backopacity "integer"]
        /// [x "integer"]
        /// [y "integer"]
        /// [Font "string"]
        /// [fontsize "integer"]
        /// [setcolor num:"integer" r:"integer" v:"integer" b:"interger"]
        /// [lockkeys "boolean"]
        /// [lockevent "true|false"
        /// </summary>
        static void MessageWindow()
        {
            if (Optional("title")) { InGame.Temp.MessageWindow.TitleText = Command("title").ToString(); }
            if (Optional("mode")) { InGame.Temp.MessageWindow.Mode = (short)Command("mode").ToInteger(); }
            if (Optional("opacity")) { InGame.Temp.MessageWindow.Opacity = (byte)Command("opacity").ToInteger(); }
            if (Optional("backopacity")) { InGame.Temp.MessageWindow.WindowBackOpacity = (byte)Command("backopacity").ToInteger(); }
            if (Optional("x")) { InGame.Temp.MessageWindow.X = Command("x").ToInteger(); }
            if (Optional("y")) { InGame.Temp.MessageWindow.Y = Command("y").ToInteger(); }
            if (Optional("font")) { InGame.Temp.MessageWindow.WindowFontName = Command("font").ToString(); }
            if (Optional("fontsize")) { InGame.Temp.MessageWindow.WindowFontSize = (short)Command("fontsize").ToInteger(); }
            if (Optional("setcolor"))
            {
                Category("setcolor");
                int num = Parameter("num").ToInteger();
                byte r = (byte)Parameter("r").ToInteger();
                byte g = (byte)Parameter("v").ToInteger();
                byte b = (byte)Parameter("b").ToInteger();
                InGame.Temp.MessageWindow.ColorTable[num] = new Color(r, g, b);
            }
            if (Optional("lockkeys")) { InGame.Temp.MessageWindow.IsKeyLocked = Command("lockkeys").ToBoolean(); }
            if (Optional("lockevent")) { InGame.Temp.MessageWindow.IsEventLocked = Command("lockevent").ToBoolean(); }
        }

        /// <summary>
        /// eloption: moveto
        /// [event "Character"]=self
        /// position x:"integer" y:"integer"
        /// map x:"integer" y:"integer"
        /// shift x:"integer" y:"integer"
        /// </summary>
        static void MoveTo()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToEvent();
            }
            int x = ev.X;
            int y = ev.Y;
            if (Optional("position"))
            {
                Category("position");
                x = Parameter("x").ToInteger();
                y = Parameter("y").ToInteger();
            }
            if (Optional("map"))
            {
                Category("map");
                x = Parameter("x").ToInteger()*32+16;
                y = Parameter("y").ToInteger()*32+32;
            }
            if (Optional("shift"))
            {
                Category("shift");
                x += Parameter("x").ToInteger();
                y += Parameter("y").ToInteger();
            }
            ev.Moveto(x, y);
        }

        /// <summary>
        /// elobject:particle
        /// [event "Character"]=self
        /// type "type"
        /// </summary>
        static void Particle()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToEvent();
            }
            string type = Command("type").ToString();
            ParticleEffect effect;
            switch (type)
            {
                case "flame":
                    effect = ParticleEffect.Flame;
                    break;
                case "aura":
                    effect = ParticleEffect.Aura;
                    break;
                case "black":
                    effect = ParticleEffect.Soot;
                    break;
                case "fireplace":
                    effect = ParticleEffect.Fireplace;
                    break;
                case "smoke":
                    effect = ParticleEffect.Smoke;
                    break;
                case "eventname":
                    effect = ParticleEffect.EventBase;
                    break;
                case "water":
                    effect = ParticleEffect.Splash;
                    break;
                case "light":
                    effect = ParticleEffect.Light;
                    break;
                case "flare":
                    effect = ParticleEffect.Flare;
                    break;
                case "teleport":
                    effect = ParticleEffect.Teleport;
                    break;
                case "spirit":
                    effect = ParticleEffect.Spirit;
                    break;
                default:
                    effect = ParticleEffect.None;
                    break;
            }
            if (!ev.IsParticleTriggered)
            {
                ev.IsParticleTriggered = true;
                InGame.Map.Particles.Add(new GameParticle(ev, effect));
            }
        }

        /// <summary>
        /// el: particle off
        /// [event "Character"]
        /// </summary>
        static void ParticleOff()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToEvent();
            }
            ev.IsParticleTriggered = false;
        }

        /// <summary>
        /// el: Picture display
        /// num "integer"
        /// Name "string"
        /// position x:"integer" y:"integer"
        /// [Size zoom_x:"integer" zoom_y:"integer"]
        /// [opacity "0-255"]
        /// [blend "normal|add|sub"]
        /// [locked "boolean"]
        /// [centered "boolean"]
        /// [background "boolean"]=false
        /// </summary>
        static void PictureDisplay()
        {
            int num = MakeCommand.Command("num").ToInteger();
            string name = MakeCommand.Command("name").ToString();
            MakeCommand.Category("position");
            int x = MakeCommand.Parameter("x").ToInteger();
            int y = MakeCommand.Parameter("y").ToInteger();
            int zoom_x = 100;
            int zoom_y = 100;
            bool background = false;
            if (MakeCommand.Optional("size"))
            {
                MakeCommand.Category("size");
                zoom_x = MakeCommand.Parameter("zoom_x").ToInteger();
                zoom_y = MakeCommand.Parameter("zoom_y").ToInteger();
            }
            int blend_type = 0;
            if (MakeCommand.Optional("blend"))
            {
                blend_type = MakeCommand.Command("blend").ToBlendType();
            }
            byte opacity = 255;
            if (MakeCommand.Optional("opacity"))
            {
                opacity = (byte)MakeCommand.Command("opacity").ToInteger();
            }
            bool locked = false;
            if (MakeCommand.Optional("locked"))
            {
                locked = MakeCommand.Command("locked").ToBoolean();
            }
            if (MakeCommand.Optional("background"))
            {
                background = MakeCommand.Command("background").ToBoolean();
            }
            if (InGame.Temp.IsInBattle)
            {
                InGame.Screen.BattlePictures[num].Show(name, 0, x, y, zoom_x, zoom_y, opacity, blend_type, locked);
            }
            else
            {
                InGame.Screen.Pictures[num].Show(name, 0, x, y, zoom_x, zoom_y, opacity, blend_type, locked, true, background);
            }
        }

        /// <summary>
        /// el: Picture move
        /// num "integer"
        /// duration "integer"
        /// position x:"integer" y:"integer"
        /// [Size zoom_x:"integer" zoom_y:"integer"]
        /// [opacity "0-255"]
        /// [blend_type "normaladd|sub"]
        /// [angle "0-360"]
        /// </summary>
        static void PictureMove()
        {
            GamePicture picture = MakeCommand.Command("num").ToPicture();
            int duration = MakeCommand.Command("duration").ToInteger();
            MakeCommand.Category("position");
            int x = MakeCommand.Parameter("x").ToInteger();
            int y = MakeCommand.Parameter("y").ToInteger();
            float zoom_x = 100f;
            float zoom_y = 100f;
            byte opacity = 255;
            int blend_type = 0;
            if (picture != null)
            {
                zoom_x = picture.ZoomX;
                zoom_y = picture.ZoomY;
                opacity = picture.Opacity;
                blend_type = picture.BlendType;
            }
            if (MakeCommand.Optional("size"))
            {
                MakeCommand.Category("size");
                zoom_x = MakeCommand.Parameter("zoom_x").ToInteger();
                zoom_y = MakeCommand.Parameter("zoom_y").ToInteger();
            }
            if (MakeCommand.Optional("blend_type"))
            {
                blend_type = MakeCommand.Command("blend_type").ToBlendType();
            }
            int angle = 0;
            if (MakeCommand.Optional("opacity"))
            {
                opacity = (byte)MakeCommand.Command("opacity").ToInteger();
            }
            if (MakeCommand.Optional("angle"))
            {
                angle = MakeCommand.Command("angle").ToInteger();
            }
            if (picture != null)
            {
                picture.Move(duration, picture.Origin, x, y, zoom_x, zoom_y, opacity, blend_type, angle);
            }
        }

        /// <summary>
        /// el: Picture rotate
        /// num "integer"
        /// [speed "integer"]
        /// [angle "integer"]
        /// </summary>
        static void PictureRotate()
        {
            GamePicture picture = MakeCommand.Command("num").ToPicture();
            int speed = 0;
            if (MakeCommand.Optional("speed"))
            {
                speed = MakeCommand.Command("speed").ToInteger();
            }
            if (picture != null) picture.Rotate(speed);
        }

        /// <summary>
        /// el: priority
        /// event "character"
        /// value "integer"
        /// </summary>
        static void Priority()
        {
            GameCharacter ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToCharacter();
            }
            ev.Priority = Command("value").ToInteger() * 32;
        }

        /// <summary>
        /// el: scroll
        /// direction "integer"
        /// case "integer"
        /// speed "integer"
        /// </summary>
        static void Scroll()
        {
            short dir = Command("direction").ToShort();
            int dist = Command("case").ToInteger();
            short speed = Command("speed").ToShort();
            InGame.Map.StartScroll(dir, dist, speed);
        }

        /// <summary>
        /// el: start Picture tone change
        /// num "integer"
        /// tone red:"0-255" green:"0-255" blue:"0-255" gray:"0-255"
        /// duration "integer"
        /// </summary>
        static void StartPictureToneChange()
        {
            GamePicture picture = MakeCommand.Command("num").ToPicture();
            MakeCommand.Category("tone");
            int red = MakeCommand.Parameter("red").ToInteger();
            int green = MakeCommand.Parameter("green").ToInteger();
            int blue = MakeCommand.Parameter("blue").ToInteger();
            int gray = MakeCommand.Parameter("gray").ToInteger();
            int duration = MakeCommand.Parameter("duration").ToInteger();
            if (picture != null)
            {
                picture.StartToneChange(new Tone(red, green, blue, gray), duration);
            }
        }

        /// <summary>
        /// el: :  Picture EraseRect
        /// num "integer"
        /// </summary>
        static void PictureErase()
        {
            GamePicture picture = MakeCommand.Command("num").ToPicture();
            if (picture != null) picture.Erase();
        }

        /// <summary>
        /// el: move
        /// event "Character"
        /// direction "left|right|up|down|toward|to|find|away|jumpto"
        /// [pixel "interger"]
        /// [position x:"integer" y:"integer"]
        /// [map x:"integer" y:"integer"]
        /// [shift x:"integer" y:"integer"]
        /// [toward "event"]
        /// [away "event"]
        /// [locked "boolean"]
        /// </summary>
        static void Move()
        {
            GameCharacter ev = InGame.Map.Events[EventId];
            if (Optional("event"))
            {
                ev = Command("event").ToCharacter();
            }
            string directionType = "moveto";
            if (Optional("direction")) directionType = Command("direction").ToString();
            GameEvent currentEvent = InGame.Map.Events[EventId];
            short pixel = 32;
            if (Optional("pixel")) pixel = (short)Command("pixel").ToInteger();
            int x = currentEvent.X;
            int y = currentEvent.Y;
            if (Optional("position"))
            {
                Category("position");
                x = Parameter("x").ToInteger();
                y = Parameter("y").ToInteger();
            }
            if (Optional("map"))
            {
                Category("map");
                x = Parameter("x").ToInteger() * 32+16;
                y = Parameter("y").ToInteger() * 32+32;
            }
            if (Optional("shift"))
            {
                Category("shift");
                x = ev.X+Parameter("x").ToInteger();
                y = ev.Y+Parameter("y").ToInteger();
            }
            if (Optional("locked")) ev.IsLocked = Command("locked").ToBoolean();
            switch (directionType.ToLower())
            {
                case "down":
                    ev.MoveDown(true, pixel);
                    break;
                case "left":
                    ev.MoveLeft(true, pixel);
                    break;
                case "up":
                    ev.MoveUp(true, pixel);
                    break;
                case "right":
                    ev.MoveRight(true, pixel);
                    break;
                case "toward":
                    ev.MoveTowardCharacter(Command("toward").ToCharacter());
                    break;
                case "away":
                    ev.MoveAwayFromCharacter(Command("away").ToCharacter());
                    break;
                case "find":
                    ev.FindPath(x,y);
                    break;
                case "jumpto":
                    ev.JumpTo(x,y);
                    break;
                case "to":
                    ev.Moveto(x,y);
                    /*if (ev==InGame.Player)
                    {
                        foreach(GameEvent e in InGame.Map.events)
                        {
                            if (e == null) continue;
                            e.RefreshUpdate();
                        }
                    }*/
                    break;
            }
        }

        /// <summary>
        ///  el:Size
        ///  [event "Character"] = "self"
        ///  dimension width:"integer" height:"integer"
        /// </summary>
        static void Size()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event")) ev = Command("event").ToEvent();
            Category("dimension");
            ev.CollisionWidth = Parameter("width").ToInteger();
            ev.CollisionHeight = Parameter("height").ToInteger();
        }
        
        /// <summary>
        /// el: selfswitch
        /// switch [map:"integer"] [id:"integer"] local:"A-Z"
        /// [set "true|false"]
        /// </summary>
        /// <returns></returns>
        static void SelfSwitch()
        {
            MakeCommand.Category("switch");
            int mapId = MakeCommand.MapId;
            int id = MakeCommand.EventId;
            if (MakeCommand.Optional("map")) mapId = MakeCommand.Parameter("map").ToInteger();
            if (MakeCommand.Optional("id")) id = MakeCommand.Parameter("id").ToInteger();
            string local = MakeCommand.Parameter("local").ToString();
            GameSwitch s = new GameSwitch(mapId, id, local);
            if (MakeCommand.Optional("set"))
            {
                InGame.System.GameSelfSwitches[s] = MakeCommand.Command("set").ToBoolean();
                if (mapId == InGame.Map.MapId) InGame.Map.IsNeedRefresh = true;
            }
            else
            {
                LastCondition = InGame.System.GameSelfSwitches[s];
            }
        }

        /// <summary>
        /// eloption: reset selfswitches
        /// [event "Character"] = "self"
        /// </summary>
        static void ResetSelfSwitch()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            if (Optional("event")) ev = Command("event").ToEvent();
            ev.isResetSelfSwitches = true;
        }

        /// <summary>
        /// el: swap full screen
        /// </summary>
        static void ToggleFullScreen()
        {
            Graphics.ToggleFullScreen();
        }

        /// <summary>
        /// el: transform
        /// who "Character"
        /// zoom x:"integer" y:"integer" 
        /// [angle "integer"]
        /// </summary>
        static void Transform()
        {
            GameCharacter ev = InGame.Map.Events[EventId];
            if (Optional("who"))
            {
                ev = Command("who").ToCharacter();
            }
            float zoom_x=1f;
            float zoom_y=1f;
            if (Optional("zoom"))
            {
                Category("zoom");
                zoom_x = Parameter("x").ToInteger()/100f;
                zoom_y = Parameter("y").ToInteger()/100f;
            }
            ev.ZoomX=zoom_x;
            ev.ZoomY =zoom_y;
            if (Optional("angle"))
            {
                ev.Angle=Command("angle").ToInteger();
            }
        }

        /// <summary>
        /// el: tag
        /// [event "Character"]
        /// [type "weapon|armor|item|skill"]
        /// [id "integer"]
        /// [duration "80+"]
        /// [text "string"]
        /// [fade "true"|"false"]
        /// [down "true"|"false"]
        /// </summary>
        static void TagCommand()
        {
            GameCharacter ev = InGame.Map.Events[EventId];
            if (Optional("event")) ev = Command("event").ToCharacter();
            string type = "";
            if (Optional("type")) type = Command("type").ToString();
            int id = 0;
            string icon = "";
            if (Optional("id"))
            {
                id = Command("id").ToInteger();
                switch (type.ToLower())
                {
                    case "item":
                        icon = Data.Items[id].IconName;
                        break;
                    case "weapon":
                        icon = Data.Weapons[id].IconName;
                        break;
                    case "armor":
                        icon = Data.Armors[id].IconName;
                        break;
                    case "skil":
                        icon = Data.Skills[id].IconName;
                        break;
                }
            }
            int duration = 80;
            if (Optional("duration")) duration = Math.Max(TAG_MIN_FRAME_DURATION,Command("duration").ToInteger());
            string text = "";
            if (Optional("text")) text = Command("text").ToString();
            bool fade = true;
            if (Optional("fade")) fade = Command("fade").ToBoolean();
            bool iconDown = false;
            if (Optional("down")) iconDown = Command("down").ToBoolean();
            InGame.Tags.TagList.Add(new Tag(ev,text,icon, duration, fade, iconDown));
         }

        /// <summary>
        /// el: Tile
        /// [tileset x:"int" y:"int"]
        /// [Size w:"int" h:"int"]
        /// [position x:"int" y:"int"]
        /// [shift ox:"int" oy:"int"]
        /// [layer "int"]
        /// </summary>
        static void Tile()
        {
            GameEvent ev = InGame.Map.Events[EventId];
            int x = (ev.X - 16) / 32;
            int y = (ev.Y - 16) / 32;
            int w = 0;
            int h = 0;
            if (Optional("tileset"))
            {
                Category("tileset");
                w = Parameter("x").ToInteger();
                h = Parameter("y").ToInteger();
            }
            int width = 1;
            int height = 1;
            if (Optional("size"))
            {
                Category("size");
                width = Parameter("w").ToInteger();
                height = Parameter("h").ToInteger();
            }
            if (Optional("position"))
            {
                Category("position");
                x = Parameter("x").ToInteger();
                y = Parameter("y").ToInteger();
            }
            if (Optional("shift"))
            {
                Category("shift");
                x += Parameter("ox").ToInteger();
                y += Parameter("oy").ToInteger();
            }
            int z = 2;
            if (Optional("layer")) z = Command("layer").ToInteger();
            int start_tile = 0;
            if (ev.TileId != 0)
            {
                start_tile = ev.TileId + h * 8 + w;
            }
            else
            {
                start_tile = h * 8 + w + 384;
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // If original Tile == 0, then it may cause problem with TileManager optimisation
                    TileManager.MapData[x + i][y + j][z] = (short)(start_tile + j * 8 + i);
                }
            }
        }

        /// <summary>
        /// el: tilechange
        /// from "id"
        /// to "id"
        /// </summary>
        static void TileChange()
        {
            short from = (short)(Command("from").ToInteger()+384);
            short to = (short)(Command("to").ToInteger() + 384);
            for (int i = 0; i < InGame.Map.Width; i++)
            {
                for (int j = 0; j < InGame.Map.Height; j++)
                {
                    for (int k = 0; k < 3;k++)
                        if (TileManager.MapData[i][j][k] == from) TileManager.MapData[i][j][k] = to;
                }
            }
        }
        
        /// <summary>
        /// elobject: transfer
        /// [left "map_id"]
        /// [up "map_id"]
        /// [right "map_id"]
        /// [down "map_id"]
        /// </summary>
        static void Transfer()
        {
            if (Optional("left"))
            {
                InGame.Player.EdgeTransferList[4]=Command("left").ToShort();
            }
            if (Optional("up"))
            {
                InGame.Player.EdgeTransferList[8] = Command("up").ToShort();
            } if (Optional("right"))
            {
                InGame.Player.EdgeTransferList[6]=Command("right").ToShort();
            }
            if (Optional("down"))
            {
                InGame.Player.EdgeTransferList[2]=Command("down").ToShort();
            }
        }

        /// <summary>
        /// el: view range
        /// who "Character"
        /// with "Character"
        /// distance radius:"int" [type:"int"]=2
        /// </summary>
        static bool ViewRange()
        {
            GameCharacter who = Command("who").ToCharacter();
            GameCharacter with = Command("with").ToCharacter();
            Category("distance");
            int radius = Parameter("radius").ToInteger();
            bool square = false;
            if (Optional("type"))
            {
                square = (Parameter("type").ToInteger()!=2);
            }
            return who.ViewRange(with,radius,square);
        }

        /// <summary></summary>
        /// el: zone
        /// x "integer"
        /// y "integer"
        /// [width "integer"]=1
        /// [height "integer"]=1
        /// [tox "integer"]
        /// [toy "integer"]
        /// </summary>
        static bool Zone()
        {
            int x = MakeCommand.Command("x").ToInteger();
            int y = MakeCommand.Command("y").ToInteger();
            int w =1;
            int h= 1;
            if (MakeCommand.Optional("width")) w = MakeCommand.Command("width").ToInteger();
            if (MakeCommand.Optional("height")) h = MakeCommand.Command("height").ToInteger();
            if (MakeCommand.Optional("tox")) w = MakeCommand.Command("tox").ToInteger() - x;
            if (MakeCommand.Optional("toy")) h = MakeCommand.Command("toy").ToInteger() - y;
            return (InGame.Player.X > x * 32 && InGame.Player.Y > y * 32 && InGame.Player.X < (x + w) * 32 && InGame.Player.Y < (y + h) * 32);
        }

        /// <summary></summary>
        /// el: screenzoom
        /// targetx "integer"
        /// targety "integer"
        /// duration "integer"
        static void ScreenZoom()
        {
            InGame.Screen.StartZoom(
                MakeCommand.Command("targetx").ToInteger() / 100f,
                MakeCommand.Command("targety").ToInteger() / 100f,
                MakeCommand.Command("duration").ToInteger());
        }
        #endregion
    }
}

