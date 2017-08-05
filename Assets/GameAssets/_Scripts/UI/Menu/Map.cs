using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Transform _miniSimon;
    [SerializeField]
    private Transform[] _ways;
    [SerializeField]
    private float _movementTimePerLevel;

    private static int _fromLevel;
    private static int _toLevel;

    private void Awake()
    {
        DOTween.Init();
    }

    private IEnumerator Start()
    {
        if (_toLevel < _fromLevel)
        {
            throw new System.Exception("_fromLevel debe ser >= que _toLevel");
        }

        if (_toLevel == 1 && _fromLevel == 1)
        {
            _miniSimon.position = _ways[0].position;
            yield return null;
        }

        _miniSimon.position = _ways[_toLevel - 1].position;
        yield return new WaitForSeconds(1.5f);

        Sequence path = DOTween.Sequence();

        Vector2[] wayPoints = GetPath(_toLevel - 1);

        foreach (Vector2 point in wayPoints)
        {
            path.Append(_miniSimon.DOMove(point, _movementTimePerLevel));
        }

        path.Append(_miniSimon.DOMove(_ways[_toLevel].position, _movementTimePerLevel));

        path.SetLoops(-1, LoopType.Restart);

        path.Play();

        Invoke("LoadLevel", 10);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("Level" + (_toLevel + 1), LoadSceneMode.Single);
    }

    public static void ShowMap(int fromLevel, int toLevel)
    {
        Map._fromLevel = fromLevel;
        Map._toLevel = toLevel;

        SceneManager.LoadScene("Map", LoadSceneMode.Single);
    }

    private Vector2[] GetPath(int level)
    {
        Transform levelWays = _ways[level];
        Vector2[] list = new Vector2[levelWays.childCount];
        for (int i = 0; i < levelWays.childCount; i++)
        {
            Transform child = levelWays.GetChild(i);
            list[i] = child.transform.position;
        }

        return list;
    }
}
