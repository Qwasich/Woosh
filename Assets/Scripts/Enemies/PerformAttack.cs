using UnityEngine;

public class PerformAttack : MonoBehaviour
{
    [SerializeField] private GameObject m_Projectile;

    [SerializeField] private bool IsRanged = false;

    [SerializeField][Range(0, int.MaxValue)] private int m_Damage = 1;

    [SerializeField][Range(0, int.MaxValue)] private float m_AttackCooldown = 1;

    private Character m_Target;
    private Character m_Host;

    private float m_Timer;

    private bool m_AllowedToAttack = false;

    private void Awake()
    {
        m_Host = transform.root.GetComponent<Character>();
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
            p.transform.up = m_Target.transform.position - transform.position;

            m_Timer = 0;
        }
        else
        {
            m_Target.TakeDamage(m_Damage);
            m_Timer = 0;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        m_Target = collision.transform.root.GetComponent<Character>();
        m_AllowedToAttack = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        m_Target = null;
        m_AllowedToAttack = false;
    }
}
