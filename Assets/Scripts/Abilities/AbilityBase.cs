using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityBase : MonoBehaviour
{
    
    [SerializeField] protected float m_CooldownTimer;
    protected float m_Timer;

    [SerializeField] protected WeaponRotationBehaviour m_RotationBehaviour;
    [SerializeField] protected int m_WeaponIndex = 0;

    [SerializeField] protected Image m_CooldownImg;
    [SerializeField] protected AudioSource m_Source;
    [SerializeField] protected AudioClip m_Clip;

    protected virtual void Update()
    {
        if (m_Timer < m_CooldownTimer) 
        {
            m_Timer += Time.deltaTime;
            m_CooldownImg.fillAmount = 1 - m_Timer / m_CooldownTimer;
        }
        
    }

    protected virtual void CheckAbilityButton()
    {

        

    }
}
