using UnityEngine;


public class TerrainDeform : MonoBehaviour {

	private Terrain TerrainGameBoard;
    //public Explosion explosion;

    void Awake()
    {
        TerrainGameBoard = GetComponent<Terrain>();
        //DeformMesh(10,10,8,30);
    }

    public void DeformMesh(int xExplosionPos, int yExplosionPos, int radius, int depth)
    {
        float distance, d;
        //get the terrain heightmap width and height
        int xSize = TerrainGameBoard.terrainData.heightmapWidth;
        int ySize = TerrainGameBoard.terrainData.heightmapHeight;

        //GetHeights - gets the heightmap points of the terrain
        float[,] heights = TerrainGameBoard.terrainData.GetHeights(0,0,xSize,ySize);

        //Manipulate the heights - deform the terrain
        for(int y = yExplosionPos - radius; y < yExplosionPos + radius; y++)
        {
            for(int x = xExplosionPos - radius; x < xExplosionPos + radius; x++)
            {
                // calculate distance from center of explosion
                distance = Mathf.Sqrt((Mathf.Pow(x - xExplosionPos,2)) + (Mathf.Pow(y - yExplosionPos,2)));

                // normalize distance to be in range
                d = distance/radius;
                if (d < 0)
                    d = 0;
                else if (d > 1)
                    d = 1.0f;
                if(x >= 0 && x < xSize && y >= 0 && y < ySize)
                {
                    heights[x,y] = heights[x,y] - (1 - Mathf.Pow(d,3)) * heights[x,y]/4 * depth;

                    // flatten at "ground"
                    if (heights[x,y] < 0)
                        heights[x,y] = 0;
                }
            }
        }

        //SetHeights to perform change
        TerrainGameBoard.terrainData.SetHeights(0,0,heights);

    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(30,30,600,120),"Deform Terrain"))
        {

            //get the terrain heightmap width adn height
            //Generate a random crater when hitting the button
            int xSize = TerrainGameBoard.terrainData.heightmapWidth;
            int ySize = TerrainGameBoard.terrainData.heightmapHeight;

            //Generate a random crater when hitting the button
            int radius = Random.Range(1,3);
            int depth  = Random.Range(0,4);

            int xExplosionPos = Random.Range(3, xSize - 3);
            int yExplosionPos = Random.Range(3, ySize - 3);



            /*
            //GetHeights - gets the heightmap points of the terrain
            float[,] heights = TerrainGameBoard.terrainData.GetHeights(0,0,xSize,ySize);
            //Debug.Log(xSize + " , " + ySize);

            //Manipulate the heights - deform the terrain
            for(int y = yExplosionPos - radius; y < yExplosionPos + radius; y++)
                {
                            varyingDepth = 0;
                            for(int x = xExplosionPos - radius; x < xExplosionPos + radius; x++)
                                {
                                    //Debug.Log("current height at [" + x + "," + y + "] is: " + heights[x,y]);
                                    heights[x,y] = heights[x,y] - ((float)depth + varyingDepth)/ySize;
                                    if (x < xExplosionPos)
                                            varyingDepth++;
                                    else if (x > xExplosionPos)
                                            varyingDepth--;
                                    //heights[x,y] = 0;
                                    if (heights[x,y] < 0)
                                            heights[x,y] = 0;
                                    //Debug.Log("after decrease height at [" + x + "," + y + "] is: " + heights[x,y]);
                                }
                        }
            */
            //SetHeights to perform change
            Debug.Log("changing height at " + xExplosionPos + " , " + yExplosionPos);
            //TerrainGameBoard.terrainData.SetHeights(0,0,heights);
            // call upon deforming function with the random parameters
            DeformMesh(xExplosionPos, yExplosionPos, 20, 2000);
        }

    }
}
