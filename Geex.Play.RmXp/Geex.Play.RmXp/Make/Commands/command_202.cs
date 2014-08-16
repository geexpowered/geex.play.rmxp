using Geex.Play.Rpg.Game;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Set Event Location
        /// </summary>
        bool Command202()
        {
            // If in battle
            if (InGame.Temp.IsInBattle) return true;
            // Get Character
            GameCharacter character = GetCharacter(intParams[0]);
            // If no Character exists
            if (character == null) return true;
            // If appointment method is [direct appointment]
            if (intParams[1] == 0)
            {
                // Set Character position
                character.Moveto(intParams[2]*32+16, intParams[3]*32+32);
                // If appointment method is [appoint with variables]
            }
            else if (intParams[1] == 1)
            {
                // Set Character position
                character.Moveto(InGame.Variables.Arr[intParams[2]] * 32 + 16, InGame.Variables.Arr[intParams[3]] * 32 + 32);
            }
            // If appointment method is [exchange with another event]
            else
            {
                int old_x = character.X;
                int old_y = character.Y;
                GameCharacter character2 = GetCharacter(intParams[2]);
                if (character2 != null)
                {
                    character.Moveto(character2.X, character2.Y);
                    character2.Moveto(old_x, old_y);
                }
            }
            // Set Character direction
            switch (intParams[4])
            {
                case 8:    // up
                    character.TurnUp();
                    break;
                case 6:    // right
                    character.TurnRight();
                    break;
                case 2:    // down
                    character.TurnDown();
                    break;
                case 4:    // left
                    character.TurnLeft();
                    break;
            }
            return true;
        }
    }
}

