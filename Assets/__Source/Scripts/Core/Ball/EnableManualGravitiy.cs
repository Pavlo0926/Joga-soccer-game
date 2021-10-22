using System.Collections;
using UnityEngine;

public class EnableManualGravitiy : MonoBehaviour
{
    public static EnableManualGravitiy Instance;

    Rigidbody rigidbodys;

    public enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

    public Direction objDir;

    public bool isAddForce;

    public float timeForce;

    public bool IsForceAddContinuous { get; set; }

    private float addForce;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rigidbodys = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isAddForce)
        {
            if (objDir == Direction.UP)
            {
                rigidbodys.velocity += new Vector3(0, addForce, 0);
            }
            else if (objDir == Direction.DOWN)
            {
                rigidbodys.velocity += new Vector3(0, -addForce, 0);
            }
            else if (objDir == Direction.RIGHT)
            {
                rigidbodys.velocity += new Vector3(addForce, 0, 0);
            }
            else if (objDir == Direction.LEFT)
            {
                rigidbodys.velocity += new Vector3(-addForce, 0, 0);
            }
        }
    }

    public void StartGiveForce(float appliedForce)
    {
        addForce = appliedForce / 40f;
        StartCoroutine(CheckforceAdded());
    }

    IEnumerator CheckforceAdded()
    {
        yield return new WaitForSeconds(0.05f);
        isAddForce = true;
        yield return new WaitForSeconds(0.07f);
        IsForceAddContinuous = true;
        yield return new WaitForSeconds(timeForce);
        isAddForce = false;
    }

    public void StopGiveForce()
    {
        IsForceAddContinuous = false;
        StopCoroutine(CheckforceAdded());
        StartCoroutine(StopAddedForce());
    }

    IEnumerator StopAddedForce()
    {
        yield return new WaitForSeconds(0.1f);
        isAddForce = false;
    }
}
