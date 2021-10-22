using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using FastSkillTeam;

[RequireComponent(typeof(PhotonView))]
public class FST_SuperSyncer : MonoBehaviourPun, IPunObservable
{
    [SerializeField] public Transform DiskTop;

    private Vector2 latestPos = Vector2.zero;
    private Vector2 latestVelocity = Vector2.zero;
    private float latestDiskTopYAngles = 0;
    Quaternion latestRotation;
    private Rigidbody m_Rigidbody;
    float lag;
    float currentTime;
    double lastPacketTime;
    double currentPacketTime;

    float diskTopYRotationsAtLastPacket = 0;
    Vector2 positionAtLastPacket = Vector2.zero;
    Vector2 velocityAtLastPacket = Vector2.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;

    PhotonStreamQueue queue = new PhotonStreamQueue(60);

    private void OnValidate()
    {
        if (photonView.OwnershipTransfer != OwnershipOption.Takeover)
            photonView.OwnershipTransfer = OwnershipOption.Takeover;

        if (photonView.ObservedComponents == null)
            photonView.ObservedComponents = new List<Component>() { this };

        if (!photonView.ObservedComponents.Contains(this))
            photonView.ObservedComponents.Add(this);

        if (photonView.Synchronization == ViewSynchronization.Off)
            photonView.Synchronization = ViewSynchronization.UnreliableOnChange;

        if (!DiskTop && transform.Find("top"))
            DiskTop = transform.Find("top");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (!FST_Gameplay.IsMultiplayer)
            return;
        if (stream.IsWriting)
        {
            hasData = false;
            queue.Serialize(stream);
        }
        else
        {
            if (!stream.IsReading)
                return;

            queue.Deserialize(stream);

            // Lag compensation
            lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            //usage > position += (velocity * lag);
            //   Debug.Log(lag);

            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            hasData = true;
        }
    }
    bool hasData = false;
    float fakeMoveSpeed = 10f;
    float fakeRotSpeed = 10f;
    bool useNetworkPrediction = true;
    // Update is called once per frame
    void Update()
    {
        if (!m_Rigidbody)
            m_Rigidbody = GetComponent<Rigidbody>();

        if (FST_DiskPlayerManager.Instance.IsOwner)
        {
            m_Rigidbody.isKinematic = false;
            if (!photonView.IsMine)
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            if (latestVelocity == (Vector2)m_Rigidbody.velocity && latestPos == (Vector2)transform.position && (DiskTop ? latestDiskTopYAngles == DiskTop.localRotation.eulerAngles.y : transform.rotation == latestRotation))
                return;

            // We own this player: send the others our data
            latestPos = transform.position;
            if (DiskTop)
                latestDiskTopYAngles = DiskTop.localRotation.eulerAngles.y;
            else latestRotation = transform.rotation;
            latestVelocity = m_Rigidbody.velocity;
            //latestAngularVelocities = new Vector3[count]/*byte[count][]*/;

            // here we send off the positions at the frame we call this function.
            queue.SendNext(latestPos);
            if (DiskTop)
                queue.SendNext(latestDiskTopYAngles);
            else queue.SendNext(latestRotation);
            queue.SendNext(latestVelocity);
            // stream.SendNext(latestAngularVelocities);
            return;
        }

        m_Rigidbody.isKinematic = true;

        if (!hasData)
            return;

        if (queue.HasQueuedObjects())
        {    // Network player, receive data
            latestPos = (Vector2)queue.ReceiveNext();
            if (DiskTop)
                latestDiskTopYAngles = (float)queue.ReceiveNext();
            else latestRotation = (Quaternion)queue.ReceiveNext();
            latestVelocity = (Vector2)queue.ReceiveNext();
            //latestAngularVelocities = (/*byte[][]*/Vector3[])stream.ReceiveNext();

            positionAtLastPacket = transform.position;
            if (DiskTop)
                diskTopYRotationsAtLastPacket = DiskTop.localRotation.eulerAngles.y;
            else rotationAtLastPacket = transform.rotation;
            velocityAtLastPacket = m_Rigidbody.velocity;

            //angularVelocitiesAtLastPacket = new Vector3[count];
        }

       // Lag compensation
        double timeToReachGoal = currentPacketTime - lastPacketTime;
        currentTime += Time.deltaTime;

        float interp = Time.deltaTime * fakeMoveSpeed;
        float rInterp = Time.deltaTime * fakeRotSpeed;
        //apply velocity
        m_Rigidbody.velocity = latestVelocity;
        //apply smoothed angular velocity
        // d.GetRigidbody.angularVelocity = latestAngularVelocities[i];//Vector3.Lerp(angularVelocitiesAtLastPacket[i], latestAngularVelocities[i]/*V3FromByte(latestAngularVelocities[i])*/, interp);

        //apply some network prediction using the velocity as the predictor
        if (useNetworkPrediction)
            transform.position += m_Rigidbody.velocity * Time.deltaTime;

        //turn our trimmed down position (Vector2) into a Vector3 with smoothing
        Vector2 p = Vector2.Lerp((Vector2)transform.position, latestPos, interp);
        //apply the calculated position to the disk
        transform.position = new Vector3(p.x, p.y, transform.position.z);

        if (DiskTop)
        {
            float y = Mathf.LerpAngle(DiskTop.localRotation.eulerAngles.y, latestDiskTopYAngles, rInterp);
            DiskTop.localRotation = Quaternion.Euler(y * Vector3.up);
        }
        else transform.rotation = Quaternion.Lerp(transform.rotation, latestRotation, rInterp);

       //  if ((Vector2)transform.position != positionAtLastPacket)
         //   FST_DiskPlayerManager.m_AllStopped = false;
    }
}
