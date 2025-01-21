using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotationBehaviour : MonoBehaviour
{
    private float m_MovementSpeed = 10;
    private float m_RotationSpeed = 45;
    private float m_MaxDistance = 10;

    private int m_ActiveWeaponIndex = 0;
    private bool m_IsAnySkillActive = false;

    [SerializeField] private List<WeaponProperties> m_Weapons;

    [SerializeField] private GameObject m_HostPos;
    [SerializeField] private GameObject m_CamPos;

    private bool m_isWeaponFixed;

    void Start()
    {

        List<Object> m_Weapons = new();
        SetActiveWeapon(0);
    }

    private void Update()
    {
        CheckSkillButtons();
        if (!m_IsAnySkillActive) CheckWeaponButtons();
    }

    void FixedUpdate()
    {
        SetAngleForWeapon();
        SetPositionForWeapon();
    }


    #region Private Functions
    private void SetPositionForWeapon()
    {

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 posn;

        if (m_MaxDistance >= Vector2.Distance(m_HostPos.transform.position, pos))
        {
            posn = new Vector3(pos.x, pos.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, posn, m_MovementSpeed * Time.deltaTime);
        }
        else
        {
            posn = new Vector3(Mathf.Cos(Vector2.SignedAngle(Vector2.right, pos) * Mathf.Deg2Rad) * m_MaxDistance, Mathf.Sin(Vector2.SignedAngle(Vector2.right, pos) * Mathf.Deg2Rad) * m_MaxDistance, 0);
            transform.position = Vector3.MoveTowards(transform.position, posn, m_MovementSpeed * Time.deltaTime);
        }

        if (m_MaxDistance * 2 < Vector2.Distance(Vector2.zero, transform.position))
        {
            transform.position = m_HostPos.transform.position;
        }

    }

    private void SetAngleForWeapon()
    {
        if (m_isWeaponFixed) return;
        float angle;
        float result;
        Vector2 dir;
        Vector2 campos;
        campos = m_CamPos.transform.position;

        switch(m_ActiveWeaponIndex)
        {
            case 0: { dir = (Vector2)transform.position - campos; break;}
            case 1: { dir = (Vector2)transform.position; break;}
            default: { dir = (Vector2)transform.position; break; }
        }
        
        angle = Vector2.SignedAngle(m_CamPos.transform.right, dir);
        result = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, angle, m_RotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, result);
    }

    private void SetActiveWeapon(int weapon)
    {
        if (m_Weapons[weapon] == null) return;
    
        foreach (WeaponProperties wp in m_Weapons) wp.gameObject.SetActive(false);
        m_Weapons[weapon].gameObject.SetActive(true);

        m_MovementSpeed = m_Weapons[weapon].Settings.MovementSpeed;
        m_RotationSpeed = m_Weapons[weapon].Settings.RotationSpeed;
        m_MaxDistance = m_Weapons[weapon].Settings.MaxDistance;

        m_ActiveWeaponIndex = weapon;

    }

    private void CheckWeaponButtons()
    {
        if (Input.GetMouseButtonDown(0)) { m_isWeaponFixed = true; }
        if (Input.GetMouseButtonUp(0)) { m_isWeaponFixed = false; }

        if (Input.GetKey(KeyCode.Alpha1)) { SetActiveWeapon(0); return; }
        if (Input.GetKey(KeyCode.Alpha2)) { SetActiveWeapon(1); return; }
    }

    private void CheckSkillButtons()
    {

    }

    #endregion
}
