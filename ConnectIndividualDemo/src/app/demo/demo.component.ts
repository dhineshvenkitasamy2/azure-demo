import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpApiService } from '../http-api.service';


@Component({
  selector: 'app-demo',
  templateUrl: './demo.component.html',
  styleUrls: ['./demo.component.css']
})
export class DemoComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal: ElementRef | undefined;
  notifications: any;
  id: any = null;//eyJFbWFpbElkIjoiZC52ZW5raXRhc2FteUBuZnAuY29tIiwiTmFtZSI6IkRoaW5lc2ggVmVua2l0YXNhbXkiLCJQYXNzd29yZCI6IkRoaW51QDEyMyIsIlVzZXJJZCI6ImRoaW51MjAwMSJ9
  user: any = {};
  imageUrl: string = "../../assets/Images/CI.PNG";
  constructor(private route: ActivatedRoute, private httpApiService: HttpApiService) {

  }
  async ngOnInit() {
    this.id = this.route.snapshot.queryParams["id"];
    if (!!this.id) {
      this.user = await this.httpApiService.onGetUser(this.id);
      if (!!this.user) {
        localStorage.setItem("userId", JSON.stringify(this.user));
        this.notifications = await this.httpApiService.getuserNotification(this.user.userId).toPromise();
        this.notifications.forEach((notification: any) => {
          notification.content = JSON.parse(notification.content);
        })
      }
      if (this.randomIntFromInterval(1, 10) % 2 == 0) {
        this.imageUrl = "../../assets/Images/CI1.PNG";
      }      
    }
  }

  randomIntFromInterval(min: number, max: number) {
   
    return Math.floor(Math.random() * (max - min + 1) + min)
     }





  openModal() {
    if (!!this.modal)
      this.modal.nativeElement.style.display = 'block';
  }
  close() {
    if (!!this.modal)
      this.modal.nativeElement.style.display = 'none';
  }
}
