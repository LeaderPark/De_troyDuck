using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionField : SearchOption
{

    public override List<GameObject> SearchGameObject(List<GameObject> gameObjects, bool first)
    {
        if (obj != null)
        {
            if (first)
            {
                UnityEngine.Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject), true);
                foreach (GameObject item in objects)
                {
                    bool contains = false;
                    AudioListener[] components = item.GetComponents<AudioListener>();
                    foreach (AudioListener component in components)
                    {
                        if (component.GetType().Name == (obj as string).Split('.')[0])
                        {
                            contains = true;
                        }
                    }
                    if (contains)
                    {
                        gameObjects.Add(item as GameObject);
                    }
                }
            }
            else
            {
                List<GameObject> removeItems = new List<GameObject>();
                foreach (GameObject item in gameObjects)
                {
                    bool contains = false;
                    AudioListener[] components = item.GetComponents<AudioListener>();
                    foreach (AudioListener component in components)
                    {
                        if (component.GetType().Name == (obj as string).Split('.')[0])
                        {
                            contains = true;
                        }
                    }
                    if (!contains)
                    {
                        removeItems.Add(item as GameObject);
                    }
                }
                foreach (GameObject item in removeItems)
                {
                    gameObjects.Remove(item);
                }
            }
        }

        return gameObjects;
    }
}
