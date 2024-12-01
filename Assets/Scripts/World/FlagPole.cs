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
            player.wins++;

            //player.AddReward((other.transform.position.y - poleBottom.position.y)/100);

            //if (player.movement.jumpCount > 50)
            //{
            //    player.AddReward(RewardSettings.JumpPenalty * (player.movement.jumpCount - 50));
            //}

            player.AddReward(RewardSettings.WinReward - player.cumulativeTimePenalty);

            /*foreach (GameObject p in GameManager.players)
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
            }*/

            Debug.Log(player.GetCumulativeReward());

            player.StopAllCoroutines();
            player.EndEpisode();
            player.SetReward(0);
        }
    }

}
