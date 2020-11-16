using System.Collections.Generic;
using UnityEngine;

namespace LocationThings
{
    public class LocationPositions : MonoBehaviour
    {

        public GameObject[] LocationGameobjects;

        public static Dictionary<LocationNames, Vector3> LocationAndPosition = new Dictionary<LocationNames, Vector3>();

        private void Awake()
        {
            for (int i = 0; i < LocationGameobjects.Length; i++)
            {
                GameObject currentGameObject = LocationGameobjects[i];

                if (!currentGameObject)
                {
                    Debug.LogWarning("LocationGameobject: " + i + " is invalid");
                    continue;
                }

                LocationAndPosition.Add((LocationNames)i, currentGameObject.transform.position);
            }
        }
        public static Vector3 GetPositionFromLocation(LocationNames locationNames)
        {
            Vector3 rv;

            bool hasSucceeded = LocationAndPosition.TryGetValue(locationNames, out rv);

            if (!hasSucceeded)
            {
                Debug.Log("Failed to get location from a position");
                return Vector3.zero;
            }

            return rv;
        }
    }



    public enum LocationNames
    {
        home = 0,
        forest = 1,
        rockMine = 2,
        farm = 3,
        trainingArea = 4,
        foodStorage = 5
    }



}