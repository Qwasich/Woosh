using UnityEngine;
using UnityEngine.AI;

public class BehaviourNavmeshMovement : MonoBehaviour
{
    [SerializeField] Transform m_Target;
    public Transform Target => m_Target;

    NavMeshAgent m_Agent;

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = true;
        m_Agent.updateUpAxis = false;
        m_Agent.updateRotation = false;
    }

    private void Update()
    {
        if (m_Target == null) return;
        m_Agent.SetDestination(m_Target.position);
    }

    public void SetTarget(Transform t) => m_Target = t;
}
