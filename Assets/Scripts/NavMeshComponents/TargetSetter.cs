
using UnityEngine;

public class TargetSetter : MonoBehaviour
{
    [SerializeField] private BehaviourNavmeshMovement bnm;

    private void Start()
    {
        if (bnm.Target != null) Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bnm.Target != null)
        {
            Destroy(this);
            return;
        }
        if (!collision.CompareTag("Player")) return;
        if (!collision.transform.TryGetComponent<Character>(out var c)) return;

        bnm.SetTarget(c.transform);
        Destroy(gameObject);
    }


}
