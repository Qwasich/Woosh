using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu]
public class WeaponMovementSettings : ScriptableObject
{
    [SerializeField][Description("Определяет насколько быстро объект будет двигатся к мыши")] public float MovementSpeed = 50;
    [SerializeField][Description("Определяет насколько быстро объект будет поворачиватся")] public float RotationSpeed = 180;
    [SerializeField][Description("Определяет насколько далеко можно отойти от бота с этим оружием")] public float MaxDistance = 20;
}
