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
    public bool isCorrect;
}


[System.Serializable]
public class LeftPaperSister
{
    public string textChallenge;
    public UrlImageSister urlImage;
    public OptionsSister[] options;
}

[System.Serializable]
public class ChallengesSister
{
    public LeftPaperSister leftpaper;
    public HintSister hint;   
    public string timeLimit;
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

public class RemoteController : MonoBehaviour
{
    
    
    
    
    
    private string REMOTE_PATH;

    //Game Authoring API Adapter
    protected const string GAME_AUTHORING_SERVER = "degauthoring-env.eba-8qzg6thz.us-east-1.elasticbeanstalk.com";
    protected const string GAME_AUTHORING_URL_API_LOGIN = "api/api-token-auth";
    protected const string GAME_AUTHORING_URL_API_GAME_CONFIG = "api/game_configs";
    protected const int GAME_ID = 9;

    [Header("USER")]

    [SerializeField] private string username;
    [SerializeField] private string password;

    private string token; //383c9115a207e6888ef82d8f604f05eabf2ad927

    private void Awake()
    {
        
        REMOTE_PATH = Application.persistentDataPath + "/Remote.json";
    }
    
   

    // REMOTE-------------------------------------------------------------------------
    // GAME'S DATA TO BE DOWNLOADED FROM THE CONTENT SERVER THAT WE CHOOSE
    public void DownloadRemote()
    {
        StartCoroutine(CRTGetJsonDEGA());
    }

    private IEnumerator CRTGetJsonDEGA()
    {
        int n;
        string url;

        //jsonRemote have the current data of Remote file in our device 
        string jsonRemote = LoadRemote();

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        url = String.Format(
            "https://{0}/{1}/",
            GAME_AUTHORING_SERVER,
            GAME_AUTHORING_URL_API_LOGIN);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SendWebRequest();

            n = 10;

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

                Login loginData = Login.FromJson(www.downloadHandler.text);

                token = loginData.Token;
            }
        }

        url = String.Format(
            "https://{0}/{1}/{2}/{3}",
            GAME_AUTHORING_SERVER,
            GAME_AUTHORING_URL_API_GAME_CONFIG,
            username,
            GAME_ID);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Token " + token);
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

                byte[] result = request.downloadHandler.data;
                string gameSessionsJSON = System.Text.Encoding.Default.GetString(result);

                //We get all previous configs not only the last
                List<RemoteSister> studentGameConfigs = JsonConvert.DeserializeObject<List<RemoteSister>>(gameSessionsJSON);

                if (studentGameConfigs != null)
                {
                    Debug.Log("Se obtuvo una nueva configuracion");

                    string jsonFetch = JsonConvert.SerializeObject(studentGameConfigs.Last());

                    if (jsonFetch != null) jsonRemote = jsonFetch;
                }
                else
                {
                    Debug.Log("No habia configuraciones");
                }

                //Have to actualize json
                File.WriteAllText(REMOTE_PATH, jsonRemote);
            }

            if (jsonRemote != null)
            {
                Debug.Log("Sobreescribimos el SO con el texto del archivo json");

                RemoteSister auxRemote = new RemoteSister();
                auxRemote = JsonConvert.DeserializeObject<RemoteSister>(jsonRemote);

                if (auxRemote.activities != null)
                {   
                    Debug.Log(JsonConvert.SerializeObject(auxRemote, Formatting.Indented));

                    GameController.Instance.RemoteData = auxRemote;
                }
            }
        }
    }

    private string LoadRemote()
    {
        if (!File.Exists(REMOTE_PATH))
        {
            Debug.Log("Creamos el archivo json");
            File.WriteAllText(REMOTE_PATH, "{\"activities\":[{\"learningActivity\":\"A01\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA01-1.png\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"HintA01-1\",\"description\":\"HintA01-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-1-1.png\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos ángulos\",\"urlImage\":{\"label\":\"HintA01-2\",\"description\":\"HintA01-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-1-2.png\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta enteriza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/ChallengeA01-1.png\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"HintA01-1\",\"description\":\"HintA01-1\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-1-1.png\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"HintA01-2\",\"description\":\"HintA01-2\",\"url\":\"https://videogamemateplusapp.s3.us-east-2.amazonaws.com/HintA01-1-2.png\"}}]},\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A02\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A03\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A04\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A05\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A06\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A07\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"}]},{\"learningActivity\":\"A08\",\"challenges\":[{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA02-1\",\"description\":\"ChallengeA02-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"},{\"leftPaper\":{\"textChallenge\":\"La puerta levadiza colocada a 40 m del piso y al mismo nivel de la base de este se tiene un ángulo de elevacion a, tal  a  como se muestra en la figura. Si tan ß=1/2, halle la  altura del edificio (h).\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"},\"options\":[{\"optionText\":\"A) 60m\",\"isCorrect\":false},{\"optionText\":\"B) 75m\",\"isCorrect\":false},{\"optionText\":\"C) 70m\",\"isCorrect\":false},{\"optionText\":\"D) 90m\",\"isCorrect\":false},{\"optionText\":\"E) 80m\",\"isCorrect\":true}]},\"hint\":{\"items\":[{\"hintText\":\"1. Definición de tangente\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1Pit_y23NRQjRUVUoAdB_kR7cYrBixG5u/view\"}},{\"hintText\":\"2. Teorema de la tangente de la suma de dos a\u00b4ngulos\",\"urlImage\":{\"label\":\"ChallengeA01-1\",\"description\":\"ChallengeA01-1\",\"url\":\"https://drive.google.com/file/d/1rwm56cD33yS1H5UPAM0MCFF7UMwDAhFF/view\"}}]},\"timeLimit\":\"05:00\"}]}]}");
        }
        Debug.Log(REMOTE_PATH);
        string json = File.ReadAllText(REMOTE_PATH);
        return json;
    }

}