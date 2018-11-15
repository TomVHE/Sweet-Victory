using UnityEngine;

namespace Core.Damage
{
    /// <summary>
    /// Damage info - a class required by some damage listeners
    /// </summary>
    public struct HitInfo
    {
        /// <summary>
        /// Gets or sets the damage change info.
        /// </summary>
        /// <value>The damage change info.</value>
        public DamageChangeInfo damageInfo;

        /// <summary>
        /// Gets or sets the damage point.
        /// </summary>
        /// <value>The damage point.</value>
        public Vector3 damagePoint;

        //public int playerID;

        /// <summary>
        /// Initializes a new instance of the <see cref="HitInfo" /> struct.
        /// </summary>
        /// <param name="info">The health change info</param>
        /// <param name="damagePoint">Damage point.</param>
        public HitInfo(DamageChangeInfo info, Vector3 point)//, int id)
        {
            damageInfo = info;
            damagePoint = point;
            //playerID = id;
        }


        /*
        /// <summary>
        /// Gets or sets the damage change info.
        /// </summary>
        /// <value>The damage change info.</value>
        public DamageChangeInfo DamageChangeInfo
        {
            get;
        }

        /// <summary>
        /// Gets or sets the damage point.
        /// </summary>
        /// <value>The damage point.</value>
        public Vector3 DamagePoint
        {
            get;
        }

        public int PlayerID
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HitInfo" /> struct.
        /// </summary>
        /// <param name="info">The health change info</param>
        /// <param name="damagePoint">Damage point.</param>
        public HitInfo(DamageChangeInfo info, Vector3 damagePoint, int id)
        {
            DamageChangeInfo = info;
            DamagePoint = damagePoint;
            PlayerID = id; 
        }
        */

    }
}