import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HttpApiService {
  baseUrl: string = "https://localhost:7239/";
  prodBaseUrl: string = "https://connect-individual.azurewebsites.net/";
  constructor(private httpClient: HttpClient) { }
  async onGetUser(id: string) {
    return await this.httpClient.get(this.prodBaseUrl + "Authentication/GetUserId?id=" + id).toPromise()
  }
  getUser(userId:string) {
    return this.httpClient.get(this.prodBaseUrl +"Authentication/GetUser?userId=" + userId)
  }
  getuserNotification(userId: string) {
    return this.httpClient.get(this.prodBaseUrl + "api/notifications/getPushNotificationByUser?userId=" + userId)
  }
}
