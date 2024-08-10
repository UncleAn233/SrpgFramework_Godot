using Godot;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.CellGrid
{
    public class AStar
    {
        public static List<Cell> GetPath(Cell start, Cell end, Unit unit)
        {
            Dictionary<Cell, AStarNode> nodeDict = new Dictionary<Cell, AStarNode>() { { start, new AStarNode(0, start.GetDistance(end), null) } };   //记录每个格子的寻路信息
            var open = new HashSet<Cell>() { start };  //候选列表
            var close = new HashSet<Cell>();    //已处理列表

            while (open.Any())
            {
                var current = open.First();
                foreach (var node in open)
                {
                    if (nodeDict[node].F < nodeDict[current].F || nodeDict[node].F == nodeDict[current].F && nodeDict[node].H < nodeDict[current].H)
                        current = node;     //找到最佳节点
                }

                if (current == end) //如果该节点为目标点 循环结束 返回Path
                {
                    var path = new List<Cell>();
                    var currentPathTile = current;
                    while (currentPathTile != start)    //路径不包含出发点
                    {
                        path.Insert(0, currentPathTile);
                        currentPathTile = nodeDict[currentPathTile].Connection;
                    }
                    return path;
                }

                close.Add(current);     //将该节点标记为已处理
                open.Remove(current);   //并移出待处理列表

                foreach (var neighbor in current.Neighbors)     //遍历邻居节点
                {
                    if (close.Contains(neighbor) || !unit.IsCellTraversable(neighbor))   //跳过已处理或无法通行的节点
                        continue;

                    var costToNeighbor = nodeDict[current].G + neighbor.MoveCost;   //计算G = 当前节点G + MoveCost

                    if (!open.Contains(neighbor))    //如果节点没处理过则加入候选列表 并创建一个node
                    {
                        open.Add(neighbor);
                        nodeDict.Add(neighbor, new AStarNode(costToNeighbor, neighbor.GetDistance(end), current));
                    }
                    else if (costToNeighbor < nodeDict[neighbor].G) //如果当前节点在候选列表中&&从当前节点到该节点的G值更小 更新节点
                    {
                        nodeDict[neighbor].SetG(costToNeighbor);
                        nodeDict[neighbor].SetConnection(current);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 移动范围，洪水算法
        /// </summary>
        public static HashSet<Cell> GetMoveableArea(Cell start, Unit unit)
        {
            HashSet<Cell> open = new(); //待处理
            HashSet<Cell> close = new();    //已处理
            HashSet<Cell> nextOpen = new(); //下一轮的待处理
            HashSet<Cell> result = new();   //返回结果

            open.Add(start);
            close.Add(start);

            var step = unit.Mov;
            
            for (var i = 1; i <= step; i++)
            {
                foreach (var cell in open)
                {
                    foreach (var neighbor in cell.Neighbors)
                    {
                        if (close.Contains(neighbor))
                            continue;               //已处理的Cell就跳过
                        else
                            close.Add(neighbor);    //将Cell标记为已处理

                        if (i < step && unit.IsCellTraversable(neighbor))
                        {
                            result.Add(neighbor);
                            nextOpen.Add(neighbor);
                        }
                        //最后一轮需要考虑格子是否被占据
                        else if (i == step && unit.IsCellMovableTo(neighbor))
                        {
                            result.Add(neighbor);
                        }
                    }
                }

                open.Clear();
                open.UnionWith(nextOpen);
                nextOpen.Clear();
            }

            return result;
        }
    }

    internal struct AStarNode
    {
        /// <summary>
        /// 从起点到该点的实际距离
        /// </summary>
        public int G { get; set; }
        /// <summary>
        /// 从该点到终点的估算距离（乐观）
        /// </summary>
        public int H { get; set; }
        /// <summary>
        /// 总估值
        /// </summary>
        public int F => G + H;
        /// <summary>
        /// 上一个节点 标志是从哪个节点到达该节点的
        /// </summary>
        public Cell Connection { set; get; }

        public AStarNode(int g, int h, Cell connection)
        {
            G = g;
            H = h;
            Connection = connection;
        }

        public void SetG(int g)
        {
            G = g;
        }
        public void SetH(int h)
        {
            H = h;
        }
        public void SetConnection(Cell connection)
        {
            this.Connection = connection;
        }
    }
}