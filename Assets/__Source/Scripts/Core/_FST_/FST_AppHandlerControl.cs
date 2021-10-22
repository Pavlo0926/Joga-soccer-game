using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FST_AppHandlerControl : MonoBehaviour
{
    public static FST_AppHandlerControl Instance = null;
    public static bool RunChecks = true;

#pragma warning disable CS0649
    [Tooltip("Any gameobject in this array if enabled will block app handler from making checks")]
    [SerializeField] private List< GameObject> Blockers = new List<GameObject>();
#pragma warning restore CS0649


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        RunChecks = true;
        for (int i = 0; i < Blockers.Count; i++)
        {
            if(Blockers[i] == null)
            {
                Blockers.RemoveAt(i);
                continue;
            }

            if (Blockers[i].activeInHierarchy)
            {
                RunChecks = false;
                break;
            }
        }        
    }

    public void AddBlocker(GameObject blocker)
    {
        if (!Blockers.Contains(blocker))
            Blockers.Add(blocker);
    }

    private void OnDisable()
    {
        RunChecks = true;
    }
}
