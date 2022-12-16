using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        SpeedIncrease,
        BombIncrease,
        RadiusIncrease,
    }
    public ItemType itemType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PowerUp(collision.gameObject);
            Destroy(gameObject);
        }
    }
    public void PowerUp(GameObject player)
    {
        if (itemType == ItemType.SpeedIncrease)
        {
            player.GetComponent<Mouvement>().speed++;
        }
        else if (itemType == ItemType.BombIncrease)
        {
            player.GetComponent<BombController>().AddBomb();
        }
        else if (itemType == ItemType.RadiusIncrease)
        {
            player.GetComponent<BombController>().explosionRadius++;
        }

    }


}
