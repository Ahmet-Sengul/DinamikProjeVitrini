import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-personal-layout',
  templateUrl: './personal-layout.component.html',
  styleUrls: ['./personal-layout.component.css']
})
export class PersonalLayoutComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    console.log("Personal Worked")
  }

}
