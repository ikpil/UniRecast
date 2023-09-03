using System.Linq;
using DotRecast.Core;
using DotRecast.Detour;
using DotRecast.Recast.Toolset.Tools;
using Plugins.UniRecast.Extensions;
using UnityEditor;
using UnityEngine;

namespace UniRecast.Editor
{
    public class RcStraightPathOption
    {
        public static readonly RcStraightPathOption None = new RcStraightPathOption(0, "None");
        public static readonly RcStraightPathOption AreaCrossings = new RcStraightPathOption(DtNavMeshQuery.DT_STRAIGHTPATH_AREA_CROSSINGS, "Area");
        public static readonly RcStraightPathOption AllCrossings = new RcStraightPathOption(DtNavMeshQuery.DT_STRAIGHTPATH_ALL_CROSSINGS, "All");

        public static readonly RcImmutableArray<RcStraightPathOption> Values = RcImmutableArray.Create(
            None, AreaCrossings, AllCrossings
        );

        public readonly int Value;
        public readonly string Label;

        private RcStraightPathOption(int value, string label)
        {
            Value = value;
            Label = label;
        }
    }


    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniRcTestNavMeshTool))]
    public class UniRcTestNavMeshToolEditor : UnityEditor.Editor
    {
        private SerializedProperty _selectedModeIdx;
        private SerializedProperty _selectedStraightPathOptionIdx;

        private static readonly string[] ModeLabels = RcTestNavmeshToolMode.Values.Select(x => x.Label).ToArray();
        private static readonly string[] StraightPathOptionLabels = RcStraightPathOption.Values.Select(x => x.Label).ToArray();

        private void OnEnable()
        {
            _selectedModeIdx = serializedObject.SafeFindProperty(nameof(_selectedModeIdx));
            _selectedStraightPathOptionIdx = serializedObject.SafeFindProperty(nameof(_selectedStraightPathOptionIdx));
        }


        public override void OnInspectorGUI()
        {
            var surface = target as UniRcTestNavMeshTool;
            if (surface is null)
                return;

            EditorGUILayout.LabelField("Mode");
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            int selectedModeIdx = GUILayout.SelectionGrid(_selectedModeIdx.intValue, ModeLabels, 1, EditorStyles.radioButton);
            if (EditorGUI.EndChangeCheck())
            {
                _selectedModeIdx.intValue = selectedModeIdx;
                // ...
            }

            // selecting mode
            var mode = RcTestNavmeshToolMode.Values[selectedModeIdx];
            EditorGUILayout.LabelField(mode.Label);
            EditorGUILayout.Separator();

            if (RcTestNavmeshToolMode.PATHFIND_FOLLOW == mode)
            {
            }

            if (RcTestNavmeshToolMode.PATHFIND_STRAIGHT == mode)
            {
                EditorGUILayout.LabelField("Vertices at crossings");
                EditorGUILayout.Separator();
                EditorGUI.BeginChangeCheck();
                int selectedStraightPathOptionIdx = GUILayout.SelectionGrid(_selectedStraightPathOptionIdx.intValue, StraightPathOptionLabels, 1, EditorStyles.radioButton);
                if (EditorGUI.EndChangeCheck())
                {
                    _selectedStraightPathOptionIdx.intValue = selectedStraightPathOptionIdx;
                }

                var straightPathOption = RcStraightPathOption.Values[selectedStraightPathOptionIdx];
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
                //UniRcEditorHelpers.Checkbox("Constrained", ref s);
                //ImGui.Checkbox("Constrained", ref _option.constrainByCircle);
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

            // ImGui.Text("Common");
            // ImGui.Separator();
            //
            // ImGui.Text("Include Flags");
            // ImGui.Separator();
            // ImGui.CheckboxFlags("Walk", ref _option.includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_WALK);
            // ImGui.CheckboxFlags("Swim", ref _option.includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_SWIM);
            // ImGui.CheckboxFlags("Door", ref _option.includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_DOOR);
            // ImGui.CheckboxFlags("Jump", ref _option.includeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_JUMP);
            // ImGui.NewLine();
            //
            // m_filter.SetIncludeFlags(_option.includeFlags);
            //
            // ImGui.Text("Exclude Flags");
            // ImGui.Separator();
            // ImGui.CheckboxFlags("Walk", ref _option.excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_WALK);
            // ImGui.CheckboxFlags("Swim", ref _option.excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_SWIM);
            // ImGui.CheckboxFlags("Door", ref _option.excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_DOOR);
            // ImGui.CheckboxFlags("Jump", ref _option.excludeFlags, SampleAreaModifications.SAMPLE_POLYFLAGS_JUMP);
            // ImGui.NewLine();
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