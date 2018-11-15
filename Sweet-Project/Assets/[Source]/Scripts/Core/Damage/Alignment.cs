using System.Collections.Generic;
using UnityEngine;

namespace Core.Damage
{
	/// <summary>
	/// A scriptable object that defines which other alignments it can harm.
	/// </summary>
	[CreateAssetMenu(fileName = "Alignment.asset", menuName = "Game Settings/Alignment", order = 1)]
	public class Alignment : ScriptableObject, IAlignmentProvider
	{
		/// <summary>
		/// A collection of other alignment objects that we can harm
		/// </summary>
		public List<Alignment> opponents;

        /// <summary>
        /// Checks if you can harm a victim
        /// </summary>
        /// <param name="victim">The Alignment you're trying to harm</param>
        public bool CanHarm(IAlignmentProvider victim)
        {
            Alignment victimAlignment = victim as Alignment;

            return victimAlignment != null && opponents.Contains(victimAlignment);
        }
    }
}
