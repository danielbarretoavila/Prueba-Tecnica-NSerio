import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Task, Developer, PagedResult } from '../../models/models';
import { TableColumn } from '../../components/reusable-table/reusable-table.component';

@Component({
  selector: 'app-project-tasks',
  templateUrl: './project-tasks.component.html',
  styleUrls: ['./project-tasks.component.scss']
})
export class ProjectTasksComponent implements OnInit {
  projectId!: number;
  tasks: Task[] = [];
  developers: Developer[] = [];

  selectedStatus: string = '';
  selectedAssignee: number | undefined;
  currentPage: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  totalPages: number = 0;

  loading = true;
  selectedTask: Task | null = null;
  showTaskDetail = false;

  statuses = ['ToDo', 'InProgress', 'Blocked', 'Completed'];

  taskColumns: TableColumn[] = [
    { key: 'title', label: 'Título', sortable: true },
    { key: 'assigneeName', label: 'Asignado a', sortable: true },
    {
      key: 'status',
      label: 'Estado',
      sortable: true,
      type: 'badge',
      badgeClass: (value) => `status-${value.toLowerCase()}`
    },
    {
      key: 'priority',
      label: 'Prioridad',
      sortable: true,
      type: 'badge',
      badgeClass: (value) => `priority-${value.toLowerCase()}`
    },
    { key: 'estimatedComplexity', label: 'Complejidad', sortable: true, type: 'number' },
    { key: 'createdAt', label: 'Creada', sortable: true, type: 'date' },
    { key: 'dueDate', label: 'Vencimiento', sortable: true, type: 'date' }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.projectId = +params['id'];
      this.loadDevelopers();
      this.loadTasks();
    });
  }

  loadDevelopers(): void {
    this.apiService.getDevelopers().subscribe({
      next: (developers) => {
        this.developers = developers;
      },
      error: (error) => {
        console.error('Error loading developers:', error);
      }
    });
  }

  loadTasks(): void {
    this.loading = true;
    this.apiService.getProjectTasks(
      this.projectId,
      this.selectedStatus || undefined,
      this.selectedAssignee,
      this.currentPage,
      this.pageSize
    ).subscribe({
      next: (result: PagedResult<Task>) => {
        this.tasks = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading tasks:', error);
        this.loading = false;
      }
    });
  }

  onStatusChange(): void {
    this.currentPage = 1;
    this.loadTasks();
  }

  onAssigneeChange(): void {
    this.currentPage = 1;
    this.loadTasks();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadTasks();
  }

  onTaskClick(task: Task): void {
    this.selectedTask = task;
    this.showTaskDetail = true;
  }

  closeTaskDetail(): void {
    this.showTaskDetail = false;
    this.selectedTask = null;
  }

  navigateToNewTask(): void {
    this.router.navigate(['/tasks/new'], { queryParams: { projectId: this.projectId } });
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}
