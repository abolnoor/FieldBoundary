using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace FieldBoundary
{
    public partial class App : Application
    {
        static BoundaryDatabase database;

        private static BoundaryDetectionsManager BoundaryMgr;

        internal static BoundaryDetectionsManager BoundaryManager { get => BoundaryMgr; set => BoundaryMgr = value; }

        public App()
        {
            
            InitializeComponent();
            BoundaryManager = new BoundaryDetectionsManager(new RestService());
            MainPage = new MainPage();
        }

       // private static BoundaryDatabase DB;
        internal static BoundaryDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new BoundaryDatabase();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
