using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    [SerializeField] private float m_Lifetime;
    private float m_Timer;

    private void Update()
    {
        if (m_Timer < m_Lifetime)
        {
            m_Timer += Time.deltaTime;
        }
        else
            Destroy(gameObject);
    }
}
