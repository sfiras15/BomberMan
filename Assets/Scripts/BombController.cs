using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode spawnBombInput = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombRemaining;
    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 2;
    [Header("Destruction")]
    public BrickDestruction destructableBrickPrefab;
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
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        GameObject bomb = Instantiate(bombPrefab,position,Quaternion.identity);
        bombRemaining--;
        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActivieRenderer(explosion.start);
        Destroy(explosion.gameObject, explosionDuration);
        Explode(Vector2.up, position, explosionRadius);
        Explode(Vector2.down, position, explosionRadius);
        Explode(Vector2.right, position, explosionRadius);
        Explode(Vector2.left, position, explosionRadius);
        Destroy(bomb);
        bombRemaining++;
    }


    public void Explode(Vector2 direction, Vector2 position, int radius)
    {
        if (radius <= 0)
        {
            return;
        }
        position += direction;
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearTile(position);
            return;
        }
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        if (radius > 1)
        {
            explosion.SetActivieRenderer(explosion.middle);
        }
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }
    public void AddBomb()
    {
        bombAmount++;
        bombRemaining++;
    }
}


