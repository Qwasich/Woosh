using UnityEngine;
using UnityEngine.AI;

public class BehaviourNavmeshMovement : MonoBehaviour
{
    [SerializeField] Transform m_Target;

    NavMeshAgent m_Agent;

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = true;
        m_Agent.updateUpAxis = false;
    }

    private void Update()
    {
        m_Agent.SetDestination(m_Target.position);
    }
}
