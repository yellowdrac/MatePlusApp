using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Converters;
using System.Globalization;
using Game;

[System.Serializable]
public class ItemsSister
{
    public string hintText;
    public UrlImageSister urlImage; 
}

[System.Serializable]
public class HintSister
{
    public ItemsSister[] items;
}

[System.Serializable]
public class UrlImageSister
{
    public string label;
    public string description;
    public string url;
}

[System.Serializable]
public class OptionsSister
{
    public string optionText;
    public string optionImg;
    public bool correct;
}


[System.Serializable]
public class LeftPaperSister
{
    public string textChallenge;
    public bool specialSymbols;
    public UrlImageSister urlImage;
    public OptionsSister[] options;
}
[System.Serializable]
public class SolutionSister
{
    public UrlImageSister urlImage;
}
[System.Serializable]
public class ChallengesSister
{
    public LeftPaperSister leftpaper;
    public HintSister hint;  
    public SolutionSister solution;  
    public string timeLimit;
    public string university;
    public bool used;
}
[System.Serializable]
public class ActivitiesSister
{
    public string learningActivity;
    public ChallengesSister[] challenges;
}
[System.Serializable]
public class RemoteSister
{
    public ActivitiesSister[] activities;
    public int totalActivities;
}

public class Login
{
    [JsonProperty("token")]
    public string Token { get; set; }

    public static Login FromJson(string json) => JsonConvert.DeserializeObject<Login>(
        json,
        new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
                {
                    //ValueConverter.Singleton,
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                },
        }
    );
}
public class LoginRequest
{
    public string email;
    public string password;
}
public class RemoteController : MonoBehaviour
{
    
    
    
    
    
    private string REMOTE_PATH;

    //Game Authoring API Adapter
    protected const string GAME_AUTHORING_SERVER = "localhost:8090";
    protected const string GAME_AUTHORING_URL_API_LOGIN = "api/auth/login";
    protected const string GAME_AUTHORING_URL_API_GAME_CONFIG = "api/challenges/grouped-by-activity";
    protected const int GAME_ID = 9;

    [Header("USER")]

    [SerializeField] private string email;
    [SerializeField] private string password;

    private string token; //383c9115a207e6888ef82d8f604f05eabf2ad927

    private void Awake()
    {
        
        REMOTE_PATH = Application.persistentDataPath + "/Remote.json";
    }
    
   

    // REMOTE-------------------------------------------------------------------------
    // GAME'S DATA TO BE DOWNLOADED FROM THE CONTENT SERVER THAT WE CHOOSE
    public IEnumerator DownloadRemote()
    {
        yield return StartCoroutine(CRTGetJsonDEGA());
    }

    private IEnumerator CRTGetJsonDEGA()
    {
        int n;
        string url;

        //jsonRemote have the current data of Remote file in our device 
        string jsonRemote = LoadRemote();

      

        url = String.Format(
            "http://{0}/{1}",
            GAME_AUTHORING_SERVER,
            GAME_AUTHORING_URL_API_LOGIN);
        Debug.Log(url);
        
        // Crear un objeto JSON para enviar los datos de forma adecuada
        LoginRequest loginRequest = new LoginRequest();
        loginRequest.email = PlayerLevelInfo.user;
        loginRequest.password = PlayerLevelInfo.pass;
        
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
        }

        url = String.Format(
            "http://{0}/{1}",
            GAME_AUTHORING_SERVER,
            GAME_AUTHORING_URL_API_GAME_CONFIG);
        Debug.Log("url2");
        Debug.Log(url);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SendWebRequest();

            n = 10;

            while (n > 0)
            {
                if (n == 0) //10f is www.timeout
                {
                    request.Abort();
                    break;
                }
                if (request.isDone) break;

                n--;
                yield return new WaitForSeconds(1f);
            }

            if (!request.isDone ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("No se pudo obtener el archivo json de configuracion educativa");
            }
            else
            {
                Debug.Log("Se obtuvo satisfactoriamente la lista de jsons de configuracion educativa");
                Debug.Log(request.downloadHandler.data);
                byte[] result = request.downloadHandler.data;
                Debug.Log(request.downloadHandler.data);
                Debug.Log(System.Text.Encoding.Default.GetString(result));
                string gameSessionsJSON = System.Text.Encoding.Default.GetString(result);

                //We get all previous configs not only the last
                RemoteSister studentGameConfigs = JsonConvert.DeserializeObject<RemoteSister>(gameSessionsJSON);

                if (studentGameConfigs != null)
                {
                    Debug.Log("Se obtuvo una nueva configuracion");

                    string jsonFetch = JsonConvert.SerializeObject(studentGameConfigs);

                    if (jsonFetch != null)
                    {
                        Debug.Log("no es null json fetch");
                        Debug.Log(jsonRemote);
                        jsonRemote = jsonFetch;
                        Debug.Log(jsonRemote);
                    }
                    Debug.Log(jsonRemote);
                    Debug.Log(jsonFetch);
                }
                else
                {
                    Debug.Log("No habia configuraciones");
                }

                //Have to actualize json
                Debug.Log("escirbiendo");
                Debug.Log(jsonRemote);
                File.WriteAllText(REMOTE_PATH, jsonRemote);
            }

            if (jsonRemote != null)
            {
                Debug.Log("Sobreescribimos el SO con el texto del archivo json");

                RemoteSister auxRemote = new RemoteSister();
                auxRemote = JsonConvert.DeserializeObject<RemoteSister>(jsonRemote);

                if (auxRemote.activities != null)
                {   
                    Debug.Log("Activities no es null");
                    Debug.Log(JsonConvert.SerializeObject(auxRemote, Formatting.Indented));

                    GameController.Instance.RemoteData = auxRemote;
                }
            }
        }

        yield return null;
    }

    private string LoadRemote()
    {
        if (!File.Exists(REMOTE_PATH))
        {
            Debug.Log("Creamos el archivo json");
            File.WriteAllText(REMOTE_PATH, "{\"activities\":[{\"learningActivity\":\"A01\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA01-1.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 60m\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"B) 75m\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) 70m\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D) 90m\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) 80m\",\"optionImg\":null,\"correct\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"HintA01-1\",\"description\":\"HintA01-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-1-1.png\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos ángulos\",\"urlImage\":{\"label\":\"HintA01-2\",\"description\":\"HintA01-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-1-2.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"SolutionA01-1\",\"description\":\"SolutionA01-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/SolutionA01-1.png\"}},\"university\":\"SAN MARCOS\",\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a un altura (x) forma una angulo beta mostrado en la figura, a una distancia de 12. Se requiere saber el valor de x teniendo en cuenta la ecuación trigonométrica mencionada arriba.\",\"urlImage\":{\"label\":\"ChallengeA01-2\",\"description\":\"ChallengeA01-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA01-2.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 5\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"B) 7\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) 9\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"D) 10\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) 11\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Identidad trigonométrica: Seno de un ángulo es la inversa de la cosecante de ese mismo ángulo\",\"urlImage\":{\"label\":\"HintA01-2-1\",\"description\":\"HintA01-2-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-2-1.png\"}},{\"hintText\":\"2. Definición de secante\",\"urlImage\":{\"label\":\"HintA01-2-2\",\"description\":\"HintA01-2-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-2-2.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"SolutionA01-2\",\"description\":\"SolutionA01-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/SolutionA01-2.png\"}},\"university\":\"PUCP\",\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a un altura h forma una angulo que es dividido en 2 por una linea mostrado en la figura. Calcular h teniendo en cuenta que la puerta en es de unos 30 m, así como la expresión mencionada arriba.\",\"urlImage\":{\"label\":\"ChallengeA01-3\",\"description\":\"ChallengeA01-3\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA01-3.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 19,5\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"B) 22,5\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) 18\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D) 18\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) 24\",\"optionImg\":null,\"correct\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Reducción al primer cuadrante\",\"urlImage\":{\"label\":\"HintA01-3-1\",\"description\":\"HintA01-3-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-3-1.png\"}},{\"hintText\":\"2. Seno y coseno de ángulos complementarios\",\"urlImage\":{\"label\":\"HintA01-3-2\",\"description\":\"HintA01-3-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-3-2.png\"}},{\"hintText\":\"3. Seno del doble de un ángulo\",\"urlImage\":{\"label\":\"HintA01-3-3\",\"description\":\"HintA01-3-3\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-3-3.png\"}},{\"hintText\":\"4. Definición de seno de un ángulo\",\"urlImage\":{\"label\":\"HintA01-3-4\",\"description\":\"HintA01-3-4\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-3-4.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"SolutionA01-3\",\"description\":\"SolutionA01-3\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/SolutionA01-3.png\"}},\"university\":\"SAN MARCOS\",\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A02\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"No se puede avanzar saltando normalmente puesto que el risco se encuentra a una altura muy alta. Sin embargo, se ha realizado una plano considerando los siguientes datos de la figura. Necesitas hallar el valor de X teniendo en cuenta los datos de los ángulos.\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-1.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 225\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"B) 200\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"C) 220\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D) 230\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) 210\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Teorema de Cossnos\",\"urlImage\":{\"label\":\"HintA02-1-1\",\"description\":\"HintA02-1-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA02-1-1.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"UNI\",\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"Este risco es muy alto... Parece que α, β y γ las medidas de los ángulos interiores del triángulo mostrado, tales que α < β < γ , γ = 2α. Si las medidas de los lados son numéricamente iguales a tres números consecutivos, entonces calcular el senβ. Con ese cálculo desbloquearás una habilidad mágica.\",\"urlImage\":{\"label\":\"ChallengeA02-2\",\"description\":\"ChallengeA02-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-2.png\"},\"specialSymbols\":true,\"options\":[{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-2-Opt-1.png\",\"correct\":false},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-2-Opt-2.png\",\"correct\":false},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-2-Opt-3.png\",\"correct\":true},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-2-Opt-4.png\",\"correct\":false},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-2-Opt-5.png\",\"correct\":false}]},\"hint\":{\"items\":[]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"SAN MARCOS\",\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"Parece ser un risco es muy alto... Este triángulo mostrado tiene los datos de los lados ubicados con variables. Sin embargo, para desbloquear una habilidad mágica se requiere resolver una cierta expresión trigonométrica.\",\"urlImage\":{\"label\":\"ChallengeA02-3\",\"description\":\"ChallengeA02-3\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA02-3.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 4\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"B) 3\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) 2\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D) 1\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) 5\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"PUCP\",\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A03\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"Merlín ha logrado ver detrás del humo en un intento de ayudarte pero, ¿será esta la figura realmente?. Complementa la información calculando el área sabiendo que en cada lado de un triángulo equilátero de 4 m de lado, se construye un cuadrado. Además uniendo los 6 vértices exteriores de los cuadrados, se determina un hexágono.\",\"urlImage\":{\"label\":\"ChallengeA03-1\",\"description\":\"ChallengeA03-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA03-1.png\"},\"specialSymbols\":true,\"options\":[{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA03-1-Opt-1.png\",\"correct\":true},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA03-1-Opt-2.png\",\"correct\":false},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA03-1-Opt-3.png\",\"correct\":false},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA03-1-Opt-4.png\",\"correct\":false},{\"optionText\":null,\"optionImg\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA03-1-Opt-5.png\",\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de áreas parciales y total\",\"urlImage\":{\"label\":\"HintA03-1-1\",\"description\":\"HintA03-1-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA03-1-1.png\"}},{\"hintText\":\"2. Definición de triángulo equilátero\",\"urlImage\":{\"label\":\"HintA03-1-2\",\"description\":\"HintA03-1-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA03-1-2.png\"}},{\"hintText\":\"3. Área del triángulo con seno de ángulo\",\"urlImage\":{\"label\":\"HintA03-1-3\",\"description\":\"HintA03-1-3\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA03-1-3.png\"}},{\"hintText\":\"4. Área del cuadrado\",\"urlImage\":{\"label\":\"HintA03-1-4\",\"description\":\"HintA03-1-4\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA03-1-4.png\"}},{\"hintText\":\"5. Ángulos complementarios y suplementarios\",\"urlImage\":{\"label\":\"HintA03-1-5\",\"description\":\"HintA03-1-5\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA03-1-5.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"UNI\",\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A04\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"Los rubíes tapan el camino; sin embargo, existe un barril con forma de cilindro recto que contiene un líquido corrosivo. Sabiendo que la altura promedio del pilar más alto es 24 y el segmento de mayor longitud que une dos puntos de sus bases determina con el plano de la base un ángulo de 74\u00b0, calcular el volumen\",\"urlImage\":{\"label\":\"ChallengeA04-1\",\"description\":\"ChallengeA04-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA04-1.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 293 pi\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"B) 295 pi\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) 296 pi\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D) 294 pi\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"E) 292 pi\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Triángulo notable de 74\",\"urlImage\":{\"label\":\"HintA04-1-1\",\"description\":\"HintA04-1-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA04-1-1.png\"}},{\"hintText\":\"2. Diámetro de una circunferencia\",\"urlImage\":{\"label\":\"HintA04-1-2\",\"description\":\"HintA04-1-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA04-1-2.png\"}},{\"hintText\":\"3. Volumen de un cilindro\",\"urlImage\":{\"label\":\"HintA04-1-3\",\"description\":\"HintA04-1-3\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA04-1-3.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"UNI\",\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A05\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"En esta superficie de hielo, parece haber inscripciones en un idioma arcano, que parecen ser un acertijo dejado por una civilización perdida. Estas inscripciones guardan la clave para descongelar el hielo. Resuelve lo que se solicita\",\"urlImage\":{\"label\":\"ChallengeA05-1\",\"description\":\"ChallengeA05-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA05-1.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 1\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"B) -2\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) 2\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D) -1\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) 0\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Método de reducción\",\"urlImage\":{\"label\":\"HintA05-1-1\",\"description\":\"HintA05-1-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA05-1-1.png\"}},{\"hintText\":\"2. Método de sustitución\",\"urlImage\":{\"label\":\"HintA05-1-2\",\"description\":\"HintA05-1-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA05-1-2.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"UNI\",\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A06\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"Parece haber inscripciones en un idioma arcano dentro de este hielo. Se visualiza un acertijo dejado por magos ancestrales. Los logaritmos en la magia pueden representar el dominio sobre la complejidad y la comprensión profunda de los principios arcanos. Resuelve lo que se solicita para desbloquear el hielo.\",\"urlImage\":{\"label\":\"ChallengeA06-1\",\"description\":\"ChallengeA06-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA06-1.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) x E [-10;10]\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"B) x E [-9;9]\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) |x|>=9\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D) |x|<9\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) |x|>9\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de base de logaritmo\",\"urlImage\":{\"label\":\"HintA06-1-1\",\"description\":\"HintA06-1-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA06-1-1.png\"}},{\"hintText\":\"2. Propiedad expresión de raiz cuadrada\",\"urlImage\":{\"label\":\"HintA06-1-2\",\"description\":\"HintA06-1-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA06-1-2.png\"}},{\"hintText\":\"3. Logaritmo en base 10\",\"urlImage\":{\"label\":\"HintA06-1-3\",\"description\":\"HintA06-1-3\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA06-1-3.png\"}},{\"hintText\":\"4. Diferencia de cuadrados\",\"urlImage\":{\"label\":\"HintA06-1-4\",\"description\":\"HintA06-1-4\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA06-1-4.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"UNI\",\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A07\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"Observa bien a los cuervos que revolotean a tu alrededor. Cada día, su número aumenta de una manera peculiar (van 5 días). Has visto por ahora una secuencia establecida. Descifra el patrón para cuando se terminen los 10 primeros días (sumando todos los cuervos).\",\"urlImage\":{\"label\":\"ChallengeA07-1\",\"description\":\"ChallengeA07-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA07-1.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A) 520\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"B) 475\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"C) 495\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"D) 459\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E) 510\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Secuencias\",\"urlImage\":{\"label\":\"HintA07-1-1\",\"description\":\"HintA07-1-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA07-1-1.png\"}},{\"hintText\":\"2. Relación cuadrática\",\"urlImage\":{\"label\":\"HintA07-1-2\",\"description\":\"HintA07-1-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA07-1-2.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"UNI\",\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A08\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"Ten cuidado, estan cayendo 5 bolas de fuego. Debes de poder saber cual es el que caerá primero para poder salvarte y seguir tu camino delante. Mira la figura que representa cuantos metros le falta a las bolas de fuego por recorrer.\",\"urlImage\":{\"label\":\"ChallengeA08-1\",\"description\":\"ChallengeA08-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA08-1.png\"},\"specialSymbols\":false,\"options\":[{\"optionText\":\"A)\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"B)\",\"optionImg\":null,\"correct\":true},{\"optionText\":\"C)\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"D)\",\"optionImg\":null,\"correct\":false},{\"optionText\":\"E)\",\"optionImg\":null,\"correct\":false}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de notación científica\",\"urlImage\":{\"label\":\"HintA08-1-1\",\"description\":\"HintA08-1-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA08-1-1.png\"}}]},\"solution\":{\"urlImage\":{\"label\":\"\",\"description\":\"\",\"url\":\"\"}},\"university\":\"UNI\",\"timeLimit\":\"05:00\"}]}],\"totalActivities\":12}");
        }
        Debug.Log(REMOTE_PATH);
        string json = File.ReadAllText(REMOTE_PATH);
        return json;
    }

}