import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthStore } from '../../store/AuthStore';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authStore = inject(AuthStore);
  const token = authStore.currentUser()?.token;

  const excludedUrls = ['/api/Auth/login', '/api/Auth/register'];
  const isExcluded = excludedUrls.some(url => req.url.includes(url));

  if (token && !isExcluded) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  return next(req);
};