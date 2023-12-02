using System.Collections;
using System.Collections.Generic;
using DotRecast.Recast.Toolset.Builder;
using UnityEngine;

namespace UniRecast.Runtime.Toolsets
{
    public class UniRcConvexVolumeTool : MonoBehaviour
    {
        [SerializeField]
        private float boxHeight = 6.0f;

        [SerializeField]
        private float boxDescent = 1.0f;

        [SerializeField]
        private float polyOffset = 0.0f;

        [SerializeField]
        private int areaTypeValue = SampleAreaModifications.SAMPLE_AREAMOD_GRASS.Value;
    }
}