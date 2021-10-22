using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/////////////////////////////////////////////////////////////////////////////////
//
//  FST_Script.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	NOTHING IS USED HERE ANY LONGER!!!
//
/////////////////////////////////////////////////////////////////////////////////






    //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^




[System.Obsolete("Please refer to GlobalGameManager.cs for any functionality that this script may have contained")]
public class OnlineGamePlayHandler : MonoBehaviour
{
    public static OnlineGamePlayHandler Instance;


      // default z positions of all object
      private const float posZ = -0.5f;
      private const float ballPosZ = -0.447f;

      public Transform[] MovingObjects;

      // array of delta position of all objects use to identify objects are moving or not. / array of all 10 players and ball
      private Vector3[] deltaPos;

      // array of all 10 players and ball
      private Vector3[] deltaRot;


    //  private bool isMatchStarted = false;

      [Header("Global")]
      private Vector3 startPos = Vector3.zero;
      public float DisconnectIfRoundTripMoreThan = 1f;
      public float SendRatePerSecond = 64f;
      public bool CalculateBasedOnPingIntervals = true;
      public bool CalculateBasedOnSendRate = true;
      [Tooltip("Has no effect if CalculateBasedOnSendRate is true")]
      public float LerpPerFrame = 0.5f;
      [Range(0.01f, 60f)]
      public float FramesPerSecond = 10f;
      public InterpolationType CurrentInterpolationType;
   //   private float interpolationPower = 0;

      private const int BALL_INDEX = 10;
      //    private readonly bool debug = false;
      //  private string PacketDebug { get { return " - Packet == " + packet; } }
      //  private int packet = 0;//for debug purpose ->FST
      public enum InterpolationType
      {
            CurrentToTargetUsingLerp,
            CurrentToTargetUsingMoveTowards
      }

   

      private void Awake()
      {
            //cannot have more than one this could issue on reconnect ->FST
            if (!Instance)
                  Instance = this;
            else Destroy(this);


        if (CalculateBasedOnPingIntervals)
            CalculateBasedOnSendRate = false;//safety check
    }
 

      /// <summary>
      /// swap the index(0 to 10)
      /// if index is 10 it is ball's index
      /// if index is less than 10 then its swap it, if index is greater than 4 then add 5 more otherwise substract 5.
      /// </summary>
      /// <returns>The index.</returns>
      /// <param name="index">Index of objects in array</param>
      private int SwapIndex(int index)
      {
            if (index == BALL_INDEX) // 10 index number is for ball 
                  return index;

        Debug.Log("<-------------------------- SILLY SWAP THAT NEEDS TO BE REMOVED AND REHANDLED -------------------------->");

        if (index > 4)
                  index -= 5;
            else index += 5;

            return index;
      }

    /// <summary>
    /// if match starts then match start boolean will became to true.
    /// </summary>
    public void StartMatch()
    {
        Debug.Log("<-------------------------- START MATCH -------------------------->");

        Instance.deltaPos = new Vector3[MovingObjects.Length];


     //   Instance.isMatchStarted = true;
   
    }

      public void CheckFirstPlayer(int IndexDisc)
      {
            //For assign First Player is Primary disc
            GlobalGameManager.Instance.Current2Player = Instance.MovingObjects[Instance.SwapIndex(IndexDisc)].gameObject.GetComponent<Rigidbody>();
            //	playerHandllers [swapIndex (IndexDisc)].gameObject.GetComponent <PlayerStats> ().PrimaryAttackingDisc = true;
            // for disable player stats eneble it when player stats enable
      }


      public void AssignPlayerNature(string StringDisc)
      {

           // string[] posData = StringDisc.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

            /*
                for (int i = 0; i < GlobalGameManager.SharedInstance.allPlayer.Length; i++) {

                      if (posData [2 + i * 2] == "Attack") {
                            playerHandllers [swapIndex (int.Parse (posData [1 + i * 2]))].gameObject.GetComponent <PlayerStats> ().isAttackingPlayer = true;
                      } else if (posData [2 + i * 2] == "Mid") {
                            playerHandllers [swapIndex (int.Parse (posData [1 + i * 2]))].gameObject.GetComponent <PlayerStats> ().isMidfieldPlayer = true;
                      } else if (posData [2 + i * 2] == "Defense") {
                            playerHandllers [swapIndex (int.Parse (posData [1 + i * 2]))].gameObject.GetComponent <PlayerStats> ().isDefensivePlayer = true;
                      } else if (posData [2 + i * 2] == "Box") {
                            playerHandllers [swapIndex (int.Parse (posData [1 + i * 2]))].gameObject.GetComponent <PlayerStats> ().isBoxToBoxPlayer = true;
                      }
                }
            */
            // for disable player stats eneble it when player stats enable

      }
      /// <summary>
      /// will allow data send if online match
      /// </summary>

}