using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Reference to the 3 renderers of the explosion
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    public void SetActivieRenderer(AnimatedSpriteRenderer spriteRenderer)
    {
        start.enabled = spriteRenderer == start;
        middle.enabled = spriteRenderer == middle;
        end.enabled = spriteRenderer == end;
    }

    public void SetRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}
