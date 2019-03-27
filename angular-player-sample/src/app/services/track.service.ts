import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Track } from '../models/track';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class TrackService {
  private apiUrl: string;
  private tracks: Array<Track> = null;
  private connection: HubConnection;

  constructor(private httpClient: HttpClient) {
    this.apiUrl = environment.apiUrl;
  }

  public connect() {
    const url = `${environment.apiUrl}/hub`;

    this.connection = new HubConnectionBuilder().withUrl(url).build();
    this.connection.on('RecieveTrack', this.RecieveTrack.bind(this));
    this.connection.start();
  }

  private RecieveTrack(track: Track) {
      this.tracks.push(track);
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
      });
    }
  }

  // .subscribe(t => this.tracks.push(t))
