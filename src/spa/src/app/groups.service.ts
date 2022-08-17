import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Group } from './group';

import { protectedResources } from './auth-config';
import { Invitation } from './invitation';

@Injectable({
  providedIn: 'root'
})
export class GroupsService {

  url = protectedResources.groupsApi.endpoint;

  constructor(private http: HttpClient) { }

  getGroups() {
    return this.http.get<Group[]>(this.url);
  }

  invite(invitation: Invitation){
    console.log("Send data to:"+ this.url + '/invite');
    console.log("Data: " + JSON.stringify(invitation));
    return this.http
      .post<Invitation>(this.url + '/invite', invitation)
      .subscribe();
  }
}
