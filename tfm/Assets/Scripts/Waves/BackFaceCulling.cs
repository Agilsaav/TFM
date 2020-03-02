using UnityEngine;
using System.Collections;

public class BackFaceCulling : MonoBehaviour {

	void Update () {
	
	}
	private void Start () {
		var meshFilters = GetComponentsInChildren<MeshFilter>();
		for (int m = 0; m < meshFilters.Length; m++)
		{
			var mesh = meshFilters[m].mesh;
			for (int sm = 0; sm < mesh.subMeshCount; sm++)
			{
				var vertices = mesh.vertices;
				var uv = mesh.uv;
				var normals = mesh.normals;
				var szV = vertices.Length;
				var newUv = new Vector2[szV];
				var newNorms = new Vector3[szV];
				for (var j=0; j< szV; j++){
					// revert the new ones
					newNorms[j] = -normals[j];
					newUv[j] = uv[j];
				}
				var triangles = mesh.GetTriangles(sm);
				var szT = triangles.Length;
				var newTris = new int[szT]; // double the triangles
				for (var i=0; i< szT; i+=3){
					// save the new reversed triangle
					var j = i; 
					newTris[j] = triangles[i];
					newTris[j+2] = triangles[i+1];
					newTris[j+1] = triangles[i+2];
				}

				mesh.uv = newUv;
				mesh.normals = newNorms;
				mesh.SetTriangles(newTris,sm); // assign triangles last!	
			}

		}
	}
}
