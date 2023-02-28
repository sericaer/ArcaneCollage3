using System;
using System.Collections.Generic;
using UnityEngine;
using Maths.Matrix;
using System.Linq;

public static class ExtetionMethods
{
    public static IEnumerable<Vector3Int> GetMaxtrix(this BuildingPlanRender plan)
    {
        return MatrixMethod.Generate((plan.cellPos.x, plan.cellPos.y), plan.x, plan.y).Select(p => new Vector3Int(p.x, p.y));
    }

    public static IEnumerable<Vector3Int> GetRing(this BuildingPlanRender plan, int length)
    {
        return MatrixMethod.GetRing((plan.cellPos.x, plan.cellPos.y), length, plan.x, plan.y).Select(p => new Vector3Int(p.x, p.y));
    }
}
