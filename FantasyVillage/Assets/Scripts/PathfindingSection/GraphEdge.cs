namespace PathfindingSection
{
    public class GraphEdge
    {
        public int To { get; }
        public int From { get; }
        public int Triangle { get;}

        public float Cost { set; get;}

        public GraphEdge(int from, int to, float cost, int triangle)
        {
            To = to;
            From = from;
            Cost = cost;
            Triangle = triangle;
        }


        public float GetCost()
        {
            return Cost;
        }

    }
}