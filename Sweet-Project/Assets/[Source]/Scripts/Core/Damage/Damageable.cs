using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Core.Damage
{
    /// <summary>
    /// Damageable class for handling damage using events
    /// Could be used on Players or enemies or even destructable world objects
    /// </summary>
    [Serializable]
    public class Damageable
    {
        #region Variables

        #region Serialized

        [DisplayAsString]
        public int playerID; //EDIT

        #region Damage

        [PropertyRange("minDamage", "maxDamage"), FoldoutGroup("Damage", expanded: false)]
        [ProgressBar(0, "maxDamage", ColorMember = "DamageBarColour"), HideLabel]
        [SerializeField] private int currentDamage = 100;

        [MinValue(0), FoldoutGroup("Damage", expanded: false), GUIColor("IsAliveColour")]
        [SerializeField] private int minDamage = 50;
        [MinValue(0), FoldoutGroup("Damage", expanded: false), GUIColor("IsAliveColour")]
        [SerializeField] private int maxDamage = 999;

        [PropertySpace(SpaceBefore = 5)]
        [MinValue(0), FoldoutGroup("Damage", expanded: false), GUIColor("IsAliveColour")]
        [SerializeField] private int startingDamage = 100;

        #endregion

        #region Lives

        [PropertyRange(0, "maxLives"), FoldoutGroup("Lives", expanded: false)]
        [ProgressBar(0, "maxLives", ColorMember = "LiveBoxesColour", Segmented = true), HideLabel]
        [SerializeField] private int currentLives = 3;

        [MinValue(0), FoldoutGroup("Lives", expanded: false), GUIColor("IsAliveColour")]
        [SerializeField] private int maxLives = 3;

        [PropertySpace(SpaceBefore = 5)]
        [PropertyRange(0, "maxLives"), FoldoutGroup("Lives", expanded: false), GUIColor("IsAliveColour")]
        [SerializeField] private int startingLives = 3;

        #endregion

        [SerializeField] private Alignment alignment;

        [SceneObjectsOnly]
        public GameObject materialObject;

        #endregion

        #region Non-Serialized

        public int CurrentLives
        {
            get
            {
                return currentLives;
            }
            protected set
            {
                currentLives = value;
            }
        }

        public int CurrentDamage
        {
            get
            {
                return currentDamage;
            }
            protected set
            {
                currentDamage = value;
            }
        }

        /// <summary>
        /// Gets the normalised damage percentage. (0.1 = 10% 1 = 100%)
        /// </summary>
        public float DamagePercentage
		{
			get
			{
				if (Math.Abs(maxDamage) <= Mathf.Epsilon)
				{
					Debug.LogError("Max Damage is 0");
					maxDamage = 1;
				}
                return ((float)CurrentDamage / (float)maxDamage);
            }
		}

        /// <summary>
        /// Gets the normalised live percentage. (0.1 = 10% 1 = 100%)
        /// </summary>
        public float LivesPercentage
        {
            get
            {
                if (Math.Abs(maxLives) <= Mathf.Epsilon)
                {
                    Debug.LogError("Max Lives is 0");
                    maxLives = 1;
                }
                return ((float)CurrentLives / (float)maxLives);
            }
        }

        public Alignment AlignmentProvider
		{
			get
			{
                return alignment;
                //return alignment?.GetInterface();
                //return alignment?.GetType();
            }
            set
            {
                alignment = value;
            }
		}

        /// <summary>
        /// Gets whether this instance is alive.
        /// </summary>
        public bool IsAlive { get; set; }

        #if UNITY_EDITOR
        private Color DamageBarColour
        {
            get
            {
                return Color.Lerp(Color.green, Color.red, DamagePercentage);
            }
        }

        private Color LiveBoxesColour
        {
            get
            {
                return Color.Lerp(Color.red, Color.green, LivesPercentage);
                //return Color.Lerp(Color.red, Color.green, _);
            }
        }

        private Color IsAliveColour()
        {
            Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
            //return IsAlive ? Color.white : new Color(0.9f, 0.25f, 0.2f);
            return Color.white;
        }
        #endif

        #endregion

        #region Events

        public event Action<HitInfo> Damaged, Healed;
        public event Action<DamageChangeInfo> LostLife, GainedLife, LivesChanged, DamageChanged, Finished;

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Initialize instance
        /// </summary>
        public virtual void Init()
		{
			CurrentDamage = startingDamage;

		}

        #region Lives

        public void SetLives(int lives)
        {
            var info = new DamageChangeInfo
            {
                damageable = this,
                newLives = lives,
                oldLives = CurrentLives
            };

            CurrentLives = lives;

            if (CurrentLives == 0)
            {
                Debug.Log("Finished");
                Finished?.Invoke(info);
            }

            LivesChanged?.Invoke(info);
        }

        public void SubtractLives(int liveSubtraction, DamageChangeInfo info)
        {
            if (CurrentLives == 0)
            {
                Debug.Log("Damageable - Finished");
                Finished?.Invoke(info);
            }

            info.oldLives = CurrentLives;

            CurrentLives -= Mathf.Abs(liveSubtraction);
            CurrentLives = Mathf.Clamp(CurrentLives, 0, maxLives);

            info.newLives = CurrentLives;

            if(CurrentLives == 0)
            {
                Debug.Log("Damageable - Finished");
                Finished?.Invoke(info);
            }

            LostLife?.Invoke(info);
        }

        public void AddLives(int liveAddition, DamageChangeInfo info)
        {
            info.oldLives = CurrentLives;

            CurrentLives += Mathf.Abs(liveAddition);
            CurrentLives = Mathf.Clamp(CurrentLives, 0, maxLives);

            info.newLives = CurrentLives;

            GainedLife?.Invoke(info);
        }

        public void ResetLives()
        {
            SetLives(startingLives);
        }

        #endregion

        #region Damage

        public void CheckDamage(int damageAddition, IAlignmentProvider damageAlignment, Vector3 hitPosition)
        {
            DamageChangeInfo info = new DamageChangeInfo();

            HitInfo hitInfo = new HitInfo(info, hitPosition);

            CheckDamage(damageAddition, damageAlignment, hitPosition, out hitInfo);
        }
        /// <summary>
        /// Use the alignment to see if adding damage is a valid action
        /// </summary>
        /// <param name="damageAddition">
        /// The damage to add
        /// </param>
        /// <param name="damageAlignment">
        /// The alignment of the other combatant
        /// </param>
        /// <param name="output">
        /// The output data if there is damage taken
        /// </param>
        /// <returns>
        /// <value>true if this instance added damage</value>
        /// <value>false if this instance was already dead, or the alignment did not allow the damage</value>
        /// </returns>
        public bool CheckDamage(int damageAddition, IAlignmentProvider damageAlignment, Vector3 hitPosition, out HitInfo output)
        {
            var info = new DamageChangeInfo
            {
                alignment = damageAlignment,
                damageable = this,
                newDamage = CurrentDamage,
                oldDamage = CurrentDamage
            };

            output = new HitInfo
            {
                damageInfo = info,
                damagePoint = hitPosition
            };

            if(damageAlignment == null || AlignmentProvider == null)
            {
                return false;
            }

            bool canDamage = damageAlignment.CanHarm(AlignmentProvider);

            //if (!IsAlive || !canDamage)
            if(IsAlive == false || canDamage == false)
            {
                return false;
            }

            AddDamage(+damageAddition, output);
            SafelyDoAction(Damaged, output);
            return true;
        }

        /// <summary>
        /// Sets instance's damage directly.
        /// </summary>
        /// <param name="damage">
        /// The value to set <see cref="currentHealth"/> to
        /// </param>
        public void SetDamage(int damage)
        {
            var dmgChangeInfo = new DamageChangeInfo
            {
                damageable = this,
                newDamage = damage,
                oldDamage = CurrentDamage
            };

            /*
            var hitInfo = new HitInfo
            {
                damageChangeInfo = dmgChangeInfo,
                damagePoint = Vector3.zero
            };*/

            CurrentDamage = damage;

            DamageChanged?.Invoke(dmgChangeInfo);
        }

        public void AddDamage(int damageAddition, HitInfo hit)
        {
            hit.damageInfo.oldDamage = CurrentDamage;

            CurrentDamage += Mathf.Abs(damageAddition);
            CurrentDamage = Mathf.Clamp(CurrentDamage, 0, maxDamage);

            hit.damageInfo.oldDamage = CurrentDamage;

            Damaged?.Invoke(hit);
        }

        public void SubtractDamage(int damageSubtraction, HitInfo hit)
        {
            hit.damageInfo.oldDamage = CurrentDamage;

            CurrentDamage -= Mathf.Abs(damageSubtraction);
            CurrentDamage = Mathf.Clamp(CurrentDamage, 0, maxDamage);

            hit.damageInfo.oldDamage = CurrentDamage;

            Healed?.Invoke(hit);
        }

        public void ResetDamage()
        {
            SetDamage(startingDamage);
        }

        #endregion

        /// <summary>
        /// A helper method for null checking actions
        /// </summary>
        /// <param name="action">Action to be done</param>
        protected void SafelyDoAction(Action action)
		{
            action?.Invoke();
        }
		/// <summary>
		/// A helper method for null checking actions
		/// </summary>
		/// <param name="action">Action to be done</param>
		/// <param name="info">The DamageChangeInfo to be passed to the Action</param>
		protected void SafelyDoAction(Action<DamageChangeInfo> action, DamageChangeInfo info)
		{
            action?.Invoke(info);
        }
        /// <summary>
        /// A helper method for null checking actions
        /// </summary>
        /// <param name="action">Action to be done</param>
        /// <param name="info">The DamageChangeInfo to be passed to the Action</param>
        protected void SafelyDoAction(Action<HitInfo> action, HitInfo info)
        {
            action?.Invoke(info);
        }

        #endregion
    }
}