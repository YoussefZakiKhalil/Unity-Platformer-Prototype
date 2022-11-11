using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsateColor : MonoBehaviour
{
    public Color color1;
    public Color color2;
    public float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Shader shader = GetComponent<MeshRenderer>().material.shader;

        for (int i = 0; i < shader.GetPropertyCount(); i++)
        {
            Debug.Log("-> propertiy: " + shader.GetPropertyName(i));

        }
        GetComponent<MeshRenderer>().material.SetColor("Color_51aef833fcc64b86ab0c17ffa639d16f", color1);


    }

    // Update is called once per frame
    void Update()
    {
        t += 0.5f * Time.deltaTime;
        if (t > 1.0f)
        {
            Color temp = color1;
            color1 = color2;
            color2 = temp;
            t = 0.0f;
        }
        GetComponent<MeshRenderer>().material.SetColor("Color_51aef833fcc64b86ab0c17ffa639d16f", Color.Lerp(color1, color2, t));

    }
}
