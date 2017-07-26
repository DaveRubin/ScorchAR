using UnityEngine;


public class TerrainDeform : MonoBehaviour {

	public Terrain TerrainGameBoard;
    //public Explosion explosion;

    void OnGUI()
    {
        if(GUI.Button(new Rect(30,30,360,60),"Deform Terrain"))
        {
            //get the terrain heightmap width adn height
            int xSize = TerrainGameBoard.terrainData.heightmapWidth;
            int ySize = TerrainGameBoard.terrainData.heightmapHeight;

            //Generate a random crater when hitting the button
            int radius = Random.Range(1,3);
            int depth   = Random.Range(0,4);
            int xExplosionPos = Random.Range(3, xSize - 3);
            int yExplosionPos = Random.Range(3, ySize - 3);
            int varyingDepth = 0;



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

            //SetHeights to perform change
            Debug.Log("changing height at " + xExplosionPos + " , " + yExplosionPos);
            TerrainGameBoard.terrainData.SetHeights(0,0,heights);
        }
    }
}
