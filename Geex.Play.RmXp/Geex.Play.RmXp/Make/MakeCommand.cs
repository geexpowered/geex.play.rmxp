#region Usings
using System;
using System.Text.RegularExpressions;
using Geex.Run;

#endregion

namespace Geex.Play.Make
{
    /// <summary>
    /// Contains methods and properties to manage Geex Make commands. The main structure is as follow:
    /// el:commandName
    /// command "value"
    /// category parameter:"value" paramter:"value"
    /// </summary>
    public static partial class MakeCommand
    {
        #region Variables
        /// <summary>
        /// Current event id where the command is started
        /// </summary>
        public static int EventId;

        /// <summary>
        /// Current map id where the command is started
        /// </summary>
        public static int MapId;
        /// <summary>
        /// Current Event Language command
        /// </summary>
        static string[] script;

        /// <summary>
        /// Separators within the el command
        /// </summary>
        public static char[] CommandSeparator = new char[2] { '\n', ' ' };

        /// <summary>
        /// Separators within the parameter
        /// <summary>
        public static char[] ParamSeparator = new char[1] { ':' };

        /// <summary>
        /// Position within command
        /// </summary>
        static uint index = 0;

        /// <summary>
        /// Command localName
        /// </summary>
        public static string Name;

        /// <summary>
        /// EL Command Type
        /// </summary>
        //public static MakeCommandType type;

        /// <summary>
        /// Result of the last condition
        /// </summary>
        public static bool LastCondition=false;
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the command
        /// </summary>
        /// <param Name="text">script to be initialized</param>
        public static void Initialize(string[] text)
        {
            // set command localName and type
            Name = text[0];
            //type = toCommandType(text[1]);
            script=text;
            index = 1;
            LastCondition = false;
        }

        /// <summary>
        /// Replaces all substrings of text matching \code[index] with the array[index]values
        /// </summary>
        /// <param Name="text">string to be changed</param>
        /// <param Name="pattern">Pattern to be searched</param>
        /// <param Name="array">array to be used</param>
        public static string GSub(string text, Regex pattern, int[] array)
        {
            Match match = pattern.Match(text);
            //Match match = Regex.Match(text,pattern);
            while (match.Success)
            {
                // figure out the index into our array
                string stringIndex = match.Value;
                int arrayIndex = int.Parse(stringIndex.Substring(3, stringIndex.Length - 4));
                text = text.Replace(match.Value, array[arrayIndex].ToString());
                // get the next match 
                match = match.NextMatch();
            }
            return text;
        }

        /// <summary>
        /// Return the MakeObject of the command. Format must be commandName "Name" or an error is raised
        /// </summary>
        /// <param Name="Name">localName of the command</param>
        public static MakeObject Command(string name)
        {
            if (script[index] == name)
            {
                // go to command value
                index+=2;
                // return value without "
                return new MakeObject(script[index-1].Substring(0, script[index-1].Length));
            }
            else
            {
                throw new ArgumentException(String.Format("Geex Make - Syntax error in Map ID:{0} and event ID:{1}", MapId, EventId));
            }
        }

        /// <summary>
        /// Category format is categoryName param1:"value param2:"value"
        /// </summary>
        /// <param Name="Name">localName of the category</param>
        public static void Category(string categoryName)
        {
            if (script[index] == categoryName)
            {
                // go to command value
                index++;
            }
            else
            {
                throw new ArgumentException(String.Format("Geex Make - Syntax error in Map ID:{0} and event ID:{1}", MapId, EventId));
            }
        }
        
        /// <summary>
        /// Return the MakeObject of the parameter 'Name'. Format must be commandName:"Name" or an error is raised
        /// </summary>
        /// <param Name="Name">localName of parameter</param>
        public static MakeObject Parameter(string commandName)
        {
            string[] param = script[index].Split(ParamSeparator);
            if (param[0] == commandName)
            {
                // return variable without "
                index++;
                return new MakeObject(param[1].Substring(0, param[1].Length));
            }
            else
            {
                throw new ArgumentException(String.Format("Geex Make - Syntax error in Map ID:{0} and event ID:{1}", MapId, EventId));
            }
        }

        /// <summary>
        /// Return true is next command is 'Name'
        /// </summary>
        /// <param Name="Name">Name of the command to be found</param>
        public static bool Optional(string name)
        {
            if (index>=script.Length) return false;
            string[] param = script[index].Split(ParamSeparator);
            return (param[0] == name);
        }

        /// <summary>
        /// Trigger the right Geex Make command, option or object
        /// </summary>
        public static void Start()
        {
            //lastCondition = false;
            switch (MakeCommand.Name)
            {
                case "animation":
                    Animation();
                    return;
                case "antilag":
                    Antilag();
                    return;
                case "collide":
                    LastCondition = Collide();
                    break;
                case "eventchange":
                    EventChange();
                    break;
                case "eventgraphic":
                    EventGraphic();
                    return;
                case "effect":
                    Effect();
                    return;
                case "face":
                    LastCondition = FaceCommand();
                    return;
                case "font":
                    Font();
                    return;
                case "messagewindow":
                    MessageWindow();
                    return;
                case "move":
                    Move();
                    return;
                case "moveto":
                    MoveTo();
                    return;
                case "particle":
                    Particle();
                    return;
                case "particleoff":
                    ParticleOff();
                    return;
                case "picturedisplay":
                    PictureDisplay();
                    return;
                case "pictureerase":
                    PictureErase();
                    return;
                case "picturemove":
                    PictureMove();
                    return;
                case "picturerotate":
                    PictureRotate();
                    return;
                case "priority":
                    Priority();
                    return;
                case "scroll":
                    Scroll();
                    return;
                case "startpicturetonechange":
                    StartPictureToneChange();
                    return;
                case "selfswitch":
                    SelfSwitch();
                    return;
                case "size":
                    Size();
                    return;
                case "swapfullscreen":
                    ToggleFullScreen();
                    return;
                case "tag":
                    TagCommand();
                    return;
                case "tile":
                    Tile();
                    return;
                case "tilechange":
                    TileChange();
                    return;
                case "transfer":
                    Transfer();
                    return;
                case "transform":
                    Transform();
                    return;
                case "viewrange":
                    LastCondition = ViewRange();
                    return;
                case "zone":
                    LastCondition = Zone();
                    return;
                case "screenzoom":
                    ScreenZoom();
                    return;
                default:
                    break;
            }
        }
        #endregion
    }

}
