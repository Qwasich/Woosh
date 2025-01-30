using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TaskEradicate : TaskBase, ISpawnerUtils
{
    //sS
    [SerializeField] private List<SpawnableUnit> m_Units;
    [SerializeField] private int m_SpawnCount;

    private List<CharacterAnimated> m_UnitsOnScene;
    private int m_MaxWeight;

    private void Start()
    {
        

        foreach (var unit in m_Units)
        {
            m_MaxWeight += unit.m_Weight;
        }
    }

    public override void SetActive(GameMaster m)
    {
        m_UnitsOnScene = new List<CharacterAnimated>(m_SpawnCount);
        base.SetActive(m);
        for (int i = 0; i < m_UnitsOnScene.Capacity; i++)
        {
            Vector2 pos = CalculateSpawnPosition();
            int ind = GetUnitIndexToSpawn();
            BehaviourNavmeshMovement bnm = Instantiate(m_Units[ind].m_Creature, pos, transform.rotation);
            CharacterAnimated ca = bnm.GetComponentInChildren<CharacterAnimated>();

            ca.SetSpawner(this);
            m_UnitsOnScene.Add(ca);
        }
    }

    private Vector2 CalculateSpawnPosition()
    {
        Vector2 pos;
        float x = Random.Range(transform.position.x - 5, transform.position.x + 5);
        float y = Random.Range(transform.position.y - 5, transform.position.y + 5);
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
        if (check)
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
