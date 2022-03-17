using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    [CreateAssetMenu(fileName = "Brush", menuName = "MapEditor/Brush", order = 1)]
    public class Brush : ScriptableObject
    {
        public Texture2D texutre;
    }
}
