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
            public float[] upgradeValue;
            public int[] elementCircleLv;
            public float[] elementCircleValue;
            public ParticleObject[] elementCircleParticles;

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
                GameObject particleParent;
                [SerializeField]
                ParticleSystem[] particles;

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
                    Debug.Log(particles[lv].name);
                    particles[lv].Play();
                }
            }
        }
    }
}