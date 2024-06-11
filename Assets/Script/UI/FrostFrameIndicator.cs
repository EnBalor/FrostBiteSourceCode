using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FrostFrameIndicator : MonoBehaviour
{
    public Image image;
    public float curVal;
    public float maxVal;

    private void Start()
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
    }

    private void Update()
    {
        maxVal = CharacterManager.Instance.Player.condition.uiCondition.frozen.maxValue;
        curVal = CharacterManager.Instance.Player.condition.uiCondition.frozen.curValue;
        Transparency();
    }

    public void Transparency()
    {
        float alpha = Mathf.Clamp01(1 - (curVal / maxVal));
        SetTransparency(alpha);
    }

    public void SetTransparency(float alpha)
    {
        if(image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
