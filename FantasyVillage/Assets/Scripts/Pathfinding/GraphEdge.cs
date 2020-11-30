namespace Pathfinding
{
    public class GraphEdge
    {
        public int To { get; }
        public int From { get; }

        public float Cost { set; get; } = 1.0f;

        public GraphEdge(int from, int to)
        {
            To = to;
            From = from;
        }

        public float GetCost()
        {
            return Cost;
        }

    }

    public class QuickGraphEdge : GraphEdge
    {
        public QuickGraphEdge(int @from, int to) : base(@from, to)
        {
            Cost = 0.1f;
        }



    }

}