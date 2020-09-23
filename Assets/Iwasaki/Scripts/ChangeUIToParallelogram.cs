using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 四角形を平行四辺形に変更します
/// </summary>
public class ChangeUIToParallelogram : BaseMeshEffect
{

    /// <summary>左側か右側か向きを決めます</summary>
    public bool isLeft;

    /// <summary>水平線とのなす角</summary>
    [Range(0, 75)]
    public int angle = 30;

    public override void ModifyMesh(Mesh in_mesh)
    {
        if (!this.IsActive()) return;

        List<UIVertex> list = new List<UIVertex>();
        using (VertexHelper vertexHelper = new VertexHelper(in_mesh))
        {
            vertexHelper.GetUIVertexStream(list);
        }

        this.ModifyVertices(list);

        using (VertexHelper vertexHelper2 = new VertexHelper())
        {
            vertexHelper2.AddUIVertexTriangleStream(list);
            vertexHelper2.FillMesh(in_mesh);
        }

    }

    public override void ModifyMesh(VertexHelper in_vh)
    {
        if (!this.IsActive())
            return;

        List<UIVertex> vertexList = new List<UIVertex>();
        in_vh.GetUIVertexStream(vertexList);

        ModifyVertices(vertexList);

        in_vh.Clear();
        in_vh.AddUIVertexTriangleStream(vertexList);
    }

    public void ModifyVertices(List<UIVertex> in_vList)
    {
        if (this.IsActive() == false || in_vList == null || in_vList.Count == 0) return;

        Vector3 vec = in_vList[1].position - in_vList[0].position;
        float h = vec.y;
        float fac = h * Mathf.Tan(Mathf.Deg2Rad * this.angle);
        float left = (this.isLeft) ? fac : 0f;
        float right = (this.isLeft) ? 0f : fac;

        List<Vector3> v = new List<Vector3>();
        v.Add(new Vector3(left, 0f, 0f));
        v.Add(new Vector3(right, 0f, 0f));
        v.Add(new Vector3(right, 0f, 0f));
        v.Add(new Vector3(right, 0f, 0f));
        v.Add(new Vector3(left, 0f, 0f));
        v.Add(new Vector3(left, 0f, 0f));

        for (int i = 0; i < in_vList.Count; i++)
        {
            UIVertex tmpV = in_vList[i];
            tmpV.position += v[i];
            in_vList[i] = tmpV;
        }
    }

}
