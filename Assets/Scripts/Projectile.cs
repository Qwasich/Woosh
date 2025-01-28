using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_ProjectileVelocity;

    [SerializeField] private int m_Damage;

    [SerializeField] private int m_MaxLifetime = 3;

    private string m_IgnoredTag;

    private Character m_Parent;
    /// <summary>
    /// Родитель, создавший этот объект
    /// </summary>
    public Character Parent => m_Parent;

    private float m_Timer = 0;

    private void Update()
    {
        float stepLength = Time.deltaTime * m_ProjectileVelocity;
        Vector2 step = transform.up * stepLength;

        RaycastHit2D rayHit = Physics2D.BoxCast(transform.position, transform.localScale, 0, transform.up);

        if (rayHit)
        {
            Character creat = rayHit.collider.transform.root.GetComponent<Character>();

            if (creat != null && creat != m_Parent && !creat.CompareTag("Enemy"))
            {
                creat.TakeDamage(m_Damage);
                OnLifetimeEnd();
            }
            else if (creat == null) OnLifetimeEnd();

        }

        m_Timer += Time.deltaTime;

        if (m_Timer > m_MaxLifetime)
        {
            OnLifetimeEnd();
        }

        transform.position += new Vector3(step.x, step.y, 0);

        

    }

    private void OnLifetimeEnd()
    {
        Debug.Log("destroyed");
        Destroy(gameObject);

    }

    /// <summary>
    /// Назначить родителя снаряда
    /// </summary>
    /// <param name="parent">Родитель</param>
    public void SetParentShooter(Character parent)
    {
        Debug.Log("parent set");
        m_Parent = parent;
    }

    public void SetTagsToIgnore(string tag)
    {

    }
}
