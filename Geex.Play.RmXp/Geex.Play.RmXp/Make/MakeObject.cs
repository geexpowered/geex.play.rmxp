#region Usings
using System;
using Geex.Edit;
using Geex.Play.Rpg;
using Geex.Play.Rpg.Game;
using System.Text.RegularExpressions;
#endregion

namespace Geex.Play.Make
{
    /// <summary>
    /// Contains complementary Event Language generic object and methods to transform them
    /// </summary>
    public partial class MakeObject
    {
        #region Variables
        /// <summary>
        /// Represents Event Language command as a string
        /// </summary>
        string expr;
        #endregion

        #region Methods
        /// <summary>
        /// Build a Geex Make object
        /// </summary>
        /// <param Name="text"></param>
        public MakeObject(string text)
        {
            expr = MakeCommand.GSub(text, new Regex(@"\\[Vv]\[([0-9]+)\]"), InGame.Variables.Arr);
        }

        /// <summary>
        /// Change an MakeObject into an integer.
        /// </summary>
        public int ToInteger()
        {
            return int.Parse(expr);
        }

        /// <summary>
        /// Change an MakeObject into a Byte.
        /// </summary>
        public byte ToByte()
        {
            return byte.Parse(expr);
        }

        /// <summary>
        /// Change an MakeObject into a short.
        /// </summary>
        public short ToShort()
        {
            return short.Parse(expr);
        }

        /// <summary>
        /// Change an MakeObject into a string.
        /// </summary>
        public string ToString()
        {
            return expr;
        }

        /// <summary>
        /// Change an MakeObject to a boolean
        /// </summary>
        public bool ToBoolean()
        {
            if (expr.ToLower() == "true" || expr == "1") return true;
            return false;
        }


        /// <summary>
        /// Change an MakeObject into blend type (normal=0, add=1, sub=2)
        /// </summary>
        public int ToBlendType()
        {
            switch (expr.ToLower())
            {
                case "normal":
                    return 0;
                case "add":
                    return 1;
                case "sub":
                    return 2;
                default:
                    throw new ArgumentException("Syntax error (Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + expr);
            }
        }

        /// <summary>
        ///  Change an MakeObject inta an event trigger option
        /// </summary>
        /// <returns></returns>
        public int ToTrigger()
        {
            switch (expr.ToLower())
            {
                case "action":
                    return 0;
                case "touch":
                    return 1;
                case "event":
                    return 2;
                case "auto":
                    return 3;
                case "parallel":
                    return 4;
                default:
                    throw new ArgumentException("Syntax error (Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + expr);
            }
        }
   
        /// <summary>
        /// Change an MakeObject into a GameCharacter
        ///"player" = the player Character
        /// "self"  = the local event
        /// "event_id:"id"" = the event id "id"
        /// "event_name:"Name"" = the event Name "Name"
        /// </summary>
        public GameCharacter ToCharacter()
        {
            try
            {
                switch (expr)
                {
                    case "self":
                        return InGame.Map.Events[MakeCommand.EventId];

                    case "player":
                        return InGame.Player;
                    default:
                        string[] temp = expr.Split(MakeCommand.ParamSeparator);
                        if (temp[0] == "event_id")
                        {
                            return InGame.Map.Events[new MakeObject(temp[1].Substring(0, temp[1].Length)).ToInteger()];
                        }
                        if (temp[0] == "event_name")
                        {
                            string name = new MakeObject(temp[1].Substring(0, temp[1].Length)).ToString();
                            foreach (GameCharacter character in InGame.Map.Events)
                            {
                                if (character!=null && character.CharacterName == name) return character;
                            }
                            throw new ArgumentException("Character name not found (Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + temp[1]);
                        }
                        return null;
                }
            }
            catch
            {
                throw new ArgumentException("Syntax error (Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + expr);
            }
        }

        /// <summary>
        /// Change an MakeObject into a GameEvent
        /// "self"  = the local event
        /// "event_id:"id"" = the event id "id"
        /// "event_name:"Name"" = the event Name "Name"
        /// </summary>
        public GameEvent ToEvent()
        {
            try
            {
                switch (expr)
                {
                    case "self":
                        return InGame.Map.Events[MakeCommand.EventId];

                    default:
                        string[] temp = expr.Split(MakeCommand.ParamSeparator);
                        if (temp[0] == "event_id")
                        {
                            return InGame.Map.Events[new MakeObject(temp[1].Substring(0, temp[1].Length)).ToInteger()];
                        }
                        if (temp[0] == "event_name")
                        {
                            string name = new MakeObject(temp[1].Substring(0, temp[1].Length)).ToString();
                            foreach (GameEvent character in InGame.Map.Events)
                            {
                                if (character!=null && character.CharacterName == name) return character;
                            }
                            throw new ArgumentException("Character name not found (Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + temp[1]);
                        }
                        return null;
                }
            }
            catch
            {
                throw new ArgumentException("Syntax error (Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + expr);
            }
        }
        
        /// <summary>
        /// Change an ELOBject to a game_character array
        /// </summary>
        public GameCharacter[] ToCharacterArray()
        {
            switch (expr)
            {
                case "any:event":
                    return InGame.Map.Events;
                case "any:character":
                    GameCharacter[] temp = new GameCharacter[InGame.Map.Events.Length + 1];
                    InGame.Map.Events.CopyTo(temp, 0);
                    temp[InGame.Map.Events.Length] = InGame.Player;
                    return temp;
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// Change an MakeObject into a GamePicture
        /// picture_num:"num" => Picture with number = "num".
        /// </summary>
        /// <returns></returns>
        public GamePicture ToPicture()
        {
            try
            {
                string[] temp = expr.Split(MakeCommand.ParamSeparator);
                int num = 0;
                if (temp[0] == "picture_num")
                {
                    num = int.Parse(temp[1].Substring(1, temp[1].Length - 2));
                }
                else
                {
                    //num = int.Parse(expr.Substring(1, expr.Length - 2));
                    num = int.Parse(expr);
                }
                if (num > GeexEdit.NumberOfPictures)
                {
                    throw new ArgumentException("Picture number out of range in(Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + expr);
                }
                else
                {
                    if (InGame.Temp.IsInBattle)
                    {
                        return InGame.Screen.BattlePictures[num];
                    }
                    else
                    {
                        return InGame.Screen.Pictures[num];
                    }
                }
            }
            catch
            {
                throw new ArgumentException("Syntax error (Map ID:" + MakeCommand.MapId + ", event id:" + MakeCommand.EventId + ") : " + expr);
            }
        }
        #endregion
    }
}