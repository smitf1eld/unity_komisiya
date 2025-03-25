using _Source.GameScript.Interfaces;
using UnityEngine;

namespace _Source.GameScript
{
    public class Sky : MonoBehaviour, IObserver
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Color nightColor, morningColor, dayColor, eveningColor;
        
        private void Start()
        {
            FindObjectOfType<DayNightCycleManager>()?.RegisterObserver(this);
        }
        
        public void UpdateTime(float timeOfDay)
        {
            if (timeOfDay < 0.25f)
            {
                mainCamera.backgroundColor = Color.Lerp(nightColor, morningColor, timeOfDay * 4);
            }
            else if (timeOfDay < 0.5f)
            {
                mainCamera.backgroundColor = Color.Lerp(morningColor, dayColor, (timeOfDay - 0.25f) * 4);
            }
            else if (timeOfDay < 0.75f)
            {
                mainCamera.backgroundColor = Color.Lerp(dayColor, eveningColor, (timeOfDay - 0.5f) * 4);
            }
            else
            {
                mainCamera.backgroundColor = Color.Lerp(eveningColor, nightColor, (timeOfDay - 0.75f) * 4);
            }
        }
    }
}