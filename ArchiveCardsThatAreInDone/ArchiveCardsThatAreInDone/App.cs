using System.Threading;
using System.Threading.Tasks;
using ArchiveCardsThatAreInDone.Services;
using Microsoft.Extensions.Hosting;

namespace ArchiveCardsThatAreInDone
{
    public class App : IHostedService
    {
        private readonly ArchiveCardsService _restoreLostCardService;

        public App(ArchiveCardsService restoreLostCardService)
        {
            _restoreLostCardService = restoreLostCardService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _restoreLostCardService.Execute();

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}