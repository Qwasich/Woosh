using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private float m_PushPower;

    private float m_TimerMax = 0.3f;
    private float m_Timer = 0;

    public void ResetTimer() => m_Timer = 0;

    private void Update()
    {
        m_Timer += Time.deltaTime;
        if(m_Timer >= m_TimerMax )
        {
            m_Timer = 0;
            gameObject.SetActive( false );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        collision.attachedRigidbody.AddForce(transform.right * m_PushPower, ForceMode2D.Force);
    }
}
