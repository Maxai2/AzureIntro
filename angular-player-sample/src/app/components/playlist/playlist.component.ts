import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Track } from 'src/app/models/track';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.scss']
})
export class PlaylistComponent implements OnInit {
  @Input()
  tracks: Array<Track> = [];
  @Input()
  currentTrack: Track = null;
  @Output()
  selected = new EventEmitter<Track>();
  constructor() {}
  ngOnInit() {}
}
