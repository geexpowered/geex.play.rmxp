using System;
using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Play.Rpg;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Conditional Branch
        /// </summary>
        bool Command111()
        {
            // Initialize local variable: result
            bool result = false;
            switch (intParams[0])
            {
                #region if switch
                case 0:  // switch
                    result = (InGame.Switches.Arr[intParams[1]] == (intParams[2] == 0));
                    break;
                #endregion

                #region if variable
                case 1:  // variable
                    int value1 = InGame.Variables.Arr[intParams[1]];
                    int value2;
                    if (intParams[2] == 0)
                    {
                        value2 = intParams[3];
                    }
                    else
                    {
                        value2 = InGame.Variables.Arr[intParams[3]];
                    }
                    switch (intParams[4])
                    {
                        case 0:  // value1 is equal to value2
                            result = (value1 == value2);
                            break;
                        case 1:  // value1 is greater than or equal to value2
                            result = (value1 >= value2);
                            break;
                        case 2:  // value1 is less than or equal to value2
                            result = (value1 <= value2);
                            break;
                        case 3:  // value1 is greater than value2
                            result = (value1 > value2);
                            break;
                        case 4:  // value1 is less than value2
                            result = (value1 < value2);
                            break;
                        case 5:  // value1 is not equal to value2
                            result = (value1 != value2);
                            break;
                    }
                    break;
                #endregion

                #region if self switch
                case 2:  // self switch
                    if (eventId > 0)
                    {
                        GameSwitch key = new GameSwitch(InGame.Map.MapId, eventId, stringParams[0]);
                        if (intParams[1] == 0)
                        {
                            result = (InGame.System.GameSelfSwitches[key] == true);
                        }
                        else
                        {
                            result = (InGame.System.GameSelfSwitches[key] != true);
                        }
                    }
                    break;
                #endregion
                
                #region  if Timer
                case 3:  // Timer
                    if (InGame.System.IsTimerWorking)
                    {
                        int sec = InGame.System.Timer / Graphics.FrameRate;
                        if (intParams[2] == 0)
                        {
                            result = (sec >= intParams[1]);
                        }
                        else
                        {
                            result = (sec <= intParams[1]);
                        }
                    }
                    break;
                #endregion

                #region if actor
                case 4:  // actor
                    GameActor actor = InGame.Actors[intParams[1]-1];
                    if (actor != null)
                    {
                        switch (intParams[2])
                        {
                            case 0:  // in party
                                result = InGame.Party.Actors.Contains(actor);
                                break;
                            case 1:  // Name
                                result = (actor.Name == stringParams[0]);
                                break;
                            case 2:  // skill
                                result = (actor.IsSkillLearn(intParams[3]));
                                break;
                            case 3:  // weapon
                                result = (actor.WeaponId == intParams[3]);
                                break;
                            case 4:  // armor
                                int test = intParams[3];
                                result = (actor.ArmorShield == test || actor.ArmorHelmet == test || actor.ArmorBody == test || actor.ArmorAccessory == test);
                                break;
                            case 5:  // state
                                result = (actor.IsState(intParams[3]));
                                break;
                        }
                    }
                    break;
                #endregion

                #region if enemy
                case 5:  // enemy
                    GameNpc enemy = InGame.Troops.Npcs[intParams[1]];
                    if (enemy != null)
                    {
                        switch (intParams[2])
                        {
                            case 0:  // appear
                                result = (enemy.IsExist);
                                break;
                            case 1:  // state
                                result = (enemy.IsState(intParams[3]));
                                break;
                        }
                    }
                    break;
                #endregion

                #region if Character
                case 6:  // Character
                    GameCharacter character = GetCharacter(intParams[1]);
                    if (character != null) result = (character.Dir == intParams[2]);
                    break;
                #endregion

                #region if gold
                case 7:  // gold
                    if (intParams[2] == 0)
                    {
                        result = (InGame.Party.Gold >= intParams[1]);
                    }
                    else
                    {
                        result = (InGame.Party.Gold <= intParams[1]);
                    }
                    break;
                #endregion

                #region if item
                case 8:  // item
                    result = (InGame.Party.ItemNumber(intParams[1]) > 0);
                    break;
                #endregion

                #region if weapon
                case 9:  // weapon
                    result = (InGame.Party.WeaponNumber(intParams[1]) > 0);
                    break;
                #endregion

                #region if armor
                case 10:  // armor
                    result = (InGame.Party.ArmorNumber(intParams[1]) > 0);
                    break;
                #endregion

                #region if button
                case 11:  // button
                    result = (Input.IsPressed(intParams[1]));
                    break;
                #endregion

                #region if EL
                case 12:  // script
                    // no scripting - result = eval(parameters[1]);
                    result = MakeCommand.LastCondition;
                    break;
                #endregion
            }
            // Store determinant results in hash
            branch[list[index].Indent].Result=result;
            // If determinant results are true
            if (result == true)
            {
                return true;
            }
            // If it doesn't meet the conditions: command skip
            return CommandSkip();
        }
    }
}

