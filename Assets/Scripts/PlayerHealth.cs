using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private float player_starthp;
    [SerializeField] public float player_hp;

    // Start is called before the first frame update
    void Start()
    {
        // Game Start HP
        player_hp = player_starthp;
    }

    public void Player_TakeDamage()
    {
        if (player_hp <= 0f)
        {
            Player_Perish();
        }
    }

    void Player_Perish()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    // Update is called once per frame
    void Update()
    {
        Player_TakeDamage();
    }
}
