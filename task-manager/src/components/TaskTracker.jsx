import React, { useState, useEffect } from 'react';
import { Trash2, Edit, Check, X, Plus } from 'lucide-react';

const TaskTracker = () => {
  const [tasks, setTasks] = useState([]);
  const [newTask, setNewTask] = useState('');
  const [editingTask, setEditingTask] = useState(null);
  const [editText, setEditText] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    const savedTasks = localStorage.getItem('tasks');
    if (savedTasks) {
      setTasks(JSON.parse(savedTasks));
    }
  }, []);

  useEffect(() => {
    localStorage.setItem('tasks', JSON.stringify(tasks));
  }, [tasks]);

  const addTask = (e) => {
    e.preventDefault();
    if (!newTask.trim()) {
      setError('Task cannot be empty');
      return;
    }
    setTasks([...tasks, { 
      id: Date.now(), 
      text: newTask.trim(), 
      completed: false 
    }]);
    setNewTask('');
    setError('');
  };

  const deleteTask = (taskId) => {
    setTasks(tasks.filter(task => task.id !== taskId));
  };

  const toggleComplete = (taskId) => {
    setTasks(tasks.map(task =>
      task.id === taskId ? { ...task, completed: !task.completed } : task
    ));
  };

  const startEditing = (task) => {
    setEditingTask(task.id);
    setEditText(task.text);
  };

  const saveEdit = () => {
    if (!editText.trim()) {
      setError('Task cannot be empty');
      return;
    }
    setTasks(tasks.map(task =>
      task.id === editingTask ? { ...task, text: editText.trim() } : task
    ));
    setEditingTask(null);
    setError('');
  };

  const cancelEdit = () => {
    setEditingTask(null);
    setError('');
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1 style={{ marginBottom: '20px' }}>Task Tracker</h1>
      
      <form onSubmit={addTask} style={{ marginBottom: '20px' }}>
        <div style={{ display: 'flex', gap: '10px' }}>
          <input
            type="text"
            value={newTask}
            onChange={(e) => setNewTask(e.target.value)}
            placeholder="Add a new task..."
            style={{ 
              flex: 1, 
              padding: '8px', 
              border: '1px solid #ccc', 
              borderRadius: '4px' 
            }}
          />
          <button 
            type="submit"
            style={{ 
              padding: '8px', 
              backgroundColor: '#3b82f6', 
              color: 'white', 
              border: 'none', 
              borderRadius: '4px',
              cursor: 'pointer'
            }}
          >
            <Plus size={24} />
          </button>
        </div>
      </form>

      {error && <p style={{ color: 'red', marginBottom: '20px' }}>{error}</p>}

      <ul style={{ listStyle: 'none', padding: 0 }}>
        {tasks.map(task => (
          <li 
            key={task.id}
            style={{
              display: 'flex',
              alignItems: 'center',
              gap: '10px',
              padding: '10px',
              border: '1px solid #ccc',
              borderRadius: '4px',
              marginBottom: '10px'
            }}
          >
            {editingTask === task.id ? (
              <>
                <input
                  type="text"
                  value={editText}
                  onChange={(e) => setEditText(e.target.value)}
                  style={{ 
                    flex: 1, 
                    padding: '8px', 
                    border: '1px solid #ccc', 
                    borderRadius: '4px' 
                  }}
                />
                <button
                  onClick={saveEdit}
                  style={{ color: 'green', cursor: 'pointer', background: 'none', border: 'none' }}
                >
                  <Check size={20} />
                </button>
                <button
                  onClick={cancelEdit}
                  style={{ color: 'red', cursor: 'pointer', background: 'none', border: 'none' }}
                >
                  <X size={20} />
                </button>
              </>
            ) : (
              <>
                <input
                  type="checkbox"
                  checked={task.completed}
                  onChange={() => toggleComplete(task.id)}
                />
                <span style={{ 
                  flex: 1,
                  textDecoration: task.completed ? 'line-through' : 'none',
                  color: task.completed ? '#666' : '#fff'
                }}>
                  {task.text}
                </span>
                <button
                  onClick={() => startEditing(task)}
                  style={{ color: '#3b82f6', cursor: 'pointer', background: 'none', border: 'none' }}
                >
                  <Edit size={20} />
                </button>
                <button
                  onClick={() => deleteTask(task.id)}
                  style={{ color: 'red', cursor: 'pointer', background: 'none', border: 'none' }}
                >
                  <Trash2 size={20} />
                </button>
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TaskTracker;