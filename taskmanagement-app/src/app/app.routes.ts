import { Routes } from '@angular/router';
import { TaskComponent } from './Components/task/task.component';

export const routes: Routes = [
    {
        path: "", component:TaskComponent
    },
    {
        path: "task", component:TaskComponent
    }
];
