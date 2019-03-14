import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { MainWindowComponent } from './components/main-window/main-window.component';
import { EditNewDocComponent } from './components/edit-new-doc/edit-new-doc.component';

@NgModule({
  declarations: [
    AppComponent,
    MainWindowComponent,
    EditNewDocComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
