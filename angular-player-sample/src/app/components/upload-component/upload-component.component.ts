import { Component, OnInit } from '@angular/core';
import { TrackService } from 'src/app/services/track.service';

@Component({
  selector: 'app-upload-component',
  templateUrl: './upload-component.component.html',
  styleUrls: ['./upload-component.component.scss']
})
export class UploadComponentComponent {
  constructor(private tracksService: TrackService) {}

  trackCode: string;

  async upload() {
    await this.tracksService.upload(this.trackCode);
    this.trackCode = '';
  }
}
