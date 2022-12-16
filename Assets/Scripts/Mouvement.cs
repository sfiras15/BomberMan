using UnityEngine;

public class Mouvement : MonoBehaviour
{
    public GameManager gameManager;
    public Rigidbody2D rigid2D { get; private set; }
    Vector2 direction = Vector2.down;
    public KeyCode inputUp = KeyCode.Z;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputLeft = KeyCode.Q;
    public float speed = 4.0f;
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererDeath;
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
        transform.position = new Vector2(transform.position.x + direction.x * speed * Time.fixedDeltaTime,
            transform.position.y + direction.y * speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }
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
    public void DeathSequenceEnded()
    {
        gameObject.SetActive(false);
        gameManager.CheckWinner();
    }
}
