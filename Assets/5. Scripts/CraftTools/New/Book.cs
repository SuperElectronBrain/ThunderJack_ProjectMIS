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
        [TextArea] public string memo;
        public float perfection;
        public float eValue1;
        public float eValue2;
        public float eValue3;
        public ElementType eType1;
        public ElementType eType2;
        public ElementType eType3;
    }

    public class Book : MonoBehaviour
    {
        [SerializeField]
        public ElementCircles elementCircles;
        [SerializeField] private SkeletonAnimation skAni;

        [SerializeField] private bool isFirst;
        [SerializeField] private List<BookPageData> bookPages;
        [SerializeField] private int curPage;
        [SerializeField] private BookPage pageObject;
        [SerializeField] private GameObject bookPageArrow;

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
            pageObject.Init();
            bookPageArrow.SetActive(false);
            CraftTableCameraController.main.m_OnCompleteMove.AddListener(ActiveArrow);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                NextPage();
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                PrevPage();
            }
        }

        void ActiveArrow(string upDown)
        {
            if(upDown.Equals("Down"))
                bookPageArrow.SetActive(true);
            else
                bookPageArrow.SetActive(false);
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

            pageObject.SetPageInfo(bookPages[0].pageName, bookPages[0].pageDescription, bookPages[0].perfection,
                bookPages[0].pageImage, bookPages[0].memo);
            skAni.state.Complete += CompleteOpen;
            curPage = 0;
        }

        private void UpdateBook()
        {
            var items = GameManager.Instance.ItemManager.GetItemListByType(ItemType.Gem);
            //bookPages = new();
            
            for (int i = 0; i < items.Count; i++)
            {
                if (!PlayerPrefs.HasKey(items[i].itemNameEg + "_JewelryPerfection")) continue;

                bookPages[i].perfection = PlayerPrefs.GetFloat(items[i].itemNameEg + "_JewelryPerfection");
                bookPages[i].eType1 = (ElementType)PlayerPrefs.GetInt(items[i].itemNameEg + "_JewelryElement1");
                bookPages[i].eType2 = (ElementType)PlayerPrefs.GetInt(items[i].itemNameEg + "_JewelryElement2");
                bookPages[i].eType3 = (ElementType)PlayerPrefs.GetInt(items[i].itemNameEg + "_JewelryElement3");
                bookPages[i].eValue1 = PlayerPrefs.GetFloat(items[i].itemNameEg + "_JewelryElementValue1");
                bookPages[i].eValue2 = PlayerPrefs.GetFloat(items[i].itemNameEg + "_JewelryElementValue2");
                bookPages[i].eValue3 = PlayerPrefs.GetFloat(items[i].itemNameEg + "_JewelryElementValue3");
            }
        }

        public void NextPage()
        {
            if (curPage > bookPages.Count)
                return;
            
            bookPageArrow.SetActive(false);
            pageObject.GetComponent<FadeIO>().Rewind();
            skAni.AnimationName = "Forward_1";
            skAni.Initialize(true);
            skAni.state.Complete += TurnThePage;
            
            curPage++;
        }

        public void PrevPage()
        {
            if (curPage <= 0)
                return;
            
            bookPageArrow.SetActive(false);
            pageObject.GetComponent<FadeIO>().Rewind();
            skAni.AnimationName = "Backward_2";
            skAni.Initialize(true);
            skAni.state.Complete += TurnThePage;
            
            curPage--;
        }

        void TurnThePage(Spine.TrackEntry te)
        {
            pageObject.gameObject.SetActive(true);
            PageSetting();
            bookPageArrow.SetActive(true);
            skAni.state.Complete -= TurnThePage;
        }

        void PageSetting()
        {
            pageObject.SetPageInfo(bookPages[curPage].pageName, bookPages[curPage].pageDescription,
                bookPages[curPage].perfection,
                bookPages[curPage].pageImage, bookPages[curPage].memo);
            pageObject.SetElement(bookPages[curPage].eType1, bookPages[curPage].eType2,
                bookPages[curPage].eType3);
            pageObject.SetElementValues(bookPages[curPage].eValue1, bookPages[curPage].eValue2,
                bookPages[curPage].eValue3);
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