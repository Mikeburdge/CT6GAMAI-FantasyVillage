namespace Pathfinding
{
    public class GraphEdge
    {
        public int To { get; }
        public int From { get; }

        public float Cost { set; get;}

        public GraphEdge(int from, int to, float cost)
        {
            To = to;
            From = from;
            Cost = cost;
        }

        public float GetCost()
        {
            return Cost;
        }

    }
}