using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using FastSkillTeam;

public class PlayerAI : MonoBehaviour
{
    ///*************************************************************************///
    /// Main Player AI class.
    /// why do we need a player AI class?
    /// This class has a reference to all player-1 (and player-2) units, their formation and their position
    /// and will be used to setup new formations for these units at the start of the game or when a goal happens.
    ///*************************************************************************///

    public Texture2D[] availableFlags;
    public Texture2D[] availableDownFlags;
    //array of all available teams

    public GameObject[] playerTeam;
    //array of all player-1 units
    public int playerFormation = 0;
    //player-1 formation
    public int playerTeamSelection;
    //player-1 team flag

    //for two player game
    public GameObject[] player2Team;
    //array of all player-2 units
    public int player2Formation;
    //player-2 formation
    public int player2Flag;
    //player-2 team flag

    public static PlayerAI instance;


    private void Awake()
    {
        instance = this;
    }

    private void Init()
    {
        //fetch player_1's flag
        playerTeamSelection = FST_SettingsManager.Team;

        player2Flag = 2;

        for (int i = 0; i < player2Team.Length; i++)
        {
            if (player2Team[i] == null)
            {
                Debug.LogWarning("FST WARNING NOTIFICATION -> no player2Team[i] to be used at this point");
                break;
            }
            player2Team[i].GetComponent<Renderer>().material.mainTexture = availableFlags[player2Flag];
            player2Team[i].transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = availableDownFlags[player2Flag];
        }

/*
        int j = 1;
        foreach (GameObject unit in player2Team)
        {
            //Optional
            unit.name = "Player2Unit-" + j;
            unit.GetComponent<Player2Controller>().unitIndex = j;
            unit.GetComponent<Renderer>().material.mainTexture = availableFlags[player2Flag];
            unit.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = availableDownFlags[player2Flag];
            j++;
        }
*/

    }

    private void Start()
    {
        int matchType = FST_SettingsManager.MatchType;
        if (matchType == 0|| matchType ==  1 || matchType == 4)//NOTE: !FST_Gameplay.IsMultiplayer will be cheaper here, double check matchtypes...
        {
            SetFormation();
        }
    }

    public void SetFormation()
    {
        Init();

        if (FST_Gameplay.IsMultiplayer)
        {
            if (PhotonNetwork.MasterClient.CustomProperties.TryGetValue(FST_PlayerProps.FORMATION, out object o))
            {
                playerFormation = (int)o;
            //    Debug.Log("PlayerAI.SetFormation() > Got PlayerProps > Formation = " + playerFormation);
            }

            if (GlobalGameManager.Instance.RemotePlayer.CustomProperties.TryGetValue(FST_PlayerProps.FORMATION, out o))
            {
                player2Formation = (int)o;
             //   Debug.Log("PlayerAI.SetFormation() Got Player2Props > Formation = " + player2Formation);
            }

            if (!GlobalGameManager.isPenaltyKick)
            {
                StartCoroutine(ChangeFormation(playerTeam, playerFormation, 1, 1));//NOTE: invert this and it will work with new PUN formation system better, as we dont need to flip shit everywhere any longer.
                StartCoroutine(ChangeFormation(player2Team, player2Formation, 1, -1));
            }
            else
            {
                StartCoroutine(GoToPosition(playerTeam, GlobalGameManager.playerDestination, 1));
                StartCoroutine(GoToPosition(player2Team, GlobalGameManager.AIDestination, 1));
            }
        }
        else
        {
            playerFormation = FST_SettingsManager.Formation;
            player2Formation = FST_SettingsManager.FormationOpponent;

         //   Debug.Log("PlayerAI.SetFormation() Got Player2Props > Formation = " + player2Formation);

            if (!GlobalGameManager.isPenaltyKick)
                StartCoroutine(ChangeFormation(playerTeam, playerFormation, 1, 1));
            else StartCoroutine(GoToPosition(playerTeam, GlobalGameManager.playerDestination, 1));

            //For two-player mode,
            if (GlobalGameManager.MatchType == 1)
            {
                if (!GlobalGameManager.isPenaltyKick)
                    StartCoroutine(ChangeFormation(player2Team, player2Formation, 1, -1));
                else StartCoroutine(GoToPosition(player2Team, GlobalGameManager.AIDestination, 1));
            }
        }
    }

    /// <summary>
    /// take all units, selected formation and side of the player (left half or right half)
    /// and then position each unit on it's destination.
    /// speed is used to fasten the translation of units to their destinations.
    /// </summary>
    /// <param name="_team">the side to change</param>
    /// <param name="_formationIndex">the formation to apply</param>
    /// <param name="_speed">how fast the units move to formation</param>
    /// <param name="_dir">direction to move -1 or 1</param>
    /// <returns></returns>
    private IEnumerator ChangeFormation(GameObject[] _team, int _formationIndex, float _speed, int _dir)
    {
        //cache the initial position of all units
        List<Vector3> unitsSartingPosition = new List<Vector3>();
        foreach (GameObject unit in _team)
        {
            unitsSartingPosition.Add(unit.transform.position); //get the initial postion of this unit for later use.
            unit.GetComponent<MeshCollider>().enabled = false;    //no collision for this unit till we are done with re positioning.
        }

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * _speed;

            for (int cnt = 0; cnt < _team.Length; cnt++)
                _team[cnt].transform.position = new Vector3(FormationManager.GetPositionInFormation(_formationIndex, cnt).x * _dir, FormationManager.GetPositionInFormation(_formationIndex, cnt).y, FormationManager.fixedZ);

            yield return 0;
        }

        foreach (GameObject unit in _team)
            unit.GetComponent<MeshCollider>().enabled = true; //collision is now enabled.
    }


    /// <summary>
    ///  moves the unit to desired position
    /// </summary>
    /// <param name="unit">the array of GameObjects to move</param>
    /// <param name="pos">the target position</param>
    /// <param name="_speed">the speed that the unit/s should move</param>
    /// <returns></returns>
    public IEnumerator GoToPosition(GameObject[] unit, Vector3 pos, float _speed)
    {
        Debug.Log("IEnumarator goToPosition");

        Vector3 unitsSartingPosition = unit[0].transform.position;
        unit[0].GetComponent<SphereCollider>().enabled = false;    //no collision for this unit until we are done with re positioning.

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * _speed;
            unit[0].transform.position = new Vector3(Mathf.SmoothStep(unitsSartingPosition.x, pos.x, t),
                Mathf.SmoothStep(unitsSartingPosition.y, pos.y, t),
                unitsSartingPosition.z);

            yield return 0;
        }
        unit[0].GetComponent<SphereCollider>().enabled = true;
    }
}