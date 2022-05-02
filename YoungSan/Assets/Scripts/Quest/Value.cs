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
            
            EditorGUI.LabelField (new Rect(position.x - 5, position.y - 20, position.width, position.height), label.text);

            GUIContent guiType = new GUIContent ("Type");

            EditorGUI.PropertyField(new Rect(position.x, position.y + 15, position.width - 60 ,position.height - 35), type);
            rows = 3;

            var typeValue = Enum.GetValues(typeof(PropertyType)).GetValue(type.enumValueIndex);
            switch (typeValue)
            {
                case PropertyType.INT:
                    GUIContent guiIntValue = new GUIContent ("intValue");
                    EditorGUI.PropertyField(new Rect(position.x, position.y + 35, position.width - 60 ,position.height - 35), intValue);
                    break;
                case PropertyType.BOOL:
                    GUIContent guiBoolValue = new GUIContent ("boolValue");
                    EditorGUI.PropertyField(new Rect(position.x, position.y + 35, position.width - 60 ,position.height - 35), boolValue);
                    break;
            }

            // if (type.enumValueIndex != 2)
            // {        
            //     rows = 4;
            //     EditorGUI.PropertyField (new Rect (position), startClip, guiClip);
            //     if (startClip.objectReferenceValue != null)
            //     {
            //         AnimationClip clip = (AnimationClip)startClip.objectReferenceValue;
            //         startLength.floatValue = clip.length;
            //     }
            // }
            // else
            // {        
            //     rows = 7;
            //     guiClip = new GUIContent ("Start Clip");

            //     EditorGUI.PropertyField (
            //         new Rect (pos.x, pos.y + 40, pos.width, pos.height),
            //         startClip, guiClip);
            //     if (startClip.objectReferenceValue != null)
            //     {
            //         AnimationClip clip = (AnimationClip)startClip.objectReferenceValue;
            //         startLength.floatValue = clip.length;
            //     }

            //     guiClip = new GUIContent ("Hold Clip");

            //     EditorGUI.PropertyField (
            //         new Rect (pos.x, pos.y + 60, pos.width, pos.height),
            //         holdClip, guiClip);
            //     if (holdClip.objectReferenceValue != null)
            //     {
            //         AnimationClip clip = (AnimationClip)holdClip.objectReferenceValue;
            //         holdLength.floatValue = clip.length;
            //     }

            //     guiClip = new GUIContent ("End Clip");

            //     EditorGUI.PropertyField (
            //         new Rect (pos.x, pos.y + 80, pos.width, pos.height),
            //         endClip, guiClip);
            //     if (endClip.objectReferenceValue != null)
            //     {
            //         AnimationClip clip = (AnimationClip)endClip.objectReferenceValue;
            //         endLength.floatValue = clip.length;
            //     }
            //}
        }
    }

    public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) 
    {
        return base.GetPropertyHeight (prop, label) * rows;
    }
}
#endif 

