using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalType
{
    Herbivor,
    Carnivor
}

public enum AnimalStat
{
    Health,
    Speed,
    Attack,
}

[CreateAssetMenu(fileName = "Animal", menuName = "New Animal")]
public class Animal : ScriptableObject
{
    [Header("Info")]
    public string animalName;
    public string animalDescription;
    public AnimalType animalType;
    public float animalHealth;
    public float animalSpeed;
    public float animalAttack;
}
