using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace MSIX_Practice
{
    public partial class FormMain : Form
    {
        readonly PackageManager _packageManager = new PackageManager();
        readonly Package _currentPackage;

        public FormMain()
        {
            InitializeComponent();

            _currentPackage = _packageManager.FindPackageForUser(string.Empty, Package.Current.Id.FullName);
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Description: {_currentPackage.Description}, " +
                $"\nDisplayName: {_currentPackage.DisplayName}, " +
                $"\nPublisher: {_currentPackage.PublisherDisplayName}, " +
                $"\nStatus: {_currentPackage.Status}");
        }

        private async void BtnCheckForUpdates_Click(object sender, EventArgs e)
        {
            await CheckForUpdates();

            // Get the current app's package for the current user.
           // try
           //{            
           //     PackageUpdateAvailabilityResult result = await _currentPackage.CheckUpdateAvailabilityAsync();
           //     switch (result.Availability)
           //     {
           //         case PackageUpdateAvailability.Available:
           //             MessageBox.Show("An update is available.");
           //             break;
           //         case PackageUpdateAvailability.Required:
           //             MessageBox.Show("A required update is ready for install.");
           //             break;
           //         case PackageUpdateAvailability.NoUpdates:
           //             MessageBox.Show("There are no available updates.");
           //             break;
           //         case PackageUpdateAvailability.Unknown:
           //         default:
           //             MessageBox.Show("The status is unknown.");
           //             break;
           //     }
           // }
           // catch(Exception ex)
           // {
           //     MessageBox.Show(ex.Message);
           // }

           // MessageBox.Show("CheckUpdateAvailabilityAsync() Completed");
        }

        private async Task CheckForUpdates()
        {
            PackageUpdateAvailabilityResult update = await _currentPackage.CheckUpdateAvailabilityAsync();
            if (update.Availability == PackageUpdateAvailability.Available || update.Availability == PackageUpdateAvailability.Required)
            {
                DialogResult result = MessageBox.Show("There's a new update available! Your application will be automatically closed in order to install it.", "Update Available", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                { 
                    MessageBox.Show("There is an update available.");
                    Application.Exit();
                    Application.ExitThread();
                }
            }
            else if (update.Availability == PackageUpdateAvailability.NoUpdates)
            {
                MessageBox.Show("No updates are available.");
            }
            else if (update.Availability == PackageUpdateAvailability.Error)
            {
                MessageBox.Show("Update checked reported an error.");
            }
            else if (update.Availability == PackageUpdateAvailability.Unknown)
            {
                MessageBox.Show("Unknown Update state.");
            }
            else
            {
                MessageBox.Show("The CheckUpdateAvailabilityAsync() method did not return an expected value.");
            }
        }
    }
}
