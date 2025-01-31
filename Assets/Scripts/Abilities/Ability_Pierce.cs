using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Pierce : AbilityBase
{
    [SerializeField] private WeaponPositionReturner wpr;
    [SerializeField] private float m_LaunchPower = 1;
    protected override void Update()
    {
        base.Update();
        if (m_WeaponIndex != m_RotationBehaviour.ActiveWeaponIndex) return;
        if (m_Timer >= m_CooldownTimer)
        {
            CheckAbilityButton();

        }
        
    }

    protected override void CheckAbilityButton()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            wpr.Boost(m_LaunchPower);
            m_Timer = 0;
            if (m_Source != null && m_Clip != null)
            {
                m_Source.clip = m_Clip;
                m_Source.Play();
            }
        }
    }
}
