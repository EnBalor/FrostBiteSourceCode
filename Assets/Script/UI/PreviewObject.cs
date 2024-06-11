using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();
    public Building.Type needType;

    [SerializeField]
    private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    private bool needTypeFlag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if(needType == Building.Type.Normal)
        {
            if(colliderList.Count > 0)
            {
                SetColor(red);
            }
            else
            {
                SetColor(green);
            }
        }
        else
        {
            if(colliderList.Count > 0 || !needTypeFlag)
            {
                SetColor(red);
            }
            else
            {
                SetColor(green);
            }
        }
    }

    private void SetColor(Material mat)
    {
        foreach(Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer >().materials.Length];

            for(int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Structure")
        {
            if (other.GetComponent<Building>().type == needType)
            {
                needTypeFlag = true;
            }
            else
            {
                colliderList.Add(other);
            }
        }
        else
        {
            if(other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            {
                colliderList.Add(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Structure")
        {
            if (other.GetComponent<Building>().type == needType)
            {
                needTypeFlag = false;
            }
            else
            {
                colliderList.Add(other);
            }
        }
        else
        {
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            {
                colliderList.Remove(other);
            }
        }
    }

    public bool isBuildable()
    {
        if(needType == Building.Type.Normal)
        {
            return colliderList.Count == 0;
        }
        else
        {
            return colliderList.Count == 0 && needTypeFlag;
        }
    }
}
