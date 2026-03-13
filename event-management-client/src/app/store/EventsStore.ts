import { signalStore, withState, withMethods, patchState, withComputed } from '@ngrx/signals';
import { computed, inject } from '@angular/core';
import {tapResponse} from '@ngrx/operators';
import { Event as MyEvent } from '../shared/models/event';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { EventService } from '../core/services/event.service';
import { pipe, switchMap, tap } from 'rxjs';


export const EventsStore = signalStore(
    {providedIn: 'root'},
    withState({
        items: [] as MyEvent[],
        loading: false,
        error: null as string | null,
        searchQuery: '',
        sortBy: 'name' as string
    }),
    withComputed(({items, searchQuery, sortBy}) => ({
        filteredEvents: computed(() => {
            const query = searchQuery().toLowerCase().trim();
            const field = sortBy();
            
            let result = items().filter(e => e.name.toLowerCase().includes(query) || e.location.toLowerCase().includes(query));

            return [...result].sort((a, b) => {
            if (field === 'date') {
                return new Date(a.dateAndTime).getTime() - new Date(b.dateAndTime).getTime();
            }
            
            const valA = (a as any)[field] || '';
            const valB = (b as any)[field] || '';
            return valA.localeCompare(valB);
        });
        })
    })),
    withMethods((store, eventsService = inject(EventService)) => ({
        loadAll: rxMethod<void>(
            pipe(
                tap(() => patchState(store, {loading: true, error: null})),
                switchMap(() =>
                    eventsService.getAll().pipe(
                        tapResponse({
                            next: (events) => patchState(store, {items: events, loading: false}),
                            error: (err: any) => patchState(store, {
                                error: err.message || 'Error',
                                loading: false
                            })
                        }),
                    ) 
                )
            )
        ),
        updateSearch: (query: string) => patchState(store, {searchQuery: query}),
        updateSort: (sort: 'name' | 'date' | 'location') => patchState(store, {sortBy: sort}),
    }))
);