using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;

    public HashSet<GameObject> playersWhoHit = new HashSet<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            !playersWhoHit.Contains(collision.gameObject)) 
        {

            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Hit(collision.gameObject);
                playersWhoHit.Add(collision.gameObject);
            }
        }
    }

    private void Hit(GameObject player)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;

        if (item != null)
        {
            GameObject itemObject = Instantiate(item, transform.position, Quaternion.identity);
            if(itemObject.TryGetComponent(out PowerUp pUp))
            {
                pUp.playerWhoCanCollect = player;
            }
            else if(item.TryGetComponent(out BlockCoin bCoin))
            {
                player.GetComponent<MarioAgent>().coins++;
            }
        }
    }

    

}
