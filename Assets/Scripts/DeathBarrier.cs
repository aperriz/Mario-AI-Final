using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Kills anything that touches the barrier
        if (collision.gameObject.TryGetComponent(out MarioAgent agent))
        {
            agent.Death();
        }
        else if (collision.gameObject.TryGetComponent(out Koopa k))
        {
            k.Hit();
        }
        else if (collision.gameObject.TryGetComponent(out Goomba g))
        {
            g.Hit();
        }
    }
}
