using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponRotationBehaviour : MonoBehaviour
{
    private float m_MovementSpeed = 10;
    private float m_RotationSpeed = 45;
    private float m_MaxDistance = 10;
    [SerializeField][Range(0, 10)] private float m_MinDistance = 0.5f;

    private int m_ActiveWeaponIndex = 0;
    public int ActiveWeaponIndex => m_ActiveWeaponIndex;
    private bool m_IsAnySkillActive = false;

    [SerializeField] private List<WeaponProperties> m_Weapons;

    [SerializeField] private GameObject m_ActiveArea;

    [SerializeField] private Transform m_HostPos;
    [SerializeField] private Transform m_CamPos;

    [SerializeField] private float m_WSCT = 1.5f;
    private float WSCT;

    [SerializeField] private GameObject[] m_UISwitcher;
    [SerializeField] private Image[] m_CooldownImages;

    private bool m_isWeaponFixed;
    Vector3 posn;
    float angle;

    void Start()
    {
        
        List<Object> m_Weapons = new();
        SetActiveWeapon(0);
    }

    private void Update()
    {
        if (WSCT < m_WSCT)
        {
            WSCT += Time.deltaTime;
            m_CooldownImages[m_ActiveWeaponIndex].fillAmount = 1 - WSCT / m_WSCT;
        }
        CheckSkillButtons();
        if (!m_IsAnySkillActive) CheckWeaponButtons();
    }

    void FixedUpdate()
    {
        if (m_HostPos == null) return;
        SetAngleForWeapon();
        SetPositionForWeapon();
    }


    #region Private Functions
    private void SetPositionForWeapon()
    {

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        angle = Vector2.SignedAngle(Vector2.right, pos - m_HostPos.position);

        if (m_MaxDistance >= Vector2.Distance(m_HostPos.position, pos))
        {
            posn = new Vector3(pos.x, pos.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, posn, m_MovementSpeed * Time.deltaTime);
        }
        else
        {
            
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * m_MaxDistance;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * m_MaxDistance;

            posn = new Vector3(x, y, 0) + m_HostPos.position;
            transform.position = Vector3.MoveTowards(transform.position, posn, m_MovementSpeed * Time.deltaTime);

            //posn = new Vector3(Mathf.Cos(Vector2.SignedAngle(m_HostPos.right, pos) * Mathf.Deg2Rad) * m_MaxDistance, Mathf.Sin(Vector2.SignedAngle(m_HostPos.right, pos) * Mathf.Deg2Rad) * m_MaxDistance, 0);
            //transform.position = posn;
        }

        if (m_MaxDistance * 2 < Vector2.Distance(m_HostPos.position, transform.position))
        {
            transform.position = m_HostPos.transform.position;
        }

    }

    private void SetAngleForWeapon()
    {
        if (m_isWeaponFixed) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (m_MinDistance > Vector2.Distance(m_CamPos.position, pos)) return;
        float angle;
        float result;
        Vector2 dir;
        Vector2 campos;
        campos = m_CamPos.position;

        switch(m_ActiveWeaponIndex)
        {
            case 0: { dir = (Vector2)transform.position - campos; break;}
            case 1: { dir = transform.position - m_HostPos.position; break;}
            default: { dir = transform.position - m_HostPos.position; break; }
        }
        
        angle = Vector2.SignedAngle(m_CamPos.right, dir);
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

        m_UISwitcher[m_ActiveWeaponIndex].SetActive(false);

        m_ActiveWeaponIndex = weapon;

        float dist = m_Weapons[weapon].Settings.MaxDistance;

        m_UISwitcher[m_ActiveWeaponIndex].SetActive(true);
        m_ActiveArea.transform.localScale = new Vector2(dist * 0.4f, dist * 0.4f);

    }

    private void CheckWeaponButtons()
    {
        if (Input.GetMouseButtonDown(0)) { m_isWeaponFixed = true; }
        if (Input.GetMouseButtonUp(0)) { m_isWeaponFixed = false; }

        if (WSCT >= m_WSCT && Input.GetKey(KeyCode.Alpha1) && m_ActiveWeaponIndex != 0)
        {
            SetActiveWeapon(0);
            WSCT = 0;
            return;
        }
        if (WSCT >= m_WSCT && Input.GetKey(KeyCode.Alpha2) && m_ActiveWeaponIndex != 1)
        {
            SetActiveWeapon(1);
            WSCT = 0;
            return;
        }
    }

    private void CheckSkillButtons()
    {

    }

    #endregion
}
