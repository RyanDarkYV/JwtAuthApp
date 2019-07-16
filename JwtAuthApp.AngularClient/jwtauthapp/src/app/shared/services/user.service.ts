import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { HttpClient , HttpHeaders } from '@angular/common/http';

import { UserRegistration } from '../models/user.registration.interface';
import { ConfigService } from '../utils/config.service';

import {BaseService} from './base.service';

import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/';
import { map, filter, switchMap, catchError } from 'rxjs/operators';

@Injectable()

export class UserService extends BaseService {

  baseUrl: string = '';

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(private http: HttpClient, private configService: ConfigService) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    this._authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getApiURI();
  }

    register(email: string, password: string, firstName: string, lastName: string, location: string): Observable<any> {
    let body = JSON.stringify({ email, password, firstName, lastName, location });
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.post(this.baseUrl + '/accounts', body, {headers: headers})
      .pipe(map(res => true));
  }

   login(userName, password) {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');

    return this.http
      .post(this.baseUrl + '/auth/login', JSON.stringify({ userName, password }), { headers })
      .pipe(map(res => {
        localStorage.setItem('auth_token', res['auth_token']);
        this.loggedIn = true;
        this._authNavStatusSource.next(true);
        return true;
      }))
      .pipe(catchError(this.handleError));
  }

  logout() {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
    this._authNavStatusSource.next(false);
  }

  isLoggedIn() {
    return this.loggedIn;
  }
}
