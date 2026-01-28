import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

export interface TableColumn {
  key: string;
  label: string;
  sortable?: boolean;
  type?: 'text' | 'number' | 'date' | 'badge';
  badgeClass?: (value: any, row: any) => string;
  format?: (value: any, row: any) => string;
}

@Component({
  selector: 'app-reusable-table',
  templateUrl: './reusable-table.component.html',
  styleUrls: ['./reusable-table.component.scss']
})
export class ReusableTableComponent implements OnInit {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() highlightRow?: (row: any) => boolean;
  @Input() clickable: boolean = false;
  @Output() rowClick = new EventEmitter<any>();

  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  ngOnInit(): void {
  }

  onSort(column: TableColumn): void {
    if (!column.sortable) return;

    if (this.sortColumn === column.key) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column.key;
      this.sortDirection = 'asc';
    }

    this.data.sort((a, b) => {
      const aVal = this.getNestedValue(a, column.key);
      const bVal = this.getNestedValue(b, column.key);

      if (aVal === bVal) return 0;

      const comparison = aVal > bVal ? 1 : -1;
      return this.sortDirection === 'asc' ? comparison : -comparison;
    });
  }

  getNestedValue(obj: any, path: string): any {
    return path.split('.').reduce((acc, part) => acc && acc[part], obj);
  }

  getCellValue(row: any, column: TableColumn): any {
    const value = this.getNestedValue(row, column.key);

    if (column.format) {
      return column.format(value, row);
    }

    if (column.type === 'date' && value) {
      return new Date(value).toLocaleDateString();
    }

    return value ?? '-';
  }

  getBadgeClass(row: any, column: TableColumn): string {
    if (column.badgeClass) {
      return column.badgeClass(this.getNestedValue(row, column.key), row);
    }
    return '';
  }

  shouldHighlight(row: any): boolean {
    return this.highlightRow ? this.highlightRow(row) : false;
  }

  onRowClick(row: any): void {
    if (this.clickable) {
      this.rowClick.emit(row);
    }
  }
}
