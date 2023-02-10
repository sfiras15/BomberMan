using UnityEngine;

public class Mouvement : MonoBehaviour
{
    public GameManager gameManager;
    public Rigidbody2D rigid2D { get; private set; }
    // Variable to store the direction of movement
    Vector2 direction = Vector2.down;

    // Variable to store the direction of movement
    public KeyCode inputUp = KeyCode.Z;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputLeft = KeyCode.Q;

    // Speed of movement
    public float speed = 4.0f;

    // Animated sprite renderers for different directions
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererLeft;

    // Animated sprite renderers for the death spriteRenderer
    public AnimatedSpriteRenderer spriteRendererDeath;

    // Variable to store the currently active sprite renderer
    private AnimatedSpriteRenderer activeSpriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the input for movement is being pressed
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up,spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down,spriteRendererDown);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right,spriteRendererRight);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left,spriteRendererLeft);
        }
        else
        {
            SetDirection(Vector2.zero,activeSpriteRenderer);
        }
    }
    // Method to set the direction and the sprite renderer to the current direction
    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRenderer.idle = direction == Vector2.zero;
        activeSpriteRenderer = spriteRenderer;
    }
    private void FixedUpdate()
    {
        Vector2 position = rigid2D.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        // Moving the rigid body component to the new position
        rigid2D.MovePosition(position + translation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            // Start the death sequence
            DeathSequence();
        }
    }
    // DeathSequence is called when the player collides with an explosion
    public void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererDeath.enabled = true;
        Invoke(nameof(DeathSequenceEnded), 1.25f);
    }
    // Once the death sequence has ended the gameManager checks for the winner of the game
    public void DeathSequenceEnded()
    {
        gameObject.SetActive(false);
        gameManager.CheckWinner();
    }
}
