using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    [SerializeField] private List<Pattern> patterns;
    [SerializeField] private int pageNumber;

    [SerializeField] private GameObject leftThomasPrefab;
    [SerializeField] private GameObject rightThomasPrefab;
    [SerializeField] private GameObject upThomasPrefab;
    [SerializeField] private GameObject downThomasPrefab;

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
                for (int i = 0; i < tileset.trails.Count; i++)
                {
                    CreateThomas(tileset.trails[i]);
                }
            }

            // 타일 상태 변환
            if (tileset.useTile)
            {
                TileManager.Instance.SetTileType(tileset.tilePositions, tileset.type, tileset.startupTime, tileset.holdingTime);
            }
        }
    }

    // 광차 생성 메서드 
    public void CreateThomas(TrailData traildata)
    {
        Trail trail = null;
        switch (traildata.hv)
        {
            case HV.LEFT:
                Instantiate(leftThomasPrefab, new Vector3(traildata.pos.x, traildata.pos.y, 0), Quaternion.identity).TryGetComponent(out trail);
                break;
            case HV.RIGHT:
                Instantiate(rightThomasPrefab, new Vector3(traildata.pos.x, traildata.pos.y, 0), Quaternion.identity).TryGetComponent(out trail);
                break;
            case HV.UP:
                Instantiate(upThomasPrefab, new Vector3(traildata.pos.x, traildata.pos.y, 0), Quaternion.identity).TryGetComponent(out trail);
                break;
            case HV.DOWN:
                Instantiate(downThomasPrefab, new Vector3(traildata.pos.x, traildata.pos.y, 0), Quaternion.identity).TryGetComponent(out trail);
                break;
            default:
                break;
        }
        trail?.Shot(traildata.speed);
    }
}
