using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberController : MonoBehaviour
{
    [SerializeField] NumberColorData colorData;
    TextMeshPro text;
    SpriteRenderer spriteRenderer;


    public int Number
    {
        get => _number;
        set
        {
            int valuePrint = (int)Mathf.Pow(2, value);
            text.SetText(valuePrint.ToString());

            spriteRenderer.color = colorData.numberColors[value - 1];

            _number = value;
        }
    }
    int _number = 1;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
    }
}



