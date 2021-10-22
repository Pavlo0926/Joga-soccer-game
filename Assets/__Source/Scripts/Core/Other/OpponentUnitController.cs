using UnityEngine;
using FastSkillTeam;

public class OpponentUnitController : MonoBehaviour
{
    ///*************************************************************************///
    /// Unit controller class for AI units
    ///*************************************************************************///

    internal int unitIndex;
    //every AI unit has an index. this is for the AI controller to know which unit must be selected.
    public bool isStayonBorderO;
    //Indexes are given to units by the AIController itself.

    public bool canShowSelectionCircle;
    //if the turn is for AI, units can show the selection circles.
    public GameObject selectionCircle;
    //reference to gameObject.
    public AudioClip unitsBallHit;
    //units hits the ball sfx
    public bool movefromGoalPost;

    public static OpponentUnitController Instance;

    private Renderer m_SelectionCircleRenderer = null;
    public Renderer SelectionCircleRenderer { get { if (!m_SelectionCircleRenderer) m_SelectionCircleRenderer = selectionCircle.GetComponent<Renderer>(); return m_SelectionCircleRenderer; } }

    private Rigidbody m_Rb = null;
    public Rigidbody GetRigidbody { get { if (!m_Rb) m_Rb = transform.GetComponent<Rigidbody>(); return m_Rb; } }
    void Awake()
    {
        Instance = this;
        canShowSelectionCircle = true;
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90f, -90f);

        if (!GlobalGameManager.IsMastersTurn && canShowSelectionCircle && GlobalGameManager.Instance.Phase != GlobalGameManager.GamePhase.GoalIntermission)
            SelectionCircleRenderer.enabled = true;
        else
            SelectionCircleRenderer.enabled = false;
    }
    private Collider _last = null;
    void OnCollisionEnter(Collision other)
    {
        if (other.collider == _last)
            return;

        _last = other.collider;

        Vector2 vel = GetRigidbody.velocity;

        float force = vel.magnitude;//avg is around 15

        float hardForce = FST_DiskPlayerManager.Instance.GetPhysicsData.HardDiskForce;

        if (force > hardForce)
            FST_ParticlePooler.Instance.DiskHit(transform, other.GetContact(0).point);

        switch (other.gameObject.tag)
        {
            case "ball":
             //   Debug.Log("disc to ball force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToBall_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToBall_Medium : FST_AudioManager.AudioID.SFX_DiscToBall_Soft);

                break;
            case "Player":
              //  Debug.Log("disc to disc force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToDisc_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToDisc_Medium : FST_AudioManager.AudioID.SFX_DiscToDisc_Soft);

                PlayerController pc = other.gameObject.GetComponent<PlayerController>();
                if (pc)
                    pc.GetRigidbody.velocity = pc.GetRigidbody.velocity * 1.2f;
                break;

            case "Player_2":
               // Debug.Log("disc to disc force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToDisc_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToDisc_Medium : FST_AudioManager.AudioID.SFX_DiscToDisc_Soft);

                Player2Controller p2c = other.gameObject.GetComponent<Player2Controller>();
                if (p2c)
                    p2c.GetRigidbody.velocity = p2c.GetRigidbody.velocity * 1.2f;
                break;

            case "Opponent":
              //  Debug.Log("disc to disc force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToDisc_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToDisc_Medium : FST_AudioManager.AudioID.SFX_DiscToDisc_Soft);

                OpponentUnitController op = other.gameObject.GetComponent<OpponentUnitController>();
                op.GetRigidbody.velocity = op.GetRigidbody.velocity * 1.2f;
                break;

            case "Border":
              //  Debug.Log("disc to wall force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToWall_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToWall_Medium : FST_AudioManager.AudioID.SFX_DiscToWall_Soft);

                break;
        }

        if (gameObject.name == GlobalGameManager.Instance.CurrentOpponentName)
        {
            //error check and report ->FST
            if (!GetRigidbody) { Debug.LogError("FST Error Notification -> No rigidbody found on: " + gameObject.name); return; }

            if (GetRigidbody.angularVelocity.magnitude <= 4)
            {
                if (GetRigidbody.velocity.x > 0)
                    GetRigidbody.AddRelativeTorque(new Vector3(0, GetRigidbody.velocity.magnitude, 0), ForceMode.Impulse);
                else GetRigidbody.AddRelativeTorque(new Vector3(0, -GetRigidbody.velocity.magnitude, 0), ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "gatePost":
                float hardForce = FST_DiskPlayerManager.Instance.GetPhysicsData.HardDiskForce;
                float force = GetRigidbody.velocity.magnitude * Time.fixedDeltaTime;
                Debug.Log("ball to gatepost force = " + force);
                FST_AudioManager.Instance.PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_BallToPost_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_BallToPost_Medium : FST_AudioManager.AudioID.SFX_BallToPost_Soft);
                break;
        }
    }

    private void PlayAudio(FST_AudioManager.AudioID audioID) => FST_AudioManager.Instance.PlayAudio(audioID);

    void OnCollisionStay(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Border":
                isStayonBorderO = true;
                break;
        }
    }

    void OnCollisionExit(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Border":
                isStayonBorderO = false;
                break;
        }
    }
}