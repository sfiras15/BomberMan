using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickDestruction : MonoBehaviour
{
    public float brickDestructionTime = 1f;
    [Range(0f, 1f)]
    public float itemSpawnChance = 0.1f;
    public GameObject[] items;
    private void Start()
    {
        Destroy(gameObject,brickDestructionTime);
    }

    private void OnDestroy()
    {
       if (items.Length > 0 && Random.Range(0f,1f) > itemSpawnChance)
        {
            int randomIndex = Random.Range(0, 3);
            Instantiate(items[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
