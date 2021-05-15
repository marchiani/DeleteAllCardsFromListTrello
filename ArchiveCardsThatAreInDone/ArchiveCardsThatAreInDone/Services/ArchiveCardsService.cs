using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ArchiveCardsThatAreInDone.Models;
using Newtonsoft.Json;

namespace ArchiveCardsThatAreInDone.Services
{
    public class ArchiveCardsService
    {
        static readonly HttpClient _httpClient = new HttpClient();
        private const string _token = "";
        private const string _key = "";
        private const string _boardName = "";
        private const string _boardListName = "Done";

        public async Task Execute()
        {
            try
            {
                var getBoardQuery =
                    $"https://api.trello.com/1/members/me/boards?fields=name,url&key={_key}&token={_token}";
                var boardResponse = await _httpClient.GetAsync(getBoardQuery);
                if (boardResponse.IsSuccessStatusCode)
                {
                    var boardResponseContent = await boardResponse.Content.ReadAsStringAsync();
                    var boards = JsonConvert.DeserializeObject<List<BoardResponse>>(boardResponseContent);
                    var board = boards.FirstOrDefault(b => b.name == _boardName);
                    Console.WriteLine($"Find board {board.name}");

                    var getBoardList = $"https://api.trello.com/1/boards/{board.id}/lists/?&key={_key}&token={_token}";
                    var boardListResponse = await _httpClient.GetAsync(getBoardList);
                    if (boardListResponse.IsSuccessStatusCode)
                    {
                        var boardListResponseContent = await boardListResponse.Content.ReadAsStringAsync();
                        var boardLists = JsonConvert.DeserializeObject<List<BoardListResponse>>(boardListResponseContent);
                        var boardList = boardLists.FirstOrDefault(bl => bl.name.Contains(_boardListName));
                        Console.WriteLine($"Find board list {boardList.name}");

                        var getCardsInListQuery = $"https://api.trello.com/1/lists/{boardList.id}/cards?&key={_key}&token={_token}";
                        var cardsResponse = await _httpClient.GetAsync(getCardsInListQuery);
                        if (cardsResponse.IsSuccessStatusCode)
                        {
                            var cardsResponseContent = await cardsResponse.Content.ReadAsStringAsync();
                            var cards = JsonConvert.DeserializeObject<List<CardResponse>>(cardsResponseContent);

                            foreach (var card in cards.Where(c => c.name != "Done"))
                            {
                                var cardArchiveQuery =
                                    $"https://api.trello.com/1/cards/{card.id}?closed=true&key={_key}&token={_token}";
                                await _httpClient.PutAsync(cardArchiveQuery, null);

                                Console.WriteLine($"Card {card.name} was archived");
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}