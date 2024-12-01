using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;

    public HashSet<GameObject> playersWhoHit = new HashSet<GameObject>();

    public Sprite emptySprite, mysterySprite;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


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
        else if(collision.gameObject.TryGetComponent(out MarioAgent ma))
        {
            ma.AddReward(RewardSettings.RegularBlockPenalty);
        }
    }

    private void Hit(GameObject player)
    {
        MarioAgent ma = player.GetComponent<MarioAgent>();

        if (item != null)
        {
            GameObject itemObject = Instantiate(item, transform.position, Quaternion.identity);
            itemObject.transform.SetParent(player.transform, true);
            spriteRenderer.sprite = emptySprite;
            spriteRenderer.color = Color.black;
            transform.parent.parent.parent.GetComponent<GameManager>().powerUps.Add(itemObject);

            ma.AddReward(RewardSettings.MysteryBoxReward);

            if (itemObject.TryGetComponent(out PowerUp pUp))
            {
                pUp.playerWhoCanCollect = player;
            }
            else if (item.TryGetComponent(out BlockCoin bCoin))
            {
                player.GetComponent<MarioAgent>().coins++;
            }
        }
        else if (!ma.big)
        {
            ma.AddReward(RewardSettings.RegularBlockPenalty);
            
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetBlock()
    {
        if(item != null)
        {
            spriteRenderer.sprite = mysterySprite;
            spriteRenderer.color = Color.white;
        }
        playersWhoHit.Clear();
    }

}
