using PathfindingSection;
using UnityEngine;

namespace Villagers
{
    public class Humanoid : MonoBehaviour
    {
        protected Pathfinding Navigation = FindObjectOfType<Pathfinding>();

        public float MoveSpeed;

    }
}