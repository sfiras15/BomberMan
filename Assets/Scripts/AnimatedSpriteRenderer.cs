using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    public float animationTime = 0.25f;
    private int currentFrame;
    public Sprite[] animationSprites;
    public Sprite idleFrame;
    public bool idle = true;
    public bool loop = true;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }


    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    public void NextFrame()
    {
        currentFrame++;
        if (currentFrame >= animationSprites.Length && loop)
        {
            currentFrame = 0;
        }
        if (idle)
        {
            spriteRenderer.sprite = idleFrame;
        }
        else if (currentFrame < animationSprites.Length && currentFrame >=0)
        {
            spriteRenderer.sprite = animationSprites[currentFrame];
        }

    }


    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }



}
