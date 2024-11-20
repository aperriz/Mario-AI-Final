using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public Camera m_Camera;
    private Rigidbody2D rb;
    private Collider2D capsuleCollider;
    [SerializeField]
    private MarioAgent player;

    public float maxX = 0;
    public Vector2 vel;
    private float input;

    [SerializeField]
    public float moveSpeed = 8f;
    [SerializeField]
    public float jumpHeight = 5f;
    [SerializeField]
    public float jumpTime = 1f;
    public int jumpCount = 0;

    public float gravity => (-2f * jumpHeight) / Mathf.Pow(jumpTime/2f, 2f);
    public float jumpForce => (2f * jumpHeight) / (jumpTime/2.5f);

    public bool againstWall = false;
    
    [SerializeField]
    public InputActionReference jumpAction, moveAction;

    [SerializeField]
    public bool jumping {
        get
        {
            return player.activeAnimator.GetBool("Jumping");
        }
        private set
        {
            player.activeAnimator.SetBool("Jumping", value);
        }
    }

    [SerializeField]
    public bool onGround { get; private set; }

    public bool falling => vel.y < 0f && !onGround;
    private MarioAgent agent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<Collider2D>();
        agent = GetComponent<MarioAgent>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        capsuleCollider.enabled = true;
        vel = Vector2.zero;
        jumping = false;
        m_Camera.gameObject.SetActive(true);

        //Debug.Log(jumpForce);
        //Debug.Log(gravity);
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
        capsuleCollider.enabled = false;
        vel = Vector2.zero;
        jumping = false;
        m_Camera.gameObject.SetActive(false);
    }

    public void DoMovement(int x, bool jump)
    {

        if (player.activeAnimator == null)
        {
            player.activeAnimator = player.smallAnimator;
            //Debug.Log("Fixed null animator");
        }

        HorrizontalMovement(x);

        onGround = Physics2D.Raycast(transform.position, Vector2.down, 0.55f, LayerMask.GetMask("Ground", "Default"));
        

        if (onGround)
        {
            GroundMovement(jump);
            jumping = false;
        }

        Gravity();

        if (vel.x > 0f)
        {
            player.activeAnimator.SetBool("Moving", true);
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (vel.x < 0f)
        {
            player.activeAnimator.SetBool("Moving", true);
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            player.activeAnimator.SetBool("Moving", false);
        }

    }

    private void HorrizontalMovement(float input)
    {
        if(input == 0f)
        {
            StartCoroutine(StillTimer());
        }
        else
        {
            StopAllCoroutines();
        }

        //input = moveAction.action.ReadValue<float>();
        vel.x = Mathf.MoveTowards(vel.x, input * moveSpeed, moveSpeed * Time.deltaTime);
        againstWall = false;
        RaycastHit2D ray = Physics2D.Raycast(rb.transform.position, Vector2.right, 0.25f, LayerMask.GetMask("Default", "Ground"));

        if(input == 0 && onGround)
        {
            vel.x = 0;
        }

        if (ray && !ray.transform.CompareTag("Checkpoint") && !jumping)
        {
            Debug.DrawLine(rb.transform.position, ray.transform.position, Color.red, Time.deltaTime);
            againstWall = true;

            vel.x = 0;
            //player.AddReward(RewardSettings.WallPenalty);
        }

        if (input > 0 && transform.position.x > agent.farthestPoint.x)
        {
            maxX = transform.position.x;
        }
    }


    private void FixedUpdate()
    {
        Vector2 pos = rb.position;
        pos += vel * Time.fixedDeltaTime;

        Vector2 leftEdge = m_Camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = m_Camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        pos.x = Mathf.Clamp(pos.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        if(pos.x == leftEdge.x + .5f)
        {
            vel.x = 0;
            //player.AddReward(RewardSettings.WallPenalty);
        }
        
        rb.MovePosition(pos);
    }

    private void GroundMovement(bool jump)
    {
        vel.y = Mathf.Max(vel.y, 0f);

        if (jump)
        {
            vel.y = jumpForce;
            jumping = true;
            jumpCount++;
            player.AddReward(RewardSettings.JumpPenalty);
            

            player.activeAnimator.SetBool("Jumping", true);
            //Debug.Log("Jump");
        }
    }

    private void Gravity()
    {

        bool falling = vel.y < 0f || !jumpAction.action.IsPressed();
        float multiplier = falling ? 2f : 1f;

        //Debug.Log("Falling: " + falling);

        vel.y += gravity * multiplier * Time.deltaTime;
        vel.y = Mathf.Max(vel.y, gravity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                vel.y = jumpForce / 2f;
                jumping = true;
                player.AddReward(RewardSettings.EnemyHitReward);
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // Stop vertical movement if mario bonks his head
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                vel.y = 0f;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flag"))
        {
            player.AddReward(RewardSettings.WinReward);
            player.EndEpisode();
        }
    }

    private IEnumerator StillTimer()
    {
        yield return new WaitForSeconds(0.5f);
        player.still = true;
        StopAllCoroutines();
    }
}
