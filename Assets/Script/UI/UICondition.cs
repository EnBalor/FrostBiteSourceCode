using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition frozen;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
