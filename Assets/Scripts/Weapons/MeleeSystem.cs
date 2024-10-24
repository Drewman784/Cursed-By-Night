using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeleeSystem : MonoBehaviour
{
    // Melee stats
    public int damage;
    public float timeBetweenAttacks, attackRange, attackDelay, timeBetweenHits;
    public bool allowButtonHold;
    public Transform attackPoint; // The point from which the melee hit is checked (e.g. in front of the player)
    public LayerMask whatIsEnemy;

    // Bools
    bool attacking, readyToAttack;

    // Reference
    public Camera fpsCam;
    public Transform camTransform;
    public TextMeshProUGUI text; // For feedback, if needed

    private void Awake()
    {
        readyToAttack = true;
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        // Use the same button input for melee as you did for shooting
        if (allowButtonHold) attacking = Input.GetKey(KeyCode.Mouse0);
        else attacking = Input.GetKeyDown(KeyCode.Mouse0);

        // Trigger the attack if ready
        if (readyToAttack && attacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        readyToAttack = false;

        // Delay before the attack happens (e.g., time for an animation to play)
        Invoke("DealDamage", attackDelay);

        // Reset attack cooldown
        Invoke("ResetAttack", timeBetweenAttacks);
    }

    private void DealDamage()
    {
        // Check for enemies in attack range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, whatIsEnemy);

        // Apply damage to each enemy hit
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);

            Enemy target = enemy.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.DrawLine(camTransform.position, enemy.transform.position, Color.green, 1f);
            }
            else
            {
                Debug.DrawRay(camTransform.position, camTransform.forward * 100f, Color.red, 1f);
            }
        }
    }

    private void ResetAttack()
    {
        readyToAttack = true;
    }

    // Optional: Visualize the melee attack range in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
