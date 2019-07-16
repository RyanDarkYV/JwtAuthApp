import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {

    apiURI: string;

    constructor() {
        this.apiURI = 'http://localhost:5000/api';
     }

     getApiURI() {
         return this.apiURI;
     }
}
