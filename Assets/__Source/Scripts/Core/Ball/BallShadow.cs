using UnityEngine;
using FastSkillTeam;

public class BallShadow : MonoBehaviour
{
	public void Update ()
	{
        if (!FST_BallManager.Instance)
            return;

		transform.position = new Vector3 (FST_BallManager.Instance.transform.position.x, FST_BallManager.Instance.transform.position.y, -0.05f);
	}
}

