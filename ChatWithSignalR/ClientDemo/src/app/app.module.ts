import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MsalModule, MsalGuard, MsalInterceptor } from '@azure/msal-angular';

import { AppComponent } from './app.component';
import { ChatComponent } from './components/chat/chat.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { RouterModule } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { ChatGuard } from './guards/chat.guard';

@NgModule({
  declarations: [
    AppComponent,
    ChatComponent,
    LoginComponent,
    RegisterComponent,
    UserListComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MsalModule.forRoot({
      clientID: '1e76b1fd-8b99-4f46-81c4-75f0021d2ea7',
      authority: 'https://chatSignalRDomain.b2clogin.com/tfp/chatSignalRDomain.onmicrosoft.com/B2C_1_SignUpSignIn',
      validateAuthority: false,
      popUp: true,
      cacheLocation: 'localStorage',
      redirectUri: 'http://localhost:4200',
      navigateToLoginRequestUrl: true
    }),
    RouterModule.forRoot([
      // { path: 'login', component: LoginComponent },
      // { path: 'register', component: RegisterComponent },
      // { path: 'chat', component: ChatComponent, canActivate: [ChatGuard] },
      { path: 'chat', component: ChatComponent, canActivate: [MsalGuard] },
      { path: '**', redirectTo: 'chat' }
    ])
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: MsalInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule {}
