import React from 'react';
import { SwaggerException, TodoItemsClient, TodoListDto, TodoListsClient } from '../web-api-client.ts';
import { TodoList } from './TodoList.js';

export const TodoLists = () => {
    const [todoLists, setTodoLists] = React.useState([]);
    const [newListTitle, setNewListTitle] = React.useState();
    const [errorMessage, setErrorMessage] = React.useState();

    React.useState(() => {
        fetchLists();
    }, []);

    function fetchLists() {
        new TodoListsClient().getTodoLists()
            .then((res) => {
                setTodoLists(res.lists);
            }, (rej) => {
                setTodoLists([]);
            });
    }

    const onAddNewItem = async (listId, title) => {
        await new TodoItemsClient().createTodoItem({
            listId,
            title,
        });
        fetchLists();
    };

    const onRemoveItem = async (item) => {
        await new TodoItemsClient().deleteTodoItem(item.id);
        fetchLists();
    };

    const onToggleItemDone = async (item) => {
        await new TodoItemsClient().updateTodoItem(item.id, {
            ...item,
            done: !item.done,
        });
        fetchLists();
    }
    
    const onUpdateListTitle = async (list, title) => {
        await new TodoListsClient().updateTodoList(list.id, {
            ...list,
            title,
        });
        fetchLists();
    }

    const onDeleteList = async (list) => {
        await new TodoListsClient().deleteTodoList(list.id);
        fetchLists();
    }

    const createNewList = async (title) => {
        try {
            await new TodoListsClient().createTodoList({
                title,
            });
            fetchLists();
        } catch (e) {
            if (SwaggerException.isSwaggerException(e)) {
                const response = JSON.parse(e.response);
                setErrorMessage(response?.errors?.Title?.[0]);
            }
        }
    };

    return <>
        <div className="mb-3">
            <div className="d-flex align-items-center">
                <input
                    onChange={(evt) => {
                        setNewListTitle(evt.target.value);
                        setErrorMessage();
                    }}
                    className="form-control w-auto flex-grow-1 d-inline me-2"
                    type="text"
                    placeholder="Add a new list..."
                    required
                ></input>
                <button type="button" onClick={() => createNewList(newListTitle)} className="btn btn-primary d-inline w-auto me-2">Add new list</button>
            </div>
            {errorMessage && errorMessage.length > 0 && <span className="text-danger">{errorMessage}</span>}
        </div>
        {todoLists.length > 0 ? 
            todoLists.map((list) => (
                <div className="mb-2">
                <TodoList
                    key={list.id}
                    list={list}
                    onAddNewItem={onAddNewItem}
                    onToggleItemDone={onToggleItemDone}
                    onRemoveItem={onRemoveItem}
                    onUpdateListTitle={onUpdateListTitle}
                    onDeleteList={onDeleteList}
                /></div>
            )) : (<>
                <p>No todo lists found!</p>
            </>)
        }
    </>;
}