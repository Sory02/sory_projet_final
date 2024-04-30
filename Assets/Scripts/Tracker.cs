using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] List<Transform> destinations = new List<Transform>();
    int destinationIndex;

    private void Update()
    {
        if (destinationIndex < destinations.Count)
        {
            Vector3 destPos = destinations[destinationIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, destPos, 2 * Time.deltaTime);

            if (Vector3.Distance(transform.position, destPos) < 0.01f)
            {
                destinationIndex++;
            }
        }
    }
}
