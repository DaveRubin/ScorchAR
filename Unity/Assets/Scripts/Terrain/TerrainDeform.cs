using UnityEngine;


public class TerrainDeform : MonoBehaviour {

	private Terrain TerrainGameBoard;
    //public Explosion explosion;

    void Awake()
    {
        TerrainGameBoard = GetComponent<Terrain>();
    }

    void DeformMesh(int xExplosionPos, int yExplosionPos, int radius, int depth)
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

                heights[x,y] = heights[x,y] - (1 - Mathf.Pow(d,3)) * heights[x,y]/4 * depth;

                // flatten at "ground"
                if (heights[x,y] < 0)
                    heights[x,y] = 0;
            }
        }

        //SetHeights to perform change
        TerrainGameBoard.terrainData.SetHeights(0,0,heights);

    }
    /*
    void OnGUI()
    {
        if(GUI.Button(new Rect(30,30,360,60),"Deform Terrain"))
        {
            //Generate a random crater when hitting the button
            int xSize = TerrainGameBoard.terrainData.heightmapWidth;
            int ySize = TerrainGameBoard.terrainData.heightmapHeight;
            int radius = Random.Range(12, 48);
            int depth = Random.Range(2, 5);
            int xExplosionPos = Random.Range(3, xSize - 3);
            int yExplosionPos = Random.Range(3, ySize - 3);

            // call upon deforming function with the random parameters
            DeformMesh(xExplosionPos, yExplosionPos, radius, depth);
        }

    }
    */
}
