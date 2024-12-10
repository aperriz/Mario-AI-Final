using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite squished;
    public Vector2 startPos;
    public Vector2 starDir;
    public HashSet<Collider2D> ignoredObjects = new();
    GameManager gm;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && 
                collision.gameObject.TryGetComponent(out MarioAgent player))
        {
            if(!(player.starpower || collision.transform.DotTest(transform, Vector2.down)))
            {
                player.Hit();
            }
            else if (player.starpower) 
            {
                player.AddReward(RewardSettings.EnemyHitReward);
                Hit();
            }
            else
            {
                Hit();
            }

        }
           
    }

    public void Hit(MarioAgent responsiblePlayer)
    {
        responsiblePlayer.enemiesKilled++;
        gameObject.SetActive(false);
    }

    public void Hit()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = startPos;
    }
}
