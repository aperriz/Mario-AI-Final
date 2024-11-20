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
            player.won = true;
            player.AddReward(RewardSettings.WinReward);
            player.AddReward(other.transform.position.x - poleBottom.position.y);

            player.flagHeight = other.transform.position.x - poleBottom.position.y;

            if (player.movement.jumpCount > 50)
            {
                player.AddReward(RewardSettings.JumpPenalty);
            }

            player.AddReward(RewardSettings.WinReward - player.cumulativeTimePenalty);

            foreach (GameObject p in GameManager.players)
            {
                if(p.TryGetComponent(out MarioAgent ma) && ma != player)
                {
                    if(ma.enabled)
                    {
                        ma.StopAllCoroutines();
                        player.enabled = false;
                        return;
                    }
                }
            }

            player.StopAllCoroutines();
            GameManager.episode++;
            Debug.Log("Episode " + GameManager.episode);
            player.EndEpisode();
        }
    }

}
