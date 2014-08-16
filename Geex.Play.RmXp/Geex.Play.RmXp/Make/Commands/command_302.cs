using System.Collections.Generic;
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
        /// Shop Processing
        /// </summary>
		bool Command302()
        {
			// Set battle abort flag
			InGame.Temp.BattleAbort = true;
			// Set shop calling flag
			InGame.Temp.IsCallingShop = true;
			// Set goods list on new item
            InGame.Temp.ShopGoods = ToListOfArray(intParams, 2);
            // Loop
            index++;
            do
            {
                if (index >= list.Length)
                {
                    return false;
                }
                // If next event command has shop on second line or after
                if (list[index].Code == 605)
                {
                    // Add goods list to new item
                    foreach (int[] item in ToListOfArray(list[index].IntParams,2))
                    {
                        InGame.Temp.ShopGoods.Add(item);
                    }
                }
                // If event command does not have shop on second line or after
                else
                {
                    return false;
                }
                // Advance index
                index += 1;
            } while (true);
		}

        /// <summary>
        /// Flatten an array of Size Size
        /// </summary>
        /// <param Name="ints">list of ints</param>
        /// <param Name="Size">array Size to be returned</param>
        List<int[]> ToListOfArray(short[] ints, int size)
        {
            List<int[]> localList = new List<int[]>();
            for (int i = 0; i < ints.Length; i += size)
            {
                int[] temp = new int[size];
                for (int j = 0; j < size; j++)
                {
                    temp[j] = ints[i + j];
                }
                localList.Add(temp);
            }
            return localList;
        }
    }
}

