using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Value 
{
    public PropertyType type;
    public int currentIntValue;
    public int intValue;
    public bool boolValue;
    public EntityData entityData;
}

public enum PropertyType
{
    INT,
    BOOL
}


#if UNITY_EDITOR
[CustomPropertyDrawer (typeof(Value))]
public class TrickPropertyDrawer : PropertyDrawer
{    
    private int rows;
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        using(new EditorGUI.PropertyScope(position, label, property))
        {
            SerializedProperty type = property.FindPropertyRelative ("type");
            SerializedProperty intValue = property.FindPropertyRelative ("intValue");
            SerializedProperty boolValue = property.FindPropertyRelative ("boolValue");
            SerializedProperty entityDataValue = property.FindPropertyRelative ("entityData");
            
            EditorGUI.LabelField (new Rect(position.x - 5, position.y - 40, position.width, position.height), label.text);

            GUIContent guiType = new GUIContent ("Type");

            EditorGUI.PropertyField(new Rect(position.x, position.y + 15, position.width - 60 ,position.height - 65), type);
            rows = 5;

            var typeValue = Enum.GetValues(typeof(PropertyType)).GetValue(type.enumValueIndex);
            switch (typeValue)
            {
                case PropertyType.INT:
                    GUIContent guiIntValue = new GUIContent ("intValue");
                    EditorGUI.PropertyField(new Rect(position.x, position.y + 35, position.width - 60 ,position.height - 75), intValue);
                    GUIContent guientityDataValue = new GUIContent ("entityDataValue");
                    EditorGUI.PropertyField(new Rect(position.x, position.y + 55, position.width - 60 ,position.height - 75), entityDataValue);
                    break;
                case PropertyType.BOOL:
                    GUIContent guiBoolValue = new GUIContent ("boolValue");
                    EditorGUI.PropertyField(new Rect(position.x, position.y + 35, position.width - 60 ,position.height - 35), boolValue);
                    break;
            }
        }
    }

    public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) 
    {
        return base.GetPropertyHeight (prop, label) * rows;
    }
}
#endif 

