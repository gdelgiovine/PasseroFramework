namespace PasseroDemo.Application
{
    partial class AuthenticationAdministrationForm
    {
        private System.ComponentModel.IContainer components = null;
        private Wisej.Web.DataGridView _usersGrid;
        private Wisej.Web.DataGridView _rolesGrid;
        private Wisej.Web.DataGridView _groupsGrid;
        private Wisej.Web.TextBox _userName;
        private Wisej.Web.TextBox _password;
        private Wisej.Web.TextBox _roleIds;
        private Wisej.Web.TextBox _userGroups;
        private Wisej.Web.TextBox _roleId;
        private Wisej.Web.TextBox _roleName;
        private Wisej.Web.TextBox _groupId;
        private Wisej.Web.TextBox _groupName;
        private Wisej.Web.TextBox _groupRoles;
        private Wisej.Web.CheckBox _enabled;
        private Wisej.Web.CheckBox _lockout;
        private Wisej.Web.Button _saveUser;
        private Wisej.Web.Button _deleteUser;
        private Wisej.Web.Button _setPassword;
        private Wisej.Web.Button _saveRole;
        private Wisej.Web.Button _deleteRole;
        private Wisej.Web.Button _refresh;
        private Wisej.Web.Button _saveGroup;
        private Wisej.Web.Button _deleteGroup;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _usersGrid = new Wisej.Web.DataGridView();
            _rolesGrid = new Wisej.Web.DataGridView();
            _groupsGrid = new Wisej.Web.DataGridView();
            _userName = new Wisej.Web.TextBox();
            _password = new Wisej.Web.TextBox();
            _roleIds = new Wisej.Web.TextBox();
            _userGroups = new Wisej.Web.TextBox();
            _roleId = new Wisej.Web.TextBox();
            _roleName = new Wisej.Web.TextBox();
            _groupId = new Wisej.Web.TextBox();
            _groupName = new Wisej.Web.TextBox();
            _groupRoles = new Wisej.Web.TextBox();
            _enabled = new Wisej.Web.CheckBox();
            _lockout = new Wisej.Web.CheckBox();
            _saveUser = new Wisej.Web.Button();
            _deleteUser = new Wisej.Web.Button();
            _setPassword = new Wisej.Web.Button();
            _saveRole = new Wisej.Web.Button();
            _deleteRole = new Wisej.Web.Button();
            _refresh = new Wisej.Web.Button();
            _saveGroup = new Wisej.Web.Button();
            _deleteGroup = new Wisej.Web.Button();
            SuspendLayout();
            // 
            // _usersGrid
            // 
            _usersGrid.Location = new System.Drawing.Point(16, 16);
            _usersGrid.Name = "_usersGrid";
            _usersGrid.ReadOnly = true;
            _usersGrid.Size = new System.Drawing.Size(280, 220);
            _usersGrid.TabIndex = 0;
            _usersGrid.CellClick += UsersGrid_CellClick;
            // 
            // _rolesGrid
            // 
            _rolesGrid.Location = new System.Drawing.Point(310, 16);
            _rolesGrid.Name = "_rolesGrid";
            _rolesGrid.ReadOnly = true;
            _rolesGrid.Size = new System.Drawing.Size(280, 220);
            _rolesGrid.TabIndex = 1;
            // 
            // _groupsGrid
            // 
            _groupsGrid.Location = new System.Drawing.Point(604, 16);
            _groupsGrid.Name = "_groupsGrid";
            _groupsGrid.ReadOnly = true;
            _groupsGrid.Size = new System.Drawing.Size(280, 220);
            _groupsGrid.TabIndex = 2;
            _groupsGrid.CellClick += GroupsGrid_CellClick;
            // 
            // _userName
            // 
            _userName.LabelText = "Username";
            _userName.Location = new System.Drawing.Point(16, 250);
            _userName.Name = "_userName";
            _userName.Size = new System.Drawing.Size(220, 46);
            _userName.TabIndex = 2;
            // 
            // _password
            // 
            _password.LabelText = "Nuova password";
            _password.Location = new System.Drawing.Point(250, 250);
            _password.Name = "_password";
            _password.PasswordChar = '*';
            _password.Size = new System.Drawing.Size(220, 46);
            _password.TabIndex = 3;
            // 
            // _roleIds
            // 
            _roleIds.LabelText = "Role IDs, separati da punto e virgola";
            _roleIds.Location = new System.Drawing.Point(484, 250);
            _roleIds.Name = "_roleIds";
            _roleIds.Size = new System.Drawing.Size(396, 46);
            _roleIds.TabIndex = 4;
            // 
            // _userGroups
            // 
            _userGroups.LabelText = "Groups, separati da punto e virgola";
            _userGroups.Location = new System.Drawing.Point(16, 310);
            _userGroups.Name = "_userGroups";
            _userGroups.Size = new System.Drawing.Size(420, 46);
            _userGroups.TabIndex = 5;
            // 
            // _enabled
            // 
            _enabled.AutoSize = false;
            _enabled.Checked = true;
            _enabled.Location = new System.Drawing.Point(460, 318);
            _enabled.Name = "_enabled";
            _enabled.Size = new System.Drawing.Size(100, 32);
            _enabled.TabIndex = 6;
            _enabled.Text = "Enabled";
            // 
            // _lockout
            // 
            _lockout.AutoSize = false;
            _lockout.Location = new System.Drawing.Point(570, 318);
            _lockout.Name = "_lockout";
            _lockout.Size = new System.Drawing.Size(100, 32);
            _lockout.TabIndex = 7;
            _lockout.Text = "Lockout";
            // 
            // _roleId
            // 
            _roleId.LabelText = "Role ID (role:...)";
            _roleId.Location = new System.Drawing.Point(16, 430);
            _roleId.Name = "_roleId";
            _roleId.Size = new System.Drawing.Size(220, 46);
            _roleId.TabIndex = 5;
            // 
            // _roleName
            // 
            _roleName.LabelText = "Nome ruolo";
            _roleName.Location = new System.Drawing.Point(250, 430);
            _roleName.Name = "_roleName";
            _roleName.Size = new System.Drawing.Size(220, 46);
            _roleName.TabIndex = 6;
            // 
            // _saveUser
            // 
            _saveUser.Location = new System.Drawing.Point(484, 370);
            _saveUser.Name = "_saveUser";
            _saveUser.Size = new System.Drawing.Size(120, 32);
            _saveUser.TabIndex = 7;
            _saveUser.Text = "Salva utente";
            _saveUser.Click += SaveUser;
            // 
            // _deleteUser
            // 
            _deleteUser.Location = new System.Drawing.Point(614, 370);
            _deleteUser.Name = "_deleteUser";
            _deleteUser.Size = new System.Drawing.Size(120, 32);
            _deleteUser.TabIndex = 8;
            _deleteUser.Text = "Elimina utente";
            _deleteUser.Click += DeleteUser;
            // 
            // _setPassword
            // 
            _setPassword.Location = new System.Drawing.Point(744, 370);
            _setPassword.Name = "_setPassword";
            _setPassword.Size = new System.Drawing.Size(136, 32);
            _setPassword.TabIndex = 9;
            _setPassword.Text = "Imposta password";
            _setPassword.Click += SetPassword;
            // 
            // _saveRole
            // 
            _saveRole.Location = new System.Drawing.Point(484, 490);
            _saveRole.Name = "_saveRole";
            _saveRole.Size = new System.Drawing.Size(120, 32);
            _saveRole.TabIndex = 10;
            _saveRole.Text = "Salva ruolo";
            _saveRole.Click += SaveRole;
            // 
            // _deleteRole
            // 
            _deleteRole.Location = new System.Drawing.Point(614, 490);
            _deleteRole.Name = "_deleteRole";
            _deleteRole.Size = new System.Drawing.Size(120, 32);
            _deleteRole.TabIndex = 11;
            _deleteRole.Text = "Elimina ruolo";
            _deleteRole.Click += DeleteRole;
            // 
            // _refresh
            // 
            _refresh.Location = new System.Drawing.Point(744, 490);
            _refresh.Name = "_refresh";
            _refresh.Size = new System.Drawing.Size(136, 32);
            _refresh.TabIndex = 12;
            _refresh.Text = "Aggiorna";
            _refresh.Click += RefreshData;
            // 
            // _groupId
            // 
            _groupId.LabelText = "Group ID (group:...)";
            _groupId.Location = new System.Drawing.Point(16, 550);
            _groupId.Name = "_groupId";
            _groupId.Size = new System.Drawing.Size(220, 46);
            _groupId.TabIndex = 13;
            // 
            // _groupName
            // 
            _groupName.LabelText = "Nome gruppo";
            _groupName.Location = new System.Drawing.Point(250, 550);
            _groupName.Name = "_groupName";
            _groupName.Size = new System.Drawing.Size(220, 46);
            _groupName.TabIndex = 14;
            // 
            // _groupRoles
            // 
            _groupRoles.LabelText = "Role IDs, separati da punto e virgola";
            _groupRoles.Location = new System.Drawing.Point(484, 550);
            _groupRoles.Name = "_groupRoles";
            _groupRoles.Size = new System.Drawing.Size(396, 46);
            _groupRoles.TabIndex = 15;
            // 
            // _saveGroup
            // 
            _saveGroup.Location = new System.Drawing.Point(484, 610);
            _saveGroup.Name = "_saveGroup";
            _saveGroup.Size = new System.Drawing.Size(120, 32);
            _saveGroup.TabIndex = 16;
            _saveGroup.Text = "Salva gruppo";
            _saveGroup.Click += SaveGroup;
            // 
            // _deleteGroup
            // 
            _deleteGroup.Location = new System.Drawing.Point(614, 610);
            _deleteGroup.Name = "_deleteGroup";
            _deleteGroup.Size = new System.Drawing.Size(120, 32);
            _deleteGroup.TabIndex = 17;
            _deleteGroup.Text = "Elimina gruppo";
            _deleteGroup.Click += DeleteGroup;
            // 
            // AuthenticationAdministrationForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 670);
            Controls.AddRange(new Wisej.Web.Control[] { _usersGrid, _rolesGrid, _groupsGrid, _userName, _password, _roleIds, _userGroups, _enabled, _lockout, _roleId, _roleName, _groupId, _groupName, _groupRoles, _saveUser, _deleteUser, _setPassword, _saveRole, _deleteRole, _refresh, _saveGroup, _deleteGroup });
            Name = "AuthenticationAdministrationForm";
            Text = "Gestione autenticazione";
            ResumeLayout(false);
        }
    }
}
