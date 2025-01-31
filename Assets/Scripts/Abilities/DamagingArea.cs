using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamagingArea : MonoBehaviour
{

    [SerializeField] CircleCollider2D m_DamageCollider;
    [SerializeField] private int m_Damage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Character c = collision.transform.GetComponentInChildren<Character>();
        if (c == null || c.CompareTag("Player")) return;
        c.TakeDamage(m_Damage);
    }
}
