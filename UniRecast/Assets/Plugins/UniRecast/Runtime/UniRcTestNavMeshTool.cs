using DotRecast.Recast.Toolset.Tools;
using UnityEngine;

namespace UniRecast
{
    public class UniRcTestNavMeshTool : MonoBehaviour
    {
        private RcTestNavMeshTool _tool;

        [SerializeField]
        private int selectedModeIdx = 0;

        [SerializeField]
        private int selectedStraightPathOptionIdx = 0;
    }
}