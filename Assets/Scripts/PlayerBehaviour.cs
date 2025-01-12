using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")] 
    public float horizontalForce;
    public float verticalForce;

    // Handle Grounded
    [Header("Ground Handler")]
    public bool isGrounded;
    public Transform ground;
    public float groundRadius;
    public LayerMask groundLayerMask;


    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
    }

    private void Move()
    {
        if (isGrounded)
        {
            // Keyboard Input
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

            // Touch Input
            Vector2 worldTouch = new Vector2();
            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            // Check for Flip

            if (x != 0)
            {
                x = FlipAnimation(x);
            } 
            


            float horizontalMoveForce = x * horizontalForce;
            float jumpMoveForce = jump * verticalForce; 
            float mass = rigidbody.mass * rigidbody.gravityScale;


            rigidbody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce) * mass);
            rigidbody.velocity *= 0.99f;
        }

    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(ground.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    private float FlipAnimation(float x)
    {
        // depending on direction scale across the x-axis either 1 or -1
        x = (x > 0) ? 1 : -1;
        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }


    // UTILITIES

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ground.position, groundRadius);
    }

}
