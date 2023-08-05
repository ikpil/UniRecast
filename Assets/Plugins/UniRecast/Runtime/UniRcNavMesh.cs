using DotRecast.Detour;

namespace UniRecast
{
    public class UniRcNavMesh : UnityEngine.Object
    {
        public DtNavMesh NavMesh { get; private set; }

        public UniRcNavMesh(DtNavMesh navMesh)
        {
            NavMesh = navMesh;
        }
    }
}