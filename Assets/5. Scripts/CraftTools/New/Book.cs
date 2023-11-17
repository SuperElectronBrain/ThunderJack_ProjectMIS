using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace RavenCraftCore
{
    public enum ElementType
    {
        Justice, Wisdom, Nature, Mystic, Insight, End
    }

    [Serializable]
    public class BookPageData
    {
        public string pageName;
        public Sprite pageImage;
        public string pageDescription;
        public int page;
        public float perfection;
        public JewelryRank JewelryRank;
    }

    public class Book : MonoBehaviour
    {
        [SerializeField]
        public ElementCircles elementCircles;
        [SerializeField] private SkeletonAnimation skAni;

        [SerializeField] private bool isFirst;
        [SerializeField] private List<BookPageData> bookPages;
        [SerializeField] private int curPage;
        [SerializeField] private string curPageName;
        [SerializeField] private BookPage pageObject;

        void OnBecameVisible()
        {
            if (isFirst)
                return;

            isFirst = true;
            skAni.AnimationName = "Open";
            
            OpenBook();
        }

        void CompleteOpen(Spine.TrackEntry tr)
        {
            pageObject.gameObject.SetActive(true);
            
            skAni.state.Complete -= CompleteOpen;
        }

        private void Awake()
        {
            skAni = GetComponentInChildren<SkeletonAnimation>();
        }

        // Start is called before the first frame update
        void Start()
        {
            elementCircles.Init();
            EventManager.Subscribe(EventType.CreateComplete, ResetBook);
            EventManager.Subscribe(EventType.CreateComplete, UpdateBook);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                OpenBook();
            }
        }

        void ResetBook()
        {
            elementCircles.Init();
        }

        public void UpdateElementCircle(ElementType elementType, float updateValue)
        {
            elementCircles.UpdateElementCircle(elementType, updateValue);
        }
        
        /*Todo
         *�����Ǻ�
         * ���۽� ����
         * å���� �ε�
         * 1. �����Ǻ� ���� �����۹�ȣ���� or ���ۼ���
         */

        private void OpenBook()
        {
            UpdateBook();

            if (bookPages.Count == 0)
                return;

            pageObject.SetPageInfo("김영훈", "역겹다", 0, bookPages[0].pageImage);
            pageObject.SetElementValues(100f, 72f, 32f);
            skAni.state.Complete += CompleteOpen;
            curPage = 0;
            curPageName = bookPages[0].pageName;
        }

        private void UpdateBook()
        {
            var items = GameManager.Instance.ItemManager.GetItemListByType(ItemType.Gem);
            bookPages = new();
            var page = 0;
            
            for (int i = 0; i < items.Count; i++)
            {
                if (!PlayerPrefs.HasKey(items[i].itemNameEg + "_JewelryPerfection")) continue;
                bookPages.Add(
                    new BookPageData
                    {
                        pageName = items[i].itemNameEg,
                        page = page,
                        JewelryRank = (JewelryRank)PlayerPrefs.GetInt(items[i].itemNameEg + "_JewelryRank"),
                        perfection = PlayerPrefs.GetFloat(items[i].itemNameEg + "_JewelryPerfection")
                    }
                );
                page++;
            }

            for (int i = 0; i < bookPages.Count; i++)
            {
                if (bookPages[i].pageName == curPageName)
                {
                    curPage = i + 1;
                }
            }
        }

        public void NextPage()
        {
            if (curPage >= bookPages.Count)
                return;
            
            curPage++;
            curPageName = bookPages[curPage + 1].pageName;
        }

        public void PrevPage()
        {
            if (curPage <= 0)
                return;
            
            curPage--;
            curPageName = bookPages[curPage - 1].pageName;
        }

        [System.Serializable]
        public class ElementCircles
        {
            [SerializeField]
            private Material magicCircleMaterial;
            public float[] upgradeValue;
            public int[] elementCircleLv;
            public float[] elementCircleValue;
            public ParticleObject[] elementCircleParticles;
            
            private readonly int power1 = Shader.PropertyToID("_Power1");
            private readonly int power2 = Shader.PropertyToID("_Power3");
            private readonly int power3 = Shader.PropertyToID("_Power5");
            private readonly int power4 = Shader.PropertyToID("_Power4");
            private readonly int power5 = Shader.PropertyToID("_Power2");

            public void Init()
            {
                for (int i = 0; i < ((int)ElementType.End); i++)
                {
                    elementCircleLv[i] = 0;
                    elementCircleValue[i] = 0;
                    elementCircleParticles[i].Init();
                }
            }

            public void UpdateElementCircle(ElementType elementType, float updateValue)
            {
                if (elementCircleLv[((int)elementType)] > 4)
                    return;

                var circlePowerValue = Mathf.Lerp(2, 35, updateValue / 100);
                
                switch (elementType)
                {
                    case ElementType.Justice:
                        magicCircleMaterial.SetFloat(power1, circlePowerValue);
                        break;
                    case ElementType.Wisdom:
                        magicCircleMaterial.SetFloat(power2, circlePowerValue);
                        break;
                    case ElementType.Nature:
                        magicCircleMaterial.SetFloat(power3, circlePowerValue);
                        break;
                    case ElementType.Mystic:
                        magicCircleMaterial.SetFloat(power4, circlePowerValue);
                        break;
                    case ElementType.Insight:
                        magicCircleMaterial.SetFloat(power5, circlePowerValue);
                        break;
                }
                
                elementCircleValue[(int)elementType] = updateValue;
                var elementLv = elementCircleLv[((int)elementType)];

                if (elementCircleValue[(int)elementType] < upgradeValue[elementLv])
                    return;
                
                ActiveElementCircle(elementType, elementLv);
                elementCircleLv[((int)elementType)]++;
            }

            public void ActiveElementCircle(ElementType elementType, int circleLv)
            {
                elementCircleParticles[((int)elementType)].PlayParticle(circleLv);                
            }

            [System.Serializable]
            public class ParticleObject
            {
                [SerializeField]
                private GameObject particleParent;
                [SerializeField]
                private ParticleSystem[] particles;

                public void Init()
                {
                    var childCount = particleParent.transform.childCount;

                    particles = new ParticleSystem[childCount];

                    for (int i = 0; i < childCount; i++)
                    {
                        particles[i] = particleParent.transform.GetChild(i).GetComponent<ParticleSystem>();
                        particles[i].Stop();
                    }
                }
                
                public void PlayParticle(int lv)
                {
                    if (lv > 0)
                        particles[lv].gameObject.SetActive(false);
                    particles[lv].gameObject.SetActive(true);
                }
            }
        }
    }
}