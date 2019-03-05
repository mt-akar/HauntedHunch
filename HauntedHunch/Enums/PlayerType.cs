using System;

namespace HauntedHunch
{
    /// <summary>
    /// The type of player
    /// </summary>
    [Flags]
    public enum PlayerType
    {
        /// <summary>
        /// Starting player
        /// </summary>
        White = 0,

        /// <summary>
        /// Second starting player
        /// </summary>
        Black = 1
    }
}
