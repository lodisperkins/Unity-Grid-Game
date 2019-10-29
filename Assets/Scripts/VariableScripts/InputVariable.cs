using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Variables/InputVariable")]
public class InputVariable : ScriptableObject
{
    [SerializeField]
    private object arg;
    [SerializeField]
    private string buttonDownMessage;
    [SerializeField]
    private string buttonUpMessage;
    [SerializeField]
    private string buttonNegativeMessage;
    [SerializeField]
    private string axis;
    [SerializeField]
    private float inputBuffer;
    private float timer;
    public string Axis
    {
        get
        {
            return axis;
        }
    }
    public float InputBuffer
    {
        get
        {
            return inputBuffer;
        }
    }

    public string ButtonDownMessage
    {
        get
        {
            return buttonDownMessage;
        }
    }
    public string ButtonUpMessage
    {
        get
        {
            return buttonUpMessage;
        }
    }
    public string ButtonNegativeMessage
    {
        get
        {
            return buttonNegativeMessage;
        }
    }


    public bool CheckTime()
    {
        if (Time.time < timer)
        {
            return false;
        }
        else
        {
            timer = Time.time + InputBuffer;
            return true;
        }
    }
    public object Arg
    {
        get
        {
            return arg;
        }

    }
    private void Init(string axisName, string funcMessage1, string funcMessage2, string funcMessage3,object funcArg,float timer)
    {
        axis = axisName;
        buttonDownMessage = funcMessage1;
        buttonUpMessage = funcMessage2;
        buttonNegativeMessage = funcMessage3;
        arg = funcArg;
        inputBuffer = timer;
    }
    public static InputVariable CreateInstance(string axisName, string funcMessage1,string funcMessage2, string funcMessage3, object funcArg, float timer)
    {
        var data = CreateInstance<InputVariable>();
        data.Init(axisName, funcMessage1,funcMessage2,funcMessage3, funcArg,timer);
        return data;
    }
}
