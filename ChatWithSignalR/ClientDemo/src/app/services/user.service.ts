import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { isNullOrUndefined } from 'util';
import { TokenDto } from 'src/app/models/tokenDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public constructor(private http: HttpClient) {}

  public isValid(): boolean {
    return !isNullOrUndefined(localStorage.getItem('accessToken'));
  }

  public get userName(): string {
    return localStorage.getItem('username');
  }

  public get token(): string {
    return localStorage.getItem('accessToken');
  }

  public async login(username: string, password: string): Promise<boolean> {
    const url = `${environment.apiUrl}/account/login`;
    const body = {
      login: username,
      password
    };
    const options = {
      headers: new HttpHeaders().set('Content-Type', 'application/json')
    };
    return await this.http
      .post<TokenDto>(url, body, options)
      .toPromise()
      .then(t => {
        localStorage.setItem('refreshToken', t.refreshToken);
        localStorage.setItem('accessToken', t.accessToken);
        localStorage.setItem('username', username);
        return true;
      })
      .catch(e => false);
  }

  public async register(username: string, password: string): Promise<boolean> {
    const url = `${environment.apiUrl}/account/register`;
    const body = {
      login: username,
      password
    };
    const options = {
      headers: new HttpHeaders().set('Content-Type', 'application/json')
    };
    return await this.http
      .post<TokenDto>(url, body, options)
      .toPromise()
      .then(t => {
        localStorage.setItem('refreshToken', t.refreshToken);
        localStorage.setItem('accessToken', t.accessToken);
        localStorage.setItem('username', username);
        return true;
      })
      .catch(e => false);
  }

  public async refresh(): Promise<boolean> {
    const url = `${environment.apiUrl}/refresh`;
    const body = localStorage.getItem('refreshToken');
    const options = {
      headers: new HttpHeaders().set('Content-Type', 'application/json')
    };
    return await this.http
      .post<TokenDto>(url, body, options)
      .toPromise()
      .then(t => {
        localStorage.setItem('refreshToken', t.refreshToken);
        localStorage.setItem('accessToken', t.accessToken);
        return true;
      })
      .catch(e => false);
  }
}
