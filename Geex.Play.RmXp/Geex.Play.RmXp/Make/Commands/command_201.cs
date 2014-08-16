using System;
using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Transfer Player
        /// </summary>
        bool Command201()
        {
            // If in battle
            if (InGame.Temp.IsInBattle) return true;
            // If transferring player, showing message, or processing Transition
            if (InGame.Temp.IsTransferringPlayer || InGame.Temp.IsMessageWindowShowing || InGame.Temp.IsProcessingTransition) return false;
            // Set transferring player flag
            InGame.Temp.IsTransferringPlayer = true;
            // If appointment method is [direct appointment]
            if (intParams[0] == 0)
            {
                // Set player move destination
                InGame.Temp.PlayerNewMapId = intParams[1];
                InGame.Temp.PlayerNewX = intParams[2];
                InGame.Temp.PlayerNewY = intParams[3];
                InGame.Temp.PlayerNewDirection = intParams[4];
            }
            // If appointment method is [appoint with variables]
            else
            {
                // Set player move destination
                InGame.Temp.PlayerNewMapId = InGame.Variables.Arr[intParams[1]];
                InGame.Temp.PlayerNewX = InGame.Variables.Arr[intParams[2]];
                InGame.Temp.PlayerNewY = InGame.Variables.Arr[intParams[3]];
                InGame.Temp.PlayerNewDirection = intParams[4];
            }
            // Advance index
            index += 1;
            // If fade is set
            if (intParams[5] == 0)
            {
                // Prepare for Transition
                //Graphics.Freeze();
                // Set Transition processing flag
                InGame.Temp.IsProcessingTransition = true;
                InGame.Temp.TransitionName = "";
            }
            return false;
        }
    }
}

