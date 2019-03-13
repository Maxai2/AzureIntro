import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent {

  connection: HubConnection;
  messages: string[] = [];
  msg: string;

  constructor() {
    const r = Math.random() > 0.5 ? 'Qwerty' : 'qwerty1';
    console.log(r);

    this.connection = new HubConnectionBuilder()
      .withUrl(`http://localhost:5000/chat?name=${r}`, {
        accessTokenFactory: () => localStorage.getItem('token')
      })
      .build();

      this.connection.on('ReceiveMessage', this.receiveMessage.bind(this));
      this.connection.start();
  }

  receiveMessage(message: string) {
    this.messages.push(message);
  }

  send() {
    this.connection.send('SendMessage', this.msg);
    this.msg = '';
  }
}
