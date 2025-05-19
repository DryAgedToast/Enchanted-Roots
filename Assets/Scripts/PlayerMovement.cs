using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true;
    float jumpPower = 5f;
    bool isJumping = false;
    public bool canMove = true;
    public Sprite idleSprite;
    public Sprite movingSprite;
    
    private SpriteRenderer sr;
    public static Vector3 spawnPosition;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        transform.position = spawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            horizontalInput = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontalInput) > 0.1f)
                sr.sprite = movingSprite;
            else
                sr.sprite = idleSprite;

            FlipSprite();

            if(Input.GetButtonDown("Jump") && !isJumping)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                isJumping = true;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }
}
