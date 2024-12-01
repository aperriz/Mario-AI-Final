using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject playerWhoCanCollect;

    public GameManager gm;

    private void Awake()
    {
        StartCoroutine(SetVars());
    }

    public enum Type
    {
        Coin,
        MagicMushroom,
        Starpower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {


        // Checks if the other object is a player, has hit the powerup block, and hasn't hit the powerup already
        if (other.CompareTag("Player") && 
            other.TryGetComponent(out MarioAgent player) &&
            other.gameObject == playerWhoCanCollect)
        {
            Collect(player);
        }
    }

    private void Collect(MarioAgent player)
    {
        switch (type)
        {
            case Type.Coin:
                player.coins++;
                player.AddReward(RewardSettings.CoinReward);
                break;

            case Type.MagicMushroom:
                if (!player.big)
                {
                    player.Grow();
                }
                break;

            case Type.Starpower:
                player.Starpower();
                break;
        }
        gm.powerUps.Remove(gameObject);
        player.powerups++;
        Destroy(gameObject);
    }

    public IEnumerator SetVars()
    {
        yield return new WaitForSeconds(0.01f);
        gm = transform.parent.GetComponent<MarioAgent>().gm;
        //Debug.Log(transform.parent.name);
        gm.powerUps.Add(gameObject);
    }
}
