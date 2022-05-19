using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Product", order = 1)]
public class Product : ScriptableObject
{
    [Space(40)]
    public string productName;
    [Space(10)]
    public int productCount;
    [Space(10)]
    public Sprite productSprite;
    
}
