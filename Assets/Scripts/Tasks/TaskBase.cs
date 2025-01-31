using UnityEngine;

public class TaskBase : MonoBehaviour
{
    protected GameMaster m_Master;
    protected bool m_IsDone = false;

    [SerializeField] protected string m_TaskText = " ";
    public string TaskText => m_TaskText;

    public virtual void SetActive(GameMaster m)
    {
        m_Master = m;
        m_Master.SetTaskCoordinates(transform);
    }

    protected virtual void SetDone()
    {

        // ����������� �������� ������
        m_Master.AdvanceTask();
    }

    public virtual bool IsTaskDone()
    {
        return false;
    }



}

