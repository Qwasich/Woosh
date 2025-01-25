using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private BehaviourNavmeshMovement m_Host;
    private CharacterAnimated m_Char;

    [SerializeField] private TaskBase[] m_Tasks;


    private int m_CurrentTask = 0;

    private void Awake()
    {
        m_Tasks[m_CurrentTask].SetActive(this);
        m_Char = m_Host.GetComponent<CharacterAnimated>();
    }

    private void Start()
    {
        m_Char.DeathTrigger += LoseCondition;
    }

    private void OnDestroy()
    {
        m_Char.DeathTrigger -= LoseCondition;
    }

    public void AdvanceTask()
    {
        m_CurrentTask++;
        if (m_CurrentTask < m_Tasks.Length)
        {
            m_Tasks[m_CurrentTask].SetActive(this);
            return;
        }
        //победа будет тут после завершения всех задач
        Debug.Log("WON");

    }

    public void LoseCondition()
    {
        Debug.Log("LOST");
    }

    public void SetTaskCoordinates(Transform t) => m_Host.SetTarget(t);

#if UNITY_EDITOR
    [ContextMenu(nameof(AdvanceTaskManual))]
    private void AdvanceTaskManual() => AdvanceTask();
#endif

}
