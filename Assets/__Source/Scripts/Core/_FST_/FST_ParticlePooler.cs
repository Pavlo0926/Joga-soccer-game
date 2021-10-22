using FastSkillTeam;
//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class FST_ParticlePooler : MonoBehaviour//Pun
{
    public static FST_ParticlePooler Instance;

    public ParticleSystem DiskToDisk;
    public ParticleSystem DiskToDiskHitDust;
    public ParticleSystem BallToWallDust;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        for (int i = 0; i < 5; i++)
        {
            ParticleSystem n = Instantiate(DiskToDisk, transform);
            n.gameObject.SetActive(false);

            n = Instantiate(DiskToDiskHitDust, transform);
            n.gameObject.SetActive(false);
        }
    }

    public void BallHitWallDust(Vector3 pos)
    {
        BallHitWallDustInternal(pos);

        //if (FST_Gameplay.IsMultiplayer)
        //    photonView.RPC("RPC_BallHitWallDust", RpcTarget.Others, pos);
    }

    //[PunRPC]
    //private void RPC_BallHitWallDust(Vector3 pos)
    //{
    //    BallHitWallDustInternal(pos);
    //}

    public void DiskHit(Transform disk, Vector3 contactPos)
    {
        int diskID = FST_DiskPlayerManager.Instance.GetDiskIdByTransform(disk);

        //   Debug.Log("diskhit index = " + diskID);

        DiskHitInternal(diskID, contactPos);

        //if (FST_Gameplay.IsMultiplayer)
        //    photonView.RPC("RPC_DiskHit", RpcTarget.Others, diskID, contactPos);
    }
    //[PunRPC]
    //private void RPC_DiskHit(int diskID, Vector3 contactPos)
    //{

    //    Debug.Log("got diskhit index = " + diskID);
    //    DiskHitInternal(diskID, contactPos);
    //}

    public void BallHitWallDustInternal(Vector3 pos)
    {
        bool b = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains(BallToWallDust.name))
            {
                GameObject g = transform.GetChild(i).gameObject;
                if (!g.activeSelf)
                {
                    g.transform.SetParent(null);
                    g.SetActive(true);
                    g.transform.position = pos;
                    g.transform.rotation = Quaternion.identity;
                    g.GetComponent<ParticleSystem>().Play();
                    b = true;
                    break;
                }
            }
        }

        if (!b)
        {
            ParticleSystem n = Instantiate(BallToWallDust);
            n.transform.position = pos;
            n.transform.rotation = Quaternion.identity;
            n.Play();
        }
    }
    
    private void DiskHitInternal(int diskIndex, Vector3 contactPos)
    {
        Transform disk = FST_DiskPlayerManager.Instance.GetDiskByID(diskIndex).Transform;

        bool b = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains(DiskToDisk.name))
            {
                GameObject g = transform.GetChild(i).gameObject;
                if (!g.activeSelf)
                {
                    g.SetActive(true);
                    g.transform.position = disk.position;
                    g.transform.rotation = Quaternion.identity;
                    g.GetComponent<ParticleSystem>().Play();
                    g.transform.SetParent(disk);
                    b = true;
                    break;
                }
            }
        }

        if (!b)
        {
            ParticleSystem n = Instantiate(DiskToDisk, disk);
            n.transform.position = disk.position;
            n.transform.rotation = Quaternion.identity;
            n.Play();
        }

        DiskHitDustInternal(contactPos);
    }

    private void DiskHitDustInternal(Vector3 pos)
    {
        bool b = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains(DiskToDiskHitDust.name))
            {
                GameObject g = transform.GetChild(i).gameObject;
                if (!g.activeSelf)
                {
                    g.transform.SetParent(null);
                    g.SetActive(true);
                    g.transform.position = pos;
                    g.transform.rotation = Quaternion.identity;
                    g.GetComponent<ParticleSystem>().Play();
                    b = true;
                    break;
                }
            }
        }

        if (!b)
        {
            ParticleSystem n = Instantiate(DiskToDiskHitDust);
            n.transform.position = pos;
            n.transform.rotation = Quaternion.identity;
            n.Play();
        }
    }
}
