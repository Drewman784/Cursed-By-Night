using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float health = 20f;
    [SerializeField] public float damage = 10f;

    [SerializeField] public float lookRadius = 10f;

    NavMeshAgent agent;
    public Transform Target;
    [SerializeField] public float MoveSpeed = 10;
    public GameObject player;
    public Player movement;

    private void Awake()
    {
        movement = player.GetComponent<Player>();
    }

    private void Start()
    {
        Target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(Target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(Target.position);
        }

        // Movement towards the target
        Vector3 moveDir = (Target.position - transform.position).normalized;
        transform.position += (moveDir * MoveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy Health: " + health); // Debug log for health tracking

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
