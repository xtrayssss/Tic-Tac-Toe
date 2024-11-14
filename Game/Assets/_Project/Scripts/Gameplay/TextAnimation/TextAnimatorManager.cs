using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Gameplay.TextAnimation
{
    public sealed class TextAnimatorManager : MonoBehaviour
    {
        private static TextAnimatorManager _instance;

        private readonly Dictionary<string, AnimatedTextData> _animatedTexts =
            new Dictionary<string, AnimatedTextData>();

        public static TextAnimatorManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<TextAnimatorManager>();
                if (_instance == null)
                {
                    var managerObject = new GameObject("TextAnimatorManager");
                    _instance = managerObject.AddComponent<TextAnimatorManager>();
                }

                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void AddTypewriter(Typewriter typewriter, TMP_Text textComponent, TMP_TextInfo textInfo)
        {
            if (typewriter == null || textComponent == null || textInfo == null)
            {
                Debug.LogWarning("TextAnimatorManager: Attempted to add typewriter with null components");
                return;
            }

            string instanceId = typewriter.gameObject.GetInstanceID().ToString();
            var manager = Instance;

            if (manager._animatedTexts.TryGetValue(instanceId, out AnimatedTextData existingData))
            {
                existingData.UpdateTypewriter(typewriter);
            }
            else
            {
                var newData = new AnimatedTextData(textComponent, textInfo, typewriter);
                manager._animatedTexts.Add(instanceId, newData);
            }
        }

        public static void ProcessAnimationsWithoutTime()
        {
            Instance.ProcessAllAnimations(false);
        }

        private void Update()
        {
            ProcessAllAnimations();
        }

        private void ProcessAllAnimations(bool processTime = true)
        {
            foreach (AnimatedTextData animatedText in _animatedTexts.Values)
            {
                if (!animatedText.Typewriter.IsStopped) 
                    ProcessSingleAnimation(animatedText, processTime);
            }
        }

        private void ProcessSingleAnimation(AnimatedTextData animatedText, bool processTime)
        {
            if (!IsAnimationValid(animatedText))
            {
                return;
            }

            animatedText.TextComponent.ForceMeshUpdate();

            if (animatedText.Typewriter != null && animatedText.TextInfo.characterCount > 0)
            {
                animatedText.Typewriter.ProcessAnimations(processTime);
                animatedText.TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
        }

        private bool IsAnimationValid(AnimatedTextData animatedText)
        {
            return animatedText != null &&
                   animatedText.TextComponent != null &&
                   animatedText.TextInfo != null;
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}