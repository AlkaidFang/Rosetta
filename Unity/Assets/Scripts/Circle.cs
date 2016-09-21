using UnityEngine;
using System.Collections.Generic;

public class Circle : MonoBehaviour {

	Mesh mMesh;

	// Use this for initialization
	void Start () {

		GameObject go = new GameObject ("circle_go");
		MeshFilter mf = go.AddComponent<MeshFilter> ();
		MeshRenderer mr = go.AddComponent<MeshRenderer> ();

		mMesh = new Mesh ();
		mMesh.name = "circle_mesh";

		// double circle
		List<Vector3> outPoses = new List<Vector3>();
		List<Vector3> inPoses = new List<Vector3>();
		Vector3 centerPos = Vector3.zero;
		float r = 1.0f;
		float R = 1.1f;
		Vector3 outStartPos = new Vector3 (centerPos.x, centerPos.y + R, centerPos.z);
		Vector3 inStartPos = new Vector3 (centerPos.x, centerPos.y + r, centerPos.z);

		CalculatePositions (outStartPos, centerPos, 128, outPoses);
		CalculatePositions (inStartPos, centerPos, 128, inPoses);

		// 生成三角面
		List<Vector3> vertices = new List<Vector3>();
		for (int i = 0; i < outPoses.Count + inPoses.Count; ++i)
		{
			if (i % 2 == 0)
				vertices.Add (outPoses [i / 2]);
			else
				vertices.Add (inPoses [i / 2]);
		}

		mMesh.vertices = vertices.ToArray ();

		List<int> trangles = new List<int> ();
		for (int i = 0; i < mMesh.vertices.Length - 2; i += 2)
		{
			trangles.Add (i);
			trangles.Add (i + 1);
			trangles.Add (i + 2);

			trangles.Add (i + 1);
			trangles.Add (i + 3);
			trangles.Add (i + 2);
		}
		mMesh.triangles = trangles.ToArray ();

		mf.mesh = mMesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void CalculatePositions(Vector3 basePos, Vector3 centerPos, int count, List<Vector3> positions)
	{
		basePos -= centerPos; // 首先获得相对位置

		positions.Add(basePos);

		// 使用四元数连续旋转
		float perAngle = 360.0f / count;
		Quaternion perQ = Quaternion.AngleAxis(perAngle, Vector3.forward);
		Vector3 temp;
		for (int i = 1; i < count; ++i)
		{
			temp = perQ * positions [i - 1];
			positions.Add(temp);
		}

		positions.Add (basePos);

		// 计算反相对后的实际位置
		for (int i = 0; i < positions.Count; ++i)
		{
			positions [i] += centerPos;
		}
	}
}
