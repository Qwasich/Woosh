using UnityEngine;

public class TaskBase : MonoBehaviour
{
    protected GameMaster m_Master;
    protected bool m_IsDone = false;

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

