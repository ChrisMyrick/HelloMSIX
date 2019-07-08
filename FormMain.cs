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
        }

        private async Task CheckForUpdates()
        {
            PackageUpdateAvailabilityResult update = await _currentPackage.CheckUpdateAvailabilityAsync();
            if (update.Availability == PackageUpdateAvailability.Available || update.Availability == PackageUpdateAvailability.Required)
            {
                DialogResult result = MessageBox.Show("There's a new update available! Your application will be automatically closed in order to install it.", "Update Available", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                { 
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
