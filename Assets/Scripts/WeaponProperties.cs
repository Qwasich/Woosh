using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProperties : MonoBehaviour
{
    [SerializeField] private WeaponMovementSettings m_Settings;

    public WeaponMovementSettings Settings => m_Settings;

}
