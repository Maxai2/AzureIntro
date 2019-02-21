using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoConverter.Api.Extensions;
using VideoConverter.Api.Models;
using VideoConverter.Api.Services;

namespace VideoConverter.Api.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase {
        private readonly ITracksService _tracksService;
        private readonly IYoutubeAudioExtractor _extractor;
        public TracksController (ITracksService tracksService,
            IYoutubeAudioExtractor extractor) {
            _tracksService = tracksService;
            _extractor = extractor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> Get () {
            return (await _tracksService.Get ()).AsList ();
        }

        [HttpPost]
        public async Task<ActionResult<Track>> Upload ([FromBody] string code) {
            var converted = await _extractor.Extract (code);
            if (converted != null) {
                var t = await _tracksService.UploadTrackAsync (converted);
                if (t != null) {
                    return Created (t.AudioUrl, t);
                }
            }
            return BadRequest ("Unexpected internal fatal chto-to poshlo ne tak");
        }
    }
}