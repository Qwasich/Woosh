using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private BehaviourNavmeshMovement m_Host;
    public BehaviourNavmeshMovement Host => m_Host;
    private CharacterAnimated m_Char;

    [SerializeField] private List<TaskBase> m_Tasks;

    [SerializeField] private int m_MaxTasks;

    [SerializeField] private float m_TaskCheckTimer = 1;

    [SerializeField] private Transform m_TaskTextHolder;
    [SerializeField] private GameObject m_TextPrefab;
    private Text[] m_TextTasks;

    private float m_Timer;
    private List<TaskBase> m_AllowedTasks;

    [SerializeField] private GameObject m_WinObject;
    [SerializeField] private GameObject m_LoseObject;

    [SerializeField] private Image m_HpObject;


    private int m_CurrentTask = 0;


    private void Start()
    {
        m_TextTasks = new Text[m_MaxTasks];
        m_AllowedTasks = new List<TaskBase>(m_MaxTasks);
        CompileTaskList();
        m_AllowedTasks[m_CurrentTask].enabled = true;
        m_AllowedTasks[m_CurrentTask].SetActive(this);
        m_Char = m_Host.GetComponent<CharacterAnimated>();
        m_Char.DeathTrigger += LoseCondition;
        m_Char.DamageTrigger += UpdateHP;
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
        m_Char.DamageTrigger -= UpdateHP;
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
            m_TextTasks[i] = Instantiate(m_TextPrefab,m_TaskTextHolder).GetComponent<Text>();
            m_TextTasks[i].text = candidate.TaskText;
        }
    }

    public void AdvanceTask()
    {
        m_TextTasks[m_CurrentTask].color = Color.green;
        m_AllowedTasks[m_CurrentTask].enabled = false;
        m_CurrentTask++;
        if (m_CurrentTask < m_AllowedTasks.Capacity)
        {
            
            m_AllowedTasks[m_CurrentTask].enabled = true;
            m_AllowedTasks[m_CurrentTask].SetActive(this);
            return;
        }
        //победа будет тут после завершения всех задач
        m_WinObject.SetActive(true);
        m_Char.MakeInvincible(true);

    }

    private void UpdateHP()
    {
        float c = m_Char.CurrentHealth;
        float m = m_Char.MaxHealth;
        m_HpObject.fillAmount = c / m;
    }

    private void LoseCondition()
    {
        m_LoseObject.SetActive(true);
    }

    public void SetTaskCoordinates(Transform t) => m_Host.SetTarget(t);

#if UNITY_EDITOR
    [ContextMenu(nameof(AdvanceTaskManual))]
    private void AdvanceTaskManual() => AdvanceTask();
#endif

}
