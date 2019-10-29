using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectBehaviour
{
    public static List<GameObject> GetAllChildren(GameObject parent)
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            childrenList.Add(child.gameObject);
        }

        return childrenList;
    }

    public static List<GameObject> GetAllChildrenWithTags(GameObject parent, params string[] tags)
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (tags.Contains(child.tag))
            {
                childrenList.Add(child.gameObject);
            }
        }
        
        return childrenList;
    }
}
