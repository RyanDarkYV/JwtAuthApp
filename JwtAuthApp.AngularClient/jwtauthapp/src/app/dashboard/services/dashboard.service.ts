import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { HomeDetails } from '../models/home.details.interface';
import { ConfigService } from '../../shared/utils/config.service';

import {BaseService} from '../../shared/services/base.service';

import { Observable } from 'rxjs/';

import { map, filter, switchMap } from 'rxjs/operators';

@Injectable()

export class DashboardService extends BaseService {

  baseUrl: string = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
     super();
     this.baseUrl = configService.getApiURI();
  }

  getHomeDetails(): Observable<HomeDetails> {
      let headers = new HttpHeaders();
      headers.append('Content-Type', 'application/json');
      let authToken = localStorage.getItem('auth_token');
      headers.append('Authorization', `Bearer ${authToken}`);
  
    return this.http.get<HomeDetails>(this.baseUrl + 'dashboard/home', {headers});
  }
}
