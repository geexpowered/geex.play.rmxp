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
        /// Control Variables
        /// </summary>

        bool Command122()
        {
            // Initialize value
            int value = 0;
            // Branch with operand
            switch (intParams[3])
            {
                case 0:  // invariable
                    value = intParams[4];
                    break;
                case 1: // variable
                    value = InGame.Variables.Arr[intParams[4]];
                    break;
                case 2:  // random number
                    value = intParams[4] + InGame.Rnd.Next(intParams[5] - intParams[4] + 1);
                    break;
                case 3:  // item
                    value = InGame.Party.ItemNumber(intParams[4]);
                    break;
                case 4:  // actor
                    GameActor actor = InGame.Actors[intParams[4]-1];
                    if (actor != null)
                    {
                        switch (intParams[5])
                        {
                            case 0:  // level
                                value = actor.Level;
                                break;
                            case 1:  // EXP
                                value = actor.Exp;
                                break;
                            case 2:  // HP
                                value = actor.Hp;
                                break;
                            case 3:  // SP
                                value = actor.Sp;
                                break;
                            case 4:  // MaxHp
                                value = actor.MaxHp;
                                break;
                            case 5:  // MaxSp
                                value = actor.MaxSp;
                                break;
                            case 6:  // strength
                                value = actor.Str;
                                break;
                            case 7:  // dexterity
                                value = actor.Dex;
                                break;
                            case 8:  // agility
                                value = actor.Agi;
                                break;
                            case 9:  // intelligence
                                value = actor.Intel;
                                break;
                            case 10:  // attack power
                                value = actor.Atk;
                                break;
                            case 11:  // physical defense
                                value = actor.Pdef;
                                break;
                            case 12:  // magic defense
                                value = actor.Mdef;
                                break;
                            case 13:  // evasion
                                value = actor.Eva;
                                break;
                        }
                    }
                    break;

                case 5:  // enemy
                    GameNpc enemy = InGame.Troops.Npcs[intParams[4]];
                    if (enemy != null)
                    {
                        switch (intParams[5])
                        {
                            case 0:  // HP
                                value = enemy.Hp;
                                break;
                            case 1:  // SP
                                value = enemy.Sp;
                                break;
                            case 2:  // MaxHp
                                value = enemy.MaxHp;
                                break;
                            case 3:  // MaxSp
                                value = enemy.MaxSp;
                                break;
                            case 4:  // strength
                                value = enemy.Str;
                                break;
                            case 5:  // dexterity
                                value = enemy.Dex;
                                break;
                            case 6:  // agility
                                value = enemy.Agi;
                                break;
                            case 7:  // intelligence
                                value = enemy.Intel;
                                break;
                            case 8:  // attack power
                                value = enemy.Atk;
                                break;
                            case 9:  // physical defense
                                value = enemy.Pdef;
                                break;
                            case 10:  // magic defense
                                value = enemy.Mdef;
                                break;
                            case 11:  // evasion correction
                                value = enemy.Eva;
                                break;
                        }
                    }
                    break;

                case 6:  // Character
                    if (intParams[4] == -1)
                    {
                        value = ChangeGamePlayer();
                    }
                    else
                    {
                        GameCharacter character = GetCharacter(intParams[4]);
                        value = ChangeGameEvent(ref character);
                    }
                    break;

                case 7:  // other
                    switch (intParams[4])
                    {
                        case 0:  // map ID
                            value = InGame.Map.MapId;
                            break;
                        case 1:  // number of party members
                            value = InGame.Party.Actors.Count;
                            break;
                        case 2:  // gold
                            value = InGame.Party.Gold;
                            break;
                        case 3:  // steps
                            value = InGame.Party.Steps;
                            break;
                        case 4:  // play time
                            value = Graphics.FrameCount / Graphics.FrameRate;
                            break;
                        case 5:  // Timer
                            value = InGame.System.Timer / Graphics.FrameRate;
                            break;
                        case 6:  // save count
                            value = InGame.System.SaveCount;
                            break;
                    }
                    break;
            }
            // Loop for group control
            for (int i=intParams[0];i<=intParams[1];i++)
            {
                // Branch with control
                switch (intParams[2])
                {
                    case 0:  // substitute
                        InGame.Variables.Arr[i] = value;
                        break;
                    case 1:  // add
                        InGame.Variables.Arr[i] += value;
                        break;
                    case 2:  // subtract
                        InGame.Variables.Arr[i] -= value;
                        break;
                    case 3:  // multiply
                        InGame.Variables.Arr[i] *= value;
                        break;
                    case 4:  // divide
                        if (value != 0)
                        {
                            InGame.Variables.Arr[i] /= value;
                        }
                        break;
                    case 5:  // remainder
                        if (value != 0) InGame.Variables.Arr[i] %= value;
                        break;
                }
                // Maximum limit check
                if (InGame.Variables.Arr[i] > 99999999) InGame.Variables.Arr[i] = 99999999;
                // Minimum limit check
                if (InGame.Variables.Arr[i] < -99999999) InGame.Variables.Arr[i] = -99999999;
                // Refresh map
                InGame.Map.IsNeedRefresh = true;
            }
            // Continue
            return true;
        }
    }
}