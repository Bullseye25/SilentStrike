using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System;
using Newtonsoft.Json;
using TMPro;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    const string serverURL = "https://minigame-backend-new.vercel.app/";
    private string token = "No Token";
    public string walletId = "No Wallet";
    public string userName = "Guest";
    public string game; // kishuInu

    private void Reset()
    {
        walletId = "No Wallet";
        userName = "Guest";
        token = "No Token";
    }

    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SendToFrontEnd(string message);
    #endif

    public void ConnectWallet()
    {
        Debug.Log("ConnectWallet");

    #if UNITY_WEBGL && !UNITY_EDITOR
        SendToFrontEnd("ConnectWallet");
    #else
            Debug.Log("ConnectWallet simulated in editor.");
    #endif
    }

    private void Start()
    {
        Debug.Log("Start");
        Initialize();
        ConnectWallet();
    }

    public void Initialize()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
            Debug.Log("NetworkManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
        return;
    }

    private string GetFirstUpToFourCharacters(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return input.Substring(0, Mathf.Min(4, input.Length));
    }

    public void SetGame(string gameName)
    {     
        game = gameName;
        
        var value = GetFirstUpToFourCharacters(game);
        Debug.Log($"setting G {value}");
    }

    public void SetWalletAddress(string walletId)
    {
        this.walletId = walletId;

        var value = GetFirstUpToFourCharacters(walletId);
        Debug.Log($"setting WI  {value}");
    }

    public void SetUserName(string userName)
    {
        this.userName = userName;

        var value = GetFirstUpToFourCharacters(userName);
        Debug.Log($"setting UN  {value}");
    }

    public void SetToken(string token)
    {
        this.token = token;

        var value = GetFirstUpToFourCharacters(token);
        Debug.Log($"setting T {value}");
    }

    private string[] GenerateChallenge(string gameId)
    {
        string symmetricKey = "p1r2i3smp1r2i3smp1r2i3smp1r2i3sm";
        byte[] key = Encoding.UTF8.GetBytes(symmetricKey);

        using (Aes aes = Aes.Create())
        {   //Set key and generate IV
            aes.Key = key;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            byte[] iv = aes.IV;
            //Encrypt
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] gameNameByte = Encoding.UTF8.GetBytes(gameId);
            byte[] gameNameByteEncrypted;

            using (var ms = new System.IO.MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(gameNameByte, 0, gameNameByte.Length);
                    cs.FlushFinalBlock();
                }
                gameNameByteEncrypted = ms.ToArray();
            }

            //Return challenge and IV seperately
            string encryptedData = Convert.ToBase64String(gameNameByteEncrypted);
            string ivBase64 = Convert.ToBase64String(iv);

            return new string[] { encryptedData, ivBase64 };

        }
    }

    private string DecryptChallenge(string encryptedData, string ivBase64)
    {
        string symmetricKey = "p1r2i3smp1r2i3smp1r2i3smp1r2i3sm";
        byte[] key = Encoding.UTF8.GetBytes(symmetricKey);
        byte[] iv = Convert.FromBase64String(ivBase64);
        byte[] encryptedDataByte = Convert.FromBase64String(encryptedData);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new System.IO.MemoryStream(encryptedDataByte))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var reader = new System.IO.StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }

    [ContextMenu("GetData")]
    public void TestEncryption()
    {
        string[] challengeData = GenerateChallenge(game);
        string decryptedData = DecryptChallenge(challengeData[0], challengeData[1]);

        if (decryptedData == game)
        {
            Debug.Log("Encryption and decryption successful");
            return;
        }

        Debug.Log("Encryption and decryption failed");
        Debug.Log("Decrypted data: " + decryptedData);

        return;
    }

    public void UpdateScoreOnLeaderBoard(int score) 
    {
        StartCoroutine(SetLeaderboard(score));
    }

    private IEnumerator SetLeaderboard(int _score, bool _isCheater = false)
    {
        string[] challengeData = GenerateChallenge(game);

        // Create the JSON body
        var jsonBody = new
        {
            walletId = walletId,
            userName = userName,
            score = _score.ToString(),
            isCheater = _isCheater.ToString(),
            token = token,
            game = game,
            challenge = challengeData[0],
            iv = challengeData[1]
        };

        string bodyString = JsonUtility.ToJson(jsonBody);

        // Create the request
        UnityWebRequest request = new UnityWebRequest(serverURL + "/leaderboard/SetLeaderboard", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error updating _score: " + request.error);
        }
        else
        {
            Debug.Log($"Score updated successfully: {_score}");
        }
    }

    public IEnumerator GetLeaderboard(Action<String> callback)
    {
        Debug.Log("Getting leaderboard...");
        using (UnityWebRequest www = UnityWebRequest.Get(serverURL + "/leaderboard/GetLeaderboard" + "?game=" + game))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving leaderboard: " + www.error);
            }
            else
            {
                string jsonData = www.downloadHandler.text;
                Debug.Log("Leaderboard retrieved successfully");
                callback(jsonData);
            }
        }
    }

    private IEnumerator AssignQuests()
    {
        Debug.Log("Assigning quests...");

        WWWForm form = new WWWForm();
        form.AddField("walletId", walletId);
        form.AddField("userName", userName);
        // form.AddField("token", token);
        form.AddField("gameId", game);
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL + "/quests/instances", form))

        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error assigning quests: " + www.error);
            }
            else
            {
                Debug.Log("Quests assigned successfully");
                string jsonData = www.downloadHandler.text;
                yield return jsonData;
            }
        }
    }

    private IEnumerator GetQuests(Action<String> callback)
    {
        Debug.Log("Getting quests...");
        // Debug.Log("WalletID in request: " + walletId + " Game: " + game);
        using (UnityWebRequest www = UnityWebRequest.Get(serverURL + "/quests/instances?walletId=" + walletId + "&gameId=" + game))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving quests: " + www.error);
            }
            else
            {
                string jsonData = www.downloadHandler.text;
                Debug.Log("Quests retrieved successfully");
                callback(jsonData);
            }
        }
    }

    public IEnumerator AssignThenFetchQuests(Action<String> callback)
    {
        yield return StartCoroutine(AssignQuests());
        yield return StartCoroutine(GetQuests(callback));
    }

    public IEnumerator UpdateQuests(int newScore)
    {
        Debug.Log("Patching quest instance...");

        Dictionary<string, string> requestBody = new Dictionary<string, string>()
            {
                {"walletId", walletId},
                {"gameId", game},
                {"newScore", newScore.ToString()}
            };

        string requestBodyString = JsonConvert.SerializeObject(requestBody);
        byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(requestBodyString);

        UnityWebRequest www = UnityWebRequest.Put(serverURL + "/quests/instances/games", requestBodyData);
        www.method = "PUT";
        www.SetRequestHeader("Content-Type", "application/json");

        using (www)
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error patching quest instance: " + www.error);
            }
            else
            {
                Debug.Log("Quest instance patched successfully");
                QuestModal.instance.ShowModal(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GameEndSequence(int currentScore)
    {   
        Debug.Log("Game Ended, Score: " + currentScore);
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        SendToFrontEnd("GameEnd: " + currentScore); 
        #endif
        
        if (token == "No Token")
        {
            Debug.Log("No token found, cannot update _score");
            yield break;
        }

        StartCoroutine(SetLeaderboard(currentScore));
        yield return new WaitForSeconds(1f);
        StartCoroutine(UpdateQuests(currentScore));
    }

}