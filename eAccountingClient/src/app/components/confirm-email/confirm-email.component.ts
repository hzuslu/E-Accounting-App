import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpService } from '../../services/http.service';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [],
  template: `
  <div class="confirmation-container">
    <h1 class="confirmation-title">{{ response }}</h1>
    <a href="/login">Back to login page</a>
  </div>
`,
})
export class ConfirmEmailComponent {
  email: string = "";
  response: string = "";

  constructor(
    private activated: ActivatedRoute,
    private http: HttpService
  ) {
    this.activated.params.subscribe(res => {
      this.email = res["email"];
      this.confirm();
    })
  }

  confirm() {
    this.http.post<string>("Auth/ConfirmEmail", { email: this.email }, (res => { this.response = res }))
  }
}
