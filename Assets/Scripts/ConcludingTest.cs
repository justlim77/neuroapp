﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConcludingTest : MonoBehaviour {

    #region Public Variables
    public Case.TestType m_TestType = Case.TestType.Single;
    public List<Text> optionsText = new List<Text>();
    public string[] optionsPrefix = { "A) ", "B) ", "C) ", "D) ", "E) " };
    public Color32 m_CorrectColor;
    public Color32 m_WrongColor;
    public float m_FlashDuration = 1.0f;
    public GameObject m_ButtonPrefab;
    public GameObject m_EndButton;
    public Text[] m_RationaleTexts;
    #endregion

    #region Private Variables
    private Case.Answer m_Answer;
    private int m_OptionCount;
    private bool m_FirstAttempt;
    #endregion

	void Start () {
        Init();
	}

    public bool Init() {
        bool result = true;

        // Disable "Next" button
        if (m_EndButton.activeInHierarchy) m_EndButton.SetActive(false);

        // Disable rationale images
        foreach (Text text in m_RationaleTexts)
            text.enabled = false;

        // Clear any previous instantiated buttons
        transform.Clear();

        // Assign new option count
        m_OptionCount = 0;
        m_OptionCount = Patient.g_PatientCase.optionCount;

        // Assign new answer
        m_Answer = Patient.g_PatientCase.answer;

        // Iterate thru loop to create buttons...
        optionsText.Clear();

        for (int i = 0; i < m_OptionCount; i++)
        {
            GameObject newButton = (GameObject) Instantiate(m_ButtonPrefab, Vector2.zero, Quaternion.identity);
            newButton.transform.SetParent(transform);
            ConcludingButton buttonScript = newButton.GetComponent<ConcludingButton>();
            buttonScript.Init();
            buttonScript.option = (Case.Answer)i;
            buttonScript.correctColor = m_CorrectColor;
            buttonScript.wrongColor = m_WrongColor;
            buttonScript.buttonFlashDuration = m_FlashDuration;
            optionsText.Add(newButton.GetComponentInChildren<Text>());
            optionsText[i].text = optionsPrefix[i] + Patient.g_PatientCase.concludingTests[i];
        }

        // Reset first attempt
        m_FirstAttempt = true;

        return result;
    }

    public bool ValidateAnswer(Case.Answer answer) {
        switch (m_TestType) {
            case Case.TestType.Single:
                if (answer == m_Answer)
                {
                    // Star system check
                    if (m_FirstAttempt)
                    {
                        Patient.g_PatientCase.caseStars += 1;
                        m_FirstAttempt = false;
                    }

                    // Enable "Next" button
                    m_EndButton.SetActive(true);

                    // Enable rationale images
                    foreach (Text text in m_RationaleTexts)
                        text.enabled = true;

                    return true;
                }
                else {
                    m_FirstAttempt = false;
                }

                return false;

            case Case.TestType.Multiple:
                return false;
        }

        return false;
    }
}
