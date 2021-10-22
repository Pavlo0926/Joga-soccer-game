using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FST_PhotonRoomAndPlayerDebug : MonoBehaviour
{
    public string RoomCode = "MatchType";
    public string Filter = "oneonone";
    [TextArea(20, 100)]
    public string DebugMssg = "";
    public bool UseGGM = true;
    public bool UpdateNow = false;
    public bool AutoUpdate = false;
    [Range(0, 30)]
    public int UpdateInterval = 1;

    private float nextFire = 0;


    void Update()
    {
        if (UseGGM && GlobalGameManager.Instance)
        {
            DebugMssg = GlobalGameManager.Instance.GetCurrentProps().ToString();
            return;
        }

        string s = "No Room Infos";

        if ((AutoUpdate && Time.time > nextFire) || UpdateNow)
        {
            if (UpdateNow)
                AutoUpdate = UpdateNow = false;

            nextFire = Time.time + UpdateInterval;
            if (Photon.Pun.PhotonNetwork.InLobby)
                if (Photon.Pun.PhotonNetwork.GetCustomRoomList(FST_MPConnection.lobby, RoomCode + Filter))
                    Debug.Log("*******************************************************************************************************************");
        }

        Photon.Realtime.RoomInfo roomInfo = Photon.Pun.PhotonNetwork.CurrentRoom;
        if (roomInfo != null)
        {
            s= "****CURRENT ROOM****\nRoomName: " + roomInfo.Name + "\nOpen: " + roomInfo.IsOpen + "\nIsVisible: " + roomInfo.IsVisible + "\nRemovedFromList: " + roomInfo.RemovedFromList 
                + "\nMasterID: " + roomInfo.masterClientId + "\nPlayerCount: " + roomInfo.PlayerCount + "\nMaxPlayers: " + roomInfo.MaxPlayers + "\nRoomProps: " + roomInfo.CustomProperties + "\n ********************************* \n";
        }

        if (FST_MPConnection.Instance.RoomInfos.Count > 0)
        {
            if (s == "No Room Infos")
                s = "";

            for (int i = 0; i < FST_MPConnection.Instance.RoomInfos.Count; i++)
            {
                roomInfo = FST_MPConnection.Instance.RoomInfos[i];
                s += "****LISTED ROOM****\nRoomName: " + roomInfo.Name + "\nOpen: " + roomInfo.IsOpen + "\nIsVisible: " + roomInfo.IsVisible + "\n" + "RemovedFromList: " + roomInfo.RemovedFromList 
                    + "\nMasterID: " + roomInfo.masterClientId + "\nPlayerCount: " + roomInfo.PlayerCount + "\nMaxPlayers: " + roomInfo.MaxPlayers + "\nRoomProps: " + roomInfo.CustomProperties + "\n ********************************* \n";
            }
        }

        DebugMssg = s;
    }
}
