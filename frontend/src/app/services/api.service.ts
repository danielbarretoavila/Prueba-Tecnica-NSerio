import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
    Developer,
    Project,
    Task,
    DeveloperWorkload,
    ProjectHealth,
    DeveloperDelayRisk,
    CreateTask,
    PagedResult
} from '../models/models';

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    private apiUrl = 'https://localhost:7291/api';

    constructor(private http: HttpClient) { }

    // Dashboard endpoints
    getDeveloperWorkload(): Observable<DeveloperWorkload[]> {
        return this.http.get<DeveloperWorkload[]>(`${this.apiUrl}/dashboard/developer-workload`);
    }

    getProjectHealth(): Observable<ProjectHealth[]> {
        return this.http.get<ProjectHealth[]>(`${this.apiUrl}/dashboard/project-health`);
    }

    getDeveloperDelayRisk(): Observable<DeveloperDelayRisk[]> {
        return this.http.get<DeveloperDelayRisk[]>(`${this.apiUrl}/dashboard/developer-delay-risk`);
    }

    // Projects endpoints
    getProjects(): Observable<Project[]> {
        return this.http.get<Project[]>(`${this.apiUrl}/projects`);
    }

    getProjectTasks(
        projectId: number,
        status?: string,
        assigneeId?: number,
        page: number = 1,
        pageSize: number = 10
    ): Observable<PagedResult<Task>> {
        let params = new HttpParams()
            .set('page', page.toString())
            .set('pageSize', pageSize.toString());

        if (status) {
            params = params.set('status', status);
        }

        if (assigneeId) {
            params = params.set('assigneeId', assigneeId.toString());
        }

        return this.http.get<PagedResult<Task>>(`${this.apiUrl}/projects/${projectId}/tasks`, { params });
    }

    // Tasks endpoints
    createTask(task: CreateTask): Observable<Task> {
        return this.http.post<Task>(`${this.apiUrl}/tasks`, task);
    }

    getTask(taskId: number): Observable<Task> {
        return this.http.get<Task>(`${this.apiUrl}/tasks/${taskId}`);
    }

    updateTaskStatus(taskId: number, status: string, priority?: string, complexity?: number): Observable<Task> {
        const body: any = { status };
        if (priority) body.priority = priority;
        if (complexity) body.estimatedComplexity = complexity;
        return this.http.put<Task>(`${this.apiUrl}/tasks/${taskId}/status`, body);
    }

    // Developers endpoints
    getDevelopers(): Observable<Developer[]> {
        return this.http.get<Developer[]>(`${this.apiUrl}/developers`);
    }
}
