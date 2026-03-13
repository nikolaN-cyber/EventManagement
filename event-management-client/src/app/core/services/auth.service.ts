import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { RegisterData, RegisterResponse, User } from "../../shared/models/user";


@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/Auth`;

    isLoading = signal(false);
    error = signal<string | null>(null);
    isSuccess = signal(false);

    login(credentials: { email: string, password: string }) {
        return this.http.post<User>(`${this.apiUrl}/login`, credentials);
    }

    register(data: RegisterData) {
        this.isLoading.set(true);
        this.error.set(null);
        return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, data)
        .subscribe({
            next: () => {
                this.isLoading.set(false);
                this.error.set(null);
            },
            error: (err) => {
                this.isLoading.set(false);
                this.error.set(err.message || "Error while creating user");
            }
        });
    }
}