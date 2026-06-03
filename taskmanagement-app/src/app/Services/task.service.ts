import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Task } from '../Models/task';
@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private apiUrl = 'https://localhost:7197/api/TaskManagement'
  constructor() { }

  http = inject(HttpClient)

  getAllTasks() {
    return this.http.get<Task[]>(this.apiUrl);
  }

  addTask(data: any) {
    return this.http.post(this.apiUrl, data);
  }
  updateTask(task: Task) {
    return this.http.put(`${this.apiUrl}/${task.taskId}`, task)
  }
  deleteTask(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  
}
