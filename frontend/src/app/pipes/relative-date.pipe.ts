import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'relativeDate'
})
export class RelativeDatePipe implements PipeTransform {
  transform(value: Date | string | undefined | null): string {
    if (!value) return '-';

    const date = new Date(value);
    const now = new Date();
    const diffMs = date.getTime() - now.getTime();
    const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));

    if (diffDays === 0) {
      return 'Hoy';
    } else if (diffDays === 1) {
      return 'Mañana';
    } else if (diffDays === -1) {
      return 'Ayer';
    } else if (diffDays > 0 && diffDays <= 7) {
      return `En ${diffDays} días`;
    } else if (diffDays < 0 && diffDays >= -7) {
      return `Hace ${Math.abs(diffDays)} días`;
    } else if (diffDays > 7) {
      return `En ${Math.floor(diffDays / 7)} semanas`;
    } else {
      return `Hace ${Math.floor(Math.abs(diffDays) / 7)} semanas`;
    }
  }
}
