using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Core.Damage
{
	/// <summary>
	/// Abstract class for any MonoBehaviours that can take damage
	/// </summary>
	public class DamageableBehaviour : MonoBehaviour
	{
        #region Variables

        /// <summary>
        /// The Damageable object
        /// </summary>
        public Damageable configuration;

        /// <summary>
        /// Gets whether this <see cref="DamageableBehaviour" /> is alive.
        /// </summary>
        /// <value>True if alive</value>
        public bool IsAlive
		{
			get { return configuration.IsAlive; }
            set
            {
                configuration.IsAlive = value;
            }
		}

        /// <summary>
        /// The position of the transform
        /// </summary>
        public virtual Vector3 Position
		{
			get { return transform.position; }
		}

        [BoxGroup("Testing Buttons", false)]
        [HorizontalGroup("Testing Buttons/Lives", width: 0.5f)]
        [Button]
        //[Button, PropertySpace(SpaceBefore = 10), HorizontalGroup("Life Group", LabelWidth = 2)]
        private void TakeLife()
        {
            configuration.SubtractLives(1, new DamageChangeInfo());
        }
        //[Button, HorizontalGroup("Life Group")]
        [HorizontalGroup("Testing Buttons/Lives", width: 0.5f)]
        [Button]
        private void GainLife()
        {
            configuration.AddLives(1, new DamageChangeInfo());
        }

        //[Button, PropertySpace(SpaceBefore = 5), HorizontalGroup("Damage Group", LabelWidth = 2)]
        [HorizontalGroup("Testing Buttons/Damage", width: 0.5f), PropertySpace(SpaceBefore = 5)]
        [Button]
        private void TakeDamage()
        {
            configuration.AddDamage(25, new HitInfo());
        }
        //[Button, HorizontalGroup("Damage Group")]
        [HorizontalGroup("Testing Buttons/Damage", width: 0.5f), PropertySpace(SpaceBefore = 5)]
        [Button]
        private void HealDamage()
        {
            configuration.SubtractDamage(25, new HitInfo());
        }

        #endregion

        #region Methods

        protected virtual void Awake()
        {
            configuration.Init();
            //configuration.Died += OnConfigurationDied;
        }

        public bool CanKnockback(IAlignmentProvider attackerAlignment)
        {
            var returnVal = attackerAlignment.CanHarm(configuration.AlignmentProvider);

            return returnVal;
        }

        #endregion
    }
}