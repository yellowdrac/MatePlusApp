using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private Button btnMenuPlay;
    [SerializeField] private Button btnMenuSettings;
    [SerializeField] private Button btnMenuQuit;

    [Header("Settings")]
    [SerializeField] private Button btnSettReturn;
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject loginIncorrecto;
    [SerializeField] private TextMeshProUGUI txtUser;
    [SerializeField] private TextMeshProUGUI txtPass;

    [SerializeField] private Button btnLogin;
    [SerializeField] private Button btnLoginIncorrecto;
    
    
    protected const string GAME_AUTHORING_SERVER = "localhost:8090";
    protected const string GAME_AUTHORING_URL_API_LOGIN = "api/auth/login";
    private string token; //383c9115a207e6888ef82d8f604f05eabf2ad927
    void Start()
    {
        btnMenuPlay.onClick.AddListener(OnMenuPlayBtnPressed);
        btnLoginIncorrecto.onClick.AddListener(OnLoginIncorrectoPlayBtnPressed);
        btnLogin.onClick.AddListener(() => StartCoroutine(OnLoginPlayBtnPressed())); 
        //btnMenuQuit.onClick.AddListener(OnMenuQuitBtnPressed);
    }
    private void OnLoginIncorrectoPlayBtnPressed()
    {
        loginIncorrecto.gameObject.SetActive(false);
    }
    private void OnMenuPlayBtnPressed()
    {
        login.gameObject.SetActive(true);
    }
    private IEnumerator OnLoginPlayBtnPressed()
    {
        yield return StartCoroutine(DownloadRemote());
        
        
    }

    public IEnumerator DownloadRemote()
    {
        yield return StartCoroutine(CRTGetJsonDEGA());
    }

    private IEnumerator CRTGetJsonDEGA()
    {
        int n;
        string url;

        //jsonRemote have the current data of Remote file in our device 
        



        url = String.Format(
            "http://{0}/{1}",
            GAME_AUTHORING_SERVER,
            GAME_AUTHORING_URL_API_LOGIN);
        Debug.Log(url);

        // Crear un objeto JSON para enviar los datos de forma adecuada
        LoginRequest loginRequest = new LoginRequest();
        loginRequest.email = txtUser.text;
        loginRequest.password =txtPass.text;

        string jsonData = JsonUtility.ToJson(loginRequest);
        Debug.Log(jsonData);
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json"); // Establecer el tipo de contenido como JSON

            www.SendWebRequest();

            n = 15;

            while (n > 0)
            {
                if (n == 0) //10f is www.timeout
                {
                    www.Abort();
                    break;
                }

                if (www.isDone) break;

                n--;
                yield return new WaitForSeconds(1f);
            }
            Debug.Log("Resutlaod wwww: "+www.result);
            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Log es incorrecto");
                loginIncorrecto.gameObject.SetActive(true);
            }
            else
            {
                if (!www.isDone ||
                    www.result == UnityWebRequest.Result.ProtocolError ||
                    www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("No se logeo al jugador en EDU Game Authoring Platform");
                    
                }
                else
                {
                    Debug.Log("Se logeo al jugador en EDU Game Authoring Platform");
                    Debug.Log("www.downloadHandler.tex");
                    Debug.Log(www.downloadHandler.text);
                    Login loginData = Login.FromJson(www.downloadHandler.text);

                    token = loginData.Token;
                    Debug.Log("token");
                    Debug.Log(token);
                }

                PlayerLevelInfo.user = loginRequest.email;
                PlayerLevelInfo.pass = loginRequest.password;
                SceneLoader.Instance.SetTargetScreen(eScreen.Introduction);
                SceneLoader.Instance.ChangeScreen(eScreen.Loading, true);
            }
            
        }
    }

    private void OnMenuQuitBtnPressed()
    {
        Application.Quit();
    }
}
