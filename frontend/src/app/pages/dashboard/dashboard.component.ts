import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { DeveloperWorkload, ProjectHealth, DeveloperDelayRisk } from '../../models/models';
import { TableColumn } from '../../components/reusable-table/reusable-table.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  developerWorkload: DeveloperWorkload[] = [];
  projectHealth: ProjectHealth[] = [];
  developerDelayRisk: DeveloperDelayRisk[] = [];

  loading = true;
  stats = {
    totalOpenTasks: 0,
    activeProjects: 0,
    highRiskDevs: 0
  };

  // Table columns for developer workload
  workloadColumns: TableColumn[] = [
    { key: 'developerName', label: 'Desarrollador', sortable: true },
    { key: 'openTasksCount', label: 'Tareas Abiertas', sortable: true, type: 'number' },
    {
      key: 'averageEstimatedComplexity',
      label: 'Complejidad Promedio',
      sortable: true,
      type: 'number',
      format: (value) => value ? value.toFixed(2) : '0.00'
    }
  ];

  // Table columns for project health
  healthColumns: TableColumn[] = [
    { key: 'projectName', label: 'Proyecto', sortable: true },
    { key: 'clientName', label: 'Cliente', sortable: true },
    { key: 'totalTasks', label: 'Total Tareas', sortable: true, type: 'number' },
    { key: 'openTasks', label: 'Abiertas', sortable: true, type: 'number' },
    { key: 'completedTasks', label: 'Completadas', sortable: true, type: 'number' }
  ];

  // Table columns for delay risk
  riskColumns: TableColumn[] = [
    { key: 'developerName', label: 'Desarrollador', sortable: true },
    { key: 'openTasksCount', label: 'Tareas Abiertas', sortable: true, type: 'number' },
    {
      key: 'avgDelayDays',
      label: 'Promedio Retraso (días)',
      sortable: true,
      type: 'number',
      format: (value) => value ? value.toFixed(1) : '0.0'
    },
    {
      key: 'nearestDueDate',
      label: 'Próximo Vencimiento',
      sortable: true,
      type: 'date'
    },
    {
      key: 'latestDueDate',
      label: 'Último Vencimiento',
      sortable: true,
      type: 'date'
    },
    {
      key: 'predictedCompletionDate',
      label: 'Fecha Estimada',
      sortable: true,
      type: 'date'
    },
    {
      key: 'highRiskFlag',
      label: 'Riesgo',
      sortable: true,
      type: 'badge',
      format: (value) => value === 1 ? 'Alto' : 'Bajo',
      badgeClass: (value) => value === 1 ? 'risk-high' : 'risk-low'
    }
  ];

  constructor(
    private apiService: ApiService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.loading = true;

    Promise.all([
      this.apiService.getDeveloperWorkload().toPromise(),
      this.apiService.getProjectHealth().toPromise(),
      this.apiService.getDeveloperDelayRisk().toPromise()
    ]).then(([workload, health, risk]) => {
      this.developerWorkload = workload || [];
      this.projectHealth = health || [];
      this.developerDelayRisk = risk || [];

      this.calculateStats();
      this.loading = false;
    }).catch(error => {
      console.error('Error loading dashboard data:', error);
      this.loading = false;
    });
  }

  calculateStats(): void {
    this.stats.totalOpenTasks = this.projectHealth.reduce((acc, curr) => acc + curr.openTasks, 0);
    this.stats.activeProjects = this.projectHealth.length;
    this.stats.highRiskDevs = this.developerDelayRisk.filter(d => d.highRiskFlag === 1).length;
  }

  highlightProjectRow(row: ProjectHealth): boolean {
    return row.openTasks > row.completedTasks;
  }

  highlightRiskRow(row: DeveloperDelayRisk): boolean {
    return row.highRiskFlag === 1;
  }

  onProjectClick(project: ProjectHealth): void {
    this.router.navigate(['/projects', project.projectId]);
  }

  navigateToNewTask(): void {
    this.router.navigate(['/tasks/new']);
  }
}
