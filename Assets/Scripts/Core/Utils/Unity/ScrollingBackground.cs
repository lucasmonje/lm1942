using UnityEngine;

namespace Core.Utils.Unity
{
    public class ScrollingBackground : MonoBehaviour
    {
        public float ScrollSpeed = 300;
        private float tileSize;

        private Vector3 startPosition;
        private RectTransform rectTransform;
        private float time;
        private bool move;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            startPosition = rectTransform.anchoredPosition;
            tileSize = rectTransform.anchoredPosition.y * 2;
        }

        public void Reset()
        {
            time = 0f;
        }

        public void StartMovement()
        {
            move = true;
        }

        public void StopMovement()
        {
            move = false;
        }
        
        private void Update()
        {
            if (!move) return;
            time += Time.deltaTime;
            var newPosition = Mathf.Repeat(time * ScrollSpeed, tileSize);
            rectTransform.anchoredPosition = startPosition + Vector3.down * newPosition;
        }
    }
}