using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Elements.ScrollSnap
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollSnap : MonoBehaviour
    {
        [FormerlySerializedAs("scrollbar")]
        [Header("Scroll Settings")] [SerializeField]
        private Scrollbar _scrollbar;

        [SerializeField] [Range(0, 1)] private float _initialPos;
        public float SmoothSnapDuration = 0.25f;

        [Header("Snap Settings")]
        [SerializeField]
        private float _snapDelayDuration = 0.15f;

        [SerializeField] private float _snapDistanceThreshold = 0.001f;

        protected ScrollRect ScrollRect;
        private float _scrollPos;
        protected List<ScrollSnapElement> Elements;
        private float _distance;
        private int _itemCount;
        private Coroutine _smoothScrollingCoroutine;
        private Coroutine _snapToNearestCoroutine;
        private bool _smoothScrolling;
        private int _selectedItemIndex;
        private bool _snapping;
        private bool Snapped => Mathf.Abs(_nearestPos - _scrollbar.value) <= _snapDistanceThreshold;
        private int _nearestIndex;
        private float _nearestPos;
        protected bool HasItem => Elements != null && Elements.Count > 0;

        protected RectTransform Content => ScrollRect.content;
        protected virtual void Start() =>
            Setup();

        protected virtual void Update() =>
            UpdateAll();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
                OnInitialPosChanged();
        }
#endif

        private void Setup()
        {
            ScrollRect = GetComponent<ScrollRect>();
            SetupItems();
        }

        private void SetupItems()
        {
            _itemCount = Content.childCount;
            Elements = new List<ScrollSnapElement>(_itemCount);

            _distance = _itemCount > 1 ? 1f / (_itemCount - 1f) : 1;
            for (int i = 0; i < _itemCount; i++)
            {
                ScrollSnapElement element = Content.GetChild(i).GetComponent<ScrollSnapElement>();
                
                Elements.Add(element);
                
                element.Construct(_distance * i);
            }
        }

        private void OnInitialPosChanged()
        {
            if (_scrollbar != null && !Mathf.Approximately(_scrollbar.value, _initialPos))
                ScrollTo(_initialPos);
        }

        private void UpdateNearest()
        {
            int nearest = GetNearestIndex();
            
            if (nearest != -1)
                _nearestIndex = nearest;

            _nearestPos = Elements[_nearestIndex].Position;
        }

        private void UpdateAll()
        {
            UpdateItemsIfChanged();

            if (!HasItem)
                return;

            _scrollPos = _scrollbar.value;
            UpdateNearest();
            if (Input.GetMouseButton(0))
            {
                ClearSmoothScrolling();
                _snapping = false;
            }
            else if (!_smoothScrolling && !_snapping && !Snapped)
                SnapToNearest();
        }

        private void UpdateItemsIfChanged()
        {
            int childCount = Content.childCount;
            bool childCountChanged = _itemCount != childCount;
            bool contentChanged = childCountChanged;
            
            if (!childCountChanged && HasItem)
            {
                for (int i = 0; i < _itemCount; i++)
                {
                    RectTransform item = Elements[i].RectTransform;
                    Transform child = Content.GetChild(i);
                    
                    if (item != child)
                    {
                        contentChanged = true;
                        break;
                    }
                }
            }

            if (contentChanged) 
                SetupItems();
        }

        private IEnumerator SnapToNearestCoroutine()
        {
            yield return new WaitForSecondsRealtime(_snapDelayDuration);
            SmoothScrollTo(_nearestPos, SmoothSnapDuration);
        }

        private void SnapToNearest()
        {
            _snapping = true;
            if (_snapToNearestCoroutine != null)
                StopCoroutine(_snapToNearestCoroutine);

            _snapToNearestCoroutine = StartCoroutine(SnapToNearestCoroutine());
        }

        private int GetNearestIndex()
        {
            if (Elements.Count <= 1)
                return 0;

            for (int i = 0; i < _itemCount; i++)
            {
                ScrollSnapElement element = Elements[i];
                
                if (Math.Abs(_scrollPos - element.Position) <= _distance / 2)
                    return i;
            }

            return -1;
        }

        private IEnumerator SmoothScroll(float ratio, float seconds)
        {
            _smoothScrolling = true;
            ratio = Mathf.Clamp01(ratio);

            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime / seconds;
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, ratio, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }

            _smoothScrolling = false;

            OnSmoothScrollEnded();
        }

        private void OnSmoothScrollEnded()
        {
            _snapping = false;
        }

        private void ClearSnapping()
        {
            _snapping = false;
            if (_snapToNearestCoroutine != null)
                StopCoroutine(_snapToNearestCoroutine);
        }

        private void ScrollTo(float ratio)
        {
            ClearSmoothScrolling();
            ClearSnapping();
            _scrollbar.value = Mathf.Clamp01(ratio);
        }

        private void SmoothScrollTo(float ratio, float seconds)
        {
            ClearSmoothScrolling();
            ClearSnapping();

            _smoothScrollingCoroutine = StartCoroutine(SmoothScroll(ratio, seconds));
        }


        private void ClearSmoothScrolling()
        {
            _smoothScrolling = false;
            if (_smoothScrollingCoroutine != null)
                StopCoroutine(_smoothScrollingCoroutine);
        }
    }
}