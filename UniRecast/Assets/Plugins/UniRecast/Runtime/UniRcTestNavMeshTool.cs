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

        [SerializeField]
        private bool constrainByCircle = true;

        [SerializeField]
        private int includeFlags = -1;
        
        [SerializeField]
        private int excludeFlags = 0;
    }
}