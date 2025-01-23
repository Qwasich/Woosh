using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Debug.Log("damaged");
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0) OnKill();

        m_Timer = m_InvincibilityTimer;
        return;
    }

    public virtual void HealDamage(int heal)
    {
        if (m_IsInvincible) return;
        if (heal <= 0) heal = 1;

        m_CurrentHealth += heal;
        if (m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;

        return;
    }

    protected virtual void OnKill()
    {
        Destroy(gameObject);
    }
}
