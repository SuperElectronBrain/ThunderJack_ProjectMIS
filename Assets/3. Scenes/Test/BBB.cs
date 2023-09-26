using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BBB : MonoBehaviour
{
    Rigidbody2D rig;
    PolygonCollider2D col;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<PolygonCollider2D>();
    }

    public void ResetObj(Vector3 pos, Vector3 rot)
    {
        col.isTrigger = true;
        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;        

        Sequence sequence = DOTween.Sequence().SetRecyclable(true).SetAutoKill(false).Pause();

        sequence.Join(transform.DOLocalMove(pos, 0.2f))
            .Join(transform.DORotate(rot, 0.2f));

        sequence.Play();
    }

    public void ReleseObj()
    {
        rig.gravityScale = 3;
        col.isTrigger = false;
    }
}
