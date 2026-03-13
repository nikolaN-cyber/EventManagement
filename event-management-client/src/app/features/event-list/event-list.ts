import { Component, inject, OnInit } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Event } from '../../shared/models/event';
import { EventsStore } from '../../store/EventsStore';

@Component({
  selector: 'app-event-list',
  imports: [
    CommonModule,
    RouterModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './event-list.html',
  styleUrl: './event-list.css',
})
export class EventList implements OnInit { 
  readonly store = inject(EventsStore);

  ngOnInit() {
    console.log("Call load")
    this.store.loadAll();
  }

  onSearch(event: any) {
    const value = (event.target as HTMLInputElement).value;
    this.store.updateSearch(value);
  }

  onSelection(event: any){
    const value = event.value;
    this.store.updateSort(value);
  }
}
