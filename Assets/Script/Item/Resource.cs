using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacy;
    public float spawnTime;

    public DayNight dayNight;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for(int i = 0; i < quantityPerHit; i++)
        {
            if(capacy <= 0)
                break;

            capacy -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        if(capacy <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void SpawnResource()
    {
        //spawnTime += Time.deltaTime;
    }
}
