import { Component } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-main-window',
  templateUrl: './main-window.component.html',
  styleUrls: ['./main-window.component.less']
})

export class MainWindowComponent {

  connetion: HubConnection;
  fileName: string;

  constructor() { }

  editDoc() {
    this.connetion = new HubConnectionBuilder().withUrl(`http://localhost:5000?`)

  }

}
