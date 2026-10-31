using System;
using System.Linq;
using Passero.Framework.Base;
using Wisej.Web;

namespace PasseroDemo.Application
{
    public sealed partial class AuthenticationAdministrationForm : Form
    {
        private readonly IAuthenticationUserStore _userStore;

        public AuthenticationAdministrationForm()
        {
            InitializeComponent();
        }

        public AuthenticationAdministrationForm(IAuthenticationUserStore userStore) : this()
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
            Reload();
        }

        private void Reload()
        {
            if (_userStore == null)
                return;

            _usersGrid.DataSource = _userStore.GetUsers().Select(user => new
            {
                user.UserName,
                Roles = string.Join(";", user.Roles),
                Groups = string.Join(";", user.Groups),
                user.Enabled,
                user.Lockout,
                user.IsBuiltIn
            }).ToList();
            _rolesGrid.DataSource = _userStore.GetRoles().ToList();
            _groupsGrid.DataSource = _userStore.GetGroups().Select(group => new
            {
                group.Id,
                group.Name,
                Roles = string.Join(";", group.Roles),
                group.IsBuiltIn
            }).ToList();
        }

        private void RefreshData(object sender, EventArgs e)
        {
            Reload();
        }

        private void SaveUser(object sender, EventArgs e)
        {
            try
            {
                var roleIds = (_roleIds.Text ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var groupIds = (_userGroups.Text ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var existingUser = _userStore.GetUser(_userName.Text);
                _userStore.SaveUser(new AuthenticationUserInfo
                {
                    UserName = _userName.Text,
                    Roles = roleIds,
                    Groups = existingUser?.IsBuiltIn == true ? existingUser.Groups : groupIds,
                    Enabled = _enabled.Checked,
                    Lockout = _lockout.Checked
                });
                _userStore.SetUserStatus(_userName.Text, _enabled.Checked, _lockout.Checked);
                if (existingUser?.IsBuiltIn != true)
                    _userStore.SetUserGroups(_userName.Text, groupIds);
                Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteUser(object sender, EventArgs e)
        {
            try
            {
                if (_userStore.DeleteUser(_userName.Text))
                    Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetPassword(object sender, EventArgs e)
        {
            try
            {
                _userStore.SetUserPassword(_userName.Text, _password.Text);
                _password.Text = string.Empty;
                Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveRole(object sender, EventArgs e)
        {
            try
            {
                _userStore.SaveRole(new SecurityRoleDefinition { Id = _roleId.Text, Name = _roleName.Text });
                Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteRole(object sender, EventArgs e)
        {
            try
            {
                if (_userStore.DeleteRole(_roleId.Text))
                    Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveGroup(object sender, EventArgs e)
        {
            try
            {
                var roles = (_groupRoles.Text ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                _userStore.SaveGroup(new SecurityGroupDefinition { Id = _groupId.Text, Name = _groupName.Text, Roles = roles.ToList() });
                Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteGroup(object sender, EventArgs e)
        {
            try
            {
                if (_userStore.DeleteGroup(_groupId.Text))
                    Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UsersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var userName = _usersGrid.Rows[e.RowIndex].Cells["UserName"].Value as string;
            var user = _userStore.GetUser(userName);
            if (user == null)
                return;
            _userName.Text = user.UserName;
            _roleIds.Text = string.Join(";", user.Roles);
            _userGroups.Text = string.Join(";", user.Groups);
            _enabled.Checked = user.Enabled;
            _lockout.Checked = user.Lockout;
            _deleteUser.Enabled = !user.IsBuiltIn;
            _roleIds.Enabled = !user.IsBuiltIn;
            _userGroups.Enabled = !user.IsBuiltIn;
            _enabled.Enabled = !string.Equals(user.UserName, AuthenticationBuiltinIds.GuestUser, StringComparison.OrdinalIgnoreCase);
        }

        private void GroupsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            _groupId.Text = _groupsGrid.Rows[e.RowIndex].Cells["Id"].Value as string;
            _groupName.Text = _groupsGrid.Rows[e.RowIndex].Cells["Name"].Value as string;
            _groupRoles.Text = _groupsGrid.Rows[e.RowIndex].Cells["Roles"].Value as string;
            var group = _userStore.GetGroups().FirstOrDefault(item => string.Equals(item.Id, _groupId.Text, StringComparison.OrdinalIgnoreCase));
            _deleteGroup.Enabled = group?.IsBuiltIn != true;
            _groupName.Enabled = group?.IsBuiltIn != true;
            _groupRoles.Enabled = group?.IsBuiltIn != true;
        }
    }
}
