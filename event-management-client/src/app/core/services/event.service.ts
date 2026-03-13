import { inject, Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment.development";
import { Event } from "../../shared/models/event";
import { tap } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class EventService{
    private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/Event`;

    getAll() {
        return this.http.get<Event[]>(`${this.apiUrl}/all-events`);
    }
}