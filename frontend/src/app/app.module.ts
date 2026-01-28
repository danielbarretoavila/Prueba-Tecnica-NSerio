import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReusableTableComponent } from './components/reusable-table/reusable-table.component';
import { RelativeDatePipe } from './pipes/relative-date.pipe';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ProjectTasksComponent } from './pages/project-tasks/project-tasks.component';
import { NewTaskComponent } from './pages/new-task/new-task.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    ReusableTableComponent,
    RelativeDatePipe,
    DashboardComponent,

    ProjectTasksComponent,
    NewTaskComponent
  ],
  imports: [
    BrowserModule,

    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
