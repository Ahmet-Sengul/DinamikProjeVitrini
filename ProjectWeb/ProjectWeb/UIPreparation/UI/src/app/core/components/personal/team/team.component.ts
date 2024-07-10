import { Component, OnInit } from '@angular/core';
import { TeamService } from '../../admin/team/services/team.service';
import { Team } from '../../admin/team/models/Team';
import { MemberService } from '../../admin/member/services/member.service';
import { Member } from '../../admin/member/models/Member';

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.css']
})
export class TeamComponent implements OnInit {
  rawxi9x: string = ' '
  rawtd7o: string = ' '
  raw2d9c: string = ' '
  constructor(private teamService:TeamService, private memberService: MemberService) { }
  teamList: Member[];
  teamListToShow: Member[];
  showAllMembers = false
  

  ngOnInit(): void {
    this.getTeamList();
  }
  
  getTeamList() {
		this.memberService.getMemberList().subscribe(data => {
			this.teamList = data;
      this.teamListToShow = this.teamList.slice(0, 4);
      this.showAllMembers = false;
		});
	}

  showAllTeamMembers() {
    this.teamListToShow = this.teamList;
    this.showAllMembers = true;
  }

}
