using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BombController : MonoBehaviour
{
    // Variables for controlling bomb spawning
    [Header("Bomb")]
    public KeyCode spawnBombInput = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombRemaining;

    // Variables for controlling the explosion
    [Header("Explosion")]
    public Explosion explosionPrefab;
    // Layer mask for objects that can be destroyed by the explosion
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 2;

    // Variables for controlling the destructable objects
    [Header("Destruction")]
    public BrickDestruction destructableBrickPrefab;
    // Reference to the bricks that can be destroyed
    public Tilemap destructableTileMap;

    private void OnEnable()
    {
        bombRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombRemaining > 0 && Input.GetKeyDown(spawnBombInput))
        {
            StartCoroutine(PlaceBomb());
        }
    }
    public IEnumerator PlaceBomb()
    {
        // Placing the bomb at the current player position
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        GameObject bomb = Instantiate(bombPrefab,position,Quaternion.identity);
        bombRemaining--;

        // Wait for the fuse time
        yield return new WaitForSeconds(bombFuseTime);

        // Instantiate the core of the explosion at the current bomb position
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActivieRenderer(explosion.start);
        Destroy(explosion.gameObject, explosionDuration);
        // Instantiate the remaining of the explosion in all four directions
        Explode(Vector2.up, position, explosionRadius);
        Explode(Vector2.down, position, explosionRadius);
        Explode(Vector2.right, position, explosionRadius);
        Explode(Vector2.left, position, explosionRadius);
        Destroy(bomb);
        bombRemaining++;
    }

    // Recursive function that handles the explosion until the radius reaches 0 
    public void Explode(Vector2 direction, Vector2 position, int radius)
    {
        // end condition
        if (radius <= 0)
        {
            return;
        }
        position += direction;
        // if the explosion reaches a destructible brick, destroy it;
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearTile(position);
            return;
        }
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        // The explosion have 3 parts : start , middle and end , if the radius is > 1 instantiate the middle part 
        if (radius > 1)
        {

            explosion.SetActivieRenderer(explosion.middle);
        }
        // else if radius = 1 instantiate the end of the explosion
        else
        {
            explosion.SetActivieRenderer(explosion.end);
        }
        explosion.SetRotation(direction);
        Destroy(explosion.gameObject, explosionDuration);
        Explode(direction, position, radius - 1);
    }


    private void ClearTile(Vector2 position)
    {
        Vector3Int cell = destructableTileMap.WorldToCell(position);
        TileBase tile = destructableTileMap.GetTile(cell);
        if (tile != null)
        {
            Instantiate(destructableBrickPrefab, position, Quaternion.identity);
            destructableTileMap.SetTile(cell, null);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // The bomb when it spawns is marked as trigger so it doesn't push the player but once he leaves its radius it becomes not a trigger
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }
    // method for the itemPickup script 
    public void AddBomb()
    {
        bombAmount++;
        bombRemaining++;
    }
}


