using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChunkGen : MonoBehaviour
{
    public Transform playerPos; Vector3 oldPlayerPos = new Vector3(0, 0, 0);
    public int renderDistance = 5;
    public int chunkSize = 16; // chunkSize
    public Vector2 textureAtlasSize = new Vector2(2, 6);
    public Material material;
    public Text updateText;
    [HideInInspector] public int seed;

    int wx; int wz; int wy; int cx; int cz; int cy; int csz;

    IEnumerator Start()
    {
        wx = Mathf.RoundToInt(playerPos.position.x/(chunkSize));
        wz = Mathf.RoundToInt(playerPos.position.z/(chunkSize));
        wy = 1;
        cx = 0; cz = 0; cy = 1; // chunk coords
        csz = chunkSize;
        seed = Random.Range(1111, 9999);

        int ca = 0;

        for (cy = 0; cy < 4; cy++)
        {
            for (cz = 0; cz < renderDistance; cz++)
            {
                for (cx = 0; cx < renderDistance; cx++)
                {
                    ChunkGenerator();
                    ca++;
                    int cf = renderDistance*renderDistance*4;
                    updateText.text = "Generating World ["+seed+"] ("+ca+"/"+cf+")";
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }

    public void ChunkGenerator()
    {
        int x = (cx*csz-csz);
        int y = (cy*csz-csz);
        int z = (cz*csz-csz);
        
        GameObject chunkInst = new GameObject("Chunk");
        MeshGen meshGen = chunkInst.AddComponent<MeshGen>(); 
        meshGen.ChunkGen = this; meshGen.material = material;
        chunkInst.transform.position = new Vector3(-(renderDistance*1.8f), 0, -(renderDistance*1.8f));
        meshGen.MeshGenerator(x, y, z, csz, seed, textureAtlasSize);
    }
}
