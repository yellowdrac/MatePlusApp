using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class FinalUIController : MonoBehaviour
{
    public static FinalUIController Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI txtStatisticsQuestions;
    [SerializeField] private TextMeshProUGUI txtStatisticsAnsweredQuestions;
    [SerializeField] private TextMeshProUGUI txtStatisticsAnsweredQuestionsPorc;
    [SerializeField] private TextMeshProUGUI[] txttotalTimeZones;
    [SerializeField] private TextMeshProUGUI[] txtQuestionsZone;
    
    [SerializeField] private TextMeshProUGUI[] txttotalTimeArea;
    [SerializeField] private TextMeshProUGUI[] txtQuestionsArea;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject solution;
    [SerializeField] private Image solutionImage;
    [SerializeField] private GameObject king;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform contentChallenge;
    [SerializeField] private Button buttonHideSolution;

    public float  contentizeChallenge; 
    // Start is called before the first frame update
    void Start()
    {
        contentizeChallenge = 0;
        txtStatisticsQuestions.text = PlayerLevelInfo.totalQuestions.ToString();
        txtStatisticsAnsweredQuestions.text = PlayerLevelInfo.totalAnsCorrectQuestions.ToString();
        float percentageQuestZone = 0f;
        for (int i = 0; i < 8; i++)
        {
            if (PlayerLevelInfo.timePerZone[i] == 0f)
            {
                txttotalTimeZones[i].fontSize = 34;
                txtQuestionsZone[i].fontSize = 34;
                txttotalTimeZones[i].text = "NO ESTABLECIDO";
                txtQuestionsZone[i].text= "NO ESTABLECIDO";
            }
            else
            {
                percentageQuestZone = (float) PlayerLevelInfo.ansCorrectQuestionsPerZone[i] / PlayerLevelInfo.questionsPerZone[i] * 100;
                txtQuestionsZone[i].text = PlayerLevelInfo.ansCorrectQuestionsPerZone[i].ToString()+" / "+PlayerLevelInfo.questionsPerZone[i].ToString()+" => "+percentageQuestZone.ToString("F2") + "%";
                txttotalTimeZones[i].text = FormatTime(PlayerLevelInfo.timePerZone[i]);                
            }
            
            
        }
        
        // Calcular el porcentaje de preguntas contestadas
        float percentage = 0f;
        if (PlayerLevelInfo.totalQuestions > 0)
        {
            percentage = (float)PlayerLevelInfo.totalAnsCorrectQuestions / PlayerLevelInfo.totalQuestions * 100;
        }

        // Asignar el porcentaje formateado al texto, con dos decimales
        txtStatisticsAnsweredQuestionsPorc.text = percentage.ToString("F2") + "%";
        
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        int j = 1;
        float contentSize = 0;
        foreach (var challenge in PlayerLevelInfo.challenges)
        {
            
            GameObject newItem = Instantiate(itemPrefab, content);
                
            TextMeshProUGUI textComponent = newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                
            Image imageComponent = newItem.transform.GetChild(1).GetComponent<Image>();
            RectTransform rectTransformX = imageComponent.GetComponent<RectTransform>();

            // Establece el tamaño deseado (450x450)
            rectTransformX.sizeDelta = new Vector2(450, 450);
            TextMeshProUGUI textComponentTimer = newItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI textComponentIsCorrect = newItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI textComponentAnswer = newItem.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            
                
            imageComponent.GetComponent<RectTransform>().sizeDelta = new Vector2(imageComponent.GetComponent<RectTransform>().sizeDelta.x, 300);

            StartCoroutine(LoadImage(challenge.urlImage, imageComponent));
            textComponent.text = challenge.txtChallenge;
            textComponentTimer.text = FormatTime(challenge.timeResult);
            if (challenge.correct)
            {
                textComponentIsCorrect.text = "Correcto";
            }
            else
            {
                textComponentIsCorrect.text = "Incorrecto";
            }

            if (challenge.correctOptionIsImage)
            {
                
                Image imageComponentResp = newItem.transform.GetChild(5).GetComponent<Image>();
                
                imageComponentResp.enabled = true;
                Color color = imageComponentResp.color;
                color.a = 1.0f; // 255 en términos de 0-1
                imageComponentResp.color = color;
                RectTransform rectTransformXResp = imageComponentResp.GetComponent<RectTransform>();

                // Establece el tamaño deseado (450x450)
                rectTransformXResp.sizeDelta = new Vector2(350, 70);
                StartCoroutine(LoadImage(challenge.urlImageRightSolution, imageComponentResp));
            }
            else
            {
                textComponentAnswer.text = challenge.txtRightSol;    
            }
            TextMeshProUGUI textChallengeNumber = newItem.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
            
            textChallengeNumber.text = "Desafío " + j;
            Button gameObjectButton = newItem.transform.GetChild(10).GetComponent<Button>();
            buttonHideSolution.onClick.AddListener(OnHideSolution);
            gameObjectButton.onClick.AddListener(() => OnSolutionBtnPressed(challenge.urlSolution));
            contentSize += newItem.GetComponent<RectTransform>().sizeDelta.y;
            j += 1;
        }
        content.sizeDelta = new Vector2 (content.sizeDelta.x, contentSize + PlayerLevelInfo.challenges.Count * 10); // RE IMPORTANTE, EL 10 ES EL SPACING DEL CONTENT
        contentizeChallenge = contentSize + PlayerLevelInfo.challenges.Count * 10;
        
        // txtStatisticsPrt1.text = PlayerLevelInfo.currentLevel.ToString();
        // txtStatisticsPrt1.text = PlayerLevelInfo.currentLevel.ToString();
        // txtStatisticsPrt1.text = PlayerLevelInfo.currentLevel.ToString();
        // txtStatisticsPrt1.text = PlayerLevelInfo.currentLevel.ToString();
        float percentageQuestArea = 0f;
        for (int i = 0; i <= 4; i++)
        {
            if (PlayerLevelInfo.timePerArea[i] == 0f)
            {
                txttotalTimeArea[i].fontSize = 34;
                txtQuestionsArea[i].fontSize = 34;
                txttotalTimeArea[i].text = "NO ESTABLECIDO";
                txtQuestionsArea[i].text= "NO ESTABLECIDO";
            }
            else
            {
                percentageQuestArea = (float) PlayerLevelInfo.ansCorrectQuestionsPerArea[i] / PlayerLevelInfo.questionsPerArea[i] * 100;
                txtQuestionsArea[i].text = PlayerLevelInfo.ansCorrectQuestionsPerArea[i].ToString()+" / "+PlayerLevelInfo.questionsPerArea[i].ToString()+" => "+percentageQuestArea.ToString("F2") + "%";
                txttotalTimeArea[i].text = FormatTime(PlayerLevelInfo.timePerArea[i]);                
            }
            
            
        }
    }
    private void OnHideSolution()
    {
        king.gameObject.SetActive(true);
        solution.gameObject.SetActive(false);
        
    }
    private void OnSolutionBtnPressed(string url)
    {
        king.gameObject.SetActive(false);
        solution.gameObject.SetActive(true);
        StartCoroutine(LoadImage(url, solutionImage));
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
         
        }
    }
    private IEnumerator LoadImage(string url, Image image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        else
        {
            Debug.LogError("Error loading image: " + www.error);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public float getContentSize()
    {
        return contentizeChallenge;
    }
}
