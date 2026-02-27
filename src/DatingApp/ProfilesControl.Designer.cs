using System.Drawing;
using System.Windows.Forms;

namespace DatingApp
{
    partial class ProfilesControl
    {
        private FlowLayoutPanel flowLayoutProfiles;

        private void InitializeComponent()
        {
            flowLayoutProfiles = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // flowLayoutProfiles
            // 
            flowLayoutProfiles.AutoScroll = true;
            flowLayoutProfiles.Dock = DockStyle.Fill;
            flowLayoutProfiles.Location = new Point(0, 0);
            flowLayoutProfiles.Name = "flowLayoutProfiles";
            flowLayoutProfiles.Padding = new Padding(20);
            flowLayoutProfiles.Size = new Size(800, 450);
            flowLayoutProfiles.TabIndex = 0;
            flowLayoutProfiles.Paint += flowLayoutProfiles_Paint;
            // 
            // ProfilesControl
            // 
            Controls.Add(flowLayoutProfiles);
            Name = "ProfilesControl";
            Size = new Size(800, 450);
            ResumeLayout(false);
        }
    }
}
