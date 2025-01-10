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

    // ����(������) ���� �޼��� 
    public void StartPattern(Pattern pattern)
    {
        // ���� �б� ���� 
        StartCoroutine(ReadTileSets(pattern.tilesets));
    }

    // ���� �а� ���� �޼��� 
    private IEnumerator ReadTileSets(List<TileSetData> tilesets)
    {
        foreach (TileSetData tileset in tilesets)
        {
            // ��ٿ� ��� 
            yield return new WaitForSeconds(tileset.coolDownTime);

            // ���� ����
            if (tileset.useTrail)
            {
                for (int i = 0; i < tileset.trails.Count; i++)
                {
                    CreateThomas(tileset.trails[i]);
                }
            }

            // Ÿ�� ���� ��ȯ
            if (tileset.useTile)
            {
                TileManager.Instance.SetTileType(tileset.tilePositions, tileset.type, tileset.startupTime, tileset.holdingTime);
            }
        }
    }

    // ���� ���� �޼��� 
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
