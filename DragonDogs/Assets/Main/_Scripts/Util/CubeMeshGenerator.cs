using UnityEngine;

// ReSharper disable IdentifierTypo

namespace Main._Scripts.Util
{
	
	/// <summary>
	/// Cube mesh generator
	/// Adapted from http://www.theappguruz.com/blog/simple-cube-mesh-generation-unity3d
	/// </summary>
	public static class CubeMeshGenerator
	{
		public static Mesh MakeCube(float pWidth, float pHeight, float pLength)
		{
			Mesh cubeMesh = new()
			{
				vertices = MakeVertices(pWidth, pHeight, pLength),
				normals = GetNormals(),
				uv = GetUVsMap(),
				triangles = GetTriangles ()
			};

			cubeMesh.RecalculateBounds ();
			cubeMesh.RecalculateNormals ();
			cubeMesh.Optimize ();

			return cubeMesh;
		}

		private static Vector3[] MakeVertices(float pWidth, float pHeight, float pLength)
		{
			Vector3 vertice_0 = new(-pWidth * .5f, -pHeight * .5f, pLength * .5f);
			Vector3 vertice_1 = new(pWidth * .5f, -pHeight * .5f, pLength * .5f);
			Vector3 vertice_2 = new(pWidth * .5f, -pHeight * .5f, -pLength * .5f);
			Vector3 vertice_3 = new(-pWidth * .5f, -pHeight * .5f, -pLength * .5f);    
			Vector3 vertice_4 = new(-pWidth * .5f, pHeight * .5f, pLength * .5f);
			Vector3 vertice_5 = new(pWidth * .5f, pHeight * .5f, pLength * .5f);
			Vector3 vertice_6 = new(pWidth * .5f, pHeight * .5f, -pLength * .5f);
			Vector3 vertice_7 = new(-pWidth * .5f, pHeight * .5f, -pLength * .5f);
			Vector3[] vertices = {
				// Bottom Polygon
				vertice_0, vertice_1, vertice_2, vertice_3,
				
				// Left Polygon
				vertice_7, vertice_4, vertice_0, vertice_3,
				
				// Front Polygon
				vertice_4, vertice_5, vertice_1, vertice_0,
				
				// Back Polygon
				vertice_6, vertice_7, vertice_3, vertice_2,
				
				// Right Polygon
				vertice_5, vertice_6, vertice_2, vertice_1,
				
				// Top Polygon
				vertice_7, vertice_6, vertice_5, vertice_4
			} ;
 
			return vertices;
		}
		
		private static Vector3[] GetNormals()
		{
			Vector3 up = Vector3.up;
			Vector3 down = Vector3.down;
			Vector3 front = Vector3.forward;
			Vector3 back = Vector3.back;
			Vector3 left = Vector3.left;
			Vector3 right = Vector3.right;
    
			Vector3[] normals = {
				// Bottom Side Render
				down, down, down, down,
                    
				// LEFT Side Render
				left, left, left, left,
                    
				// FRONT Side Render
				front, front, front, front,
                    
				// BACK Side Render
				back, back, back, back,
                    
				// RIGHT Side Render
				right, right, right, right,
                    
				// UP Side Render
				up, up, up, up
			} ;
 
			return normals;
		}
		
		private static Vector2[] GetUVsMap()
		{
			Vector2 _00_coordinates = new(0f, 0f);
			Vector2 _10_coordinates = new(1f, 0f);
			Vector2 _01_coordinates = new(0f, 1f);
			Vector2 _11_coordinates = new(1f, 1f);
			
			Vector2[] uvs = {
				// Bottom
				_11_coordinates, _01_coordinates, _00_coordinates, _10_coordinates,
				// Left
				_11_coordinates, _01_coordinates, _00_coordinates, _10_coordinates,
				// Front
				_11_coordinates, _01_coordinates, _00_coordinates, _10_coordinates,
				// Back
				_11_coordinates, _01_coordinates, _00_coordinates, _10_coordinates,
				// Right
				_11_coordinates, _01_coordinates, _00_coordinates, _10_coordinates,
				// Top
				_11_coordinates, _01_coordinates, _00_coordinates, _10_coordinates,
			};
			
			return uvs;
		}
		
		private static int[] GetTriangles()
		{
			int[] triangles = {
				// Cube Bottom Side Triangles
				3, 1, 0,
				3, 2, 1,    
				// Cube Left Side Triangles
				3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
				3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
				// Cube Front Side Triangles
				3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
				3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
				// Cube Back Side Triangles
				3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
				3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
				// Cube Right Side Triangles
				3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
				3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
				// Cube Top Side Triangles
				3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
				3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
			} ;
			return triangles;
		}
	}
}