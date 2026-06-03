import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Task } from '../../Models/task';
import { TaskService } from '../../Services/task.service';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './task.component.html',
  styleUrl: './task.component.css',
})
export class TaskComponent implements OnInit{
  @ViewChild('myModal') model: ElementRef | undefined;
  taskList: Task[] = [];
  taskService = inject(TaskService);
  taskForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) { }
  ngOnInit(): void {
    this.setFormState();
    this.getTaskList();
  }
openModal() {
    const taskModal = document.getElementById('myModal');
    if (taskModal != null) {
      taskModal.style.display = 'block';
    }
  }

  closeModal() {
    this.setFormState();
    if (this.model != null) {
      this.model.nativeElement.style.display = 'none';
    }

  }

  getTaskList() {
    this.taskService.getAllTasks().subscribe((res) => {
      this.taskList = res;
      console.log("task list values::"+this.taskList[0].taskId);
    })
  }

   setFormState() {
    this.taskForm = this.fb.group({
      taskId: [0],
      taskTitle: ['', [Validators.required]],
      taskDesc: ['', [Validators.required]],
      taskStatus: [false, [Validators.required]],
    });
  }

  formValues: any;
   onSubmit() {
    console.log(this.taskForm.value);
    if (this.taskForm.invalid) {
      alert('Please Fill All Fields');
      return;
    }
    if (this.taskForm.value.taskId == 0) {
      this.formValues = this.taskForm.value;
      this.taskService.addTask(this.formValues).subscribe((res) => {

        alert('Employee Added Successfully');
        this.getTaskList();
        this.taskForm.reset();
        this.closeModal();

      });
    } else {
      this.formValues = this.taskForm.value;
      this.taskService.updateTask(this.formValues).subscribe((res) => {

        alert('Employee Updated Successfully');
        this.getTaskList();
        this.taskForm.reset();
        this.closeModal();

      });
    }

  }

  OnEdit(task: Task) {
    this.openModal();
    this.taskForm.patchValue(task);
  }

   onDelete(task: Task) {
    const isConfirm = confirm("Are you sure you want to delete this task " + task.taskTitle);
    if (isConfirm) {
      this.taskService.deleteTask(task.taskId).subscribe((res) => {
        alert("Task Deleted Successfully");
        this.getTaskList();
      });
    }

  }

}
