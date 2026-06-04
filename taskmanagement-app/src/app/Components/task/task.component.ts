import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { Task } from '../../Models/task';
import { TaskService } from '../../Services/task.service';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './task.component.html',
  styleUrl: './task.component.css',
})
export class TaskComponent implements OnInit{
  @ViewChild('myModal') model: ElementRef | undefined;
  taskList: Task[] = [];
  filteredTaskList: Task[] = [];
  selectedFilter: string = 'All';
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
      this.filterTasks();
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

        alert('Task Added Successfully');
        this.getTaskList();
        this.taskForm.reset();
        this.closeModal();

      });
    } else {
      this.formValues = this.taskForm.value;
      this.taskService.updateTask(this.formValues).subscribe((res) => {

        alert('Task Updated Successfully');
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

  filterTasks() {
    if (this.selectedFilter === 'Active') {
      this.filteredTaskList = this.taskList.filter(t => t.taskStatus === true);
    } else if (this.selectedFilter === 'Completed') {
      this.filteredTaskList = this.taskList.filter(t => t.taskStatus === false);
    } else {
      this.filteredTaskList = [...this.taskList];
    }
  }
}
