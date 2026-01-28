import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Developer, Project, CreateTask } from '../../models/models';

@Component({
    selector: 'app-new-task',
    templateUrl: './new-task.component.html',
    styleUrls: ['./new-task.component.scss']
})
export class NewTaskComponent implements OnInit {
    taskForm: FormGroup;
    projects: Project[] = [];
    developers: Developer[] = [];
    loading = false;
    submitted = false;
    errorMessage = '';

    PRIORITIES = ['Low', 'Medium', 'High'];
    STATUSES = ['ToDo', 'InProgress', 'Blocked', 'Completed'];

    constructor(
        private formBuilder: FormBuilder,
        private apiService: ApiService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.taskForm = this.formBuilder.group({
            projectId: ['', Validators.required],
            title: ['', [Validators.required, Validators.maxLength(100)]],
            description: [''],
            assigneeId: ['', Validators.required],
            status: ['ToDo', Validators.required],
            priority: ['Medium', Validators.required],
            estimatedComplexity: [1, [Validators.required, Validators.min(1), Validators.max(5)]],
            dueDate: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        this.loadProjects();
        this.loadDevelopers();

        // Check for query param projectId to pre-select
        this.route.queryParams.subscribe(params => {
            if (params['projectId']) {
                this.taskForm.patchValue({ projectId: +params['projectId'] });
            }
        });
    }

    loadProjects(): void {
        this.apiService.getProjects().subscribe({
            next: (projects) => this.projects = projects,
            error: (err) => console.error('Error loading projects', err)
        });
    }

    loadDevelopers(): void {
        this.apiService.getDevelopers().subscribe({
            next: (devs) => this.developers = devs,
            error: (err) => console.error('Error loading developers', err)
        });
    }

    get f() { return this.taskForm.controls; }

    onSubmit(): void {
        this.submitted = true;
        this.errorMessage = '';

        if (this.taskForm.invalid) {
            return;
        }

        this.loading = true;
        const formValue = this.taskForm.value;

        const newTask: CreateTask = {
            ...formValue,
            projectId: +formValue.projectId,
            assigneeId: +formValue.assigneeId,
            estimatedComplexity: +formValue.estimatedComplexity,
            dueDate: new Date(formValue.dueDate)
        };

        this.apiService.createTask(newTask).subscribe({
            next: () => {
                this.loading = false;
                // Navigate back to project tasks or dashboard
                this.router.navigate(['/projects', newTask.projectId]);
            },
            error: (error) => {
                this.loading = false;
                this.errorMessage = 'Ocurrió un error al crear la tarea. Por favor intente nuevamente.';
                console.error('Error creating task:', error);
            }
        });
    }

    onCancel(): void {
        this.router.navigate(['/']);
    }
}
