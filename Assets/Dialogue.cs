using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] private Image backgroundImg;
    [SerializeField] private Sprite[] backgroundSprites; 
    public string[] lines;
    public float textSpeed;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        Feedback.Do(eFeedbackType.IntroMusic);
        textComponent.text = string.Empty;
        StartDialogue();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
                Debug.Log("Nueva Linea");
            }
            else
            {
                Debug.Log("se termino");
                StopAllCoroutines();
                textComponent.text = lines[index];
                
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        
    }
    void EndDialogue()
    {
        Feedback.Stop(eFeedbackType.IntroMusic);
        Debug.Log("estoy cambiando");
        SceneLoader.Instance.SetTargetScreen(eScreen.Game);
        SceneLoader.Instance.ChangeScreen(eScreen.Loading, true);
    }
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            Debug.Log(index);
            backgroundImg.GetComponent<Image>().sprite = backgroundSprites[index];
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
            gameObject.SetActive(false);
        }
    }
}
