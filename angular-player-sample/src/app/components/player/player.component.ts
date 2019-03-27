import { Component, OnInit } from '@angular/core';
import { TrackService } from 'src/app/services/track.service';
import { Track } from 'src/app/models/track';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit {
  position = 0;
  tracks: Array<Track> = [];
  audio = new Audio();

  _currentTrack: Track = new Track(0, '', '', '', '', '');

  public get currentTrack(): Track {
    return this._currentTrack;
  }
  public set currentTrack(v: Track) {
    this._currentTrack = v;
    this.audio.src = v.audioUrl;
  }

  currentTrackIndex = 0;

  get volume(): number {
    return this.audio.volume * 100;
  }
  set volume(value: number) {
    this.audio.volume = value / 100.0;
  }

  highlightSlider() {
    const slider = document.querySelector('.slider') as HTMLElement;
    let val = (this.audio.currentTime / this.audio.duration) * 100;
    val = parseInt((val * 100).toString(), 10) / 100.0;
    slider.style.background = `linear-gradient(to right, #000, #000 ${val}%, #d3d3d3 ${val}%)`;
  }

  constructor(private trackService: TrackService) {}

  async ngOnInit() {
    this.trackService.connect();
    this.tracks = await this.trackService.getAll();
    this.audio.ontimeupdate = () => {
      this.position = (this.audio.currentTime / this.audio.duration) * 3000;
      this.highlightSlider();
    };
  }
  play() {
    if (this.currentTrack === null) {
      this.currentTrack = this.tracks[this.currentTrackIndex];
    }
    this.audio.play();
  }
  pause() {
    this.audio.pause();
  }
  next() {
    this.currentTrackIndex++;
    if (this.currentTrackIndex >= this.tracks.length) {
      this.currentTrackIndex = 0;
    }
    this.currentTrack = this.tracks[this.currentTrackIndex];
  }
  prev() {
    this.currentTrackIndex--;
    if (this.currentTrackIndex < 0) {
      this.currentTrackIndex = this.tracks.length - 1;
    }
    this.currentTrack = this.tracks[this.currentTrackIndex];
  }
  drag(position: number) {
    this.audio.currentTime = (position * this.audio.duration) / 3000.0;
    this.highlightSlider();
  }
  select(track: Track) {
    this.audio.src = track.audioUrl;
    this.audio.load();
    this.audio.play();
  }
}
