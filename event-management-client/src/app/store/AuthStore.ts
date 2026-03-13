import { patchState, signalStore, withMethods, withState } from "@ngrx/signals";
import { RegisterData, User } from "../shared/models/user";
import { inject } from "@angular/core";
import { AuthService } from "../core/services/auth.service";
import { Router } from "@angular/router";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { pipe, switchMap, tap } from "rxjs";
import { tapResponse } from "@ngrx/operators";

export const AuthStore = signalStore(
    { providedIn: 'root' },
    withState({
        currentUser: JSON.parse(localStorage.getItem('user') || 'null') as User | null,
        isLoading: false
    }),
    withMethods((store, authService = inject(AuthService), router = inject(Router)) => ({
        login: rxMethod<{ email: string, password: string }>(
            pipe(
                tap(() => patchState(store, { isLoading: true })),
                switchMap((credentials) =>
                    authService.login(credentials).pipe(
                        tapResponse({
                            next: (response) => {
                                localStorage.setItem('user', JSON.stringify(response));

                                patchState(store, { currentUser: response });

                                router.navigate(['/events']);
                            },
                            error: (err) => {
                                patchState(store, { isLoading: false });
                                console.error('Login error:', err);
                                alert('Invalid credentials');
                            }
                        })
                    )
                )
            )
        )
    }))
);