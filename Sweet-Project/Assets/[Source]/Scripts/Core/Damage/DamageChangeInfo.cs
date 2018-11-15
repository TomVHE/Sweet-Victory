using UnityEngine;

namespace Core.Damage
{
	/// <summary>
	/// Damage change info - stores information about changesin Damage
	/// </summary>
	public struct DamageChangeInfo
	{
		public Damageable damageable;

        public int playerID;

        public float oldDamage, newDamage;

        public int oldLives, newLives;

		public IAlignmentProvider alignment;

		public float Difference
		{
			get { return newDamage - oldDamage; }
		}

        public float AbsoluteDifference
		{
			get { return Mathf.Abs(Difference); }
		}
	}
}