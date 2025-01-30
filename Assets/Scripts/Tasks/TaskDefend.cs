using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDefend : TaskBase, ISpawnerUtils
{
    //sS
    [SerializeField] private List<SpawnableUnit> m_Units;

    //Общее количество юнитов для спавна
    [SerializeField] private int m_SpawnCount;

    //Колво юнитов в одной пачке для спавна
    [SerializeField] private int m_SpawnAmount;

    [SerializeField] private Transform[] m_SpawnPoints;

    [SerializeField] private float m_SpawnTimer = 1;
    private float m_Timer = 0;


    private List<CharacterAnimated> m_UnitsOnScene;
    private int m_MaxWeight;

    private int m_Remain;

    private void Start()
    {
        m_Remain = m_SpawnCount;
        foreach (var unit in m_Units)
        {
            m_MaxWeight += unit.m_Weight;
        }
    }

    private void Update()
    {
        if (m_IsDone) return;
        if (m_Timer < m_SpawnTimer) m_Timer += Time.deltaTime;

        if (m_Timer >= m_SpawnTimer && Vector3.Distance(m_Master.Host.transform.position, transform.position) < 2)
        {
            SpawnBatch();
            m_Timer = 0;
        }


    }

    public override void SetActive(GameMaster m)
    {
        m_UnitsOnScene = new List<CharacterAnimated>(m_SpawnCount);
        base.SetActive(m);
    }

    private void SpawnBatch()
    {
        for (int i = 0; i < m_SpawnAmount; i++)
        {
            m_Remain--;
            if (m_Remain == 0)
            {
                m_IsDone = true;
                break;
            }
            Vector2 pos = CalculateSpawnPosition();
            int ind = GetUnitIndexToSpawn();
            BehaviourNavmeshMovement bnm = Instantiate(m_Units[ind].m_Creature, pos, transform.rotation);
            CharacterAnimated ca = bnm.GetComponentInChildren<CharacterAnimated>();
            ca.SetSpawner(this);
            bnm.SetTarget(m_Master.Host.transform);
            m_UnitsOnScene.Add(ca);

            if (m_UnitsOnScene.Count == m_UnitsOnScene.Capacity) break;
        }
    }

    private Vector2 CalculateSpawnPosition()
    {
        Vector2 v = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length)].position;
        Vector2 pos;
        float x = Random.Range(v.x - 5, v.x + 5);
        float y = Random.Range(v.y - 5, v.y + 5);
        pos = new Vector2(x, y);

        return pos;
    }

    protected override void SetDone()
    {
        m_UnitsOnScene = null;
        base.SetDone();
    }

    public override bool IsTaskDone()
    {
        bool check = true;

        foreach (CharacterAnimated c in m_UnitsOnScene)
        {
            if (c != null) check = false;
        }
        if (check && m_IsDone)
        {
            SetDone();
            return true;
        }
        return false;
    }

    private int GetUnitIndexToSpawn()
    {
        float[] index = new float[m_Units.Count];

        int ind = 0;
        float num = 0;
        for (int i = 0; i < index.Length; i++)
        {
            index[i] = Random.Range(0f, m_MaxWeight * m_Units[i].m_Weight);
            if (index[i] > num)
            {
                ind = i;
                num = index[i];
            }
        }

        return ind;
    }

    public void RemoveCharacterFromList(CharacterAnimated c)
    {
        m_UnitsOnScene.Remove(c);
    }
}
