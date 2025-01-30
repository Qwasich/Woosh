using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private BehaviourNavmeshMovement m_Host;
    public BehaviourNavmeshMovement Host => m_Host;
    private CharacterAnimated m_Char;

    [SerializeField] private List<TaskBase> m_Tasks;

    [SerializeField] private int m_MaxTasks;

    [SerializeField] private float m_TaskCheckTimer = 1;

    private float m_Timer;
    private List<TaskBase> m_AllowedTasks;


    private int m_CurrentTask = 0;


    private void Start()
    {
        m_AllowedTasks = new List<TaskBase>(m_MaxTasks);
        CompileTaskList();
        m_AllowedTasks[m_CurrentTask].enabled = true;
        m_AllowedTasks[m_CurrentTask].SetActive(this);
        m_Char = m_Host.GetComponent<CharacterAnimated>();
        m_Char.DeathTrigger += LoseCondition;
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= m_TaskCheckTimer && m_CurrentTask < m_AllowedTasks.Capacity)
        {
            m_AllowedTasks[m_CurrentTask].IsTaskDone();
            m_Timer = 0;
        }
    }

    private void OnDestroy()
    {
        m_Char.DeathTrigger -= LoseCondition;
    }

    private void CompileTaskList()
    {
        if (m_MaxTasks > m_Tasks.Capacity) m_MaxTasks = m_Tasks.Capacity;
        for (int i = 0; i < m_MaxTasks; i++)
        {
            TaskBase candidate = m_Tasks[Random.Range(0, m_Tasks.Count)];
            if (m_AllowedTasks.Contains(candidate))
            {
                i--;
                continue;
            }
            m_AllowedTasks.Add(candidate);
        }
    }

    public void AdvanceTask()
    {
        m_AllowedTasks[m_CurrentTask].enabled = false;
        m_CurrentTask++;
        if (m_CurrentTask < m_AllowedTasks.Capacity)
        {
            m_AllowedTasks[m_CurrentTask].enabled = true;
            m_AllowedTasks[m_CurrentTask].SetActive(this);
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
