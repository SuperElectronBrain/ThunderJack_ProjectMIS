using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RavenCraftCore
{
    public enum ElementType
    {
        Justice, Wisdom, Nature, Mystic, Insight, End
    }

    public class Book : MonoBehaviour
    {
        [SerializeField]
        public ElementCircles elementCircles;

        // Start is called before the first frame update
        void Start()
        {
            elementCircles.Init();
        }

        public void UpdateElementCircle(ElementType elementType, float updateValue)
        {
            elementCircles.UpdateElementCircle(elementType, updateValue);
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
            private readonly int power2 = Shader.PropertyToID("_Power2");
            private readonly int power3 = Shader.PropertyToID("_Power3");
            private readonly int power4 = Shader.PropertyToID("_Power4");
            private readonly int power5 = Shader.PropertyToID("_Power5");

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
                    }
                }
                
                public void PlayParticle(int lv)
                {
                    if(lv > 0)
                        particles[lv].Stop();
                    particles[lv].Play();
                }
            }
        }
    }
}