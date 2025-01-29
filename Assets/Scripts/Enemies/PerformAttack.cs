using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerformAttack : MonoBehaviour
{
    [SerializeField] private GameObject m_Projectile;

    [SerializeField] private bool m_IsRanged = false;
    
    [HideInInspector]public bool IsRanged => m_IsRanged;

    [SerializeField] private bool m_IsSelfDestrruct = false;

    [SerializeField] private int m_Damage = 1;

    [SerializeField][Range(0, int.MaxValue)] private float m_AttackCooldown = 1;

    [SerializeField] private Transform m_Offset;

    private List<Character> m_Target;
    private Character m_Host;

    [HideInInspector]  public Vector3 m_CurrentTarget;

    private float m_Timer;

    private bool m_AllowedToAttack = false;
    public bool AllowedToAttack => m_AllowedToAttack;

    private void Awake()
    {   
        m_Host = transform.root.GetComponent<Character>();
        m_CurrentTarget = Vector3.zero;
    }

    private void Start()
    {
        m_Target = new List<Character>(10);
    }

    private void Update()
    {
        if (!m_AllowedToAttack) return;

        m_Timer += Time.deltaTime;
        m_CurrentTarget = m_Target.First().transform.position;
        if (m_Timer < m_AttackCooldown) return;

        if (m_IsRanged && m_Projectile != null)
        {
            Projectile p = Instantiate(m_Projectile).GetComponent<Projectile>();
            p.SetParentShooter(m_Host);

            if (m_Offset != null) p.transform.position = m_Offset.transform.position;
            else p.transform.position = m_Host.transform.position;
            p.transform.up = m_Target.First().transform.position - transform.position;
            m_Timer = 0;
        }
        else
        {
            m_Target.First().TakeDamage(m_Damage);
            m_Timer = 0;

            if (m_IsSelfDestrruct) m_Host.TakeDamage(999999);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (!collision.transform.TryGetComponent<Character>(out var c)) return;

        if (m_Target.Count == m_Target.Capacity) return;
        m_Target.Add(c);
        m_AllowedToAttack = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (!collision.transform.TryGetComponent<Character>(out var c)) return;
        if (!m_Target.Contains(c)) return;

        m_Target.Remove(c);

        if (m_Target.Count == 0)
        {
            m_AllowedToAttack = false;
            m_CurrentTarget = Vector3.zero;
        }
        
    }
}
