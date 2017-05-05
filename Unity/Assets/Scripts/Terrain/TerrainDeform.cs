using UnityEngine;


public class TerrainDeform : MonoBehaviour {

	public Terrain TerrainGameBoard;
    //public Explosion explosion;

    void OnGUI()
    {
        if(GUI.Button(new Rect(30,30,200,30),"Deform Terrain"))
        {
            //get the terrain heightmap width adn height
            int xSize = TerrainGameBoard.terrainData.heightmapWidth;
            int ySize = TerrainGameBoard.terrainData.heightmapHeight;

            //Generate a random crater when hitting the button
            int radius = Random.Range(3,30);
            int depth   = Random.Range(3,10);
            int xExplosionPos = Random.Range(30, xSize - 30);
            int yExplosionPos = Random.Range(30, ySize - 30);



            //GetHeights - gets the heightmap points of the terrain
            float[,] heights = TerrainGameBoard.terrainData.GetHeights(0,0,xSize,ySize);
            //Debug.Log(xSize + " , " + ySize);

            //Manipulate the heights - deform the terrain
            for(int y = yExplosionPos - radius; y < yExplosionPos + radius; y++)
            {
                for(int x = xExplosionPos - radius; x < xExplosionPos + radius; x++)
                {
                    Debug.Log("current height at [" + x + "," + y + "] is: " + heights[x,y]);
                    //heights[x,y] = heights[x,y] - (float)depth/ySize;
                    heights[x,y] = 0;  
                    if (heights[x,y] < 0)
                        heights[x,y] = 0;
                    Debug.Log("after decrease height at [" + x + "," + y + "] is: " + heights[x,y]);
                }
            }

            /*
            Debug.Log("current height at [" + xExplosionPos + "," + yExplosionPos + "] is: " + heights[xExplosionPos,yExplosionPos]);
            Debug.Log("decreasing " + (float)depth/ySize + " from " + heights[xExplosionPos,yExplosionPos]);
            heights[xExplosionPos,yExplosionPos] = heights[xExplosionPos,yExplosionPos] -  (float)depth/ySize;
            if (heights[xExplosionPos,yExplosionPos] < 0)
                heights[xExplosionPos,yExplosionPos] = 0;
            Debug.Log("after decrease height at [" + xExplosionPos + "," + yExplosionPos + "] is: " + heights[xExplosionPos,yExplosionPos]);

            heights[xExplosionPos - 1, yExplosionPos -1] = 0;
            heights[xExplosionPos - 1, yExplosionPos] = 0;
            heights[xExplosionPos - 1, yExplosionPos +1] = 0;
            heights[xExplosionPos , yExplosionPos -1] = 0;
            heights[xExplosionPos , yExplosionPos ] = 0;
            heights[xExplosionPos , yExplosionPos +1] = 0;
            heights[xExplosionPos + 1, yExplosionPos -1] = 0;
            heights[xExplosionPos + 1, yExplosionPos ] = 0;
            heights[xExplosionPos + 1, yExplosionPos +1] = 0;
            */


            //SetHeights to perform change
            Debug.Log("changing height at " + xExplosionPos + " , " + yExplosionPos);
            TerrainGameBoard.terrainData.SetHeights(0,0,heights);
        }
    }

}
