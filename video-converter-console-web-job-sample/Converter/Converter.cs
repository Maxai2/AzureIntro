using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConverterApp.Interfaces;
using ConverterApp.Models;
using ConverterApp.Services;
using Jil;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace ConverterApp {
    public class Converter {
        private IAudioExtractor _audioExtractor;
        private IBlobService _blobService;
        private ILogger _logger;
        private ITracksApi _tracksApi;

        private QueueClient _listenerQueueClient;

        public Converter (
            IAudioExtractor audioExtractor,
            IBlobService blobService,
            ILogger logger,
            ITracksApi tracksApi,
            string serviceBusConnectionString,
            string queueName
        ) {

            _audioExtractor = audioExtractor;
            _blobService = blobService;
            _logger = logger;
            _tracksApi = tracksApi;

            _listenerQueueClient = new QueueClient (serviceBusConnectionString, queueName);
        }

        public async Task SubscribeAndListen () {
            _listenerQueueClient.RegisterMessageHandler (OnMessage, OnException);
            await Task.Yield ();
        }

        private async Task OnException (ExceptionReceivedEventArgs e) {
            _logger.Error(e.Exception.Message);
            await Task.Yield ();
        }

        private async Task OnMessage (Message message, CancellationToken cancellationToken) {
            try {
                _logger.Information ("New message");
                var text = Encoding.Default.GetString (message.Body);
                var videoQueueItem = JSON.Deserialize<VideoQueueItem> (text);
                _logger.Information ("Downloading");
                var convertedItem = await _audioExtractor.Extract (videoQueueItem.Code);
                _logger.Information ("Downloaded and converted");
                var url = await _blobService.Upload (convertedItem.Stream, null, "mp3");
                _logger.Information ("Uploading");

                await _tracksApi.AddTrack (new Track {
                    VideoUrl = convertedItem.VideoUrl,
                        AudioUrl = url,
                        Title = convertedItem.Title,
                        Album = null
                });
            } catch (Exception ex) {
                _logger.Error (ex.Message);
                throw;
            }
        }
    }
}