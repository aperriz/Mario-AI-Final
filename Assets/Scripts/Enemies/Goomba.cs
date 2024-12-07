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
            }

            Collider2D collider = collision.gameObject.GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
            player.enemies.Remove(this.gameObject);
            Hit();
        }
        /*else if (collision.gameObject.layer == LayerMask.GetMask("Shell"))
        {
            Hit();

            
        }*/
           
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

    private void Awake()
    {
        startPos = transform.position;
        
    }

    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        foreach (Collider2D c in ignoredObjects)
        {
            if(c != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), c, false);
                ignoredObjects.Remove(c);
            }
            
        }
    }
}
