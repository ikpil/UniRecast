namespace UniRecast
{
    using DotRecast.Detour;
    using UnityEngine;

    public class UniRcNavMesh : Object
    {
        public DtNavMesh NavMesh { get; private set; }

        public UniRcNavMesh(DtNavMesh navMesh)
        {
            NavMesh = navMesh;
        }
    }
}