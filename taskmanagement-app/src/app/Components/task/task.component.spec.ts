import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { of } from 'rxjs';
import { TaskComponent } from './task.component';
import { TaskService } from '../../Services/task.service';
import { Task } from '../../Models/task';

describe('TaskComponent', () => {
  let component: TaskComponent;
  let fixture: ComponentFixture<TaskComponent>;
  let mockTaskService: jasmine.SpyObj<TaskService>;

  const dummyTasks: Task[] = [
    { taskId: 1, taskTitle: 'Task A', taskDesc: 'Desc1', taskStatus: true, createdDate: new Date() },
    { taskId: 2, taskTitle: 'Task B', taskDesc: 'Desc2', taskStatus: false, createdDate: new Date() },
    { taskId: 3, taskTitle: 'Task C', taskDesc: 'Desc3', taskStatus: true, createdDate: new Date() }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('TaskService', ['getAllTasks', 'addTask', 'updateTask', 'deleteTask']);

    await TestBed.configureTestingModule({
      imports: [TaskComponent, ReactiveFormsModule, FormsModule, CommonModule],
      providers: [
        { provide: TaskService, useValue: spy }
      ]
    }).compileComponents();

    mockTaskService = TestBed.inject(TaskService) as jasmine.SpyObj<TaskService>;
  });

  beforeEach(() => {
    mockTaskService.getAllTasks.and.returnValue(of(dummyTasks));
    
    fixture = TestBed.createComponent(TaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges(); 
  });

  it('should create the component instance', () => {
    expect(component).toBeTruthy();
  });

  it('should get all tasks', () => {
    expect(mockTaskService.getAllTasks).toHaveBeenCalled();
    expect(component.taskList.length).toBe(3);
    expect(component.filteredTaskList.length).toBe(3);
  });

  it('should validate form field requirements correctly', () => {
    const titleControl = component.taskForm.controls['taskTitle'];
    titleControl.setValue('');
    expect(titleControl.hasError('required')).toBeTrue();

    titleControl.setValue('Valid Task Title');
    expect(titleControl.hasError('required')).toBeFalse();
  });

  it('should execute addTask service on new task submission', () => {
    component.taskForm.setValue({
      taskId: 0,
      taskTitle: 'New Created Task',
      taskDesc: 'New Task Description',
      taskStatus: false
    });

    const mockResponseTask: Task = { taskId: 4, taskTitle: 'New Created Task', taskDesc: 'New Task Description', taskStatus: false, createdDate: new Date() };
    mockTaskService.addTask.and.returnValue(of(mockResponseTask));
    spyOn(window, 'alert');

    component.onSubmit();

    expect(mockTaskService.addTask).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('Task Added Successfully');
  });

  it('should map chosen record values directly to the form on OnEdit invocation', () => {
    const taskToEdit = dummyTasks[0];
    spyOn(component, 'openModal');

    component.OnEdit(taskToEdit);

    expect(component.openModal).toHaveBeenCalled();
    expect(component.taskForm.value.taskId).toBe(1);
    expect(component.taskForm.value.taskTitle).toBe('Task A');
  });

  it('should trigger deleteTask service when confirmation prompt is accepted', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(window, 'alert');
    mockTaskService.deleteTask.and.returnValue(of({}));

    component.onDelete(dummyTasks[0]);

    expect(mockTaskService.deleteTask).toHaveBeenCalledWith(1);
    expect(window.alert).toHaveBeenCalledWith('Task Deleted Successfully');
  });

  it('should cancel deletion process if confirmation prompt is declined', () => {
    spyOn(window, 'confirm').and.returnValue(false);

    component.onDelete(dummyTasks[0]);

    expect(mockTaskService.deleteTask).not.toHaveBeenCalled();
  });
});