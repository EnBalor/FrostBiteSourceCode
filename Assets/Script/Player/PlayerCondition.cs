using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Damagable
{
    void TakeDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, Damagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina;} }
    Condition frozen { get { return uiCondition.frozen; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    public GameObject gameOver;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        frozen.Subtract(frozen.passiveValue * Time.deltaTime);

        if(hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    { 
        hunger.Add(amount); 
    }

    public void Heat(float amount)
    {
        frozen.Add(amount);
    }

    public void TakeDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public void Die()
    {
        CharacterManager.Instance.Player.controller.ToggleCursor();
        gameOver.SetActive(true);
    }
}
