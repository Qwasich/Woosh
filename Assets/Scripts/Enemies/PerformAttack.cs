using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerformAttack : MonoBehaviour
{
    [SerializeField] private GameObject m_Projectile;

    [SerializeField] private bool IsRanged = false;

    [SerializeField][Range(0, int.MaxValue)] private int m_Damage = 1;

    [SerializeField][Range(0, int.MaxValue)] private float m_AttackCooldown = 1;

    private List<Character> m_Target;
    private Character m_Host;

    private float m_Timer;

    private bool m_AllowedToAttack = false;

    private void Awake()
    {
        m_Host = transform.root.GetComponent<Character>();
    }

    private void Start()
    {
        m_Target = new List<Character>(10);
    }

    private void FixedUpdate()
    {
        if (!m_AllowedToAttack) return;

        m_Timer += Time.fixedDeltaTime;
        if (m_Timer < m_AttackCooldown) return;

        if (IsRanged && m_Projectile != null)
        {
            Projectile p = Instantiate(m_Projectile).GetComponent<Projectile>();
            p.SetParentShooter(m_Host);
            p.transform.position = m_Host.transform.position;
            p.transform.up = m_Target.First().transform.position - transform.position;

            m_Timer = 0;
        }
        else
        {
            m_Target[0].TakeDamage(m_Damage);
            m_Timer = 0;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Character c = collision.transform.GetComponent<Character>();
        if (c == null) return;

        if (m_Target.Count == m_Target.Capacity) return;
        m_Target.Add(c);
        m_AllowedToAttack = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Character c = collision.transform.GetComponent<Character>();
        if (c == null) return;
        if (!m_Target.Contains(c)) return;

        m_Target.Remove(c);

        if (m_Target.Count == 0)
        m_AllowedToAttack = false;
    }
}
