using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;
    public float heatAmount;
    public float heatRadius;

    List<Damagable> things = new List<Damagable>(); 
    List<PlayerCondition> players = new List<PlayerCondition>();
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }

     void Update()
    {
        HeatNearbyPlayers();
    }
    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakeDamage(damage);
        }
    }

    void HeatNearbyPlayers()
    {
        PlayerCondition[] allPlayers = FindObjectsOfType<PlayerCondition>();
        foreach (PlayerCondition player in allPlayers)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= heatRadius)
            {
                player.Heat(heatAmount * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Damagable damagable))
        {
            things.Add(damagable);
        }
        if(other.TryGetComponent(out PlayerCondition player))
        {
            players.Add(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Damagable damagable))
        {
            things.Remove(damagable);
        }
        if(other.TryGetComponent(out PlayerCondition player))
        {
            players.Remove(player);
        }
    }
}
