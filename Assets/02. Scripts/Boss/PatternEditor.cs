using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pattern))]
public class DataEditor : Editor
{
    private Dictionary<TileSetData, bool[,]> checkboxStates = new Dictionary<TileSetData, bool[,]>(); // üũ�ڽ� ����
    private int selectedPlayerIndex = 0; // ���õ� TileSetData �ε���
    private Dictionary<TileSetData, bool> foldoutStates = new Dictionary<TileSetData, bool>(); // TileSetData ����/��ġ�� ����
    private Dictionary<TrailData, bool> trailFoldoutStates = new Dictionary<TrailData, bool>(); // TrailData ����/��ġ�� ����

    public override void OnInspectorGUI()
    {
        Pattern data = (Pattern)target;

        // TileSetData �߰� ��ư
        if (GUILayout.Button("Add New TileSetData"))
        {
            TileSetData newTileSet = new TileSetData();
            data.tilesets.Add(newTileSet);
            if (!checkboxStates.ContainsKey(newTileSet))
            {
                checkboxStates[newTileSet] = InitializeCheckboxState(newTileSet); // üũ�ڽ� ���� �ʱ�ȭ
            }
            if (!foldoutStates.ContainsKey(newTileSet))
                foldoutStates[newTileSet] = true; // �� TileSetData ������ ���·� �ʱ�ȭ
            selectedPlayerIndex = data.tilesets.Count - 1; // ���� �߰��� TileSetData ����
            EditorUtility.SetDirty(target); // ���� ���� ����
        }

        EditorGUILayout.Space();

        if (data.tilesets != null && data.tilesets.Count > 0)
        {
            // TileSetData ����Ʈ ��Ӵٿ� �޴�
            string[] tileSetOptions = new string[data.tilesets.Count];
            for (int i = 0; i < data.tilesets.Count; i++)
            {
                tileSetOptions[i] = $"TileSet {i + 1}";
            }

            selectedPlayerIndex = EditorGUILayout.Popup("Select TileSet", selectedPlayerIndex, tileSetOptions);
            TileSetData selectedTileSet = data.tilesets[selectedPlayerIndex];

            // TileSetData ����/��ġ�� ���� �ʱ�ȭ
            if (!foldoutStates.ContainsKey(selectedTileSet))
            {
                foldoutStates[selectedTileSet] = true; // �⺻��: ������ ����
            }

            foldoutStates[selectedTileSet] = EditorGUILayout.Foldout(foldoutStates[selectedTileSet], $"TileSet {selectedPlayerIndex + 1}");

            if (foldoutStates[selectedTileSet]) // TileSetData ������ ���
            {
                // TileSetData �Ӽ� ǥ��
                selectedTileSet.useTile = EditorGUILayout.Toggle("Use Tile", selectedTileSet.useTile);
                if (selectedTileSet.useTile)
                {
                    selectedTileSet.type = (TileType)EditorGUILayout.EnumPopup("TileType", selectedTileSet.type);
                    selectedTileSet.startupTime = EditorGUILayout.FloatField("Startup Time", selectedTileSet.startupTime);
                    selectedTileSet.holdingTime = EditorGUILayout.FloatField("Holding Time", selectedTileSet.holdingTime);

                    EditorGUILayout.Space();

                    // Ÿ�� üũ�ڽ� UI
                    EditorGUILayout.LabelField($"Tile Positions {selectedPlayerIndex + 1}", EditorStyles.boldLabel);

                    for (int y = 0; y < 5; y++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        for (int x = 0; x < 10; x++)
                        {
                            if (!checkboxStates.ContainsKey(selectedTileSet))
                            {
                                checkboxStates[selectedTileSet] = InitializeCheckboxState(selectedTileSet);
                            }

                            bool isChecked = checkboxStates[selectedTileSet][y, x];
                            bool newState = EditorGUILayout.Toggle(isChecked, GUILayout.Width(20));

                            if (newState != isChecked)
                            {
                                checkboxStates[selectedTileSet][y, x] = newState;

                                Vector2 coord = new Vector2(x, y);

                                if (newState)
                                {
                                    if (!selectedTileSet.tilePositions.Contains(coord))
                                    {
                                        selectedTileSet.tilePositions.Add(coord);
                                    }
                                }
                                else
                                {
                                    selectedTileSet.tilePositions.Remove(coord);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                selectedTileSet.useTrail = EditorGUILayout.Toggle("Use Trail", selectedTileSet.useTrail);
                if (selectedTileSet.useTrail)
                {
                    EditorGUILayout.LabelField("Trails", EditorStyles.boldLabel);

                    if (selectedTileSet.trails == null)
                        selectedTileSet.trails = new List<TrailData>();

                    // TrailData ����Ʈ ������
                    for (int i = 0; i < selectedTileSet.trails.Count; i++)
                    {
                        TrailData trail = selectedTileSet.trails[i];

                        // �� TrailData ����/��ġ�� ���� �ʱ�ȭ
                        if (!trailFoldoutStates.ContainsKey(trail))
                        {
                            trailFoldoutStates[trail] = true; // �⺻��: ������ ����
                        }

                        trailFoldoutStates[trail] = EditorGUILayout.Foldout(trailFoldoutStates[trail], $"Trail {i + 1}");

                        if (trailFoldoutStates[trail]) // TrailData ������ ���
                        {
                            EditorGUILayout.BeginVertical("box");
                            trail.isFast = EditorGUILayout.Toggle("Is Fast", trail.isFast);
                            trail.pos = EditorGUILayout.Vector2Field("Position", trail.pos);
                            trail.hv = (HV)EditorGUILayout.EnumPopup("HV Direction", trail.hv);
                            trail.speed = EditorGUILayout.FloatField("Speed", trail.speed);

                            // TrailData ���� ��ư
                            if (GUILayout.Button($"Remove Trail {i + 1}"))
                            {
                                selectedTileSet.trails.RemoveAt(i);
                                trailFoldoutStates.Remove(trail); // ������ TrailData�� ���� ����
                                break;
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }

                    // TrailData �߰� ��ư
                    if (GUILayout.Button("Add New Trail"))
                    {
                        TrailData newTrail = new TrailData();
                        selectedTileSet.trails.Add(newTrail);
                        trailFoldoutStates[newTrail] = true; // �� TrailData�� �⺻������ ������ ���·� �ʱ�ȭ
                    }
                }

                selectedTileSet.coolDownTime = EditorGUILayout.FloatField("Cooldown Time", selectedTileSet.coolDownTime);

                // TileSetData ���� ��ư
                if (GUILayout.Button($"Remove TileSet {selectedPlayerIndex + 1}"))
                {
                    checkboxStates.Remove(selectedTileSet);
                    foldoutStates.Remove(selectedTileSet);
                    data.tilesets.RemoveAt(selectedPlayerIndex);
                    selectedPlayerIndex = Mathf.Clamp(selectedPlayerIndex - 1, 0, data.tilesets.Count - 1);
                    EditorUtility.SetDirty(target); // ���� ���� ����
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("No TileSets Available in the List");
        }

        // ScriptableObject ������� ����
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private bool[,] InitializeCheckboxState(TileSetData tileSetData)
    {
        bool[,] state = new bool[5, 10];

        foreach (var position in tileSetData.tilePositions)
        {
            int x = Mathf.Clamp((int)position.x, 0, 9);
            int y = Mathf.Clamp((int)position.y, 0, 4);
            state[y, x] = true;
        }

        return state;
    }
}
