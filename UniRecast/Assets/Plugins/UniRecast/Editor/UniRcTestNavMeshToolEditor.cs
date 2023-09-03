using System.Linq;
using DotRecast.Core;
using DotRecast.Detour;
using DotRecast.Recast.Toolset.Builder;
using DotRecast.Recast.Toolset.Tools;
using Plugins.UniRecast.Extensions;
using UnityEditor;
using UnityEngine;

namespace UniRecast.Editor
{


    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniRcTestNavMeshTool))]
    public class UniRcTestNavMeshToolEditor : UnityEditor.Editor
    {
        private SerializedProperty _selectedModeIdx;
        private SerializedProperty _selectedStraightPathOptionIdx;
        private SerializedProperty _constrainByCircle;
        private SerializedProperty _includeFlags;
        private SerializedProperty _excludeFlags;

        private static readonly string[] ModeLabels = RcTestNavmeshToolMode.Values.Select(x => x.Label).ToArray();
        private static readonly string[] StraightPathOptionLabels = DtStraightPathOption.Values.Select(x => x.Label).ToArray();

        private void OnEnable()
        {
            _selectedModeIdx = serializedObject.SafeFindProperty(nameof(_selectedModeIdx));
            _selectedStraightPathOptionIdx = serializedObject.SafeFindProperty(nameof(_selectedStraightPathOptionIdx));
            _constrainByCircle = serializedObject.SafeFindProperty(nameof(_constrainByCircle));
            _includeFlags = serializedObject.SafeFindProperty(nameof(_includeFlags));
            _excludeFlags = serializedObject.SafeFindProperty(nameof(_excludeFlags));
        }


        public override void OnInspectorGUI()
        {
            var surface = target as UniRcTestNavMeshTool;
            if (surface is null)
                return;

            UniRcGui.Text("Mode");
            UniRcGui.Separator();
            EditorGUI.BeginChangeCheck();
            int selectedModeIdx = GUILayout.SelectionGrid(_selectedModeIdx.intValue, ModeLabels, 1, EditorStyles.radioButton);
            if (EditorGUI.EndChangeCheck())
            {
                _selectedModeIdx.intValue = selectedModeIdx;
                // ...
            }

            // selecting mode
            var mode = RcTestNavmeshToolMode.Values[selectedModeIdx];
            UniRcGui.Text(mode.Label);
            UniRcGui.Separator();

            if (RcTestNavmeshToolMode.PATHFIND_FOLLOW == mode)
            {
            }

            if (RcTestNavmeshToolMode.PATHFIND_STRAIGHT == mode)
            {
                UniRcGui.Text("Vertices at crossings");
                UniRcGui.Separator();
                EditorGUI.BeginChangeCheck();
                int selectedStraightPathOptionIdx = GUILayout.SelectionGrid(_selectedStraightPathOptionIdx.intValue, StraightPathOptionLabels, 1, EditorStyles.radioButton);
                if (EditorGUI.EndChangeCheck())
                {
                    _selectedStraightPathOptionIdx.intValue = selectedStraightPathOptionIdx;
                }

                var straightPathOption = DtStraightPathOption.Values[selectedStraightPathOptionIdx];
            }

            if (RcTestNavmeshToolMode.PATHFIND_SLICED == mode)
            {
            }

            if (RcTestNavmeshToolMode.DISTANCE_TO_WALL == mode)
            {
            }

            if (RcTestNavmeshToolMode.RAYCAST == mode)
            {
            }

            if (RcTestNavmeshToolMode.FIND_POLYS_IN_CIRCLE == mode)
            {
                UniRcGui.Checkbox("Constrained", _constrainByCircle);
            }

            if (RcTestNavmeshToolMode.FIND_POLYS_IN_SHAPE == mode)
            {
            }

            if (RcTestNavmeshToolMode.FIND_LOCAL_NEIGHBOURHOOD == mode)
            {
            }

            if (RcTestNavmeshToolMode.RANDOM_POINTS_IN_CIRCLE == mode)
            {
            }

            UniRcGui.Text("Common");
            UniRcGui.Separator();

            UniRcGui.Text("Include Flags");
            UniRcGui.Separator();
            UniRcGui.CheckboxFlags("Walk", _includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_WALK);
            UniRcGui.CheckboxFlags("Swim", _includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_SWIM);
            UniRcGui.CheckboxFlags("Door", _includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_DOOR);
            UniRcGui.CheckboxFlags("Jump", _includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_JUMP);
            UniRcGui.Separator();
            UniRcGui.NewLine();
            //
            // m_filter.SetIncludeFlags(_option.includeFlags);
            //
            UniRcGui.Text("Exclude Flags");
            UniRcGui.Separator();
            UniRcGui.CheckboxFlags("Walk", _excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_WALK);
            UniRcGui.CheckboxFlags("Swim", _excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_SWIM);
            UniRcGui.CheckboxFlags("Door", _excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_DOOR);
            UniRcGui.CheckboxFlags("Jump", _excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_JUMP);
            UniRcGui.NewLine();
            //
            // m_filter.SetExcludeFlags(_option.excludeFlags);
            //
            // bool previousEnableRaycast = _option.enableRaycast;
            // ImGui.Checkbox("Raycast shortcuts", ref _option.enableRaycast);
            //

            serializedObject.ApplyModifiedProperties();
            // if (previousToolMode != _option.mode || _option.straightPathOptions != previousStraightPathOptions
            //                                      || previousIncludeFlags != _option.includeFlags || previousExcludeFlags != _option.excludeFlags
            //                                      || previousEnableRaycast != _option.enableRaycast || previousConstrainByCircle != _option.constrainByCircle)
            // {
            //     Recalc();
            // }
        }
    }
}