import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Group } from '../group';
import { GroupsService } from '../groups.service';
import { Invitation } from '../invitation';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent implements OnInit {

  groups: Group[] = [];
  selectedGroup?: Group;
  selectedGroups?: Group[] = [];

  invitation: Invitation = {
    name: "",
    email: "",
    groupId: this.selectedGroup === undefined ? "" : this.selectedGroup.id
  };

  constructor(private service: GroupsService) { }

  ngOnInit(): void {
    this.getGroups();
  }

  getGroups(): void {
    this.service.getGroups()
      .subscribe((groups: Group[]) => {
        this.groups = groups;
        this.selectGroup(groups[0])
      });
  }

  groupChanged(groups: Group[]): void {
    this.selectGroup(groups[0]);
  }

  selectGroup(group: Group)
  {
    this.selectedGroup = group;
    this.invitation.groupId = group.id;
  }

  invite(invitation: NgForm): void {
    console.log(invitation.value);
    this.service.invite(invitation.value);

    invitation.resetForm();
  }
}
