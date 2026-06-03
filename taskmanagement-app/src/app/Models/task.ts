export interface Task {
    taskId: number;
    taskTitle: string;
    taskDesc: string;
    taskStatus: boolean; 
    createdDate: Date;
    updatedDate?: Date;
}
