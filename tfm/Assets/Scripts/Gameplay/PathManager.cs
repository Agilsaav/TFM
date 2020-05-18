using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to create the path using PathElements. It spawn a new path element each time we colide with the previous one.
namespace Gameplay
{
    public class PathManager : MonoBehaviour
    {
        [SerializeField] PathElement prefab;
        [SerializeField] Vector3[] positions;
        [SerializeField] Vector3[] rotations;

        int index;

        void Start()
        {
            index = 0;
            if (positions.Length != rotations.Length) Debug.LogWarning("The Path positions and rotations are not correct!");
            InstantiateNextElement();
        }

        public void InstantiateNextElement()
        {
            if(index < positions.Length)
            {
                PathElement newPathElement = Instantiate(prefab, positions[index], Quaternion.Euler(rotations[index]));
                newPathElement.transform.parent = gameObject.transform;
                index++;
            }

        }
    }
}

