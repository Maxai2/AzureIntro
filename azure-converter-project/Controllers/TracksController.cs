using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using azure_converter_project.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using VideoConverter.Api.Extensions;
using VideoConverter.Api.Models;
using VideoConverter.Api.Services;

namespace VideoConverter.Api.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase {
        private readonly ITracksService _tracksService;
        private readonly IYoutubeAudioExtractor _extractor;
        private readonly IHubContext<CustomHub, ITracksDisplay> _hub;

        public TracksController (ITracksService tracksService,
            IYoutubeAudioExtractor extractor,
            IHubContext<CustomHub, ITracksDisplay> hub) {
            _tracksService = tracksService;
            _extractor = extractor;
            _hub = hub;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> Get () {
            return (await _tracksService.Get ()).AsList ();
        }

        [HttpPost ("Info")]
        public async Task<IActionResult> Info ([FromBody] Track track) {
            await _tracksService.Add (track);
            await _hub.Clients.User(track.UserId).RecieveTrack(track);
            return Ok ("okay");
        }

        [HttpPost]
        public async Task<ActionResult<Track>> Upload ([FromBody] string code) {
            // var converted = await _extractor.Extract (code);
            // if (converted != null) {
            //     var t = await _tracksService.UploadTrackAsync (converted);
            //     if (t != null) {
            //         return Created (t.AudioUrl, t);
            //     }
            // }
            QueueClient client = new QueueClient ("Endpoint=sb://musicplayerservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=CTyqAAykXEnWSrJhriOUi4zzCAceMXycvnM8WH2ri98=", "convertqueue");
            var data = new {
                UserId = "test",
                Code = code
            };
            var json = JsonConvert.SerializeObject (data);
            var bytes = Encoding.UTF8.GetBytes (json);
            var msg = new Message (bytes);
            // msg.UserProperties.Add ("encoding", Encoding.UTF8.EncodingName);
            await client.SendAsync (msg);
            return Ok ("okay");
            // return BadRequest ("Unexpected internal fatal chto-to poshlo ne tak");
        }
    }
}