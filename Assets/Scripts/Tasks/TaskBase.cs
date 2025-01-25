using UnityEngine;

public class TaskBase : MonoBehaviour
{
    private GameMaster m_Master;

    public virtual void SetActive(GameMaster m)
    {
        m_Master = m;
        m_Master.SetTaskCoordinates(transform);
    }

    protected virtual void SetDone()
    {

        // деактивация скриптов задачи
        m_Master.AdvanceTask();
    }



}

