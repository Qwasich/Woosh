using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu]
public class WeaponMovementSettings : ScriptableObject
{
    [SerializeField][Description("���������� ��������� ������ ������ ����� �������� � ����")] public float MovementSpeed = 50;
    [SerializeField][Description("���������� ��������� ������ ������ ����� �������������")] public float RotationSpeed = 180;
    [SerializeField][Description("���������� ��������� ������ ����� ������ �� ���� � ���� �������")] public float MaxDistance = 20;
}
