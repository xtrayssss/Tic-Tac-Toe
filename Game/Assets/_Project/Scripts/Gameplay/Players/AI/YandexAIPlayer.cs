using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using _Project.Scripts.Gameplay.Board;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;

namespace _Project.Scripts.Gameplay.Players.AI
{
    public class YandexAIPlayer : Player
    {
        private readonly bool _debugMode;
        private readonly string _apiKey;
        private readonly string _folderId;
        private readonly string _difficulty;
        private readonly float _timeout;
        private readonly float _moveDelay;

        private const string API_URL = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";

        public YandexAIPlayer(AIPlayerConfig config, BoardGenerator boardGenerator) : base(config, boardGenerator)
        {
            _debugMode = config.DebugMode;
            _apiKey = config.ApiKey;
            _folderId = config.FolderId;
            _difficulty = config.Difficulty;
            _timeout = config.Timeout;

            _moveDelay = config.MoveDelay;
        }

        public override async UniTask<int> GetMoveAsync(CancellationToken token)
        {
            string boardState = BoardGenerator.Board.ConvertBitboardToString();
            List<int> availableMoves = BoardGenerator.Board.GetFreeCells();

            YandexRequest request = new YandexRequest(boardState, BoardGenerator.Board.Size, _difficulty, availableMoves);

            string jsonRequest = JsonUtility.ToJson(request);

            var downloadHandler = new DownloadHandlerBuffer();
            var uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonRequest));

            if (_debugMode)
            {
                Debug.Log($"Request URL: {API_URL}");
                Debug.Log("Request Headers:");
                Debug.Log("Content-Type: application/json");
                Debug.Log(
                    $"Authorization: Api-Key {_apiKey.Substring(0, Math.Min(10, _apiKey.Length))}...");
                Debug.Log($"x-folder-id: {_folderId}");
                Debug.Log($"Request Body: {jsonRequest}");
            }

            using (UnityWebRequest webRequest = new UnityWebRequest(API_URL, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequest);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonBytes);
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                webRequest.downloadHandler = downloadHandler;
                webRequest.uploadHandler = uploadHandler;

                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", $"Api-Key {_apiKey}");
                webRequest.SetRequestHeader("x-folder-id", _folderId);
                webRequest.timeout = (int)math.round(_timeout);

                await webRequest.SendWebRequest();
                await UniTask.Delay(TimeSpan.FromSeconds(_moveDelay), cancellationToken: token);

                if (_debugMode)
                {
                    Debug.Log($"Response Code: {webRequest.responseCode}");
                    Debug.Log($"Response: {webRequest.downloadHandler?.text}");
                }

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string response = webRequest.downloadHandler.text;

                    YandexAIResponse yandexResponse = JsonUtility.FromJson<YandexAIResponse>(response);
                    if (yandexResponse?.result?.alternatives != null &&
                        yandexResponse.result.alternatives.Length > 0)
                    {
                        string moveStr = yandexResponse.result.alternatives[0].message.text.Trim();
                        if (int.TryParse(moveStr, out int move))
                        {
                            if (availableMoves.Contains(move))
                            {
                                return move + 1;
                            }

                            Debug.LogError(
                                $"Move {move} is not available. Available moves: {string.Join(", ", availableMoves)}");
                        }
                        else
                        {
                            Debug.LogError($"Invalid move format: {moveStr}");
                        }
                    }
                }

                // fallback
                return availableMoves.Any() ? availableMoves.First() : -1;
            }
        }
    }

    [Serializable]
    public class YandexRequest
    {
        public string modelUri;
        public CompletionOptions completionOptions;
        public MessageRequest[] messages;

        public YandexRequest(string boardState, int boardSize, string difficulty, List<int> availableMoves)
        {
            modelUri = "gpt://b1gldfe52f1j2ja6gtsr/yandexgpt-lite";
            completionOptions = new CompletionOptions
            {
                stream = false,
                temperature = 0.6f,
                maxTokens = "2000"
            };
            messages = new[]
            {
                new MessageRequest
                {
                    role = "system",
                    text = $@"You are playing Tic-Tac-Toe as 'X' on a {boardSize}x{boardSize} board. 
                The difficulty level is {difficulty}.
                Available moves: {string.Join(", ", availableMoves)}
                Your responses should be ONLY the cell number from the available moves list.
                The cells are numbered from left to right, top to bottom, starting from 1 to {boardSize * boardSize}.
                Analyze the board state carefully and make strategic moves based on the difficulty level.
                Victory conditions: {boardSize} in a row horizontally, vertically, or diagonally.
                
                For {difficulty} difficulty:
                - Easy: Make somewhat random moves, occasionally miss winning opportunities
                - Medium: Block obvious wins and take obvious winning moves
                - Hard: Play optimally, using the best strategic moves

                Respond only with one of these available moves: {string.Join(", ", availableMoves)}"
                },
                new MessageRequest
                {
                    role = "user",
                    text = $@"Current board state (O: player, X: AI, _: empty):
                {boardState}
                Make your move by responding with just a number from the available moves: {string.Join(", ", availableMoves)}"
                }
            };
        }
    }

    [Serializable]
    public class CompletionOptions
    {
         public bool stream;
        public float temperature;
        public string maxTokens;
    }

    [Serializable]
    public class MessageRequest
    {
        public string role;
        public string text;
    }

    [Serializable]
    public class YandexAIResponse
    {
        public Result result;
    }

    [Serializable]
    public class Result
    {
        public Alternative[] alternatives;
    }

    [Serializable]
    public class Alternative
    {
        public Message message;
    }

    [Serializable]
    public class Message
    {
        public string text;
    }
}