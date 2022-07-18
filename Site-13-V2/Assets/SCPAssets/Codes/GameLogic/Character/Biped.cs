using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class Biped : MonoBehaviour
    {
        public Transform Root;
        public float MaxHorizontalRootTolerance;
        #region Spine
        [Header("Spine")]
        public Transform HorizontalLevel0;
        public Transform HorizontalLevel1;
        public Transform VerticalLevel0;
        public Transform VerticalLevel1;
        #endregion
        #region Arm
        [Header("Arm")]
        public Transform L_Arm_T;
        public Transform R_Arm_T;
        public Collider L_Arm_C;
        public Collider R_Arm_C;
        #endregion

        #region Forearm
        [Header("Forearm")]
        public Transform L_Forearm_T;
        public Transform R_Forearm_T;
        public Collider L_Forearm_C;
        public Collider R_Forearm_C;
        #endregion

        #region Hand
        [Header("Hand")]
        public Transform L_Hand_T;
        public Transform R_Hand_T;
        public Collider L_Hand_C;
        public Collider R_Hand_C;
        #endregion

        #region Thigh
        [Header("Thigh")]
        public Transform L_Thigh_T;
        public Transform R_Thigh_T;
        public Collider L_Thigh_C;
        public Collider R_Thigh_C;
        #endregion

        #region Shin
        [Header("Shin")]
        public Transform L_Shin_T;
        public Transform R_Shin_T;
        public Collider L_Shin_C;
        public Collider R_Shin_C;
        #endregion


        #region Foot
        [Header("Foot")]
        public Transform L_Foot_T;
        public Transform R_Foot_T;
        public Collider L_Foot_C;
        public Collider R_Foot_C;
        #endregion

        /// <summary>
        /// _I for `immediately`.
        /// </summary>
        /// <param name="Rotation"></param>
        public void HeadTo_I(Vector3 Rotation)
        {

        }
        public void HeadTo_Smooth(Vector3 Target, float Delta)
        {

        }
    }
}
