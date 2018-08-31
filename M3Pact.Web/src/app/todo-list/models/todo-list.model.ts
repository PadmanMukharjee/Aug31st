export class TodoListModel {
    taskName: string
    clients: TodoListClient[];
}

export class TodoListClient {
    clientId: number;
    clientCode: string;
    clientName: string;
}



