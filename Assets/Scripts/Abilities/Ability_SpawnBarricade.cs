using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SpawnBarricade : AbilityBase
{
    [SerializeField] private GameObject m_BarricadePrefab;
    [SerializeField] private Transform m_rotationVector;

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
            Instantiate(m_BarricadePrefab,transform.position, m_rotationVector.rotation);
            m_Timer = 0;
            if (m_Source != null && m_Clip != null)
            {
                m_Source.clip = m_Clip;
                m_Source.Play();
            }
        }
    }
}
