using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out MarioAgent player))
        {
            //Adds win reward
            player.won = true;
            player.wins++;

            player.AddReward(RewardSettings.WinReward - player.cumulativeTimePenalty);

            Debug.Log(player.GetCumulativeReward());

            player.StopAllCoroutines();

            player.EndEpisode();
            
        }
    }

}
