using UnityEngine;
using UnityEngine.Events;


public class TerrainDeform : MonoBehaviour {
    private const float FACTOR = 1025/64;
	private Terrain TerrainGameBoard;
    //public Explosion explosion;
    public Object[] heightmaps; //gets image 2
    public UnityEvent onTerrainDeformed = new UnityEvent();

    void Awake()
    {
        TerrainGameBoard = GetComponent<Terrain>();

        //Terrain terrain;
        //DeformMesh(10,10,8,30);
    }

    public void DeformMesh(int yExplosionPos, int xExplosionPos, int radius, int depth)
    {
        //convert from world to image
        yExplosionPos = Mathf.FloorToInt(yExplosionPos*FACTOR);
        xExplosionPos = Mathf.FloorToInt(xExplosionPos*FACTOR);

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
        onTerrainDeformed.Invoke();
    }
}
