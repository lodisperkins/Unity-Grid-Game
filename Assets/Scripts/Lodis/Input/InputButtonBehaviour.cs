using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class InputButtonBehaviour : MonoBehaviour {
    //list of inputs the player has
    [SerializeField]
    private List<InputVariable> inputs;
    //reference to the last input added
    [SerializeField] private InputVariable newInput;
    public float inputBuffer;
    private bool canMove;
    //used for input buffer
    private float timer;

    // Use this for initialization
    void Start()
    {
        canMove = true;
    }
    //checks the list to see if any of the button are down
    public void CheckButton()
    {
        foreach (var input in inputs)
        {
            if (Input.GetAxisRaw(input.Axis) == 1)
            {
                if(input.CheckTime())
                {
                    SendMessage(input.ButtonDownMessage,input.Arg);
                }
            }
            else if(Input.GetAxisRaw(input.Axis) == -1)
            {
                if (input.CheckTime())
                {
                    SendMessage(input.ButtonNegativeMessage);
                }
            }
            else if(input.ButtonUpMessage != "")
            {
                SendMessage(input.ButtonUpMessage);
            }
        }
    }
    //adds an input to the list
    public void AddInput(string Axis,string message1,string message2,string message3, object Arg)
    {
        newInput = InputVariable.CreateInstance(Axis, message1,message2,message3, Arg,inputBuffer);
        inputs.Add(newInput);
    }
    //clears the entire input list
    public void Clear()
    {
        inputs.Clear();
    }
	// Update is called once per frame
	void Update () {
        CheckButton();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(InputButtonBehaviour))]
public class InputButtonEditor : Editor
{
    string button;
    string message1;
    string message2;
    string message3;
    [SerializeField]
    Object arg;
    
    public override void OnInspectorGUI()
    {
        InputButtonBehaviour myscript = (InputButtonBehaviour)target;
        DrawDefaultInspector();
        button = EditorGUILayout.TextField("Input Button Name",button);
        message1 = EditorGUILayout.TextField("Button Down Func",message1);
        message2 = EditorGUILayout.TextField("Button Up Func", message2);
        message3 = EditorGUILayout.TextField("Button Negative Func", message3);
        arg = EditorGUILayout.ObjectField("Argument",arg, typeof(object), true);
        if(GUILayout.Button("Add Input"))
        {
            myscript.AddInput(button,message1,message2,message3,arg);
        }
        if(GUILayout.Button("Clear"))
        {
            myscript.Clear();
        }
    }
}
#endif
