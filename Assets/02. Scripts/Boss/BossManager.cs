using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    [SerializeField] private List<Pattern> patterns;
    [SerializeField] private int pageNumber;

    protected override void Awake()
    {
        StartPattern(patterns[0]);
    }

    // 패턴(페이지) 시작 메서드 
    public void StartPattern(Pattern pattern)
    {
        // 패턴 읽기 시작 
        StartCoroutine(ReadTileSets(pattern.tilesets));
    }

    // 패턴 읽고 실행 메서드 
    private IEnumerator ReadTileSets(List<TileSetData> tilesets)
    {
        foreach (TileSetData tileset in tilesets)
        {
            // 쿨다운 대기 
            yield return new WaitForSeconds(tileset.coolDownTime);

            // 기차 출현
            if (tileset.useTrail)
            {
                // TODO - 기차 코드 작성
            }

            // 타일 상태 변환
            if (tileset.useTile)
            {
                TileManager.Instance.SetTileType(tileset.tilePositions, tileset.type, tileset.startupTime, tileset.holdingTime);
            }
        }
    }
}
