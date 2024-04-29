using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FirebaseStorageExample
{
    public class FirebaseStorageHelper
    {
        private const string FirebaseStorageBucket = "degree-eaeef.appspot.com";
        private const string FirebaseAuthToken = "AIzaSyCSY4_IcFMcIsOu7048DDlHy0f8xFQoBPk";

        public async Task UploadFileToFirebaseStorage(string filePath)
        {
            try
            {
                var stream = File.OpenRead(filePath);

                var task = Task.Run(async () =>
                {
                    var storage = new FirebaseStorage(FirebaseStorageBucket, new FirebaseStorageOptions { AuthTokenAsyncFactory = () => Task.FromResult(FirebaseAuthToken) });

                    // Загрузка файла
                    await storage
                        .Child("pdfs/" + Path.GetFileName(filePath))
                        .PutAsync(stream,
                            cancellationToken: default
                            );

                    MessageBox.Show("Файл успешно загружен в Firebase Storage!");
                });

                await task;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке файла в Firebase Storage: {ex.Message}");
            }
        }
    }
}