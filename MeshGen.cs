using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    public ChunkGen ChunkGen;
    public Material material;
    bool setup;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    int ox; int oy; int oz;
    int seed;
    
    void Update()
    {
        /*
        int x = (ox);
        int y = (oy);
        int z = (oz);

        if (ChunkGen.playerPos.position.x - (x) > ChunkGen.chunkSize*ChunkGen.renderDistance /2f)
        {
            MeshGenerator(x+(ChunkGen.renderDistance*ChunkGen.chunkSize), y, z, ChunkGen.chunkSize, ChunkGen.textureAtlasSize);
        }
        else
        if ((x) - ChunkGen.playerPos.position.x > ChunkGen.chunkSize*ChunkGen.renderDistance /2f)
        {
            MeshGenerator(x-(ChunkGen.renderDistance*ChunkGen.chunkSize), y, z, ChunkGen.chunkSize, ChunkGen.textureAtlasSize);
        }
        else 
        if (ChunkGen.playerPos.position.z - (z) > ChunkGen.chunkSize*ChunkGen.renderDistance /2f)
        {
            MeshGenerator(x, y, z+(ChunkGen.renderDistance*ChunkGen.chunkSize), ChunkGen.chunkSize, ChunkGen.textureAtlasSize);
        }
        else
        if ((z) - ChunkGen.playerPos.position.z > ChunkGen.chunkSize*ChunkGen.renderDistance /2f)
        {
            MeshGenerator(x, y, z-(ChunkGen.renderDistance*ChunkGen.chunkSize), ChunkGen.chunkSize, ChunkGen.textureAtlasSize);
        }
        */
    }
    
    public void MeshGenerator(int x, int y, int z, int csz, int s, Vector2 atlas)
    {   
        // initialise mesh
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();

        ox = x; oy = y; oz = z;
        seed = s;
        
        // create faces
        int vt = 0; // vertex index
        int vx = 0; // voxel  index
        for (int i = 0; i < csz; i++)
        {
            for (int j = 0; j < csz; j++)
            {
                for (int k = 0; k < csz; k++)
                {
                    Vector3 coord = new Vector3(i+(x), j+(y), k+(z));
                    int id = BlockId(coord);
                    
                    if (BlockId(coord) != 0)
                    {
                        int sr = 0; // sides rendered
                        List<int> so = new List<int>(); // side order
                        // front
                        if (BlockTransparent(new Vector3(coord.x, coord.y, coord.z-1)))
                        {
                            vertices.Add(new Vector3(i  +(x), j  +(y), k  +(z))); // 0
                            vertices.Add(new Vector3(i  +(x), j+1+(y), k  +(z))); // 1
                            vertices.Add(new Vector3(i+1+(x), j  +(y), k  +(z))); // 2
                            vertices.Add(new Vector3(i+1+(x), j+1+(y), k  +(z))); // 3
                            triangles.Add(vt+(sr*4)+0 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+2 );
                            triangles.Add(vt+(sr*4)+2 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+3 ); 
                            sr++; so.Add(0);
                        }
                        // left
                        if (BlockTransparent(new Vector3(coord.x-1, coord.y, coord.z)))
                        {
                            vertices.Add(new Vector3(i  +(x), j  +(y), k+1+(z))); // 4
                            vertices.Add(new Vector3(i  +(x), j+1+(y), k+1+(z))); // 5
                            vertices.Add(new Vector3(i  +(x), j  +(y), k  +(z))); // 6
                            vertices.Add(new Vector3(i  +(x), j+1+(y), k  +(z))); // 7
                            triangles.Add(vt+(sr*4)+0 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+2 );
                            triangles.Add(vt+(sr*4)+2 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+3 ); 
                            sr++; so.Add(1);
                        }
                        //right
                        if (BlockTransparent(new Vector3(coord.x+1, coord.y, coord.z)))
                        {
                            vertices.Add(new Vector3(i+1+(x), j  +(y), k  +(z))); // 8
                            vertices.Add(new Vector3(i+1+(x), j+1+(y), k  +(z))); // 9
                            vertices.Add(new Vector3(i+1+(x), j  +(y), k+1+(z))); // 10
                            vertices.Add(new Vector3(i+1+(x), j+1+(y), k+1+(z))); // 11
                            triangles.Add(vt+(sr*4)+0 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+2 );
                            triangles.Add(vt+(sr*4)+2 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+3 ); 
                            sr++; so.Add(2);
                        }
                        // back
                        if (BlockTransparent(new Vector3(coord.x, coord.y, coord.z+1)))
                        {
                            vertices.Add(new Vector3(i  +(x), j  +(y), k+1+(z))); // 12
                            vertices.Add(new Vector3(i  +(x), j+1+(y), k+1+(z))); // 13
                            vertices.Add(new Vector3(i+1+(x), j  +(y), k+1+(z))); // 14
                            vertices.Add(new Vector3(i+1+(x), j+1+(y), k+1+(z))); // 15
                            triangles.Add(vt+(sr*4)+2 ); triangles.Add(vt+(sr*4)+3 ); triangles.Add(vt+(sr*4)+0 );
                            triangles.Add(vt+(sr*4)+0 ); triangles.Add(vt+(sr*4)+3 ); triangles.Add(vt+(sr*4)+1 );
                            sr++; so.Add(3);
                        }
                        // top
                        if (BlockTransparent(new Vector3(coord.x, coord.y+1, coord.z)))
                        {
                            vertices.Add(new Vector3(i  +(x), j+1+(y), k  +(z))); // 16
                            vertices.Add(new Vector3(i  +(x), j+1+(y), k+1+(z))); // 17
                            vertices.Add(new Vector3(i+1+(x), j+1+(y), k  +(z))); // 18
                            vertices.Add(new Vector3(i+1+(x), j+1+(y), k+1+(z))); // 19
                            triangles.Add(vt+(sr*4)+0 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+2 );
                            triangles.Add(vt+(sr*4)+2 ); triangles.Add(vt+(sr*4)+1 ); triangles.Add(vt+(sr*4)+3 ); 
                            sr++; so.Add(4);
                        }
                        // bottom
                        if (BlockTransparent(new Vector3(coord.x, coord.y-1, coord.z)))
                        {
                            vertices.Add(new Vector3(i  +(x), j  +(y), k  +(z))); // 20
                            vertices.Add(new Vector3(i  +(x), j  +(y), k+1+(z))); // 21
                            vertices.Add(new Vector3(i+1+(x), j  +(y), k  +(z))); // 22
                            vertices.Add(new Vector3(i+1+(x), j  +(y), k+1+(z))); // 23
                            triangles.Add(vt+(sr*4)+2 ); triangles.Add(vt+(sr*4)+3 ); triangles.Add(vt+(sr*4)+0 );
                            triangles.Add(vt+(sr*4)+0 ); triangles.Add(vt+(sr*4)+3 ); triangles.Add(vt+(sr*4)+1 );
                            sr++; so.Add(5);
                        }
                        
                        // uvs
                        for (int u = 0; u < sr; u++)
                        {
                            // uv coords are slightly off to stop edge leaking
                            // vector2 atlas describes the size of the texture atlas
                            float xOffset = (float)id;        // get block id
                            float yOffset = (float)so[u];     // get block side
                            uv.Add(new Vector2((0f+0.01f+xOffset-1)/atlas.x, (0f+0.01f+yOffset)/atlas.y));
                            uv.Add(new Vector2((0f+0.01f+xOffset-1)/atlas.x, (1f-0.01f+yOffset)/atlas.y));
                            uv.Add(new Vector2((1f-0.01f+xOffset-1)/atlas.x, (0f+0.01f+yOffset)/atlas.y));
                            uv.Add(new Vector2((1f-0.01f+xOffset-1)/atlas.x, (1f-0.01f+yOffset)/atlas.y));
                        }

                        // normals
                        for (int u = 0; u < sr; u++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                if (so[u] == 0)
                                {normals.Add(-Vector3.forward);} else
                                if (so[u] == 1)
                                {normals.Add( Vector3.left   );} else
                                if (so[u] == 2)
                                {normals.Add( Vector3.right  );} else
                                if (so[u] == 3)
                                {normals.Add(-Vector3.back   );} else
                                if (so[u] == 4)
                                {normals.Add( Vector3.up     );} else
                                if (so[u] == 5)
                                {normals.Add( Vector3.down   );}
                            }
                        }

                        vt = vt + sr*4;
                    }
                    vx++;
                }
            }
        }

        // finilise mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.normals = normals.ToArray();
        mesh.Optimize(); mesh.MarkDynamic();

        if (!setup)
        {
            meshRenderer = this.gameObject.AddComponent<MeshRenderer>(); 
            meshFilter = this.gameObject.AddComponent<MeshFilter>(); 
            setup = true;
        }
            
        meshRenderer.material = material;
        meshFilter.mesh = mesh;
    }

    bool BlockTransparent(Vector3 coord)
    {
        bool trans;
        int id = BlockId(coord);
        if (id == 0 || id == 6)
        {trans = true;} else {trans = false;}
        return trans;
    }
    
    int BlockId(Vector3 coord)
    {
        int id = 0;
        id = PassTwo(coord);
        return id;
    }

    int PassTwo(Vector3 coord)
    {
        int id = 0;
        if (PassOne(coord) > 1)
        {id = PassOne(coord);}
        else
        {
            // air
            if (PassOne(coord) == 0)
            {id = 0;}

            // dirt
            if (PassOne(coord) == 1)
            {id = 1;}

            // grass and soft rock
            if (PassOne(coord) == 1 && PassOne(new Vector3(coord.x, coord.y+1, coord.z)) == 0)
            {if ((float)coord.y/10f < GetNoiseMount(coord)-1f) {id = 3;} else {id = 2;}}

            // tall grass
            if (PassOne(coord) == 0 && PassOne(new Vector3(coord.x, coord.y-1, coord.z)) == 1
              && (float)coord.y/10f > GetNoiseMount(coord)-1f && GetNoiseGrass(coord) > 0.7f)
            {id = 6;}

            // hard rock
            if (PassOne(coord) == 1 && PassOne(new Vector3(coord.x, coord.y+3, coord.z)) == 1 || PassOne(coord) == 1 && coord.y < 0)
            {id = 4;}

            // bedrock
            if (coord.y < -14)
            {id = 5;}
        }
        return id;
    }

    int PassOne(Vector3 coord)
    {
        float noiseDetail = GetNoiseDetail(coord);
        float noiseMount = GetNoiseMount(coord);

        float noiseCave;
        if ((float)coord.y/10f > noiseMount-1f && noiseDetail > 0.03f) {noiseCave = 1f;}
        else {noiseCave = PerlinNoise3D(coord.x/10f, coord.y/10f, coord.z/10f);}

        int id = 0;
        if ((float)coord.y / 10f < noiseDetail+noiseMount && noiseCave > 0.4f)
        {id = 1;}
        else
        {id = 0;}
        return id;
    }
    
    // height calculations
    float GetNoiseDetail(Vector3 coord)
    {
        float noiseDetail = PerlinNoise2D(coord.x/10f, coord.z/10f)/3f;
        return noiseDetail;
    }
    float GetNoiseMount(Vector3 coord)
    {
        float noiseMount = PerlinNoise2D(coord.x/100f, coord.z/100f)*5f; if (noiseMount < 0) {noiseMount = 0;}
        return noiseMount;
    }
    float GetNoiseGrass(Vector3 coord)
    {
        float noiseGrass = PerlinNoise2D(coord.x/50f, coord.z/50f)*1f;
        return noiseGrass;
    }
    float GetNoiseStump(Vector3 coord)
    {
        Random.seed = seed+(Mathf.RoundToInt(coord.x*coord.z*coord.y));
        float noiseStump = Random.value;
        return noiseStump;
    }

    // noise functions
    float PerlinNoise2D(float x, float z)
    {
        return Mathf.PerlinNoise(x+seed, z+seed);
    }
    float PerlinNoise3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x+seed, y+seed);
        float xz = Mathf.PerlinNoise(x+seed, z+seed);
        float yz = Mathf.PerlinNoise(y+seed, z+seed);
        float yx = Mathf.PerlinNoise(y+seed, x+seed);
        float zx = Mathf.PerlinNoise(z+seed, x+seed);
        float zy = Mathf.PerlinNoise(z+seed, y+seed);

        return (xy + xz + yz + yx + zx + zy) / 6;
    }
}
