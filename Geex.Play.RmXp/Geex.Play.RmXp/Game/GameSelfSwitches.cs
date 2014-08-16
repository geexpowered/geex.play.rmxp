using System;
using System.Collections.Generic;
using Geex.Run;


namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class handles game self switches
    /// </summary>
    public partial class GameSelfSwitches : GeexDictionary<GameSwitch, bool>
    {
        #region Variables
        /// <summary>
        /// True if GameSwitch is set to true
        /// </summary>
        /// <param Name="sw"></param>
        /// <returns></returns>
        public new bool this[GameSwitch sw]
        {
            get
            {
                if (ContainsKey(sw))
                {
                    return base[sw];
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (ContainsKey(sw))
                {
                    base[sw]=value;
                }
                else
                {
                    base.Add(sw, value);
                }
            }
        }
        #endregion

        /*#region Methods
        /// <summary>
        /// The Node as a reference to hastable. XML syntax: (ArrayOfNode)(node)(key)key(/key)(value)(object)object(/object)(/value)n(/node)
        /// </summary>
        public partial class Node
        {
            /// <summary>
            /// The Node as a reference to Dictionary
            /// </summary>
            public Node()
            { }
            /// <summary>
            /// The Node as a reference to Dictionary
            /// </summary>
            /// <param Name="k">nodeKey</param>
            /// <param Name="v">nodeValue</param>
            public Node(GameSwitch  nodeKey, bool nodeValue)
            {
                key = nodeKey;
                value = nodeValue;
            }
            /// <summary>
            /// Node Key
            /// </summary>
            public GameSwitch key;
            /// <summary>
            /// Node value
            /// </summary>
            public bool value;
        }
        #endregion*/
    }
}

