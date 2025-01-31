using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public struct SpawnableUnit
{
    public BehaviourNavmeshMovement m_Creature;
    public int m_Weight;
}

public interface ISpawnerUtils
{
    public void RemoveCharacterFromList(CharacterAnimated c);
}

public class MapGeneralSpawner : MonoBehaviour, ISpawnerUtils
{
    [SerializeField] private List<SpawnableUnit> m_Units;

    [SerializeField] private float m_SpawnTimer;
    [SerializeField] private int m_SpawnAmount;
    [Header("SpawnRestrictions")]
    [SerializeField] private Transform m_SpawnArea;
    [SerializeField] private Transform m_PlayerPosition;
    [SerializeField] private Transform m_CameraPosition;
    [SerializeField] private float m_SafeDistanceFromObject;

    [SerializeField] private int m_MaxObjects;


    private List<CharacterAnimated> m_UnitsOnScene;
    private int m_MaxWeight;
    private float m_Timer = 0;

    private void Start()
    {
        m_UnitsOnScene = new List<CharacterAnimated>(m_MaxObjects);

        foreach (var unit in m_Units)
        {
            m_MaxWeight += unit.m_Weight;
        }
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer < m_SpawnTimer || m_PlayerPosition == null) return;
        if (m_UnitsOnScene.Count == m_UnitsOnScene.Capacity) return;

        for (int i = 0; i < m_SpawnAmount; i++)
        {
            Vector2 pos = CalculateSpawnPosition();
            int ind = GetUnitIndexToSpawn();
            BehaviourNavmeshMovement bnm = Instantiate(m_Units[ind].m_Creature, pos, transform.rotation);
            CharacterAnimated ca = bnm.GetComponentInChildren<CharacterAnimated>();
            
            ca.SetSpawner(this);
            bnm.SetTarget(m_PlayerPosition);
            m_UnitsOnScene.Add(ca);
            m_Timer = 0;
            if (m_UnitsOnScene.Count == m_UnitsOnScene.Capacity) break;
        }


    }

    private Vector2 CalculateSpawnPosition()
    {
        float xoffset = m_SpawnArea.localScale.x / 2;
        float yoffset = m_SpawnArea.localScale.y / 2;
        Vector2 pos;

        while (true)
        {
            float x = UnityEngine.Random.Range(-xoffset + 5, xoffset - 5);
            float y = UnityEngine.Random.Range(-yoffset + 5, yoffset - 5);
            bool pass = true;
            pos = new Vector2(x, y);
            
            if (Vector2.Distance(m_PlayerPosition.transform.position, pos) < m_SafeDistanceFromObject) pass = false;
            if (Vector2.Distance(m_CameraPosition.transform.position, pos) < m_SafeDistanceFromObject) pass = false;

            if (pass == true) break;

        }

        return pos;
    }

    private int GetUnitIndexToSpawn()
    {
        float[] index = new float[m_Units.Count];

        int ind = 0;
        float num = 0;
        for (int i = 0; i < index.Length; i++)
        {
            index[i] = UnityEngine.Random.Range(0f,m_MaxWeight * m_Units[i].m_Weight);
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


