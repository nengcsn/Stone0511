using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stone
{
    GameObject _goStone;
    Vector3 _centerOfGravity;
    Vector3 _stoneNormal;
    //List<Voxel> _voxels;
    float _longestLength;
    List<NormalGroup> _stoneNormals;
    List<MeshTriangle> _triangleMesh;
    float _weight;
    float _voxelSize = 0.1f;
    private GameObject[,,] _voxels;
    public Vector3 Normal;
    public float NormalTollerance;


    List<Stone> neighbours;
    List<SpringJoint> joints;

    public Stone(GameObject goStone)
    {
        _goStone = goStone;
    }
    public void VoxeliseMesh()
    {
        //Get bounds of the mesh
        //divide bounds into voxelsize
        //create voxelgrid in the bounds of the mesh
        //Check which voxels are inside the mesh
        //set the voxels active

        //Make a variable that store stoneMesh.bounds
        Mesh stoneMesh = _goStone.GetComponent<MeshFilter>().mesh;
        Vector3 centerPoint = stoneMesh.bounds.center;
        Vector3 extents = stoneMesh.bounds.extents;
        Debug.Log(extents);

        int gridX = Mathf.CeilToInt(stoneMesh.bounds.size.x / _voxelSize);
        int gridY = Mathf.CeilToInt(stoneMesh.bounds.size.y / _voxelSize);
        int gridZ = Mathf.CeilToInt(stoneMesh.bounds.size.z / _voxelSize);

        //float r = Mathf.Min(extents.x, extents.y, extents.z) / 10;
        _voxels = new GameObject[gridX, gridY, gridZ];
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    Vector3 localPosition = stoneMesh.bounds.min + (new Vector3(x, y, z) * _voxelSize);
                    if (Util.IsPointInCollider(_goStone.GetComponent<MeshCollider>(), _goStone.transform.position + _goStone.transform.TransformVector(localPosition)))
                    {
                        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        voxel.transform.SetParent(_goStone.transform);
                        voxel.transform.localEulerAngles = Vector3.zero;
                        voxel.transform.localPosition = localPosition;
                        voxel.transform.localScale = Vector3.one * _voxelSize;
                        GameObject.Destroy(voxel.GetComponent<Collider>());

                        _voxels[x, y, z] = voxel;
                    }
                }
            }
        }
        _goStone.GetComponent<MeshRenderer>().enabled = false;


        GetStoneNormal();
    }

    public void GetStoneNormal()
    {
        List<GameObject> voxelList = new List<GameObject>();

        for (int x = 0; x < _voxels.GetLength(0); x++)
            for (int y = 0; y < _voxels.GetLength(1); y++)
                for (int z = 0; z < _voxels.GetLength(2); z++)
                {
                    if (_voxels[x, y, z] != null) voxelList.Add(_voxels[x, y, z]);
                }



        Vector3 longestLine = Vector3.zero;
        for (int i = 0; i < voxelList.Count; i++)
        {
            for (int j = 0; j < voxelList.Count; j++)
            {
                Vector3 line = voxelList[i].transform.position - voxelList[j].transform.position;
                if (line.magnitude > longestLine.magnitude)
                {
                    longestLine = line;
                }
            }
        }

        Debug.Log($"{longestLine} is the stones longest line");
        RotateStoneToX(longestLine);
    }

    public void RotateStoneToX(Vector3 normal)
    {
        _goStone.transform.rotation = Quaternion.FromToRotation(normal.normalized, Vector3.right);
    }

    public Vector3 GetCenterOfGravity(List<Voxel> stoneVoxel)
    {
        //add the center position of each voxel
        //divide result by the amount of voxel

        return Vector3.zero;
    }




    public List<Vector3> GetMeshNormals(Mesh stone)
    {
        //take a random face in the mesh
        // Get its normal
        // make a new normal group with the face normal
        //loop untill entire mesh is checked
        //Get next face using Breadth First Search
        //if angle between new face normal and group normal < tolerance
        //add the face to the normal group
        //get the average normal of all faces in this normal group
        //if angle between new face normal and group normal > tolerance
        //Make a new normal group
        //Set next face as checked


        _triangleMesh = new List<MeshTriangle>();

        //Fetching triangles from a 3D Mesh
        for (int i = 0; i < stone.triangles.Length - 2; i += 3)
        {
            int index = i;
            Vector3 v1 = stone.vertices[stone.triangles[i]];
            Vector3 v2 = stone.vertices[stone.triangles[i + 1]];
            Vector3 v3 = stone.vertices[stone.triangles[i + 2]];
            //calculate mesh normal
            Vector3 newNormal = Vector3.Cross((v2 - v1), (v3 - v1));

            MeshTriangle newTriMesh = new MeshTriangle(new int[3] { index, index + 1, index + 2 }, newNormal, new Vector3[3] { v1, v2, v3 });
            _triangleMesh.Add(newTriMesh);

            /*
            List<TriMesh> newMeshGroup = new List<TriMesh>();
            
            newMeshGroup.Add(newTriMesh);
            MeshGroups.Add(newMeshGroup);
            newTriMesh.Group = newMeshGroup;
            UnCheckedMeshes.Add(newTriMesh);*/
        }

        return new List<Vector3>();
    }

    public void GetNormalGroups()
    {
        List<MeshTriangle> uncheckedTriangles = new List<MeshTriangle>(_triangleMesh);
        _stoneNormals = new List<NormalGroup>();

        for (int i = 0; i < uncheckedTriangles.Count; i++)
        {
            if (i == 0)
            {
                _stoneNormals.Add(new NormalGroup(uncheckedTriangles[i]));
            }
            else
            {
                bool addedToGroup = false;
                //Check the current triangle with all the normalgroups
                for (int j = 0; j < _stoneNormals.Count; j++)
                {
                    if (Vector3.Angle(uncheckedTriangles[i].TriangleNormal, _stoneNormals[j].GroupNormal) < NormalTollerance)
                    {
                        _stoneNormals[j].AddNormal(uncheckedTriangles[i]);
                        addedToGroup = true;
                    }
                }
                if (!addedToGroup)
                {
                    //add a new normalgroup to _stoneNormals
                }
            }
        }
    }
}
