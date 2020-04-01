using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarManager : Singleton<AstarManager>
{
    List<BaseNode> open_list = new List<BaseNode>();
    List<BaseNode> close_list = new List<BaseNode>();

    //地图的宽高
    private int width;
    private int height;

    public List<List<BaseNode>> mapLists = new List<List<BaseNode>>();

    public void InitMap(int w, int h)
    {
        for(int i = 0; i < w; i++)
        {
            List<BaseNode> colList = new List<BaseNode>();
            for(int j = 0; j < h; j++)
            {
                BaseNode node = new BaseNode(i, j, Random.Range(0, 100) < 20 ? NodeType.unwalk : NodeType.walk);
                colList.Add(node);
            }
            mapLists.Add(colList);
        }

        width = w;
        height = h;
    }

    public List<BaseNode> FindPath(int start_x, int start_y, int end_x, int end_y)
    {
        open_list.Clear();
        close_list.Clear();


        BaseNode start = mapLists[start_x][start_y];
        BaseNode end = mapLists[end_x][end_y];

        if(start.nodeType == NodeType.unwalk || end.nodeType == NodeType.unwalk)
        {
            Debug.LogError("寻路起始点不可达");
            return null;
        }

        start.parentNode = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        close_list.Add(start); //初始，将起点加入到open_list中

        //查找8个临近的节点

        while(true)
        {
            AddNearNodeToOpenList(start.x, start.y - 1, 1, start, end);
            AddNearNodeToOpenList(start.x, start.y + 1, 1, start, end);
            AddNearNodeToOpenList(start.x - 1, start.y , 1, start, end);
            AddNearNodeToOpenList(start.x + 1, start.y , 1, start, end);

            AddNearNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            AddNearNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            AddNearNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);
            AddNearNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);

            //开启列表为空，说明找不到了
            if(open_list.Count == 0)
            {
                Debug.LogError("not find path");
                return null;
            }
            open_list.Sort(Compare);

            close_list.Add(open_list[0]);
            start = open_list[0]; //开启下一次遍历
            open_list.Remove(open_list[0]);
           

            if(start == end)
            {
                Debug.LogError("find path");
                List<BaseNode> paths = new List<BaseNode>();
                paths.Add(end);
                while(end.parentNode != null)
                {
                    paths.Add(end.parentNode);
                    end = end.parentNode;
                }
                paths.Reverse();
                return paths;
            }

        }
       
    }

    public void AddNearNodeToOpenList(int x, int y, float g, BaseNode parent, BaseNode end)
    {
        if (x < 0 || y < 0 || x >= width || y >= width)
            return;

        BaseNode nearNode = mapLists[x][y];
        if (nearNode.nodeType == NodeType.unwalk || close_list.Contains(nearNode) || open_list.Contains(nearNode))
            return;
        nearNode.parentNode = parent;
        nearNode.g = parent.g + g;
        nearNode.h = Mathf.Abs(nearNode.x - end.x) + Mathf.Abs(nearNode.y - end.y);
        nearNode.f = nearNode.g + nearNode.h;

        open_list.Add(nearNode);

    }

    private int Compare(BaseNode a, BaseNode b)
    {
        return - a.f.CompareTo(b.f);
    }


}
