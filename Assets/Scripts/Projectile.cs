
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_ProjectileVelocity;

    [SerializeField] private int m_Damage;

    [SerializeField] private int m_MaxLifetime = 3;

    RaycastHit2D rayHit;

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
        /*
        rayHit = Physics2D.BoxCast(transform.position, , 0, transform.up);
        //rayHit = Physics2D.Raycast(transform.position, transform.up, transform.localScale.y);

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

        }*/

        m_Timer += Time.deltaTime;

        if (m_Timer > m_MaxLifetime)
        {
            OnLifetimeEnd();
        }

        transform.position += (Vector3)step;

        

    }

    private void OnLifetimeEnd()
    {
        Debug.Log("HUH");
        Destroy(gameObject); 

    }

    /// <summary>
    /// Назначить родителя снаряда
    /// </summary>
    /// <param name="parent">Родитель</param>
    public void SetParentShooter(Character parent)
    {
        m_Parent = parent;
    }

    public void SetTagsToIgnore(string tag)
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Sword") || col.CompareTag("SmallObject") || col.CompareTag("Enemy")) return;
        col.transform.TryGetComponent<Character>(out var c);
        Debug.Log("huh");
        if (c != null && c != m_Parent)
        {
            c.TakeDamage(m_Damage);
            OnLifetimeEnd();
            
        }
        else if (c == null) OnLifetimeEnd();

    }
}
