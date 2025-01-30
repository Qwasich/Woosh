using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CharacterAnimated : Character
{
    [SerializeField] private NavMeshAgent m_Agent;
    public NavMeshAgent Agent => m_Agent;

    [SerializeField] private SpriteRenderer m_Body;

    [SerializeField] private PerformAttack m_Attack;

    [SerializeField] private Sprite m_StandingSprite;
    [SerializeField] private Sprite[] m_MoveSprites;
    [SerializeField] private Sprite[] m_AttackSprites;

    [SerializeField] private float m_AnimationSpeed;

    private float m_AnimationTimer;

    private Sprite[] m_ActiveLoop;

    private int m_CurrentSprite = 0;

    private bool m_Animate = false;

    private float m_CurrentVelocity => m_Agent.velocity.magnitude;

    private Vector3 target;

    private MapGeneralSpawner m_Spawner;

    protected override void Awake()
    {
        m_Body.sprite = m_StandingSprite;
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (m_Attack != null && m_Attack.AllowedToAttack && !m_Agent.isStopped) m_Agent.isStopped = true;
        if (m_Attack != null && !m_Attack.AllowedToAttack && m_Agent.isStopped) m_Agent.isStopped = false;

        m_AnimationTimer += Time.deltaTime;

        if (m_AnimationTimer >= m_AnimationSpeed)
        {
            m_AnimationTimer = 0;
            if (m_Animate)
            {
                m_Body.sprite = m_ActiveLoop[m_CurrentSprite];
                m_CurrentSprite++;
                if (m_ActiveLoop.Length <= m_CurrentSprite) m_CurrentSprite = 0;
            }

        }
    }

    private void FixedUpdate()
    {
        if (m_CurrentVelocity <= 0.1 && m_Body.sprite != m_StandingSprite && (m_Attack == null || !m_Attack.AllowedToAttack))
        {
            m_Body.sprite = m_StandingSprite;
            m_CurrentSprite = 0;
            m_Animate = false;

            
        }
        if (m_CurrentVelocity > 0.1 && m_ActiveLoop != m_MoveSprites && (m_Attack == null || !m_Attack.AllowedToAttack))
        {
            m_ActiveLoop = m_MoveSprites;
            m_CurrentSprite = 0;
            m_Animate = true;

        }
        if (m_Attack != null && m_Attack.AllowedToAttack && m_ActiveLoop != m_AttackSprites)
        {
            m_ActiveLoop = m_AttackSprites;
            m_CurrentSprite = 0;
            m_Animate = true;
        }
        if (m_Agent.hasPath == false && !m_Agent.isStopped) m_Body.sprite = m_StandingSprite;



        if (m_Attack != null && m_Attack.m_CurrentTarget != Vector3.zero && m_Attack.IsRanged) target = m_Attack.m_CurrentTarget - transform.position;
        else if (m_Attack != null && m_Attack.AllowedToAttack && !m_Attack.IsRanged) target = m_Agent.pathEndPosition - transform.position;
        else if (m_Attack != null && m_Agent != null && m_Agent.velocity.magnitude <= 0.2) target = m_Agent.pathEndPosition - transform.position;
        else target = m_Agent.velocity;
        
        if(m_Agent.hasPath) transform.up = target;

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
    }

    public void SetSpawner(MapGeneralSpawner sp)
    {
        m_Spawner = sp;
    }

    public void ReturnSpawner()
    {
        if (m_Spawner != null) m_Spawner.RemoveCharacterFromList(this);

    }


    protected override void OnKill(NavMeshAgent ogj)
    {
        ReturnSpawner();
        ogj = m_Agent;
        base.OnKill(ogj);
    }


}
