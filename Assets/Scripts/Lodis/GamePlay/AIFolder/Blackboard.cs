using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodis;
using Lodis.GamePlay;
using Lodis.GamePlay.GridScripts;
public static class BlackBoard{
    //Updated in blockbehaviour in InitializeBlock func and OnDestroy func
    public static List<BlockBehaviour> p1Blocks = new List<BlockBehaviour>();
    public static List<BlockBehaviour> p2Blocks = new List<BlockBehaviour>();
    //Updated in playermovementbehaviour in update func
    public static Vector2 p1Position = new Vector2();
    public static Vector2 p2Position = new Vector2();
    //Set in playermovementbehaviour in start func
    public static GameObject Player1;
    public static GameObject Player2;
    //Updates in gridbehaviour in update func
    public static PanelList p1PanelList;
    public static PanelList p2PanelList;
    //Set in corebehaviour in start func
    public static GameObject p1Core;
    public static GameObject p2Core;
}
