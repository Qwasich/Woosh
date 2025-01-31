using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Creature Parameters")]
    [SerializeField] protected string m_Nickname;
    /// <summary>
    /// Имя Существа
    /// </summary>
    public string Name => m_Nickname;

    [SerializeField] protected int m_MaxHealth;
    protected int m_CurrentHealth;

    public int MaxHealth => m_MaxHealth;
    public int CurrentHealth => m_CurrentHealth;


    [SerializeField] protected bool m_IsInvincible;
    /// <summary>
    /// Если истинно, игнорирует любой урон.
    /// </summary>
    public bool IsInvincible => m_IsInvincible;

    [SerializeField] protected float m_InvincibilityTimer = 0.05f;
    /// <summary>
    /// Сколько объект остается неуязвимым к повреждениям, после получения урона
    /// </summary>
    public float InincibilityTimer => m_InvincibilityTimer;
    protected float m_Timer;

    public UnityAction DeathTrigger;
    public UnityAction DamageTrigger;

    [SerializeField] private GameObject m_DamageParticles;
    [SerializeField] protected AudioSource m_Source;
    [SerializeField] protected AudioClip m_Clip;

    protected virtual void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    protected virtual void Update()
    {
        if (m_Timer > 0) m_Timer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (m_IsInvincible || m_Timer > 0) return;
        if (m_CurrentHealth <= 0) return;
        if (damage <= 0) return;
        m_CurrentHealth -= damage;
        DamageTrigger?.Invoke();
        if (m_DamageParticles != null) Instantiate(m_DamageParticles,transform.root);
        if (m_Source != null && m_Clip != null)
        {
            m_Source.clip = m_Clip;
            m_Source.Play();
        }
        if (m_CurrentHealth <= 0) OnKill();

        m_Timer = m_InvincibilityTimer;
        return;
    }

    public virtual void HealDamage(int heal)
    {
        if (m_IsInvincible) return;
        if (heal <= 0) heal = 1;

        m_CurrentHealth += heal;
        DamageTrigger?.Invoke();
        if (m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;

        return;
    }

    protected virtual void OnKill()
    {
        DeathTrigger?.Invoke();
        Destroy(transform.root.gameObject);
    }

    protected virtual void OnKill(NavMeshAgent ogj)
    {
        DeathTrigger?.Invoke();
        Destroy(ogj.gameObject);
    }

    public virtual void MakeInvincible(bool s)
    {
        m_IsInvincible = s;
    }

#if UNITY_EDITOR
    [ContextMenu(nameof(KILL))]
    private void KILL() => OnKill();
#endif
}
