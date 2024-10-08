using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private float starthp;
    [SerializeField] public float hp;

    // Start is called before the first frame update
    void Start()
    {
        // Game Start HP
        hp = starthp;
    }

    public void TakeDamage()
    {
        if (hp <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage();
    }
}
