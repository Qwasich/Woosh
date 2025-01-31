using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_DamagingArea : AbilityBase
{
    [SerializeField] private GameObject m_Area;
    [SerializeField] private float m_ActiveTimer = 2f;
    private float aTimer = 2f;

    private void Awake()
    {
        aTimer = m_ActiveTimer;
    }

    protected override void Update()
    {
        base.Update();

        if (aTimer < m_ActiveTimer) aTimer += Time.deltaTime;
        if (aTimer >= m_ActiveTimer && m_Area.activeSelf)
        {
            m_Area.SetActive(false);
            m_Timer = 0;
            return;
        }
        if (m_WeaponIndex != m_RotationBehaviour.ActiveWeaponIndex && !m_Area.activeSelf) return;
        else if(m_WeaponIndex != m_RotationBehaviour.ActiveWeaponIndex && m_Area.activeSelf)
        {
            m_Area.SetActive(false);
            m_Timer = 0;
            return;
        }
        if (m_Timer >= m_CooldownTimer)
        {
            CheckAbilityButton();
            
        }
        
    }

    protected override void CheckAbilityButton()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!m_Area.activeSelf)
            {
                m_Area.SetActive(true);
                aTimer = 0;
                if (m_Source != null && m_Clip != null)
                {
                    m_Source.clip = m_Clip;
                    m_Source.Play();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (m_Area.activeSelf)
            {
                m_Area.SetActive(false);
                m_Timer = 0;
                aTimer = m_ActiveTimer;
            }
                
        }

    }
}
