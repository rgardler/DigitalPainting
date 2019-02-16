using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.digitalpainting.devtest;
using wizardscode.environment;
using wizardscode.environment.test;

namespace wizardscode.devtest
{
    public class PlayTestManager : MonoBehaviour
    {
        [Header("UI Configuration")]
        [Tooltip("Prefab for UI")]
        public GameObject UIPrefab;
        public bool showWeatherUI = true;
        public bool showDayNightUI = true;
        public bool showInterestingThingsUI = true;
        public bool showDroneControlUI = true;

        protected DigitalPaintingManager manager;
        private GameObject ui;

        void Awake()
        {
            manager = FindObjectOfType<DigitalPaintingManager>();

            // Place and configure the UI in the scene	
            ui = GameObject.Instantiate(UIPrefab);
            ShowUIElements();
        }

        protected virtual void Start()
        {

            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject go = new GameObject("EventSystem");
                go.AddComponent<EventSystem>();
                go.AddComponent<StandaloneInputModule>();
            }
        }

        private void ShowUIElements()
        {
            Component component = ui.GetComponentInChildren<WeatherUI>();
            component.gameObject.SetActive(showWeatherUI);

            component = ui.GetComponentInChildren<DayNightCycleUI>();
            component.gameObject.SetActive(showDayNightUI);

            component = ui.GetComponentInChildren<InterestingThingsUI>();
            component.gameObject.SetActive(showInterestingThingsUI);

            component = ui.GetComponentInChildren<DroneControlUI>();
            component.gameObject.SetActive(showDroneControlUI);
        }
    }
}
