using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_ProjectileVelocity;

    [SerializeField] private int m_Damage;

    [SerializeField] private int m_MaxLifetime = 3;

    RaycastHit2D rayHit;

    private Character m_Parent;
    /// <summary>
    /// ��������, ��������� ���� ������
    /// </summary>
    public Character Parent => m_Parent;

    private float m_Timer = 0;

    private void Update()
    {
        float stepLength = Time.deltaTime * m_ProjectileVelocity;
        Vector2 step = transform.up * stepLength;

        rayHit = Physics2D.BoxCast(transform.position, transform.localScale, 0, transform.up);

        if (rayHit)
        {
            Character creat = rayHit.collider.transform.root.GetComponent<Character>();
            Transform go = rayHit.collider.GetComponent<Transform>();

            if (!go.CompareTag("Sword") && !go.CompareTag("SmallObject") && !go.CompareTag("Enemy"))
            {

                if (creat != null && creat != m_Parent)
                {
                    creat.TakeDamage(m_Damage);
                    OnLifetimeEnd();
                }
                else if (creat == null) OnLifetimeEnd();
            }

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
        Destroy(gameObject);

    }

    /// <summary>
    /// ��������� �������� �������
    /// </summary>
    /// <param name="parent">��������</param>
    public void SetParentShooter(Character parent)
    {
        m_Parent = parent;
    }

    public void SetTagsToIgnore(string tag)
    {

    }
}
