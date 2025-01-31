using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z + (m_Speed));
    }
}
