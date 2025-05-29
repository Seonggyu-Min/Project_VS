using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Book : BaseSkill
//{
    //[SerializeField] private BaseSkill _bookPiecePrefab;

    //private List<BaseSkill> _spawnedPieces = new();

    //public override void InitSkillPosition()
    //{
    //    StartCoroutine(SpawnAndRotateRoutine());
    //}

    //private IEnumerator SpawnAndRotateRoutine()
    //{
    //    float angleStep = 360f / _projectileNumber;

    //    for (int i = 0; i < _projectileNumber; i++)
    //    {
    //        float angle = angleStep * i;

    //        BaseSkill piece = GameManager.Instance.SkillPoolManager.GetSkillInstance(_bookPiecePrefab);
    //        piece.SetPlayerReferences(_playerTransform, _playerMove);

    //        BookPiece book = piece as BookPiece;
    //        book.Init(_playerTransform, angle, _size, _projectileSpeed);

    //        _spawnedPieces.Add(book);
    //    }

    //    yield return new WaitForSeconds(_duration);

    //    foreach (var piece in _spawnedPieces)
    //    {
    //        GameManager.Instance.SkillPoolManager.ReturnSkillInstance(piece);
    //    }

    //    _spawnedPieces.Clear();
    //    GameManager.Instance.SkillPoolManager.ReturnSkillInstance(this); // BookSkill 자체도 반환
    //}
//}
