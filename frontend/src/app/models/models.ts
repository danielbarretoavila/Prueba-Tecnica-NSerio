export interface Developer {
  developerId: number;
  fullName: string;
  email: string;
}

export interface Project {
  projectId: number;
  name: string;
  clientName: string;
  status: string;
  totalTasks: number;
  openTasks: number;
  completedTasks: number;
}

export interface Task {
  taskId: number;
  projectId: number;
  title: string;
  description?: string;
  assigneeId: number;
  assigneeName: string;
  status: string;
  priority: string;
  estimatedComplexity: number;
  dueDate: Date;
  completionDate?: Date;
  createdAt: Date;
}

export interface DeveloperWorkload {
  developerId: number;
  developerName: string;
  openTasksCount: number;
  averageEstimatedComplexity: number;
}

export interface ProjectHealth {
  projectId: number;
  projectName: string;
  clientName: string;
  projectStatus: string;
  totalTasks: number;
  openTasks: number;
  completedTasks: number;
}

export interface DeveloperDelayRisk {
  developerId: number;
  developerName: string;
  openTasksCount: number;
  avgDelayDays: number;
  nearestDueDate?: Date;
  latestDueDate?: Date;
  predictedCompletionDate?: Date;
  highRiskFlag: number;
}

export interface CreateTask {
  projectId: number;
  title: string;
  description?: string;
  assigneeId: number;
  status: string;
  priority: string;
  estimatedComplexity: number;
  dueDate: Date;
  completionDate?: Date;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
