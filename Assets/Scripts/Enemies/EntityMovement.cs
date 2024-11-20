using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 dir = Vector2.left;

    public Vector2 initialDir;

    public Rigidbody2D rb;
    private Vector2 vel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialDir = dir;
    }

    private void FixedUpdate()
    {
        vel.x = dir.x * speed;
        vel.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + vel * Time.fixedDeltaTime);

        if (rb.Raycast(dir))
        {
            dir = -dir;
        }

        if (rb.Raycast(Vector2.down))
        {
            vel.y = Mathf.Max(vel.y, 0f);
        }

        if (dir.x > 0f)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (dir.x < 0f)
        {
            transform.localEulerAngles = Vector3.zero;
        }
    }

}
