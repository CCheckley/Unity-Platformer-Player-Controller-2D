using UnityEngine;

// Ensures existance of required components attached to gameobject
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    // Rigidbody variable to hold component reference
    new Rigidbody2D rigidbody2D;

    [SerializeField] float movementSpeed = 100.0f;
    [SerializeField] float jumpForce = 500.0f;
    [SerializeField] LayerMask floorLayer; // To check floor collision

    bool isFacingRight; // Checks to see if player direction is consistent with movement direction

    void Awake()
    {
        // Setup rigidbody to be affected by physics and to not rotate
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.mass = 1.0f; // mass = 1kg
        rigidbody2D.isKinematic = false;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody2D.angularDrag = 0.0f;
        rigidbody2D.gravityScale = 10.0f; // faster falling with higher gravity

        isFacingRight = (transform.localScale.x > 0); // set is facing right based on initial scale
    }

    void Update()
    {
        Move(Input.GetAxisRaw("Horizontal"), Input.GetButtonDown("Jump")); // called every frame
    }

    public void Move(float xInput, bool isJumping)
    {
        // horizontal movement
        float xDelta = (xInput * movementSpeed) * Time.deltaTime; // multiplied by time.deltatime to make framerate independant
        // vertical movement, can only jump if colliding with floor layer
        float yDelta = (isJumping && HasLanded()) ? jumpForce * Time.deltaTime: rigidbody2D.velocity.y;

        rigidbody2D.velocity = new Vector2(xDelta, yDelta); // manipulate rigidbody velocity directly for precise movement

        // set local scale x based on horizontal velocity, -x = left, +x = right
        if ((xDelta > 0 && !isFacingRight) || (xDelta < 0 && isFacingRight)) { FlipHorizontal(); }
    }

    public void FlipHorizontal()
    {
        // set target scale to current local
        Vector3 targetScale = transform.localScale;
        targetScale.x = -transform.localScale.x; // manipulate target scale
        transform.localScale = targetScale; // set local scale to new target scale

        isFacingRight = !isFacingRight; // set is facing right to the opposite of current value
    }

    public bool HasLanded()
    {
        return rigidbody2D.IsTouchingLayers(floorLayer); // check collision with floor layer and return result
    }
}