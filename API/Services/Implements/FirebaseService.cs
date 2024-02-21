using API.Services.Interfaces;
using Domain.Exceptions;
using Firebase.Auth;
using Firebase.Storage;

namespace API.Services.Implements
{
    public class FirebaseService : IFirebaseService
    {
        private static string Apikey = "AIzaSyAyZedBeNxjG2BRINYU0rqdsRRsLdYdC9I";
        private static string Bucket = "swp-project-cef68.appspot.com";
        private static string AuthEmail = "auctionweb@gmail.com";
        private static string AuthPassword = "auctionweb";

        public async Task<bool> SignUp(string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(Apikey));
            var userCredentials = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            return userCredentials is not null;
        }

        public async Task<string> UploadFirebaseAsync(MemoryStream stream, string fileName)
        {
            string link = "";
            var auth = new FirebaseAuthProvider(new FirebaseConfig(Apikey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true,
            })
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

            try
            {
                link = await task;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }

            return link;
        }
    }
}
