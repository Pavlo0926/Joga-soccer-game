/////////////////////////////////////////////////////////////////////////////////
//
//  FST_PhysicData.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	Edit all variables in Inspector, DO NOT CHANGE THESE DEFAULTS!
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace FastSkillTeam
{
    [CreateAssetMenu(fileName = "FST_PhysicsData", menuName = "FST_Physics/FST_PhysicsData", order = 1)]
    public class FST_PhysicsData : ScriptableObject
    {
        [Header("Disk Base Properties")]
        //rigidbody props
        public float DiskMass = 1.7f;
        public float DiskDrag = 0.3f;
        public float DiskAngularDrag = 0.05f;
        public float DiskMaxVelocity = 50f;
        public float DiskMaxAngularVelocity = 7f;

        public float DiskBounciness = 0.78555f;
        public float DiskStaticFriction= 0.3f;
        public float DiskDynamicFriction = 2f;

        public CollisionDetectionMode DiskCollisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        public PhysicMaterialCombine DiskFrictionCombineMode = PhysicMaterialCombine.Multiply;
        public PhysicMaterialCombine DiskBounceCombineMode = PhysicMaterialCombine.Maximum;

        [Header("Disk Stop Properties")]
        public float DiskStopThreshold_Velocity = 0.15f;
        public float DiskStopThreshold_AngularVelocity = 0.5f;
        public float DiskStopThreshold_Torque = 0.5f;

        [Header("Ball Base Properties")]
        //rigidbody props
        public float BallMaxVelocity = 60f;
        public float BallMaxAngularVelocity = 360f;
        public float BallMass = 0.5f;//410–450 g in real life
        public float BallDrag = 1f;
        public float BallAngularDrag = 0.4f;
        //physical material props
        public float BallStaticFriction = 0.9f;
        public float BallDynamicFriction = 0.9f;
        public float BallBounciness = 1f;
        public PhysicMaterialCombine BallFrictionCombineMode = PhysicMaterialCombine.Average;
        public PhysicMaterialCombine BallBounceCombineMode = PhysicMaterialCombine.Average;

        [Header("Ball Stop Properties")]
        public float BallStopThreshold_Velocity = 0.15f;
        public float BallStopThreshold_AngularVelocity = 0.5f;
        public float BallStopThreshold_Torque = 0.5f;

        [Header("Extra Mechanics Properties")]
        public float BallInCornerForceMultiplier = 1.8f;
        public float DiskToDiskForceMultiplier = 1.2f;
        public float DiskToBallForceMultiplier = 1.2f;
        [Tooltip("in order to activate the curve shot button,\nthe ball must be this close the the border at very least")]
        public float CurveShotActivationRange = 0.65f; // the ball must be this close the the border at very least to activate the curve shot button
        public float CurveShotForceMultiplier = 1.2f;

        [Header("Border Properties")]
        //physical material props
        public float BorderStaticFriction = 0.9f;
        public float BorderDynamicFriction = 0.9f;
        public float BorderBounciness = 1f;
        public PhysicMaterialCombine BorderFrictionCombineMode = PhysicMaterialCombine.Average;
        public PhysicMaterialCombine BorderBounceCombineMode = PhysicMaterialCombine.Average;

        public PhysicMaterial BorderPhysMat;

        [Header("Ground Properties")]
        //physical material props
        public float GroundStaticFriction = 0.15f;
        public float GroundDynamicFriction = 0.15f;
        public float GroundBounciness = 0f;
        public PhysicMaterialCombine GroundFrictionCombineMode = PhysicMaterialCombine.Average;
        public PhysicMaterialCombine GroundBounceCombineMode = PhysicMaterialCombine.Minimum;

        public PhysicMaterial GroundPhysMat;

        [Header("Ball Field Bounds")]
        /// <summary>
        /// Borders <br></br>
        /// x = top, y = right, z = bottom, w = left
        /// </summary>
        [Tooltip("x = top, y = right, z = bottom, w = left")]
        public Vector4 FieldBoundsBall = new Vector4(6.81015f, 14.65f, -9.40395f, -14.65f);//DO NOT MODIFY
        public float GoalMouthTopBall = 1.82f;//DO NOT MODIFY
        public float GoalMouthBottomBall = -4.43f;//DO NOT MODIFY
        public float LeftGoalNetBackBall = -16.706f;//DO NOT MODIFY
        public float RightGoalNetBackBall = 16.706f;//DO NOT MODIFY


        [Header("Disk Field Bounds")]
        /// <summary>
        /// Borders <br></br>
        /// x = top, y = right, z = bottom, w = left
        /// </summary>
        [Tooltip("x = top, y = right, z = bottom, w = left")]
        public Vector4 FieldBoundsDisk = new Vector4(6.32f, 14.1f, -8.9f, -14.1f);//DO NOT MODIFY
        public float GoalMouthTopDisk = 2.4f;//DO NOT MODIFY
        public float GoalMouthBottomDisk = -5.15f;//DO NOT MODIFY
        public float LeftGoalNetBackDisk = -16.18f;//DO NOT MODIFY
        public float RightGoalNetBackDisk = 16.18f;//DO NOT MODIFY

        [Header("Audio and VFX thresholds")]
        public float HardDiskForce = 15f;
        public float HardBallForce = 15f;
    }
}