using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;


[CustomEditor(typeof(Pattern))]
public class DataEditor : Editor
{
    private Dictionary<TileSetData, bool[,]> checkboxStates = new Dictionary<TileSetData, bool[,]>(); // 각 PlayerClass별 체크박스 상태
    private int selectedPlayerIndex = 0; // 현재 선택된 PlayerClass의 인덱스

    public override void OnInspectorGUI()
    {
        Pattern data = (Pattern)target;

        // PlayerClass 추가 버튼
        if (GUILayout.Button("Add New TileSetData"))
        {
            TileSetData newPlayer = new TileSetData();
            data.tilesets.Add(newPlayer);
            checkboxStates[newPlayer] = new bool[5, 10]; // 새로운 PlayerClass에 대한 체크박스 상태 초기화
            selectedPlayerIndex = data.tilesets.Count - 1; // 새로 추가된 PlayerClass 선택
            EditorUtility.SetDirty(target); // 변경 사항 저장
        }

        EditorGUILayout.Space();

        if (data.tilesets != null && data.tilesets.Count > 0)
        {
            // PlayerClass 리스트 드롭다운 메뉴
            string[] playerOptions = new string[data.tilesets.Count];
            for (int i = 0; i < data.tilesets.Count; i++)
            {
                playerOptions[i] = $"Tile_ {i + 1}";
            }

            selectedPlayerIndex = EditorGUILayout.Popup("Select TileSet", selectedPlayerIndex, playerOptions);
            TileSetData selectedPlayer = data.tilesets[selectedPlayerIndex];

            // 선택된 PlayerClass의 체크박스 상태 초기화
            if (!checkboxStates.ContainsKey(selectedPlayer))
            {
                checkboxStates[selectedPlayer] = new bool[5, 10];

                // 기존 tilePositions 데이터를 체크박스 상태와 동기화
                foreach (var pos in selectedPlayer.tilePositions)
                {
                    if (pos.x >= 0 && pos.x < 10 && pos.y >= 0 && pos.y < 5)
                    {
                        checkboxStates[selectedPlayer][(int)pos.y, (int)pos.x] = true;
                    }
                }
            }

            // PlayerClass의 다른 변수들 편집 UI
            //EditorGUILayout.LabelField($"Tile {selectedPlayerIndex + 1} Properties", EditorStyles.boldLabel);
            selectedPlayer.useTile = EditorGUILayout.Toggle("Use Tile", selectedPlayer.useTile);
            if (selectedPlayer.useTile)
            {
                selectedPlayer.type = (TileType)EditorGUILayout.EnumPopup("TileType", selectedPlayer.type);
                selectedPlayer.startupTime = EditorGUILayout.FloatField("Startup Time", selectedPlayer.startupTime);
                selectedPlayer.holdingTime = EditorGUILayout.IntField("Holding Time", selectedPlayer.holdingTime);

                EditorGUILayout.Space();

                // 타일 체크박스 UI
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
            // PlayerClass 삭제 버튼
            if (GUILayout.Button($"Remove Player {selectedPlayerIndex + 1}"))
            {
                checkboxStates.Remove(selectedPlayer);
                data.tilesets.RemoveAt(selectedPlayerIndex);
                selectedPlayerIndex = Mathf.Clamp(selectedPlayerIndex - 1, 0, data.tilesets.Count - 1);
                EditorUtility.SetDirty(target); // 변경 사항 저장
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Players Available in the List");
        }

        // ScriptableObject 변경사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}