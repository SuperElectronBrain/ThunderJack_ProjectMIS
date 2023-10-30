using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BBB : MonoBehaviour
{
    Rigidbody2D rig;
    CircleCollider2D col;
    FadeIO fade;

    bool isInMillstone;

    private void OnMouseDown()
    {
        if (isInMillstone)
            return;
        transform.parent.GetComponent<AAA>().SelectObject(gameObject);
        Select();
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        fade = GetComponent<FadeIO>();
    }

    public void Select()
    {
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        col.isTrigger = true;
        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;
        rig.angularVelocity = 0;
    }

    public void ResetObj(Vector3 pos, Vector3 rot)
    {
        Select();

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
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;     
    }

    public void DestroyObject()
    {
        fade.onFadeEvent.AddListener(() =>
        {
            Destroy(transform.parent.gameObject);
        });

        fade.StartFade();
    }

    public void InMillstone()
    {
        if (col.isTrigger)
            return;

        isInMillstone = true;
        Destroy(this);
    }
}
