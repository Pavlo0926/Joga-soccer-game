using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FST_Particle : MonoBehaviour
{
    ParticleSystem m_Particle;

    private void Awake()
    {
        m_Particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(!m_Particle.isPlaying && transform.parent != FST_ParticlePooler.Instance.transform)
        {
            transform.SetParent(FST_ParticlePooler.Instance.transform);
            gameObject.SetActive(false);
        }
    }
}
