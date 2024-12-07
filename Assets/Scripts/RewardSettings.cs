using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class RewardSettings
{
    [Header("Positive Events")]
    public static float KoopaShellReward = 0.001f; // Pushing koopa shell
    public static float StarPowerReward = 0.002f; // When mario hits star
    public static float CoinReward = 0.0005f; // When Mario collects a coin
    public static float GrowReward = 0.0075f; // Magic Mushroom
    public static float WinReward = 1f; // Flag
    public static float EnemyHitReward = 0.02f; // Kill enemy
    public static float CheckpointReward = 0.1f; // Cross checkpoint
    public static float MoveRightReward = 0.00001f;
    public static float MysteryBoxReward = 0.01f;

    [Header("Negative Events")]
    public static float ShrinkPenalty = -(GrowReward);
    public static float DeathPenalty = -(WinReward);
    public static float WallPenalty = -0.0003f; // If running into a wall while not jumping or the left bound of the 
    public static float TimePenalty = -0.00001f; // Per tick of the game, meant to increase speed of Mario 0.001
    public static float JumpPenalty = -0.0001f;  // If over 50 jumps, average needed is 20-30
    public static float RegularBlockPenalty = -0.00001f;

    public static bool loadingNewLevel = false;
}
