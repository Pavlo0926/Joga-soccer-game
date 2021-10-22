using UnityEngine;

public class Goal_animation : MonoBehaviour
{
    public static Goal_animation Instance;
    Animator animator;

    public bool Dance;
    public bool Idle;
    private bool isDancing = false;
 

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = this.GetComponent<Animator>();
        Dance = false;
    }


    void Update()
    {
        if (Dance && !isDancing)
        {
            animator.SetBool("setOn", true);
            isDancing = true;
        }
        else if(isDancing)
        {
            animator.SetBool("setOn", false);
            isDancing = false;
        }
    }
}
