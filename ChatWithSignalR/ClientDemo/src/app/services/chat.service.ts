import { environment } from './../../environments/environment';
import { UserService } from 'src/app/services/user.service';
import { Message } from './../models/message';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { MsalService } from '@azure/msal-angular';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private messages = new Array<Message>();
  private connection: HubConnection;
  private room: string;
  constructor(
    private userService: UserService,
    private authService: MsalService
    ) { }

  public connect(room: string) {
    this.room = room;
    const url = `${environment.apiUrl}/hub?room=${this.room}`;

    this.connection = new HubConnectionBuilder()
      .withUrl(url, {
        // accessTokenFactory: () => this.userService.token
        accessTokenFactory: () => {
          return this.authService.getCachedTokenInternal([]).token;
        }
      })
      .build();
    this.connection.on('ReceiveMessage', this.receiveMessage.bind(this));
    this.connection.start();
  }

  public getMessages(): Array<Message> {
    return this.messages;
  }

  private receiveMessage(message: Message) {
    this.messages.push(message);
  }

  public sendMessage(message: string) {
    const msg = new Message(0, message, this.userService.userName);
    this.connection.send('SendMessage', msg, this.room);
  }
}
