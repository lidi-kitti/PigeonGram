using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using WCF_Library_Server.DB_Context;
using WCF_Library_Server.Model;

namespace WCF_Library_Server
{
    //не дает возможности запустить сразу два сервера
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WCF_Service : IWCF_Service
    {
        List<User> users;
        List<Message> Messages;

        // метод авторизации получает логин и пароль пользователя, после чего хеширует его и сравнивает с хешированным паролем из БД
        // если успех - отгружает имена всех юзеров для общения и все переписки пользователя


        public bool Authorisation(int id, string password, ref IDictionary<string, string> messages, ref IDictionary<string, bool> usersInDB)
        {

            // достаём пользователя из бд если он существует
            User user = users.FirstOrDefault(u => u.Id == id);

            if (user != null && MyHash.Authefication(user.HashedPassword, password))
            {
                using (var db = new DBContext())
                {
                    Messages = new List<Message>();
                    Messages.AddRange(db.Messages.Where(m => m.User_From_Id == id || m.User_To_Id == id));

                    foreach (var item in Messages)
                    {
                        if (item.User_To_Id != user.Id)
                        {
                            messages.Add(item.User_To_Id.ToString(), item.Content); //можно в будущем пофиксить, добавить вместо id имена пользователей
                        }
                        else
                        {
                            messages.Add(item.User_From_Id.ToString(), item.Content); //same 
                        }

                    }
                }

                user.operationContext = OperationContext.Current;

                // передача данных об именах
                //foreach (var item in users)
                //{
                //    //чтобы не добавлять себя
                //    if (id != item.Id)
                //        usersInDB.Add(item.Id.ToString(), );
                //}


                return true;

            }
            return false;
        }

        public bool Registration(string firstName, string lastName, string email, string password)
        {
            try
            {
                using (var usersDB = new DBContext())
                {

                    if (users.FirstOrDefault(u => u.FirstName == firstName) == null)
                    {
                        var user = new User()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email,
                            // хеширование пароля происходит в статическом классе
                            HashedPassword = MyHash.HashedPassword(password)
                        };

                        // добавляем в БД
                        usersDB.Users.Add(user);
                        // в серверный список
                        users.Add(user);

                        usersDB.SaveChanges();

                        // новый пользователь успешно зареган
                        return true;
                    }
                    else
                    {
                        //пользователь с таким именем уже существует
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

        }

        public void SendMessage(int FromUser, int ToUser, string MessageData)
        {
            var msg = new Message()
            {
                User_From_Id = FromUser,
                User_To_Id = ToUser,
                Content = MessageData
            };

            if (users.FirstOrDefault(u => u.Id == ToUser) != null)
            {
                // отправка юзеру

                if (users.FirstOrDefault(u => u.Id == ToUser).operationContext != null)
                {
                    users.FirstOrDefault(u => u.Id == ToUser).operationContext.GetCallbackChannel<IWCF_ServiceChatCallBack>()
                .MessageCallBack($"{DateTime.Now.ToShortTimeString()}| {users.FirstOrDefault(u => u.Id == FromUser).Id}: {MessageData}");
                }

                // отпечатка у отправившего
                users.FirstOrDefault(u => u.Id == FromUser).operationContext.GetCallbackChannel<IWCF_ServiceChatCallBack>()
                    .MessageCallBack($"{DateTime.Now.ToShortTimeString()}| Me: {MessageData}");
            }

        }
    }}

