using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject /*farthestPlayer, lastFarthest, */flag;

    public HashSet<GameObject> enemies = new();
    public HashSet<GameObject> powerUps = new();
    public HashSet<PipeCheckpoint> pipes = new();
    public HashSet<GameObject> powerupBlocks = new();
    public HashSet<GameObject> regularBlocks = new();
    public HashSet<GameObject> hardBlocks = new();
    public HashSet<Checkpoint> checkpoints = new();
    public HashSet<GameObject> holes = new();

    public int sceneCount = 3;

    private void Awake()
    {

        //farthestPlayer = GameObject.FindGameObjectWithTag("Player");

        //DontDestroyOnLoad(gameObject);

        pipes.AddRange(FindObjectsOfType<PipeCheckpoint>());
        flag = GameObject.FindGameObjectWithTag("Flag");
        powerupBlocks.AddRange(GameObject.FindGameObjectsWithTag("PowerupBlock"));
        regularBlocks.AddRange(GameObject.FindGameObjectsWithTag("Block"));
        hardBlocks.AddRange(GameObject.FindGameObjectsWithTag("HardBlock"));
        checkpoints.AddRange(GameObject.FindObjectsOfType<Checkpoint>());
        holes.AddRange(GameObject.FindGameObjectsWithTag("Hole"));

        //Debug.Log($"Mystery Blocks: {powerupBlocks.Count}");
        //Debug.Log($"Regular Blocks: {regularBlocks.Count}");
        //Debug.Log($"Hard Blocks: {hardBlocks.Count}");
        //Debug.Log($"Pipes: {pipes.Count}");
    }

    public void ResetEnv()
    {
        DisableEnemies();

        if (GameObject.FindFirstObjectByType<MarioAgent>().training)
        {
            DisableHole();
            DisablePipes();
            DisableHardBlocks();
        }

        DisableMysteryBlocks();
        DisableRegularBlocks();
        DisableCheckpoints();
    }

    public void DisableEnemies()
    {
        foreach (var e in enemies)
        {
            e.gameObject.SetActive(false);
        }
    }

    public void EnableEnemies()
    {
        foreach (var e in enemies)
        {
            e.gameObject.SetActive(true);
        }
        Debug.Log("Enabled Enemies");
    }

    public void DisableHole()
    {
        foreach(var h in holes)
        {
            h.gameObject.SetActive(true);
        }
    }

    public void EnableHoles()
    {
        foreach( var h in holes)
        {
            h.gameObject.SetActive(false);
        }
    }

    public void DisablePipes()
    {
        foreach (var p in pipes)
        {
            p.transform.parent.gameObject.SetActive(false);
        }
    }

    public void EnablePipes()
    {
        foreach(var p in pipes)
        {
            p.transform.parent.gameObject.SetActive(true);
        }
        Debug.Log("Enabled Pipes");
    }

    public void DisableHardBlocks()
    {
        foreach(var b in hardBlocks)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void EnableHardBlocks()
    {
        foreach( var b in hardBlocks)
        {
            b.gameObject.SetActive(true);
        }
        Debug.Log("Enabled Hard Blocks");

    }

    public void DisableRegularBlocks()
    {
        foreach(var b in regularBlocks)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void EnableRegularBlocks()
    {
        foreach(var b in regularBlocks)
        {
            b.gameObject.SetActive(true);
        }
        Debug.Log("Enabled Regular Blocks");
    }

    public void DisableMysteryBlocks()
    {
        foreach(var b in powerupBlocks)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void EnableMysteryBlocks()
    {
        foreach(var b in powerupBlocks)
        {
            b.gameObject.SetActive(true);
            b.GetComponent<BlockHit>().ResetBlock();
            //Debug.Log(b.gameObject.name);
        }
        Debug.Log("Enabled Mystery Blocks");
    }

    public void DisableCheckpoints()
    {
        foreach(var c in checkpoints)
        {
            c.gameObject.SetActive(false);
        }
    }

    public void EnableCheckpoints()
    {
        foreach(var c in checkpoints)
        {
            c.gameObject.SetActive(true);
        }
        Debug.Log("Enabled Checkpoints");
    }

    public void GetRandomScene()
    {
        Scene active = SceneManager.GetActiveScene();
        SceneManager.LoadScene(Random.Range(0, sceneCount - 1));
    }
}
