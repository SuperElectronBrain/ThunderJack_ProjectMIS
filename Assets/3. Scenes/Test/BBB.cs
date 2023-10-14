using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BBB : MonoBehaviour
{
    Rigidbody2D rig;
    CircleCollider2D col;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    public void ResetObj(Vector3 pos, Vector3 rot)
    {
        col.isTrigger = true;
        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;
        rig.angularVelocity = 0;

        Sequence sequence = DOTween.Sequence().SetRecyclable(true).SetAutoKill(false).Pause();

        sequence.Join(transform.DOLocalMove(pos, 0.2f))
            .Join(transform.DORotate(rot, 0.2f));

        sequence.Play();
    }

    public void ReleseObj()
    {
        rig.gravityScale = 3;
        col.isTrigger = false;
        var forceX = Random.Range(-2f, 2f);
        var forceY = Random.Range(-2f, 0f);
        rig.AddForce(new Vector2(forceX, forceY),ForceMode2D.Impulse);
    }
}
