using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    GameManager gm;
    public Sprite shellSprite, standardSprite;
    public float shellSpeed = 12f;

    public Vector2 startPos;

    private bool shelled;
    private bool pushed;

    public Collider2D[] ignoredObjects;

    public MarioAgent playerWhoPushed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out MarioAgent player))
        {
            if (player.starpower)
            {
                player.AddReward(RewardSettings.EnemyHitReward);

                Collider2D collider = collision.gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
                ignoredObjects.Append(collider);
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
                GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shelled && collision.CompareTag("Player") && collision.TryGetComponent(out MarioAgent player))
        {
            if (!pushed)
            {
                Vector2 dir = new(transform.position.x - collision.transform.position.x, 0f);
                playerWhoPushed = player;
                PushShell(dir);
            }
            else
            {
                if (!player.starpower)
                {
                    player.Hit();
                }
            }
        }
        else if (shelled && collision.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            GetComponent<EntityMovement>().rb.velocity = collision.gameObject.GetComponent<EntityMovement>().rb.velocity;
            collision.gameObject.GetComponent<EntityMovement>().rb.velocity = Vector2.zero;
        }
        else if (shelled && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Vector2 velocity = GetComponent<EntityMovement>().rb.velocity;

            if(collision.gameObject.TryGetComponent(out Goomba gooba))
            {
                gooba.Hit(playerWhoPushed);
            }

            else if (collision.gameObject.TryGetComponent(out Koopa poopa))
            {
                poopa.Hit(playerWhoPushed);
            }

            GetComponent<EntityMovement>().rb.velocity = velocity;
        }
    }

    private void EnterShell()
    {
        shelled = true;
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
    }

    private void PushShell(Vector2 direction)
    {
        
        pushed = true;

        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.dir = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
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
        standardSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        shelled = false;
        playerWhoPushed = null;
        GetComponent<EntityMovement>().speed = 2f;
        GetComponent<SpriteRenderer>().sprite = standardSprite;

    }

    private IEnumerator setVars()
    {
        yield return new WaitForSeconds(0.1f);

        shelled = false;
        GetComponent<Animator>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = standardSprite;
        GetComponent<AnimatedSprite>().enabled = true;
        GetComponent<EntityMovement>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        GetComponent<EntityMovement>().rb.velocity = Vector2.zero;
        pushed = false;
        playerWhoPushed = null;

        foreach (Collider2D c in ignoredObjects)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), c, false);
        }
    }
}
