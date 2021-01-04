using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LocationThings
{
    public class LocationPositions : MonoBehaviour
    {

        [FormerlySerializedAs("LocationGameobjects")] public GameObject[] locationGameobjects;

        public static Dictionary<LocationNames, Vector3> LocationAndPosition = new Dictionary<LocationNames, Vector3>();

        private void Awake()
        {
            for (var i = 0; i < locationGameobjects.Length; i++)
            {
                var currentGameObject = locationGameobjects[i];

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

            var hasSucceeded = LocationAndPosition.TryGetValue(locationNames, out rv);

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
        Home = 0,
        Forest = 1,
        RockMine = 2,
        Farm = 3,
        TrainingArea = 4,
        FoodStorage = 5
    }



}