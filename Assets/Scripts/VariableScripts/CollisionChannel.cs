using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChannel : ScriptableObject {

	public string name;
	public bool collisionEnabled;
    private void Init(string tag, bool collisionVal)
    {
        name = tag;
        collisionEnabled = collisionVal;
    }
    public static CollisionChannel CreateInstance(string tag, bool collisionVal)
    {
        var data = CreateInstance<CollisionChannel>();
        data.Init(tag,collisionVal);
        return data;
    }
}
