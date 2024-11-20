using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static float farthestX;
    public static GameObject farthestPlayer, lastFarthest, flag;
    public static int episode = 0;

    public static HashSet<GameObject> enemies = new();
    public static float lerpTime = 1;
    public static HashSet<GameObject> players = new();
    public static HashSet<GameObject> powerUps = new();
    public static HashSet<PipeCheckpoint> pipes = new();
    public static HashSet<GameObject> blocks = new();
    public static HashSet<Checkpoint> checkpoints = new();

    private void Awake()
    {
        if(Instance != null) { Destroy(this.gameObject); }

        Instance = this;

        farthestPlayer = GameObject.FindGameObjectWithTag("Player");

        DontDestroyOnLoad(gameObject);

        pipes.AddRange(FindObjectsOfType<PipeCheckpoint>());
        flag = GameObject.FindGameObjectWithTag("Flag");
        blocks.AddRange(GameObject.FindGameObjectsWithTag("Block"));
        checkpoints.AddRange(GameObject.FindObjectsOfType<Checkpoint>());
    }

    private void Update()
    {

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                if (player.transform.position.x > farthestPlayer.transform.position.x)
                {
                    SideCamera.player = player.transform;
                    farthestPlayer = player;
                }
                else if (!farthestPlayer.activeSelf)
                {
                    //Debug.Log("[SideCamera] Lost farthest player");
                    farthestPlayer = player;
                    SideCamera.player = player.transform;
                }
            }

        }

        if (farthestPlayer == lastFarthest)
        {
            lerpTime = 1;
        }
        else
        {
            lerpTime = Time.deltaTime;
        }

        lastFarthest = farthestPlayer;

    }
}
