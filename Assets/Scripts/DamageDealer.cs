using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_WeaponRB;

    [SerializeField][Range(0, int.MaxValue)] private float m_MaxDamage;
    private float m_CalculatedDamage;

    private float m_ImpulsiveVelocity;
    private float m_AverageVelocity = 0;

    [SerializeField][Range(0, 1)] private float m_MedianTimer;
    private float m_Timer = 0;

    private Vector2 m_OldPosition = Vector2.zero;

    private List<float> m_VelocityList;

    [SerializeField] private Transform m_RayPos;
    [SerializeField] private Character m_Host;

    [SerializeField] protected AudioSource m_Source;
    [SerializeField] protected AudioClip m_Clip;

    Character m_LastChar = null;

    private void Start()
    {
        m_VelocityList = new List<float>(10);
    }


    private void FixedUpdate()
    {
        m_Timer += Time.fixedDeltaTime;
        if (m_Timer >= m_MedianTimer / 10)
        {
            m_ImpulsiveVelocity = Vector2.Distance(transform.position, m_OldPosition);
            m_OldPosition = transform.position;
            if (m_VelocityList.Count == 10)
            {
                float c = 0;
                foreach (float f in m_VelocityList)
                {
                    c += f;
                }
                m_AverageVelocity = c / 10;
                m_VelocityList.Clear();
                CalculateDamage();
            }
            m_VelocityList.Add(m_ImpulsiveVelocity);
            m_Timer = 0;

        }


        RaycastHit2D rayHit = Physics2D.BoxCast(m_RayPos.position, transform.localScale, 0f, transform.right);

        if (rayHit)
        {
            Character creat = rayHit.collider.transform.root.GetComponentInChildren<Character>();

            if (creat != null && m_LastChar != creat && creat != m_Host)
            {
                if (Vector2.Distance(transform.position, creat.transform.position) > transform.localScale.x) return;
                m_LastChar = creat;
                if(creat == null)
                {
                    m_LastChar = null;
                    return;
                }
                (creat as CharacterAnimated).TakeDamage((int)m_CalculatedDamage);
                if (m_Source != null && m_Clip != null)
                {
                    m_Source.clip = m_Clip;
                    m_Source.Play();
                }
            }
            if (creat == null) m_LastChar = null;
        }
    }

    private void CalculateDamage()
    {
        m_CalculatedDamage = m_AverageVelocity * 10;
        if (m_CalculatedDamage > m_MaxDamage) m_CalculatedDamage = m_MaxDamage;
        else if (m_CalculatedDamage < 1) m_CalculatedDamage = 1;
        else m_CalculatedDamage = Mathf.FloorToInt(m_CalculatedDamage);
    }

}
