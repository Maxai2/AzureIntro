import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Track } from '../models/track';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TrackService {
  private apiUrl: string;
  private tracks: Array<Track> = null;

  constructor(private httpClient: HttpClient) {
    this.apiUrl = environment.apiUrl;
  }

  public async getAll(): Promise<Array<Track>> {
    if (this.tracks === null) {
      this.tracks = new Array<Track>();
      await this.httpClient
        .get<Array<Track>>(`${this.apiUrl}/api/tracks`)
        .subscribe(tracks => this.tracks.push(...tracks));
    }
    return this.tracks;
  }

  public async upload(url: string): Promise<void> {
    await this.httpClient
      .post<Track>(`${this.apiUrl}/api/tracks`, JSON.stringify(url), {
        headers: new HttpHeaders().append('Content-Type', 'application/json')
      })
      .subscribe(t => this.tracks.push(t));
  }
}
