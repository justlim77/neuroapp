﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NeuroApp;

public class TutorialSequence : MonoBehaviour
{
    public CaseLoader caseLoader;
    public RectTransform tutorialLabel;
    public TextTyper textTyper;
    public string introMsg;
    public RectTransform bed;
    public RectTransform rightPanel;
    public string patientMsg;
    public RectTransform speechBubble;
    public string reactionMsg;
    public RectTransform descHeader;
    public RectTransform descText;
    public string descMsg;
    public RectTransform testPanel;
    public string testText;

    public Transform toolCursor;

    Button button;
    Image _image;
    bool _proceed = false;

    void Awake()
    {
        button = tutorialLabel.GetComponent<Button>();
        _image = GetComponent<Image>();
        _image.enabled = false;
    }

    void OnEnable()
    {
        //StartCoroutine(RunTutorial());
    }

    void Start()
    {
    }

    public void RunTutorial()
    {
        StartCoroutine(TutorialRoutine());
    }

    IEnumerator TutorialRoutine()
    {
        _image.enabled = true;
        tutorialLabel.gameObject.SetActive(true);
        PanelManager.Instance.EnablePanel(PanelType.Main);

        // Load case 1 as tutorial stage
        //caseLoader.LoadCase(0);
        yield return caseLoader.RunLoadCase(0);

        // Set parent to main panel > border panel render top
        transform.SetParent(bed.parent);
        tutorialLabel.SetParent(bed.parent);
        tutorialLabel.SetAsLastSibling();
        transform.SetSiblingIndex(tutorialLabel.GetSiblingIndex() - 1);
        yield return StartCoroutine(Type(introMsg));

        // Highlight patient & bed
        tutorialLabel.SetAsLastSibling();
        bed.SetSiblingIndex(tutorialLabel.GetSiblingIndex() - 1);
        yield return Type(patientMsg);

        // Highlight speech bubble
        rightPanel.SetAsLastSibling();
        transform.SetParent(rightPanel);
        tutorialLabel.SetParent(rightPanel);
        speechBubble.SetAsLastSibling();
        speechBubble.gameObject.SetActive(true);    // Enable speech bubble
        speechBubble.gameObject.GetComponentInChildren<Text>().text = "Hello!";
        tutorialLabel.SetAsLastSibling();
        transform.SetSiblingIndex(tutorialLabel.GetSiblingIndex() - 1);
        yield return Type(reactionMsg);

        // Highlight description header & text
        speechBubble.SetSiblingIndex(bed.GetSiblingIndex() + 1);
        speechBubble.gameObject.SetActive(false);    // Disable speech bubble
        descHeader.SetAsLastSibling();
        descText.SetAsLastSibling();
        yield return Type(descMsg);

        // Highlight test panel
        descHeader.SetAsFirstSibling();
        descText.SetAsFirstSibling();
        testPanel.SetAsLastSibling();
        yield return Type(testText);

        // Set tool cursor to highest layer
        toolCursor.SetAsLastSibling();

        // End
        tutorialLabel.gameObject.SetActive(false);
        //gameObject.SetActive(false);
        _image.enabled = false;

        yield break;
    }

    public void Proceed()
    {
        _proceed = true;
    }

    IEnumerator Type(string msg)
    {
        button.interactable = false;
        _proceed = false;
        print(msg);
        yield return textTyper.RunTypeText(msg);
        button.interactable = true;
        while (!_proceed)
            yield return null;
    }
}
