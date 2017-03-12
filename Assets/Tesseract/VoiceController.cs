using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Analog.Platform.ShellPrototyping.Common
{
    /// <summary>
    /// Controls demo via voice or keyboard keys.
    /// Any object can register Actions that should run as a result of different keywords
    /// DEMO CONTROLLER MUST BE BEFORE ALL THINGS IT CONTROLS!!
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class VoiceController : Singleton<VoiceController>
    {
        public int RecentCommandSize = 5;

        private KeywordRecognizer keywordRecognizer;
        private Dictionary<string, List<Action>> m_keywords = new Dictionary<string, List<Action>>();
        private Dictionary<KeyCode, List<Action>> m_keys = new Dictionary<KeyCode, List<Action>>();
        private AudioSource m_audioSource;

        /// <summary>
        /// Instantiate this object on Awake instead of Start to ensure that the instance is created
        /// before other game objects try to use it.
        /// </summary>
        void Awake()
        {
            Instance = this;
            m_audioSource = GetComponent<AudioSource>();
        }

        public void AddKeyword(string keyword, KeyCode key, Action action)
        {
            AddKeyword(key, action);
            AddKeyword(keyword, action);
        }


        public void AddKeyword(KeyCode key, Action action)
        {
            List<Action> keyActions;
            if (!m_keys.TryGetValue(key, out keyActions))
            {
                keyActions = new List<Action>();
                m_keys[key] = keyActions;
            }
            keyActions.Add(action);
        }

        public void AddKeyword(string keyword, Action action)
        {
            keyword = keyword.ToLower();
            List<Action> keywordActions;
            if (!m_keywords.TryGetValue(keyword, out keywordActions))
            {
                keywordActions = new List<Action>();
                m_keywords[keyword] = keywordActions;
            }
            keywordActions.Add(action);
        }


        public bool HasKeyword(string keyword)
        {
            return m_keywords.ContainsKey(keyword.ToLower());
        }


        public void DoActionByString(String s)
        {
            List<Action> keywordActions;
            if (m_keywords.TryGetValue(s.ToLower(), out keywordActions))
            {
                DoActions(keywordActions);

            }
        }

        public void vc(String s)
        {
            DoActionByString(s);
        }

        void Update()
        {
            if (keywordRecognizer == null)
            {
                // Tell the KeywordRecognizer about our keywords.
                keywordRecognizer = new KeywordRecognizer(m_keywords.Keys.ToArray());
                // Register a callback for the KeywordRecognizer and start recognizing!
                keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
                keywordRecognizer.Start();
            }
            foreach (KeyCode s in m_keys.Keys)
            {
                // Inefficient; Change algorithm if we end up having many keywords
                if (UnityEngine.Input.GetKeyDown(s))
                {
                    DoActions(m_keys[s]);
                }
            }
        }

        public void PrintVoiceCommands()
        {
            var sb = new StringBuilder();
            foreach (var keyword in m_keywords)
            {
                sb.Append(string.Format("\t {0}\n", keyword.Key));
            }
            Debug.Log("Keywords:\n" + sb);
        }


        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            DoActionByString(args.text);
        }

        private void DoActions(List<Action> actions)
        {
            PlayFeedback();
            foreach (var action in actions)
            {
                action.Invoke();
            }
        }

        private void PlayFeedback()
        {
            m_audioSource.Play();
        }
    }
}
