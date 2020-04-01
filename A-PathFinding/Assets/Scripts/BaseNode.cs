using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    walk  = 1,
    unwalk  = 2
}

public class BaseNode
{
    public float f; //总的消耗
    public float g;
    public float h;

    public BaseNode parentNode;

    public int x;
    public int y;

    //方格的状态
    public NodeType nodeType;
    
    public BaseNode(int x, int y, NodeType type)
    {
        this.x = x;
        this.y = y;
        this.nodeType = type;
    }

}
