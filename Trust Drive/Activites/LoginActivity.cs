using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using Trust_Drive.EventListeners;

namespace Trust_Drive.Activites
{
    [Activity(Label = "@string/app_name", Theme = "@style/TrustTheme", MainLauncher = false)]
    public class LoginActivity : AppCompatActivity
    {

        TextInputLayout emailText;
        TextInputLayout passwordText;
        TextView clickToRegisterText;
        Button loginButton;
        CoordinatorLayout rootView;
        FirebaseAuth mAuth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.login);

            emailText = (TextInputLayout)FindViewById(Resource.Id.emailText);
            passwordText = (TextInputLayout)FindViewById(Resource.Id.passwordText);
            rootView = (CoordinatorLayout)FindViewById(Resource.Id.rootView);
            loginButton = (Button)FindViewById(Resource.Id.loginButton);
            clickToRegisterText = (TextView)FindViewById(Resource.Id.clickToRegisterText);

            clickToRegisterText.Click += clickToRegisterText_Click;
            loginButton.Click += loginButton_Click;

            InitializeFirebase();

        }

        private void clickToRegisterText_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegistrationActivity));
        }

        void InitializeFirebase()
        {
            var app = FirebaseApp.InitializeApp(this);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()

                    .SetApplicationId("trustdrive-b2d25")
                    .SetApiKey("AIzaSyBBA-qebMWayyjKSl1LDI8Kk1TOITYQz8w")
                    .SetDatabaseUrl("https://trustdrive-b2d25.firebaseio.com")
                    .SetStorageBucket("trustdrive-b2d25.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;
            }
            else
            {
                mAuth = FirebaseAuth.Instance;
            }

        }



        private void loginButton_Click(object sender, EventArgs e)
        {
            string email, password;
            email = emailText.EditText.Text;
            password = passwordText.EditText.Text;

            if (!email.Contains("@"))
            {
                Snackbar.Make(rootView, "SVP fait une vrai inscription", Snackbar.LengthShort).Show();
                return;
            }
            else if (password.Length < 8)
            {
                Snackbar.Make(rootView, "SVP fait une vrai inscription", Snackbar.LengthShort).Show();
                return;
            }

            TaskCompletionListener taskCompletionListener = new TaskCompletionListener();
            taskCompletionListener.Success += TaskCompletionListener_Success;
            taskCompletionListener.Failure += TaskCompletionListener_Failure;

            mAuth.SignInWithEmailAndPassword(email, password)
                .AddOnSuccessListener(taskCompletionListener)
                .AddOnFailureListener(taskCompletionListener);

        }

        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(rootView, "Erreur recommence", Snackbar.LengthShort).Show();
        }

        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}