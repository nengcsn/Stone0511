using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGroup
{
    List<Pose> normals;
    Pose _groupNormal;

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



        List<List<TriMesh>> MeshGroups = new List<List<TriMesh>>();
        List<TriMesh> UnCheckedMeshes = new List<TriMesh>();

        //Fetching triangles from a 3D Mesh
        for (int i = 0; i < stone.triangles.Length - 2; i += 3)
        {
            int index = i;
            Vector3 v1 = stone.vertices[stone.triangles[i]];
            Vector3 v2 = stone.vertices[stone.triangles[i + 1]];
            Vector3 v3 = stone.vertices[stone.triangles[i + 2]];
            //calculate mesh normal
            Vector3 newNormal = Vector3.Cross((v2 - v1), (v3 - v1));

            List<TriMesh> newMeshGroup = new List<TriMesh>();
            TriMesh newTriMesh = new TriMesh(new int[3] { index, index + 1, index + 2 }, newNormal, new Vector3[3] { v1, v2, v3 });
            newMeshGroup.Add(newTriMesh);
            MeshGroups.Add(newMeshGroup);
            newTriMesh.Group = newMeshGroup;
            UnCheckedMeshes.Add(newTriMesh);
        }
    }

    public class TriMesh
    {
        public List<TriMesh> Group;
        //Index of 3 vertices of the triangle mesh
        public int[] verticeIndexes;
        //vertices of the triangle mesh
        public Vector3[] vertices;
        public Vector3 meshNormal;

        public TriMesh(int[] _verticeIndexes, Vector3 _meshNormal, Vector3[] _vertices)
        {
            verticeIndexes = _verticeIndexes;
            vertices = _vertices;
            meshNormal = _meshNormal;
         
        }

        public TriMesh(List<TriMesh> group, int[] verticeIndexes, Vector3[] vertices, Vector3 meshNormal)
        {
            Group = group;
            this.verticeIndexes = verticeIndexes;
            this.vertices = vertices;
            this.meshNormal = meshNormal;
        }
    }
}

//        List<Vector3> normalGroup = new List<Vector3>();
//        float tolerance = 0.2f;
//        Vector3 randomNormal = stone.normals[0];
//        normalGroup.Add(firstNormal);
//        Vector3 groupNormal = randomNormal;
//        for (int i = 0; i < stone.normals.Length; i++)
//        {
//            if (Vector3.Angle(groupNormal, stone.normals[i]) < tolerance)
//            {
//                normalGroup.Add(stone.normals[i]);
//                groupNormal = normalGroup.Average();
//            }
//            else
//            {
//                normalGroup = new List<Vector3>();
//            }
//        }
//        return normalGroup;
//    }
//}


