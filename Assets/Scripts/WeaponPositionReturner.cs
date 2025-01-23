using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPositionReturner : MonoBehaviour
{

    [SerializeField] private WeaponMovementSettings m_WeaponProperties;
    [SerializeField] private GameObject m_Host;

    [SerializeField] private Rigidbody2D m_Rb;

    float angleresult;

    void FixedUpdate()
    {
        SetPositionForWeapon();
        SetAngleForWeapon();
    }

    private void SetPositionForWeapon()
    {
        if (Vector2.Distance(m_Host.transform.position, transform.position) <= 0.1) return;

        Vector2 dir = m_Host.transform.position - transform.position;

        Vector2 force = dir.normalized * m_WeaponProperties.MovementSpeed / 50 * Vector2.Distance(m_Host.transform.position, transform.position);

        m_Rb.AddForce(force, ForceMode2D.Impulse);

        if (m_WeaponProperties.MaxDistance * 2 < Vector2.Distance(m_Host.transform.position, transform.position))
        {
            transform.position = m_Host.transform.position;
        }

    }

    private void SetAngleForWeapon()
    {
        //angle = Quaternion.Angle(m_Host.transform.rotation, transform.rotation);
        angleresult = Mathf.DeltaAngle(transform.eulerAngles.z, m_Host.transform.eulerAngles.z);
        if (Mathf.Abs(angleresult) < 0.1f) return;

        m_Rb.AddTorque(angleresult * Time.fixedDeltaTime * 4, ForceMode2D.Impulse);
    }
        

        
}
