using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource m_Source;
    [SerializeField] private AudioClip m_StartClip;
    [SerializeField] private AudioClip m_LoopClip;

    private void Awake()
    {
        m_Source.clip = m_StartClip;
        m_Source.loop = false;
        m_Source.Play();
    }

    private void Update()
    {
        if (m_Source.isPlaying) return;

        m_Source.clip = m_LoopClip;
        m_Source.loop = true;
        m_Source.Play();

        Destroy(this);
    }
}
