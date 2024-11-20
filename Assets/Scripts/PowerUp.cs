using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject playerWhoCanCollect;

    private void Awake()
    {
        GameManager.powerUps.Add(gameObject);
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
        GameManager.powerUps.Remove(gameObject);
        player.powerups++;
        Destroy(gameObject);
    }
}
