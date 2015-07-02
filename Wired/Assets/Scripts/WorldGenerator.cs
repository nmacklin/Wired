using UnityEngine;
using System.Collections;

public class WorldGenerator : MonoBehaviour
{
    // Generates 50x50 flat region of foundation blocks as children of Static World empty GameObject. 
    // Plan to eventually expand to write terrain / buildings.

    public GameObject foundationBlock;

    void Start()
    {
        for (int x = -50; x <= 50; x++)
        {
            for (int z = -50; z <= 50; z++)
            {
                GameObject newBlock = Instantiate(foundationBlock, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
                newBlock.transform.parent = this.gameObject.transform;
            }
        }
    }
}
