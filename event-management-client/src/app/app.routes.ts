import { Routes } from '@angular/router';
import { EventList } from './features/event-list/event-list';
import { Login } from './features/login/login';

export const routes: Routes = [
    { path: '', component: Login },
    { path: 'events', component: EventList }
];
