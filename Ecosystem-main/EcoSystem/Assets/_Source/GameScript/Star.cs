using System.Collections.Generic;
using _Source.GameScript.Interfaces;
using UnityEngine;

namespace _Source.GameScript
{
    public class Star : MonoBehaviour, IObserver
    {
        [SerializeField] private List<SpriteRenderer> stars;
        [SerializeField] private float fadeSpeed = 1f;

        private float _targetAlpha = 0f;
        private MaterialPropertyBlock _materialPropertyBlock;

        private void Start()
        {
            _materialPropertyBlock = new MaterialPropertyBlock(); 
            SetStarsAlpha(0f); 
        }

        public void UpdateTime(float time)
        {
            if (time >= 0f && time < 0.25f) 
            {
                _targetAlpha = 1f;
            }
            else if (time >= 0.25f && time < 0.5f) 
            {
                _targetAlpha = 0f; 
            }
            else if (time >= 0.5f && time < 0.75f) 
            {
                _targetAlpha = 0f; 
            }
            else if (time >= 0.75f && time <= 1f) 
            {
                _targetAlpha = Mathf.Lerp(0f, 1f, (time - 0.75f) * 4f); 
            }

            StopAllCoroutines();
            StartCoroutine(FadeStars());
        }

        private System.Collections.IEnumerator FadeStars()
        {
            float currentAlpha = stars[0].color.a;
            
            while (!Mathf.Approximately(currentAlpha, _targetAlpha))
            {
                currentAlpha = Mathf.MoveTowards(currentAlpha, _targetAlpha, fadeSpeed * Time.deltaTime);
                SetStarsAlpha(currentAlpha);
                yield return null;
            }
        }

        private void SetStarsAlpha(float alpha)
        {
            foreach (var star in stars)
            {
                star.GetPropertyBlock(_materialPropertyBlock);
                
                _materialPropertyBlock.SetColor("_Color", new Color(star.color.r, star.color.g, star.color.b, alpha));
                
                star.SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }
}