using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class MarioAgent : Agent
{
    public GameManager gm;

    public int par = 0;

    [SerializeField]
    private Transform flag;
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public PlayerMovement movement { get; private set; }

    [SerializeField]
    private SpriteRenderer bigRenderer;
    [SerializeField]
    private SpriteRenderer smallRenderer;
    private SpriteRenderer activeRenderer;
    public Animator bigAnimator, smallAnimator;
    [SerializeField]
    private Animator _activeAnimator;
    private Vector2 startPos;

    public int checkpointsHit = 0;
    public HashSet<GameObject> enemies = new();

    [SerializeField]
    private Camera sideCamera;


    [Header("Agent Stats")]
    public int deaths = 0;
    public int wins = 0;
    public float cumulativeScore = 0;
    public Vector2 farthestPoint;
    public int enemiesKilled = 0, powerups = 0;
    public bool won = false, dead = false, still = false;
    public float flagHeight = 0, cumulativeTimePenalty = 0;
    public bool training = false;

    public Animator activeAnimator
    {
        get
        {
            return _activeAnimator;
        }
        set
        {
            if (value != _activeAnimator)
            {
                _activeAnimator.enabled = false;
                value.enabled = true;
                _activeAnimator = value;
                //Debug.Log("Changed renderer");
            }
        }
    }

    public int coins = 0;

    public bool starpower { get; private set; }
    public bool big => bigRenderer.enabled;

    private void Awake()
    {
        gm = transform.parent.GetComponentInParent<GameManager>();

        capsuleCollider = GetComponent<CapsuleCollider2D>();
        movement = GetComponent<PlayerMovement>();
        activeRenderer = smallRenderer;
        activeAnimator = smallAnimator;

        activeAnimator.enabled = true;

        startPos = transform.position;

        //DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        
    }

    private void FixedUpdate()
    {
        cumulativeScore = GetCumulativeReward();

        cumulativeTimePenalty += RewardSettings.TimePenalty;
    }

    

    public void Hit()
    {
        if (!starpower)
        {
            if (big)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    public void Death()
    {


        deaths++;
        //if(movement.jumpCount > 50)
        //{
        //    AddReward(RewardSettings.JumpPenalty * (movement.jumpCount - 50));
        //}

        AddReward(RewardSettings.DeathPenalty);
        movement.enabled = false;
        dead = true;

        /*foreach (GameObject player in gm.players)
        {
            if (player.activeSelf && player.GetComponent<MarioAgent>().enabled && player != gameObject)
            {
                gameObject.SetActive(false);
                return;
            }
        }*/

        foreach (GameObject p in gm.powerUps)
        {
            Destroy(p);
        }

        /*MarioAgent bestPlayer = this;
        foreach (GameObject player in gm.players)
        {
            if (player.GetComponent<MarioAgent>().GetCumulativeReward() > bestPlayer.GetCumulativeReward())
            {
                bestPlayer = player.GetComponent<MarioAgent>();
            }
        }*/

        StopAllCoroutines();

        //Debug.Log("Best player: " + bestPlayer.gameObject.transform.parent.name + " with " + bestPlayer.GetCumulativeReward());
        Debug.Log("Episode " +  CompletedEpisodes);

        Debug.Log(GetCumulativeReward());

        //gm.GetRandomScene();
        EndEpisode();
        
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;
        activeAnimator = bigAnimator;

        AddReward(RewardSettings.GrowReward);

    }

    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;
        activeAnimator = smallAnimator;

        AddReward(RewardSettings.ShrinkPenalty);
    }

    public void Starpower()
    {
        /*AddReward(RewardSettings.StarPowerReward);*/
        StartCoroutine(StarpowerBuff());
    }

    private IEnumerator StarpowerBuff()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.color = Color.white;
        starpower = false;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveX = 0;
        switch (actions.DiscreteActions[1])
        {
            case 2:
                moveX = -1;
                break;
            case 1:
                moveX = 1;
                break;
            case 0:
                moveX = 0;
                break;
        }

        bool jumping = actions.DiscreteActions[0] == 1;

        movement.DoMovement(moveX, jumping);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.position - startPos);
        sensor.AddObservation((Vector2)flag.position - startPos);
        sensor.AddObservation(GetComponent<Rigidbody2D>().velocity);
        sensor.AddObservation(starpower);
        sensor.AddObservation(big);
        sensor.AddObservation(checkpointsHit);
    }

    public override void OnEpisodeBegin()
    {
        RewardSettings.loadingNewLevel = false;
        movement.moved = false;

        gm.SetupArrays();
        gm.ResetEnv();
        
        //Debug.Log(Academy.Instance.EnvironmentParameters.GetWithDefault("enabled_items", 0));

        if (training)
        {
            par = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("enabled_items", 0));
            switch (par)
            {
                case 0:
                    break;
                case 1:
                    gm.EnableMysteryBlocks();
                    break;
                case 2:
                    gm.EnableMysteryBlocks();
                    gm.EnableRegularBlocks();
                    break;
                case 3:
                    gm.EnableHardBlocks();
                    gm.EnableMysteryBlocks();
                    gm.EnableRegularBlocks();
                    gm.EnablePipes();
                    gm.EnableCheckpoints();
                    break;
                case 4:
                    gm.EnableHardBlocks();
                    gm.EnableMysteryBlocks();
                    gm.EnableRegularBlocks();
                    gm.EnableEnemies();
                    gm.EnablePipes();
                    gm.EnableCheckpoints();
                    break;

                case >= 5:
                    gm.EnableHardBlocks();
                    gm.EnableMysteryBlocks();
                    gm.EnableRegularBlocks();
                    gm.EnableEnemies();
                    gm.EnableHoles();
                    gm.EnablePipes();
                    gm.EnableCheckpoints();
                    break;

                default:
                    break;
            }

            Debug.Log("Param selection: " + par);
        }
        else
        {
            gm.EnableMysteryBlocks();
            gm.EnableRegularBlocks();
            gm.EnableEnemies();
            gm.EnableHardBlocks();
            gm.EnablePipes();
            gm.EnableHoles();
            gm.EnableCheckpoints();
        }

        StopAllCoroutines();
        StartCoroutine(maxTime());

        foreach (PipeCheckpoint p in gm.pipes)
        {
            p.ResetHitList();
        }

        foreach (Checkpoint c in gm.checkpoints)
        {
            c.ResetHitList();
        }

        foreach(GameObject p in gm.powerUps)
        {
            Destroy(p);
        }

        /*foreach (GameObject player in gm.players)
        {
            player.SetActive(true);
            player.GetComponent<MarioAgent>().enabled = true;
        }*/

        //sideCamera.gameObject.transform.position = transform.position;

        movement.m_Camera.transform.position = new Vector3(11.5f, 6.5f, -10);
        transform.localPosition = new Vector3(2, 2, -1);
        movement.vel = Vector2.zero;
        movement.enabled = true;
        still = false;
        cumulativeTimePenalty = 0;


        if (big)
        {
            smallRenderer.enabled = true;
            bigRenderer.enabled = false;
            activeRenderer = smallRenderer;
            activeAnimator = smallAnimator;

            capsuleCollider.size = new Vector2(1f, 1f);
            capsuleCollider.offset = new Vector2(0f, 0f);

        }
        starpower = false;
        smallRenderer.color = Color.white;
        bigRenderer.color = Color.white;

        //Debug.Log($"{transform.parent.name} Jump count: {movement.jumpCount}");

        movement.jumpCount = 0;
        coins = 0;
        powerups = 0;
        enemiesKilled = 0;
        checkpointsHit = 0;
        dead = false;
        won = false;
        farthestPoint = transform.position;
        flagHeight = 0;

        SetReward(0);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        //Debug.Log(discreteActions[0]);

        if (movement.jumpAction.action.IsPressed())
        {
            discreteActions[0] = 1;
        }
        else
        {
            discreteActions[0] = 0;
        }

        switch (movement.moveAction.action.ReadValue<float>())
        {
            case < 0:
                discreteActions[1] = 2; break;
            case > 0:
                discreteActions[1] = 1; break;
            case 0:
                discreteActions[1] = 0; break;
        }
    }

    private IEnumerator maxTime()
    {
        yield return new WaitForSeconds(150);
        Death();
    }

}
