using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;


[CustomEditor(typeof(Pattern))]
public class DataEditor : Editor
{
    private Dictionary<TileSetData, bool[,]> checkboxStates = new Dictionary<TileSetData, bool[,]>(); // �� PlayerClass�� üũ�ڽ� ����
    private int selectedPlayerIndex = 0; // ���� ���õ� PlayerClass�� �ε���

    public override void OnInspectorGUI()
    {
        Pattern data = (Pattern)target;

        // PlayerClass �߰� ��ư
        if (GUILayout.Button("Add New TileSetData"))
        {
            TileSetData newPlayer = new TileSetData();
            data.tilesets.Add(newPlayer);
            checkboxStates[newPlayer] = new bool[5, 10]; // ���ο� PlayerClass�� ���� üũ�ڽ� ���� �ʱ�ȭ
            selectedPlayerIndex = data.tilesets.Count - 1; // ���� �߰��� PlayerClass ����
            EditorUtility.SetDirty(target); // ���� ���� ����
        }

        EditorGUILayout.Space();

        if (data.tilesets != null && data.tilesets.Count > 0)
        {
            // PlayerClass ����Ʈ ��Ӵٿ� �޴�
            string[] playerOptions = new string[data.tilesets.Count];
            for (int i = 0; i < data.tilesets.Count; i++)
            {
                playerOptions[i] = $"Tile_ {i + 1}";
            }

            selectedPlayerIndex = EditorGUILayout.Popup("Select TileSet", selectedPlayerIndex, playerOptions);
            TileSetData selectedPlayer = data.tilesets[selectedPlayerIndex];

            // ���õ� PlayerClass�� üũ�ڽ� ���� �ʱ�ȭ
            if (!checkboxStates.ContainsKey(selectedPlayer))
            {
                checkboxStates[selectedPlayer] = new bool[5, 10];

                // ���� tilePositions �����͸� üũ�ڽ� ���¿� ����ȭ
                foreach (var pos in selectedPlayer.tilePositions)
                {
                    if (pos.x >= 0 && pos.x < 10 && pos.y >= 0 && pos.y < 5)
                    {
                        checkboxStates[selectedPlayer][(int)pos.y, (int)pos.x] = true;
                    }
                }
            }

            // PlayerClass�� �ٸ� ������ ���� UI
            //EditorGUILayout.LabelField($"Tile {selectedPlayerIndex + 1} Properties", EditorStyles.boldLabel);
            selectedPlayer.useTile = EditorGUILayout.Toggle("Use Tile", selectedPlayer.useTile);
            if (selectedPlayer.useTile)
            {
                selectedPlayer.type = (TileType)EditorGUILayout.EnumPopup("TileType", selectedPlayer.type);
                selectedPlayer.startupTime = EditorGUILayout.FloatField("Startup Time", selectedPlayer.startupTime);
                selectedPlayer.holdingTime = EditorGUILayout.IntField("Holding Time", selectedPlayer.holdingTime);

                EditorGUILayout.Space();

                // Ÿ�� üũ�ڽ� UI
                EditorGUILayout.LabelField($"Tile Positions {selectedPlayerIndex + 1}", EditorStyles.boldLabel);

                for (int y = 0; y < 5; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < 10; x++)
                    {
                        bool isChecked = checkboxStates[selectedPlayer][y, x];
                        bool newState = EditorGUILayout.Toggle(isChecked, GUILayout.Width(20));

                        if (newState != isChecked)
                        {
                            checkboxStates[selectedPlayer][y, x] = newState;

                            Vector2 coord = new Vector2(x, y);

                            if (newState)
                            {
                                if (!selectedPlayer.tilePositions.Contains(coord))
                                {
                                    selectedPlayer.tilePositions.Add(coord);
                                }
                            }
                            else
                            {
                                selectedPlayer.tilePositions.Remove(coord);
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }


            selectedPlayer.useTrail = EditorGUILayout.Toggle("Use Trail", selectedPlayer.useTrail);
            if (selectedPlayer.useTrail)
            {

            }

            selectedPlayer.coolDownTime = EditorGUILayout.FloatField("Cooldown Time", selectedPlayer.coolDownTime);
            // PlayerClass ���� ��ư
            if (GUILayout.Button($"Remove Player {selectedPlayerIndex + 1}"))
            {
                checkboxStates.Remove(selectedPlayer);
                data.tilesets.RemoveAt(selectedPlayerIndex);
                selectedPlayerIndex = Mathf.Clamp(selectedPlayerIndex - 1, 0, data.tilesets.Count - 1);
                EditorUtility.SetDirty(target); // ���� ���� ����
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Players Available in the List");
        }

        // ScriptableObject ������� ����
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}