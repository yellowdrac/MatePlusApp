using System.Collections;
using DG.Tweening;
using Game;
using PowerTools;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject challengeContainer;
    [SerializeField] private GameObject scrollXPContainer;
    [SerializeField] private GameObject confirmationPopUp; // Referencia al Popup de Confirmación
    [SerializeField] private GameObject hintPopup; // Referencia al Popup de Hint
    [SerializeField] private CanvasGroup canvasGroupBlack; // Referencia al Popup de Hint
    [SerializeField] private TextMeshProUGUI confirmationText; // Referencia al texto dentro del Popup de Confirmación
    [SerializeField] private TextMeshProUGUI TextChallenge; // Referencia al texto del desafio
    [SerializeField] private TextMeshProUGUI option1TextChallenge; // Referencia al texto de la opcion 1
    [SerializeField] private TextMeshProUGUI option2TextChallenge; // Referencia al texto de la opcion 2
    [SerializeField] private TextMeshProUGUI option3TextChallenge; // Referencia al texto de la opcion 3
    [SerializeField] private TextMeshProUGUI option4TextChallenge; // Referencia al texto de la opcion 4
    [SerializeField] private TextMeshProUGUI option5TextChallenge;
    [SerializeField] private TextMeshProUGUI textCompChlallengeAfter;// Referencia al texto de la opcion 5
    [SerializeField] private Button confirmButton; // Referencia al texto del boton aceptar
    [SerializeField] private Button declineButton; // Referencia al texto del boton declinar
    [SerializeField] private Image imageChallenge;
    [SerializeField] private Image imageChallengeOption1;
    [SerializeField] private Image imageChallengeOption2;
    [SerializeField] private Image imageChallengeOption3;
    [SerializeField] private Image imageChallengeOption4;
    [SerializeField] private Image imageChallengeOption5;
    public Timer timer;
    
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private RectTransform content;
    
    private int optionSelected;
    private Coroutine crtStartChallenge;
    private ChallengesSister cs;
    private bool[] correctOptions;
    public void StartChallenge(Zone zone)
    {
        if (crtStartChallenge==null)
        {
            crtStartChallenge = StartCoroutine(CRTStartChallenge(zone));
        };
    }
    

    public void StartChallengeTimer(float duration)
    {
        if (timer != null)
        {
            Debug.Log("Entrando startchallngetimer");
            timer.StartTimer(duration);
        }
    }

    public void PauseChallengeTimer()
    {
        if (timer != null)
        {
            timer.PauseTimer();
        }
    }

    public void ResumeChallengeTimer()
    {
        if (timer != null)
        {
            timer.ResumeTimer();
        }
    }
    IEnumerator CRTStartChallenge(Zone zone)
    {
        GameController.Instance.Zone = zone;
        int randomChallengeId = Random.Range(0, GameController.Instance.RemoteData.activities[zone.ZoneID].challenges.Length);
        cs = GameController.Instance.RemoteData.activities[zone.ZoneID].challenges[randomChallengeId];
        
        
        //LEFT PAPER ##################################################################################################
        TextChallenge.text = cs.leftpaper.textChallenge;
        
        string imageUrl = cs.leftpaper.urlImage.url;
        
        // Cargar la imagen desde la URL
        StartCoroutine(LoadImage(imageUrl, imageChallenge));

        if (!cs.leftpaper.specialSymbols)
        {
            option1TextChallenge.text = cs.leftpaper.options[0].optionText;
            option2TextChallenge.text = cs.leftpaper.options[1].optionText;
            option3TextChallenge.text = cs.leftpaper.options[2].optionText;
            option4TextChallenge.text = cs.leftpaper.options[3].optionText;
            option5TextChallenge.text = cs.leftpaper.options[4].optionText;
        }else
        {
            Debug.Log(cs.leftpaper);
            StartCoroutine(LoadImage(cs.leftpaper.options[0].optionImg, imageChallengeOption1));
            imageChallengeOption1.enabled = true;
            StartCoroutine(LoadImage(cs.leftpaper.options[1].optionImg, imageChallengeOption2));
            imageChallengeOption2.enabled = true;
            StartCoroutine(LoadImage(cs.leftpaper.options[2].optionImg, imageChallengeOption3));
            imageChallengeOption3.enabled = true;
            StartCoroutine(LoadImage(cs.leftpaper.options[3].optionImg, imageChallengeOption4));
            imageChallengeOption4.enabled = true;
            StartCoroutine(LoadImage(cs.leftpaper.options[4].optionImg, imageChallengeOption5));
            imageChallengeOption5.enabled = true;
            
        }
        
        correctOptions= new bool[5];
        correctOptions[0] = cs.leftpaper.options[0].isCorrect;
        correctOptions[1] = cs.leftpaper.options[1].isCorrect;
        correctOptions[2] = cs.leftpaper.options[2].isCorrect;
        correctOptions[3] = cs.leftpaper.options[3].isCorrect;
        correctOptions[4] = cs.leftpaper.options[4].isCorrect;

        
        //HINTS #######################################################################################################
        var items = cs.hint.items;

        if (items != null && items.Length > 0)
        {
            float contentSize = 0;
            
            foreach (var item in items)
            {
                GameObject newItem = Instantiate(itemPrefab, content);
                
                TextMeshProUGUI textComponent = newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                
                Image imageComponent = newItem.transform.GetChild(1).GetComponent<Image>();

                StartCoroutine(LoadImage(item.urlImage.url, imageComponent));
                textComponent.text = item.hintText;

                contentSize += newItem.GetComponent<RectTransform>().sizeDelta.y;
            }
            
            content.sizeDelta = new Vector2 (content.sizeDelta.x, contentSize + items.Length * 10); // RE IMPORTANTE, EL 10 ES EL SPACING DEL CONTENT

            yield return new WaitForSeconds(3);

            challengeContainer.SetActive(true);
            Debug.Log("Antes de startchallngetimer");
            StartChallengeTimer(120f);
        }
        else
        {
            Debug.LogWarning("La lista de items está vacía o es nula.");
        }

        crtStartChallenge = null;
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

    public void OptionSelected(int option)
    {
        // Actualiza el texto del popup de confirmación con la opción seleccionada
        string optionLetter = "";

        if (option == 1)
        {
            optionLetter = "A)";
        }
        else if (option == 2)
        {
            optionLetter = "B)";
        }
        else if (option == 3)
        {
            optionLetter = "C)";
        }
        else if (option == 4)
        {
            optionLetter = "D)";
        }
        else if (option == 5)
        {
            optionLetter = "E)";
        }

        confirmationText.text = $"¿Estás seguro que deseas elegir la opción {optionLetter}?";
        // Muestra el popup de confirmación
        optionSelected = option-1;
        confirmationPopUp.SetActive(true);
    }
    public void HintSelected()
    {
        // Muestra el popup de confirmación
        hintPopup.SetActive(!hintPopup.activeSelf);
    }
    // Este método puede ser llamado por los botones de confirmar y cancelar en tu popup
    public void ConfirmSelection(bool isConfirmed)
    {
        if (isConfirmed)
        {
            // Lógica para manejar la confirmación de la selección aquí
            Debug.Log("Opción confirmada: " + confirmationText.text);
            Feedback.Do(eFeedbackType.ChallengeAccepted);
            confirmationPopUp.SetActive(false);
            challengeContainer.SetActive(false);

            for (int i = 0; i < correctOptions.Length; i++)
            {
                if (correctOptions[i])
                {
                    Debug.Log("optionSelected");
                    Debug.Log(optionSelected);
                    Debug.Log("i");
                    Debug.Log(i);
                    if (i == optionSelected) //Comprueba respuesta correcta, asigna putnos de Experiencia de 60 si es correcta, y 30 si es incorrecta, para no castigar al jugador
                    {
                        Debug.Log("aca es igual");
                        TextMeshProUGUI xpText = scrollXPContainer.GetComponentInChildren<TextMeshProUGUI>();
    
                        // Verificar si se encontró el componente TextMeshProUGUI
                        if (xpText != null)
                        {
                            xpText.text = "60 XP"; // Actualizar el texto con la experiencia ganada
                        }
                        scrollXPContainer.SetActive(true);
                        StartCoroutine(DelayedPlusExp(60, 3f));
                        
                    }
                    else
                    {
                        TextMeshProUGUI xpText = scrollXPContainer.GetComponentInChildren<TextMeshProUGUI>();
    
                        // Verificar si se encontró el componente TextMeshProUGUI
                        if (xpText != null)
                        {
                            xpText.text = "30 XP"; // Actualizar el texto con la experiencia ganada
                        }
                        scrollXPContainer.SetActive(true);
                        StartCoroutine(DelayedPlusExp(30, 3f));
                        
                    }

                    StartCoroutine(DoFade());
                    GameController.Instance.Player.IsChallenging = false;

                }
            }
            
        }
        else
        {
            // Esconde el popup de confirmación después de cualquier elección
            confirmationPopUp.SetActive(false);
        }    
    }
    // Corrutina para cambiar de imagen
    private IEnumerator DoFade()
    {
        Debug.Log("Estoy viendo la zona:");
        Debug.Log(GameController.Instance.Zone.ZoneID);
        yield return new WaitForSeconds(3);
        canvasGroupBlack.DOFade(1, 1);
        
        
        if (GameController.Instance.Zone.ZoneID == 0)
        {
            
            textCompChlallengeAfter.text = "Algo parece estar desbloqueandose en el camino...";
            yield return new WaitForSeconds(5);
            GameController.Instance.Zone.InitialAnim.SetActive(false);
            GameController.Instance.Zone.FinishedAnim.SetActive(true);
            
            GameController.Instance.Zone.ChallengeBlock.SetActive(false);
            canvasGroupBlack.DOFade(0, 1);
        }
        if (GameController.Instance.Zone.ZoneID == 1)
        {
            
            textCompChlallengeAfter.text = "Parece que ahora tienes más fuerza en las piernas para ejecutar el salto...";
            yield return new WaitForSeconds(5);
            GameController.Instance.Player.JumpForce = 1000;
            StartCoroutine(ResetJumpForceAfterDistance(10, 500));
            
            GameController.Instance.Zone.ChallengeBlock.SetActive(false);
            canvasGroupBlack.DOFade(0, 1);
        }
        if (GameController.Instance.Zone.ZoneID == 2)
        {
            
            textCompChlallengeAfter.text = "El humo se está disipando, parece ser una plataforma en la que se puede saltar...";
            yield return new WaitForSeconds(5);
            canvasGroupBlack.DOFade(0, 1);
            GameController.Instance.Zone.FinishedAnim.SetActive(true);
            yield return new WaitForSeconds(1);
            GameController.Instance.Zone.ChallengeBlock.SetActive(false);
            SpriteRenderer spriteRenderer = GameController.Instance.Zone.FinishedAnim.GetComponent<SpriteRenderer>();
            SmokeModel smokeModel = GameController.Instance.Zone.InitialAnim.GetComponent<SmokeModel>();
            if (spriteRenderer != null)
            {
                // Asegurarse de que el color inicial tenga alfa en 0
                Color color = spriteRenderer.color;
                color.a = 0f;
                spriteRenderer.color = color;
                
                // Usar DOTween para hacer el fade del alfa de 0 a 1 en 1 segundo
                spriteRenderer.DOFade(1f, 1f);
                
                if (smokeModel == null)
                {
                    Debug.Log("no esta bien");
                }
                smokeModel.StartAnim("dissappear");
            }
        }
        if (GameController.Instance.Zone.ZoneID == 3)
        {
            textCompChlallengeAfter.text = "Parece que ahora que sabes los datos finales del barril, se está empezando a caer...";
            yield return new WaitForSeconds(5);
            canvasGroupBlack.DOFade(0, 1);
            GameController.Instance.Zone.FinishedAnim.SetActive(true);
            yield return new WaitForSeconds(1);
            GameController.Instance.Zone.ChallengeBlock.SetActive(false);
            
            SmokeModel barrelModel = GameController.Instance.Zone.FinishedAnim.GetComponent<SmokeModel>();
            if (barrelModel == null)
            {
                Debug.Log("no esta bien");
            }
            barrelModel.StartAnim("explote");
            yield return new WaitForSeconds(0.2f);
            canvasGroupBlack.DOFade(1, 1);
            textCompChlallengeAfter.text = "El barril ha explotado, el camino ahora es libre...";
            yield return new WaitForSeconds(2);
            GameController.Instance.Zone.InitialAnim.SetActive(false);
            yield return new WaitForSeconds(2);
            canvasGroupBlack.DOFade(0, 1);
            
        }
        if (GameController.Instance.Zone.ZoneID == 4)
        {
            textCompChlallengeAfter.text = "Parece que el hielo se está empezando a descongelar...";
            yield return new WaitForSeconds(5);
            canvasGroupBlack.DOFade(0, 1);
            GameController.Instance.Zone.FinishedAnim.SetActive(true);
            yield return new WaitForSeconds(1);
            GameController.Instance.Zone.ChallengeBlock.SetActive(false);
            
            IceModel iceModel = GameController.Instance.Zone.FinishedAnim.GetComponent<IceModel>();
            if (iceModel == null)
            {
                Debug.Log("no esta bien");
            }
            iceModel.StartAnim("meltHigh");
            yield return new WaitForSeconds(1);
            iceModel.StartAnim("melt");
        }
        if (GameController.Instance.Zone.ZoneID == 5)
        {
            textCompChlallengeAfter.text = "Parece que el hielo se está empezando a descongelar...";
            yield return new WaitForSeconds(5);
            canvasGroupBlack.DOFade(0, 1);
            GameController.Instance.Zone.FinishedAnim.SetActive(true);
            yield return new WaitForSeconds(1);
            GameController.Instance.Zone.ChallengeBlock.SetActive(false);
            
            IceModel iceModel = GameController.Instance.Zone.FinishedAnim.GetComponent<IceModel>();
            if (iceModel == null)
            {
                Debug.Log("no esta bien");
            }
            iceModel.StartAnim("meltHigh");
            yield return new WaitForSeconds(1);
            iceModel.StartAnim("melt");
        }
    }
    // Corrutina para resetear la fuerza del salto
    private IEnumerator ResetJumpForceAfterDistance(float distance, float newJumpForce)
    {
        PlayerModel player = GameController.Instance.Player;
        float startX = player.transform.position.x;

        // Espera hasta que el jugador haya avanzado la distancia especificada en el eje x
        while (player.transform.position.x < startX + distance)
        {
            yield return null; // Espera un frame y vuelve a comprobar
        }

        // Restablece el JumpForce a su nuevo valor
        player.JumpForce = newJumpForce;
    }
    // Corrutina para agregar experiencia después de un retraso
    private IEnumerator DelayedPlusExp(int exp, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameController.Instance.Player.PlusExp(exp);
        yield return new WaitForSeconds(delayTime);
        scrollXPContainer.SetActive(false); 
    }
    
    
}
