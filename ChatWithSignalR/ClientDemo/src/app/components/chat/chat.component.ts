import { UserService } from 'src/app/services/user.service';
import { ChatService } from './../../services/chat.service';
import { Message } from './../../models/message';
import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent {
  messages = new Array<Message>();
  text = '';
  username = '';
  room = '';
  connected = false;

  constructor(
    private chatService: ChatService,
    private userService: UserService
  ) {
    this.messages = this.chatService.getMessages();
    this.username = this.userService.userName;
  }

  public send(): void {
    this.chatService.sendMessage(this.text);
    this.text = '';
  }

  public connect() {
    this.chatService.connect(this.room);
    this.connected = true;
  }
}
