using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TODOList.DAO;
using TODOList.Model;
using TODOList.Observer;

namespace TODOList.Controller;

public class Controller
{
    public DAO<Notification> daoNotification;
    public DAO<Task> daoTask;
    public DAO<User> daoUser;
   

    public Subject publisher;

    public Controller()
    {
        publisher = new Subject();
        daoNotification = new DAO<Notification>();
        daoTask = new DAO<Task>();
        daoUser = new DAO<User>();
      
    }

    

    public List<Task> GetAllStudents()
    {
        return daoTask.GetAllObjects();
    }

    public List<Notification> GetAllProfessors()
    {
        return daoNotification.GetAllObjects();
    }

    public List<User> GetAllSubjects()
    {
        return daoUser.GetAllObjects();
    }

   

    public void AddTask(Task task)
    {
        daoTask.AddObject(task);
        publisher.NotifyObservers();
    }

   
    public void AddUser(User user)
    {
        daoUser.AddObject(user);
        publisher.NotifyObservers();
    }

    public void UpdateTask(Task task)
    {
        daoTask.UpdateObject(task);
        publisher.NotifyObservers();
    }

    public void AddNotification(Notification notification)
    {
        daoNotification.AddObject(notification);
        publisher.NotifyObservers();
    }

    public void UpdateUser(User user)
    {
        daoUser.UpdateObject(user);
        publisher.NotifyObservers();
    }

    public void UpdateNotification(Notification notification)
    {
        daoNotification.UpdateObject(notification);
        publisher.NotifyObservers();
    }

    public void DeleteTask(int taskId)
    {
       daoTask.RemoveObject(taskId);
       publisher.NotifyObservers();
    }

    public void DeleteNotification(int notificationId)
    {
       daoNotification.RemoveObject(notificationId);
        publisher.NotifyObservers();
    }

    public void DeleteUser(int userId)
    {
        daoUser.RemoveObject(userId);
        publisher.NotifyObservers();
    }

       

    public void SaveAllToStorage()
    {
        daoNotification.SaveToStorage();
        daoTask.SaveToStorage();
        daoUser.SaveToStorage();
                
    }
}
