using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCheckpoint : MonoBehaviour
{
    private HashSet<MarioAgent> playersWhoHit = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out MarioAgent player))
        {
            if (!playersWhoHit.Contains(player))
            {
                //Adds checkpoint reward
                player.checkpointsHit++;
                player.AddReward(RewardSettings.CheckpointReward);
                playersWhoHit.Add(player);
            }
        }
    }

    public void ResetHitList()
    {
        playersWhoHit.Clear();
    }

    public void OnEnable()
    {
        ResetHitList();
    }

}
