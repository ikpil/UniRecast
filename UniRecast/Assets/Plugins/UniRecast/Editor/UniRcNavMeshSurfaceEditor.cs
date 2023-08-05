using System;
using DotRecast.Detour;
using DotRecast.Recast.Toolset;
using UnityEditor;
using UnityEngine;

namespace UniRecast.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniRcNavMeshSurface))]
    public class UniRcNavMeshSurfaceEditor : UnityEditor.Editor
    {
        SerializedProperty NavMesh;

        // Rasterization
        private SerializedProperty _cellSize;
        private SerializedProperty _cellHeight;

        // Agent
        private SerializedProperty _agentHeight;
        private SerializedProperty _agentRadius;
        private SerializedProperty _agentMaxClimb;
        private SerializedProperty _agentMaxSlope;
        private SerializedProperty _agentMaxAcceleration;
        private SerializedProperty _agentMaxSpeed;

        // Region
        private SerializedProperty _minRegionSize;
        private SerializedProperty _mergedRegionSize;

        // Filtering
        private SerializedProperty _filterLowHangingObstacles;
        private SerializedProperty _filterLedgeSpans;
        private SerializedProperty _filterWalkableLowHeightSpans;

        // Polygonization
        private SerializedProperty _edgeMaxLen;
        private SerializedProperty _edgeMaxError;
        private SerializedProperty _vertsPerPoly;

        // Detail Mesh
        private SerializedProperty _detailSampleDist;
        private SerializedProperty _detailSampleMaxError;
        
        // Tiles
        private SerializedProperty _tileSize;
        static Color s_HandleColor = new Color(127f, 214f, 244f, 100f) / 255;
        //static Color s_HandleColorSelected = new Color(127f, 63.0f, 244f, 210f) / 255;
        private static Color s_HandleColorSelected = new Color(0.0f, 0.75f, 1f, 0.5f);
        static Color s_HandleColorDisabled = new Color(127f * 0.75f, 214f * 0.75f, 244f * 0.75f, 100f) / 255;

        private void OnDisable()
        {
        }

        private void OnEnable()
        {
            NavMesh = serializedObject.FindProperty(nameof(UniRcNavMeshSurface.NavMesh));

            // Rasterization
            _cellSize = serializedObject.FindProperty(nameof(_cellSize));
            _cellHeight = serializedObject.FindProperty(nameof(_cellHeight));

            // Agent
            _agentHeight = serializedObject.FindProperty(nameof(_agentHeight));
            _agentRadius = serializedObject.FindProperty(nameof(_agentRadius));
            _agentMaxClimb = serializedObject.FindProperty(nameof(_agentMaxClimb));
            _agentMaxSlope = serializedObject.FindProperty(nameof(_agentMaxSlope));
            _agentMaxAcceleration = serializedObject.FindProperty(nameof(_agentMaxAcceleration));
            _agentMaxSpeed = serializedObject.FindProperty(nameof(_agentMaxSpeed));

            // Region
            _minRegionSize = serializedObject.FindProperty(nameof(_minRegionSize));
            _mergedRegionSize = serializedObject.FindProperty(nameof(_mergedRegionSize));

            // Filtering
            _filterLowHangingObstacles = serializedObject.FindProperty(nameof(_filterLowHangingObstacles));
            _filterLedgeSpans = serializedObject.FindProperty(nameof(_filterLedgeSpans));
            _filterWalkableLowHeightSpans = serializedObject.FindProperty(nameof(_filterWalkableLowHeightSpans));

            // Polygonization
            _edgeMaxLen = serializedObject.FindProperty(nameof(_edgeMaxLen));
            _edgeMaxError = serializedObject.FindProperty(nameof(_edgeMaxError));
            _vertsPerPoly = serializedObject.FindProperty(nameof(_vertsPerPoly));

            // Detail Mesh
            _detailSampleDist = serializedObject.FindProperty(nameof(_detailSampleDist));
            _detailSampleMaxError = serializedObject.FindProperty(nameof(_detailSampleMaxError));
            
            // Tiles
            _tileSize = serializedObject.FindProperty(nameof(_tileSize));
        }

        private void Clear()
        {
            // 기본값 초기화
            var bs = new RcNavMeshBuildSettings();

            // Rasterization
            _cellSize.floatValue = bs.cellSize;
            _cellHeight.floatValue = bs.cellHeight;

            // Agent
            _agentHeight.floatValue = bs.agentHeight;
            _agentRadius.floatValue = bs.agentRadius;
            _agentMaxClimb.floatValue = bs.agentMaxClimb;
            _agentMaxSlope.floatValue = bs.agentMaxSlope;
            _agentMaxAcceleration.floatValue = bs.agentMaxAcceleration;
            _agentMaxSpeed.floatValue = bs.agentMaxSpeed;

            // Region
            _minRegionSize.intValue = bs.minRegionSize;
            _mergedRegionSize.intValue = bs.mergedRegionSize;

            // Filtering
            _filterLowHangingObstacles.boolValue = bs.filterLowHangingObstacles;
            _filterLedgeSpans.boolValue = bs.filterLedgeSpans;
            _filterWalkableLowHeightSpans.boolValue = bs.filterWalkableLowHeightSpans;

            // Polygonization
            _edgeMaxLen.floatValue = bs.edgeMaxLen;
            _edgeMaxError.floatValue = bs.edgeMaxError;
            _vertsPerPoly.intValue = bs.vertsPerPoly;

            // Detail Mesh
            _detailSampleDist.floatValue = bs.detailSampleDist;
            _detailSampleMaxError.floatValue = bs.detailSampleMaxError;
            
            // Tiles
            _tileSize.intValue = bs.tileSize;
        }

        private void Bake()
        {
            var surface = target as UniRcNavMeshSurface;
            if (null == surface)
            {
                Debug.LogError($"not found UniRcNavMeshSurface");
                return;
            }

            surface.Bake();
        }

        public override void OnInspectorGUI()
        {
            var surface = target as UniRcNavMeshSurface;
            if (surface is null)
                return;

            // Draw image
            const float diagramHeight = 80.0f;
            Rect agentDiagramRect = EditorGUILayout.GetControlRect(false, diagramHeight);
            UniRcEditorHelpers.DrawAgentDiagram(agentDiagramRect, _agentRadius.floatValue, _agentHeight.floatValue, _agentMaxClimb.floatValue, _agentMaxSlope.floatValue);

            EditorGUILayout.LabelField("Rasterization");
            EditorGUILayout.Separator();
            UniRcEditorHelpers.SliderFloat("Cell Size", _cellSize, 0.01f, 1f, 0.01f);
            UniRcEditorHelpers.SliderFloat("Cell Height", _cellHeight, 0.01f, 1f, 0.01f);
            //EditorGUILayout.LabelField($"Voxels {voxels[0]} x {voxels[1]}");
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Agent");
            EditorGUILayout.Separator();
            UniRcEditorHelpers.SliderFloat("Height", _agentHeight, 0.1f, 5f, 0.1f);
            UniRcEditorHelpers.SliderFloat("Radius", _agentRadius, 0.1f, 5f, 0.1f);
            UniRcEditorHelpers.SliderFloat("Max Climb", _agentMaxClimb, 0.1f, 5f, 0.1f);
            UniRcEditorHelpers.SliderFloat("Max Slope", _agentMaxSlope, 1f, 90f, 1);
            UniRcEditorHelpers.SliderFloat("Max Acceleration", _agentMaxAcceleration, 8f, 999f, 0.1f);
            UniRcEditorHelpers.SliderFloat("Max Speed", _agentMaxSpeed, 1f, 10f, 0.1f);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Region");
            EditorGUILayout.Separator();
            UniRcEditorHelpers.SliderInt("Min Region Size", _minRegionSize, 1, 150);
            UniRcEditorHelpers.SliderInt("Merged Region Size", _mergedRegionSize, 1, 150);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Filtering");
            EditorGUILayout.Separator();
            UniRcEditorHelpers.Checkbox("Low Hanging Obstacles", _filterLowHangingObstacles);
            UniRcEditorHelpers.Checkbox("Ledge Spans", _filterLedgeSpans);
            UniRcEditorHelpers.Checkbox("Walkable Low Height Spans", _filterWalkableLowHeightSpans);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Polygonization");
            EditorGUILayout.Separator();
            UniRcEditorHelpers.SliderFloat("Max Edge Length", _edgeMaxLen, 0f, 50f, 0.1f);
            UniRcEditorHelpers.SliderFloat("Max Edge Error", _edgeMaxError, 0.1f, 3f, 0.1f);
            UniRcEditorHelpers.SliderInt("Vert Per Poly", _vertsPerPoly, 3, 12);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Detail Mesh");
            EditorGUILayout.Separator();
            UniRcEditorHelpers.SliderFloat("Sample Distance", _detailSampleDist, 0f, 16f, 0.1f);
            UniRcEditorHelpers.SliderFloat("Max Sample Error", _detailSampleMaxError, 0f, 16f, 0.1f);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Tiling");
            EditorGUILayout.Separator();
            UniRcEditorHelpers.SliderInt("Tile Size", _tileSize, 16, 1024, 16);
            
            // EditorGUILayout.LabelField($"Tiles {tiles[0]} x {tiles[1]}");
            // EditorGUILayout.LabelField($"Max Tiles {maxTiles}");
            // EditorGUILayout.LabelField($"Max Polys {maxPolys}");
            //}

            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();

            using (new EditorGUI.DisabledScope(Application.isPlaying))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth);

                if (GUILayout.Button("Clear"))
                {
                    Clear();
                    serializedObject.ApplyModifiedProperties();
                }

                if (GUILayout.Button("Bake"))
                {
                    Bake();
                }

                GUILayout.EndHorizontal();
            }
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active | GizmoType.Pickable)]
        static void RenderGizmoSelected(UniRcNavMeshSurface navSurface, GizmoType gizmoType)
        {
            var zTestOld = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

            //navSurface.navMeshDataInstance.FlagAsInSelectionHierarchy();
            DrawBoundingBoxGizmoAndIcon(navSurface, true);
            Handles.zTest = zTestOld;
        }

        [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
        static void RenderGizmoNotSelected(UniRcNavMeshSurface navSurface, GizmoType gizmoType)
        {
            var zTestOld = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

            DrawBoundingBoxGizmoAndIcon(navSurface, false);

            Handles.zTest = zTestOld;
        }

        static void DrawPoly(DtMeshTile tile, int index, Vector3 offset)
        {
            //Handles.color = new Color(64, 64, 64, 64);
            Handles.color = s_HandleColorSelected;
            var polygonVertices = new Vector3[3];

            DtPoly p = tile.data.polys[index];
            if (tile.data.detailMeshes != null)
            {
                DtPolyDetail pd = tile.data.detailMeshes[index];
                if (pd != null)
                {
                    for (int j = 0; j < pd.triCount; ++j)
                    {
                        int t = (pd.triBase + j) * 4;
                        for (int k = 0; k < 3; ++k)
                        {
                            int v = tile.data.detailTris[t + k];
                            if (v < p.vertCount)
                            {
                                polygonVertices[k] = new Vector3(
                                    -tile.data.verts[p.verts[v] * 3],
                                    tile.data.verts[p.verts[v] * 3 + 1],
                                    tile.data.verts[p.verts[v] * 3 + 2]);
                            }
                            else
                            {
                                polygonVertices[k] = new Vector3(
                                    -tile.data.detailVerts[(pd.vertBase + v - p.vertCount) * 3],
                                    tile.data.detailVerts[(pd.vertBase + v - p.vertCount) * 3 + 1],
                                    tile.data.detailVerts[(pd.vertBase + v - p.vertCount) * 3 + 2]);
                            }
                        }

                        Handles.DrawAAConvexPolygon(polygonVertices);
                    }
                }
            }
            else
            {
                for (int j = 1; j < p.vertCount - 1; ++j)
                {
                    var v0 = new Vector3(
                        -tile.data.verts[p.verts[0] * 3],
                        tile.data.verts[p.verts[0] * 3 + 1],
                        tile.data.verts[p.verts[0] * 3 + 2]
                    );
                    polygonVertices[0] = v0;

                    for (int k = 0; k < 2; ++k)
                    {
                        var vn = new Vector3(
                            -tile.data.verts[p.verts[j + k] * 3],
                            tile.data.verts[p.verts[j + k] * 3 + 1],
                            tile.data.verts[p.verts[j + k] * 3 + 2]
                        );
                        polygonVertices[k + 1] = vn;
                    }

                    Handles.DrawAAConvexPolygon(polygonVertices);
                }
            }
        }

        static void DrawBoundingBoxGizmoAndIcon(UniRcNavMeshSurface navSurface, bool selected)
        {
            // var color = selected ? s_HandleColorSelected : s_HandleColor;
            // if (!navSurface.enabled)
            //     color = s_HandleColorDisabled;

            var oldColor = Gizmos.color;
            var oldMatrix = Gizmos.matrix;

            // Use the unscaled matrix for the NavMeshSurface
            var localToWorld = Matrix4x4.TRS(navSurface.transform.position, navSurface.transform.rotation, Vector3.one);
            Gizmos.matrix = localToWorld;


            if (null == navSurface.NavMesh)
            {
                return;
            }

            var handleOldColor = Handles.color;
            // Handles의 Z 테스트 방식 설정

            // 폴리 그리기
            int count = navSurface.NavMesh.GetMaxTiles();
            for (int i = 0; i < count; ++i)
            {
                var tile = navSurface.NavMesh.GetTile(i);
                if (null == tile.data)
                    continue;

                for (int ii = 0; ii < tile.data.header.polyCount; ++ii)
                {
                    DtPoly p = tile.data.polys[ii];
                    if (p.GetPolyType() == DtPoly.DT_POLYTYPE_OFFMESH_CONNECTION)
                    {
                        continue;
                    }

                    DrawPoly(tile, ii, navSurface.transform.position);
                }
            }


            // 폴리곤 점들의 배열
            //Vector3[] polygonVertices = GetPolygonVertices(navSurface.transform.position);

            // 폴리곤 그리기
            //Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 폴리곤 색상
            //Handles.DrawAAConvexPolygon(polygonVertices);

            Handles.color = Color.black;
            // 검은색 큐브 그리기
            for (int i = 0; i < count; ++i)
            {
                var tile = navSurface.NavMesh.GetTile(i);
                if (null == tile.data)
                    continue;

                for (int ii = 0; ii < tile.data.header.vertCount; ++ii)
                {
                    int v = ii * 3;
                    var pt = new Vector3(-tile.data.verts[v + 0], tile.data.verts[v + 1], tile.data.verts[v + 2]);

                    Handles.DotHandleCap(
                        0,
                        pt + navSurface.transform.position,
                        Quaternion.identity,
                        HandleUtility.GetHandleSize(navSurface.transform.position + pt) * 0.015f,
                        EventType.Repaint
                    );
                }
            }

            Handles.color = handleOldColor;

            // if (navSurface.collectObjects == CollectObjects.Volume)
            // {
            //     Gizmos.color = color;
            //     Gizmos.DrawWireCube(navSurface.center, navSurface.size);
            //
            //     if (selected && navSurface.enabled)
            //     {
            //         var colorTrans = new Color(color.r * 0.75f, color.g * 0.75f, color.b * 0.75f, color.a * 0.15f);
            //         Gizmos.color = colorTrans;
            //         Gizmos.DrawCube(navSurface.center, navSurface.size);
            //     }
            // }
            // else
            // {
            //     if (navSurface.NavMesh != null)
            //     {
            //         var bounds = navSurface.NavMesh.sourceBounds;
            //         Gizmos.color = Color.grey;
            //         Gizmos.DrawWireCube(bounds.center, bounds.size);
            //     }
            // }

            Gizmos.matrix = oldMatrix;
            Gizmos.color = oldColor;

            //var startSize = HandleUtility.GetHandleSize(startPt);
            //Handles.CubeHandleCap(0, navSurface.transform.position + Vector3.up * 30, navSurface.transform.rotation, 10, EventType.Ignore);
            // Handles.DrawWireCube(navSurface.transform.position, Vector3.one * 100);
            // Handles.DrawDottedLine(navSurface.transform.position, navSurface.transform.position + Vector3.forward * 100, 1.0f);
            // Handles.Label(navSurface.transform.position, "Custom Label");

            Gizmos.DrawIcon(navSurface.transform.position, "NavMeshSurface Icon", true);
        }


        [MenuItem("GameObject/UniRecast/Unity Recast NavMesh Surface", false, 2000)]
        public static void CreateNavMeshSurface(MenuCommand menuCommand)
        {
            var parent = menuCommand.context as GameObject;
            // var go = NavMeshComponentsGUIUtility.CreateAndSelectGameObject("NavMesh Surface", parent);
            // go.AddComponent<NavMeshSurface>();
            // var view = SceneView.lastActiveSceneView;
            // if (view != null)
            //     view.MoveToView(go.transform);
        }
    }
}