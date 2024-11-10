import React from 'react';

export const TodoList = ({ list, onToggleItemDone, onAddNewItem, onRemoveItem, onUpdateListTitle, onDeleteList }) => {
    const [newItemTitle, setNewItemTitle] = React.useState();
    const [isEditTitle, setIsEditTitle] = React.useState(false);
    const [newListTitle, setNewListTitle] = React.useState();
    const [error, setError] = React.useState(false);
    
    const submitNewItem = () => {
        if (!newItemTitle || newItemTitle === "") {
            setError(true);
        }
        else {
            onAddNewItem(list.id, newItemTitle);
        }
    }
    return (
        <div className="card">
            <div className="card-header">
                {isEditTitle ? (
                    <div className="d-flex flex-row align-items-center">
                        <input
                            onChange={(evt) => {
                                setNewListTitle(evt.target.value);
                                setError(false);
                            }}
                            className="form-control form-control-sm w-auto flex-grow-1 d-inline me-2"
                            type="text"
                            defaultValue={list.title}
                            required
                        ></input>
                        <button
                            type="button" 
                            onClick={() => {
                                onUpdateListTitle(list, newListTitle);
                                setIsEditTitle(false);
                            }}
                            className="btn text-success"
                        >
                            <i className="fas fa-check"></i>
                        </button>
                        <button type="button" onClick={() => setIsEditTitle(false)} className="btn text-danger">
                            <i className="fas fa-times"></i>
                        </button>
                    </div>
                ) : (
                    <div className="d-flex flex-row justify-content-between align-items-center">
                        <div className="d-flex flex-row align-items-center">
                            <h4 className="mb-0">{list.title}</h4>
                            <button className="btn text-primary" onClick={() => setIsEditTitle(true)}>
                                <i className="fas fa-edit"></i>
                            </button>
                        </div>
                        <button className="btn text-danger" onClick={() => onDeleteList(list)}>
                            <i className="fas fa-trash"></i>
                        </button>
                    </div>
                )}
            </div>
            {list.items.length > 0 && (
                <div className="px-2 pt-3">
                    <ul className="w-100">
                        {list.items.map(item => (
                            <li key={item.id} className="mb-1">
                                <div className="d-flex justify-content-between align-items-baseline">
                                    <div>
                                    <input
                                        id={`todo-${list.id}-item-checkbox-${item.id}`}
                                        type="checkbox"
                                        checked={item.done}
                                        onChange={() => onToggleItemDone(item)}
                                        className="me-2"
                                    ></input>
                                    <label htmlFor={`todo-${list.id}-item-checkbox-${item.id}`} className={item.done ? "me-2 text-decoration-line-through": "me-2"}>{item.title}</label>
                                    </div>
                                    <button className="btn btn-danger rounded-circle btn-sm me-2" onClick={() => onRemoveItem(item)}>
                                        <i className="fas fa-times"></i>
                                    </button>
                                </div>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
            <div className="card-footer d-flex align-items-center px-2">
                <input
                    id={`todo-${list.id}-new-item`}
                    onChange={(evt) => {
                        setNewItemTitle(evt.target.value);
                        setError(false);
                    }}
                    className="form-control form-control-sm w-auto flex-grow-1 d-inline me-2"
                    type="text"
                    placeholder="Add a new todo item..."
                    required
                ></input>
                <button type="button" onClick={submitNewItem} className="btn btn-primary btn-sm d-inline w-auto me-2">Add new item</button>
                {error && <span className="text-danger">Todo item description is required!</span>}
                
            </div>
        </div>
    );
}