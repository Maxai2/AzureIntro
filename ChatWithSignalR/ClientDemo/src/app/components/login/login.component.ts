import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  username = '';
  password = '';
  return = '';

  constructor(
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(
      params => (this.return = params.return || '/chat')
    );
  }

  // login() {
  //   if (this.username && this.password) {
  //     this.userService
  //       .login(this.username, this.password)
  //       .then(t => this.router.navigateByUrl(this.return));
  //   }
  // }

  async login() {
    if (this.username && this.password) {
      if (await this.userService.login(this.username, this.password))
        this.router.navigateByUrl(this.return);
    }
  }
}
