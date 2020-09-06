using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodis.GamePlay.GridScripts;
using Lodis.GamePlay.OtherScripts;
using UnityEditor;
using Lodis.Movement;
using UnityEngine.Experimental;

namespace Lodis.Movement
{
    [RequireComponent(typeof(SeekBehaviour),typeof(Rigidbody))]
    public class GridPhysicsBehaviour : MonoBehaviour
    {
        
        private PanelBehaviour trailingPanel;
        public PanelBehaviour currentPanel;
        private SeekBehaviour seekScript;
        private Rigidbody rigidbody;
        private Vector3 heightOffset;
        private Vector2 currentVelocity;
        private PanelBehaviour targetPanel;
        private ScreenShakeBehaviour shakeScript;
        [SerializeField]
        private bool isMoving;
        [SerializeField]
        private Lodis.Event onHit;
        [SerializeField]
        private bool stopsWhenHit = true;
        public List<CollisionChannel> collisions;
        [SerializeField]
        private bool damagesOnHit = true;

        public bool IsMoving
        {
            get
            {
                return isMoving;
            }
        }

        public bool StopsWhenHit
        {
            get
            {
                return stopsWhenHit;
            }

            set
            {
                stopsWhenHit = value;
            }
        }

        // Use this for initialization
        void Start()
        {
            
        }
        private void Awake()
        {
            seekScript = GetComponent<SeekBehaviour>();
            seekScript.SeekEnabled = false;
            rigidbody = GetComponent<Rigidbody>();
            currentVelocity = new Vector2(0, 0);
            heightOffset = new Vector3(0, transform.position.y, 0);
            shakeScript = GetComponent<ScreenShakeBehaviour>();
        }
        public void AddForce(Vector2 force,float distance = 0.3f)
        {
            currentVelocity = force;
            Vector2 destination = currentPanel.Position + force;
            destination.Set(Mathf.Clamp(destination.x,0,9),Mathf.Clamp(destination.y,0,3));
            targetPanel = BlackBoard.grid.GetPanelFromGlobalList(destination);
            //rigidbody is null for blackhole
            seekScript.Init(targetPanel.transform.position + heightOffset, rigidbody.velocity, (int)currentVelocity.magnitude, distance, true, false,true);
            seekScript.SeekEnabled = true;
        
        }
        public void AddForce(float speed, PanelBehaviour destination, float distance = 0.3f)
        {
            targetPanel = destination;
            currentVelocity = (targetPanel.Position - currentPanel.Position).normalized * speed;
            //rigidbody is null for blackhole
            seekScript.Init(targetPanel.transform.position + heightOffset, rigidbody.velocity, (int)currentVelocity.magnitude, distance, true, false, true);
            seekScript.SeekEnabled = true;

        }
        public static Vector2 ConvertToGridVector(Vector3 worldVector)
        {
            return new Vector2(worldVector.z, -worldVector.x);
        }
        public bool CheckCollisions(string tag)
        {
            foreach(var channel in collisions)
            {
                if(channel)
                {
                    if (channel.name == tag && channel.collisionEnabled)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void OnTriggerEnter(Collider other)
        {
          
            if(seekScript.SeekEnabled && currentPanel)
            {
                if(other.CompareTag("Panel") && other != currentPanel.gameObject)
                {
                    trailingPanel = currentPanel;
                    currentPanel = other.GetComponent<PanelBehaviour>();
                }
                if (CheckCollisions(other.tag) == false)
                {
                    return;
                }
                ResolveCollsion(other);
            }
        }
        public void ResolveCollsion(Collider other)
        {
            onHit.Raise(gameObject);
            if (stopsWhenHit)
            {
                targetPanel = trailingPanel;
                seekScript.Init(targetPanel.transform.position + heightOffset, rigidbody.velocity, (int)currentVelocity.magnitude, 0.1f, true, false);
                seekScript.SeekEnabled = true;
                currentPanel = trailingPanel;
            }
            if(damagesOnHit)
            {
                HealthBehaviour healthScript = other.GetComponent<HealthBehaviour>();
                if (healthScript)
                {
                    healthScript.takeDamage((int)currentVelocity.magnitude);
                } 
            }
        }
        public void CleanList()
        {
            if(collisions != null)
            {
                if(collisions[0] == null)
                {
                    collisions.Clear();
                    return;
                }
            }
            else
            {
                collisions = new List<CollisionChannel>();
            }
        }
        public List<bool> GetCollisionValues()
        {
            List<bool> collisionValues = new List<bool>();
            if(collisions == null)
            {
                collisions = new List<CollisionChannel>();
            }
            foreach(var channel in collisions)
            {
                if(channel)
                    collisionValues.Add(channel.collisionEnabled);
            }
            return collisionValues;
        }
        public void UpdateCollisionChannels(List<bool> newValues)
        {
            collisions.Clear();
            for(int i =0; i< UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
            {
                collisions.Add(CollisionChannel.CreateInstance(UnityEditorInternal.InternalEditorUtility.tags[i], newValues[i]));
            }
        }
        // Update is called once per frame
        void Update()
        {
            isMoving = seekScript.SeekEnabled;
            if(shakeScript)
            {
                shakeScript.isMoving = isMoving;
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(GridPhysicsBehaviour))]
public class GridPhysicsEditor : Editor
{

    List<bool> collisions = new List<bool>(10);
    public override void OnInspectorGUI()
    {
        GridPhysicsBehaviour myscript = (GridPhysicsBehaviour)target;
        collisions = myscript.GetCollisionValues();
        DrawDefaultInspector();
        for (int i =0; i< UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if(i >= collisions.Count)
            {
                collisions.Add(new bool());
            }
            collisions[i] = EditorGUILayout.Toggle(UnityEditorInternal.InternalEditorUtility.tags[i], collisions[i]);
        }
        
        myscript.UpdateCollisionChannels(collisions);
        if(GUILayout.Button("Clean List"))
        {
            myscript.CleanList();
        }
    }

}
#endif

