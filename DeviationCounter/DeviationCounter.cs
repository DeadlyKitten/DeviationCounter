using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.UI;

namespace DeviationCounter
{
    class DeviationCounter : MonoBehaviour
    {
        private TextMeshProUGUI _counter;
        private float _sum;

        void Start()
        {
            StartCoroutine(FindScoreController());

            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler cs = gameObject.AddComponent<CanvasScaler>();
            cs.scaleFactor = 10.0f;
            cs.dynamicPixelsPerUnit = 10f;
            GraphicRaycaster gr = gameObject.AddComponent<GraphicRaycaster>();
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);

            _counter = CustomUI.BeatSaber.BeatSaberUI.CreateText(canvas.transform as RectTransform, $"0 ms\nEarly", Vector2.zero);
            _counter.alignment = TextAlignmentOptions.Center;
            _counter.transform.localScale *= .12f;
            _counter.fontSize = 2.5f;
            _counter.color = Color.white;
            _counter.lineSpacing = -50f;
            _counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            _counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            _counter.enableWordWrapping = false;
            _counter.transform.localPosition = new Vector3(-0.1f, 2.5f, 8f);
        }

        IEnumerator FindScoreController()
        {
            yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<ScoreController>().Any());
            yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().Any());

            ScoreController scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().First();
            AudioTimeSyncController timeSyncController = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().First();

            scoreController.noteWasCutEvent += OnNoteHit;
        }

        void OnNoteHit(NoteData data, NoteCutInfo info, int score)
        {
            _sum += info.timeDeviation;
            
            Logger.Log($"Note {data.id} cut with deviation: {info.timeDeviation}");

            var deviation = _sum / (data.id + 1) * 1000;
            var descriptor = (deviation > 0) ? "Early" : "Late";
            _counter.text = $"{Mathf.Round(deviation)} ms\n{descriptor}";
        }
    }
}
